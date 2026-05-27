# SSO B2C — Decisões de Implementação no CRM

> Documento que registra as decisões arquiteturais tomadas ao traduzir
> [`sso-implementacao-crm.md`](sso-implementacao-crm.md) para a base de
> código real do CRM (TopConWeb — .NET Framework 4.6.2 + Angular 8).
>
> O guia original assume um cenário "limpo" com cookies HttpOnly emitidos
> pelo backend. O CRM real **não usa cookies** — autenticação é via OAuth
> bearer tokens armazenados no localStorage. As decisões abaixo reconciliam
> os dois mundos.

---

## D1. Sessão local: bearer token, não cookie

**Spec:** "emite cookie de sessão local (HttpOnly, Secure, SameSite=Lax)".

**CRM hoje:** OAuth bearer token retornado por `POST /api/security/token`
(`Microsoft.Owin.Security.OAuth`), guardado no `localStorage` (chave
`t.tcw.token`). Todo `Authorization: Bearer …` é interceptado pelo OWIN.

**Decisão:** **manter bearer token**. Implementar o SSO B2C como um novo
`grant_type = b2c` dentro de `AuthAuthorizationServerProvider.GrantCustomExtension`,
exatamente no mesmo padrão do `grant_type = azure` já existente.

**Por quê:**
- Trocar para cookie obriga reescrever todos os controllers + filtros +
  middleware + interceptor Angular.
- O bearer token entrega a mesma propriedade essencial: sessão local
  desacoplada do IdP, com lifetime curto e revogável.
- O frontend já sabe lidar com o fluxo OAuth (linha de código equivalente
  ao `loginWithSSo` já existente).

**Implicação prática:** o endpoint POST do spec (`/auth/sso/complete`)
vira **`POST /api/security/token`** com `grant_type=b2c&assertion=<id_token>`.
Mantemos também um endpoint nomeado em `SsoController` (`POST /api/v1/sso/complete`)
que aceita os mesmos parâmetros e delega para a mesma máquina OAuth via
HTTP interno — útil para a página de callback que segue a nomenclatura do spec.

---

## D2. Configuração: dados do tenant + feature gate no banco

**Spec:** appSettings em `Web.config`.

**CRM hoje:** existe uma tabela `parametros_sso` (entidade `ParametrosSSO`)
indexada por `tipo_provedor`, com uma linha por provedor SSO ativo.
O Azure AD legado já vive lá (`tipo_provedor = 0` / Microsoft) e usa
as colunas `dominio` (não atualmente lida), `tenant_id` e `client_id`
pra montar o issuer (`https://login.microsoftonline.com/{tenant_id}/v2.0`)
e validar audience.

**Decisão:** espelhar o padrão do Azure AD legado pro B2C — uma única
linha em `topsys.parametros_sso` com `tipo_provedor = 1` (B2C) carrega
**tudo que é por-deploy**:

| Coluna             | Conteúdo B2C                                    |
|--------------------|--------------------------------------------------|
| `sso_habilitado`   | Feature gate (false até cutover)                 |
| `dominio`          | Subdomínio B2C (`topconidentity.b2clogin.com`)   |
| `tenant_id`        | B2C directory GUID                               |
| `client_id`        | App Reg / audience                               |

O `B2CTokenValidator` monta `https://{dominio}/{tenant_id}/v2.0/` como
issuer e usa `client_id` como audience — exatamente o shape do Azure
AD legado, só apontado pro b2clogin.com em vez de
login.microsoftonline.com.

**Chaves que ficam em `Web.config`** (são por-ambiente OU listas, não
casam com colunas simples):
- `Sso:MetadataUrl` — URL completa do JWKS, embute o nome do user flow
- `Sso:AcceptedTfp` — lista de user flows aceitos (varia por env)
- `Sso:IdentityUrl` — `topconsuite.io` / `hml.topconsuite.io` / etc
- `Sso:ModuleName` — `CRM`

**Por que essa separação:**
- Tenant config (`dominio`, `tenant_id`, `client_id`) é constante por
  deploy → encaixa em uma linha do banco, fácil de auditar e mudar sem
  restart do pool.
- Os Sso:* restantes ou variam por env (transforms) ou são listas que
  não cabem em uma coluna simples.

**Implementação:**
- `ETipoProvedor.B2C = 1` na entity.
- `ISsoApplicationService.ObterParametroAtivoB2C()` espelha o método
  Azure AD: retorna a linha B2C ativa, ou `null` se ausente/desativada.
- `B2CTokenValidator.Validate(jwt, ssoConfig)` recebe a linha e usa
  `dominio`/`tenant_id`/`client_id` pra construir as `TokenValidationParameters`.
- `AuthAuthorizationServerProvider.HandleB2CGrant` busca a linha uma
  única vez no início, falha rápido se `null` (SSO desabilitado), e
  passa pro validator.
- Migration `0140 - SSO B2C Parametro.sql` insere a linha default com
  `sso_habilitado = false`, `dominio`/`tenant_id`/`client_id` populados
  com os valores do Topcon Identity de PRD (idempotente: só insere se
  nenhuma linha B2C existir).

---

## D3. JIT provisioning: indexar por **email**, não `sub`

**Spec:** indexar usuário local por `external_id = sub` do JWT; cair pra
match por email só como fallback de merge.

**CRM hoje:** entidade `Usuario` tem `Id` (string), `Nome`, `Email`,
`Senha` — **não tem coluna `external_id`**.

**Decisão para Phase 1:** matching por **email** apenas. Quando o
`extension_Modules` permitir entrar:
1. Procura `Usuario` por `Email`.
2. Se achou: chama `Autenticar(Id, Senha)` para emitir o ticket OAuth.
3. Se não achou: cria via `Registrar(email)` (gera ID derivado do email
   e senha randômica), depois autentica.

Esse é exatamente o caminho que `GrantCustomExtension` já usa para o
`grant_type=azure`. Mantemos consistência.

**Por quê não adicionar `external_id` agora:**
- Exige migração de banco (DbUp).
- Adiciona risco a um patch que já mexe em auth.
- Email funciona porque o claim `email` ou `preferred_username` é
  estável no B2C para usuários corporativos.

**Para Phase 2 (recomendado):** adicionar coluna `external_id` em
`tb_usuario`, fazer migração de match `email → sub` no primeiro SSO de
cada usuário (lock no `external_id`), e mudar a busca para indexar por
`sub` primeiro. Documentar essa dívida.

---

## D4. Endpoint do controller

**Spec:** `POST /auth/sso/complete` (MVC-style).

**CRM hoje:** padrão `api/v1/{controller}` Web API + OAuth bearer via
`POST /api/security/token`.

**Decisão:** **caminho único e canônico**: `POST /api/security/token`
com `grant_type=b2c&assertion=<id_token>`. O `SsoController` continua
expondo só o `GET /api/v1/sso/parametros/azure-ad` (legado MSAL).

Não criamos `POST /api/v1/sso/complete` separado porque seria um proxy
para o token endpoint já existente — duplicação sem ganho. O nome
"complete" do spec → o `grant_type=b2c` do OAuth.

**Endpoint `GET /api/v1/sso/parametros/b2c` removido:** a versão
inicial expôs as configurações B2C pra o frontend, mas o
`SsoCallbackComponent` acabou postando direto no token endpoint e
deixando o backend rejeitar com `invalid_grant` se o SSO estiver
desligado — nunca chegou a consumir o endpoint. Diagnóstico/health-check
não foi implementado. Código morto, removido.

---

## D5. Frontend: rota top-level `auth/sso`

**Spec:** `/auth/sso` é uma rota da SPA.

**CRM hoje:** rotas top-level vivem no `app.module.ts` (`appRoutes`).
Login está em `pages/auth/login`.

**Decisão:** registrar `auth/sso` como rota top-level no `appRoutes` do
`app.module.ts`, lazy-friendly (componente standalone), seguindo o
padrão do `pages/auth/login`.

---

## D6. Coexistência com fluxo MSAL/Azure AD legado

**CRM hoje:** existe um `loginSso()` na tela de login que usa MSAL e
`grant_type=azure`. Continua funcionando.

**Decisão:** o fluxo B2C **adiciona** uma porta de entrada; não
substitui nada. Enquanto `Sso:Enabled=false`, o `grant_type=b2c` é
recusado com `invalid_grant`. Quando `Sso:Enabled=true`:
- O launcher do `topconsuite.io` consegue logar via `/auth/sso`.
- A tela de login antiga + MSAL/Azure-AD legado continuam funcionando.

A remoção do caminho antigo é fora deste escopo (item 7 do spec).

---

## D7. Open-redirect guard no `return_to`

**Spec:** validar `return_to` é relativo OU same-host.

**Decisão:** implementado **no frontend** (SsoCallbackComponent), porque
todo o roteamento depois do POST é client-side (SPA). O backend retorna
JSON; não há `302 Location` para sanitizar server-side. A validação:
- string vazia → `/home`
- relativa (`/foo/bar`) → aceita
- absoluta same-origin → aceita o `pathname + search`
- qualquer outra coisa → `/home`

---

## D8. `extension_Modules` gate

**Spec:** se `CRM` não está em `extension_Modules`, devolver **403**
com mensagem "You do not have access to CRM. Please contact your Topcon admin."

**Decisão:** implementado no backend dentro do grant `b2c`. Erro
retornado como `invalid_grant` com a mensagem do spec (em pt-BR, alinhado
com o resto do CRM). O HTTP code do OAuth endpoint nesse caso é 400 com
`error_description` carregando a mensagem — esse é o comportamento
padrão do `OAuthAuthorizationServerProvider.SetError`. O controller
proxy `POST /api/v1/sso/complete` traduz isso de volta para 403 com o
texto correto.

---

## D9. Lifetime do token local

**Spec:** ≤ 1h.

**CRM hoje:** `AccessTokenExpireTimeSpan = TimeSpan.FromHours(10)`
configurado globalmente.

**Decisão:** **não alterar** o lifetime global agora — quebraria todos
os usuários atuais que esperam 10h. Em vez disso, o ticket emitido via
SSO B2C continua com 10h. Aceitável porque:
- O bearer token é stateless mas serve com o mesmo grau de risco que o
  cookie de 1h proposto (ambos guardados client-side).
- Renovação via "novo SSO redirect" continua funcionando: o usuário
  expira → cai pro fluxo de relogin.

**Para Phase 2:** considerar baixar para 1h e implementar refresh
token, alinhando ao spec.

---

## D10. Audience / `aud` opcional

**Spec:** valida `aud == clientId comum` quando configurado.

**Decisão:** seguir o spec. Se `Sso:Audience` estiver setado no
`Web.config`, valida; vazio, não valida. Defesa-em-profundidade barata.

---

## D11. Cache do JWKS

**Spec:** `ConfigurationManager<OpenIdConnectConfiguration>` faz cache
automático.

**Decisão:** uma instância **estática** do `ConfigurationManager` no
`B2CTokenValidator`. O retrieval é caro (HTTP); cache implícito da lib
é suficiente. Mesmo padrão que `GetAzureSigningKeys` já usa.

---

## D12. Filtro de redirect 401 UI → Identity

**Spec (4.5):** quando a UI bate em 401, redirecionar pro Identity em
vez de retornar 401 puro.

**Decisão:** **não implementado nesta fase**. O CRM é uma SPA Angular —
o redirecionamento de "sem sessão → tela de login" é feito pelo
`AuthGuardService` no frontend. O `AuthGuardService` será atualizado em
fase posterior para enxergar `Sso:Enabled` e redirecionar para
`{IdentityUrl}/launch?product=CRM&return=…` em vez da tela antiga.

Por agora, o `auth-guard.service.ts` mantém o comportamento atual
(redireciona para `pages/auth/login`). Quando o Identity estiver
roteando 100% do tráfego, troca-se aqui. Documentar dívida.

---

## D13. Logging

**Decisão:** falhas de validação de token logam via
`System.Diagnostics.Trace.TraceWarning` (já é o sink do Application
Insights do projeto). Mensagens NÃO incluem o token nem trechos dele.

---

## D14. Login como ADMIN via claim `extension_Admin`

**Contexto:** o Identity emite tokens com um claim booleano
`extension_Admin = true` para usuários administradores do Topcon Suite.
Esses usuários precisam entrar no CRM como o usuário local "ADMIN"
(linha com `Id = 'ADMIN'` em `topsys.usr_usuario` — convenção legada
amplamente usada no domínio, ver por exemplo
`ObraService.cs` e `ComercialLegacyService.cs`).

**Decisão:** no `HandleB2CGrant`, depois da validação do token e antes
do gate de módulo, ler `extension_Admin`. Quando `true`:
- **Pula** o gate `CRM ∈ extension_Modules` — admin tem acesso elevado
  por definição, não precisa estar no rol de módulos por cliente.
- **Pula** o JIT-by-email — em vez de procurar/registrar usuário pelo
  email do token, carrega direto o `Usuario` com `Id = "ADMIN"` via
  `ObterPorId("ADMIN")`.
- Se a linha ADMIN não existir no banco daquele deploy, devolve
  `invalid_grant` com mensagem "Usuário ADMIN não encontrado." (não cria
  on-the-fly — admin é provisionado fora do SSO).

Quando `extension_Admin` é falso/ausente, o fluxo segue o caminho
normal (gate de CRM + JIT por email).

**Por quê pular o gate:** um admin Topcon que acessa o CRM de um cliente
não precisa estar listado nos módulos daquele cliente — ele entra
operacionalmente, não como usuário do tenant.

**Por quê não criar a linha ADMIN automaticamente:** é um usuário com
poderes especiais; criação/manutenção fica fora do fluxo de SSO.

---

## Resumo de arquivos tocados

### Back-end
- `Backend/TopSys.TopConWeb.API/Web.config` — chaves `Sso:*` de ambiente (sem `Sso:Enabled`)
- `Backend/TopSys.TopConWeb.API/Security/B2CTokenValidator.cs` — **novo**
- `Backend/TopSys.TopConWeb.API/Security/AuthAuthorizationServerProvider.cs` — adiciona grant `b2c`, gate lido de `parametros_sso`
- `Backend/TopSys.TopConWeb.API/Controllers/SsoController.cs` — mantém só `azure-ad`
- `Backend/TopSys.TopConWeb.API/TopSys.TopConWeb.API.csproj` — inclui `B2CTokenValidator.cs`
- `Backend/TopSys.TopConWeb.Domain/Entities/ParametrosSSO.cs` — `ETipoProvedor.B2C = 1`
- `Backend/TopSys.TopConWeb.Application/Interfaces/ISsoApplicationService.cs` — `ObterParametroAtivoB2C()`
- `Backend/TopSys.TopConWeb.Application/SsoApplicationService.cs` — implementação
- `Backend/TopSys.TopConWeb.Infra.Data/Migrations/Scripts/0140 - SSO B2C Parametro.sql` — **novo** (seed)

### Front-end
- `Frontend/src/app/main/pages/authentication/sso-callback/sso-callback.component.ts` — **novo**
- `Frontend/src/app/main/pages/authentication/sso-callback/sso-callback.module.ts` — **novo**
- `Frontend/src/app/services/user.service.ts` — método `loginWithB2C`
- `Frontend/src/app/services/sso.service.ts` — mantém só `listarConfiguracaoAzureAd`
- `Frontend/src/app/main/pages/pages.module.ts` — registra módulo

---

## Pendências (Phase 2)

1. Coluna `external_id` em `tb_usuario` + migração de matching.
2. Lifetime de token reduzido (1h) + refresh.
3. `AuthGuardService` redireciona para Identity quando `Sso:Enabled`.
4. Single logout / backchannel logout.
5. Mapear `extension_TenantId` (GUID Topcon) para tenant interno do CRM
   se houver multi-tenant — hoje o CRM já é mono-tenant por deploy
   (`TenantName` em `Web.config`), então não há trabalho aqui.

# SSO — Guia de Implementação no CRM

> Guia operacional pra o time do CRM ligar o SSO com o Topcon Identity.
> Fonte de verdade da arquitetura: [`sso-implementacao-tecnica.md`](sso-implementacao-tecnica.md).
> Este documento traduz aquela especificação pra a stack do CRM
> (.NET Framework + Angular) com exemplos concretos.

---

## 1. Objetivo

Permitir que Maria, depois de logar no `topconsuite.io`, clique no card do
CRM no launcher e caia no CRM **já autenticada**, sem digitar senha. Bookmarks
antigos (`<cliente>-remote.topconsuite.app:20200`) continuam funcionando:
ao abrir sem sessão, o CRM redireciona pro Identity, valida o token
emitido pelo B2C e cria sessão local.

O CRM **não chama o Identity** durante esse fluxo. Recebe o token, valida
contra o JWKS do B2C, lê os claims, faz JIT provisioning e emite cookie
de sessão. Toda informação que o CRM precisa está no token.

---

## 2. Pré-requisitos

### 2.1. Configuração (em `Web.config` ou `appSettings`)

```xml
<appSettings>
  <!-- Authority discovery endpoint do B2C -->
  <add key="Sso:MetadataUrl"
       value="https://topconidentity.b2clogin.com/topconidentity.onmicrosoft.com/B2C_1_signinflow/v2.0/.well-known/openid-configuration" />

  <!-- Issuer esperado (string exata, sem barra trocada) -->
  <add key="Sso:Issuer"
       value="https://topconidentity.b2clogin.com/eb415800-08f8-4c8c-a912-8e2246ff2408/v2.0/" />

  <!-- Audience opcional (clientId da App Reg compartilhada) — defesa em
       profundidade contra tokens da Microsoft genéricos vazando -->
  <add key="Sso:Audience"
       value="1d8965e0-2e10-45f0-a8dc-a6fb9ac06fc4" />

  <!-- User flows aceitos. Por ambiente: signinflow (PRD),
       HML_signinflow (HML), QA_signinflow (QA/DEV) -->
  <add key="Sso:AcceptedTfp"
       value="B2C_1_signinflow,B2C_1_HML_signinflow,B2C_1_QA_signinflow" />

  <!-- URL do Identity (só pra redirect quando o usuário precisa logar) -->
  <add key="Sso:IdentityUrl" value="https://topconsuite.io" />

  <!-- Nome do módulo deste deploy (gate de autorização) -->
  <add key="Sso:ModuleName" value="CRM" />

  <!-- Feature flag pra ativar SSO neste deploy (coexistência com login antigo) -->
  <add key="Sso:Enabled" value="false" />
</appSettings>
```

### 2.2. Dependências NuGet

- `Microsoft.IdentityModel.Protocols.OpenIdConnect` (≥ 6.x) — pra ler o
  metadata do B2C e expor o JWKS.
- `Microsoft.IdentityModel.Tokens` — pra validar o JWT.
- `System.IdentityModel.Tokens.Jwt` — pra parsear o JWT.

Caso o CRM já use `Microsoft.Owin.Security.Jwt`, dá pra reusar a mesma
infra — só amarra contra o metadata B2C.

---

## 3. Fluxo end-to-end

```
[Launcher Identity]
      │
      │ click no card CRM → acquireTokenSilent → redirect com fragmento
      ▼
GET https://<cliente>-remote.topconsuite.app/auth/sso
   #id_token=eyJ...&access_token=eyJ...&token_type=Bearer&expires_in=3600
      │
      │ página estática lê fragmento via JS, faz POST
      ▼
POST /auth/sso/complete   (body: id_token, access_token, return_to)
      │
      ├─ valida assinatura via JWKS B2C
      ├─ valida iss exato
      ├─ valida tfp ∈ aceitos
      ├─ valida exp/nbf
      ├─ valida CRM ∈ split(extension_Modules, ",")  ← gate de autorização
      ├─ JIT: cria/atualiza usuário local indexado por sub
      └─ emite cookie de sessão local (HttpOnly, Secure, SameSite=Lax)
      │
      ▼
302 Location: <return_to ou "/">
```

---

## 4. Implementação backend (.NET Framework)

### 4.1. Validador de token

Crie uma classe `B2CTokenValidator` que carrega o metadata B2C (cacheado
24h pelo `ConfigurationManager` do `IdentityModel`) e expõe `Validate(token)`:

```csharp
// File: Crm.Web/Auth/B2CTokenValidator.cs
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Web.Auth
{
    public class B2CTokenValidator
    {
        private static readonly ConfigurationManager<OpenIdConnectConfiguration> _cfg =
            new ConfigurationManager<OpenIdConnectConfiguration>(
                ConfigurationManager.AppSettings["Sso:MetadataUrl"],
                new OpenIdConnectConfigurationRetriever());

        public ValidatedToken Validate(string jwt)
        {
            var openIdConfig = _cfg.GetConfigurationAsync(CancellationToken.None)
                .GetAwaiter().GetResult();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = ConfigurationManager.AppSettings["Sso:Issuer"],
                ValidateAudience = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Sso:Audience"]),
                ValidAudience = ConfigurationManager.AppSettings["Sso:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                IssuerSigningKeys = openIdConfig.SigningKeys,
                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(jwt, parameters, out var validated);

            var token = (JwtSecurityToken)validated;

            // Reject tokens emitted by user flows we do not accept (e.g. a
            // standalone password-reset flow that would not yield a usable
            // session).
            var acceptedTfp = (ConfigurationManager.AppSettings["Sso:AcceptedTfp"] ?? string.Empty)
                .Split(',')
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var tfp = token.Claims.FirstOrDefault(c => c.Type == "tfp")?.Value
                   ?? token.Claims.FirstOrDefault(c => c.Type == "acr")?.Value;

            if (tfp == null || !acceptedTfp.Contains(tfp))
                throw new SecurityTokenInvalidIssuerException($"Unrecognised user flow: {tfp}");

            return new ValidatedToken(principal, token);
        }
    }
}
```

Algumas notas:
- `ConfigurationManager<OpenIdConnectConfiguration>` faz cache automático
  do JWKS (default ≈ 1h, com refresh em background). Não precisa cachear
  por conta própria.
- `ClockSkew = 2min` cobre dessincronia de relógio entre servidores.
- O claim `tfp` é o nome do user flow. Em alguns Custom Policies do B2C
  ele vem como `acr` — o validador aceita os dois nomes.

### 4.2. Endpoint `POST /auth/sso/complete`

```csharp
// File: Crm.Web/Controllers/Auth/SsoController.cs
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Web.Controllers.Auth
{
    [RoutePrefix("auth/sso")]
    public class SsoController : Controller
    {
        private readonly B2CTokenValidator _validator;
        private readonly IUserProvisioningService _provisioning;
        private readonly ISessionCookieIssuer _cookie;

        public SsoController(
            B2CTokenValidator validator,
            IUserProvisioningService provisioning,
            ISessionCookieIssuer cookie)
        {
            _validator = validator;
            _provisioning = provisioning;
            _cookie = cookie;
        }

        [HttpPost, Route("complete"), AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Complete(string id_token, string access_token, string return_to)
        {
            // SSO feature flag — when off, this endpoint is a no-op so the
            // legacy form-based login keeps owning the tenant.
            if (!bool.TryParse(ConfigurationManager.AppSettings["Sso:Enabled"], out var enabled) || !enabled)
                return new HttpStatusCodeResult(404);

            if (string.IsNullOrWhiteSpace(id_token))
                return new HttpStatusCodeResult(400, "Missing id_token.");

            ValidatedToken validated;
            try
            {
                validated = _validator.Validate(id_token);
            }
            catch (Exception ex)
            {
                // Do not leak the exception message to the user; it may
                // include token internals.
                System.Diagnostics.Trace.TraceWarning("SSO token validation failed: " + ex.Message);
                return new HttpStatusCodeResult(401, "Invalid token.");
            }

            // Authorization gate: this CRM deploy only accepts tokens whose
            // extension_Modules carries the configured module name.
            var modulesClaim = validated.Token.Claims
                .FirstOrDefault(c => c.Type == "extension_Modules")?.Value ?? string.Empty;
            var modules = modulesClaim
                .Split(',')
                .Select(m => m.Trim())
                .Where(m => m.Length > 0)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var moduleName = ConfigurationManager.AppSettings["Sso:ModuleName"] ?? "CRM";
            if (!modules.Contains(moduleName))
                return new HttpStatusCodeResult(403,
                    $"You do not have access to {moduleName}. Please contact your Topcon admin.");

            // JIT provisioning + cookie
            var sub = validated.Token.Claims.First(c => c.Type == "sub" || c.Type == "oid").Value;
            var email = validated.Token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var name = validated.Token.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var tenantId = validated.Token.Claims
                .FirstOrDefault(c => c.Type == "extension_TenantId")?.Value;

            var localUser = _provisioning.UpsertFromSso(sub, email, name, tenantId);

            _cookie.Issue(HttpContext, localUser);

            // Validate the return_to is within this CRM host — open-redirect
            // guard. Anything outside is replaced with "/".
            var safeReturn = ResolveSafeReturn(return_to);

            return new RedirectResult(safeReturn);
        }

        private string ResolveSafeReturn(string return_to)
        {
            if (string.IsNullOrWhiteSpace(return_to)) return "/";
            if (!Uri.TryCreate(return_to, UriKind.RelativeOrAbsolute, out var uri)) return "/";

            // Allow only relative URLs OR absolute URLs to the same host.
            if (!uri.IsAbsoluteUri) return return_to;
            if (uri.Host.Equals(Request.Url.Host, StringComparison.OrdinalIgnoreCase)) return uri.PathAndQuery;
            return "/";
        }
    }
}
```

### 4.3. JIT provisioning

`UpsertFromSso` deve:

1. Procurar usuário local pelo `external_id = sub`. Achou → atualiza
   `email`/`name` se mudaram, retorna.
2. Senão, procurar pelo `email`. Achou → faz **merge**: setar
   `external_id = sub` no registro existente, atualiza `name` se vazio.
   Isso cobre o cenário do usuário que já existia no CRM antes do SSO
   ligar — ele passa a logar via SSO sem perder histórico interno
   (vendas, contatos, etc.).
3. Senão, cria novo: `external_id = sub`, `email`, `name`, e o
   `tenant_id` interno do CRM mapeado a partir de `extension_TenantId`
   (que é o GUID do tenant Topcon, não do CRM).

```csharp
public LocalUser UpsertFromSso(string externalId, string email, string name, string tenantId)
{
    var byExternal = _users.FindByExternalId(externalId);
    if (byExternal != null)
    {
        byExternal.Email = email ?? byExternal.Email;
        byExternal.Name = name ?? byExternal.Name;
        _users.Update(byExternal);
        return byExternal;
    }

    var byEmail = _users.FindByEmail(email);
    if (byEmail != null)
    {
        byEmail.ExternalId = externalId;
        if (string.IsNullOrWhiteSpace(byEmail.Name)) byEmail.Name = name;
        _users.Update(byEmail);
        return byEmail;
    }

    var created = new LocalUser
    {
        ExternalId = externalId,
        Email = email,
        Name = name,
        TenantId = ResolveCrmTenantId(tenantId),
        CreatedVia = "SSO",
        CreatedAt = DateTime.UtcNow
    };
    _users.Insert(created);
    return created;
}
```

### 4.4. Cookie de sessão local

Usar o mesmo mecanismo de cookie que o CRM já usa hoje pro login antigo
(ASP.NET Forms Authentication ou cookie próprio assinado). Requisitos
mínimos:

- `HttpOnly = true`
- `Secure = true` (HTTPS obrigatório)
- `SameSite = Lax` (não pode ser `None` sem `Secure`)
- Tempo de vida: ≤ 1h (alinhado com o `exp` típico do token B2C).
  Renovação via novo SSO redirect.

### 4.5. Middleware "se 401-UI, redirecionar pro Identity"

Pra UI: quando o usuário acessa uma página interna sem cookie válido,
em vez de mostrar 401 redireciona pra Identity com `return_to`:

```csharp
public class SsoRedirectFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationContext context)
    {
        if (context.Result is HttpUnauthorizedResult
            && context.HttpContext.Request.AcceptTypes?.Contains("text/html") == true)
        {
            var identityUrl = ConfigurationManager.AppSettings["Sso:IdentityUrl"];
            var moduleName = ConfigurationManager.AppSettings["Sso:ModuleName"];
            var currentUrl = context.HttpContext.Request.Url.PathAndQuery;
            var encoded = HttpUtility.UrlEncode(currentUrl);

            context.Result = new RedirectResult(
                $"{identityUrl}/launch?product={moduleName}&return={encoded}");
        }
    }
}
```

Registrar como filtro global ou seletivamente nos controllers de UI.
APIs (JSON) seguem retornando 401 puro.

---

## 5. Implementação frontend (Angular)

### 5.1. Página `/auth/sso`

Componente que lê o fragmento e faz POST no backend. Pode ser uma rota
Angular vazia que executa o handoff e some.

```typescript
// File: src/app/auth/sso-callback.component.ts
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-sso-callback',
  template: '<p>Signing in…</p>'
})
export class SsoCallbackComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const hash = window.location.hash.startsWith('#')
      ? window.location.hash.substring(1)
      : window.location.hash;
    const params = new URLSearchParams(hash);

    const idToken = params.get('id_token');
    const accessToken = params.get('access_token');
    const returnTo = params.get('return_to') ?? '/';

    if (!idToken) {
      this.router.navigate(['/login']);
      return;
    }

    // Strip the fragment so the token does not linger in window.location
    // or get bookmarked by mistake.
    history.replaceState(null, '', window.location.pathname);

    const formData = new FormData();
    formData.append('id_token', idToken);
    formData.append('access_token', accessToken ?? '');
    formData.append('return_to', returnTo);

    this.http.post('/auth/sso/complete', formData, { withCredentials: true, observe: 'response' })
      .subscribe({
        next: (resp) => {
          // The backend responds with 302 Location; the browser follows it
          // automatically if we use a form-style POST instead. Either way,
          // navigate the SPA to the resolved return_to.
          window.location.assign(resp.headers.get('Location') ?? returnTo);
        },
        error: () => this.router.navigate(['/login', { ssoFailed: true }])
      });
  }
}
```

### 5.2. Rota

```typescript
// File: src/app/auth/auth.module.ts
const routes: Routes = [
  { path: 'auth/sso', component: SsoCallbackComponent },
  // … resto
];
```

Configurar o servidor (IIS/Kestrel) pra **não interceptar `/auth/sso`** com
fallback de UI — deixar o Angular roteá-la, ou implementar como página
estática `.html` separada.

### 5.3. Modo coexistência

Enquanto `Sso:Enabled = false`:
- A página `/auth/sso` continua existindo, mas o `/auth/sso/complete`
  retorna 404 (item 4.2). Usuário cai no formulário antigo.
- A tela de Login antiga continua sendo o caminho principal.

Quando ativar `Sso:Enabled = true`:
- O fluxo SSO passa a funcionar.
- A tela de Login antiga **continua funcionando em paralelo** durante a
  coexistência (~4 meses). Usuários migrados usam SSO; não-migrados
  usam o caminho antigo.

---

## 6. Segurança

Validações obrigatórias (resumo do que o código acima já faz):

| # | Validação | Falha → resposta |
|---|---|---|
| 1 | Assinatura via JWKS B2C | 401 |
| 2 | `iss` exato | 401 |
| 3 | `tfp`/`acr` ∈ user flows aceitos | 401 |
| 4 | `exp > now`, `nbf <= now` | 401 |
| 5 | `sub` presente | 401 |
| 6 | `extension_Modules` contém `"CRM"` | **403** com mensagem clara |
| 7 | `return_to` é relativo OU mesmo host | 200 (sanitizado) |
| 8 | `aud == <clientId comum>` (opcional) | 401 |

HTTPS obrigatório em todas as rotas. HSTS recomendado.

Cookie:
- `HttpOnly`, `Secure`, `SameSite=Lax`.
- Tempo de vida ≤ 1h.

---

## 7. Roteiro de cutover por cliente

1. Time CRM faz deploy com `Sso:Enabled = false` em todas as instâncias.
2. Identity team confirma que o claim `extension_Modules` aparece nos
   tokens de PRD (validado via JWT decoder).
3. Selecionar 1 cliente piloto. Definir um dia D.
4. No dia D, no deploy do piloto:
   - Setar `Sso:Enabled = true` no `Web.config`.
   - Comunicar usuários: "agora vocês podem entrar via `topconsuite.io`,
     mas o login antigo ainda funciona."
5. Monitorar ~1 semana: erros de validação, 403 indevidos, latência.
6. Repetir pra mais clientes em batches.
7. Após ~4 meses estáveis, **desativar** o login antigo (mexer no código
   do form-based: removê-lo ou esconder atrás de um feature flag interno
   pra emergências).

---

## 8. Testes recomendados

| # | Cenário | Esperado |
|---|---|---|
| T1 | POST com `id_token` válido, módulo CRM em `extension_Modules` | 302 Location + cookie de sessão |
| T2 | POST com `id_token` válido, **sem** CRM em `extension_Modules` | 403 com mensagem |
| T3 | POST com `id_token` expirado | 401 |
| T4 | POST com `id_token` de outro issuer (login.microsoft.com) | 401 |
| T5 | POST com `id_token` de user flow não aceito (`B2C_1_reset`) | 401 |
| T6 | POST com `return_to=https://attacker.com/x` | redirect pra `/` (sanitizado) |
| T7 | POST com `return_to=/pedidos/123` | redirect pra `/pedidos/123` |
| T8 | Primeiro SSO de usuário que já existia por email no CRM | merge: `external_id` atualizado, dados preservados |
| T9 | Primeiro SSO de usuário totalmente novo | usuário criado com `CreatedVia=SSO` |
| T10 | `Sso:Enabled=false` | endpoint retorna 404 |
| T11 | UI sem cookie → redirect pra Identity `/launch?product=CRM&return=...` | 302 |
| T12 | API sem cookie → 401 puro (sem redirect) | 401 |

---

## 9. O que **não** está neste guia

- **Backchannel logout / Single Logout**: Phase 2.
- **MFA**: herdado do user flow B2C; o CRM não trata.
- **Auditoria detalhada**: o CRM já tem logs; só recomenda incluir
  `external_id` (sub) em events relevantes pra correlacionar com o Identity.
- **Permissões internas do CRM** (telas, ações): continuam como hoje —
  Identity só responde "esta pessoa pode entrar?", não "esta pessoa pode
  ver isso?".

---

## 10. Dúvidas previstas

- **Posso usar o `access_token` em vez do `id_token`?**
  Pode, são essencialmente os mesmos sob o modelo de App Reg compartilhada
  (mesmo audience, mesmos claims). O `id_token` é o canônico OIDC e o que
  recomendamos validar.
- **E se o usuário não tem `extension_TenantId`?**
  Não deveria acontecer — o Connector do Identity preenche pra todo usuário
  conhecido. Se acontecer, devolva 401 ("malformed token") e abra ticket
  com o time Identity.
- **O que muda quando trocar o IdP no futuro?**
  Os valores em `Sso:Issuer` e `Sso:MetadataUrl` mudam. Toda a lógica de
  validação continua. Mantenha esses valores em configuração, não no
  código.

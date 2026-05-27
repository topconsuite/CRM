using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.API.Security
{
    public class AuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        IUsuarioApplicationService _usuarioAppService;
        ISsoApplicationService _ssoApplicationService;

        public IHandler<DomainNotification> Notifications;

        const string API_VERSION = "2017.08.18/001";


        public AuthAuthorizationServerProvider(IUsuarioApplicationService usuarioAppService, ISsoApplicationService ssoApplicationService)
        {
            _usuarioAppService = usuarioAppService;
            Notifications = DomainEvent.Container.GetService<IHandler<DomainNotification>>();
            _ssoApplicationService = ssoApplicationService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Recupera parametro de versão da API e, se foi passado, adiciona ao OwinContext
            string[] apiVersion = context.Parameters.Where(f => f.Key == "api_version").Select(f => f.Value).SingleOrDefault();
            if (apiVersion != null)
            {
                context.OwinContext.Set<string>("api_version", apiVersion[0]);
            }

            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Caso seja necessário criar uma aplicação com a finalidade única de autenticação
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.OwinContext.Response.Headers.Add("api-date-version", new[] { VersionHelper.topconApiDateVersion });
            context.OwinContext.Response.Headers.Add("Access-Control-Expose-Headers", new[] { "api-date-version" });

            // Recupera parametro de versão da API informada pelo client e valida
            string apiVersionFrontend = context.OwinContext.Get<string>("api_version");
            if (apiVersionFrontend == null || !apiVersionFrontend.Equals(API_VERSION))
            {
                context.SetError("invalid_client", "Versão inválida.\nVerifique a versão do seu aplicativo.");
                return;
            }

            AutenticarUsuarioResponse autenticarUsuarioResponse = null;

            try
            {
                autenticarUsuarioResponse = _usuarioAppService.Autenticar(context.UserName, context.Password);
            }
            catch(Exception e)
            {
                context.SetError("internal_server_error", e?.Message);
                return;
            }
            
            if (autenticarUsuarioResponse == null)
            {
                context.SetError("invalid_grant", Notifications.Notify().FirstOrDefault()?.Message);
                Notifications.Dispose();
                return;
            }

            Notifications.Dispose();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            AddClaimsToIdentity(autenticarUsuarioResponse, identity);

            // Adicionando o objeto identy na thread principal
            GenericPrincipal principal = new GenericPrincipal(identity, new string[] { });

            Thread.CurrentPrincipal = principal;

            AuthenticationTicket ticket = GenerateAuthenticationTicket(autenticarUsuarioResponse, identity);
            await Task.FromResult(context.Validated(ticket));

            //await Task.FromResult(context.Validated(identity));

        }

        private static AuthenticationTicket GenerateAuthenticationTicket(AutenticarUsuarioResponse autenticarUsuarioResponse, ClaimsIdentity identity)
        {
            var kvs = autenticarUsuarioResponse.Direitos.Select(kvp => string.Format("'{0}':'{1}'", kvp.Key, kvp.Value));
            var jsonDireitos = string.Concat("'direitos':{", string.Join(",", kvs), "}");
            var user = string.Concat("{", string.Format("'id':'{0}','nome':'{1}',{2}", autenticarUsuarioResponse.UsuarioId, autenticarUsuarioResponse.Nome, jsonDireitos), "}").Replace("'", "\"");
            var props = new AuthenticationProperties(new Dictionary<string, string> { { "user", user } });
            var ticket = new AuthenticationTicket(identity, props);
            return ticket;
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            if (context.GrantType == "b2c")
            {
                await HandleB2CGrant(context);
                return;
            }

            if (context.GrantType == "azure")
            {

                // Caso seja necessário criar uma aplicação com a finalidade única de autenticação
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("api-date-version", new[] { VersionHelper.topconApiDateVersion });
                context.OwinContext.Response.Headers.Add("Access-Control-Expose-Headers", new[] { "api-date-version" });

                var azureToken = context.Parameters.Get("assertion");

                if (string.IsNullOrEmpty(azureToken))
                {
                    context.SetError("invalid_grant", "Azure AD token faltando.");
                    return;
                }

                var ssoConfig = _ssoApplicationService.ObterParametroAtivoAzureAd();
                if (ssoConfig == null || string.IsNullOrEmpty(ssoConfig.ClientId) || string.IsNullOrEmpty(ssoConfig.TenantId))
                {
                    context.SetError("server_error", "Configuração não encontrada.");
                    return;
                }

                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = ssoConfig.ClientId,
                    ValidIssuer = $"https://login.microsoftonline.com/{ssoConfig.TenantId}/v2.0",
                    IssuerSigningKeys = GetAzureSigningKeys(ssoConfig.TenantId)
                };

                
                ClaimsPrincipal principal;
                try
                {
                    principal = handler.ValidateToken(azureToken, validationParameters, out var validatedToken);
                }
                catch (Exception ex)
                {
                    context.SetError("invalid_grant", "Validação do Token falhou " + ex.Message);
                    return;
                }

                var userIdentifier = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var email = principal.FindFirst("preferred_username")?.Value
                         ?? principal.FindFirst(ClaimTypes.Upn)?.Value;

                if (string.IsNullOrEmpty(userIdentifier))
                {
                    context.SetError("invalid_grant", "Identificação do usuário não encontrada.");
                    return;
                }

                var usuario = _usuarioAppService.ObterUsuarioPeloEmail(email);
                if (usuario == null)
                {
                    var newUsuarioRequest = new Application.DTOS.Request.Usuario.RegistrarUsuarioRequest(email);
                    var newUsuarioResponse = _usuarioAppService.Registrar(newUsuarioRequest);

                    if (newUsuarioResponse == null)
                    {
                        context.SetError("invalid_grant", "Não foi possível registrar o usuário.");
                        return;
                    }


                    usuario = _usuarioAppService.ObterUsuarioPeloEmail(email);
                }

                var autenticarUsuarioResponse = _usuarioAppService.Autenticar(usuario.Id, StringHelper.EncrypTopSys(usuario.Senha));

                if (autenticarUsuarioResponse == null)
                {
                    context.SetError("invalid_grant", "Não foi possível autenticar o usuário no Topcon.");
                    return;
                }


                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                AddClaimsToIdentity(autenticarUsuarioResponse, identity);

                AuthenticationTicket ticket = GenerateAuthenticationTicket(autenticarUsuarioResponse, identity);

                await Task.FromResult(context.Validated(ticket));
            }
        }

        private static readonly B2CTokenValidator _b2cValidator = new B2CTokenValidator();

        private async Task HandleB2CGrant(OAuthGrantCustomExtensionContext context)
        {
            // Common CORS / versioning headers — same as the legacy "azure" grant.
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.OwinContext.Response.Headers.Add("api-date-version", new[] { VersionHelper.topconApiDateVersion });
            context.OwinContext.Response.Headers.Add("Access-Control-Expose-Headers", new[] { "api-date-version" });

            // SSO B2C config + feature gate live together in topsys.parametros_sso
            // (tipo_provedor = B2C). The row carries dominio (B2C subdomain),
            // tenant_id (B2C directory GUID) and client_id (App Reg audience),
            // mirroring how the Azure AD legacy grant reads its own row.
            // See docs/sso-decisoes-implementacao.md (D2).
            var ssoConfig = _ssoApplicationService.ObterParametroAtivoB2C();
            if (ssoConfig == null)
            {
                context.SetError("invalid_grant", "SSO desabilitado.");
                return;
            }

            var idToken = context.Parameters.Get("assertion") ?? context.Parameters.Get("id_token");
            if (string.IsNullOrEmpty(idToken))
            {
                context.SetError("invalid_grant", "id_token ausente.");
                return;
            }

            B2CValidatedToken validated;
            try
            {
                validated = _b2cValidator.Validate(idToken, ssoConfig);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("SSO B2C token validation failed: " + ex.GetType().Name + ": " + ex.Message);
                context.SetError("invalid_grant", "Token inválido.");
                return;
            }

            // ADMIN bypass: tokens carrying extension_Admin = true authenticate
            // as the local "ADMIN" row in topsys.usr_usuario, skipping the
            // CRM module gate and the JIT-by-email path. See D14 in
            // docs/sso-decisoes-implementacao.md.
            Usuario usuario;
            bool isAdmin;
            bool.TryParse(validated.GetClaim("extension_Admin"), out isAdmin);

            if (isAdmin)
            {
                usuario = _usuarioAppService.ObterPorId("ADMIN");
                if (usuario == null)
                {
                    context.SetError("invalid_grant", "Usuário ADMIN não encontrado.");
                    return;
                }
            }
            else
            {
                // Authorization gate: this deploy only accepts tokens whose
                // extension_Modules carries the configured module name (CRM).
                var moduleName = ConfigurationManager.AppSettings["Sso:ModuleName"] ?? "CRM";
                var modules = validated.GetModules();
                if (!modules.Contains(moduleName))
                {
                    context.SetError("invalid_grant",
                        $"Você não tem acesso ao {moduleName}. Procure o administrador Topcon.");
                    return;
                }

                var email = validated.GetClaim("email", "emails", "preferred_username", "upn", ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    context.SetError("invalid_grant", "Email não encontrado no token.");
                    return;
                }

                // JIT provisioning: mirror the existing "azure" grant path —
                // index by email. See docs/sso-decisoes-implementacao.md (D3).
                usuario = _usuarioAppService.ObterUsuarioPeloEmail(email);
                if (usuario == null)
                {
                    var register = new Application.DTOS.Request.Usuario.RegistrarUsuarioRequest(email);
                    var registered = _usuarioAppService.Registrar(register);
                    if (registered == null)
                    {
                        context.SetError("invalid_grant", "Não foi possível registrar o usuário.");
                        return;
                    }
                    usuario = _usuarioAppService.ObterUsuarioPeloEmail(email);
                }
            }

            AutenticarUsuarioResponse autenticarUsuarioResponse;
            try
            {
                autenticarUsuarioResponse = _usuarioAppService.Autenticar(usuario.Id, StringHelper.EncrypTopSys(usuario.Senha));
            }
            catch (Exception ex)
            {
                context.SetError("internal_server_error", ex?.Message);
                return;
            }

            if (autenticarUsuarioResponse == null)
            {
                context.SetError("invalid_grant", Notifications.Notify().FirstOrDefault()?.Message ?? "Falha de autenticação local.");
                Notifications.Dispose();
                return;
            }
            Notifications.Dispose();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            AddClaimsToIdentity(autenticarUsuarioResponse, identity);

            var ticket = GenerateAuthenticationTicket(autenticarUsuarioResponse, identity);
            await Task.FromResult(context.Validated(ticket));
        }

        private void AddClaimsToIdentity(AutenticarUsuarioResponse autenticatUsuarioResponse, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, autenticatUsuarioResponse.UsuarioId.ToString()));

            var claimsVendedores = _usuarioAppService.ObterClaimsVendedores(autenticatUsuarioResponse);
            foreach (var kv in claimsVendedores)
            {
                identity.AddClaim(new Claim(kv.Key, kv.Value));
            }
        }

        private static IEnumerable<SecurityKey> GetAzureSigningKeys(string tenantId)
        {
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"https://login.microsoftonline.com/{tenantId}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

            var config = configManager.GetConfigurationAsync().Result;
            return config.SigningKeys;
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            if ( context.Properties.Dictionary.ContainsKey("user") ) context.Properties.Dictionary.Remove("user");

            return Task.FromResult<object>(null);
        }
    }
}
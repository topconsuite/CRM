using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Security
{
    public class B2CValidatedToken
    {
        public ClaimsPrincipal Principal { get; }
        public JwtSecurityToken Token { get; }

        public B2CValidatedToken(ClaimsPrincipal principal, JwtSecurityToken token)
        {
            Principal = principal;
            Token = token;
        }

        public string GetClaim(params string[] claimTypes)
        {
            foreach (var t in claimTypes)
            {
                var c = Token.Claims.FirstOrDefault(x => x.Type == t);
                if (c != null && !string.IsNullOrWhiteSpace(c.Value)) return c.Value;
            }
            return null;
        }

        public ISet<string> GetModules()
        {
            var raw = GetClaim("extension_Modules") ?? string.Empty;
            return new HashSet<string>(
                raw.Split(',').Select(m => m.Trim()).Where(m => m.Length > 0),
                StringComparer.OrdinalIgnoreCase);
        }
    }

    public class B2CTokenValidator
    {
        private static readonly Lazy<ConfigurationManager<OpenIdConnectConfiguration>> _cfg =
            new Lazy<ConfigurationManager<OpenIdConnectConfiguration>>(() =>
            {
                var metadataUrl = ConfigurationManager.AppSettings["Sso:MetadataUrl"];
                if (string.IsNullOrWhiteSpace(metadataUrl))
                    throw new InvalidOperationException("Sso:MetadataUrl not configured.");
                return new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataUrl, new OpenIdConnectConfigurationRetriever());
            });

        public virtual B2CValidatedToken Validate(string jwt, ParametrosSSO ssoConfig)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                throw new ArgumentException("Empty token.", nameof(jwt));
            if (ssoConfig == null)
                throw new ArgumentNullException(nameof(ssoConfig));
            if (string.IsNullOrWhiteSpace(ssoConfig.Dominio))
                throw new InvalidOperationException("B2C parametros_sso row is missing dominio.");
            if (string.IsNullOrWhiteSpace(ssoConfig.TenantId))
                throw new InvalidOperationException("B2C parametros_sso row is missing tenant_id.");
            if (string.IsNullOrWhiteSpace(ssoConfig.ClientId))
                throw new InvalidOperationException("B2C parametros_sso row is missing client_id.");

            var openIdConfig = _cfg.Value
                .GetConfigurationAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            // Issuer is constructed from the B2C directory subdomain (dominio)
            // and tenant GUID — same shape the Azure AD legacy grant uses, just
            // pointed at b2clogin.com instead of login.microsoftonline.com.
            var issuer = $"https://{ssoConfig.Dominio}/{ssoConfig.TenantId}/v2.0/";

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = ssoConfig.ClientId,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                IssuerSigningKeys = openIdConfig.SigningKeys,
                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken validated;
            var principal = handler.ValidateToken(jwt, parameters, out validated);
            var token = (JwtSecurityToken)validated;

            // Reject tokens issued by user flows we do not accept (e.g. a
            // standalone password-reset flow that would not yield a usable
            // session). B2C custom policies emit "acr"; built-in user flows
            // emit "tfp" — accept either.
            var acceptedRaw = ConfigurationManager.AppSettings["Sso:AcceptedTfp"] ?? string.Empty;
            var accepted = new HashSet<string>(
                acceptedRaw.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0),
                StringComparer.OrdinalIgnoreCase);

            var tfp = token.Claims.FirstOrDefault(c => c.Type == "tfp")?.Value
                   ?? token.Claims.FirstOrDefault(c => c.Type == "acr")?.Value;

            if (string.IsNullOrEmpty(tfp) || (accepted.Count > 0 && !accepted.Contains(tfp)))
                throw new SecurityTokenInvalidIssuerException("Unrecognised user flow: " + (tfp ?? "<null>"));

            var sub = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                   ?? token.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;
            if (string.IsNullOrWhiteSpace(sub))
                throw new SecurityTokenException("Token is missing sub/oid.");

            return new B2CValidatedToken(principal, token);
        }
    }
}

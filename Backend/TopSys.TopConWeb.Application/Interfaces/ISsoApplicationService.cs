using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ISsoApplicationService
    {
        ParametrosSSO ObterParametroAtivoAzureAd();

        /// <summary>
        /// Returns the active Azure AD B2C SSO parameter row, or null when
        /// no B2C row exists or the existing row is disabled. Used as the
        /// feature gate for the OAuth grant_type=b2c flow.
        /// </summary>
        ParametrosSSO ObterParametroAtivoB2C();
    }
}

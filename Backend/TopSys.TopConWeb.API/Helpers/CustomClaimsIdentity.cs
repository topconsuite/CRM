using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq.Expressions;
using TopSys.TopConWeb.API.Converters;

namespace TopSys.TopConWeb.API.Helpers
{
    public static class CustomClaimsIdentity
    {
        private static string GetClaimValue(IIdentity identity, string ClaimName)
        {
            var _claimsIdentity = (ClaimsIdentity)identity;
            return _claimsIdentity.FindFirst(ClaimName).Value;
        }

        public static string VendedoresVinculados(this IIdentity identity)
        {
            return GetClaimValue(identity, "vendedoresVinculados");
        }

        public static string VendedoresPermitidos(this IIdentity identity)
        {
            return GetClaimValue(identity, "vendedoresPermitidos");
        }

        public static Expression<Func<T, bool>> FiltroVendedoresVinculados<T>(this IIdentity identity, string propriedade) where T : class
        {
            var strCodigos = identity.VendedoresVinculados();

            if (strCodigos != "*")
            {
                return UrlFilterParser.Parse<T>($"$({propriedade}=%{strCodigos})");
            }
            else
            {
                return (t => true);
            }
        }

        public static Expression<Func<T, bool>> FiltroVendedoresPermitidos<T>(this IIdentity identity, string propriedade) where T : class
        {
            var strCodigos = identity.VendedoresPermitidos();

            if (strCodigos != "*")
            {
                return UrlFilterParser.Parse<T>($"$({propriedade}=%{strCodigos})");
            }
            else
            {
                return (t => true);
            }
        }
    }
}
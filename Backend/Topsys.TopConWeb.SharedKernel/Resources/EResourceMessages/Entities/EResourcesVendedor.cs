using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de vendedor.
    /// </summary>
    public enum EResourcesVendedor
    {
        /// <summary>
        /// 9.4.1 No records found
        /// </summary>
        VENDEDOR_ERROR_TCON393431,

        /// <summary>
        /// 9.4.2 Company name cannot be empty
        /// </summary>
        VENDEDOR_ERROR_TCON393432,

        /// <summary>
        /// 9.4.3 Name cannot be empty
        /// </summary>
        VENDEDOR_ERROR_TCON393433,

        /// <summary>
        /// 9.4.4 Id not found
        /// </summary>
        VENDEDOR_ERROR_TCON393434,

        /// <summary>
        /// 9.4.5 External_id not found
        /// </summary>
        VENDEDOR_ERROR_TCON393435,

        /// <summary>
        /// 9.4.6 It is not possible to delete the informed record, as it is being used in a registration.
        /// </summary>
        VENDEDOR_ERROR_TCON393436,

        /// <summary>
        /// 9.4.7 Role cannot be empty
        /// </summary>
        VENDEDOR_ERROR_TCON393437,

        /// <summary>
        /// 9.4.8 System user cannot be empty for sellers
        /// </summary>
        VENDEDOR_ERROR_TCON393438,

        /// <summary>
        /// 9.4.9 System user not found
        /// </summary>
        VENDEDOR_ERROR_TCON393439,

        /// <summary>
        /// 9.4.10 Intervener cannot be 0 for representatives
        /// </summary>
        VENDEDOR_ERROR_TCON39343130,

        /// <summary>
        /// 9.4.11 Payment terms cannot be 0 for representatives
        /// </summary>
        VENDEDOR_ERROR_TCON39343131,

        /// <summary>
        /// 9.4.12 Intervener not found
        /// </summary>
        VENDEDOR_ERROR_TCON39343132,

        /// <summary>
        /// 9.4.13 Payment terms not found
        /// </summary>
        VENDEDOR_ERROR_TCON39343133,

        /// <summary>
        /// 9.4.14 RE employee not found
        /// </summary>
        VENDEDOR_ERROR_TCON39343134,

        /// <summary>
        /// 9.4.15 Godfather seller not found
        /// </summary>
        VENDEDOR_ERROR_TCON39343135,

        /// <summary>
        /// 9.4.16 Role doesn’t exist
        /// </summary>
        VENDEDOR_ERROR_TCON39343136,

        /// <summary>
        /// 9.4.17 RE already used by another representative/seller
        /// </summary>
        VENDEDOR_ERROR_TCON39343137,

        /// <summary>
        /// 9.4.18 County id not found
        /// </summary>
        VENDEDOR_ERROR_TCON39343138,

        /// <summary>
        /// 9.4.19 External id provided is already registered
        /// </summary>
        VENDEDOR_ERROR_TCON39343139
    }


    public static class EResourcesVendedorExtensions
    {
        public static string GetResourceMessage(this EResourcesVendedor erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesVendedor error)
        {
            switch (error)
            {
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393431:
                    return "TCON393431";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393432:
                    return "TCON393432";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393433:
                    return "TCON393433";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393434:
                    return "TCON393434";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393435:
                    return "TCON393435";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393436:
                    return "TCON393436";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393437:
                    return "TCON393437";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393438:
                    return "TCON393438";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON393439:
                    return "TCON393439";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343130:
                    return "TCON39343130";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343131:
                    return "TCON39343131";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343132:
                    return "TCON39343132";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343133:
                    return "TCON39343133";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343134:
                    return "TCON39343134";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343135:
                    return "TCON39343135";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343136:
                    return "TCON39343136";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343137:
                    return "TCON39343137";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343138:
                    return "TCON39343138";
                case EResourcesVendedor.VENDEDOR_ERROR_TCON39343139:
                    return "TCON39343139";
                default:
                    return "";
            }
        }
    }
}

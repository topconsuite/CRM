using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de remessa.
    /// </summary>
    public enum EResourcesRemessa
    {
        /// <summary>
        /// 9.7.1 No records found
        /// </summary>
        REMESSA_ERROR_TCON393731
    }

    public static class EResourcesRemessaExtensions
    {
        public static string GetResourceMessage(this EResourcesRemessa erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesRemessa error)
        {
            switch (error)
            {
                case EResourcesRemessa.REMESSA_ERROR_TCON393731:
                    return "TCON393731";
                default:
                    return "";
            }
        }
    }
}

using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de programação.
    /// </summary>
    public enum EResourcesContratoSimplificado
    {
        /// <summary>
        /// 9.8.1 No records found
        /// </summary>
        CONTRATO_SIMPLIFICADO_ERROR_TCON393831,

        /// <summary>
        /// 9.8.2 Waiting for payment data
        /// </summary>
        CONTRATO_SIMPLIFICADO_ERROR_TCON393832,

        ///<summary>
        /// 9.8.3 Payment sequence not found
        ///</summary>
        CONTRATO_SIMPLIFICADO_ERROR_TCON393833,

        ///<summary>
        /// 9.8.4 Payment already approved/disapproved financially
        ///</summary>
        CONTRATO_SIMPLIFICADO_ERROR_TCON393834
    }


    public static class EResourcesContratoSimplificadoExtensions
    {
        public static string GetResourceMessage(this EResourcesContratoSimplificado erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesContratoSimplificado error)
        {
            switch (error)
            {
                case EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393831:
                    return "TCON393831";
                case EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393832:
                    return "TCON393832";
                case EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393833:
                    return "TCON393833";
                case EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393834:
                    return "TCON393834";
                default:
                    return "";
            }
        }
    }
}
using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos do Interveniente.
    /// </summary>
    public enum EResourcesInterveniente
    {
        /// <summary>
        /// 8.1.2 The cnpj_cpf field must have 11 digits for Cpf or 14 digits for Cnpj.
        /// </summary>
        INTERVENIENTE_ERROR_TCON385F315F32,

        /// <summary>
        /// 8.1.3 The client_type field must be either C, F, J or P
        /// </summary>
        INTERVENIENTE_ERROR_TCON385F315F33
    }

    public static class EResourcesIntervenienteExtenions
    {
        public static string GetResourceMessage(this EResourcesInterveniente erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }
        
        public static string GetMessageCode(this EResourcesInterveniente error)
        {
            switch (error)
            {
                case EResourcesInterveniente.INTERVENIENTE_ERROR_TCON385F315F32:
                    return "TCON385F315F32";
                case EResourcesInterveniente.INTERVENIENTE_ERROR_TCON385F315F33:
                    return "TCON385F315F33";
                default:
                    return "";
            }
        }
    }
}
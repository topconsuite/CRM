using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos do cadastro geral.
    /// </summary>
    public enum EResourcesCadastroGeral
    {
        /// <summary>
        /// 9.1.1 No records found
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393131,

        /// <summary>
        /// 9.1.2 External id provided is already registered
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393132,

        /// <summary>
        /// 9.1.3 Numbering range completed.
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393133,

        /// <summary>
        /// 9.1.4 Code provided is already registered
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393134,

        /// <summary>
        /// 9.1.5 Description cannot be empty
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393135,

        /// <summary>
        /// 9.1.6 Short description cannot be empty
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393136,

        /// <summary>
        /// 9.1.7 Id not found.
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393137,

        /// <summary>
        /// 9.1.8 External_id not found.
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393138,

        /// <summary>
        /// 9.1.9 It is not possible to delete the informed record, as it is being used in a registration
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON393139,

        /// <summary>
        /// 9.1.10 It is not possible to update the code of informed record, as it is being used in a registration
        /// </summary>
        CADASTRO_GERAL_ERROR_TCON39313130
    }


    public static class EResourcesCadastroGeralExtensions
    {
        public static string GetResourceMessage(this EResourcesCadastroGeral erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesCadastroGeral error)
        {
            switch (error)
            {
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131:
                    return "TCON393131";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393132:
                    return "TCON393132"; 
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393133:
                    return "TCON393133";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393134:
                    return "TCON393134"; 
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393135:
                    return "TCON393135";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393136:
                    return "TCON393136";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393137:
                    return "TCON393137";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393138:
                    return "TCON393138";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393139:
                    return "TCON393139";
                case EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON39313130:
                    return "TCON39313130";
                default:
                    return "";
            }
        }
    }
}

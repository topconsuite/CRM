using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de funcionário.
    /// </summary>
    public enum EResourcesFuncionario
    {
        /// <summary>
        /// 9.5.1 No records found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393531,

        /// <summary>
        /// 9.5.2 CPF provided is already registered
        /// </summary>
        FUNCIONARIO_ERROR_TCON393532,

        /// <summary>
        /// 9.5.3 External ID provided is already registered
        /// </summary>
        FUNCIONARIO_ERROR_TCON393533,

        /// <summary>
        /// 9.5.4 System user not found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393534,

        /// <summary>
        /// 9.5.5 It is not possible to delete employees that have already been used
        /// </summary>
        FUNCIONARIO_ERROR_TCON393535,

        /// <summary>
        /// 9.5.6 Department not found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393536,

        /// <summary>
        /// 9.5.7 Status not found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393537,

        /// <summary>
        /// 9.5.8 Role not found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393538,

        /// <summary>
        /// 9.5.9 Concrete batching plant not found
        /// </summary>
        FUNCIONARIO_ERROR_TCON393539,

        /// <summary>
        /// 9.5.10 RE already linked to another employee
        /// </summary>
        FUNCIONARIO_ERROR_TCON393540
    }

    public static class EResourcesFuncionarioExtensions
    {
        public static string GetResourceMessage(this EResourcesFuncionario erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesFuncionario error)
        {
            switch (error)
            {
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393532:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393533:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393534:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393535:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393536:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393537:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393538:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393539:
                    return "TCON393731";
                case EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393540:
                    return "TCON393540";
                default:
                    return "";
            }
        }
    }
}

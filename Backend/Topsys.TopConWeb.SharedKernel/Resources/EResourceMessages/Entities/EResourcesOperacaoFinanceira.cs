using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos da Operação Financeira.
    /// </summary>
    public enum EResourcesOperacaoFinanceira
    {
        /// <summary>
        /// 7.2.1 The <T> field must be 0 if the update_bank field is not equal to 9
        /// </summary>
        OPERACAO_FINANCEIRA_ERROR_TCON375f325f31,
        
        /// <summary>
        /// 7.2.2 The sell_operation field must reference a FinanceOperation that has inclusion_discharge = B
        /// </summary>
        OPERACAO_FINANCEIRA_ERROR_TCON375f325f32,
        
        /// <summary>
        /// 7.2.3 The bank_movement_operation field must reference a FinanceOperation that has inclusion_discharge = B and subsystem = <X>
        /// </summary>
        OPERACAO_FINANCEIRA_ERROR_TCON375f325f33
    }

    public static class EResourcesOperacaoFinanceiraExtenions
    {
        public static string GetResourceMessage(this EResourcesOperacaoFinanceira erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }
        
        public static string GetMessageCode(this EResourcesOperacaoFinanceira error)
        {
            switch (error)
            {
                case EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31:
                    return "TCON375f385f31";
                case EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f32:
                    return "TCON375f385f32";
                case EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33:
                    return "TCON375f385f33";
                default:
                    return "";
            }
        }
    }
}
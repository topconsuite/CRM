using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    public enum EResourcesTituloContasAReceber
    {
        /// <summary>
        /// 7.3.1 The value of '{0}' value can't be higher than the title value
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f31,
        
        /// <summary>
        /// 7.3.2 To liquidate a account is necessary the following fields: liquidation date, bank liquidation, operation liquidate
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f32,
        
        /// <summary>
        /// 7.3.3 The amount in the accounts receivable record cannot be less than the amount received
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f33,
        
        /// <summary>
        /// 7.3.4 If the value of the accounts receivable is lower than 0, the amount received can't be even lower than it

        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f34,
        
        /// <summary>
        /// 7.3.5 The sum the current received amount and the liquidation amount can't be higher than the title value
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f35,
        
        /// <summary>
        /// 7.3.6 To liquidate a title the operation subsystem of the operation_liquidate field must be equal to 'BC'
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f36,
        
        /// <summary>
        /// 7.3.7 It is not possible to liquidate this title, it has already been completely liquidated
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f37,

        /// <summary>
        /// 7.3.8 It is not possible to cancel receipt of this title, because it belongs to a closed month
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f38,

        /// <summary>
        /// 7.3.9 It is not possible to cancel receipt of this title, because the main title is liquidated
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f39,

        /// <summary>
        /// 7.4.0 It is not possible to cancel receipt of this title, because full cancellation is necessary when the settlement was made by card
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f40,

        /// <summary>
        /// 7.4.1 It is not possible to cancel receipt of this title, because full cancellation is necessary when the settlement was made by bank check
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f41,

        /// <summary>
        /// 7.4.2 It is not possible to cancel receipt of this title, because a payment return title exists
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f42,

        /// <summary>
        /// 7.4.3 It is not possible to cancel receipt of this title, because there is settlement of the title generated in accounts payable for the title
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f43,

        /// <summary>
        /// 7.4.4 It is not possible to cancel receipt of this title, because there are reconciled bank transactions linked to the title
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f44,

        /// <summary>
        /// 7.4.5 It is not possible to cancel receipt of this title, because there are accounts payable documents used for title write-off
        /// </summary>
        CONTAS_A_RECEBER_ERROR_TCON375f335f45,

        /// <summary>
        /// 7.4.6 It is not possible to cancel receipt of this title, because there is a settled check related to this title
        CONTAS_A_RECEBER_ERROR_TCON375f335f46,

        /// <summary>
        /// 7.4.7 It is not possible to cancel receipt of this title, because there is a credit generated in the settlement that was offset
        CONTAS_A_RECEBER_ERROR_TCON375f335f47,

        /// <summary>
        /// 7.4.8 It is not possible to liquidate this title, because the bank operation for the informed liquidation operation could not be found.
        CONTAS_A_RECEBER_ERROR_TCON375f335f48
    }
    
    public static class EResourcesTituloContasAReceberExtenions
    {
        public static string GetResourceMessage(this EResourcesTituloContasAReceber erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }
        
        public static string GetMessageCode(this EResourcesTituloContasAReceber error)
        {
            switch (error)
            {
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31:
                    return "TCON375f335f31";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f32:
                    return "TCON375f335f32";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33:
                    return "TCON375f335f33";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34:
                    return "TCON375f335f34";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f35:
                    return "TCON375f335f35";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f36:
                    return "TCON375f335f36";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f37:
                    return "TCON375f335f37";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f38:
                    return "TCON375f335f38";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f39:
                    return "TCON375f335f39";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f40:
                    return "TCON375f335f40";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f41:
                    return "TCON375f335f41";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f42:
                    return "TCON375f335f42";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f43:
                    return "TCON375f335f43";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f44:
                    return "TCON375f335f44";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f45:
                    return "TCON375f335f45";
                case EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f48:
                    return "TCON375f335f48";
                default:
                    return "";
            }
        }
    }
}
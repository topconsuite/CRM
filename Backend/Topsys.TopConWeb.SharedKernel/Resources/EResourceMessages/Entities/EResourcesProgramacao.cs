using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de programação.
    /// </summary>
    public enum EResourcesProgramacao
    {
        /// <summary>
        /// 9.6.1 No records found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393631,

        /// <summary>
        /// 9.6.2 Concrete batching plant not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393632,

        /// <summary>
        /// 9.6.3 Delivery concrete batching plant not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393633,

        /// <summary>
        /// 9.6.4 Construction site city not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393634,

        /// <summary>
        /// 9.6.5 Gravel not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393635,

        /// <summary>
        /// 9.6.6 Product group not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393636,

        /// <summary>
        /// 9.6.7 Pump code not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393637,

        /// <summary>
        /// 9.6.8 Resistance type not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393638,

        /// <summary>
        /// 9.6.9 Slump not found
        /// </summary>
        PROGRAMACAO_ERROR_TCON393639,

        /// <summary>
        /// 9.6.10 Not found the contract for the combination of concrete_batching_plant_contract + contract_number + contract_year
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363130,

        /// <summary>
        /// 9.6.11 Not found construction site information for the combination of concrete_batching_plant_contract + construction_site_number + contract_item_sequence
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363131,

        /// <summary>
        /// 9.6.12 Concrete recipe information does not match what was reported for the construction site
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363132,

        /// <summary>
        /// 9.6.13 Scheduled volume greater than contracted volume
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363133,

        /// <summary>
        /// 9.6.14 This quantity released field is mandatory when confirm_delivery is equal to 'P'
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363134,

        /// <summary>
        /// 9.6.15 Pump time must be less than delivery time
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363135,

        /// <summary>
        /// 9.6.16 The pump piping distance field is mandatory if it informs the type of pump
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363136,

        /// <summary>
        /// 9.6.17 The pump code field is mandatory if it informs the type of pump
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363137,

        /// <summary>
        /// 9.6.18 Registered key (concrete_batching_plant_contract + contract_year + contract_number + concreting_sequence) already exists.
        /// </summary>
        PROGRAMACAO_ERROR_TCON39363138
    }


    public static class EResourcesProgramacaoExtensions
    {
        public static string GetResourceMessage(this EResourcesProgramacao erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesProgramacao error)
        {
            switch (error)
            {
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631:
                    return "TCON393631";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393632:
                    return "TCON393632";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393633:
                    return "TCON393633";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393634:
                    return "TCON393634";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393635:
                    return "TCON393635";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393636:
                    return "TCON393636";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393637:
                    return "TCON393637";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393638:
                    return "TCON393638";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393639:
                    return "TCON393639";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363130:
                    return "TCON39363130";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363131:
                    return "TCON39363131";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363132:
                    return "TCON39363132";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363133:
                    return "TCON39363133";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363134:
                    return "TCON39363134";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363135:
                    return "TCON39363135";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363136:
                    return "TCON39363136";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363137:
                    return "TCON39363137";
                case EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363138:
                    return "TCON39363138";
                default:
                    return "";
            }
        }
    }
}

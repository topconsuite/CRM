using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de mensagens padrão.
    /// </summary>
    public enum EResourcesDefaultMessages
    {
        /// <summary>
        /// Internal endpoint error
        /// </summary>
        DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR,

        /// <summary>
        /// Internal error occurred while processing record {0} of the list
        /// </summary>
        DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST,

        /// <summary>
        /// The field '{0}' provided doesn't exist in our system
        /// </summary>
        DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM,

        /// <summary>
        /// The {0} does not follow the correct pattern
        /// </summary>
        DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN,

        /// <summary>
        /// The field '{0}' provided already exists in our system
        /// </summary>
        DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM,

        /// <summary>
        /// {0} can’t be deleted since it’s already in use by the system
        /// </summary>
        DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE,

        /// <summary>
        /// {0} is necessary because it follows the rule of: <RULE>
        /// </summary>
        DEFAULT_MESSAGES_REQUIRED_TO_FOLLOW_THE_RULE,

        /// <summary>
        /// Field '{0}' cannot be empty
        /// </summary>
        DEFAULT_MESSAGES_CANNOT_BE_EMPTY,

        /// <summary>
        /// The '{0}' field cannot be longer than {1} characters
        /// </summary>
        DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED,

        /// <summary>
        /// The '{0}' field must be a number greater than 0 with up to {1} digits
        /// </summary>
        DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED,

        /// <summary>
        /// The '{0}' field must be a number with up to {1} digits
        /// </summary>
        DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED,

        /// <summary>
        /// Precondition failed
        /// </summary>
        DEFAULT_MESSAGES_PRECONDITION_FAILED,

        /// <summary>
        /// Error processing records
        /// </summary>
        DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS,
        
        /// <summary>
        /// Duplicate <T> in the list
        /// </summary>
        DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST,
        
        /// <summary>
        /// The <T> field cannot have more than <X> elements
        /// </summary>
        DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED, 
        
        /// <summary>
        /// The <T> field must be a number greater than <X> and less than <Y>
        /// </summary>
        DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN, 
        
        /// <summary>
        /// The Field <T> must have at least <X> and at most <Y> elements in the list
        /// </summary>
        DEFAULT_MESSAGES_AT_LEAST_AND_AT_MOST_ELEMENTS,
        
        
        /// <summary>
        /// The sum of <T> field must be equal to <X>
        /// </summary>
        DEFAULT_MESSAGES_SUM_OF_FIELD,
        
        
        /// <summary>
        /// The length of <T> field must be equal to <X>
        /// </summary>
        DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO,
        
        /// <summary>
        /// The field <T> must be a number with up to <X> digits and <Y> decimals
        /// </summary>
        DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS,

        /// <summary>
        /// No records found
        /// </summary>
        DEFAULT_MESSAGES_NO_RECORDS_FOUND
    }

    public static class EResourcesDefaultMessagesExtensions
    {
        public static string GetResourceMessage(this EResourcesDefaultMessages erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesDefaultMessages error)
        {
            switch (error)
            {
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR:
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST:
                    return "500";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM:
                    return "80001";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN:
                    return "80002";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM:
                    return "80003";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE:
                    return "80004";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_REQUIRED_TO_FOLLOW_THE_RULE:
                    return "80005";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY:
                    return "80006";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED:
                    return "80007";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED:
                    return "80008";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED:
                    return "80009";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_PRECONDITION_FAILED:
                    return "80010";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST:
                    return "80011";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED:
                    return "80012";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN:
                    return "80013";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_AT_LEAST_AND_AT_MOST_ELEMENTS:
                    return "80014";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_SUM_OF_FIELD:
                    return "80015";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO:
                    return "80016";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS:
                    return "80017";
                case EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND:
                    return "80018";
                default:
                    return "";
            }
        }
    }
}

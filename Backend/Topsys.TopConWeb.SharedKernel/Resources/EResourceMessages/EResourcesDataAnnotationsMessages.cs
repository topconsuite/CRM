using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de mensagens do DataAnnotations.
    /// </summary>
    public enum EResourcesDataAnnotationsMessages
    {
        /// <summary>
        /// Field '{0}' cannot be empty
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_REQUIRED,

        /// <summary>
        /// The field '{0}' is mandatory if it informs the field '{1}'
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY,

        /// <summary>
        /// The field '{0}' is mandatory if it informs the field '{1}' equal to '{2}'
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_EQUAL,

        /// <summary>
        /// The value of the field '{0}' must be between {1} and {2}
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_RANGE,

        /// <summary>
        /// The field '{0}' must have a maximum of {1} characters
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH,

        /// <summary>
        /// The field '{0}' must have a minimum of {1} characters and a maximum of {2} characters
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH_WITH_MINIMUM,

        /// <summary>
        /// The key formed by '{0}' is duplicated in the list
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_UNIQUE_COMBINATIONS,

        /// <summary>
        /// Duplicate field(s) '{0}' in the list
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS,

        /// <summary>
        /// The list cannot have more than {0} elements
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_MAX_LIST_LENGTH,

        /// <summary>
        ///  License plate vehicle invalid
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_LICENSE_PLATE_VALIDATION,

        /// <summary>
        ///  Value is not a valid option for field '{0}'
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_ENUM_DATA_TYPE,

        /// <summary>
        ///  For the field '{0}' it must be one of the options: {1}
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS,

        /// <summary>
        /// The '{0}' field format must be 'HH:mm'
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT,

        /// <summary>
        /// The field '{0}' must be less than field '{1}'
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_TIME_LESS_THAN,

        /// <summary>
        /// The field '{0}' must be a multiple of {1}
        /// </summary>
        DATA_ANNOTATIONS_MESSAGES_MULTIPLE_OF
    }

    public static class EResourcesDataAnnotationsMessagesExtensions
    {
        public static string GetResourceMessage(this EResourcesDataAnnotationsMessages erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesDataAnnotationsMessages error)
        {
            return "";
        }
    }
}

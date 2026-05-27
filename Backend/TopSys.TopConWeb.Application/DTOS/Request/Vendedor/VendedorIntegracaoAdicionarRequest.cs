using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Vendedor
{
    public class VendedorIntegracaoAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::active")]
        [JsonProperty(PropertyName = "active")]
        public bool Ativo { get; set; } = true;

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address::40")]
        [JsonProperty(PropertyName = "address")]
        public string EnderecoLogradouro { get; set; } = "";

        [JsonProperty(PropertyName = "cell_phone")]
        public string Celular { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::company_name")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH_WITH_MINIMUM) + "::company_name::1::40")]
        [JsonProperty(PropertyName = "company_name")]
        public string RazaoSocial { get; set; }

        [StringLength(10, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::complement::10")]
        [JsonProperty(PropertyName = "complement")]
        public string Complemento { get; set; } = "";

        [JsonProperty(PropertyName = "county")]
        public int Municipio { get; set; }

        [JsonProperty(PropertyName = "ddd_cell_phone")]
        public int DDDCelular { get; set; }

        [StringLength(50, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::email::50")]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = "";

        [JsonProperty(PropertyName = "godfather_seller")]
        public int VendedorPadrinho { get; set; }

        [JsonProperty(PropertyName = "intervener")]
        public int Interveniente { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::name")]
        [StringLength(15, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::name::15")]
        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "payment_terms")]
        public int CondicaoPagamento { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::role")]
        [RegularExpression("^(R|V)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::role::R ⇒ Representative or V ⇒ Saller")]
        [JsonProperty(PropertyName = "role")]
        public string Funcao { get; set; }

        [JsonProperty(PropertyName = "re")]
        public int Re { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::zip_code::8")]
        [JsonProperty(PropertyName = "zip_code")]
        public string Cep { get; set; } = "";

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::system_user")]
        [StringLength(10, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::system_user::10")]
        [JsonProperty(PropertyName = "system_user")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::external_id")]
        [StringLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::external_id::100")]
        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }
    }
}

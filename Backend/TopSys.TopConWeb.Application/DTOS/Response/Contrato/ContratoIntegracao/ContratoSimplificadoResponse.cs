using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao
{
    public class ContratoSimplificadoResponse
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::id_intervener")]
        [JsonProperty(PropertyName = "id_intervener")]
        public int IntervenienteCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::concrete_batching_plant_contract")]
        [JsonProperty(PropertyName = "concrete_batching_plant_contract")]
        public int Usina { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_year")]
        [JsonProperty(PropertyName = "contract_year")]
        public int Ano { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_number")]
        [JsonProperty(PropertyName = "contract_number")]
        public int Numero { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "contract_date")]
        public DateTime DataContrato { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::previous_contract_number")]
        [JsonProperty(PropertyName = "previous_contract_number")]
        public string NumeroContratoAnterior { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_zip_code::8")]
        [JsonProperty(PropertyName = "address_construction_site_zip_code")]
        public string EnderecoCep { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_street::40")]
        [JsonProperty(PropertyName = "address_construction_site_street")]
        public string EnderecoLogradouro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_number")]
        public int EnderecoNumero { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_complement::30")]
        [JsonProperty(PropertyName = "address_construction_site_complement")]
        public string EnderecoComplemento { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_district::20")]
        [JsonProperty(PropertyName = "address_construction_site_district")]
        public string EnderecoBairro { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::address_construction_site_city_code")]
        [JsonProperty(PropertyName = "address_construction_site_city_code")]
        public int? EnderecoMunicipioCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::address_construction_site_state")]
        [JsonProperty(PropertyName = "address_construction_site_state")]
        public string EnderecoUf { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::address_construction_site_city_code")]
        [JsonProperty(PropertyName = "address_construction_site_city")]
        public string EnderecoMunicipio { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_purpose")]
        [JsonProperty(PropertyName = "contract_purpose")]
        public int ContratoFinalidade { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::seller")]
        [JsonProperty(PropertyName = "seller")]
        public int VendedorCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::seller_external_id")]
        [JsonProperty(PropertyName = "seller_external_id")]
        public string VendedorExternalId { get; set; }

        [JsonProperty(PropertyName = "contract_payments")]
        public IEnumerable<ContratoPagamentoSimplificadoResponse> Pagamentos { get; set; }

    }
}

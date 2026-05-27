using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao
{
    public class ProgramacaoAtualizarRequest
    {
        [JsonProperty(PropertyName = "address_construction_site_city")]
        public int? EnderecoMunicipioCodigo { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_complement::30")]
        [JsonProperty(PropertyName = "address_construction_site_complement")]
        public string EnderecoComplemento { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_district::20")]
        [JsonProperty(PropertyName = "address_construction_site_district")]
        public string EnderecoBairro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_number")]
        public int? EnderecoNumero { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_street::40")]
        [JsonProperty(PropertyName = "address_construction_site_street")]
        public string EnderecoLogradouro { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_zip_code::8")]
        [JsonProperty(PropertyName = "address_construction_site_zip_code")]
        public string EnderecoCep { get; set; }

        [JsonProperty(PropertyName = "amount_of_concrete_test_specimen")]
        public int? CorpoDeProvaQuantidade { get; set; }

        [Range(0, 2, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::concrete_test_specimen_molder::0 ⇒ None 1 ⇒ Driver 2 ⇒ Laboratory")]
        [JsonProperty(PropertyName = "concrete_test_specimen_molder")]
        public int? CorpoDeProvaMoldador { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::concrete_test_specimen_type::8")]
        [JsonProperty(PropertyName = "concrete_test_specimen_type")]
        public string CorpoDeProvaTipo { get; set; }

        [JsonProperty(PropertyName = "concreting_date")]
        public DateTime? DataConcretagem { get; set; }

        [EnumDataType(typeof(EProgramacaoStatus), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::concreting_status::9161 ⇒ Awaiting Confirmation 9162 ⇒ Scheduled 9163 ⇒ Rejected 9164 ⇒ Canceled 9165 - Revalidation 9166 ⇒ Awaiting Credit Limit Analysis 9167 ⇒ Insufficient Credit Limit")]
        [JsonProperty(PropertyName = "concreting_status")]
        public EProgramacaoStatus? Status { get; set; }

        [RegularExpression("^(S|N|P)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::confirm_delivery::N =false S=true P=Partial")]
        [StringLength(1, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::confirm_delivery::1")]
        [JsonProperty(PropertyName = "confirm_delivery")]
        public string NecessitaConfirmacao { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::construction_site_name::30")]
        [JsonProperty(PropertyName = "construction_site_name")]
        public string ObraNome { get; set; }

        [JsonProperty(PropertyName = "construction_site_number")]
        public int? ObraNumero { get; set; }

        [JsonProperty(PropertyName = "consumption")]
        public int? Consumo { get; set; }

        [JsonProperty(PropertyName = "contract_item_sequence")]
        public int? ObraTracoSequencia { get; set; }

        [JsonProperty(PropertyName = "delivery_concrete_batching_plant")]
        public int? UsinaEntregaCodigo { get; set; }

        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::delivery_time::5")]
        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::delivery_time")]
        [JsonProperty(PropertyName = "delivery_time")]
        public string Horario { get; set; }

        [JsonProperty(PropertyName = "fck")]
        public float? Mpa { get; set; }

        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::floor_number::5")]
        [JsonProperty(PropertyName = "floor_number")]
        public string Andar { get; set; }

        [JsonProperty(PropertyName = "gravel")]
        public int? PedraCodigo { get; set; }

        [MultipleOf(15, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MULTIPLE_OF) + "::interval_between_loads::15")]
        [JsonProperty(PropertyName = "interval_between_loads")]
        public int? IntervaloEmMinutosEntreCargas { get; set; }

        [JsonProperty(PropertyName = "interval_in_m3_per_concrete_test_specimen")]
        public int? CorpoDeProvaIntervalo { get; set; }

        [JsonProperty(PropertyName = "m3_quantity_per_concrete_mixer_load")]
        public float? VolumePorCarga { get; set; }

        [StringLength(90, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::observation::90")]
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pieces_to_be_concreted::20")]
        [JsonProperty(PropertyName = "pieces_to_be_concreted")]
        public string PecaConcretar { get; set; }

        [JsonProperty(PropertyName = "product_group")]
        public int? UsoCodigo { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pump_code::8")]
        [JsonProperty(PropertyName = "pump_code")]
        public string EquipamentoBombaCodigo { get; set; }

        [JsonProperty(PropertyName = "pump_piping_distance")]
        public int? DistanciaTubulacao { get; set; }

        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::pump_time")]
        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pump_time::5")]
        [JsonProperty(PropertyName = "pump_time")]
        public string HorarioBomba { get; set; }

        [JsonProperty(PropertyName = "pump_type")]
        public int? ObraBombaSequencia { get; set; }

        [JsonProperty(PropertyName = "quantity_delivered")]
        public float? VolumeEntregue { get; set; }

        [JsonProperty(PropertyName = "quantity_of_m3")]
        public float? VolumeTotal { get; set; }

        [JsonProperty(PropertyName = "quantity_released")]
        public float? VolumeLiberado { get; set; }

        [JsonProperty(PropertyName = "remote_molding_of_the_concrete_test_specimen")]
        public string CorpoDeProvaMoldagemRemota { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::requester::30")]
        [JsonProperty(PropertyName = "requester")]
        public string Solicitante { get; set; }

        [JsonProperty(PropertyName = "resistance_type")]
        public int? ResistenciaTipoCodigo { get; set; }

        [JsonProperty(PropertyName = "slump")]
        public int? SlumpCodigo { get; set; }

        [JsonProperty(PropertyName = "slump_invoice")]
        public string SlumpNotaFiscal { get; set; }
    }
}

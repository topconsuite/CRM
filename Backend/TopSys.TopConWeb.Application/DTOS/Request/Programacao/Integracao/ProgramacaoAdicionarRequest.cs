using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao
{
    public class ProgramacaoAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::address_construction_site_city")]
        [JsonProperty(PropertyName = "address_construction_site_city")]
        public int EnderecoMunicipioCodigo { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_complement::30")]
        [JsonProperty(PropertyName = "address_construction_site_complement")]
        public string EnderecoComplemento { get; set; } = "";

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_district::20")]
        [JsonProperty(PropertyName = "address_construction_site_district")]
        public string EnderecoBairro { get; set; } = "";

        [JsonProperty(PropertyName = "address_construction_site_number")]
        public int EnderecoNumero { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_street::40")]
        [JsonProperty(PropertyName = "address_construction_site_street")]
        public string EnderecoLogradouro { get; set; } = "";

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::address_construction_site_zip_code::8")]
        [JsonProperty(PropertyName = "address_construction_site_zip_code")]
        public string EnderecoCep { get; set; }

        [JsonProperty(PropertyName = "amount_of_concrete_test_specimen")]
        public int CorpoDeProvaQuantidade { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::concrete_batching_plant_contract")]
        [JsonProperty(PropertyName = "concrete_batching_plant_contract")]
        public int UsinaCodigo { get; set; }

        [Range(0, 2, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::concrete_test_specimen_molder::0 ⇒ None 1 ⇒ Driver 2 ⇒ Laboratory")]
        [JsonProperty(PropertyName = "concrete_test_specimen_molder")]
        public int CorpoDeProvaMoldador { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::concrete_test_specimen_type::8")]
        [JsonProperty(PropertyName = "concrete_test_specimen_type")]
        public string CorpoDeProvaTipo { get; set; } = "";

        [JsonProperty(PropertyName = "concreting_date")]
        public DateTime DataConcretagem { get; set; }

        [JsonProperty(PropertyName = "concreting_sequence")]
        public int Sequencia { get; set; }

        [EnumDataType(typeof(EProgramacaoStatus), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::concreting_status::9161 ⇒ Awaiting Confirmation 9162 ⇒ Scheduled 9163 ⇒ Rejected 9164 ⇒ Canceled 9165 - Revalidation 9166 ⇒ Awaiting Credit Limit Analysis 9167 ⇒ Insufficient Credit Limit")]
        [JsonProperty(PropertyName = "concreting_status")]
        public EProgramacaoStatus Status { get; set; }

        [RegularExpression("^(S|N|P)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::confirm_delivery::N =false S=true P=Partial")]
        [StringLength(1, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::confirm_delivery::1")]
        [JsonProperty(PropertyName = "confirm_delivery")]
        public string NecessitaConfirmacao { get; set; } = "";

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::construction_site_name")]
        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::construction_site_name::30")]
        [JsonProperty(PropertyName = "construction_site_name")]
        public string ObraNome { get; set; } = "";

        [JsonProperty(PropertyName = "construction_site_number")]
        public int ObraNumero { get; set; }

        [JsonProperty(PropertyName = "consumption")]
        public int Consumo { get; set; }

        [JsonProperty(PropertyName = "contract_item_sequence")]
        public int ObraTracoSequencia { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_number")]
        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_year")]
        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::delivery_concrete_batching_plant")]
        [JsonProperty(PropertyName = "delivery_concrete_batching_plant")]
        public int UsinaEntregaCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::delivery_time")]
        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::delivery_time::5")]
        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::delivery_time")]
        [JsonProperty(PropertyName = "delivery_time")]
        public string Horario { get; set; }

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::fck::contract_item_sequence")]
        [JsonProperty(PropertyName = "fck")]
        public float Mpa { get; set; }

        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::floor_number::5")]
        [JsonProperty(PropertyName = "floor_number")]
        public string Andar { get; set; } = "";

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::gravel::contract_item_sequence")]
        [JsonProperty(PropertyName = "gravel")]
        public int PedraCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::interval_between_loads")]
        [MultipleOf(15, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MULTIPLE_OF) + "::interval_between_loads::15")]
        [JsonProperty(PropertyName = "interval_between_loads")]
        public int IntervaloEmMinutosEntreCargas { get; set; }

        [JsonProperty(PropertyName = "interval_in_m3_per_concrete_test_specimen")]
        public int CorpoDeProvaIntervalo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::m3_quantity_per_concrete_mixer_load")]
        [JsonProperty(PropertyName = "m3_quantity_per_concrete_mixer_load")]
        public float VolumePorCarga { get; set; }

        [StringLength(90, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::observation::90")]
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; } = "";

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::pieces_to_be_concreted")]
        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pieces_to_be_concreted::20")]
        [JsonProperty(PropertyName = "pieces_to_be_concreted")]
        public string PecaConcretar { get; set; } = "";

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::product_group::contract_item_sequence")]
        [JsonProperty(PropertyName = "product_group")]
        public int UsoCodigo { get; set; }

        [RequiredIfNotEmpty(nameof(ObraBombaSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::pump_code::pump_type")]
        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pump_code::8")]
        [JsonProperty(PropertyName = "pump_code")]
        public string EquipamentoBombaCodigo { get; set; } = "";

        [RequiredIfNotEmpty(nameof(ObraBombaSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::pump_piping_distance::pump_type")]
        [JsonProperty(PropertyName = "pump_piping_distance")]
        public int DistanciaTubulacao { get; set; }

        [RequiredIfNotEmpty(nameof(ObraBombaSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::pump_time::pump_type")]
        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::pump_time")]
        [StringLength(5, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::pump_time::5")]
        [TimeLessThan(nameof(Horario), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_TIME_LESS_THAN) + "::pump_time::delivery_time")]
        [JsonProperty(PropertyName = "pump_time")]
        public string HorarioBomba { get; set; } = "";

        [JsonProperty(PropertyName = "pump_type")]
        public int ObraBombaSequencia { get; set; }

        [JsonProperty(PropertyName = "quantity_delivered")]
        public float VolumeEntregue { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::quantity_of_m3")]
        [JsonProperty(PropertyName = "quantity_of_m3")]
        public float VolumeTotal { get; set; }

        [RequiredIfEqual(nameof(NecessitaConfirmacao), "P", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_EQUAL) + "::quantity_released::confirm_delivery::P")]
        [JsonProperty(PropertyName = "quantity_released")]
        public float VolumeLiberado { get; set; }

        [RegularExpression("^(Na Obra|Na Usina|Padrão da Central)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::remote_molding_of_the_concrete_test_specimen::'Na Obra', 'Na Usina' or 'Padrão da Central'")]
        [JsonProperty(PropertyName = "remote_molding_of_the_concrete_test_specimen")]
        public string CorpoDeProvaMoldagemRemota { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::requester")]
        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::requester::30")]
        [JsonProperty(PropertyName = "requester")]
        public string Solicitante { get; set; } = "";

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::resistance_type::contract_item_sequence")]
        [JsonProperty(PropertyName = "resistance_type")]
        public int ResistenciaTipoCodigo { get; set; }

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::slump::contract_item_sequence")]
        [JsonProperty(PropertyName = "slump")]
        public int SlumpCodigo { get; set; }

        [RequiredIfNotEmpty(nameof(ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED_IF_NOT_EMPTY) + "::slump_invoice::contract_item_sequence")]
        [JsonProperty(PropertyName = "slump_invoice")]
        public string SlumpNotaFiscal { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::external_id")]
        [StringLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::external_id::100")]
        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }
    }
}

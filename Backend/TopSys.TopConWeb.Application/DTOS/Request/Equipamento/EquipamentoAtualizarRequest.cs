using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Equipamento
{
    [AtLeastOnePropertyRequired]
    public class EquipamentoAtualizarRequest
    {
        [JsonProperty(PropertyName = "year_manufacture")]
        public int? Ano { get; set; }

        [JsonProperty(PropertyName = "year_model")]
        public int? AnoModelo { get; set; }

        [JsonProperty(PropertyName = "pump")]
        public bool? Bomba { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "cylinder_capacity::20")]
        [JsonProperty(PropertyName = "cylinder_capacity")]
        public string CapacidadePotCilindros { get; set; }

        [JsonProperty(PropertyName = "fuel_tank_liter_capacity")]
        public int? CapacidadeLitrosCombustivel { get; set; }

        [JsonProperty(PropertyName = "m3_capacity")]
        public int? CapacidadeM3 { get; set; }

        [JsonProperty(PropertyName = "hour_meter_control")]
        public bool? ControlaHorimetro { get; set; }

        [JsonProperty(PropertyName = "km_control")]
        public bool? ControlaKm { get; set; }

        [JsonProperty(PropertyName = "occurrence_date")]
        public DateTime? DataOcorrencia { get; set; }

        [JsonProperty(PropertyName = "hour_meter_replacement_date")]
        public DateTime? DataSubstituicaoHorimetro { get; set; }

        [JsonProperty(PropertyName = "tachograph_replacement_date")]
        public DateTime? DataSubstituicaoTacografo { get; set; }

        [StringLength(60, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::description::60")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "has_gps")]
        public bool? PossuiGps { get; set; }

        [JsonProperty(PropertyName = "active_gps")]
        public bool? Gps { get; set; }

        [JsonProperty(PropertyName = "gps_installation_date")]
        public DateTime? DataInstalacaoGps { get; set; }

        [StringLength(10, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::ipva_range::10")]
        [JsonProperty(PropertyName = "ipva_range")]
        public string FaixaIpva { get; set; }

        [Range(0, 99999999.9, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::initial_hour_meter::0::99999999.9")]
        [JsonProperty(PropertyName = "initial_hour_meter")]
        public double? HorimetroInicial { get; set; }

        [JsonProperty(PropertyName = "broken_hour_meter")]
        public bool? HorimetroQuebrado { get; set; }

        [JsonProperty(PropertyName = "initial_km")]
        public int? KmInicial { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::brand::40")]
        [JsonProperty(PropertyName = "brand")]
        public string Marca { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::model::40")]
        [JsonProperty(PropertyName = "model")]
        public string Modelo { get; set; }

        [JsonProperty(PropertyName = "maximum_km_allowed")]
        public int? MaxKmAbastecimento { get; set; }

        [Range(0, 9999999.999, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::maximum_liters_allowed_supply::0::9999999.999")]
        [JsonProperty(PropertyName = "maximum_liters_allowed_supply")]
        public double? MaxLitrosAbastecimento { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::chassis_number::40")]
        [JsonProperty(PropertyName = "chassis_number")]
        public string Chassis { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::process_number::20")]
        [JsonProperty(PropertyName = "process_number")]
        public string NumeroProcesso { get; set; }

        [StringLength(50, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::observation::50")]
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }

        [Range(0, 99999.999, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::weight_equipment::0::99999.999")]
        [JsonProperty(PropertyName = "weight_equipment")]
        public double? PesoEquipamento { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::license_plate::8")]
        [LicensePlateValidation(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_LICENSE_PLATE_VALIDATION))]
        [JsonProperty(PropertyName = "license_plate")]
        public string Placa { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::renavam::20")]
        [JsonProperty(PropertyName = "renavam")]
        public string Renavam { get; set; }

        [StringLength(8, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::rntrc::8")]
        [JsonProperty(PropertyName = "rntrc")]
        public string RNTRC { get; set; }

        [EnumDataType(typeof(EEquipamentoStatus), ErrorMessage = "Value is not a valid option for status field.")]
        [JsonProperty(PropertyName = "status")]
        public EEquipamentoStatus? Status { get; set; }

        [JsonProperty(PropertyName = "broken_tachograph")]
        public bool? TacografoQuebrado { get; set; }

        [Range(1, 9999, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::type::1::9999")]
        [JsonProperty(PropertyName = "type")]
        public int? Tipo { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::state::2")]
        [JsonProperty(PropertyName = "state")]
        public string UF { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::concrete_batching_plant::0::999")]
        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int? UsinaAlocada { get; set; }

        [JsonProperty(PropertyName = "structure_group_1")]
        public string Grupo { get; set; }

        [JsonProperty(PropertyName = "structure_item_1")]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "subgroup_structure_1")]
        public string SubGrupo { get; set; }

        [JsonProperty(PropertyName = "cost_center")]
        public string CC { get; set; }

        [JsonProperty(PropertyName = "allocated_employee")]
        public string FuncionarioAlocado { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public string Proprietario { get; set; }

        [RegularExpression("^(0|1|2|3|4|5)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::body_type::0 ⇒ Not applicable 1 ⇒ Open 2 ⇒ Closed / Trunk 3 ⇒ Bulk carrier 4 ⇒ Container door 5 ⇒ Sider.")]
        [JsonProperty(PropertyName = "body_type")]
        public string TipoCarroceria { get; set; }

        [RegularExpression("^(0|1|2|3|4|5|6)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::wheelset_type::1 ⇒ Truck 2 ⇒ Stump 3 ⇒ Mechnical horse 4 ⇒ VAN 5 ⇒ Utility 6 ⇒ Others.")]
        [JsonProperty(PropertyName = "wheelset_type")]
        public string TipoRodado { get; set; }

        [RegularExpression("^(0|1|2)$", ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_FIELD_WITH_OPTIONS) + "::owner_type::0 ⇒ TAC Aggregate 1 ⇒ TAC Independent 2 ⇒ Others.")]
        [JsonProperty(PropertyName = "owner_type")]
        public string TipoProprietario { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Equipamento
{
    public class EquipamentoResponse
    {
        [JsonProperty(PropertyName = "year_manufacture")] public int Ano { get; set; }
        [JsonProperty(PropertyName = "year_model")] public int AnoModelo { get; set; }
        [JsonProperty(PropertyName = "pump")] public string Bomba { get; set; }
        [JsonProperty(PropertyName = "cylinder_capacity")] public string CapacidadePotCilindros { get; set; }
        [JsonProperty(PropertyName = "fuel_tank_liter_capacity")] public int CapacidadeLitrosCombustivel { get; set; }
        [JsonProperty(PropertyName = "m3_capacity")] public int CapacidadeM3 { get; set; }
        [JsonProperty(PropertyName = "hour_meter_control")] public bool ControlaHorimetro { get; set; }
        [JsonProperty(PropertyName = "km_control")] public bool ControlaKm { get; set; }
        [JsonProperty(PropertyName = "code")] public string Codigo { get; set; }
        [JsonProperty(PropertyName = "occurrence_date")] public DateTime DataOcorrencia { get; set; }
        [JsonProperty(PropertyName = "hour_meter_replacement_date")] public DateTime DataSubstituicaoHorimetro { get; set; }
        [JsonProperty(PropertyName = "tachograph_replacement_date")] public DateTime DataSubstituicaoTacografo { get; set; }
        [JsonProperty(PropertyName = "description")] public string Descricao { get; set; }
        [JsonProperty(PropertyName = "gps_installation_date")] public DateTime DataInstalacaoGps { get; set; }
        [JsonProperty(PropertyName = "structure_group_1")] public string Grupo { get; set; }
        [JsonProperty(PropertyName = "structure_item_1")] public string Item { get; set; }
        [JsonProperty(PropertyName = "subgroup_structure_1")] public string SubGrupo { get; set; }
        [JsonProperty(PropertyName = "external_id")] public string ExternalID { get; set; }
        [JsonProperty(PropertyName = "ipva_range")] public string FaixaIpva { get; set; }
        [JsonProperty(PropertyName = "cost_center")] public string CC { get; set; }
        [JsonProperty(PropertyName = "end_license_plate")] public string FinalPlaca { get; set; }
        [JsonProperty(PropertyName = "allocated_employee")] public string FuncionarioAlocado { get; set; }
        [JsonProperty(PropertyName = "active_gps")] public string Gps { get; set; }
        [JsonProperty(PropertyName = "initial_hour_meter")] public double HorimetroInicial { get; set; }
        [JsonProperty(PropertyName = "broken_hour_meter")] public string HorimetroQuebrado { get; set; }
        [JsonProperty(PropertyName = "initial_km")] public int KmInicial { get; set; }
        [JsonProperty(PropertyName = "brand")] public string Marca { get; set; }
        [JsonProperty(PropertyName = "model")] public string Modelo { get; set; }
        [JsonProperty(PropertyName = "maximum_km_allowed")] public int MaxKmAbastecimento { get; set; }
        [JsonProperty(PropertyName = "maximum_liters_allowed_supply")] public double MaxLitrosAbastecimento { get; set; }
        [JsonProperty(PropertyName = "chassis_number")] public string Chassis { get; set; }
        [JsonProperty(PropertyName = "process_number")] public string NumeroProcesso { get; set; }
        [JsonProperty(PropertyName = "observation")] public string Observacao { get; set; }
        [JsonProperty(PropertyName = "weight_equipment")] public double PesoEquipamento { get; set; }
        [JsonProperty(PropertyName = "license_plate")] public string Placa { get; set; }
        [JsonProperty(PropertyName = "has_gps")] public string PossuiGps { get; set; }
        [JsonProperty(PropertyName = "owner")] public string Proprietario { get; set; }
        [JsonProperty(PropertyName = "renavam")] public string Renavam { get; set; }
        [JsonProperty(PropertyName = "rntrc")] public string RNTRC { get; set; }
        [JsonProperty(PropertyName = "status")] public EEquipamentoStatus Status { get; set; }
        [JsonProperty(PropertyName = "broken_tachograph")] public string TacografoQuebrado { get; set; }
        [JsonProperty(PropertyName = "type")] public string Tipo { get; set; }
        [JsonProperty(PropertyName = "body_type")] public string TipoCarroceria { get; set; }
        [JsonProperty(PropertyName = "wheelset_type")] public string TipoRodado { get; set; }
        [JsonProperty(PropertyName = "owner_type")] public string TipoProprietario { get; set; }
        [JsonProperty(PropertyName = "state")] public string UF { get; set; }
        [JsonProperty(PropertyName = "concrete_batching_plant")] public string UsinaAlocada { get; set; }


    }
}

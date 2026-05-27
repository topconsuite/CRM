using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoIntegracao
{
    public class ProgramacaoResponse
    {
        [JsonProperty(PropertyName = "address_construction_site_city")]
        public string EnderecoMunicipioCodigo { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_complement")]
        public string EnderecoComplemento { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_district")]
        public string EnderecoBairro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_number")]
        public int EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_street")]
        public string EnderecoLogradouro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_zip_code")]
        public string EnderecoCep { get; set; }

        [JsonProperty(PropertyName = "amount_of_concrete_test_specimen")]
        public int CorpoDeProvaQuantidade { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant_contract")]
        public int UsinaCodigo { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_molder")]
        public int CorpoDeProvaMoldador { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_type")]
        public string CorpoDeProvaTipo { get; set; }

        [JsonProperty(PropertyName = "concrete_value")]
        public float ValorConcreto { get; set; }

        [JsonProperty(PropertyName = "concreting_date")]
        public DateTime DataConcretagem { get; set; }

        [JsonProperty(PropertyName = "concreting_sequence")]
        public int Sequencia { get; set; }

        [JsonProperty(PropertyName = "concreting_status")]
        public EProgramacaoStatus Status { get; set; }

        [JsonProperty(PropertyName = "confirm_delivery")]
        public string NecessitaConfirmacao { get; set; }

        [JsonProperty(PropertyName = "construction_site_name")]
        public string ObraNome { get; set; }

        [JsonProperty(PropertyName = "construction_site_number")]
        public int ObraNumero { get; set; }

        [JsonProperty(PropertyName = "consumption")]
        public int Consumo { get; set; }

        [JsonProperty(PropertyName = "contract_item_sequence")]
        public int ObraTracoSequencia { get; set; }

        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [JsonProperty(PropertyName = "date_time_request")]
        public DateTime DataHoraSolicitacao { get; set; }

        [JsonProperty(PropertyName = "delivery_concrete_batching_plant")]
        public int UsinaEntregaCodigo { get; set; }

        [JsonProperty(PropertyName = "delivery_time")]
        public string Horario { get; set; }

        [JsonProperty(PropertyName = "extra_fees_value")]
        public float ValorExtras { get; set; }

        [JsonProperty(PropertyName = "fck")]
        public float Mpa { get; set; }

        [JsonProperty(PropertyName = "floor_number")]
        public string Andar { get; set; }

        [JsonProperty(PropertyName = "gravel")]
        public int PedraCodigo { get; set; }

        [JsonProperty(PropertyName = "interval_between_loads")]
        public int IntervaloEmMinutosEntreCargas { get; set; }

        [JsonProperty(PropertyName = "interval_in_m3_per_concrete_test_specimen")]
        public int CorpoDeProvaIntervalo { get; set; }

        [JsonProperty(PropertyName = "m3_quantity_per_concrete_mixer_load")]
        public float VolumePorCarga { get; set; }

        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }

        [JsonProperty(PropertyName = "pieces_to_be_concreted")]
        public string PecaConcretar { get; set; }

        [JsonProperty(PropertyName = "product_group")]
        public int UsoCodigo { get; set; }

        [JsonProperty(PropertyName = "programming_value")]
        public float ValorTotal { get; set; }

        [JsonProperty(PropertyName = "pump_code")]
        public string EquipamentoBombaCodigo { get; set; }

        [JsonProperty(PropertyName = "pump_piping_distance")]
        public int DistanciaTubulacao { get; set; }

        [JsonProperty(PropertyName = "pump_time")]
        public string HorarioBomba { get; set; }

        [JsonProperty(PropertyName = "pump_type")]
        public int ObraBombaSequencia { get; set; }

        [JsonProperty(PropertyName = "pump_value")]
        public float ValorBomba { get; set; }

        [JsonProperty(PropertyName = "quantity_delivered")]
        public float VolumeEntregue { get; set; }

        [JsonProperty(PropertyName = "quantity_of_m3")]
        public float VolumeTotal { get; set; }

        [JsonProperty(PropertyName = "quantity_released")]
        public float VolumeLiberado { get; set; }

        [JsonProperty(PropertyName = "registration_id")]
        public string IdCadastro { get; set; }

        [JsonProperty(PropertyName = "remote_molding_of_the_concrete_test_specimen")]
        public string CorpoDeProvaMoldagemRemota { get; set; }

        [JsonProperty(PropertyName = "requester")]
        public string Solicitante { get; set; }

        [JsonProperty(PropertyName = "resistance_type")]
        public int ResistenciaTipoCodigo { get; set; }

        [JsonProperty(PropertyName = "slump")]
        public int SlumpCodigo { get; set; }

        [JsonProperty(PropertyName = "slump_invoice")]
        public string SlumpNotaFiscal { get; set; }

        [JsonProperty(PropertyName = "vibrator_value")]
        public int VibradorValorTotal { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_consumption")]
        public int TracoPesadoConsumo { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_fck")]
        public float TracoPesadoMpa { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_gravel")]
        public int TracoPesadoPedraCodigo { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_product_group")]
        public int TracoPesadoUsoCodigo { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_slump")]
        public int TracoPesadoSlumpCodigo { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_strength_type")]
        public int TracoPesadoResistenciaTipoCodigo { get; set; }

        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "product_numbering")]
        public int NumeracaoProduto { get; set; }

        [JsonProperty(PropertyName = "previous_contract_number")]
        public string NumeroContratoAnterior { get; set; }

        [JsonProperty(PropertyName = "canceled_by")]
        public string CanceladoPorString { get; set; } = "";
    }
}

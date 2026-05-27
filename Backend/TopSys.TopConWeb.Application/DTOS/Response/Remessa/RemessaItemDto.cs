using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Remessa
{
    public class RemessaItemDto
    {
        [JsonProperty(PropertyName = "cfop")]
        public int Cfop { get; set; }

        [JsonProperty(PropertyName = "total_cost")]
        public double CustoTotal { get; set; }

        [JsonProperty(PropertyName = "date_time_last_update")]
        public DateTime? DataHoraUltimaAtualizacao { get; set; } 

        [JsonProperty(PropertyName = "operation_date")]
        public DateTime? DataOperacao { get; set; }

        [JsonProperty(PropertyName = "branch")]
        public int FilialCodigo { get; set; }

        [JsonProperty(PropertyName = "update_id")]
        public string IdAtual { get; set; }

        [JsonProperty(PropertyName = "registration_id")]
        public string IdCadastro { get; set; }

        [JsonProperty(PropertyName = "inventory_intervener")]
        public int IntervenienteEstoque { get; set; }

        [JsonProperty(PropertyName = "intervener")]
        public int IntervenienteCodigo { get; set; }

        [JsonProperty(PropertyName = "inventory_location")]
        public string LocalEstoque { get; set; }

        [JsonProperty(PropertyName = "input_supply")]
        public int LocalInsumo { get; set; }

        [JsonProperty(PropertyName = "merchandise")]
        public string MercadoriaCodigo
        {

            get { return _mercadoria; }
            set => _mercadoria = value.Replace("M", "");

        }

        private string _mercadoria { get; set; }
        
        [JsonProperty(PropertyName = "merchandise_external_id")]
        public string IdExternoMercadoria { get; set; }

        [JsonProperty(PropertyName = "packing_list_invoice_number")]
        public int Numero { get; set; }

        [JsonProperty(PropertyName = "packing_list_invoice_sequence_item")]
        public int SequenciaItem { get; set; }

        [JsonProperty(PropertyName = "adjustment_percentage")]
        public double PercentualAjuste { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public double Peso { get; set; }

        [JsonProperty(PropertyName = "unit_price")]
        public double PrecoUnitario { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public double Quantidade { get; set; }

        [JsonProperty(PropertyName = "commission_amount")]
        public double QuantidadeComissao { get; set; }

        [JsonProperty(PropertyName = "inventory_quantity")]
        public double QuantidadeEstoque { get; set; }

        [JsonProperty(PropertyName = "theoretical_quantity")]
        public double QuantidadeTeorica { get; set; }

        [JsonProperty(PropertyName = "cfop_sequence")]
        public int SequenciaCfop { get; set; }

        [JsonProperty(PropertyName = "packing_list_invoice_sequence")]
        public int Sequencia { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumentoCodigo { get; set; }

        [JsonProperty(PropertyName = "inventory_type")]
        public int TipoEstoque { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe")]
        public string TracoConcreto { get; set; }

        [JsonProperty(PropertyName = "transaction")]
        public int Transacao { get; set; }

        [JsonProperty(PropertyName = "moisture")]
        public float? Umidade { get; set; }

        [JsonProperty(PropertyName = "discount_value")]
        public double ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "shipping_value")]
        public double ValorFrete { get; set; }

        [JsonProperty(PropertyName = "value_other_expenses")]
        public double ValorOutrasDespesas { get; set; }

        [JsonProperty(PropertyName = "safe_value")]
        public double ValorSeguro { get; set; }

        [JsonProperty(PropertyName = "total_value")]
        public double ValorTotal { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public double Volume { get; set; }
    }
}

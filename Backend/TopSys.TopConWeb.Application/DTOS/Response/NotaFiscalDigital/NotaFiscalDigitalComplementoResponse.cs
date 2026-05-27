using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalComplementoResponse
    {
        [JsonProperty(PropertyName = "presence_indicator")]
        public int? IndicadorPresenca { get; set; }

        [JsonProperty(PropertyName = "payment_method")]
        public int? MeioPagamento { get; set; }

        [JsonProperty(PropertyName = "transporter_plate")]
        public string PlacaTransportador { get; set; }

        [JsonProperty(PropertyName = "transaction_sequence")]
        public int SequenciaTransacao { get; set; }

        [JsonProperty(PropertyName = "government_reduction_percentage")]
        public decimal PercentualReducaoGoverno { get; set; }

        [JsonProperty(PropertyName = "government_entity_type")]
        public int TipoEntidadeGoverno { get; set; }

        [JsonProperty(PropertyName = "government_operation_type")]
        public int TipoOperacaoGoverno { get; set; }

        [JsonProperty(PropertyName = "base_cbs_ibs")]
        public double BaseCBSIBS { get; set; }

        [JsonProperty(PropertyName = "value_cbs")]
        public double ValorCBS { get; set; }

        [JsonProperty(PropertyName = "value_ibs")]
        public double ValorIBS { get; set; }

        [JsonProperty(PropertyName = "value_ibs_municipal")]
        public double ValorIBSMunicipal { get; set; }

        [JsonProperty(PropertyName = "value_ibs_state")]
        public double ValorIBSEstadual { get; set; }

        [JsonProperty(PropertyName = "base_is")]
        public double BaseIS { get; set; }

        [JsonProperty(PropertyName = "value_is")]
        public double ValorIS { get; set; }

        [JsonProperty(PropertyName = "debit_invoice_type")]
        public int TipoNotaDebito { get; set; }

        [JsonProperty(PropertyName = "credit_invoice_type")]
        public int TipoNotaCredito { get; set; }
    }
}

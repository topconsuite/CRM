using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalDetalhesDistribuicaoResponse
    {
        [JsonProperty(PropertyName = "supplier_cnpj_cpf")]
        public long? CnpjCpfFornecedor { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "issue_date_time")]
        public DateTime? DataHoraEmissao { get; set; }

        [JsonProperty(PropertyName = "invoice_value_in_sefaz")]
        public float? ValorNotaFiscalSefaz { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "receipt_date_time")]
        public DateTime? DataHoraRecebimento { get; set; }

        [JsonProperty(PropertyName = "protocol_number")]
        public long? NumeroProtocolo { get; set; }

        [JsonProperty(PropertyName = "event_type_code")]
        public int? CodigoTipoEvento { get; set; }

        [JsonProperty(PropertyName = "incoming_xml")]
        public string XmlEntrada { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "event_date_time")]
        public DateTime? DataHoraEvento { get; set; }
        
        [JsonProperty(PropertyName = "recipient_state_registration_indicator")]
        public string IndicadorDestinatarioIe { get; set; }
    }
}

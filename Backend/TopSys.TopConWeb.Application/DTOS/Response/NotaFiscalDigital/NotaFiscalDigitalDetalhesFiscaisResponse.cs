using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalDetalhesFiscaisResponse
    {
        [JsonProperty(PropertyName = "invoice_situation_in_sefaz")]
        public string SituacaoSefaz { get; set; }

        [JsonProperty(PropertyName = "sefaz_receipt")]
        public long? ReciboSefaz { get; set; }

        [JsonProperty(PropertyName = "sefaz_protocol")]
        public long? ProtocoloSefaz { get; set; }

        [JsonProperty(PropertyName = "invoice_authorization_status")]
        public int? StatusAutorizacao { get; set; }

        [JsonProperty(PropertyName = "status_reason_description")]
        public string MotivoDescricaoStatus { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "protocol_date_time")]
        public DateTime? DataHoraProtocolo { get; set; }

        [JsonProperty(PropertyName = "xml")]
        public string Xml { get; set; }
        
        [JsonProperty(PropertyName = "xml_author")]
        public string XmlAutor { get; set; }

        [JsonProperty(PropertyName = "invoice_uf")]
        public string NfeUf { get; set; }
        
        [JsonProperty(PropertyName = "recipient_state_registration_indicator")]
        public string IndicadorDestinatarioIe { get; set; }
    }
}

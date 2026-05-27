using Newtonsoft.Json;
namespace TopSys.TopConWeb.Application.DTOS.Response.Fatura
{
    public class ChaveNotaFiscalDTO
    {
        [JsonProperty(PropertyName = "branch")]
        public int Filial { get; set; }

        [JsonProperty(PropertyName = "client")]
        public int Cliente { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumento { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "invoice_number")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "invoice_sequence")]
        public int Sequencia { get; set; }
    }
}
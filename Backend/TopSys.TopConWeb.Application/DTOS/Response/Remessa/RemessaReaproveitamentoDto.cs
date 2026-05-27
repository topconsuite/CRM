using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Remessa
{
    public class RemessaReaproveitamentoDto
    {
        [JsonProperty(PropertyName = "branch")]
        public int FilialCodigo { get; set; }

        [JsonProperty(PropertyName = "intervener")]
        public int IntervenienteCodigo { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumentoCodigo { get; set; }

        [JsonProperty(PropertyName = "invoice_number_packing_list")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "packing_list_invoice_sequence")]
        public int Sequencia { get; set; }

        [JsonProperty(PropertyName = "concrete_mixer_reuse")]
        public string BetoneiraReaproveitamento { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int UsinaCodigo { get; set; }

        [JsonProperty(PropertyName = "date_packing_list")]
        public DateTime? DataRemessa { get; set; }

        [JsonProperty(PropertyName = "return_volume")]
        public float VolumeRetorno { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }

        [JsonProperty(PropertyName = "branch_destination_note")]
        public int FilialNotaDestino { get; set; }

        [JsonProperty(PropertyName = "intervener_destination_note")]
        public int IntervenienteNotaDestino { get; set; }

        [JsonProperty(PropertyName = "document_type_destination_note")]
        public int TipoDocumentoNotaDestino { get; set; }

        [JsonProperty(PropertyName = "invoice_number_destination_note")]
        public long NumeroNotaDestino { get; set; }

        [JsonProperty(PropertyName = "series_destination_note")]
        public string SerieNotaDestino { get; set; }

        [JsonProperty(PropertyName = "invoice_sequence_destination_note")]
        public int SequenciaNotaDestino { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant_destination_note")]
        public int UsinaNotaDestino { get; set; }

        [JsonProperty(PropertyName = "registration_id")]
        public string IdCadastro { get; set; }

        [JsonProperty(PropertyName = "current_id")]
        public string IdAtual { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Remessa
{
    public class RemessaDemaisServicosDto
    {
        [JsonProperty(PropertyName = "service_sequence")]
        public int SequenciaServico { get; set; }

        [JsonProperty(PropertyName = "merchandise")]
        public string MercadoriaCodigo { get; set; }

        [JsonProperty(PropertyName = "merchandise_description")]
        public string MercadoriaDescricao { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public double Quantidade { get; set; }

        [JsonProperty(PropertyName = "unit_value")]
        public double ValorUnitario { get; set; }

        [JsonProperty(PropertyName = "amount_charged")]
        public double ValorCobrado { get; set; }
    }
}

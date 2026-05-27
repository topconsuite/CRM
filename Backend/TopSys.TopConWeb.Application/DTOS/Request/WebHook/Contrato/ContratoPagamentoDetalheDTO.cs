using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{
    public class ContratoPagamentoDetalheDTO
    {
        public ContratoPagamentoDetalheDTO(int detalheSequencia, float valor, DateTime? data, PortadorDTO portador)
        {
            DetalheSequencia = detalheSequencia;
            Valor = valor;
            Data = data;
            Portador = portador;
        }

        [JsonProperty(PropertyName = "sequence")]
        public int DetalheSequencia { get; set; }

        [JsonProperty(PropertyName = "value")]
        public float Valor { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime? Data { get; set; }

        [JsonProperty(PropertyName = "bill-collector")]
        public PortadorDTO Portador { get; set; }

    }

}

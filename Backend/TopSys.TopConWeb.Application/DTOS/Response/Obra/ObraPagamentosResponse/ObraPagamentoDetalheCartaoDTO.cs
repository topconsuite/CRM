using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class ObraPagamentoDetalheCartaoDTO : ObraPagamentoDetalheDTO
    {
        public CartaoBandeiraDTO Bandeira { get; set; }

        public int NumeroCartao { get; set; }

        public DateTime DataTransacao { get; set; }

        public float Valor { get; set; }

        public int QuantidadeParcelas { get; set; }

        public string NumeroAutorizacao { get; set; }
    }
}

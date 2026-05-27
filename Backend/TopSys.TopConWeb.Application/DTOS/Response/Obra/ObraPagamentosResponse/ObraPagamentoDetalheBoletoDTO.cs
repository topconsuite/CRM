using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class ObraPagamentoDetalheBoletoDTO : ObraPagamentoDetalheDTO
    {
        public DateTime DataVencimento { get; set; }

        public DateTime DataHoraImpressao { get; set; }

        public string NossoNumero { get; set; }

        public string LinhaDigitavel { get; set; }

        public string CodigoDeBarras { get; set; }

        public DateTime DataRemessa { get; set; }

        public DateTime DataLiquidacao { get; set; }

        public float Valor { get; set; }

        public float ValorLiquidacao { get; set; }

        public string IdLiquidacao { get; set; }
    }
}

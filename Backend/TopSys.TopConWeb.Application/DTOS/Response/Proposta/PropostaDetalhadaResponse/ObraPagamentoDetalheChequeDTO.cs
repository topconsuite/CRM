using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class ObraPagamentoDetalheChequeDTO : ObraPagamentoDetalheDTO
    {
        public int BancoCodigoOficial { get; set; }
        public int BancoAgencia { get; set; }
        public long BancoContaNumero { get; set; }
        public int BancoContaDV { get; set; }
        public int NumeroCheque { get; set; }

        public DateTime? DataRecebimento { get; set; }

        public DateTime? DataBomPara { get; set; }
        public string Observacao { get; set; }
    }
}

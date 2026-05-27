using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao
{
    public class ObraPagamentoDetalheDTO
    {
        public int DetalheSequencia { get; set; }

        public float Valor { get; set; }

        // Cartao
        public CartaoBandeiraDTO Bandeira { get; set; }
        public int NumeroCartao { get; set; }
        public DateTime DataTransacao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public string NumeroAutorizacao { get; set; }

        // Deposito
        public DateTime DataDeposito { get; set; }
        public PortadorDTO Portador { get; set; }
        public string NumeroTerminal { get; set; }
        public string IdAprovacao { get; set; }

        // Boleto
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataHoraImpressao { get; set; }
        public string NossoNumero { get; set; }
        public string LinhaDigitavel { get; set; }
        public string CodigoDeBarras { get; set; }
        public DateTime? DataRemessa { get; set; }
        public DateTime? DataLiquidacao { get; set; }
        public float ValorLiquidacao { get; set; }
        public string IdLiquidacao { get; set; }

        // Cheque
        public int BancoCodigoOficial { get; set; }
        public int BancoAgencia { get; set; }
        public long BancoContaNumero { get; set; }
        public int BancoContaDV { get; set; }
        public int NumeroCheque { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public DateTime? DataBomPara { get; set; }
        public string Observacao { get; set; }

        // Dinheiro
        public string NumeroRecibo { get; set; }
        public DateTime? DataPagamento { get; set; }
    }
}

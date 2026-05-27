using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigitalComplemento
    {
        public int Filial { get; set; }
        public int Cliente { get; set; }
        public int TipoDocumento { get; set; }
        public string Serie { get; set; }
        public long Numero { get; set; }
        public int Sequencia { get; set; }
        public int? IndicadorPresenca { get; set; }
        public int? MeioPagamento { get; set; }
        public string PlacaTransportador { get; set; }
        public int SequenciaTransacao { get; set; }
        public decimal PercentualReducaoGoverno { get; set; }
        public short TipoEntidadeGoverno { get; set; }
        public short TipoOperacaoGoverno { get; set; }
        public decimal BaseIBSCBS { get; set; }
        public decimal ValorCBS { get; set; }
        public decimal ValorIBS { get; set; }
        public decimal ValorIBSMunicipal { get; set; }
        public decimal ValorIBSEstadual { get; set; }
        public decimal BaseIS { get; set; }
        public decimal ValorIS { get; set; }
        public short TipoNotaDebito { get; set; }
        public short TipoNotaCredito { get; set; }
    }
}

using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigitalItemComplemento
    {
        public int Filial { get; set; }
        public int Cliente { get; set; }
        public int TipoDocumento { get; set; }
        public string Serie { get; set; }
        public long Numero { get; set; }
        public int Sequencia { get; set; }
        public int SequenciaItem { get; set; }

        public int IdImpostoCBS { get; set; }
        public int IdImpostoIS { get; set; }
        public int IdImpostoIBS { get; set; }
        public string CST_CBS_IBS { get; set; }
        public string ClassificacaoTributariaCBSIBS { get; set; }
        public string CST_IS { get; set; }
        public string ClassificacaoTributariaIS { get; set; }
        public decimal BaseIBSCBS { get; set; }
        public decimal AliquotaCBSEfetiva { get; set; }
        public decimal AliquotaCBS { get; set; }
        public decimal PercentualReducaoCBS { get; set; }
        public decimal ValorCBS { get; set; }
        public decimal AliquotaIBSMunicipalEfetiva { get; set; }
        public decimal AliquotaIBSMunicipal { get; set; }
        public decimal PercentualReducaoIBSMunicipal { get; set; }
        public decimal ValorIBSMunicipal { get; set; }
        public decimal AliquotaIBSEstadualEfetiva { get; set; }
        public decimal AliquotaIBSEstadual { get; set; }
        public decimal PercentualReducaoIBSEstadual { get; set; }
        public decimal ValorIBSEstadual { get; set; }
        public decimal ValorIBS { get; set; }
        public decimal BaseIS { get; set; }
        public decimal AliquotaIS { get; set; }
        public decimal ValorIS { get; set; }
        public short SequenciaDFERef { get; set; }
        public string ChaveNFeDFERef { get; set; }
    }
}

using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class FaturaItem
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }

        public int TipoDocumento { get; set; }

        public long Numero { get; set; }

        public string Serie { get; set; }

        public int SubSerie { get; set; }

        public int SequenciaItem { get; set; }

        public int? FilialNf { get; set; }

        public int? TipoDocumentoNf { get; set; }

        public long? NumeroNf { get; set; }

        public string SerieNf { get; set; }

        public DateTime? DataNf { get; set; }

        public int UsinaFaturamentoNf { get; set; }

        public int UsinaPesagemNf { get; set; }

        public string CodigoTraco { get; set; }
        
        public virtual Mercadoria Mercadoria { get; set; }
        
        public string IdExternoMercadoria => Mercadoria?.IdExterno ?? "";
        
        public int? NumeracaoProduto => Mercadoria?.NumeracaoProduto;
        
        public string DescricaoProduto  => Mercadoria?.Descricao;

        public string Unidade { get; set; }

        public float Quantidade { get; set; }

        public float PrecoUnitario { get; set; }

        public float PrecoTotal { get; set; }

        public float ValorMaterial { get; set; }

        public float ValorServicoBruto { get; set; }

        public float ValorServico { get; set; }

        public float? ValorDesconto { get; set; }

        public float? ValorLiquido { get; set; }

        public string PisCodigoSituacaoTributaria { get; set; }

        public float? PisBaseCalculo { get; set; }

        public float? PisPercentual { get; set; }

        public float? PisValor { get; set; }

        public string CofinsCodigoSituacaoTributaria { get; set; }

        public float? CofinsBaseCalculo { get; set; }

        public float? CofinsPercentual { get; set; }

        public float? CofinsValor { get; set; }

        public string TributacaoContribuicao { get; set; }

        public DateTime InicioVigenciaTribContribuicao { get; set; }

        public float? ValorPisRetido { get; set; }

        public float? ValorCofinsRetido { get; set; }

        public float? ValorIrrf { get; set; }

        public float? ValorCsllRetido { get; set; }

        public float? IssBaseCalculo { get; set; }

        public float? IssPercentual { get; set; }

        public float? IssValor { get; set; }

        public float? IssValorRetido { get; set; }

        public long? NumeroRecibo { get; set; }
        
        public int? NumeroRps { get; set; }
        
        public long? NumeroNfse { get; set; }

        public int? CentroCusto { get; set; }
        
        public int IdImpostoCbs { get; set; }
        
        public int IdImpostoIbs { get; set; }
        
        public string CstCbsIbs { get; set; }
        
        public string ClassificacaoTributariaCbsIbs { get; set; }
        
        public decimal BaseCbsIbs{ get; set; }
        
        public decimal AliquotaCbsEfetiva { get; set; }
        
        public decimal AliquotaCbs { get; set; }
        
        public decimal PercentualReducaoCbs { get; set; }
        
        public decimal ValorCbs { get; set; }
        
        public decimal AliquotaIbsMunicipalEfetiva { get; set; }
        
        public decimal AliquotaIbsMunicipal { get; set; }
        
        public decimal PercentualReducaoIbsMunicipal { get; set; }
        
        public decimal ValorIbsMunicipal { get; set; }
        
        public decimal AliquotaIbsEstadualEfetiva { get; set; }
        
        public decimal AliquotaIbsEstadual { get; set; }
        
        public decimal PercentualReducaoIbsEstadual { get; set; }
        
        public decimal ValorIbsEstadual { get; set; }
        
        public decimal ValorIbs { get; set; }
    }
}

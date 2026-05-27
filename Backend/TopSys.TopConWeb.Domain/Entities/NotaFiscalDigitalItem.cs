using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigitalItem
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }

        public int TipoDocumento { get; set; }

        public string Serie { get; set; }

        public long Numero { get; set; }

        public int Sequencia { get; set; }

        public int SequenciaItem { get; set; }

        public DateTime DataOperacao { get; set; }

        public int TransacaoDaOperacao { get; set; }

        public int Cfop { get; set; }

        public int SequenciaCfop { get; set; }

        public int? TipoEstoque { get; set; }

        public string CódigoMercadoria { get; set; }
        
        public virtual Mercadoria Mercadoria { get; set; }
        
        public string IdExternoMercadoria => Mercadoria?.IdExterno;

        public float Quantidade { get; set; }

        public float PrecoUnitario { get; set; }

        public float ValorTotal { get; set; }

        public float? ValorDesconto { get; set; }

        public float? ValorFrete { get; set; }

        public float? ValorSeguro { get; set; }

        public float? ValorOutrasDespesas { get; set; }

        public string SituacaoTributaria { get; set; }

        public float? BaseCalculoIcms { get; set; }

        public float? AliquotaIcms { get; set; }

        public float? ValorIcms { get; set; }

        public float? BaseCalculoIcmsSubstituicao { get; set; }

        public float? AliquotaIcmsSubstituicao { get; set; }

        public float? ValorIcmsSubstituicao { get; set; }

        public float? BaseCalculoIpi { get; set; }

        public float? AliquotaIpi { get; set; }

        public float? ValorIpi { get; set; }

        public float? ValorPisNaoCumulativo { get; set; }

        public float? ValorCofinsNaoCumulativo { get; set; }

        public float CustoTotalItem { get; set; }

        public float? Peso { get; set; }

        public string TracoConcreto { get; set; }

        public float Volume { get; set; }

        public float QuantidadeEstoque { get; set; }

        public string CodigoSituacaoTributariaPis { get; set; }

        public string CodigoSituacaoTributariaCofins { get; set; }

        public float? ValorFicalIcms1 { get; set; }

        public float? ValorFicalIcms2 { get; set; }

        public float? ValorFicalIcms3 { get; set; }

        public float? ValorFicalIpi1 { get; set; }

        public float? ValorFicalIpi2 { get; set; }

        public float? ValorFicalIpi3 { get; set; }

        public float? PercentualPisNaoCumulativo { get; set; }

        public float? PercentualCofinsNaoCumulativo { get; set; }

        public int? NotaReferenciaFornecedor { get; set; }

        public int? NotaReferenciaTipoDocumento { get; set; }

        public string NotaReferenciaSerie { get; set; }

        public long? NotaReferenciaNumero { get; set; }

        public int? NotaReferenciaItem { get; set; }

        public float? BaseCalculoPis { get; set; }

        public float? PercentualPis { get; set; }

        public float? ValorPis { get; set; }

        public float? BaseCalculoCofins { get; set; }

        public float? PercentualCofins { get; set; }

        public float? ValorCofins { get; set; }

        public int? IntervenienteEstoque { get; set; }

        public string ItemPisCodigoSituacaoTributaria { get; set; }

        public float? ItemPisBaseCalculo { get; set; }

        public float? ItemPisPercentual { get; set; }

        public float? ItemPisValor { get; set; }

        public string ItemCofinsCodigoSituacaoTributaria { get; set; }

        public float? ItemCofinsBaseCalculo { get; set; }

        public float? ItemCofinsPercentual { get; set; }

        public float? ItemCofinsValor { get; set; }

        public string TributacaoContribuicaoPisCofins { get; set; }

        public DateTime? InicioVigenciaTribContribuicao { get; set; }

        public int OperacaoFinanceira { get; set; }

        public string IpiCodigoSituacaoTributaria { get; set; }

        public string Unidade { get; set; }

        public float? ValorCreditoIcmsSimplesNacional { get; set; }

        public float? PercentualCreditoIcmsSimplesNacional { get; set; }

        public int? CsoSimplesNacional { get; set; }

        public float? BaseCalculoIcmsSimplesNacional { get; set; }

        public NotaFiscalDigitalItemComplemento Complemento { get; set; }
    }
}

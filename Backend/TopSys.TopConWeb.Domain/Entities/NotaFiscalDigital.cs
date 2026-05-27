using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigital: IQueryResult
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }

        public int TipoDocumento { get; set; }

        public string Serie { get; set; }

        public long Numero { get; set; }

        public int Sequencia { get; set; }

        public DateTime DataNf { get; set; }

        public DateTime DataOperacao { get; set; }

        public DateTime? DataEmissao { get; set; }

        public int HoraEmissao { get; set; }

        public DateTime? DataLancamentoCancelamento { get; set; }

        public string Cancelada { get; set; }

        public EIndicadorOperacao IndicadorOperacao { get; set; }

        public EIndicadorEmitente IndicadorEmitente { get; set; }

        public int TransacaoDaOperacao { get; set; }

        public string ModeloDocumentoFiscalSefaz { get; set; }

        public int? SituacaoFiscal { get; set; }

        public int Cfop { get; set; }

        public int? SequenciaCfop { get; set; }

        public int? Vendedor { get; set; }

        public string UfDestino { get; set; }

        public float? ValorMercadoria { get; set; }

        public float? ValorDesconto { get; set; }

        public float? ValorFrete { get; set; }

        public float? ValorSeguro { get; set; }

        public float? ValorOutrasDespesas { get; set; }

        public float? ValorContabil { get; set; }

        public float? BaseCalculoIcms { get; set; }

        public float? AliquotaIcms { get; set; }

        public float? ValorIcms { get; set; }

        public float? BaseCalculoIcmsSubstituicao { get; set; }

        public float? ValorIcmsSubstituicao { get; set; }

        public float? ValorIpi { get; set; }

        public float? ValorFicalIcms1 { get; set; }

        public float? ValorFicalIcms2 { get; set; }

        public float? ValorFicalIcms3 { get; set; }

        public float? ValorFicalIpi1 { get; set; }

        public float? ValorFicalIpi2 { get; set; }

        public float? ValorFicalIpi3 { get; set; }

        public int? Transportador { get; set; }

        public float? QuantidadeVolume { get; set; }

        public string EspecieVolume { get; set; }

        public float? PesoBruto { get; set; }

        public float? PesoLiquido { get; set; }

        public int? IndicadorFrete { get; set; }

        public string IdentificacaoVeiculoPlaca { get; set; }

        public string ObservacaoFiscal { get; set; }

        public int? FilialDestino { get; set; }

        public string UfVeiculo { get; set; }

        public float? ValorPis { get; set; }

        public float? ValorCofins { get; set; }

        public string MensagemFiscalNfe { get; set; }

        public int? FornecedorIss { get; set; }

        public float? TotalBasePis { get; set; }

        public float? TotalPis { get; set; }

        public float? TotalBaseCofins { get; set; }

        public float? TotalCofins { get; set; }

        public string ChaveNfe { get; set; }

        public int? Operacao { get; set; }

        public float? ValorServico { get; set; }

        public int? CentroCusto { get; set; }

        public int? Requisitante { get; set; }

        public int? ClassificacaoConsumoEnergia { get; set; }

        public int? TipoLigacao { get; set; }

        public int? GrupoTensaoEnregia { get; set; }

        public int? TipoAssinante { get; set; }

        public int? TipoFrete { get; set; }

        public int? NaturezaFrete { get; set; }

        public int? FilialEstoque { get; set; }

        public int? NumeroRequisicao { get; set; }

        public int? AnoRequisicao { get; set; }

        public int? EmpresaRequisicao { get; set; }

        public DateTime? DataAtualizacao { get; set; }
        
        public Fatura FaturaVendaDeMateriais { get; set; }
        
        public ChaveFatura ChaveNotaServicoPai => FaturaVendaDeMateriais?.ChaveNotaServicoPai;

        public string ChaveNfDevolucao { get; set; }
        
        public string UsinaExternalId { get; set; }
        
        public string ClienteCfpCnpj { get; set; }
        
        public string ClienteInscEstadual { get; set; }

        public ChaveTituloContasAReceber ChaveTituloContasAReceber
        {
            get
            {
                if (FaturaVendaDeMateriais == null) return null;
                return new ChaveTituloContasAReceber
                {
                    EmpresaCodigo = (int)Math.Floor((double)FaturaVendaDeMateriais.Filial / 1000),
                    DocumentoTipoCodigo = FaturaVendaDeMateriais.TipoDocumento,
                    DocumentoSerie = FaturaVendaDeMateriais.Serie,
                    DocumentoNumero = FaturaVendaDeMateriais.Numero,
                    DocumentoSequencia = "1"
                };
            }
        }

        public virtual NotaFiscalDigitalComplemento Complemento { get; set; }

        public virtual NotaFiscalDigitalDetalhesFiscais DetalhesFiscais { get; set; }

        public virtual NotaFiscalDigitalDetalhesDistribuicao DetalhesDistribuicao { get; set; }

        public ICollection<NotaFiscalDigitalItem> Itens { get; set; }
    }
}

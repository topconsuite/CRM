using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Fatura : IQueryResult
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }
        public string ClienteCodigoExterno { get; set; }
        public string ClienteCfpCnpj { get; set; }
        
        public string ClienteInscEstadual { get; set; }

        public int TipoDocumento { get; set; }

        public long Numero { get; set; }

        public string Serie { get; set; }

        public int SubSerie { get; set; }

        public int? NumeroRps { get; set; }

        public long? NumeroNfse { get; set; }

        public DateTime DataNf { get; set; }

        public int? CodFiscalPrestadorServico { get; set; }

        public int MunicipioPrestacaoServico { get; set; }

        public int? NaturezaPrestacao { get; set; }

        public int ContratoUsina { get; set; }

        public int ContratoNumero { get; set; }

        public int ContratoAno { get; set; }

        public string FaturamentoAC { get; set; }

        public int? LocalFaturamento { get; set; }

        public int? LocalCobranca { get; set; }

        public int CondicaoPagamento { get; set; }

        public int? IndicadorPagamento { get; set; }

        public string Cancelada { get; set; }

        public int? MotivoCancelamento { get; set; }

        public float ValorServicoBruto { get; set; }

        public float? ValorDesconto { get; set; }

        public float? ValorServico { get; set; }

        public float ValorMateriaisProprio { get; set; }

        public float? ValorMateriaisTerceiros { get; set; }

        public float? ValorDespesasAcessorias { get; set; }

        public float? ValorSubContratada { get; set; }

        public float? ValorTotal { get; set; }

        public float BaseCalculoIss { get; set; }

        public float PercentualIss { get; set; }

        public float ValorIss { get; set; }

        public string ObservacaoFiscalNf { get; set; }

        public float? BaseCalculoRetencao { get; set; }

        public float? PercentualIssRetido { get; set; }

        public float? ValorIssRetido { get; set; }

        public float? BaseCalculoIrrf { get; set; }

        public float? PercentualIrrf { get; set; }

        public float? ValorIrrf { get; set; }

        public float? PercentualPis { get; set; }

        public float? ValorPis { get; set; }

        public float? PercentualCofins { get; set; }

        public float? ValorCofins { get; set; }

        public float? BaseCalculoInssRetido { get; set; }

        public float? ValorInssRetido { get; set; }

        public int? Representante { get; set; }

        public float? ValorComissaoRepresentante { get; set; }

        public int Vendedor { get; set; }

        public float? ValorComissaoVendedor { get; set; }

        public int? QuantidadeParcelas { get; set; }

        public long NumeroFaturamento { get; set; }

        public float? ValorTotalBomba { get; set; }

        public float? BaseCalculoIssBomba { get; set; }

        public float? ValorIssBomba { get; set; }

        public float? ValorIssRetidoBomba { get; set; }

        public float? PercentualInssRetido { get; set; }

        public float? BaseCalculoPis { get; set; }

        public float? BaseCalculoCofins { get; set; }

        public float? BaseCalculoIrpj { get; set; }

        public float? PercentualIrpj { get; set; }

        public float? ValorIrpj { get; set; }

        public float? BaseCalculoCsll { get; set; }

        public float? PercentualCsll { get; set; }

        public float? ValorCsll { get; set; }

        public string CodigoVerificadorNfse { get; set; }

        public string RequisicaoInterna { get; set; }

        public int? Requisitante { get; set; }

        public int? FornecedorIss { get; set; }

        public float? TotalBasePis { get; set; }

        public float? TotalPis { get; set; }

        public float? TotalBaseCofins { get; set; }

        public float? TotalCofins { get; set; }

        public DateTime? DataLancamento { get; set; }

        public int? Pendente { get; set; }

        public float? BcRetencoes { get; set; }

        public int CentroCusto { get; set; }

        public long? NumeroRecibo { get; set; }

        public long? Encapsulamento { get; set; }

        public string NumeroProtocolo { get; set; }

        public float? PercentualInss { get; set; }

        public float? ValorInss { get; set; }

        public float? BaseCalculoInss { get; set; }
        
        public Segmentacao SegmentacaoContrato { get; set; }

        public int? SegmentacaoId => SegmentacaoContrato?.Id;
        
        public string SegmentacaoNome => SegmentacaoContrato?.Nome ?? "";
        
        public string SegmentacaoNomeAbreviado  => SegmentacaoContrato?.NomeAbreviado ?? "";

        public DateTime? DataAtualizacao { get; set; }
        
        public Obra Obra { get; set; }
        
        public string CnoObra => Obra?.Cei ?? "";
        
        public string NomeObra => Obra?.Nome ?? "";
        
        public int VersaoContrato { get; set; }
        
        public ChaveNotaFiscalDigital ChaveNotaVendaMateriais { get; set; }
        
        public ChaveFatura ChaveNotaServicoPai { get; set; }
        public ChaveTituloContasAReceber ChaveTituloJuncao { get; set; }
        
        public decimal BaseCbsIbs{ get; set; }
        
        public decimal ValorCbs { get; set; }
        
        public decimal ValorIbsMunicipal { get; set; }
        
        public decimal ValorIbsEstadual { get; set; }
        
        public decimal ValorIbs { get; set; }
        
        public string EnderecoObraCompleto
        {
            get
            {
                var endereco = "";
                if (Obra == null) return "";
                
                if (!string.IsNullOrEmpty(Obra.EnderecoLogradouro)) endereco = Obra.EnderecoLogradouro + ", ";
                
                endereco += Obra.EnderecoNumero + ", ";
                
                if (!string.IsNullOrEmpty(Obra.EnderecoComplemento)) endereco += Obra.EnderecoComplemento + " - ";
                
                if (!string.IsNullOrEmpty(Obra.EnderecoBairro)) endereco += Obra.EnderecoBairro + " - ";
                
                if (!string.IsNullOrEmpty(Obra.EnderecoMunicipio?.Nome)) endereco += Obra.EnderecoMunicipio.Nome  + ", ";
                
                endereco += $"CEP: {Convert.ToUInt64(string.IsNullOrEmpty(Obra?.EnderecoCep) ? "0" : Obra?.EnderecoCep ):00000\\-000}";

                return endereco;
                
            }
        }

        public ICollection<FaturaItem> Itens { get; set; }
        
        public void SetSegmentacaoContratoPorBombaTaxaExtra()
        {
            var codigosTaxaExtra = 
                (from EMercadoriaBombaTaxasExtras cod in Enum.GetValues(typeof(EMercadoriaBombaTaxasExtras)) 
                 where cod != EMercadoriaBombaTaxasExtras.Bombeamento 
                 select ((int)cod).ToString()).ToList();
            
            if (SegmentacaoContrato == null || Itens == null || Itens?.Count == 0) return;
            
            if (Itens.All(item => item.CodigoTraco == ((int)EMercadoriaBombaTaxasExtras.Bombeamento).ToString()))
            {
                SegmentacaoContrato.Nome= "Bombeamento";
                SegmentacaoContrato.NomeAbreviado = "BOM";
            }
            else if (Itens.All(item => item.CodigoTraco == ((int)EMercadoriaBombaTaxasExtras.TxPermanencia).ToString()))
            {
                SegmentacaoContrato.Nome = "Taxa Permanência Obra";
                SegmentacaoContrato.NomeAbreviado = "TPO";
            }
            else if (Itens.All(item => item.CodigoTraco == ((int)EMercadoriaBombaTaxasExtras.ServicoConcretagem).ToString()))
            {
                SegmentacaoContrato.Nome = "Serviço Concretagem";
                SegmentacaoContrato.NomeAbreviado = "SER";
            }
            else if (Itens.All(item => codigosTaxaExtra.Contains(item.CodigoTraco)))
            {
                SegmentacaoContrato.Nome= "Taxas Extras";
                SegmentacaoContrato.NomeAbreviado = "TAX";
            }
            
        }
        
    }
}

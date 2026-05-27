using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class FaturaMap : EntityTypeConfiguration<Fatura>
    {
        public FaturaMap()
        {
            ToTable("topsys.fis_nf_servico");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Numero, t.Serie, t.SubSerie });

            Ignore(t => t.SegmentacaoContrato);
            
            Ignore(t => t.SegmentacaoId);
            
            Ignore(t => t.SegmentacaoNome);
            
            Ignore(t => t.SegmentacaoNomeAbreviado);
            
            Ignore(t => t.Obra);
            
            Ignore(t => t.ChaveNotaVendaMateriais);
            
            Ignore(t => t.ChaveTituloJuncao);
            
            Ignore(t => t.ChaveNotaServicoPai);

            Property(t => t.Filial)
                .HasColumnOrder(0)
                .HasColumnName("filial");

            Property(t => t.Cliente)
                .HasColumnOrder(1)
                .HasColumnName("cli");

            Property(t => t.TipoDocumento)
                .HasColumnOrder(2)
                .HasColumnName("tp_doc");

            Property(t => t.Numero)
                .HasColumnOrder(3)
                .HasColumnName("num_nf");

            Property(t => t.Serie)
                .HasColumnOrder(4)
                .HasColumnName("ser");

            Property(t => t.SubSerie)
                .HasColumnOrder(5)
                .HasColumnName("sub_ser");

            Property(t => t.NumeroRps)
                .HasColumnName("num_rps");

            Property(t => t.NumeroNfse)
                .HasColumnName("num_nf_serv_seq");

            Property(t => t.DataNf)
                .HasColumnName("dt_nf");

            Property(t => t.CodFiscalPrestadorServico)
                .HasColumnName("cod_fisc_pserv");

            Property(t => t.MunicipioPrestacaoServico)
                .HasColumnName("munic_pserv");

            Property(t => t.NaturezaPrestacao)
                .HasColumnName("nat_prestacao");

            Property(t => t.ContratoUsina)
                .HasColumnName("usina_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnName("num_contrato");

            Property(t => t.ContratoAno)
                .HasColumnName("ano_contrato");

            Property(t => t.FaturamentoAC)
                .HasColumnName("faturamento_ac");

            Property(t => t.LocalFaturamento)
                .HasColumnName("local_faturamen");

            Property(t => t.LocalCobranca)
                .HasColumnName("local_cobranca");

            Property(t => t.CondicaoPagamento)
                .HasColumnName("cond_pgto");

            Property(t => t.IndicadorPagamento)
                .HasColumnName("ind_pgto");

            Property(t => t.Cancelada)
                .HasColumnName("cancelada");

            Property(t => t.MotivoCancelamento)
                .HasColumnName("motivo_cancelam");

            Property(t => t.ValorServicoBruto)
                .HasColumnName("vl_serv_bruto");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desco");

            Property(t => t.ValorServico)
                .HasColumnName("vl_serv");

            Property(t => t.ValorMateriaisProprio)
                .HasColumnName("vlr_mat_prop_ut");

            Property(t => t.ValorMateriaisTerceiros)
                .HasColumnName("vlr_mat_3os_ut");

            Property(t => t.ValorDespesasAcessorias)
                .HasColumnName("desp_acessorias");

            Property(t => t.ValorSubContratada)
                .HasColumnName("vlr_subcontrata");

            Property(t => t.ValorTotal)
                .HasColumnName("valor_total");

            Property(t => t.BaseCalculoIss)
                .HasColumnName("base_calc_iss");

            Property(t => t.PercentualIss)
                .HasColumnName("pct_iss");

            Property(t => t.ValorIss)
                .HasColumnName("vl_iss");

            Property(t => t.ObservacaoFiscalNf)
                .HasColumnName("obs_fisc_nf");

            Property(t => t.BaseCalculoRetencao)
                .HasColumnName("base_calc_reten");

            Property(t => t.PercentualIssRetido)
                .HasColumnName("pct_iss_retido");

            Property(t => t.ValorIssRetido)
                .HasColumnName("vlr_iss_retido");

            Property(t => t.BaseCalculoIrrf)
                .HasColumnName("base_cal_irrf");

            Property(t => t.PercentualIrrf)
                .HasColumnName("pct_irrf");

            Property(t => t.ValorIrrf)
                .HasColumnName("vlr_irrf");

            Property(t => t.PercentualPis)
                .HasColumnName("pct_pis");

            Property(t => t.ValorPis)
                .HasColumnName("vlr_pis");

            Property(t => t.PercentualCofins)
                .HasColumnName("pct_cofins");

            Property(t => t.ValorCofins)
                .HasColumnName("vlr_cofins");

            Property(t => t.BaseCalculoInssRetido)
                .HasColumnName("base_cal_inss_ret");

            Property(t => t.ValorInssRetido)
                .HasColumnName("vlr_inss_retido");

            Property(t => t.Representante)
                .HasColumnName("representante");

            Property(t => t.ValorComissaoRepresentante)
                .HasColumnName("vlr_comis_repre");

            Property(t => t.Vendedor)
                .HasColumnName("vendedor");

            Property(t => t.ValorComissaoVendedor)
                .HasColumnName("vlr_comis_vende");

            Property(t => t.QuantidadeParcelas)
                .HasColumnName("qtde_parcelas");

            Property(t => t.NumeroFaturamento)
                .HasColumnName("no_faturamento");

            Property(t => t.ValorTotalBomba)
                .HasColumnName("valor_total_bomba");

            Property(t => t.BaseCalculoIssBomba)
                .HasColumnName("base_calc_iss_bomba");

            Property(t => t.ValorIssBomba)
                .HasColumnName("vl_iss_bomba");

            Property(t => t.ValorIssRetidoBomba)
                .HasColumnName("vlr_iss_retido_bomba");

            Property(t => t.PercentualInssRetido)
                .HasColumnName("pct_inss_retido");

            Property(t => t.BaseCalculoPis)
                .HasColumnName("base_calc_pis");

            Property(t => t.BaseCalculoCofins)
                .HasColumnName("base_cal_cofins");

            Property(t => t.BaseCalculoIrpj)
                .HasColumnName("base_calc_irpj");

            Property(t => t.PercentualIrpj)
                .HasColumnName("pct_irpj");

            Property(t => t.ValorIrpj)
                .HasColumnName("vlr_irpj");

            Property(t => t.BaseCalculoCsll)
                .HasColumnName("base_calc_csll");

            Property(t => t.PercentualCsll)
                .HasColumnName("pct_csll");

            Property(t => t.ValorCsll)
                .HasColumnName("vlr_csll");

            Property(t => t.CodigoVerificadorNfse)
                .HasColumnName("cod_verfic_nfse");

            Property(t => t.RequisicaoInterna)
                .HasColumnName("requis_interna");

            Property(t => t.Requisitante)
                .HasColumnName("requisitante");

            Property(t => t.FornecedorIss)
                .HasColumnName("forn_iss");

            Property(t => t.TotalBasePis)
                .HasColumnName("total_base_pis");

            Property(t => t.TotalPis)
                .HasColumnName("total_pis");

            Property(t => t.TotalBaseCofins)
                .HasColumnName("total_base_cofins");

            Property(t => t.TotalCofins)
                .HasColumnName("total_cofins");

            Property(t => t.DataLancamento)
                .HasColumnName("dt_lancamento");

            Property(t => t.Pendente)
                .HasColumnName("pendente");

            Property(t => t.BcRetencoes)
                .HasColumnName("bc_retencoes");

            Property(t => t.CentroCusto)
                .HasColumnName("cc");

            Property(t => t.NumeroRecibo)
                .HasColumnName("num_recibo");

            Property(t => t.Encapsulamento)
                .HasColumnName("encapsulamento");

            Property(t => t.NumeroProtocolo)
                .HasColumnName("num_protocolo");

            Property(t => t.PercentualInss)
                .HasColumnName("pct_inss");

            Property(t => t.ValorInss)
                .HasColumnName("vlr_inss");

            Property(t => t.BaseCalculoInss)
                .HasColumnName("base_cal_inss");
            
            Property(t => t.VersaoContrato)
                .HasColumnName("versao_contrato");
            
            Property(t => t.BaseCbsIbs)
                .HasColumnName("base_ibscbs");
            
            Property(t => t.ValorCbs)
                .HasColumnName("vl_cbs");
            
            Property(t => t.ValorIbs)
                .HasColumnName("vl_ibs");
            
            Property(t => t.ValorIbsMunicipal)
                .HasColumnName("vl_ibs_mun");
            
            Property(t => t.ValorIbsEstadual)
                .HasColumnName("vl_ibs_uf");

            Property(t => t.DataAtualizacao)
                .HasColumnName("atualizado_em")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            HasMany(t => t.Itens)
                .WithOptional()
                .HasForeignKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Numero, t.Serie, t.SubSerie });

            Ignore(t => t.ClienteCfpCnpj);
            Ignore(t => t.ClienteCodigoExterno);
            Ignore(t => t.ClienteInscEstadual);
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalItemMap : EntityTypeConfiguration<NotaFiscalDigitalItem>
    {
        public NotaFiscalDigitalItemMap()
        {
            ToTable("topsys.fis_item_nf");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero, t.Sequencia, t.SequenciaItem });

            Ignore(t => t.Complemento);

            Property(t => t.Filial)
                .HasColumnName("filial");

            Property(t => t.Cliente)
                .HasColumnName("interv");

            Property(t => t.TipoDocumento)
                .HasColumnName("tp_doc");

            Property(t => t.Serie)
                .HasColumnName("ser");

            Property(t => t.Numero)
                .HasColumnName("num_nf");
            
            Property(t => t.Sequencia)
                .HasColumnName("seq_nf");

            Property(t => t.SequenciaItem)
                .HasColumnName("num_seq_item_nf");

            Property(t => t.DataOperacao)
                .HasColumnName("dt_op");

            Property(t => t.TransacaoDaOperacao)
                .HasColumnName("trans");

            Property(t => t.Cfop)
                .HasColumnName("cfop");

            Property(t => t.SequenciaCfop)
                .HasColumnName("seq_cfop");

            Property(t => t.TipoEstoque)
                .HasColumnName("tp_estq");

            Property(t => t.CódigoMercadoria)
                .HasColumnName("merc");

            Property(t => t.Quantidade)
                .HasColumnName("qt");

            Property(t => t.PrecoUnitario)
                .HasColumnName("preco_un");

            Property(t => t.ValorTotal)
                .HasColumnName("vl_tot");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desc");

            Property(t => t.ValorFrete)
                .HasColumnName("vl_frete");

            Property(t => t.ValorSeguro)
                .HasColumnName("vl_seg");

            Property(t => t.ValorOutrasDespesas)
                .HasColumnName("vl_o_desp");

            Property(t => t.SituacaoTributaria)
                .HasColumnName("sit_trib");

            Property(t => t.BaseCalculoIcms)
                .HasColumnName("base_calc_icms");

            Property(t => t.AliquotaIcms)
                .HasColumnName("aliq_icms");

            Property(t => t.ValorIcms)
                .HasColumnName("vl_icms");

            Property(t => t.BaseCalculoIcmsSubstituicao)
                .HasColumnName("base_icms_sub_t");

            Property(t => t.AliquotaIcmsSubstituicao)
                .HasColumnName("aliq_icms_sub");

            Property(t => t.ValorIcmsSubstituicao)
                .HasColumnName("vl_icms_sub_tri");

            Property(t => t.BaseCalculoIpi)
                .HasColumnName("base_calc_ipi");

            Property(t => t.AliquotaIpi)
                .HasColumnName("aliq_ipi");

            Property(t => t.ValorIpi)
                .HasColumnName("vl_ipi");

            Property(t => t.ValorPisNaoCumulativo)
                .HasColumnName("vl_pis_n_cum");

            Property(t => t.ValorCofinsNaoCumulativo)
                .HasColumnName("vl_cofins_n_cum");

            Property(t => t.CustoTotalItem)
                .HasColumnName("custo_tot_item");

            Property(t => t.Peso)
                .HasColumnName("peso");

            Property(t => t.TracoConcreto)
                .HasColumnName("traco_concreto");

            Property(t => t.Volume)
                .HasColumnName("volume");

            Property(t => t.QuantidadeEstoque)
                .HasColumnName("qtde_estoque");

            Property(t => t.CodigoSituacaoTributariaPis)
                .HasColumnName("cst_pis");

            Property(t => t.CodigoSituacaoTributariaCofins)
                .HasColumnName("cst_cofins");

            Property(t => t.ValorFicalIcms1)
                .HasColumnName("vl_fisc_1_icms");

            Property(t => t.ValorFicalIcms2)
                .HasColumnName("vl_fisc_2_icms");

            Property(t => t.ValorFicalIcms3)
                .HasColumnName("vl_fisc_3_icms");

            Property(t => t.ValorFicalIpi1)
                .HasColumnName("vl_fisc_1_ipi");

            Property(t => t.ValorFicalIpi2)
                .HasColumnName("vl_fisc_2_ipi");

            Property(t => t.ValorFicalIpi3)
                .HasColumnName("vl_fisc_3_ipi");

            Property(t => t.PercentualPisNaoCumulativo)
                .HasColumnName("pct_pis_n_cum");

            Property(t => t.PercentualCofinsNaoCumulativo)
                .HasColumnName("pct_cofins_ncum");

            Property(t => t.NotaReferenciaFornecedor)
                .HasColumnName("ref_forn");

            Property(t => t.NotaReferenciaTipoDocumento)
                .HasColumnName("ref_tp_doc");

            Property(t => t.NotaReferenciaSerie)
                .HasColumnName("ref_serie");

            Property(t => t.NotaReferenciaNumero)
                .HasColumnName("ref_num_nf");

            Property(t => t.NotaReferenciaItem)
                .HasColumnName("ref_item");

            Property(t => t.BaseCalculoPis)
                .HasColumnName("vlr_bc_pis");

            Property(t => t.PercentualPis)
                .HasColumnName("pct_pis");

            Property(t => t.ValorPis)
                .HasColumnName("vlr_pis");

            Property(t => t.BaseCalculoCofins)
                .HasColumnName("vlr_bc_cofins");

            Property(t => t.PercentualCofins)
                .HasColumnName("pct_cofins");

            Property(t => t.ValorCofins)
                .HasColumnName("vlr_cofins");

            Property(t => t.IntervenienteEstoque)
                .HasColumnName("interv_estq");

            Property(t => t.ItemPisCodigoSituacaoTributaria)
                .HasColumnName("PisCst");

            Property(t => t.ItemPisBaseCalculo)
                .HasColumnName("PisBC");

            Property(t => t.ItemPisPercentual)
                .HasColumnName("PisPct");

            Property(t => t.ItemPisValor)
                .HasColumnName("PisValor");

            Property(t => t.ItemCofinsCodigoSituacaoTributaria)
                .HasColumnName("CofinsCst");

            Property(t => t.ItemCofinsBaseCalculo)
                .HasColumnName("CofinsBC");

            Property(t => t.ItemCofinsPercentual)
                .HasColumnName("CofinsPct");

            Property(t => t.ItemCofinsValor)
                .HasColumnName("CofinsValor");

            Property(t => t.TributacaoContribuicaoPisCofins)
                .HasColumnName("tribContrib");

            Property(t => t.InicioVigenciaTribContribuicao)
                .HasColumnName("IniVigTribCont");

            Property(t => t.OperacaoFinanceira)
                .HasColumnName("oper");

            Property(t => t.IpiCodigoSituacaoTributaria)
                .HasColumnName("ipi_cst");

            Property(t => t.Unidade)
                .HasColumnName("unidade");

            Property(t => t.ValorCreditoIcmsSimplesNacional)
                .HasColumnName("vCredICMSSN");

            Property(t => t.PercentualCreditoIcmsSimplesNacional)
                .HasColumnName("pCredSN");

            Property(t => t.CsoSimplesNacional)
                .HasColumnName("CSOSN");

            Property(t => t.BaseCalculoIcmsSimplesNacional)
                .HasColumnName("base_calc_icmssn");
            
            HasOptional(t => t.Mercadoria)
                .WithMany()
                .HasForeignKey(t => t.CódigoMercadoria);
        }
    }
}

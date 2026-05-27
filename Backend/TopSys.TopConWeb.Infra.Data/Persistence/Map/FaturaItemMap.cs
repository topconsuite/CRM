using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class FaturaItemMap : EntityTypeConfiguration<FaturaItem>
    {
        public FaturaItemMap()
        {
            ToTable("topsys.fis_item_nf_serv");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Numero, t.Serie, t.SubSerie, t.SequenciaItem });

            Property(t => t.Filial)
                .HasColumnName("filial");

            Property(t => t.Cliente)
                .HasColumnName("cli");

            Property(t => t.TipoDocumento)
                .HasColumnName("tp_docs");

            Property(t => t.Numero)
                .HasColumnName("num_nfs");

            Property(t => t.Serie)
                .HasColumnName("serie_nfs");

            Property(t => t.SubSerie)
                .HasColumnName("sub_series");

            Property(t => t.SequenciaItem)
                .HasColumnName("seq_item");

            Property(t => t.FilialNf)
                .HasColumnName("filial_nf");

            Property(t => t.TipoDocumentoNf)
                .HasColumnName("tp_doc_nf");

            Property(t => t.NumeroNf)
                .HasColumnName("num_nf");

            Property(t => t.SerieNf)
                .HasColumnName("serie_nf");

            Property(t => t.DataNf)
                .HasColumnName("dt_nf");

            Property(t => t.CodigoTraco)
                .HasColumnName("prod_serv");

            Property(t => t.Unidade)
                .HasColumnName("unidade");

            Property(t => t.Quantidade)
                .HasColumnName("qt");

            Property(t => t.PrecoUnitario)
                .HasColumnName("preco_unit");

            Property(t => t.PrecoTotal)
                .HasColumnName("preco_tot");

            Property(t => t.ValorMaterial)
                .HasColumnName("vlr_material");

            Property(t => t.ValorServicoBruto)
                .HasColumnName("vl_serv_bruto");

            Property(t => t.ValorServico)
                .HasColumnName("vlr_servico");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desco");

            Property(t => t.ValorLiquido)
                .HasColumnName("vl_liq");

            Property(t => t.PisCodigoSituacaoTributaria)
                .HasColumnName("PisCst");

            Property(t => t.PisBaseCalculo)
                .HasColumnName("PisBC");

            Property(t => t.PisPercentual)
                .HasColumnName("PisPct");

            Property(t => t.PisValor)
                .HasColumnName("PisValor");

            Property(t => t.CofinsCodigoSituacaoTributaria)
                .HasColumnName("CofinsCst");

            Property(t => t.CofinsBaseCalculo)
                .HasColumnName("CofinsBC");

            Property(t => t.CofinsPercentual)
                .HasColumnName("CofinsPct");

            Property(t => t.CofinsValor)
                .HasColumnName("CofinsValor");

            Property(t => t.TributacaoContribuicao)
                .HasColumnName("tribContrib");

            Property(t => t.InicioVigenciaTribContribuicao)
                .HasColumnName("IniVigTribCont");

            Property(t => t.ValorPisRetido)
                .HasColumnName("vlr_pis_ret");

            Property(t => t.ValorCofinsRetido)
                .HasColumnName("vlr_cofins_ret");

            Property(t => t.ValorIrrf)
                .HasColumnName("vlr_irrf");

            Property(t => t.ValorCsllRetido)
                .HasColumnName("vlr_csll_ret");

            Property(t => t.IssBaseCalculo)
                .HasColumnName("iss_bc");

            Property(t => t.IssPercentual)
                .HasColumnName("iss_pct");

            Property(t => t.IssValor)
                .HasColumnName("iss_vlr");

            Property(t => t.IssValorRetido)
                .HasColumnName("iss_vlr_ret");

            Property(t => t.NumeroRecibo)
                .HasColumnName("num_recibo");
            
            Property(t => t.NumeroRps)
                .HasColumnName("num_rps");
            
            Property(t => t.NumeroNfse)
                .HasColumnName("num_nf_serv_seq");

            Property(t => t.CentroCusto)
                .HasColumnName("ccusto");

            Property(t => t.IdImpostoCbs)
                .HasColumnName("id_imp_cbs");
            
            Property(t => t.IdImpostoIbs)
                .HasColumnName("id_imp_ibs");
            
            Property(t => t.CstCbsIbs)
                .HasColumnName("cst_cbs_ibs");
            
            Property(t => t.ClassificacaoTributariaCbsIbs)
                .HasColumnName("clas_trib_cbs_ibs");
            
            Property(t => t.BaseCbsIbs)
                .HasColumnName("base_ibscbs");
            
            Property(t => t.AliquotaCbs)
                .HasColumnName("aliq_cbs");
            
            Property(t => t.AliquotaCbsEfetiva)
                .HasColumnName("aliq_cbs_efet");
            
            Property(t => t.PercentualReducaoCbs)
                .HasColumnName("p_redcbs");
            
            Property(t => t.ValorCbs)
                .HasColumnName("vl_cbs");
            
            Property(t => t.AliquotaIbsMunicipalEfetiva)
                .HasColumnName("aliq_ibs_mun_efet");
            
            Property(t => t.AliquotaIbsMunicipal)
                .HasColumnName("aliq_ibs_mun");
            
            Property(t => t.PercentualReducaoIbsMunicipal)
                .HasColumnName("p_redibs_mun");
            
            Property(t => t.ValorIbsMunicipal)
                .HasColumnName("vl_ibs_mun");
            
            Property(t => t.AliquotaIbsEstadualEfetiva)
                .HasColumnName("aliq_ibs_uf_efet");
            
            Property(t => t.AliquotaIbsEstadual)
                .HasColumnName("aliq_ibs_uf");
            
            Property(t => t.PercentualReducaoIbsEstadual)
                .HasColumnName("p_redibs_uf");
            
            Property(t => t.ValorIbsEstadual)
                .HasColumnName("vl_ibs_uf");
            
            Property(t => t.ValorIbs)
                .HasColumnName("vl_ibs");
            
            HasRequired(t => t.Mercadoria)
                .WithMany()
                .HasForeignKey(t => t.CodigoTraco);

            Ignore(t => t.UsinaFaturamentoNf);

            Ignore(t => t.UsinaPesagemNf);
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalItemComplementoMap : EntityTypeConfiguration<NotaFiscalDigitalItemComplemento>
    {
        public NotaFiscalDigitalItemComplementoMap()
        {
            ToTable("topsys.fis_item_nf_complemento");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero, t.Sequencia, t.SequenciaItem });

            Property(t => t.Filial).HasColumnOrder(0).HasColumnName("filial");
            Property(t => t.Cliente).HasColumnOrder(1).HasColumnName("interv");
            Property(t => t.TipoDocumento).HasColumnOrder(2).HasColumnName("tp_doc");
            Property(t => t.Serie).HasColumnOrder(3).HasColumnName("ser");
            Property(t => t.Numero).HasColumnOrder(4).HasColumnName("num_nf");
            Property(t => t.Sequencia).HasColumnOrder(5).HasColumnName("seq_nf");
            Property(t => t.SequenciaItem).HasColumnOrder(6).HasColumnName("num_seq_item_nf");

            Property(t => t.IdImpostoCBS).HasColumnName("id_imp_cbs");
            Property(t => t.IdImpostoIBS).HasColumnName("id_imp_ibs");
            Property(t => t.IdImpostoIS).HasColumnName("id_imp_is");

            Property(t => t.CST_CBS_IBS).HasColumnName("cst_cbs_ibs");
            Property(t => t.ClassificacaoTributariaCBSIBS).HasColumnName("clas_trib_cbs_ibs");
            Property(t => t.CST_IS).HasColumnName("cst_is");
            Property(t => t.ClassificacaoTributariaIS).HasColumnName("clas_trib_is");

            Property(t => t.BaseIBSCBS).HasColumnName("base_ibscbs");
            Property(t => t.AliquotaCBSEfetiva).HasColumnName("aliq_cbs_efet");
            Property(t => t.AliquotaCBS).HasColumnName("aliq_cbs");
            Property(t => t.PercentualReducaoCBS).HasColumnName("p_redcbs");
            Property(t => t.ValorCBS).HasColumnName("vl_cbs");

            Property(t => t.AliquotaIBSMunicipalEfetiva).HasColumnName("aliq_ibs_mun_efet");
            Property(t => t.AliquotaIBSMunicipal).HasColumnName("aliq_ibs_mun");
            Property(t => t.PercentualReducaoIBSMunicipal).HasColumnName("p_redibs_mun");
            Property(t => t.ValorIBSMunicipal).HasColumnName("vl_ibs_mun");

            Property(t => t.AliquotaIBSEstadualEfetiva).HasColumnName("aliq_ibs_uf_efet");
            Property(t => t.AliquotaIBSEstadual).HasColumnName("aliq_ibs_uf");
            Property(t => t.PercentualReducaoIBSEstadual).HasColumnName("p_redibs_uf");
            Property(t => t.ValorIBSEstadual).HasColumnName("vl_ibs_uf");

            Property(t => t.ValorIBS).HasColumnName("vl_ibs");
            Property(t => t.BaseIS).HasColumnName("base_is");
            Property(t => t.AliquotaIS).HasColumnName("aliq_is");
            Property(t => t.ValorIS).HasColumnName("vl_is");

            Property(t => t.SequenciaDFERef).HasColumnName("seq_dfe_ref");
            Property(t => t.ChaveNFeDFERef).HasColumnName("chNFe_dfe_ref");
        }
    }
}

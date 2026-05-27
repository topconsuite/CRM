using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class MunicipioMap : EntityTypeConfiguration<Municipio>
    {
        public MunicipioMap()
        {
            ToTable("topsys.ger_municipio");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Nome)
                .HasColumnName("mun");

            Property(t => t.Uf)
                .HasColumnName("uf");

            Property(t => t.IbgeCodigo)
                .HasColumnName("oficial");

            Property(t => t.AliquotaIss)
                .HasColumnName("pct_iss");

            Property(t => t.BaseCalculo)
                .HasColumnName("base_calc_iss");

            Property(t => t.TributacaoIntegralDemaisServicos)
                .HasColumnName("dem_serv_trib_integralmente");

            Property(t => t.TributacaoIntegralBomba)
                .HasColumnName("iss_bomba");

            Property(t => t.PorcentagemServico)
                .HasColumnName("pct_servico");

            Property(t => t.PorcentagemDeducaoMaterial)
                .HasColumnName("pct_ded_material");

            Property(t => t.Pais)
                .HasColumnName("pais");

            Property(t => t.NomeReduzido)
                .HasColumnName("nome_reduzido");

            Property(t => t.TributacaoIss)
                .HasColumnName("iss");

            Property(t => t.PercentualIssRetido)
                .HasColumnName("pct_iss_retido");

            Property(t => t.IssRetido)
                .HasColumnName("iss_retido");

            Property(t => t.IntervPrefeituraRetencao)
                .HasColumnName("interv");

            Property(t => t.CodigoSiafi)
                .HasColumnName("oficial_siafi");

            Property(t => t.MensagemFiscal)
               .HasColumnName("msg_fis_nfes");

            Property(t => t.IdExterno)
               .HasColumnName("cod_integracao");

            Property(t => t.TaxasTributadasIntegralmente)
               .HasColumnName("taxas_trib_integralmente");

            Property(t => t.ValorMinimoRetencaoIss)
               .HasColumnName("vlr_min_retencao_iss");
        }
    }
}

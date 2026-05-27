using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialTipoPessoaMap: EntityTypeConfiguration<AprovacaoComercialTipoPessoa>
    {

        public AprovacaoComercialTipoPessoaMap()
        {

            ToTable("topsys.con_aprovacao_comercial_tipo_pessoa");

            HasKey(a => a.Id);

            Property(a => a.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(32)
                .IsRequired();

            Property(a => a.Sigla)
                .HasColumnName("sigla")
                .HasMaxLength(1)
                .IsRequired();

        }

    }
}

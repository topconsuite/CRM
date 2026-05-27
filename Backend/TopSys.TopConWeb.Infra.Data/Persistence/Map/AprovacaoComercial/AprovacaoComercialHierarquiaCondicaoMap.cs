using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaCondicaoMap : EntityTypeConfiguration<AprovacaoComercialHierarquiaCondicao>
    {

        public AprovacaoComercialHierarquiaCondicaoMap()
        {

            ToTable("topsys.con_aprovacao_comercial_hierarquia_condicao");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("id");

            Property(x => x.ValorDe)
                .HasColumnOrder(0)
                .HasColumnName("valor_de");

            Property(x => x.ValorAte)
                .HasColumnOrder(1)
                .HasColumnName("valor_ate");

            Property(x => x.PercentualDe)
                .HasColumnOrder(2)
                .HasColumnName("percentual_de");

            Property(x => x.PercentualAte)
                .HasColumnOrder(3)
                .HasColumnName("percentual_ate");

            Property(x => x.Valor)
                .HasColumnOrder(4)
                .HasColumnName("tipo_valor");


            Property(x => x.TipoPessoaId)
                .HasColumnOrder(5)
                .HasColumnName("tipo_pessoa_id");

            Property(x => x.AprovacaoComercialHierarquiaId)
                .HasColumnOrder(6)
                .HasColumnName("aprovacao_comercial_hierarquia_id");

            HasRequired(x => x.TipoPessoa)
                .WithMany()
                .HasForeignKey(x => x.TipoPessoaId);

            HasRequired(x => x.AprovacaoComercialHierarquia)
                .WithMany()
                .HasForeignKey(x => x.AprovacaoComercialHierarquiaId);

        }

    }
}

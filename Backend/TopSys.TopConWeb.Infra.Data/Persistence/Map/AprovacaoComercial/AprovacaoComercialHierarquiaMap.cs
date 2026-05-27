using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaMap : EntityTypeConfiguration<AprovacaoComercialHierarquia>
    {

        public AprovacaoComercialHierarquiaMap()
        {

            ToTable("topsys.con_aprovacao_comercial_hierarquia");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("id");

            Property(x => x.AprovacaoComercialUsinaId)
                .HasColumnOrder(0)
                .HasColumnName("aprovacao_comercial_usina_id");

            Property(x => x.Titulo)
                .HasColumnOrder(2)
                .HasColumnName("titulo");

            Property(x => x.NivelAutoridade)
                .HasColumnOrder(3)
                .HasColumnName("nivel_autoridade");

            Property(x => x.QuantidadeAprovacoesNecessarias)
                .HasColumnOrder(4)
                .HasColumnName("quantidade_aprovacoes_necessarias");

            Property(x => x.AprovacaoObrigatoria)
                .HasColumnOrder(5)
                .HasColumnName("aprovacao_obrigatoria");

            Property(x => x.CreatedAt)
                .HasColumnOrder(6)
                .HasColumnName("created_at");

            Property(x => x.UpdatedAt)
                .HasColumnOrder(7)
                .HasColumnName("updated_at");

            HasMany(x => x.Condicoes)
                .WithRequired()
                .HasForeignKey(x => x.AprovacaoComercialHierarquiaId);

        }

    }
}

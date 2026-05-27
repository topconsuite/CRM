using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialUsinaMap : EntityTypeConfiguration<AprovacaoComercialUsina>
    {

        public AprovacaoComercialUsinaMap()
        {
            ToTable("topsys.con_aprovacao_comercial_usina");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(a => a.UsinaId)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(a => a.Ativo)
                .HasColumnOrder(2)
                .HasColumnName("ativo");

            Property(a => a.CreatedAt)
                .HasColumnOrder(3)
                .HasColumnName("created_at")
                .IsRequired();

            Property(a => a.UpdatedAt)
                .HasColumnOrder(4)
                .HasColumnName("updated_at");

            Property(a => a.FluxoAprovacao)
                .HasColumnOrder(5)
                .HasColumnName("fluxo_aprovacao");

            HasRequired(o => o.Usina)
                .WithMany()
                .HasForeignKey(o => o.UsinaId);

            HasMany(x => x.Hierarquias)
                .WithRequired()
                .HasForeignKey(x => x.AprovacaoComercialUsinaId);


        }

    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PrensaMap : EntityTypeConfiguration<Prensa>
    {
        public PrensaMap()
        {
            ToTable("topsys.con_prensa_carga");

            HasKey(t => t.PrensaNome);

            Property(t => t.PrensaNome)
                .HasColumnName("prensa_nome");

            Property(t => t.Carga)
                .HasColumnName("carga");
        }
    }
}

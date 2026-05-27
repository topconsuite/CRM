using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class UsoMap:EntityTypeConfiguration<Uso>
    {
        public UsoMap()
        {
            ToTable("topsys.con_uso");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.Segmentacao)
                .HasColumnName("id_segmentacao");

        }
    }
}

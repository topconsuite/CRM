using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class SlumpMap:EntityTypeConfiguration<Slump>
    {
        public SlumpMap()
        {
            ToTable("topsys.con_slump");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.AmplitudeDe)
                .HasColumnName("amplitude_de");

            Property(t => t.Variacao)
                .HasColumnName("variavao");

        }
    }
}

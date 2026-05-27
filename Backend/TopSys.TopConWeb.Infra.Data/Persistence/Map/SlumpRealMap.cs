using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class SlumpRealMap : EntityTypeConfiguration<SlumpReal>
    {
        public SlumpRealMap()
        {
            ToTable("topsys.view_slump_real");

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

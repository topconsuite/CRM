using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ResistenciaTipoMap:EntityTypeConfiguration<ResistenciaTipo>
    {
        public ResistenciaTipoMap()
        {
            ToTable("topsys.con_tipo_resistencia");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.Abreviatura)
                .HasColumnName("abrev");

            Property(t => t.Vinculo)
                .HasColumnName("mpa_cons");
        }
    }
}

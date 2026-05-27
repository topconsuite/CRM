using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class AprovacaoTipoMap : EntityTypeConfiguration<AprovacaoTipo>
    {
        public AprovacaoTipoMap()
        {
            ToTable("topsys.con_tipo_aprov");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");

        }
    }
}

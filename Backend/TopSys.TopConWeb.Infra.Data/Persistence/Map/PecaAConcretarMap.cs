using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PecaAConcretarMap : EntityTypeConfiguration<PecaAConcretar>
    {
        public PecaAConcretarMap()
        {
            ToTable("topsys.con_peca_concretar");

            HasKey(t => t.Descricao);

            Property(t => t.Descricao)
                .HasColumnOrder(0)
                .HasColumnName("descricao");
        }
    }
}

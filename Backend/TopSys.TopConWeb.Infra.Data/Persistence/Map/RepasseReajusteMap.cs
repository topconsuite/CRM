using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class RepasseReajusteMap : EntityTypeConfiguration<RepasseReajuste>
    {
        public RepasseReajusteMap()
        {
            ToTable("topsys.con_repas_reajuste");

            HasKey(t => new { t.DataInicioValidade, t.ProdutoCodigo });

            Property(t => t.DataInicioValidade)
                .HasColumnOrder(0)
                .HasColumnName("dt_inicio_valid");

            Property(t => t.ProdutoCodigo)
                .HasColumnOrder(1)
                .HasColumnName("produto");

            Property(t => t.PercentualAreia)
                .HasColumnName("areia");

            Property(t => t.PercentualCimento)
                .HasColumnName("cimento");

            Property(t => t.PercentualDiesel)
                .HasColumnName("diesel");

            Property(t => t.PercentualMaoDeObra)
                .HasColumnName("mao_obra");

            Property(t => t.PercentualPedra)
                .HasColumnName("pedra");
        }
    }
}

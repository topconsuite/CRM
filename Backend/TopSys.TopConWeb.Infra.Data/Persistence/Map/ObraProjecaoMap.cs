using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraProjecaoMap : EntityTypeConfiguration<ObraProjecao>
    {
        public ObraProjecaoMap()
        {
            ToTable("topsys.con_obras_projecao");

            HasKey(t => new { t.Usina, t.NoObra, t.AnoChamada, t.NoChamada, t.Periodo });

            Ignore(x => x.Obra);

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.NoObra)
                .HasColumnName("no_obra");

            Property(t => t.AnoChamada)
                .HasColumnName("ano_chamada");

            Property(t => t.NoChamada)
                .HasColumnName("no_chamada");

            Property(t => t.Periodo)
                .HasColumnName("periodo");

            Property(t => t.Volume)
                .HasColumnName("volume_m3");

            Property(t => t.Saldo)
                .HasColumnName("saldo_m3");
        }
    }
}

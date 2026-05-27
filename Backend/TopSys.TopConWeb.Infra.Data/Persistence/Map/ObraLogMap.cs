using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraLogMap:EntityTypeConfiguration<ObraLog>
    {
        public ObraLogMap()
        {
            ToTable("topsys.con_obras_log");

            HasKey(t => new { t.UsinaCodigo, t.ObraCodigo, t.DataHora, t.Usuario, t.Evento, t.AnoChamada, t.NumChamada, t.Sequencia});
            
            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(1)
                .HasColumnName("obra");
            
            Property(t => t.DataHora)
                .HasColumnOrder(2)
                .HasColumnName("dt_hora_evento");
          
             Property(t => t.Usuario)
                 .HasColumnOrder(3)
                 .HasColumnName("usuario");

             Property(t => t.Evento)
                 .HasColumnOrder(4)
                 .HasColumnName("evento");

             Property(t => t.AnoChamada)
                 .HasColumnOrder(5)
                 .HasColumnName("ano_chamada");

             Property(t => t.NumChamada)
                 .HasColumnOrder(6)
                 .HasColumnName("no_chamada");
            
            Property(t => t.Sequencia)
                .HasColumnOrder(7)
                .HasColumnName("seq_log");
            
            Property(t => t.Complemento)
                .HasColumnName("complemento");
       
            Property(t => t.Observacao)
                .HasColumnName("obs")
                .HasColumnType("text");
        }
    }
}

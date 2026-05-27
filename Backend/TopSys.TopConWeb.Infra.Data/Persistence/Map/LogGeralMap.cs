using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LogGeralMap : EntityTypeConfiguration<LogGeral>
    {
        public LogGeralMap()
        {
            ToTable("topsys.ger_log");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(t => t.Local)
                .HasColumnName("local");

            Property(t => t.Data)
                .HasColumnName("data");

            Property(t => t.Hora)
                .HasColumnName("hora");

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.Tabela)
                .HasColumnName("tabela");

            Property(t => t.Script)
                .HasColumnName("script");

            Property(t => t.AtualizouServidor)
                .HasColumnName("at");
        }
    }
}

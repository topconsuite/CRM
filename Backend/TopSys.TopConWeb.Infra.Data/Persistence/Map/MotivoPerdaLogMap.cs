using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class MotivoPerdaLogMap : EntityTypeConfiguration<MotivoPerdaLog>
    {
        public MotivoPerdaLogMap()
        {
            ToTable("topsys.con_motivo_perda_log");

            HasKey(t => new { t.Tipo, t.DataHoraEvento, t.Usuario, t.Evento });

            Property(t => t.Tipo)
                .HasColumnOrder(0)
                .HasColumnName("tipo")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.DataHoraEvento)
                .HasColumnOrder(1)
                .HasColumnName("dt_hora_evento")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Usuario)
                .HasColumnOrder(2)
                .HasColumnName("usuario")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Evento)
                .HasColumnOrder(3)
                .HasColumnName("evento")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Complemento)
                .HasColumnName("complemento");
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class VisitaLogMap : EntityTypeConfiguration<VisitaLog>
    {
        public VisitaLogMap()
        {
            ToTable("topsys.con_visita_log");

            HasKey(t => new { t.Tipo, t.DataHoraEvento, t.Usina, t.Evento });
            HasIndex(t => new { t.Usina, t.Ano, t.Numero });

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.Ano)
                .HasColumnOrder(1)
                .HasColumnName("ano_visita");

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnName("num_visita");

            Property(t => t.Tipo)
                .HasColumnOrder(3)
                .HasColumnName("tipo");

            Property(t => t.DataHoraEvento)
                .HasColumnOrder(4)
                .HasColumnName("dt_hora_evento");

            Property(t => t.Usuario)
                .HasColumnOrder(5)
                .HasColumnName("usuario");

            Property(t => t.Evento)
                .HasColumnOrder(6)
                .HasColumnName("evento");

            Property(t => t.Complemento)
                .HasColumnOrder(7)
                .HasColumnName("complemento");
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades
{
    public class OportunidadeLogMap : EntityTypeConfiguration<OportunidadeLog>
    {
        public OportunidadeLogMap()
        {
            ToTable("topsys.con_oportunidade_log");

            HasKey(t => new { t.Tipo, t.DataHoraEvento, t.Usina, t.Evento });
            HasIndex(t => new { t.Usina, t.Ano, t.Numero });

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.Ano)
                .HasColumnOrder(1)
                .HasColumnName("ano_oportunidade");

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnName("num_oportunidade");

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

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    class CompromissoLogMap : EntityTypeConfiguration<CompromissoLog>
    {
        public CompromissoLogMap()
        {
            ToTable("topsys.con_compromisso_log");

            HasKey(t => new { t.CodigoCompromisso, t.DataHoraEvento, t.Usuario, t.Evento });

            Property(t => t.CodigoCompromisso)
                .HasColumnOrder(0)
                .HasColumnName("codigo_compromisso");

            Property(t => t.DataHoraEvento)
                .HasColumnOrder(1)
                .HasColumnName("dt_hora_evento");

            Property(t => t.Usuario)
                .HasColumnOrder(2)
                .HasColumnName("usuario");

            Property(t => t.Evento)
                .HasColumnOrder(3)
                .HasColumnName("evento");

            Property(t => t.Descricao)
                .HasColumnName("descricao_compromisso");

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.AnoVisita)
                .HasColumnName("ano_visita");

            Property(t => t.NumeroVisita)
                .HasColumnName("numero_visita");

            Property(t => t.AnoLead)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnName("numero_lead");

            Property(t => t.AnoOportunidade)
                .HasColumnName("ano_oportunidade");

            Property(t => t.NumeroOportunidade)
                .HasColumnName("numero_oportunidade");

            Property(t => t.Complemento)
                .HasColumnName("complemento");
        }
    }
}
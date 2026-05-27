using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadLogMap : EntityTypeConfiguration<LeadLog>
    {
        public LeadLogMap()
        {
			ToTable("topsys.con_lead_log");

			HasKey(t => new { t.Usina, t.AnoLead, t.NumeroLead, t.Sequencia });

			Property(t => t.Usina)
				.HasColumnOrder(0)
				.HasColumnName("usina");

			Property(t => t.AnoLead)
				.HasColumnOrder(1)
				.HasColumnName("ano_lead");

			Property(t => t.NumeroLead)
				.HasColumnOrder(2)
				.HasColumnName("numero_lead");

			Property(t => t.Sequencia)
				.HasColumnOrder(3)
				.HasColumnName("sequencia");

			Property(t => t.Tipo)
				.HasColumnOrder(4)
				.HasColumnName("tipo");

			Property(t => t.DataHoraEvento)
				.HasColumnOrder(5)
				.HasColumnName("dt_hora_evento");

			Property(t => t.Usuario)
				.HasColumnOrder(6)
				.HasColumnName("usuario");

			Property(t => t.Evento)
				.HasColumnOrder(7)
				.HasColumnName("evento");

			Property(t => t.Complemento)
				.HasColumnOrder(8)
				.HasColumnName("complemento");
		}
    }
}

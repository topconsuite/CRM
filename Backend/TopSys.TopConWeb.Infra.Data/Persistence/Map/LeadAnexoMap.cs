using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadAnexoMap : EntityTypeConfiguration<LeadAnexo>
    {
        public LeadAnexoMap()
        {
            ToTable("topsys.con_lead_anexo");

            HasKey(t => t.Id);
            HasIndex(t => new { t.Usina, t.AnoLead, t.NumeroLead });

            Property(t => t.Id)
                .HasColumnName("id");

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.AnoLead)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnName("numero_lead");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.DataHora)
                .HasColumnName("data_hora");

            Property(t => t.Arquivo)
                .HasColumnName("arquivo");

        }
    }
}

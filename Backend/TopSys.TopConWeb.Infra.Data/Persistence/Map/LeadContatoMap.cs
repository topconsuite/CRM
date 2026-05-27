using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadContatoMap : EntityTypeConfiguration<LeadContato>
    {
        public LeadContatoMap()
        {
            ToTable("topsys.con_lead_contato");

            HasKey(t => new { t.Usina, t.AnoLead, t.NumeroLead, t.Sequencia });

            HasOptional(t => t.Funcao)
             .WithMany()
             .HasForeignKey(t => t.FuncaoCodigo)
             .WillCascadeOnDelete(false);

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.AnoLead)
                .HasColumnOrder(1)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnOrder(2)
                .HasColumnType("umediumint")
                .HasColumnName("numero_lead")
                .HasDatabaseGeneratedOption((DatabaseGeneratedOption.None));

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("sequencia");

            Property(t => t.FuncaoCodigo)
                .HasColumnName("funcao");

            Property(t => t.Ddd)
                .HasColumnName("ddd");

            Property(t => t.Telefone)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnName("email");
        }
    }
}

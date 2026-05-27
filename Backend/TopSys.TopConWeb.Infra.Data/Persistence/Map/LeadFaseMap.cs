using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadFaseMap : EntityTypeConfiguration<LeadFase>
    {
        public LeadFaseMap()
        {
            ToTable("topsys.con_fase_lead");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descricao");
        }
    }
}

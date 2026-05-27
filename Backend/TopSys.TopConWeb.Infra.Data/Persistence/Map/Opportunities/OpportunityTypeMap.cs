using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Opportunities
{
    public class OpportunityTypeMap : EntityTypeConfiguration<OpportunityType>
    {

        public OpportunityTypeMap()
        {

            ToTable("topsys.tpe_opportunity_type");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("id")
                .IsRequired();

            Property(t => t.Description)
                .HasColumnOrder(1)
                .HasColumnName("description");


            Property(o => o.DeletedAt)
                .HasColumnOrder(15)
                .HasColumnName("deleted_at")
                .IsOptional();

            Property(o => o.Deleted)
                .HasColumnOrder(16)
                .HasColumnName("deleted")
                .IsOptional();


        }


    }
}

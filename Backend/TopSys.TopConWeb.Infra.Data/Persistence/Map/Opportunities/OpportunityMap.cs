using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Opportunities
{
    public class OpportunityMap : EntityTypeConfiguration<Opportunity>
    {

        public OpportunityMap()
        {

            ToTable("topsys.reg_opportunity");

            HasKey(o => o.Id);

            Property(o => o.Id)
                .HasColumnOrder(0)
                .HasColumnName("id")
                .IsRequired();

            Property(o => o.OpportunityName)
                .HasColumnOrder(1)
                .HasColumnName("opportunity_name")
                .HasMaxLength(200);

            Property(o => o.CustomerId)
                .HasColumnOrder(2)
                .HasColumnName("id_customer")
                .IsRequired();

            Property(o => o.UserOpportunityOwnerId)
                .HasColumnOrder(3)
                .HasColumnName("id_user_opportunity_owner")
                .IsRequired();

            Property(o => o.OpportunityTypeId)
                .HasColumnOrder(4)
                .HasColumnName("id_opportunity_type")
                .IsOptional();

            Property(o => o.OpportunityStage)
                .HasColumnOrder(5)
                .HasColumnName("opportunity_stage");

            Property(o => o.OpportunityFailureReasonId)
                .HasColumnOrder(6)
                .HasColumnName("id_opportunity_failure_reason")
                .IsOptional();

            Property(o => o.ClosingDate)
                .HasColumnOrder(7)
                .HasColumnName("closing_date")
                .IsRequired();

            Property(o => o.OpportunityClassification)
                .HasColumnOrder(8)
                .HasColumnName("opportunity_classification")
                .IsRequired();

            Property(o => o.EstimatedQuantityM3)
                .HasColumnOrder(9)
                .HasColumnName("estimated_quantity_m3");

            Property(o => o.OpportunityOriginId)
                .HasColumnOrder(10)
                .HasColumnName("id_opportunity_origin")
                .IsOptional();

            Property(o => o.NextStage)
                .HasColumnOrder(11)
                .HasColumnName("next_stage")
                .HasMaxLength(200);

            Property(o => o.EstimatedValue)
                .HasColumnOrder(12)
                .HasColumnName("estimated_value");

            Property(o => o.CreatedAt)
                .HasColumnOrder(13)
                .HasColumnName("created_at")
                .IsRequired();

            Property(o => o.UpdatedAt)
                .HasColumnOrder(14)
                .HasColumnName("updated_at")
                .IsOptional();

            Property(o => o.DeletedAt)
                .HasColumnOrder(15)
                .HasColumnName("deleted_at")
                .IsOptional();

            Property(o => o.Deleted)
                .HasColumnOrder(16)
                .HasColumnName("deleted")
                .IsOptional();

            Property(o => o.ConstructionName)
                .HasColumnOrder(17)
                .HasColumnName("construction_name")
                .HasMaxLength(200)
                .IsOptional();

            Property(o => o.ConcreteBatchingPlantId)
                .HasColumnOrder(18)
                .HasColumnName("concrete_batching_plant");

            Property(o => o.DistanceKmConcreteBatchingPlant)
                .HasColumnOrder(19)
                .HasColumnName("distance_km_concrete_batching_plant");

            Property(o => o.ConstructionStage)
                .HasColumnOrder(20)
                .HasColumnName("construction_stage");

            Property(o => o.ConstructionSize)
                .HasColumnOrder(21)
                .HasColumnName("construction_size");

            Property(o => o.AddressStreet)
                .HasColumnOrder(22)
                .HasColumnName("address_street")
                .IsOptional();

            Property(o => o.AddressNumber)
                .HasColumnOrder(23)
                .HasColumnName("address_number")
                .IsOptional();

            Property(o => o.AddressDistrict)
                .HasColumnOrder(24)
                .HasColumnName("address_district")
                .IsOptional();

            Property(o => o.AddressCityId)
                .HasColumnOrder(25)
                .HasColumnName("id_address_city")
                .IsOptional();

            Property(o => o.AddressReference)
                .HasColumnOrder(26)
                .HasColumnName("address_reference");

            Property(o => o.ExpectedConstructionStartDate)
                .HasColumnOrder(27)
                .HasColumnName("expected_construction_start_date")
                .IsOptional();

            Property(o => o.ExpectedConstructionCompletionDate)
                .HasColumnOrder(28)
                .HasColumnName("expected_construction_completion_date")
                .IsOptional();

            Property(o => o.ContactName1)
                .HasColumnOrder(29)
                .HasColumnName("contact_name_1")
                .HasMaxLength(70)
                .IsOptional();

            Property(o => o.ContactPosition1)
                .HasColumnOrder(30)
                .HasColumnName("contact_position_1")
                .HasMaxLength(70)
                .IsOptional();

            Property(o => o.ContactPhone1)
                .HasColumnOrder(31)
                .HasColumnName("contact_phone_1")
                .HasMaxLength(20)
                .IsOptional();

            Property(o => o.ContactName2)
                .HasColumnOrder(32)
                .HasColumnName("contact_name_2")
                .HasMaxLength(70)
                .IsOptional();

            Property(o => o.ContactPosition2)
                .HasColumnOrder(33)
                .HasColumnName("contact_position_2")
                .HasMaxLength(70)
                .IsOptional();

            Property(o => o.ContactPhone2)
                .HasColumnOrder(34)
                .HasColumnName("contact_phone_2")
                .HasMaxLength(20)
                .IsOptional();

            Property(o => o.OpportunityNote)
                .HasColumnOrder(35)
                .HasColumnName("opportunity_note")
                .HasMaxLength(200)
                .IsOptional();

            HasRequired(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId);

            HasOptional(o => o.OpportunityType)
                .WithMany()
                .HasForeignKey(o => o.OpportunityTypeId);

            HasOptional(o => o.OpportunityFailureReason)
                .WithMany()
                .HasForeignKey(o => o.OpportunityFailureReasonId);

            HasOptional(o => o.OpportunityOrigin)
                .WithMany()
                .HasForeignKey(o => o.OpportunityOriginId);

            HasRequired(o => o.ConcreteBatchingPlant)
                .WithMany()
                .HasForeignKey(o => o.ConcreteBatchingPlantId);

            HasOptional(o => o.AddressCity)
                .WithMany()
                .HasForeignKey(o => o.AddressCityId);


        }

    }
}

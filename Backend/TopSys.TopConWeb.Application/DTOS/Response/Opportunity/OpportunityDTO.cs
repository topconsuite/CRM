using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Opportunity
{
    public class OpportunityTypeDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; } = false;
    }

    public class OpportunityFailureReasonDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; } = false;
    }

    public class OpportunityOriginDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; } = false;
    }

    public class OpportunityDTO
    {
        public Guid Id { get; set; }
        public string OpportunityName { get; set; }

        public int CustomerId { get; set; }
        public virtual IntervenienteDTO Customer { get; set; }

        public string UserOpportunityOwnerId { get; set; }

        public Guid OpportunityTypeId { get; set; }
        public virtual OpportunityTypeDTO OpportunityType { get; set; }

        public Guid? OpportunityFailureReasonId { get; set; }
        public virtual OpportunityFailureReasonDTO OpportunityFailureReason { get; set; }

        public Guid? OpportunityOriginId { get; set; }
        public virtual OpportunityOriginDTO OpportunityOrigin { get; set; }

        public DateTime ClosingDate { get; set; }

        public EOpportunityStage OpportunityStage { get; set; }
        public EOpportunityClassification OpportunityClassification { get; set; }

        public float EstimatedQuantityM3 { get; set; }

        public string NextStage { get; set; }
        public decimal EstimatedValue { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; } = false;

        public string ConstructionName { get; set; }

        public int ConcreteBatchingPlantId { get; set; }
        public virtual UsinaDTO ConcreteBatchingPlant { get; set; }

        public float DistanceKmConcreteBatchingPlant { get; set; }
        public EConstructionStage ConstructionStage { get; set; }
        public EConstructionSize ConstructionSize { get; set; }

        public string AddressStreet { get; set; }
        public string AddressNumber { get; set; }
        public string AddressDistrict { get; set; }

        public int AddressCityId { get; set; }
        public virtual MunicipioDTO AddressCity { get; set; }

        public string AddressReference { get; set; }

        public DateTime? ExpectedConstructionStartDate { get; set; }
        public DateTime? ExpectedConstructionCompletionDate { get; set; }

        public string ContactName1 { get; set; } = "";
        public string ContactPosition1 { get; set; } = "";
        public string ContactPhone1 { get; set; } = "";

        public string ContactName2 { get; set; } = "";
        public string ContactPosition2 { get; set; } = "";
        public string ContactPhone2 { get; set; } = "";

        public string OpportunityNote { get; set; } = "";
    }
}

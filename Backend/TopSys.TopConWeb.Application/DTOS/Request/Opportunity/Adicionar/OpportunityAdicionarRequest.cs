using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Adicionar
{

    public class OpportunityTypeAdicionarRequest
    {
        public string Description { get; set; }
    }

    public class OpportunityFailureReasonAdicionarRequest
    {
        public string Description { get; set; }
    }

    public class OpportunityOriginAdicionarRequest
    {
        public string Description { get; set; }
    }


    public class OpportunityAdicionarRequest
    {

        public string OpportunityName { get; set; }

        public int CustomerId { get; set; }

        public string UserOpportunityOwnerId { get; set; }

        public Guid? OpportunityTypeId { get; set; }

        public Guid? OpportunityFailureReasonId { get; set; }

        public Guid? OpportunityOriginId { get; set; }

        public DateTime ClosingDate { get; set; }

        public EOpportunityStage OpportunityStage { get; set; }
        public EOpportunityClassification OpportunityClassification { get; set; }

        public float EstimatedQuantityM3 { get; set; }

        public string NextStage { get; set; }
        public decimal EstimatedValue { get; set; }

        public string ConstructionName { get; set; }

        public int ConcreteBatchingPlantId { get; set; }

        public float DistanceKmConcreteBatchingPlant { get; set; }
        public EConstructionStage ConstructionStage { get; set; }
        public EConstructionSize ConstructionSize { get; set; }

        public string AddressStreet { get; set; } = "";
        public string AddressNumber { get; set; } = "";
        public string AddressDistrict { get; set; } = "";

        public int AddressCityId { get; set; } = 0;

        public string AddressReference { get; set; } = "";

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

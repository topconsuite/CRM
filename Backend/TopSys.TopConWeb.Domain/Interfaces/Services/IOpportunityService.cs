using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IOpportunityService : IServiceBase<Opportunity>
    {
        void AdicionarOpportunityType(OpportunityType opportunityType);
        void AdicionarOpportunityFailureReason(OpportunityFailureReason opportunityFailureReason);
        void AdicionarOpportunityOrigin(OpportunityOrigin opportunityOrigin);

        IEnumerable<OpportunityType> ListarOpportunityTypes();
        IEnumerable<OpportunityFailureReason> ListarOpportunityFailureReasons();
        IEnumerable<OpportunityOrigin> ListarOpportunityOrigins();

        OpportunityType ObterOpportunityTypePorId(Guid id);
        OpportunityFailureReason ObterOpportunityFailureReasonPorId(Guid id);
        OpportunityOrigin ObterOpportunityOriginPorId(Guid id);

        Opportunity ObterPorId(Guid id);
        IEnumerable<Opportunity> Listar();
        PagedList<Opportunity> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter);
    }
}

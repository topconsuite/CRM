using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IOpportunityRepository : IRepositoryBase<Opportunity>
    {

        void AdicionarOpportunityType(OpportunityType opportunityType);
        void AdicionarOpportunityFailureReason(OpportunityFailureReason opportunityFailureReason);
        void AdicionarOpportunityOrigin(OpportunityOrigin opportunityOrigin);

        IEnumerable<OpportunityType> ListarOpportunityTypes(bool tracking = false);
        IEnumerable<OpportunityFailureReason> ListarOpportunityFailureReasons(bool tracking = false);
        IEnumerable<OpportunityOrigin> ListarOpportunityOrigins(bool tracking = false);

        OpportunityType ObterOpportunityTypePorId(Guid id, bool tracking = false);
        OpportunityFailureReason ObterOpportunityFailureReasonPorId(Guid id, bool tracking = false);
        OpportunityOrigin ObterOpportunityOriginPorId(Guid id, bool tracking = false);

        Opportunity ObterPorId(Guid id, bool tracking = false);
        IEnumerable<Opportunity> Listar(bool tracking = false);
        PagedList<Opportunity> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter);



    }
}

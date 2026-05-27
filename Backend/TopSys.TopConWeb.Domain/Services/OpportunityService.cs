using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class OpportunityService : ServiceBase<Opportunity>, IOpportunityService
    {

        private readonly IOpportunityRepository _opportunityRepository;

        public OpportunityService(IOpportunityRepository opportunityRepository) : base(opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        public void AdicionarOpportunityType(OpportunityType opportunityType)
        {
            _opportunityRepository.AdicionarOpportunityType(opportunityType);
        }

        public void AdicionarOpportunityFailureReason(OpportunityFailureReason opportunityFailureReason)
        {
            _opportunityRepository.AdicionarOpportunityFailureReason(opportunityFailureReason);
        }

        public void AdicionarOpportunityOrigin(OpportunityOrigin opportunityOrigin)
        {
            _opportunityRepository.AdicionarOpportunityOrigin(opportunityOrigin);
        }

        public IEnumerable<OpportunityType> ListarOpportunityTypes()
        {
            return _opportunityRepository.ListarOpportunityTypes(false);
        }

        public IEnumerable<OpportunityFailureReason> ListarOpportunityFailureReasons()
        {
            return _opportunityRepository.ListarOpportunityFailureReasons(false);
        }

        public IEnumerable<OpportunityOrigin> ListarOpportunityOrigins()
        {
            return _opportunityRepository.ListarOpportunityOrigins(false);
        }

        public OpportunityType ObterOpportunityTypePorId(Guid id)
        {
            return _opportunityRepository.ObterOpportunityTypePorId(id, true);
        }

        public OpportunityFailureReason ObterOpportunityFailureReasonPorId(Guid id)
        {
            return _opportunityRepository.ObterOpportunityFailureReasonPorId(id, true);
        }

        public OpportunityOrigin ObterOpportunityOriginPorId(Guid id)
        {
            return _opportunityRepository.ObterOpportunityOriginPorId(id, true);
        }

        public Opportunity ObterPorId(Guid id)
        {
            return _opportunityRepository.ObterPorId(id, true);
        }

        public IEnumerable<Opportunity> Listar()
        {
            return _opportunityRepository.Listar();
        }

        public PagedList<Opportunity> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter)
        {
            return _opportunityRepository.ListarPorPagina(pagina, porPagina, filter);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class OpportunityRepository : RepositoryBase<Opportunity>, IOpportunityRepository
    {
        public OpportunityRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public void AdicionarOpportunityFailureReason(OpportunityFailureReason opportunityFailureReason)
        {
            _context.OpportunityFailureReason.Add(opportunityFailureReason);
        }

        public void AdicionarOpportunityOrigin(OpportunityOrigin opportunityOrigin)
        {
            _context.OpportunityOrigin.Add(opportunityOrigin);
        }

        public void AdicionarOpportunityType(OpportunityType opportunityType)
        {
            _context.OpportunityType.Add(opportunityType);
        }

        public IEnumerable<Opportunity> Listar(bool tracking = false)
        {
            return _context.Opportunity
                .Include(o => o.Customer)
                .Include(o => o.OpportunityType)
                .Include(o => o.OpportunityFailureReason)
                .Include(o => o.OpportunityOrigin)
                .Include(o => o.ConcreteBatchingPlant)
                .Include(o => o.AddressCity)
                .Where(o => o.Deleted == false)
                .Tracking(tracking)
                .ToList();
        }

        public IEnumerable<OpportunityFailureReason> ListarOpportunityFailureReasons(bool tracking = false)
        {
            return _context.OpportunityFailureReason
                .Where(o => o.Deleted == false)
                .Tracking(tracking)
                .ToList();
        }

        public IEnumerable<OpportunityOrigin> ListarOpportunityOrigins(bool tracking = false)
        {
            return _context.OpportunityOrigin
                .Where(o => o.Deleted == false)
                .Tracking(tracking)
                .ToList();
        }

        public IEnumerable<OpportunityType> ListarOpportunityTypes(bool tracking = false)
        {
            return _context.OpportunityType
                .Where(o => o.Deleted == false)
                .Tracking(tracking)
                .ToList();
        }

        public PagedList<Opportunity> ListarPorPagina(int pagina, int porPagina, Expression<Func<Opportunity, bool>> filter)
        {
            var pagedList = _context.Opportunity
                .Include(o => o.Customer)
                .Include(o => o.OpportunityType)
                .Include(o => o.OpportunityFailureReason)
                .Include(o => o.OpportunityOrigin)
                .Include(o => o.ConcreteBatchingPlant)
                .Include(o => o.AddressCity)
                .Where(o => o.Deleted == false)
                .Where(filter)
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public OpportunityFailureReason ObterOpportunityFailureReasonPorId(Guid id, bool tracking = false)
        {

            return _context.OpportunityFailureReason
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();

        }

        public OpportunityOrigin ObterOpportunityOriginPorId(Guid id, bool tracking = false)
        {
            return _context.OpportunityOrigin
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public OpportunityType ObterOpportunityTypePorId(Guid id, bool tracking = false)
        {
            return _context.OpportunityType
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Opportunity ObterPorId(Guid id, bool tracking = false)
        {
            return _context.Opportunity
                .Include(o => o.Customer)
                .Include(o => o.OpportunityType)
                .Include(o => o.OpportunityFailureReason)
                .Include(o => o.OpportunityOrigin)
                .Include(o => o.ConcreteBatchingPlant)
                .Include(o => o.AddressCity)
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

    }
}

using Dapper;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class LeadContatoRepository : RepositoryBase<LeadContato>, ILeadContatoRepository
    {
        private readonly IdentityHelperService _identityHelperService;

        public LeadContatoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(LeadContato contato)
        {
            var sqlComando = contato.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_lead_contato", sqlComando.ToString());
        }
    }
}

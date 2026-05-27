using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CustoServicoRepository: RepositoryBase<CustoServico>, ICustoServicoRepository
    {
        public CustoServicoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public CustoServico ObterPorUsina(int idUsina)
        {
            var custoServicoResult = _context.CustoServicos
                .Where(cs => cs.UsinaCodigo == idUsina)
                .OrderByDescending(cs => cs.DataInicioVigencia)
                .FirstOrDefault();

            return custoServicoResult;
        }

        public PagedList<CustoServico> ListaEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter)
        {
            var pagedList = _context.CustoServicos
                .OrderByDescending(t => new { t.DataInicioVigencia })
                .Include(t => t.Usina)
                .Where(filter)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }
    }
}

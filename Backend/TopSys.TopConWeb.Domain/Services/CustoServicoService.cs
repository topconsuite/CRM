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
    public class CustoServicoService : ServiceBase<CustoServico>, ICustoServicoService
    {
        private readonly ICustoServicoRepository _custoServicoRepository;

        public CustoServicoService(ICustoServicoRepository custoServicoRepository) : base(custoServicoRepository)
        {
            _custoServicoRepository = custoServicoRepository;
        }

        public PagedList<CustoServico> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter)
        {
            var pagedList = _custoServicoRepository.ListaEmOrdemDecrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}

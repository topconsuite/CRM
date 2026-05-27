using System;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Response.CustoServico;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CustoServicoApplicationService : ApplicationServiceBase<CustoServico>, ICustoServicoApplicationService
    {
        private readonly ICustoServicoService _custoServicoService;

        public CustoServicoApplicationService(ICustoServicoService custoServicoService, IUnitOfWork unityOfWork) : base(custoServicoService, unityOfWork)
        {
            _custoServicoService = custoServicoService;
        }

        public CustoServicoResponse ObterCustoServicoVigentePorUsina(int idUsina)
        {
            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == idUsina).OrderByDescending(t => t.CustoMedioHoraTransporte).FirstOrDefault();
            return AutoMapper.Mapper.Map(custoServicoVigente, new CustoServicoResponse());
        }

        public void Deletar(int idUsina, DateTime dataInicioVigencia)
        {
            var custoServico = _custoServicoService.ObterPorId(idUsina, dataInicioVigencia);
            _custoServicoService.Remover(custoServico);

            Commit();
        }

        public PagedList<CustoServicoResponse> Listar(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_custoServicoService.ListarEmOrdemDecrescente(pagina, porPagina, filter), new PagedList<CustoServicoResponse>());
        }
    }
}

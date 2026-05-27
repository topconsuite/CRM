using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.DemaisServicos;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class DemaisServicosApplicationService: ApplicationServiceBase<DemaisServicos>, IDemaisServicosApplicationService
    {
        private readonly IDemaisServicosService _demaisServicosService;

        public DemaisServicosApplicationService(IDemaisServicosService demaisServicosService, IUnitOfWork unityOfWork)
            :base(demaisServicosService, unityOfWork)
        {
            _demaisServicosService = demaisServicosService;
        }

        public void Adicionar(DemaisServicosInclusaoRequest demaisServicosRequest)
        {
            var newServico = AutoMapper.Mapper.Map(demaisServicosRequest, new DemaisServicos());

            _demaisServicosService.Adicionar(newServico);

            Commit();
        }

        public void Atualizar(DemaisServicosAlteracaoRequest demaisServicosRequest)
        {
            var servicoAntigo = _demaisServicosService.ObterPorId(demaisServicosRequest.Codigo);
            var servicoAtualizado = AutoMapper.Mapper.Map(demaisServicosRequest, servicoAntigo);

            Commit();
        }

        public void Deletar(int idServico)
        {
            var servicoAntigo = _demaisServicosService.ObterPorId(idServico);
            _demaisServicosService.Remover(servicoAntigo);

            Commit();
        }

        public PagedList<DemaisServicosResponse> Listar(int pagina, int porPagina, Expression<Func<DemaisServicos, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_demaisServicosService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<DemaisServicosResponse>());
        }
    }
}

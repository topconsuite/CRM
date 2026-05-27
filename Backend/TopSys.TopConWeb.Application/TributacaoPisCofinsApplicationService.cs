using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.TributacaoPisCofins;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class TributacaoPisCofinsApplicationService : ApplicationServiceBase<TributacaoPisCofins>, ITributacaoPisCofinsApplicationService
    {
        private ITributacaoPisCofinsService _tributacaoPisCofinsService;

        public TributacaoPisCofinsApplicationService(ITributacaoPisCofinsService tributacaoPisCofinsService,
            IUnitOfWork unityOfWork) : base(tributacaoPisCofinsService, unityOfWork)
        {
            _tributacaoPisCofinsService = tributacaoPisCofinsService;
        }

        public IEnumerable<TributacaoPisCofinsResponse> ListarTodosDeSaida()
        {
            return AutoMapper.Mapper.Map(_tributacaoPisCofinsService.ListarTributacoesDeSaida(), new List<TributacaoPisCofinsResponse>());
        }
    }
}
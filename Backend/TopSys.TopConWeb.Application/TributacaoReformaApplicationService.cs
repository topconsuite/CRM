using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.TributacaoReforma;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class TributacaoReformaApplicationService : ApplicationServiceBase<TributacaoReforma>, ITributacaoReformaApplicationService
    {
        private readonly ITributacaoReformaService _tributacaoReformaService;

        public TributacaoReformaApplicationService(ITributacaoReformaService tributacaoReformaService, IUnitOfWork unityOfWork)
                                            : base(tributacaoReformaService, unityOfWork)
        {
            _tributacaoReformaService = tributacaoReformaService;
        }

        public IEnumerable<TributacaoReformaResponse> ListarTodosProducao(string imposto)
        {
            return AutoMapper.Mapper.Map(_tributacaoReformaService.ListarTodosProducao(imposto), new List<TributacaoReformaResponse>());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Response.TipoCobranca;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class TipoCobrancaApplicationService : ApplicationServiceBase<TipoCobranca>, ITipoCobrancaApplicationService
    {
        private readonly ITipoCobrancaService _tipoCobrancaService;

        public TipoCobrancaApplicationService(ITipoCobrancaService tipoCobrancaService, IUnitOfWork unityOfWork)
            : base(tipoCobrancaService, unityOfWork)
        {
            _tipoCobrancaService = tipoCobrancaService;
        }

        public IEnumerable<TipoCobrancaResponse> ListarPorCondicaoPagamento(int idCondicaoPagamento)
        {
            return AutoMapper.Mapper.Map(_tipoCobrancaService.ListarPorCondicaoPagamento(idCondicaoPagamento), new List<TipoCobrancaResponse>());
        }
    }
}

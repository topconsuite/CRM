using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.CustoServico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.CustoServico.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.CustoServico;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.API.Converters;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CustoServicoController : BaseController
    {
        private readonly ICustoServicoApplicationService _custoServicoApplicationService;

        public CustoServicoController(ICustoServicoApplicationService custoServicoApplicationService)
        {
            _custoServicoApplicationService = custoServicoApplicationService;
        }

        [HttpPost]
        [Route("v1/custo-servico")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] CustoServicoInclusaoRequest custoServico)
        {
            _custoServicoApplicationService.Adicionar(custoServico);

            return CreateResponse(HttpStatusCode.OK, "Custo de Serviço Adicionado com Sucesso");
        }

        [HttpPatch]
        [Route("v1/custo-servico")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] CustoServicoAlteracaoRequest custoServico)
        {
            _custoServicoApplicationService.Atualizar(custoServico);

            return CreateResponse(HttpStatusCode.OK, "Custo de Serviço Alterado com Sucesso");
        }

        [HttpDelete]
        [Route("v1/custo-servico/usina/{idUsina}/dataInicioVigencia/{dataInicioVigencia}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int idUsina,string dataInicioVigencia)
        {
            var dataFormatada = DateTime.Parse(dataInicioVigencia);
            _custoServicoApplicationService.Deletar(idUsina, dataFormatada);

            return CreateResponse(HttpStatusCode.OK, "Custo de Serviço deletado com Sucesso");
        }

        [HttpGet]
        [Route("v1/custo-servico")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] CustoServicoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<CustoServico>(request.Filter);

            var pagedList = _custoServicoApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/custo-servico/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterCustoServicoVigentePorUsina(int idUsina)
        {
            var custoServicoVigente = _custoServicoApplicationService.ObterCustoServicoVigentePorUsina(idUsina);

            return CreateResponse(HttpStatusCode.OK, custoServicoVigente);
        }

    }
}
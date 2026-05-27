using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecao;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ObraProjecaoController : BaseController
    {

        private readonly IObraProjecaoApplicationService _obraProjecaoApplicationService;

        public ObraProjecaoController(
            IObraProjecaoApplicationService obraProjecaoApplicationService)
        {
            _obraProjecaoApplicationService = obraProjecaoApplicationService;
        }



        [HttpGet]
        [Route("v1/obra-projecao/{obraUsina}/{obraNumero}")]
        public Task<HttpResponseMessage> ListarPorObra(int obraUsina, int obraNumero)
        {
            var result = _obraProjecaoApplicationService.ListarPorObra(obraUsina, obraNumero);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/obra-projecao")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] ObraProjecaoRequest request)
        {
            _obraProjecaoApplicationService.Adicionar(User.Identity.Name, request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Projeção Adicionada com Sucesso");
        }

        [HttpPatch]
        [Route("v1/obra-projecao")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] ObraProjecaoRequest request)
        {
            _obraProjecaoApplicationService.Atualizar(User.Identity.Name, request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Projeção Alterada com Sucesso");
        }

        [HttpGet]
        [Route("v1/saldo-obra-projecao/usina/{usina}/noObra/{noObra}/anoChamada/{anoChamada}/noChamada/{noChamada}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var response = _obraProjecaoApplicationService.ObterSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/previsao-saldo-obra-projecao/usina/{usina}/noObra/{noObra}/anoChamada/{anoChamada}/noChamada/{noChamada}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var response = _obraProjecaoApplicationService.ObterPrevisaoSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/proximo-periodo-obra-projecao/usina/{usina}/noObra/{noObra}/anoChamada/{anoChamada}/noChamada/{noChamada}")]
        [Authorize]
        public Task<HttpResponseMessage> GetProximoPeriodoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var response = _obraProjecaoApplicationService.GetProximoPeriodoPorContrato(usina, noObra, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
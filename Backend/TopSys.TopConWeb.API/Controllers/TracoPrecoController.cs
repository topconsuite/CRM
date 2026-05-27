using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.TracoPreco;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Application.DTOS.Request.TracoPreco.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Obra;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TracoPrecoController : BaseController
    {
        private readonly ITracoPrecoApplicationService _tracoPrecoApplicationService;
        private readonly IPreTracoPrecoApplicationService _preTracoPrecoApplicationService;

        public TracoPrecoController(ITracoPrecoApplicationService tracoPrecoApplicationService)
        {
            _tracoPrecoApplicationService = tracoPrecoApplicationService;
        }

        [HttpGet]
        [Route("v1/tracoPrecos/data/{data}/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorDataUsina(DateTime data, int idUsina)
        {
            var tracoPrecos = _tracoPrecoApplicationService.ListarPorDataUsina(data, idUsina);

            return CreateResponse(HttpStatusCode.OK, tracoPrecos);
        }

        [HttpGet]
        [Route("v1/tracoPrecos")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] TracoPrecoPagedRequest request)
        {

            var urlFilter = UrlFilterParser.Parse<TracoPreco>(request.Filter);

            var tracoPrecos = _tracoPrecoApplicationService.ListarPorDataUsinaPagina(request.Data, request.Usina, request.Pagina, request.PorPagina, request.Segmentacao, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, tracoPrecos);

        }

        [HttpPatch]
        [Route("v1/tracoPreco")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] TracoPrecoAlteracaoRequest tracoPreco)
        {

            _tracoPrecoApplicationService.Atualizar(User.Identity.Name, tracoPreco);

            return CreateResponse(HttpStatusCode.OK, "Alteração realizada com sucesso.");

        }


        [HttpGet]
        [Route("v1/tracoPreco/numero-tabela-vigente/data/{dataBase}/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            var numeroTabela = _tracoPrecoApplicationService.ObterNumeroTabelaVigentePorDataBaseUsina(dataBase, idUsina);

            return CreateResponse(HttpStatusCode.OK, numeroTabela);
        }

        [HttpPost]
        [Route("v1/tracoPreco/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}/mpa/{mpa}/consumo/{consumo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorDataUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, [FromBody] Obra obra)
        {
            var tracoPreco = _tracoPrecoApplicationService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, obra);

            return CreateResponse(HttpStatusCode.OK, tracoPreco);
        }

        [HttpGet]
        [Route("v1/tracoPreco/usina/{idUsina}/volume/{volume}/preco-unitario-tabela/{precoUnitarioTabela}/valor-adicional")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela)
        {
            var valorAdicional = _tracoPrecoApplicationService.ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(idUsina, volume, precoUnitarioTabela);

            return CreateResponse(HttpStatusCode.OK, valorAdicional);
        }

        [HttpGet]
        [Route("v1/tracoPreco/numeracoesProduto/usina/{idUsina}/segmentacao/{idSegmentacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarNumeracoesProdutoPorDataUsina(int idUsina, int idSegmentacao)
        {
            var numeracoesProduto = _tracoPrecoApplicationService.ListarNumeracoesProdutoPorNumeroTabelaUsina(idUsina, idSegmentacao);

            return CreateResponse(HttpStatusCode.OK, numeracoesProduto) ;
        }

        [HttpGet]
        [Route("v1/tracoPreco/numeracoesProduto")]
        [Authorize]
        public Task<HttpResponseMessage> ListarNumeracoesProduto()
        {
            var numeracoesProduto = _tracoPrecoApplicationService.ListarNumeracoesProduto();

            return CreateResponse(HttpStatusCode.OK, numeracoesProduto);
        }

        [HttpPost]
        [Route("v1/tracoPreco/status/data/{dataBase}/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> VerificarObraPossuiTracoStatusCustoVirtual(int idUsina, DateTime dataBase, [FromBody] Obra obra)
        {
            var obraPossuiStatusCustoVirtual = _tracoPrecoApplicationService.VerificarObraPossuiTracoStatusCustoVirtual(idUsina, dataBase, obra);

            return CreateResponse(HttpStatusCode.OK, obraPossuiStatusCustoVirtual);
        }

        [HttpPost]
        [Route("v1/tracoPreco/status/usina/{idUsina}/numeracaoProduto/{numeracaoProduto}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterStatusPorNumeracaoProduto(int idUsina, int numeracaoProduto, [FromBody] Obra obra)
        {
            var statusTraco = _tracoPrecoApplicationService.ObterStatusPorNumeracaoProduto(idUsina, numeracaoProduto, obra);

            return CreateResponse(HttpStatusCode.OK, statusTraco);
        }

        [HttpPost]
        [Route("v1/tracoPreco/usina/{idUsina}/numeracaoProduto/{numeracaoProduto}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorNumeracaoProduto(int idUsina, int numeracaoProduto, [FromBody] Obra obra)
        {
            var tracoPreco = _tracoPrecoApplicationService.ObterPorNumeracaoProduto(idUsina, numeracaoProduto, obra);

            return CreateResponse(HttpStatusCode.OK, tracoPreco);
        }

        [HttpGet]
        [Route("v1/tracoPreco/usos/usina/{idUsina}/segmentacao/{idSegmentacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsosPorDataUsina(int idUsina, int idSegmentacao)
        {
            var usos = _tracoPrecoApplicationService.ListarUsosPorNumeroTabelaUsina(idUsina, idSegmentacao);

            return CreateResponse(HttpStatusCode.OK, usos);
        }

        [HttpGet]
        [Route("v1/tracoPreco/pedras/usina/{idUsina}/uso/{idUso}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPedrasPorDataUsinaUso(int idUsina, int idUso)
        {
            var pedras = _tracoPrecoApplicationService.ListarPedrasPorNumeroTabelaUsinaUso(idUsina, idUso);
            
            return CreateResponse(HttpStatusCode.OK, pedras);
        }

        [HttpGet]
        [Route("v1/tracoPreco/slumps/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarSlumpsPorDataUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            var slumps = _tracoPrecoApplicationService.ListarSlumpsPorNumeroTabelaUsinaUsoPedra(idUsina, idUso, idPedra);

            return CreateResponse(HttpStatusCode.OK, slumps);
        }

        [HttpGet]
        [Route("v1/tracoPreco/slumpsNominais/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarSlumpsNominaisPorDataUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            var slumps = _tracoPrecoApplicationService.ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(idUsina, idUso, idPedra);

            return CreateResponse(HttpStatusCode.OK, slumps);
        }

        [HttpGet]
        [Route("v1/tracoPreco/resistenciaTipos/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarResistenciaTiposPorDataUsinaUsoPedraSlump
            (int idUsina, int idUso, int idPedra, int idSlump)
        {
            var resistenciaTipos = _tracoPrecoApplicationService.ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(idUsina, idUso, idPedra, idSlump);

            return CreateResponse(HttpStatusCode.OK, resistenciaTipos);
        }

        [HttpGet]
        [Route("v1/tracoPreco/mpas/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarMpasPorDataUsinaUsoPedraSlumpResistenciaTipo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var mpas = _tracoPrecoApplicationService.ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo);

            return CreateResponse(HttpStatusCode.OK, mpas);
        }

        [HttpGet]
        [Route("v1/tracoPreco/consumos/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarConsumosPorDataUsinaUsoPedraSlumpResistenciaTipo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var consumos = _tracoPrecoApplicationService.ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo);

            return CreateResponse(HttpStatusCode.OK, consumos);
        }

        [HttpGet]
        [Route("v1/tracoPreco/familia/homologados/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}/mpa/{mpa}/consumo/{consumo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {

            var particularidades = _tracoPrecoApplicationService.ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);

            return CreateResponse(HttpStatusCode.OK, particularidades);
        }

        [HttpGet]
        [Route("v1/tracoPreco/pendente-aprovacao/usina/{idUsina}/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}/mpa/{mpa}/consumo/{consumo}")]
        [Authorize]
        public Task<HttpResponseMessage> VerificaTracoPendenteAprovacaoTabelaDeVenda
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {

            var response = _tracoPrecoApplicationService.VerificaTracoPendenteAprovacaoTabelaDeVenda(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);

            return CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpGet]
        [Route("v1/tracoPrecos/usina/{idUsina}/obra/{obraNumero}/contratoNumero/{contratoNumero}/contratoAno/{contratoAno}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPrecosAtuaisPorObra(int idUsina, int obraNumero, int contratoNumero, int contratoAno)
        {
            var response = _tracoPrecoApplicationService.ListarPrecosAtuaisPorObra(idUsina, obraNumero, contratoNumero, contratoAno);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPatch]
        [Route("v1/tracoPreco/atualizarLote")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarLote( [FromBody] TracoPrecoAlteracaoLoteRequest tracoPrecoAlteracao)
        {
            _tracoPrecoApplicationService.AtualizarLote(tracoPrecoAlteracao.Tracos, User.Identity.Name, tracoPrecoAlteracao.Tipo, tracoPrecoAlteracao.Valor);

            return CreateResponse(HttpStatusCode.OK, "Alteração em Lote confirmada com sucesso");

        }

        [HttpGet]
        [Route("v1/tracoPreco/usos-segmentacao/segmentacao/{idSegmentacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsosPorSegmentacao(int idSegmentacao)
        {
            var usos = _tracoPrecoApplicationService.ListarUsosPorSegmentacao(idSegmentacao);

            return CreateResponse(HttpStatusCode.OK, usos);
        }
    }
}
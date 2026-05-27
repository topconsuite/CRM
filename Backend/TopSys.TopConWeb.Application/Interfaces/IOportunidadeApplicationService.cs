using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IOportunidadeApplicationService : IApplicationServiceBase<Oportunidade>
    {

        OportunidadeAdicionarResponse Adicionar(OportunidadeAdicionarRequest request);
        void Atualizar(OportunidadeAtualizarRequest request, string usuario);
        PagedList<OportunidadeResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Oportunidade, bool>> filter);
        OportunidadeResponse ObterPorId(int usina, int ano, int numero);
        PropostaDetalhadaResponse ObterPropostaDeOportunidade(int usina, int ano, int numero);

        List<OportunidadeFaseDTO> ListarFases();
        List<OportunidadeTipoDTO> ListarTiposAtivos();
        List<ConcorrenteDTO> ListarConcorrentesAtivos();

        PagedList<OportunidadeInteracaoResponse> ListarInteracoes(int pagina, int porPagina, Expression<Func<OportunidadeInteracao, bool>> filter);
        void AdicionarInteracao(string usuario, OportunidadeInteracaoAdicionarRequest request);
    }
}

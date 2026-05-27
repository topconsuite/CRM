using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarDadosFiscaisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraBomba;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraDistanciaUsinaCepAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraEngenhariaAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraTraco;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraZMRCAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraBombaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraLogResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendentesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecao;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracoResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracosResponse;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IObraApplicationService : IApplicationServiceBase<Obra>
    {
        PagedList<ObraCarteiraResponse> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter);
        IEnumerable<ObraPendenteResponse> ListaPendentesDeAprovacao(string usuario);

        bool TemAprovacaoPendente(int usina, int numero, int anoChamada, int noChamada);

        ObraPendenteAprovacaoResponse ObtemPendentePorId(int usina, int numero, int anoChamada, int noChamada, int numContrato, int anoContrato, string usuario);

        bool AprovarObraPendente(string usuario, ObraPendenteAprovacaoRequest obra);

        IEnumerable<ObraLogResponse> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada);

        IEnumerable<ObraLogResponse> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada, int anoContrato, int numoContrato);

        IEnumerable<ObraSimplesResponse> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao);

        ObraPagamentosResponse ListarObraPagamentos(int idUsina, int obraNumero, int numeroContrato, int anoContrato);

        void AprovarObraPagamentos(string usuario, ObraPagamentosAprovacaoRequest obra);

        ObraTracosResponse ListarObraTracos(int idUsina, int obraNumero, int numeroContrato, int anoContrato);

        void AprovarEngenharia(string usuario, ObraEngenhariaAprovacaoRequest obra);

        float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato);

        ObraParaAnaliseCadastroResponse ObterObraParaAnaliseCadastro(int idUsina, int obraNumero, int numeroContrato, int anoContrato);

        PontoCargaResponse ObterTempoDescarga(int idUsina);

        void AprovarDistanciaUsinaCep(string usuario, ObraDistanciaUsinaCepAprovacaoRequest obra);

        void AlterarStatusCadastroEAnalista(ObraAlterarStatusCadastroEAnalistaRequest obraAlterarStatusCadastroEAnalistaRequest, string usuario);

        void AlterarDadosFiscais(ObraAlterarDadosFiscaisRequest obra, string usuario);

        void AprovarAutomaticamentePagamentosDaCieloLio(string usuario, int idUsina, int anoChamada, int numeroChamada);

        ObraTracoResponse CalcularEbitdaObraTraco(CalcularEbitdaObraTraco calcularEbitdaObraTraco);

        ObraBombaResponse CalcularEbitdaObraBomba(CalcularEbitdaObraBomba calcularEbitdaObraBomba);

        void AtualizarContratoComVersao(int numVersao, int usina, int anoProposta, int numProposta, ICollection<DTOS.Request.Proposta.Alteracao.ObraTracoDTO> obraTracosRequest = null);

        void AtualizarContratoVersao(int numVersao, int usina, int anoProposta, int numProposta);

        float? ObterConsumoPorTraco(int numeroContrato, int AnoContrato, string tracoResistencia, int tracoPedra, int slumpCodigo, int tracoUso, int slumpVariacao, int interveniente);

        void AprovarZmrc(string usuario, ObraZMRCAprovacaoRequest obra);

        void ReprovarZmrc(string usuario, ObraZMRCAprovacaoRequest obra);

        IEnumerable<ObraProjecaoResponse> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada);

        float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);

        float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada);
        float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);
        float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato);

        void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, IEnumerable<ObraTraco> obraTracos);

        void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, IEnumerable<ObraBomba> obraBombas);

        void ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(int obraNumero, int usina, EObraStatusFinanceiro statusAnterior);

        void ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceiraVersao(int obraNumero, int usina, int numeroVersao, EObraStatusFinanceiro statusAnterior);
    }
}

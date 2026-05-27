using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarDadosFiscaisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraBomba;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraDistanciaUsinaCepAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraEngenhariaAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecaoDTO;
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
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class ObraApplicationService : ApplicationServiceBase<Obra>, IObraApplicationService
    {

        private readonly IObraService _obraService;
        private readonly IPropostaService _propostaService;
        private readonly IDemaisAprovacaoService _demaisAprovacaoService;
        private readonly ICadastroDiversoService _cadastroDiversoService;
        private readonly IContratoService _contratoService;
        private readonly IObraTaxaService _obraTaxaService;
        public readonly IDemaisServicosService _demaisServicosService;
        private readonly IAprovacaoComercialPendenteService _aprovacaoComercialPendenteService;
        private readonly IObraRepository _obraRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly IWebHookApplicationService _webHookApplicationService;

        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IParametroService _parametroService;

        public ObraApplicationService(
            IObraService obraService, 
            IPropostaService propostaService, 
            IDemaisAprovacaoService demaisAprovacaoService, 
            ICadastroDiversoService cadastroDiversoService, 
            IContratoService contratoService, 
            IObraTaxaService obraTaxaService, 
            IDemaisServicosService demaisServicosService,
            IAprovacaoComercialPendenteService aprovacaoComercialPendenteService,
            IObraRepository obraRepository,
            IParametroRepository parametroRepository,
            IAprovacaoComercialService aprovacaoComercialService,
            IWebHookApplicationService webHookApplicationService,
            IComercialLegacyService comercialLegacyService,
            IParametroService parametroService,
        IUnitOfWork unitofWork)
            :base(obraService, unitofWork)
        {
            _obraService = obraService;
            _propostaService = propostaService;
            _demaisAprovacaoService = demaisAprovacaoService;
            _contratoService = contratoService;
            _obraTaxaService = obraTaxaService;
            _demaisServicosService = demaisServicosService;
            _aprovacaoComercialPendenteService = aprovacaoComercialPendenteService;
            _obraRepository = obraRepository;
            _parametroRepository = parametroRepository;
            _aprovacaoComercialService = aprovacaoComercialService;
            _webHookApplicationService = webHookApplicationService;

            _comercialLegacyService = comercialLegacyService;
            _parametroService = parametroService;
        }

        public PagedList<ObraCarteiraResponse> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter)
        {

            var obras = _obraService.ListarObraPorPaginaParaCarteira(pagina, porPagina, filter);
            var obraResponse = AutoMapper.Mapper.Map(obras, new PagedList<ObraCarteiraResponse>());

            return obraResponse;

        }

        public bool AprovarObraPendente(string usuario, ObraPendenteAprovacaoRequest obra)
        {
            var parametroDesativarObrigatorioedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var obraVersao = _obraService.ListarFiltrados<Obra>(t => t.UsinaCodigo == obra.UsinaCodigo
                    && t.Numero == obra.Numero).FirstOrDefault();
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(obraVersao.UsinaCodigo, obraVersao.AnoContrato ?? 0, obraVersao.NumContrato ?? 0);
            var aprovado = false;

            if (versaoAtual == 0)
            {
                var _obra = AutoMapper.Mapper.Map(obra, new Obra());

                _obraService.AprovarObraPendente(usuario, _obra);

                if (Commit())
                {
                    
                    _obraService.FinalizarAprovacaoObraPendente(usuario, _obra);

                    if (obra.DemaisAprovacoes != null)
                    {
                        _demaisAprovacaoService.AtualizarAprovacoes(usuario, _obra.DemaisAprovacoes);

                        Commit();
                    }

                    var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntrega.Codigo);

                    var utilizaAprovacaoComercicalPorAlcada = aprovacaoComercialUsina == null ? false : aprovacaoComercialUsina.Ativo;

                    _obraService.AtualizarStatusComercial(_obra, utilizaAprovacaoComercicalPorAlcada);

                    Commit();

                    var obraBase = _obraRepository
                    .ListarFiltrados<Obra>(t => t.UsinaCodigo == _obra.UsinaCodigo && t.Numero == _obra.Numero)
                    .FirstOrDefault();

                    var contrato = _obraService.ObterPorId<Contrato>(obraBase.UsinaCodigo, obraBase.AnoContrato, obraBase.NumContrato);
                    if (contrato != null
                    && obraBase.StatusComercial == EObraStatusComercial.Aprovado
                    && obraBase.StatusCadastro == EObraStatusCadastro.Aprovado)
                        _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);

                    aprovado = obraBase.StatusComercial == EObraStatusComercial.Aprovado;
                }
            }
            else
            {
                var _obra = AutoMapper.Mapper.Map(obra, new ObraVersao());
                _obra.NumeroVersao = versaoAtual;
                //AtualizarContratoComVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0);
                _obraService.AprovarObraPendente(usuario, _obra);                

                if (Commit())
                {
                    _obraService.FinalizarAprovacaoObraPendente(usuario, _obra);

                    if (obra.DemaisAprovacoes != null)
                    {
                        _demaisAprovacaoService.AtualizarAprovacoes(usuario, _obra.DemaisAprovacoes);

                        Commit();
                    }

                    _obraService.AtualizarStatusComercial(_obra);

                    Commit();

                    var obraBase = _obraRepository
                    .ListarFiltrados<ObraVersao>(t => t.NumeroVersao == _obra.NumeroVersao && t.UsinaCodigo == _obra.UsinaCodigo && t.Numero == _obra.Numero)
                    .FirstOrDefault();

                    using (var scope = new TransactionScope())
                    {
                        AtualizarContratoComVersao(versaoAtual, obraBase.UsinaCodigo, obraBase.AnoChamada ?? 0, obraBase.NumChamada ?? 0);

                        var contrato = _obraService.ObterPorId<Contrato>(obraBase.UsinaCodigo, obraBase.AnoContrato, obraBase.NumContrato);
                        if (contrato != null
                        && obraBase.StatusComercial == EObraStatusComercial.Aprovado
                        && obraBase.StatusCadastro == EObraStatusCadastro.Aprovado)
                        {
                            _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);
                            var contratoAposAprovacao = _contratoService.ListarFiltrados<Contrato>(t => t.Usina == obra.UsinaCodigo
                                && t.Ano == obraBase.AnoContrato && t.Numero == obraBase.NumContrato).FirstOrDefault();

                            if (!_notifications.HasNotifications() && contratoAposAprovacao.Inconsistencias == "")
                            {
                                AtualizarContratoVersao(versaoAtual, obraBase.UsinaCodigo, obraBase.AnoChamada ?? 0, obraBase.NumChamada ?? 0);

                                var obraTracos = AutoMapper.Mapper.Map(obra.ObraTracos, new List<ObraTraco>());
                                foreach (var traco in obraTracos)
                                {
                                    traco.PrecoReajustadoAtual = traco.M3PrecoProposto;
                                }
                                AtualizarValorReajustePropostaItemVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraTracos);

                                var obraBombas = AutoMapper.Mapper.Map(obra.ObraBombas, new List<ObraBomba>());
                                foreach (var bomba in obraBombas)
                                {
                                    bomba.M3ReajustadoAteAtual = bomba.M3PropostoAte;
                                    bomba.TaxaMinimaReajustadaAtual = bomba.TaxaMinimaPrecoProposto;
                                    bomba.M3PrecoReajustadoAtual = bomba.M3PrecoProposto;
                                }
                                AtualizarValorReajustePropostaBombaVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraBombas);

                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                                var contratoVersaoAposAprovacao = _contratoService.ListarFiltradosTracking<ContratoVersao>(t => t.NumeroVersao == versaoAtual && t.Usina == obraBase.UsinaCodigo
                                    && t.Ano == obraBase.AnoContrato && t.Numero == obraBase.NumContrato).FirstOrDefault();
                                contratoVersaoAposAprovacao.Inconsistencias = contratoAposAprovacao.Inconsistencias;

                                if (contratoVersaoAposAprovacao != null)
                                    _comercialLegacyService.ValidarContrato(contratoVersaoAposAprovacao, usuario, out string mensagens, aprovContratoDirAuto);

                                Commit();
                            }
                        }
                        else scope.Dispose();

                        aprovado = obraBase.StatusComercial == EObraStatusComercial.Aprovado;
                    }
                }
            }

            return aprovado;
        }

        public IEnumerable<ObraPendenteResponse> ListaPendentesDeAprovacao(string usuario)
        {
            return AutoMapper.Mapper.Map(_obraService.ListaPendentesDeAprovacao(usuario), new List<ObraPendenteResponse>());
        }

        public bool TemAprovacaoPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            return _obraService.TemAprovacaoPendente(usina, numero, anoChamada, noChamada);
        }

        public IEnumerable<ObraLogResponse> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada, int anoContrato, int numContrato)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(usina, anoContrato, numContrato);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_obraService.ListarObraLogsPorId(usina, numero, anoChamada, noChamada), new List<ObraLogResponse>());
            else return AutoMapper.Mapper.Map(_obraService.ListarObraLogsPorId(versaoAtual, usina, numero, anoChamada, noChamada), new List<ObraLogResponse>());            
        }

        public IEnumerable<ObraLogResponse> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada)
        {
           return AutoMapper.Mapper.Map(_obraService.ListarObraLogsPorId(usina, numero, anoChamada, noChamada), new List<ObraLogResponse>());
        }

        public ObraPendenteAprovacaoResponse ObtemPendentePorId(int usina, int numero, int anoChamada, int noChamada, int numContrato, int anoContrato, string usuario)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(usina, anoContrato, numContrato);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_obraService.ObtemPendentePorId( usina, numero, anoChamada, noChamada, usuario), new ObraPendenteAprovacaoResponse());
            else return AutoMapper.Mapper.Map(_obraService.ObtemPendentePorId(versaoAtual, usina, numero, anoChamada, noChamada, usuario), new ObraPendenteAprovacaoResponse());
        }

        public IEnumerable<ObraSimplesResponse> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao)
        {
            return AutoMapper.Mapper.Map(_obraService.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, obraNumero, numeroCartao, autorizacao), new List<ObraSimplesResponse>());
        }

        public ObraPagamentosResponse ListarObraPagamentos(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(idUsina, anoContrato, numeroContrato);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_obraService.ListarObraPagamentos(idUsina, obraNumero), new ObraPagamentosResponse());
            else return AutoMapper.Mapper.Map(_obraService.ListarObraPagamentos(versaoAtual,idUsina, obraNumero), new ObraPagamentosResponse());
        }

        public void AprovarObraPagamentos(string usuario, ObraPagamentosAprovacaoRequest obra)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(obra.UsinaCodigo, obra.AnoContrato ?? 0, obra.NumContrato ?? 0);
            if (versaoAtual == 0)
            {
                var _obra = AutoMapper.Mapper.Map(obra, new Obra());
                var _movimentosBancoAVincular = AutoMapper.Mapper.Map(obra.MovimentosBancoAVincular, new List<MovimentoBanco>());

                using (var scope = new TransactionScope())
                {
                    _obraService.AprovarObraPagamentos(usuario, _obra, _movimentosBancoAVincular);

                    if (!_notifications.HasNotifications())
                        scope.Complete();
                    else
                        scope.Dispose();
                }
            }
            else
            { 
                var _obra = AutoMapper.Mapper.Map(obra, new ObraVersao());
                _obra.NumeroVersao = versaoAtual;
                var _movimentosBancoAVincular = AutoMapper.Mapper.Map(obra.MovimentosBancoAVincular, new List<MovimentoBanco>());

                using (var scope = new TransactionScope())
                {
                    _obraService.AprovarObraPagamentos(usuario, _obra, _movimentosBancoAVincular);

                    if (!_notifications.HasNotifications())
                        scope.Complete();
                    else
                        scope.Dispose();
                }

                var contratoAtual = _contratoService.ContratoVersaoObterPorId(_obra.NumeroVersao, _obra.UsinaCodigo, _obra.AnoContrato.Value, _obra.NumContrato.Value);
                if (contratoAtual != null)
                {
                    if (contratoAtual.Status == EContratoStatus.Aprovado)
                    {
                        AtualizarContratoComVersao(_obra.NumeroVersao, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0);

                        var obraTracosVersao = _obraService.ListarFiltrados<ObraTracoVersao>(t => t.UsinaCodigo == _obra.UsinaCodigo && t.ObraCodigo == _obra.Numero && t.NumeroVersao == _obra.NumeroVersao);
                        var obraTracos = AutoMapper.Mapper.Map(obraTracosVersao, new List<ObraTraco>());
                        foreach (var traco in obraTracos)
                        {
                            traco.PrecoReajustadoAtual = traco.M3PrecoProposto;
                        }
                        AtualizarValorReajustePropostaItemVersao(_obra.NumeroVersao, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraTracos);

                        var obraBombasVersao = _obraService.ListarFiltrados<ObraBombaVersao>(t => t.UsinaCodigo == _obra.UsinaCodigo && t.ObraCodigo == _obra.Numero && t.NumeroVersao == _obra.NumeroVersao);
                        var obraBombas = AutoMapper.Mapper.Map(obraBombasVersao, new List<ObraBomba>());
                        foreach (var bomba in obraBombas)
                        {
                            bomba.M3ReajustadoAteAtual = bomba.M3PropostoAte;
                            bomba.TaxaMinimaReajustadaAtual = bomba.TaxaMinimaPrecoProposto;
                            bomba.M3PrecoReajustadoAtual = bomba.M3PrecoProposto;
                        }
                        AtualizarValorReajustePropostaBombaVersao(_obra.NumeroVersao, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraBombas);

                        Commit();
                    }
                }
            }
        }

        public ObraTracosResponse ListarObraTracos(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(idUsina, anoContrato, numeroContrato);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_obraService.ListarObraTracos(idUsina, obraNumero), new ObraTracosResponse());
            else return AutoMapper.Mapper.Map(_obraService.ListarObraTracos(versaoAtual, idUsina, obraNumero), new ObraTracosResponse());
        }

        public void AprovarEngenharia(string usuario, ObraEngenhariaAprovacaoRequest obra)
        {
            var _obra = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(_obra.UsinaCodigo, _obra.AnoContrato ?? 0, _obra.NumContrato ?? 0);
            if (versaoAtual == 0)
            {
                _obraService.AprovarEngenharia(usuario, _obra);
                if (Commit())
                {
                    var contrato = _obraService.ObterPorId<Contrato>(_obra.UsinaCodigo, _obra.AnoContrato, _obra.NumContrato);

                    if (_obra.StatusCadastro == EObraStatusCadastro.Aprovado
                    && (_obra.StatusComercial == EObraStatusComercial.Aprovado || _obra.StatusComercial == EObraStatusComercial.NaoNecessita)
                    && (_obra.StatusFinanceiro == EObraStatusFinanceiro.Aprovado || _obra.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita))
                        _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);
                }
            }
            else
            {
                var _obraVersao = _obraService.ObterPorId<ObraVersao>(versaoAtual, obra.UsinaCodigo, obra.Numero);
                _obraService.AprovarEngenharia(usuario, _obraVersao);
                if (Commit())
                {
                    using (var scope = new TransactionScope())
                    {
                        AtualizarContratoComVersao(versaoAtual, _obraVersao.UsinaCodigo, _obraVersao.AnoChamada ?? 0, _obraVersao.NumChamada ?? 0);
                        var contrato = _obraService.ObterPorId<Contrato>(_obraVersao.UsinaCodigo, _obraVersao.AnoContrato, _obraVersao.NumContrato);

                        if (_obraVersao.StatusCadastro == EObraStatusCadastro.Aprovado
                        && (_obraVersao.StatusComercial == EObraStatusComercial.Aprovado || _obraVersao.StatusComercial == EObraStatusComercial.NaoNecessita)
                        && (_obraVersao.StatusFinanceiro == EObraStatusFinanceiro.Aprovado || _obraVersao.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita))
                        {
                            _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);

                            var contratoAposAprovacao = _contratoService.ListarFiltrados<Contrato>(t => t.Usina == _obraVersao.UsinaCodigo
                            && t.Ano == _obraVersao.AnoContrato && t.Numero == _obraVersao.NumContrato).FirstOrDefault();

                            if (!_notifications.HasNotifications() && contratoAposAprovacao.Inconsistencias == "")
                            {
                                AtualizarContratoVersao(versaoAtual, _obraVersao.UsinaCodigo, _obraVersao.AnoChamada ?? 0, _obraVersao.NumChamada ?? 0);
                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                                var contratoVersaoAposAprovacao = _contratoService.ListarFiltradosTracking<ContratoVersao>(t => t.NumeroVersao == versaoAtual && t.Usina == _obraVersao.UsinaCodigo
                                    && t.Ano == _obraVersao.AnoContrato && t.Numero == _obraVersao.NumContrato).FirstOrDefault();
                                contratoVersaoAposAprovacao.Inconsistencias = contratoAposAprovacao.Inconsistencias;
                                Commit();
                            }
                        }
                        else scope.Dispose();
                    }
                }
            }
        }

        public ObraParaAnaliseCadastroResponse ObterObraParaAnaliseCadastro(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(idUsina, anoContrato, numeroContrato);
            if (versaoAtual == 0) return AutoMapper.Mapper.Map(_obraService.ObterObraParaAnaliseCadastro(idUsina, obraNumero), new ObraParaAnaliseCadastroResponse());
            else return AutoMapper.Mapper.Map(_obraService.ObterObraParaAnaliseCadastro(versaoAtual, idUsina, obraNumero), new ObraParaAnaliseCadastroResponse());

        }

        public void AprovarDistanciaUsinaCep(string usuario, ObraDistanciaUsinaCepAprovacaoRequest obra)
        {
            var _obra = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
            _obra.DistanciaUsina = obra.DistanciaUsina;
            _obraService.AprovarDistanciaUsinaCep(usuario, _obra);
            Commit();
        }

        public void AlterarStatusCadastroEAnalista(ObraAlterarStatusCadastroEAnalistaRequest obraAlterarStatusCadastroEAnalistaRequest, string usuario)
        {            
            var _obra = AutoMapper.Mapper.Map(obraAlterarStatusCadastroEAnalistaRequest, new Obra());
            var versaoAtual = _contratoService.GetUltimaVersaoContrato(_obra.UsinaCodigo, _obra.Contrato.Ano, _obra.Contrato.Numero);
            var parametroDesativarObrigatoriedadeAprovacaoComercial = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            if (versaoAtual == 0)
            {
                using (var scope = new TransactionScope())
                {
                    var observacao = obraAlterarStatusCadastroEAnalistaRequest.ObservacaoAcompanhamento;
                    _obraService.AlterarStatusCadastroEAnalista(_obra, observacao, usuario);

                    var _contratoOld = _obraService.ObterPorId<Contrato>(_obra.Contrato.Usina, _obra.Contrato.Ano, _obra.Contrato.Numero);
                    _contratoOld.FaturamentoPendente = obraAlterarStatusCadastroEAnalistaRequest.Contrato.FaturamentoPendente;

                    if (_obra.StatusCadastro != EObraStatusCadastro.Encerrado)
                    {
                        if (_contratoOld != null) _contratoOld.DataEncerramento = null;
                    } else
                    {
                        if (_contratoOld != null)
                        {
                            if (_contratoOld.DataEncerramento == null)
                            {
                                _contratoOld.DataEncerramento = DateTime.Now;
                                _contratoOld.StatusAnterior = _contratoOld.Status;
                                _contratoOld.Status = EContratoStatus.Encerrado;
                            }
                        }  
                    }

                    Commit();

                    if (!_notifications.HasNotifications())
                        scope.Complete();
                    else
                        scope.Dispose();
                }
            }
            else
            {
                var _obraVersao = AutoMapper.Mapper.Map(obraAlterarStatusCadastroEAnalistaRequest, new ObraVersao());
                _obraVersao.NumeroVersao = versaoAtual;

                if (_obraVersao.StatusCadastro == EObraStatusCadastro.Reprovado)
                {
                    using (var scope = new TransactionScope())
                    {
                        var _obraReprovar = _obraService.ObterPorId<ObraVersao>(versaoAtual, _obraVersao.UsinaCodigo, _obraVersao.Numero);
                        AtualizarContratoComVersao(versaoAtual, _obraReprovar.UsinaCodigo, _obraReprovar.AnoChamada ?? 0, _obraReprovar.NumChamada ?? 0);
                        var contratoReprovar = _contratoService.ListarFiltradosTracking<Contrato>(t => t.Usina == _obraReprovar.UsinaCodigo
                                && t.Ano == _obraReprovar.AnoContrato && t.Numero == _obraReprovar.NumContrato).FirstOrDefault();
                        contratoReprovar.Status = EContratoStatus.Reprovado;
                        Commit();
                        AtualizarContratoVersao(versaoAtual, _obraReprovar.UsinaCodigo, _obraReprovar.AnoChamada ?? 0, _obraReprovar.NumChamada ?? 0);
                        scope.Complete();
                    }
                }
                else
                {
                    using (var scope = new TransactionScope())
                    {
                        var observacao = obraAlterarStatusCadastroEAnalistaRequest.ObservacaoAcompanhamento;
                        _obraService.AlterarStatusCadastroEAnalista(_obraVersao, observacao, usuario);

                        var _obraAposAprovar = _obraService.ObterPorId<ObraVersao>(versaoAtual, _obraVersao.UsinaCodigo, _obraVersao.Numero);
                        AtualizarContratoComVersao(versaoAtual, _obraAposAprovar.UsinaCodigo, _obraAposAprovar.AnoChamada ?? 0, _obraAposAprovar.NumChamada ?? 0);
                        var _contratoOld = //_obraService.ObterPorId<Contrato>(_obraAposAprovar.UsinaCodigo, _obraAposAprovar.AnoContrato, _obraAposAprovar.NumContrato);
                        _contratoService.ListarFiltradosTracking<Contrato>(t => t.Usina == _obraAposAprovar.UsinaCodigo
                                && t.Ano == _obraAposAprovar.AnoContrato && t.Numero == _obraAposAprovar.NumContrato).FirstOrDefault();
                        _contratoOld.FaturamentoPendente = obraAlterarStatusCadastroEAnalistaRequest.Contrato.FaturamentoPendente;
                        _contratoOld.IntervenienteCodigo = _obraAposAprovar.Proposta.IntervenienteCodigo;
                        _contratoOld.Interveniente = _obraAposAprovar.Proposta.Interveniente;


                        if (_obraAposAprovar.StatusCadastro == EObraStatusCadastro.Aprovado)
                            _obraService.TentarAprovarCadastroEContrato(_contratoOld, usuario, true);

                        var contratoAposAprovacao = _contratoService.ListarFiltrados<Contrato>(t => t.Usina == _obraAposAprovar.UsinaCodigo
                            && t.Ano == _obraAposAprovar.AnoContrato && t.Numero == _obraAposAprovar.NumContrato).FirstOrDefault();

                        Commit();

                        if (!_notifications.HasNotifications() && contratoAposAprovacao.Inconsistencias == "")
                        {
                            if (_obraAposAprovar.StatusCadastro != EObraStatusCadastro.Encerrado)
                            {
                                _contratoOld.DataEncerramento = null;
                            }
                            else
                            {
                                _contratoOld.DataEncerramento = DateTime.Now;
                                _contratoOld.StatusAnterior = _contratoOld.StatusAnterior;
                                _contratoOld.Status = EContratoStatus.Encerrado;
                            }

                            Commit();
                            
                            AtualizarContratoVersao(versaoAtual, _obraAposAprovar.UsinaCodigo, _obraAposAprovar.AnoChamada ?? 0, _obraAposAprovar.NumChamada ?? 0);

                            if (_obraAposAprovar.StatusCadastro == EObraStatusCadastro.Aprovado)
                            {
                                var obraTracos = AutoMapper.Mapper.Map(_obraAposAprovar.ObraTracos, new List<ObraTraco>());
                                foreach (var traco in obraTracos)
                                {
                                    traco.PrecoReajustadoAtual = traco.M3PrecoProposto;
                                    traco.CustoServicoReajustado = traco.ValorServico;
                                }
                                AtualizarValorReajustePropostaItemVersao(versaoAtual, _obraAposAprovar.UsinaCodigo, _obraAposAprovar.AnoChamada ?? 0, _obraAposAprovar.NumChamada ?? 0, obraTracos);

                                var obraBombas = AutoMapper.Mapper.Map(_obraAposAprovar.ObraBombas, new List<ObraBomba>());
                                foreach (var bomba in obraBombas)
                                {
                                    bomba.M3ReajustadoAteAtual = bomba.M3PropostoAte;
                                    bomba.TaxaMinimaReajustadaAtual = bomba.TaxaMinimaPrecoProposto;
                                    bomba.M3PrecoReajustadoAtual = bomba.M3PrecoProposto;
                                }   
                                AtualizarValorReajustePropostaBombaVersao(versaoAtual, _obraAposAprovar.UsinaCodigo, _obraAposAprovar.AnoChamada ?? 0, _obraAposAprovar.NumChamada ?? 0, obraBombas);
                            }

                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                            var contratoVersaoAposAprovacao = _contratoService.ListarFiltradosTracking<ContratoVersao>(t => t.NumeroVersao == versaoAtual && t.Usina == _obraAposAprovar.UsinaCodigo
                                && t.Ano == _obraAposAprovar.AnoContrato && t.Numero == _obraAposAprovar.NumContrato).FirstOrDefault();
                            contratoVersaoAposAprovacao.Inconsistencias = contratoAposAprovacao.Inconsistencias;

                            var _obraVersaoAposAprovacao = _obraRepository.ListarFiltrados<ObraVersao>(x => x.NumeroVersao == _obraVersao.NumeroVersao && x.UsinaCodigo == _obraVersao.UsinaCodigo && x.Numero == _obraVersao.Numero).FirstOrDefault();

                            if (_obraVersaoAposAprovacao.StatusCadastro != EObraStatusCadastro.Encerrado)
                            {
                                if (contratoVersaoAposAprovacao != null) contratoVersaoAposAprovacao.DataEncerramento = null;
                            }
                            else
                            {
                                if (contratoVersaoAposAprovacao != null)
                                {
                                    if (contratoVersaoAposAprovacao.DataEncerramento == null)
                                    {
                                        contratoVersaoAposAprovacao.DataEncerramento = DateTime.Now;
                                        contratoVersaoAposAprovacao.StatusAnterior = contratoVersaoAposAprovacao.Status;
                                        contratoVersaoAposAprovacao.Status = EContratoStatus.Encerrado;
                                    }
                                }
                            }

                            if ((contratoVersaoAposAprovacao.IsCadastroAprovado() && _obraVersaoAposAprovacao.StatusCadastro == EObraStatusCadastro.Aprovado) || parametroDesativarObrigatoriedadeAprovacaoComercial)
                            {
                                if (_obraVersaoAposAprovacao.StatusComercial == EObraStatusComercial.Aguardando)
                                    contratoVersaoAposAprovacao.Status = EContratoStatus.AguardandoAprovacaoComercial;
                                else if (_obraVersaoAposAprovacao.StatusFinanceiro == EObraStatusFinanceiro.AguardandoConfirmacao)
                                    contratoVersaoAposAprovacao.Status = EContratoStatus.AguardandoConfirmacaoPagamento;
                                else if (_obraVersaoAposAprovacao.StatusFinanceiro == EObraStatusFinanceiro.AguardandoDadosPagamento)
                                    contratoVersaoAposAprovacao.Status = EContratoStatus.AguardandoDadosPagamento;
                            }

                            Commit();
                        }
                    }

                    using (var scope = new TransactionScope())
                    {
                        var observacao = obraAlterarStatusCadastroEAnalistaRequest.ObservacaoAcompanhamento;
                        var _obraNova = AutoMapper.Mapper.Map(obraAlterarStatusCadastroEAnalistaRequest, new Obra());

                        _obraService.AlterarStatusCadastroEAnalista(_obraNova, observacao, usuario);

                        var _obraNovaAprovar = _obraService.ObterPorId<Obra>(_obraNova.UsinaCodigo, _obraNova.Numero);

                        var contratoVersao = _contratoService.ListarFiltrados<ContratoVersao>(t => t.NumeroVersao == versaoAtual && t.Usina == _obra.UsinaCodigo
                                    && t.Ano == _obra.Contrato.Ano && t.Numero == _obra.Contrato.Numero).FirstOrDefault();

                        var contrato = _contratoService.ListarFiltradosTracking<Contrato>(t => t.Usina == _obraNova.UsinaCodigo && t.Ano == _obraNova.Contrato.Ano && t.Numero == _obraNova.Contrato.Numero).FirstOrDefault();
                        if (_obraNova.StatusCadastro != EObraStatusCadastro.Encerrado)
                        {
                            contrato.DataEncerramento = null;
                        }
                        else
                        {
                            contrato.DataEncerramento = DateTime.Now;
                            contrato.StatusAnterior = contratoVersao.StatusAnterior;
                            contrato.Status = EContratoStatus.Encerrado;
                        }

                        Commit();

                        if (!_notifications.HasNotifications())
                            scope.Complete();
                        else
                            scope.Dispose();
                    }
                }
            }            
        }

        public void AlterarDadosFiscais(ObraAlterarDadosFiscaisRequest obra, string usuario)
        {
            using (var scope = new TransactionScope())
            {
                var _obra = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
                var _contrato = _obraService.ObterPorId<Contrato>(_obra.UsinaCodigo, _obra.AnoContrato ?? 0, _obra.NumContrato ?? 0);
                var _proposta = _propostaService.ObterPorUsinaAnoNumero(_obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, true);

                // ObraTributacoesMunicipais
                var usinasTributacaoMunicipal = new List<int>();
                foreach (var tDto in obra.ObraTributacoesMunicipais)
                {
                    usinasTributacaoMunicipal.Add(tDto.UsinaEntregaCodigo);
                    tDto.ObraUsinaCodigo = _obra.UsinaCodigo;
                    tDto.ObraNumero = _obra.Numero;
                    tDto.ContratoAno = _obra.AnoContrato ?? 0;
                    tDto.ContratoNumero = _obra.NumContrato ?? 0;

                    var tribMunOld = _obraService.ObterPorId<ObraTributacaoMunicipal>
                        (tDto.ObraUsinaCodigo, tDto.ObraNumero, tDto.UsinaEntregaCodigo);

                    if (tribMunOld != null)
                    {
                        tribMunOld = AutoMapper.Mapper.Map(tDto, tribMunOld);
                    }
                    else
                    {
                        tribMunOld = AutoMapper.Mapper.Map<ObraTributacaoMunicipal>(tDto);
                        _obraService.Adicionar(tribMunOld);
                    }
                    Commit();
                }

                var usinasTribMun = usinasTributacaoMunicipal.ToArray();
                var usinasTribMunExcluidas = _obraService.ListarFiltradosTracking<ObraTributacaoMunicipal>
                    (t => t.ObraUsinaCodigo == _obra.UsinaCodigo
                        && t.ObraNumero == _obra.Numero
                        && !usinasTribMun.Contains(t.UsinaEntregaCodigo));

                foreach (var t in usinasTribMunExcluidas)
                {
                    _obraService.Remover(t);
                    Commit();
                }

                _obra.ObservacaoNf = obra.ObservacaoNf ?? "";
                _obra.Cei = obra.Cei ?? "";
                Commit();

                _proposta.CodigoObraPrefeitura = obra.Contrato.CodigoObraPrefeitura;
                Commit();
                
                _contrato.CodigoObraPrefeitura = obra.Contrato.CodigoObraPrefeitura;
                Commit();

                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
                
        }

        public void AprovarAutomaticamentePagamentosDaCieloLio(string usuario, int idUsina, int anoChamada, int numeroChamada)
        {
            using (var scope = new TransactionScope())
            {
                var obra = _obraService.ListarFiltradosTracking(t => t.UsinaCodigo == idUsina && t.AnoChamada== anoChamada && t.NumChamada == numeroChamada).FirstOrDefault();

                _obraService.AprovarAutomaticamentePagamentosDaCieloLio(obra);

                _obraService.AtualizarStatusFinanceiro(obra, usuario);

                if (!_notifications.HasNotifications())
                    scope.Complete();
            }
        }

        public ObraTracoResponse CalcularEbitdaObraTraco(CalcularEbitdaObraTraco calcularEbitdaObraTraco)
        {
            
            var obraContrato = _obraService.ObterPorId(calcularEbitdaObraTraco.UsinaCodigo,calcularEbitdaObraTraco.ObraCodigo);

            var numContrato = obraContrato?.NumContrato ?? 0;
            var anoContrato = obraContrato?.AnoContrato ?? 0;
            var usinaCodigo = obraContrato?.UsinaCodigo ?? 0;

            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(usinaCodigo, anoContrato, numContrato);

            if (versaoAtual != 0)
            {
                var obraTracoVersao = AutoMapper.Mapper.Map(calcularEbitdaObraTraco, new ObraTracoVersao());
                var obraVersao = AutoMapper.Mapper.Map(calcularEbitdaObraTraco, new ObraVersao());
                _obraService.CalcularEbitdaObraTraco(obraTracoVersao, obraVersao);
                return AutoMapper.Mapper.Map(obraTracoVersao, new ObraTracoResponse());

            }
            else
            {
                var obraTraco = AutoMapper.Mapper.Map(calcularEbitdaObraTraco, new ObraTraco());
                var obra = AutoMapper.Mapper.Map(calcularEbitdaObraTraco, new Obra());
                _obraService.CalcularEbitdaObraTraco(obraTraco, obra);
                return AutoMapper.Mapper.Map(obraTraco, new ObraTracoResponse());
            }
        }

        public ObraBombaResponse CalcularEbitdaObraBomba(CalcularEbitdaObraBomba calcularEbitdaObraBomba)
        {

            var obraContrato = _obraService.ObterPorId(calcularEbitdaObraBomba.UsinaCodigo, calcularEbitdaObraBomba.ObraCodigo);

            if (obraContrato == null)
            {
                obraContrato = new Obra();
            }

            var numContrato = obraContrato.NumContrato ?? 0;
            var anoContrato = obraContrato.AnoContrato ?? 0;
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(obraContrato.UsinaCodigo, anoContrato, numContrato);

            if (versaoAtual != 0)
            {
                var obraBombaVersao = AutoMapper.Mapper.Map(calcularEbitdaObraBomba, new ObraBomba());
                var obraVersao = AutoMapper.Mapper.Map(calcularEbitdaObraBomba, new Obra());
                _obraService.CalcularEbitdaObraBomba(obraBombaVersao, obraVersao);
                return AutoMapper.Mapper.Map(obraBombaVersao, new ObraBombaResponse());

            }
            else
            {
                var obraBomba = AutoMapper.Mapper.Map(calcularEbitdaObraBomba, new ObraBomba());
                var obra = AutoMapper.Mapper.Map(calcularEbitdaObraBomba, new Obra());
                _obraService.CalcularEbitdaObraBomba(obraBomba, obra);
                return AutoMapper.Mapper.Map(obraBomba, new ObraBombaResponse());
            }
        }

        public float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato)
        {
            var consumo = _obraService.ObterConsumoTracoPorContrato(usinaContrato, numeroContrato, anoContrato, sequenciaTracoContrato);

            if (consumo == null)
                consumo = 0;

            return consumo;
        }

        public float? ObterConsumoPorTraco(int numeroContrato, int anoContrato, string tracoResistencia, int tracoPedra, int slumpCodigo, int tracoUso, int slumpVariacao, int interveniente)
        {
            var stringTracoConcreto = $"{tracoResistencia}/{tracoPedra}/{slumpCodigo}/{tracoUso}/{slumpVariacao}";

            var stringTracoConcretoDecimal = $"{tracoResistencia},0/{tracoPedra}/{slumpCodigo}/{tracoUso}/{slumpVariacao}";

            var consumo = _obraService.ObterConsumoPorTraco(numeroContrato, anoContrato, stringTracoConcreto, interveniente);

            if (consumo == null)
                consumo = _obraService.ObterConsumoPorTraco(numeroContrato, anoContrato, stringTracoConcretoDecimal, interveniente);

            if (consumo == null)
                consumo = 0;

            return consumo;
        }

        public void AtualizarContratoComVersao(int numVersao, int usina, int anoProposta, int numProposta, ICollection<DTOS.Request.Proposta.Alteracao.ObraTracoDTO> obraTracosRequest = null)
        {
            var propostaAprovada = _propostaService.ListarFiltrados<Proposta>
                   (t => t.UsinaCodigo == usina
                       && t.Ano == anoProposta
                       && t.Numero == numProposta).FirstOrDefault();
            var obraAprovada = _obraService.ObterPorId<Obra>(usina, propostaAprovada.ObraCodigo);
            var obraNew = _obraService.ObterPorId<ObraVersao>(numVersao, usina, propostaAprovada.ObraCodigo);

            var usinaEntrega = obraAprovada.UsinaEntregaCodigo;

            if(obraNew != null)
            {
                var usinaValida = obraNew.UsinaEntregaCodigo > 0;
                var usinasDiferentes = obraAprovada.UsinaEntregaCodigo != obraNew.UsinaEntregaCodigo;

                if (usinaValida && usinasDiferentes)
                    usinaEntrega = obraNew.UsinaEntregaCodigo;
            }

            var contratoVersao = _contratoService.ContratoVersaoObterPorId(numVersao, usina, obraAprovada.AnoContrato ?? 0, obraAprovada.NumContrato ?? 0);
            var obraTracos = AutoMapper.Mapper.Map(_obraService.ListarObraTracos(usina, propostaAprovada.ObraCodigo), new ObraTracosResponse());
            var obraBombas = AutoMapper.Mapper.Map(_obraService.ListarObraBombas(usina, propostaAprovada.ObraCodigo), new ObraBombasResponse());

            var pagamentosOld = _contratoService.ObterContratoPagamentos(usina, obraAprovada.NumContrato ?? 0, obraAprovada.AnoContrato ?? 0);
            var pagamentosNew = _contratoService.ObterContratoPagamentosVersao(numVersao, usina, obraAprovada.NumContrato ?? 0, obraAprovada.AnoContrato ?? 0);

            var houveAlteracaoPagamento = false;

            foreach(var pagamentoNew in pagamentosNew)
            {
                var pagamentoOld = pagamentosOld.FirstOrDefault(x => x.Sequencia == pagamentoNew.Sequencia);

                if(pagamentoOld is null)
                {
                    houveAlteracaoPagamento = true;
                    break;
                }

                houveAlteracaoPagamento = houveAlteracaoPagamento || (pagamentoNew.TipoCobranca.Forma != pagamentoOld.TipoCobranca.Forma);
                houveAlteracaoPagamento = houveAlteracaoPagamento || (pagamentoNew.CondicaoPagamentoCodigo != pagamentoOld.CondicaoPagamento.Codigo);
                houveAlteracaoPagamento = houveAlteracaoPagamento || (pagamentoNew.Valor != pagamentoOld.Valor);

                if (houveAlteracaoPagamento)
                    break;
            }

            //if (houveAlteracaoPagamento)
            //    _webHookApplicationService.AdicionarWebHookContratoPagamentoVersao(contratoVersao, pagamentosNew.ToList(), EWebHookTipoEvento.Update);

            _propostaService.ExcluirContrato(usina, anoProposta, numProposta);
            //_contratoService.ExcluirContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value);
            _obraService.ExcluirContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, usina, anoProposta, numProposta, obraAprovada.Numero);
            _obraTaxaService.ExcluirContrato(obraAprovada.UsinaEntregaCodigo, obraAprovada.Numero);
            _demaisServicosService.ExcluirContrato(usina, obraAprovada.Numero);

            _propostaService.AdicionarContrato(usina, anoProposta, numProposta, numVersao);
            _contratoService.AdicionarContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao);
            _obraService.AdicionarContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao, usina, anoProposta, numProposta, obraAprovada.Numero);
            _obraTaxaService.AdicionarContrato(usinaEntrega, numVersao, obraAprovada.Numero);
            _demaisServicosService.AdicionarContrato(usina, numVersao, obraAprovada.Numero);

            using (var scope = new TransactionScope())
            {
                foreach (var tDto in obraTracos.ObraTracos)
                {
                    if (tDto.DataUltimoReajuste != null)
                    {
                        var ultimoReajusteTraco = _contratoService.ListarFiltrados<ContratoTracoReajuste>(t => t.UsinaCodigo == usina && t.ContratoAno == obraAprovada.AnoContrato.Value
                        && t.ContratoNumero == obraAprovada.NumContrato.Value && t.ObraTracoSequencia == tDto.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        if (obraTracosRequest != null)
                        {
                            var obraTraco = obraTracosRequest.Where(t => t.Usina.Codigo == tDto.UsinaCodigo && tDto.ObraCodigo == tDto.ObraCodigo && t.Sequencia == tDto.Sequencia).FirstOrDefault();

                            _obraService.AtualizaObraTracoReajuste(usina, obraAprovada.Numero, tDto.Sequencia, tDto.DataUltimoReajuste.Value, obraTraco.M3PrecoProposto, obraTraco.ValorServico, obraTraco.DescontoPercentual, ultimoReajusteTraco);
                        }
                        else
                        {
                            _obraService.AtualizaObraTracoReajuste(usina, obraAprovada.Numero, tDto.Sequencia, tDto.DataUltimoReajuste.Value, tDto.M3PrecoProposto, tDto.ValorServico, tDto.DescontoPercentual, ultimoReajusteTraco);
                        }
                        Commit();
                    }
                }

                foreach (var tDto in obraBombas.ObraBombas)
                {
                    if (tDto.DataUltimoReajuste != null)
                    {
                        var ultimoReajusteBomba = _contratoService.ListarFiltrados<ContratoBombaReajuste>(t => t.UsinaCodigo == usina && t.ContratoAno == obraAprovada.AnoContrato.Value
                        && t.ContratoNumero == obraAprovada.NumContrato.Value && t.ObraBombaReajusteSequencia == tDto.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        _obraService.AtualizaObraBombaReajuste(usina, obraAprovada.Numero, tDto.Sequencia, tDto.DataUltimoReajuste.Value, tDto.M3PropostoAte, tDto.TaxaMinimaPrecoProposto, tDto.M3PrecoProposto, tDto.DescontoPercentual, ultimoReajusteBomba);

                        Commit();
                    }
                }

                scope.Complete();
            }
            /*var propostaAprovada = _propostaService.ListarFiltrados<Proposta>(t => t.UsinaCodigo == usina
                    && t.Ano == anoProposta
                    && t.Numero == numProposta).FirstOrDefault();
            var cobrancaAprovada = _propostaService.ListarFiltrados<PropostaCobranca>(t => t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();
            var faturamentoAprovado = _propostaService.ListarFiltrados<PropostaFaturamento>(t => t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();
            var responsavelSolidarioAprovado = _propostaService.ListarFiltrados<PropostaResponsavelSolidario>(t => t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();
            var propostaVersao = _propostaService.ListarFiltrados<PropostaVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                    && t.Ano == anoProposta
                    && t.Numero == numProposta).FirstOrDefault();
            var cobrancaVersao = _propostaService.ListarFiltrados<PropostaCobrancaVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();
            var faturamentoVersao = _propostaService.ListarFiltrados<PropostaFaturamentoVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();
            var responsavelSolidarioVersao = _propostaService.ListarFiltrados<PropostaResponsavelSolidarioVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                    && t.PropostaAno == anoProposta
                    && t.PropostaNumero == numProposta).FirstOrDefault();

            //var propostaAtual = new Proposta();
            var propostaAtual = AutoMapper.Mapper.Map(propostaVersao, propostaAprovada);
            propostaAtual.Cobranca = AutoMapper.Mapper.Map(cobrancaVersao, cobrancaAprovada);
            propostaAtual.Faturamento = AutoMapper.Mapper.Map(faturamentoVersao, faturamentoAprovado);
            propostaAtual.ResponsavelSolidario = AutoMapper.Mapper.Map(responsavelSolidarioVersao, responsavelSolidarioAprovado);
            Commit();

            var obraAprovada = _obraService.ObterPorId<Obra>(usina, propostaAprovada.ObraCodigo);
            //_propostaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == usina && t.Numero == propostaAprovada.ObraCodigo).FirstOrDefault();
            var obraVersao = _obraService.ObterPorId<ObraVersao>(numVersao, usina, propostaVersao.ObraCodigo);
           // _propostaService.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina && t.Numero == propostaAprovada.ObraCodigo).FirstOrDefault();
            AutoMapper.Mapper.Map(obraVersao, obraAprovada);
            Commit();

            var anoContrato = obraAprovada.AnoContrato;
            var numContrato = obraAprovada.NumContrato;

            var contratoAprovado = _contratoService.ObterPorId<Contrato>(usina, obraAprovada.AnoContrato, obraAprovada.NumContrato);
            //_contratoService.ListarFiltradosTracking<Contrato>(t => t.Usina == usina && t.Ano == obraAprovada.AnoContrato && t.Numero == obraAprovada.NumContrato).FirstOrDefault();
            var contratoVersao = //_contratoService.ContratoVersaoObterPorId(numVersao, usina, obraAprovada.AnoContrato ?? 0, obraAprovada.NumContrato ?? 0);
                _contratoService.ListarFiltrados<ContratoVersao>(t => t.NumeroVersao == numVersao && t.Usina == usina
                    && t.Ano == obraAprovada.AnoContrato && t.Numero == obraAprovada.NumContrato).FirstOrDefault();
            //var contratoAp = AutoMapper.Mapper.Map(contratoAprovado, contratoVersao);
            AutoMapper.Mapper.Map(contratoVersao, contratoAprovado);
            Commit();
            



            //Traços
            var tracosAprovados = _obraService.ListarFiltradosTracking<ObraTraco>
                (t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo);

            foreach (var t in tracosAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var tracoVersao = _obraService.ListarFiltrados<ObraTracoVersao>(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);
                
            foreach (var tDto in tracoVersao)
            {                    
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraTraco>(tDto));                   
                Commit();
            }

            //Bombas
            var bombasAprovadas = _obraService.ListarFiltradosTracking<ObraBomba>
                (t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo);

            foreach (var t in bombasAprovadas)
            {
                _obraService.Remover(t);
                Commit();
            }

            var bombaVersao = _obraService.ListarFiltrados<ObraBombaVersao>(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);
                
            foreach (var tDto in bombaVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraBomba>(tDto));                    
                Commit();
            }

            // ObraTaxas
            var taxasAprovadas = _obraService.ListarFiltradosTracking<ObraTaxa>
                (t => t.UsinaCodigo == obraVersao.UsinaEntregaCodigo
                    && t.ObraCodigo == propostaAprovada.ObraCodigo);

            foreach (var t in taxasAprovadas)
            {
                _obraService.Remover(t);
                Commit();
            }

            var taxaVersao = _obraService.ListarFiltrados<ObraTaxaVersao>(t => t.UsinaCodigo == obraVersao.UsinaEntregaCodigo
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);               

            foreach (var tDto in taxaVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraTaxa>(tDto));                   
                Commit();
            }

            // Taxas Extra
            var taxasExtraAprovadas = _obraService.ListarFiltradosTracking<TaxaExtra>
                (t => t.UsinaCodigo == obraVersao.UsinaEntregaCodigo
                    && t.ObraCodigo == propostaAprovada.ObraCodigo);

            foreach (var t in taxasExtraAprovadas)
            {
                _obraService.Remover(t);
                Commit();
            }

            var taxaExtraVersao = _obraService.ListarFiltrados<TaxaExtraVersao>(t => t.UsinaCodigo == obraVersao.UsinaEntregaCodigo
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in taxaExtraVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<TaxaExtra>(tDto));                    
                Commit();
            }

            //Tributação Municipal
            var usinasTribMunAprovadas = _obraService.ListarFiltradosTracking<ObraTributacaoMunicipal>
                (t => t.ObraUsinaCodigo == usina
                    && t.ObraNumero == propostaAprovada.ObraCodigo);

            foreach (var t in usinasTribMunAprovadas)
            {
                _obraService.Remover(t);
                Commit();
            }

            var tribMunVersao = _obraService.ListarFiltrados<ObraTributacaoMunicipalVersao>(t => t.UsinaEntregaCodigo == obraVersao.UsinaEntregaCodigo
                    && t.ObraUsinaCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao && t.ObraUsinaCodigo == usina);
                
            foreach (var tDto in tribMunVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraTributacaoMunicipal>(tDto));            
                Commit();
            }

            // Demais Servicos
            var demaisServicosAprovados = _obraService.ListarFiltradosTracking<ObraDemaisServicos>
                (t => t.UsinaCodigo == usina
                    && t.ObraNumero == propostaAprovada.ObraCodigo);

            foreach (var t in demaisServicosAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var demaisServVersao = _obraService.ListarFiltrados<ObraDemaisServicosVersao>
                (t => t.UsinaCodigo == usina
                    && t.ObraNumero == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in demaisServVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraDemaisServicos>(tDto));                    
                Commit();
            }

            // Obra Logs
            var logsAprovados = _obraService.ListarFiltradosTracking<ObraLog>
                (t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo);

            foreach (var t in logsAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var logsVersao = _obraService.ListarFiltrados<ObraLogVersao>
                (t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);
                
            foreach (var tDto in logsVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraLog>(tDto));
                Commit();
            }

            // Obra Mensagem Padrão
            var mensagensAprovadas = _obraService.ListarFiltradosTracking<ObraMensagemPadrao>
                (t => t.UsinaCodigo == usina
                    && t.ObraNumero == propostaAprovada.ObraCodigo);

            foreach (var t in mensagensAprovadas)
            {
                _obraService.Remover(t);
                Commit();
            }

            var mensagemVersao = _obraService.ListarFiltrados<ObraMensagemPadraoVersao>
                (t => t.UsinaCodigo == usina
                    && t.ObraNumero == propostaAprovada.ObraCodigo
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in mensagemVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ObraMensagemPadrao>(tDto));                       
                Commit();
            }

            //Traco Reajuste
            var tracoReajusteAprovados = _obraService.ListarFiltradosTracking<ContratoTracoReajuste>
                (t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato
                    && t.ContratoNumero == numContrato);

            foreach (var t in tracoReajusteAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var tracoReajusteVersao = _obraService.ListarFiltrados<ContratoTracoReajusteVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in tracoReajusteVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoTracoReajuste>(tDto));
                Commit();
            }

            //Bomba Reajuste
            var bombaReajusteAprovados = _obraService.ListarFiltradosTracking<ContratoBombaReajuste>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in bombaReajusteAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }
            var bombaReajusteVersao = _obraService.ListarFiltrados<ContratoBombaReajusteVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in bombaReajusteVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoBombaReajuste>(tDto));
                Commit();
            }

            //Proposta Pagamento
            var propostaPagamentoAprovados = _obraService.ListarFiltradosTracking<PropostaPagamento>
                (t => t.UsinaCodigo == usina && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.PropostaAno == propostaAprovada.Ano && t.PropostaNumero == propostaAprovada.Numero);

            foreach (var t in propostaPagamentoAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var propostaPagamentoVersao = _obraService.ListarFiltrados<PropostaPagamentoVersao>(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == propostaAprovada.ObraCodigo
                    && t.PropostaAno == propostaAprovada.Ano && t.PropostaNumero == propostaAprovada.Numero
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in propostaPagamentoVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<PropostaPagamento>(tDto));                    
                Commit();
            }

            //Contrato Pagamento
            var contratoPagamentoAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoForSaving>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }
            var contratoPagamentoVersao = _obraService.ListarFiltrados<ContratoPagamentoForSavingVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in contratoPagamentoVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoForSaving>(tDto));
                Commit();
            }

            //Contrato Pagamento Detalhe Cartao
            var contratoPagamentoDetalheCartaoAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalheCartao>
                    (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoDetalheCartaoAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentoDetalheCartaoVersao = _obraService.ListarFiltrados<ContratoPagamentoDetalheCartaoVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in contratoPagamentoDetalheCartaoVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoDetalheCartao>(tDto));
                Commit();
            }

            //Contrato Pagamento Detalhe Deposito
            var contratoPagamentoDetalheDepositoAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalheDeposito>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoDetalheDepositoAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentoDetalheDepositoVersao = _obraService.ListarFiltrados<ContratoPagamentoDetalheDepositoVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);
                
            foreach (var tDto in contratoPagamentoDetalheDepositoVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoDetalheDeposito>(tDto));
                Commit();
            }

            //Contrato Pagamento Detalhe Dinheiro
            var contratoPagamentoDetalheDinheiroAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalheDinheiro>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoDetalheDinheiroAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentoDetalheDinheiroVersao = _obraService.ListarFiltrados<ContratoPagamentoDetalheDinheiroVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in contratoPagamentoDetalheDinheiroVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoDetalheDinheiro>(tDto));
                Commit();
            }

            //Contrato Pagamento Detalhe Boletos
            var contratoPagamentoDetalheBoletoAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalheBoleto>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoDetalheBoletoAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentoDetalheBoletoVersao = _obraService.ListarFiltrados<ContratoPagamentoDetalheBoletoVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in contratoPagamentoDetalheBoletoVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoDetalheBoleto>(tDto));
                Commit();
            }

            //Contrato Pagamento Detalhe Cheque
            var contratoPagamentoDetalheChequeAprovados = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalheCheque>
                (t => t.UsinaCodigo == usina && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato);

            foreach (var t in contratoPagamentoDetalheChequeAprovados)
            {
                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentoDetalheChequeVersao = _obraService.ListarFiltrados<ContratoPagamentoDetalheChequeVersao>(t => t.UsinaCodigo == usina
                    && t.ContratoAno == anoContrato && t.ContratoNumero == numContrato
                    && t.NumeroVersao == numVersao);

            foreach (var tDto in contratoPagamentoDetalheChequeVersao)
            {
                _obraService.Adicionar(AutoMapper.Mapper.Map<ContratoPagamentoDetalheCheque>(tDto));
                Commit();
            }*/

        }

        public void AtualizarContratoVersao(int numVersao, int usina, int anoProposta, int numProposta)
        {
            var propostaAprovada = _propostaService.ListarFiltrados<Proposta>
                   (t => t.UsinaCodigo == usina
                       && t.Ano == anoProposta
                       && t.Numero == numProposta).FirstOrDefault();
            var obraAprovada = _obraService.ObterPorId<Obra>(usina, propostaAprovada.ObraCodigo);
            var contratoVersao = _contratoService.ContratoVersaoObterPorId(numVersao, usina, obraAprovada.AnoContrato ?? 0, obraAprovada.NumContrato ?? 0);
            var obraTracos = AutoMapper.Mapper.Map(_obraService.ListarObraTracos(usina, propostaAprovada.ObraCodigo), new ObraTracosResponse());
            var obraBombas = AutoMapper.Mapper.Map(_obraService.ListarObraBombas(usina, propostaAprovada.ObraCodigo), new ObraBombasResponse());

            _propostaService.ExcluirVersaoContrato(usina, anoProposta, numProposta, numVersao);
            _contratoService.ExcluirVersaoContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao);
            _obraService.ExcluirVersaoContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao, usina, anoProposta, numProposta, obraAprovada.Numero);
            _obraTaxaService.ExcluirVersaoContrato(obraAprovada.UsinaEntregaCodigo, numVersao, obraAprovada.Numero);
            _demaisServicosService.ExcluirVersaoContrato(usina, numVersao, obraAprovada.Numero);

            _propostaService.AdicionarVersaoContrato(usina, anoProposta, numProposta, numVersao);
            _contratoService.AdicionarVersaoContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao);
            _obraService.AdicionarVersaoContrato(obraAprovada.UsinaCodigo, obraAprovada.AnoContrato.Value, obraAprovada.NumContrato.Value, numVersao, usina, anoProposta, numProposta, obraAprovada.Numero);
            _obraTaxaService.AdicionarVersaoContrato(obraAprovada.UsinaEntregaCodigo, numVersao, obraAprovada.Numero);
            _demaisServicosService.AdicionarVersaoContrato(usina, numVersao, obraAprovada.Numero);

            using (var scope = new TransactionScope())
            {
                foreach (var tDto in obraTracos.ObraTracos)
                {
                    if (tDto.DataUltimoReajuste != null)
                    {
                        var ultimoReajusteTraco = _contratoService.ListarFiltrados<ContratoTracoReajuste>(t => t.UsinaCodigo == usina && t.ContratoAno == obraAprovada.AnoContrato.Value
                        && t.ContratoNumero == obraAprovada.NumContrato.Value && t.ObraTracoSequencia == tDto.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        _obraService.AtualizaObraTracoReajuste(usina, obraAprovada.Numero, tDto.Sequencia, tDto.DataUltimoReajuste.Value, tDto.M3PrecoProposto, tDto.ValorServico, tDto.DescontoPercentual, ultimoReajusteTraco);

                        Commit();
                    }
                }

                foreach (var tDto in obraBombas.ObraBombas)
                {
                    if (tDto.DataUltimoReajuste != null)
                    {
                        var ultimoReajusteBomba = _contratoService.ListarFiltrados<ContratoBombaReajuste>(t => t.UsinaCodigo == usina && t.ContratoAno == obraAprovada.AnoContrato.Value
                        && t.ContratoNumero == obraAprovada.NumContrato.Value && t.ObraBombaReajusteSequencia == tDto.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        _obraService.AtualizaObraBombaReajuste(usina, obraAprovada.Numero, tDto.Sequencia, tDto.DataUltimoReajuste.Value, tDto.M3PropostoAte, tDto.TaxaMinimaPrecoProposto, tDto.M3PrecoProposto, tDto.DescontoPercentual, ultimoReajusteBomba);

                        Commit();
                    }
                }

                scope.Complete();
            }
        }

        public void AprovarZmrc(string usuario, ObraZMRCAprovacaoRequest obra)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(obra.Contrato.Usina, obra.Contrato.Ano, obra.Contrato.Numero);
            if (versaoAtual == 0)
            {
                var _obra = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
                _obra.NecessitaAprovacaoZMRC = "N";

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = 1,
                    Evento = "APROVAÇÃO ZMRC",
                    Complemento = "ALTERAÇÃO ZMRC DE SIM PARA NÃO",
                    Observacao = ""
                });
            }
            else
            {
                var _obraVersao = _obraService.ListarFiltradosTracking<ObraVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.Numero == obra.Numero
                && t.NumeroVersao == versaoAtual).FirstOrDefault();
                _obraVersao.NecessitaAprovacaoZMRC = "N";
                _obraService.Adicionar(new ObraLogVersao
                {
                    UsinaCodigo = _obraVersao.UsinaCodigo,
                    AnoChamada = _obraVersao.AnoChamada ?? 0,
                    NumChamada = _obraVersao.NumChamada ?? 0,
                    ObraCodigo = _obraVersao.Numero,
                    NumeroVersao = versaoAtual,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = 1,
                    Evento = "APROVAÇÃO ZMRC",
                    Complemento = "ALTERAÇÃO ZMRC DE SIM PARA NÃO",
                    Observacao = ""
                });
            }
            Commit();
        }

        public void ReprovarZmrc(string usuario, ObraZMRCAprovacaoRequest obra)
        {
            var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(obra.Contrato.Usina, obra.Contrato.Ano, obra.Contrato.Numero);
            if (versaoAtual == 0)
            {
                var _obra = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
                _obra.NecessitaAprovacaoZMRC = "N";
                _obra.UsaAdicionalZMRC = "S";

                var taxas = _obraTaxaService.ListarByIdObra(_obra.UsinaEntregaCodigo, _obra.Numero);

                foreach (var tDto in taxas)
                {
                    if (tDto.Tipo == "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
                        _obraTaxaService.MarcarTaxa(tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);
                }

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = 1,
                    Evento = "REPROVAÇÃO ZMRC",
                    Complemento = "REPROVADO A ALTERAÇÃO ZMRC DE SIM PARA NÃO",
                    Observacao = ""
                });
            }
            else
            {
                var _obraVersao = _obraService.ListarFiltradosTracking<ObraVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.Numero == obra.Numero
                && t.NumeroVersao == versaoAtual).FirstOrDefault();
                _obraVersao.NecessitaAprovacaoZMRC = "N";
                _obraVersao.UsaAdicionalZMRC = "S";

                var taxas = _obraTaxaService.ListarByIdObra(_obraVersao.UsinaEntregaCodigo, _obraVersao.Numero, _obraVersao.NumeroVersao);

                foreach (var tDto in taxas)
                {
                    if (tDto.Tipo == "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
                        _obraTaxaService.MarcarTaxa(tDto.NumeroVersao, tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);
                }

                _obraService.Adicionar(new ObraLogVersao
                {
                    UsinaCodigo = _obraVersao.UsinaCodigo,
                    AnoChamada = _obraVersao.AnoChamada ?? 0,
                    NumChamada = _obraVersao.NumChamada ?? 0,
                    ObraCodigo = _obraVersao.Numero,
                    NumeroVersao = versaoAtual,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = 1,
                    Evento = "REPROVAÇÃO ZMRC",
                    Complemento = "REPROVADO A ALTERAÇÃO ZMRC DE SIM PARA NÃO",
                    Observacao = ""
                });
            }
            Commit();
        }

        public IEnumerable<ObraProjecaoResponse> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada)
        {
            return AutoMapper.Mapper.Map(_obraService.ListarProjecaoPorObra(usina, numero, anoChamada, noChamada), new List<ObraProjecaoResponse>());
        }

        public float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraService.ObterConsumoPorContrato(usinaContrato, numeroContrato, anoContrato);

            if (consumo == null)
                consumo = 0;

            return consumo;
        }

        public float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var volume = _obraService.ObterVolumePorContrato(usina, noObra, anoChamada, noChamada);

            if (volume == null)
                volume = 0;

            return volume;
        }

        public float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraService.ObterConsumoAcumuladoPorContrato(usinaContrato, numeroContrato, anoContrato);

            if (consumo == null)
                consumo = 0;

            return consumo;
        }

        public float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraService.ObterConsumoMesAtualPorContrato(usinaContrato, numeroContrato, anoContrato);

            if (consumo == null)
                consumo = 0;

            return consumo;
        }

        public void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, IEnumerable<ObraTraco> obraTracos)
        {
            foreach (var traco in obraTracos)
            {
                var dataUltimoReajuste = traco.DataUltimoReajuste;
                if (dataUltimoReajuste == null)
                {
                    var dbObraTraco = AutoMapper.Mapper.Map(_obraService.ListarObraTracos(usina, traco.ObraCodigo), new ObraTracosResponse());
                    dataUltimoReajuste = dbObraTraco.ObraTracos.Where(x => x.Sequencia == traco.Sequencia).FirstOrDefault().DataUltimoReajuste;
                }
                    
                if (dataUltimoReajuste != null)
                    _obraService.AtualizarValorReajustePropostaItemVersao(numVersao, usina, anoProposta, numProposta, traco.Sequencia, traco.PrecoReajustadoAtual, traco.CustoServicoReajustado, traco.DescontoPercentual);
            }
        }

        public void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, IEnumerable<ObraBomba> obraBombas)
        {
            foreach (var bomba in obraBombas)
            {
                var dataUltimoReajuste = bomba.DataUltimoReajuste;
                if (dataUltimoReajuste == null)
                {
                    var dbObraBomba = AutoMapper.Mapper.Map(_obraService.ListarObraBombas(usina, bomba.ObraCodigo), new ObraBombasResponse());
                    dataUltimoReajuste = dbObraBomba.ObraBombas.Where(x => x.Sequencia == bomba.Sequencia).FirstOrDefault().DataUltimoReajuste;
                }

                if (dataUltimoReajuste != null)
                    _obraService.AtualizarValorReajustePropostaBombaVersao(numVersao, usina, anoProposta, numProposta, bomba.Sequencia, bomba.M3ReajustadoAteAtual, bomba.TaxaMinimaReajustadaAtual, bomba.M3PrecoReajustadoAtual, bomba.DescontoPercentual);
            }
        }

        public void ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(int obraNumero, int usina, EObraStatusFinanceiro statusAnterior)
        {

            var obra = _obraService.ListarFiltrados(t => t.Numero == obraNumero && t.UsinaCodigo == usina).FirstOrDefault();

            if(obra.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita 
                || obra.StatusFinanceiro == EObraStatusFinanceiro.Reprovado 
                || obra.StatusFinanceiro == EObraStatusFinanceiro.Aprovado
                || obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoDadosPagamento
                || obra.NumContrato == 0)
            {
                return;
            }

            _webHookApplicationService.AdicionarWebhookContratoPendenteAprovacaoFinanceira(obra);
        }

        public void ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceiraVersao(int obraNumero, int usina, int numeroVersao, EObraStatusFinanceiro statusAnterior)
        {

            var obra = _obraService.ListarFiltrados<ObraVersao>(t => t.Numero == obraNumero && t.UsinaCodigo == usina && t.NumeroVersao == numeroVersao).FirstOrDefault();

            if (obra.StatusFinanceiro == EObraStatusFinanceiro.NaoNecessita
                || obra.StatusFinanceiro == EObraStatusFinanceiro.Reprovado
                || obra.StatusFinanceiro == EObraStatusFinanceiro.Aprovado
                || obra.StatusFinanceiro == EObraStatusFinanceiro.AguardandoDadosPagamento
                || obra.NumContrato == 0)
            {
                return;
            }

            _webHookApplicationService.AdicionarWebhookContratoPendenteAprovacaoFinanceiraVersao(obra);
        }

        public PontoCargaResponse ObterTempoDescarga(int idUsina)
        {
            var tempoDescarga = _obraService.ObterTempoDescarga(idUsina);

            return new PontoCargaResponse { TempoDescarga = tempoDescarga};
        }
    }
}

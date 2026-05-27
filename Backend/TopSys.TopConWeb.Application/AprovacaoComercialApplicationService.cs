using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracosResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class AprovacaoComercialApplicationService : ApplicationServiceBase<AprovacaoComercialUsina>, IAprovacaoComercialApplicationService
    {

        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly IAprovacaoComercialHierarquiaService _aprovacaoComercialHierarquiaService;
        private readonly IObraApplicationService _obraApplicationService;
        private readonly IAprovacaoComercialPendenteService _aprovacaoComercialPendenteService;
        private readonly IObraService _obraService;
        private readonly IContratoService _contratoService;
        private readonly IDemaisAprovacaoService _demaisAprovacaoService;
        private readonly IObraTaxaService _obraTaxaService;

        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IParametroService _parametroService;

        public AprovacaoComercialApplicationService(
            IAprovacaoComercialService aprovacaoComercialService,
            IAprovacaoComercialHierarquiaService aprovacaoComercialHierarquiaService,
            IAprovacaoComercialPendenteService aprovacaoComercialPendenteService,
            IObraService obraService,
            IContratoService contratoService,
            IObraApplicationService obraApplicationService, 
            IDemaisAprovacaoService demaisAprovacaoService,
            IObraTaxaService obraTaxaService,
            IComercialLegacyService comercialLegacyService,
            IParametroService parametroService,
        IUnitOfWork unityOfWork) : base(aprovacaoComercialService, unityOfWork)
        {
            _aprovacaoComercialService = aprovacaoComercialService;
            _aprovacaoComercialHierarquiaService = aprovacaoComercialHierarquiaService;
            _obraApplicationService = obraApplicationService;
            _obraService = obraService;
            _contratoService = contratoService;
            _aprovacaoComercialPendenteService = aprovacaoComercialPendenteService;
            _demaisAprovacaoService = demaisAprovacaoService;
            _obraTaxaService = obraTaxaService;

            _comercialLegacyService = comercialLegacyService;
            _parametroService = parametroService;
        }

        public void AprovarObraPendente(string usuario, ObraPendenteAprovacaoRequest obra)
        {
            var parametroDesativarObrigatorioedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var aprovContratoDirAuto = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntrega.Codigo);

            var utilizaAprovacaoComercicalPorAlcada = aprovacaoComercialUsina == null ? false : aprovacaoComercialUsina.Ativo;
            var versaoAtual = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);

            if (!utilizaAprovacaoComercicalPorAlcada)
            { // Usa aprovação modelo antigo
                
                var obraAprovado = _obraApplicationService.AprovarObraPendente(usuario, obra);

                if(obraAprovado)
                    _aprovacaoComercialPendenteService.ForcarAprovacaoRegistrosAlcada(obra.UsinaCodigo, obra.Numero, versaoAtual);

                return;
            }

            var usuarioHierarquia = _aprovacaoComercialHierarquiaService.ObterUsuarioNivelHierarquiaPorUsuarioUsina(usuario, aprovacaoComercialUsina.Id);

            if(usuarioHierarquia == null)
            {
                AssertionConcern.Notify("Usuário", $"Usuário não esta cadastrado em nenhuma hierarquia comercial para usina {obra.UsinaEntrega.Codigo}.");
                return;
            }

            var hierarquiaComercialNivel = usuarioHierarquia.AprovacaoComercialHierarquia;

            var reprovado = false;
            var volumeAprovado = false;
            var condicaoPagamentoAprovado = false;

            if(versaoAtual == 0)
            {
                var obraOld = _obraService.ObterPorId(obra.UsinaCodigo, obra.Numero);
                if(obraOld != null)
                {
                    volumeAprovado = obraOld.VolumeStatusComercial == EObraDemaisStatusComercial.Aprovado;
                    condicaoPagamentoAprovado = obraOld.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Aprovado;
                    reprovado = obraOld.StatusComercial == EObraStatusComercial.Reprovado;
                }
            } 
            else
            {
                var obraOld = _obraService.ObterPorId<ObraVersao>(versaoAtual, obra.UsinaCodigo, obra.Numero);
                if (obraOld != null)
                {
                    volumeAprovado = obraOld.VolumeStatusComercial == EObraDemaisStatusComercial.Aprovado;
                    condicaoPagamentoAprovado = obraOld.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Aprovado;
                    reprovado = obraOld.StatusComercial == EObraStatusComercial.Reprovado;
                }
            }

            if(reprovado)
            {

                var payload = new
                {
                    volumeAprovado,
                    condicaoPagamentoAprovado,
                    reprovado,
                    observacao = "OBRA REPROVADA (obraOld.StatusComercial == EObraStatusComercial.Reprovado)"
                };

                _aprovacaoComercialService.AdicionarLog(
                    new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, versaoAtual, "", "AprovacaoComercialApplicationService.AprovarObraPendente", "", PayloadHelper.ConvertToJson(payload))
                    );

                AssertionConcern.Notify("Proposta/Contrato", $"Proposta/Contrato já foi reprovado comercialmente.");
                return;
            }

            var aprovacaoPendente = _aprovacaoComercialPendenteService.ObterAprovacaoPendentePorObraVersaoNivelHierarquia(obra.UsinaCodigo, obra.Numero, versaoAtual, hierarquiaComercialNivel.NivelAutoridade);

            if(aprovacaoPendente == null)
                aprovacaoPendente = _aprovacaoComercialPendenteService.ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(obra.UsinaCodigo, obra.Numero, versaoAtual, hierarquiaComercialNivel.NivelAutoridade);

            // Caso não tenha pendencias ele utiliza o antigo
            if (aprovacaoPendente == null)
            {

                var payload = new
                {
                    volumeAprovado,
                    condicaoPagamentoAprovado,
                    reprovado,
                    observacao = "NÃO POSSUI APROVAÇÃO, APROVAÇÃO ANTIGA ACIONADA."
                };

                _aprovacaoComercialService.AdicionarLog(
                    new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, versaoAtual, "", "AprovacaoComercialApplicationService.AprovarObraPendente", "", PayloadHelper.ConvertToJson(payload))
                    );

                _obraApplicationService.AprovarObraPendente(usuario, obra);
                return;
            }

            // Caso utilize Workflow e tente aprovar status AguardandoAprovacaoNivelAnterior
            if(aprovacaoPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
            {

                var payload = new
                {
                    volumeAprovado,
                    condicaoPagamentoAprovado,
                    reprovado,
                    observacao = "WORKFLOW -> Utiliza Workflow e tentou aprovar status AguardandoAprovacaoNivelAnterior "
                };

                _aprovacaoComercialService.AdicionarLog(
                    new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, versaoAtual, "", "AprovacaoComercialApplicationService.AprovarObraPendente", "", PayloadHelper.ConvertToJson(payload))
                    );

                AssertionConcern.Notify("Ag. Aprov. Hierarquia Inferior", $"Nível de hierarquia {aprovacaoPendente.NivelHierarquia} esta aguardando a aprovação de uma hierarquia inferior.");
                return;
            }

            var tracosPendentes = obra.ObraTracos.Where(x => x.StatusAprovacao != (int)EStatusAprovacao.NaoNecessita);
            var bombasPendentes = obra.ObraBombas.Where(x => x.StatusAprovacao != (int)EStatusAprovacao.NaoNecessita);

            foreach (var traco in tracosPendentes)
            {

                if (aprovacaoPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                    continue;

                var aprovacaoTracos = aprovacaoPendente.Tracos.Where(x => x.ObraSeq == traco.Sequencia).ToList();
                var aprovacaoTracoPendente = aprovacaoTracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao).FirstOrDefault();

                var aprovacaoRealizadaPorEsteUsuario = aprovacaoTracos.Where(x => x.AprovacaoUsuario.Equals(usuario)).FirstOrDefault();

                if(aprovacaoRealizadaPorEsteUsuario != null)
                {

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado)
                        continue;

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                        aprovacaoTracoPendente = aprovacaoRealizadaPorEsteUsuario;

                }

                var novaAprovacao = (aprovacaoTracoPendente == null);

                if (novaAprovacao)
                {
                    
                    aprovacaoTracoPendente = new AprovacaoComercialPendenteTraco()
                    {
                        Id = Guid.NewGuid(),
                        IdAprovacao = aprovacaoPendente.Id,

                        ObraVersao = aprovacaoPendente.ObraVersao,
                        ObraUsina = aprovacaoPendente.ObraUsina,
                        ObraNumero = aprovacaoPendente.ObraNumero,
                        ObraSeq = traco.Sequencia,

                        NivelHierarquia = aprovacaoPendente.NivelHierarquia,
                        AprovacaoSequencia = aprovacaoPendente.Tracos.Count() == 0 ? 0 : aprovacaoPendente.Tracos.Max(x => x.AprovacaoSequencia) + 1
                    };
                }

                switch (traco.StatusAprovacao)
                {
                    case (int)EStatusAprovacao.Aprovado:
                        aprovacaoTracoPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        break;
                    case (int)EStatusAprovacao.Reprovado:
                        aprovacaoTracoPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        break;
                }

                if(aprovacaoTracoPendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                {
                    aprovacaoTracoPendente.AprovacaoUsuario = usuario;
                    aprovacaoTracoPendente.AprovacaoData = DateTime.Now;
                    _aprovacaoComercialPendenteService.AdicionarAprovacaoPendenteTraco(aprovacaoTracoPendente);
                }

            }

            foreach (var bomba in bombasPendentes)
            {

                if (aprovacaoPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                    continue;

                var aprovacaoBombas = aprovacaoPendente.Bombas.ToList().Where(x => x.ObraSeq == bomba.Sequencia).ToList();
                var aprovacaoBombaPendente = aprovacaoBombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao).FirstOrDefault();

                var aprovacaoRealizadaPorEsteUsuario = aprovacaoBombas.Where(x => x.AprovacaoUsuario.Equals(usuario)).FirstOrDefault();

                if(aprovacaoRealizadaPorEsteUsuario != null)
                {

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado)
                        continue;

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                        aprovacaoBombaPendente = aprovacaoRealizadaPorEsteUsuario;

                }


                var novaAprovacao = (aprovacaoBombaPendente == null);

                if (novaAprovacao)
                {

                    aprovacaoBombaPendente = new AprovacaoComercialPendenteBomba()
                    {
                        Id = Guid.NewGuid(),
                        IdAprovacao = aprovacaoPendente.Id,

                        ObraVersao = aprovacaoPendente.ObraVersao,
                        ObraUsina = aprovacaoPendente.ObraUsina,
                        ObraNumero = aprovacaoPendente.ObraNumero,
                        ObraSeq = bomba.Sequencia,

                        NivelHierarquia = aprovacaoPendente.NivelHierarquia,
                        AprovacaoSequencia = aprovacaoPendente.Bombas.Count() == 0 ? 0 : aprovacaoPendente.Bombas.Max(x => x.AprovacaoSequencia) + 1
                    };
                }

                switch (bomba.StatusAprovacao)
                {
                    case (int)EStatusAprovacao.Aprovado:
                        aprovacaoBombaPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        break;
                    case (int)EStatusAprovacao.Reprovado:
                        aprovacaoBombaPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        break;
                }

                if (aprovacaoBombaPendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                {
                    aprovacaoBombaPendente.AprovacaoUsuario = usuario;
                    aprovacaoBombaPendente.AprovacaoData = DateTime.Now;
                    _aprovacaoComercialPendenteService.AdicionarAprovacaoPendenteBomba(aprovacaoBombaPendente);
                }               

            }


            if(!volumeAprovado && obra.VolumeStatusComercial == EObraDemaisStatusComercial.Aprovado && aprovacaoPendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Reprovado)
            {

                var aprovacaoVolumePendente = aprovacaoPendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao).FirstOrDefault();
                var aprovacaoRealizadaPorEsteUsuario = aprovacaoPendente.Volumes.Where(x => x.AprovacaoUsuario.Equals(usuario)).FirstOrDefault();
                var realizarAprovacao = true;

                if (aprovacaoRealizadaPorEsteUsuario != null)
                {

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado)
                        realizarAprovacao = false;

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                        aprovacaoVolumePendente = aprovacaoRealizadaPorEsteUsuario;

                }

                if(realizarAprovacao)
                {

                    var novaAprovacao = (aprovacaoVolumePendente == null);

                    if (novaAprovacao)
                    {

                        aprovacaoVolumePendente = new AprovacaoComercialPendenteVolume()
                        {
                            Id = Guid.NewGuid(),
                            IdAprovacao = aprovacaoPendente.Id,

                            ObraVersao = aprovacaoPendente.ObraVersao,
                            ObraUsina = aprovacaoPendente.ObraUsina,
                            ObraNumero = aprovacaoPendente.ObraNumero,

                            NivelHierarquia = aprovacaoPendente.NivelHierarquia,
                            AprovacaoSequencia = aprovacaoPendente.Tracos.Count() == 0 ? 0 : aprovacaoPendente.Tracos.Max(x => x.AprovacaoSequencia) + 1
                        };
                    }

                    switch (obra.VolumeStatusComercial)
                    {
                        case EObraDemaisStatusComercial.Aprovado:
                            aprovacaoVolumePendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                            break;
                        case EObraDemaisStatusComercial.Reprovado:
                            aprovacaoVolumePendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                            break;
                    }

                    if (aprovacaoVolumePendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                    {
                        aprovacaoVolumePendente.AprovacaoUsuario = usuario;
                        aprovacaoVolumePendente.AprovacaoData = DateTime.Now;
                        _aprovacaoComercialPendenteService.AdicionarAprovacaoPendenteVolume(aprovacaoVolumePendente);
                    }

                }

            }

            if (!condicaoPagamentoAprovado && obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Aprovado && aprovacaoPendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Reprovado)
            {

                var aprovacaoDemaisPendente = aprovacaoPendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao).FirstOrDefault();
                var aprovacaoRealizadaPorEsteUsuario = aprovacaoPendente.CondicoesPagamento.Where(x => x.AprovacaoUsuario.Equals(usuario)).FirstOrDefault();
                var realizarAprovacao = true;

                if (aprovacaoRealizadaPorEsteUsuario != null)
                {

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado)
                        realizarAprovacao = false;

                    if (aprovacaoRealizadaPorEsteUsuario.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                        aprovacaoDemaisPendente = aprovacaoRealizadaPorEsteUsuario;

                }

                if (realizarAprovacao)
                {

                    var novaAprovacao = (aprovacaoDemaisPendente == null);

                    if (novaAprovacao)
                    {

                        aprovacaoDemaisPendente = new AprovacaoComercialPendenteCondicaoPagamento()
                        {
                            Id = Guid.NewGuid(),
                            IdAprovacao = aprovacaoPendente.Id,

                            ObraVersao = aprovacaoPendente.ObraVersao,
                            ObraUsina = aprovacaoPendente.ObraUsina,
                            ObraNumero = aprovacaoPendente.ObraNumero,

                            NivelHierarquia = aprovacaoPendente.NivelHierarquia,
                            AprovacaoSequencia = aprovacaoPendente.Tracos.Count() == 0 ? 0 : aprovacaoPendente.Tracos.Max(x => x.AprovacaoSequencia) + 1
                        };
                    }

                    switch (obra.CondicaoPagamentoStatusComercial)
                    {
                        case EObraDemaisStatusComercial.Aprovado:
                            aprovacaoDemaisPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                            break;
                        case EObraDemaisStatusComercial.Reprovado:
                            aprovacaoDemaisPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                            break;
                    }

                    if (aprovacaoDemaisPendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                    {
                        aprovacaoDemaisPendente.AprovacaoUsuario = usuario;
                        aprovacaoDemaisPendente.AprovacaoData = DateTime.Now;
                        _aprovacaoComercialPendenteService.AdicionarAprovacaoPendenteCondicaoPagamento(aprovacaoDemaisPendente);
                    }

                }

            }

            if (Commit())
            {

                if (versaoAtual == 0)
                {
                    var _obra = _obraService.ObterPorIdAprovacaoComercial(obra.UsinaCodigo, obra.Numero);

                    foreach (var traco in _obra.ObraTracos)
                    {
                        traco.AtualizaStatusAprovacao(usuario);
                    }

                    foreach (var bomba in _obra.ObraBombas)
                    {
                        bomba.AtualizaStatusAprovacao(usuario);
                    }

                    var statusVolume = _obra.VolumeStatusComercial;
                    var statusDemaisCondicao = _obra.CondicaoPagamentoStatusComercial;

                    _aprovacaoComercialPendenteService.ValidaAprovacoesObra(usuario, _obra);

                    var logVolume = 
                               statusVolume != EObraDemaisStatusComercial.NaoNecessita
                            && statusVolume != EObraDemaisStatusComercial.AguardandoAprovacao
                            && statusVolume != _obra.VolumeStatusComercial;

                    var logDemaisCondicao = 
                               statusDemaisCondicao != EObraDemaisStatusComercial.NaoNecessita
                            && statusDemaisCondicao != EObraDemaisStatusComercial.AguardandoAprovacao
                            && statusDemaisCondicao != _obra.CondicaoPagamentoStatusComercial;

                    foreach (var traco in _obra.ObraTracos)
                    {

                        if(traco.StatusAprovacao != EStatusAprovacao.Reprovado)
                            traco.AtualizaStatusAprovacao(usuario);

                        if (traco.StatusAprovacao == EStatusAprovacao.Reprovado)
                        {
                            var tracoRequest = obra.ObraTracos.Where(x => x.Sequencia == traco.Sequencia).FirstOrDefault();

                            if (tracoRequest == null)
                                continue;

                            traco.LogObservacao = tracoRequest.LogObservacao;

                        }

                    }

                    foreach (var bomba in _obra.ObraBombas)
                    {
                        
                        if(bomba.StatusAprovacao != EStatusAprovacao.Reprovado)
                            bomba.AtualizaStatusAprovacao(usuario);
                        
                        if(bomba.StatusAprovacao == EStatusAprovacao.Reprovado)
                        {
                            var bombaRequest = obra.ObraBombas.Where(x => x.Sequencia == bomba.Sequencia).FirstOrDefault();

                            if (bombaRequest == null)
                                continue;

                            bomba.LogObservacao = bombaRequest.LogObservacao;
                        }

                    }

                    if (obra.ObraTaxas != null)
                    {
                        _obra.ObraTaxas = AutoMapper.Mapper.Map(obra.ObraTaxas, new List<ObraTaxa>());
                        _obraTaxaService.AprovarTaxas(usuario, _obra.ObraTaxas);
                    }

                    Commit();

                    var statusObra = new Dictionary<string, EObraStatusComercial>();
                    var statusProposta = new Dictionary<string, EPropostaStatus>();

                    statusObra.Add("1-INICIO", _obra.StatusComercial);
                    statusProposta.Add("1-INICIO", _obra.Proposta.Status);

                    _obraService.FinalizarAprovacaoObraPendente(usuario, _obra, logVolume, logDemaisCondicao);

                    statusObra.Add("2-ObraService.FinalizarAprovacaoObraPendente", _obra.StatusComercial);
                    statusProposta.Add("2-ObraService.FinalizarAprovacaoObraPendente", _obra.Proposta.Status);

                    if (obra.DemaisAprovacoes != null)
                        _demaisAprovacaoService.AtualizarAprovacoes(usuario, AutoMapper.Mapper.Map(obra, new Obra()).DemaisAprovacoes);

                    Commit();

                    _obraService.AtualizarStatusComercial(_obra, utilizaAprovacaoComercicalPorAlcada);

                    statusObra.Add("3-ObraService.AtualizarStatusComercial", _obra.StatusComercial);
                    statusProposta.Add("3-ObraService.AtualizarStatusComercial", _obra.Proposta.Status);

                    _obra = _obraService.ObterPorId(_obra.UsinaCodigo, obra.Numero);

                    statusObra.Add("4-ObraService.ObterPorId", _obra.StatusComercial);
                    statusProposta.Add("4-ObraService.ObterPorId", _obra.Proposta.Status);

                    var contrato = _obraService.ObterPorId<Contrato>(_obra.UsinaCodigo, _obra.AnoContrato, _obra.NumContrato);
                    if (contrato != null
                    && _obra.StatusComercial == EObraStatusComercial.Aprovado
                    && _obra.StatusCadastro == EObraStatusCadastro.Aprovado)
                        _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);

                    statusObra.Add("5-ObraService.TentarAprovarCadastroEContrato", _obra.StatusComercial);
                    statusProposta.Add("5-ObraService.TentarAprovarCadastroEContrato", _obra.Proposta.Status);

                    var payload = new
                    {
                        Obra = _obra,
                        Aprovacoes = aprovacaoPendente,
                        Contrato = contrato,
                        AcompanhamentoStatusObra = statusObra,
                        AcompanhamentoStatusProposta = statusProposta
                    };

                    _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(
                        _obra.UsinaCodigo, _obra.Numero, versaoAtual, "", "AprovacaoComercialApplicationService.AprovarObraPendente", "", PayloadHelper.ConvertToJson(payload)
                        )
                    );

                } else
                {

                    var _obra = _obraService.ObterPorIdAprovacaoComercial(obra.UsinaCodigo, obra.Numero, versaoAtual);

                    foreach (var traco in _obra.ObraTracos)
                    {
                        traco.AtualizaStatusAprovacao(usuario);
                    }

                    foreach (var bomba in _obra.ObraBombas)
                    {
                        bomba.AtualizaStatusAprovacao(usuario);
                    }

                    var statusVolume = _obra.VolumeStatusComercial;
                    var statusDemaisCondicao = _obra.CondicaoPagamentoStatusComercial;

                    _aprovacaoComercialPendenteService.ValidaAprovacoesObraVersao(usuario, _obra);

                    var logVolume =
                               statusVolume != EObraDemaisStatusComercial.NaoNecessita
                            && statusVolume != EObraDemaisStatusComercial.AguardandoAprovacao
                            && statusVolume != _obra.VolumeStatusComercial;

                    var logDemaisCondicao =
                               statusDemaisCondicao != EObraDemaisStatusComercial.NaoNecessita
                            && statusDemaisCondicao != EObraDemaisStatusComercial.AguardandoAprovacao
                            && statusDemaisCondicao != _obra.CondicaoPagamentoStatusComercial;


                    foreach (var traco in _obra.ObraTracos)
                    {
                        if(traco.StatusAprovacao != EStatusAprovacao.Reprovado)
                            traco.AtualizaStatusAprovacao(usuario);
                    }

                    foreach (var bomba in _obra.ObraBombas)
                    {
                        if (bomba.StatusAprovacao != EStatusAprovacao.Reprovado)
                            bomba.AtualizaStatusAprovacao(usuario);
                    }

                    //Aprovação ObraTaxas
                    if (obra.ObraTaxas != null)
                    {
                        _obra.ObraTaxas = AutoMapper.Mapper.Map(obra.ObraTaxas, new List<ObraTaxaVersao>());
                        _obraTaxaService.AprovarTaxas(usuario, _obra.ObraTaxas, versaoAtual);
                    }

                    Commit();

                    var statusObra = new Dictionary<string, EObraStatusComercial>();
                    var statusProposta = new Dictionary<string, EPropostaStatus>();

                    statusObra.Add("1-INICIO", _obra.StatusComercial);
                    statusProposta.Add("1-INICIO", _obra.Proposta.Status);

                    _obraService.FinalizarAprovacaoObraPendente(usuario, _obra, logVolume, logDemaisCondicao);

                    statusObra.Add("2-ObraService.FinalizarAprovacaoObraPendente", _obra.StatusComercial);
                    statusProposta.Add("2-ObraService.FinalizarAprovacaoObraPendente", _obra.Proposta.Status);

                    if (obra.DemaisAprovacoes != null)
                    {
                        _obra.DemaisAprovacoes = AutoMapper.Mapper.Map(obra.DemaisAprovacoes, new List<DemaisAprovacao>());
                        _demaisAprovacaoService.AtualizarAprovacoes(usuario, _obra.DemaisAprovacoes);
                    }

                    Commit();

                    _obraService.AtualizarStatusComercial(_obra, utilizaAprovacaoComercicalPorAlcada);

                    statusObra.Add("3-ObraService.AtualizarStatusComercial", _obra.StatusComercial);
                    statusProposta.Add("3-ObraService.AtualizarStatusComercial", _obra.Proposta.Status);

                    _obra = _obraService.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == versaoAtual
                    && t.UsinaCodigo == _obra.UsinaCodigo
                    && t.Numero == obra.Numero).FirstOrDefault();

                    statusObra.Add("4-ObraService.ObterPorId", _obra.StatusComercial);
                    if (_obra.Proposta != null)
                        statusProposta.Add("4-ObraService.ObterPorId", _obra.Proposta.Status);

                    using (var scope = new TransactionScope())
                    {

                        if (_obra.StatusComercial != EObraStatusComercial.Aprovado)
                            return;
                        
                        _obraApplicationService.AtualizarContratoComVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0);

                        var contrato = _obraService.ObterPorId<Contrato>(_obra.UsinaCodigo, _obra.AnoContrato, _obra.NumContrato);
                        if (contrato != null
                        && _obra.StatusComercial == EObraStatusComercial.Aprovado
                        && _obra.StatusCadastro == EObraStatusCadastro.Aprovado)
                        {
                            _obraService.TentarAprovarCadastroEContrato(contrato, usuario, false);

                            var contratoAposAprovacao = _contratoService.ListarFiltrados<Contrato>(t => t.Usina == obra.UsinaCodigo
                            && t.Ano == _obra.AnoContrato && t.Numero == _obra.NumContrato).FirstOrDefault();

                            statusObra.Add("5-ObraService.TentarAprovarCadastroEContrato", _obra.StatusComercial);
                            if (_obra.Proposta != null)
                                statusProposta.Add("5-ObraService.TentarAprovarCadastroEContrato", _obra.Proposta.Status);

                            var payload = new
                            {
                                Obra = _obra,
                                Aprovacoes = aprovacaoPendente,
                                Contrato = contrato,
                                AcompanhamentoStatusObra = statusObra,
                                AcompanhamentoStatuaProposta = statusProposta
                            };

                            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(
                                _obra.UsinaCodigo, _obra.Numero, versaoAtual, "", "AprovacaoComercialApplicationService.AprovarObraPendente", "", PayloadHelper.ConvertToJson(payload)
                                )
                            );

                            if (!_notifications.HasNotifications() && contratoAposAprovacao.Inconsistencias == "")
                            {
                                _obraApplicationService.AtualizarContratoVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0);

                                var obraTracos = AutoMapper.Mapper.Map(obra.ObraTracos, new List<ObraTraco>());
                                foreach(var traco in obraTracos)
                                {
                                    traco.PrecoReajustadoAtual = traco.M3PrecoProposto;
                                }
                                _obraApplicationService.AtualizarValorReajustePropostaItemVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraTracos);

                                var obraBombas = AutoMapper.Mapper.Map(obra.ObraBombas, new List<ObraBomba>());
                                foreach (var bomba in obraBombas)
                                {
                                    bomba.M3ReajustadoAteAtual = bomba.M3PropostoAte;
                                    bomba.TaxaMinimaReajustadaAtual = bomba.TaxaMinimaPrecoProposto;
                                    bomba.M3PrecoReajustadoAtual = bomba.M3PrecoProposto;
                                }
                                _obraApplicationService.AtualizarValorReajustePropostaBombaVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, obraBombas);

                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                                var contratoVersaoAposAprovacao = _contratoService.ListarFiltradosTracking<ContratoVersao>(t => t.NumeroVersao == versaoAtual && t.Usina == _obra.UsinaCodigo
                                    && t.Ano == _obra.AnoContrato && t.Numero == _obra.NumContrato).FirstOrDefault();
                                contratoVersaoAposAprovacao.Inconsistencias = contratoAposAprovacao.Inconsistencias;

                                if (contratoVersaoAposAprovacao != null)
                                    _comercialLegacyService.ValidarContrato(contratoVersaoAposAprovacao, usuario, out string mensagens, aprovContratoDirAuto);

                                Commit();
                            }
                        }  else scope.Dispose();
                    }

                }

                

            }

        }

        public AprovacaoComercialUsina Adicionar(AprovacaoComercialUsinaInsercaoRequest insercaoRequest)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(insercaoRequest.UsinaId);

            if (aprovacaoComercialUsina != null)
            {
                AssertionConcern.Notify("usina", "Usina já cadastrada.");
                return null;
            }

            aprovacaoComercialUsina = AutoMapper.Mapper.Map<AprovacaoComercialUsinaInsercaoRequest, AprovacaoComercialUsina>(insercaoRequest);

            aprovacaoComercialUsina.Id = Guid.NewGuid();
            aprovacaoComercialUsina.CreatedAt = DateTime.Now;

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_usina", "AprovacaoComercialApplicationService.Adicionar", "", PayloadHelper.ConvertToJson(aprovacaoComercialUsina))
                );

            _aprovacaoComercialService.Adicionar(aprovacaoComercialUsina);

            Commit();

            return aprovacaoComercialUsina;

        }

        public void Atualizar(AprovacaoComercialUsinaAlteracaoRequest alteracaoRequest)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorId(alteracaoRequest.Id, true);

            if (aprovacaoComercialUsina == null)
                AssertionConcern.Notify("usina", "Não encontrado pelo ID informado.");

            _aprovacaoComercialService.AdicionarLog(
                new AprovacaoComercialLog("con_aprovacao_comercial_usina", "AprovacaoComercialApplicationService.Atualizar", "", PayloadHelper.ConvertToJson(aprovacaoComercialUsina, alteracaoRequest))
                );
            aprovacaoComercialUsina = AutoMapper.Mapper.Map(alteracaoRequest, aprovacaoComercialUsina);

            aprovacaoComercialUsina.UpdatedAt = DateTime.Now;

            Commit();

            _aprovacaoComercialService.Atualizar(aprovacaoComercialUsina);

        }

        public PagedList<AprovacaoComercialUsinaResponse> ListarAprovacaoComercialUsina(int pagina, int porPagina, Expression<Func<AprovacaoComercialUsina, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_aprovacaoComercialService.ListarAprovacaoComercialUsina(pagina, porPagina, filter), new PagedList<AprovacaoComercialUsinaResponse>());
        }

        public void AdicionarLog(AprovacaoComercialLog log)
        {
            _aprovacaoComercialService.AdicionarLog(log);
        }

    }
}

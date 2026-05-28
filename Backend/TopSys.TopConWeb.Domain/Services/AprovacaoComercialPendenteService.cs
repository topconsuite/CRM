using System;
using System.Collections.Generic;
using System.Linq;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;

namespace TopSys.TopConWeb.Domain.Services
{
    public class AprovacaoComercialPendenteService : ServiceBase<AprovacaoComercialPendente>, IAprovacaoComercialPendenteService
    {

        private readonly IAprovacaoComercialPendenteRepository _aprovacaoComercialPendenteRepository;
        private readonly IAprovacaoComercialHierarquiaService _aprovacaoComercialHierarquiaService;
        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly IObraService _obraService;

        public AprovacaoComercialPendenteService(
            IAprovacaoComercialPendenteRepository aprovacaoComercialPendenteRepository, 
            IAprovacaoComercialHierarquiaService aprovacaoComercialHierarquiaService,
            IAprovacaoComercialService aprovacaoComercialService,
            ICondicaoPagamentoService condicaoPagamentoService,
            IObraService obraService) : base(aprovacaoComercialPendenteRepository)
        {
            _aprovacaoComercialPendenteRepository = aprovacaoComercialPendenteRepository;
            _aprovacaoComercialHierarquiaService = aprovacaoComercialHierarquiaService;
            _obraService = obraService;
            _condicaoPagamentoService = condicaoPagamentoService;
            _aprovacaoComercialService = aprovacaoComercialService;
        }

        public void RemoverVestigiosAprovacoesAnteriores(int obraUsina, int obraNumero, int obraVersao, int excluirAcimaNivelAutoridade = 0)
        {
            _aprovacaoComercialPendenteRepository.RemoverVestigiosAprovacoesAnteriores(obraUsina, obraNumero, obraVersao, excluirAcimaNivelAutoridade);
        }
        public void ValidarComercialObra(int obraUsina, int obraNumero, string usuario)
        {

            var obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero);
            ValidarComercialObra(obra, usuario);

        }

        

        public void ValidarComercialObra(Obra obra, string usuario)
        {
            var utilizaAprovacaoComercialAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(obra.UsinaEntregaCodigo);
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);

            if (!utilizaAprovacaoComercialAlcada || aprovacaoComercialUsina == null)
                return;

            var obraVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(obra.Proposta.IntervenienteTipo);
            var hierarquiasComercial = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var pendentes = new List<AprovacaoComercialPendente>();

            foreach (var hierarquia in hierarquiasComercial)
            {

                AprovacaoComercialPendente pendente = null;

                foreach (var traco in obra.ObraTracos)
                {
                    pendente = ValidarComercialObraTraco(pendente, traco, obra, obraVersao, tipoPessoa, hierarquia);
                }

                foreach (var bomba in obra.ObraBombas)
                {
                    pendente = ValidarComercialObraBomba(pendente, bomba, obra, obraVersao, tipoPessoa, hierarquia);
                }

                pendente = ValidarComercialObraVolume(pendente, obra, obraVersao, tipoPessoa, hierarquia);

                pendente = ValidarComercialObraCondicaoPagamento(pendente, obra, obraVersao, tipoPessoa, hierarquia);

                if (pendente != null)
                    pendentes.Add(pendente);

            }

            // Valida se o traço é pendente ou não
            foreach (var obraTraco in obra.ObraTracos)
            {
                var pendentesDoTraco = pendentes.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);

                if (pendentesDoTraco)
                {
                    obraTraco.AprovacaoOperacao = "G";
                    obraTraco.AprovacaoVerbal = "N";
                    obraTraco.AprovacaoObservacao = "";
                }
                else
                {
                    obraTraco.AprovacaoOperacao = "";
                    obraTraco.AprovacaoVerbal = "";
                    obraTraco.AprovacaoObservacao = "";
                }

                obraTraco.AtualizaStatusAprovacao(usuario);

            }

            foreach (var obraBomba in obra.ObraBombas)
            {

                var pendentesDoTraco = pendentes.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);

                if (pendentesDoTraco)
                {
                    obraBomba.AprovacaoOperacao = "G";
                    obraBomba.AprovacaoVerbal = "S";
                    obraBomba.AprovacaoObservacao = "";
                }
                else
                {
                    obraBomba.AprovacaoOperacao = "";
                    obraBomba.AprovacaoVerbal = "";
                    obraBomba.AprovacaoObservacao = "";
                }

                obraBomba.AtualizaStatusAprovacao(usuario);

            }

            var possuiPendentesVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();
            if(possuiPendentesVolume)
            {
                obra.VolumeStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;
                obra.StatusComercial = EObraStatusComercial.Aguardando;
            } 
            else
            {
                var novoStatus = EObraDemaisStatusComercial.NaoNecessita;

                if (obra.VolumeStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao)
                    novoStatus = EObraDemaisStatusComercial.Aprovado;

                obra.VolumeStatusComercial = novoStatus;
            }


            var possuiPendenteCondicaoPagamento = pendentes.CondicaoPagamentoNecessitaAprovacaoComercialScope();
            if(possuiPendenteCondicaoPagamento)
            {
                obra.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;
                obra.StatusComercial = EObraStatusComercial.Aguardando;
            }
            else
            {
                var novoStatus = EObraDemaisStatusComercial.NaoNecessita;

                if (obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao)
                    novoStatus = EObraDemaisStatusComercial.Aprovado;

                obra.CondicaoPagamentoStatusComercial = novoStatus;
            }

            if (obra.ObraTracos != null)
                foreach (var obraTracoLog in obra.ObraTracos)
                    _obraService.AdicionarLogPropostaItem(obraTracoLog, "AprovacaoComercialPendenteService.ValidarComercialObra");

            _aprovacaoComercialPendenteRepository.SaveChanges();

            RevisarAprovacaoComercialPendente(obra, pendentes);

            var payLoad = new
            {
                obra,
                aprovacoes = pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, obraVersao, "", "AprovacaoComercialPendenteService.ValidarComercialObra", "", PayloadHelper.ConvertToJson(payLoad)));
            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);
        }

        private AprovacaoComercialPendente ValidarComercialObraTraco(AprovacaoComercialPendente aprovPendente, ObraTraco obraTraco, Obra obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicoes = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id);

            if(!necessitaAprovacaoComercial)
            {
                foreach (var condicao in condicoes)
                {

                    if ((condicao.PercentualDe == 0 && condicao.PercentualAte == 0) && (condicao.ValorDe == 0 && condicao.ValorAte == 0))
                        continue;

                    double valor = 0;
                    double percentual = 0;
                    double valorComparacao = 0;

                    if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaTracos)
                    {

                        valor = (obraTraco.PrecoReajustadoAtual != 0 ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);
                        percentual = obraTraco.DescontoPercentual;
                        valorComparacao = obraTraco.M3PrecoTabela - (obraTraco.PrecoReajustadoAtual != 0 ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);
                    }
                    else if(condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemMCC)
                    {
                        var valorServicoAtual = obraTraco.CustoServicoReajustado == 0 ? obraTraco.ValorServico : obraTraco.CustoServicoReajustado;

                        valor = 0;
                        percentual = 0;
                        valorComparacao = valorServicoAtual - obraTraco.TotalImpostos;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemTransporte)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraTraco.MargemPosTransporte;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.Ebtida)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraTraco.Ebitda;

                    }

                    var valorDeZeradoEValorComparacaoMenorZero = condicao.ValorDe == 0 && valorComparacao < 0;

                    var realizaComparacaoComValorZerado = !(condicao.ValorDe == 0  && condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaTracos);

                    var comparaValor = condicao.ValorDe > 0 && condicao.ValorAte > 0;
                    var comparaPercentual = condicao.PercentualDe > 0 || condicao.PercentualAte > 0;

                    var percentualAtende = percentual > 0 
                                           && comparaPercentual
                                           && condicao.PercentualDe <= percentual 
                                           && condicao.PercentualAte >= percentual;

                    var valorAtende = realizaComparacaoComValorZerado 
                                      && comparaValor
                                      && (condicao.ValorDe <= valorComparacao || valorDeZeradoEValorComparacaoMenorZero) 
                                      && condicao.ValorAte >= valorComparacao;

                    if (percentualAtende || valorAtende)
                        necessitaAprovacaoComercial = true;

                }
            }
            

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if(novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for(int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteTraco = new AprovacaoComercialPendenteTraco()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,
                    ObraSeq = obraTraco.Sequencia,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Tracos.Add(pendenteTraco);
            }

            return pendente;

        }

        
        private AprovacaoComercialPendente ValidarComercialObraVolume(AprovacaoComercialPendente aprovPendente, Obra obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicao = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id && x.TipoValor == EAprovacaoComercialHierarquiaValor.Volume).FirstOrDefault();

            if(!necessitaAprovacaoComercial && condicao != null)
            {
                var condicaoEstaZerada = condicao.PercentualDe == 0 && condicao.PercentualAte == 0 && condicao.ValorDe == 0 && condicao.ValorAte == 0;

                if(!condicaoEstaZerada)
                {

                    double volumeTotalObraTraco = obra.ObraTracos.Sum(x => x.M3Quantidade);

                    var valorAtende = volumeTotalObraTraco >= condicao.ValorDe && volumeTotalObraTraco <= condicao.ValorAte;
                    if (valorAtende)
                        necessitaAprovacaoComercial = true;

                }


            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteVolume = new AprovacaoComercialPendenteVolume()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Volumes.Add(pendenteVolume);
            }

            return pendente;


        }

        private AprovacaoComercialPendente ValidarComercialObraCondicaoPagamento(AprovacaoComercialPendente aprovPendente, Obra obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var aprovCondicaoPagamento = _aprovacaoComercialHierarquiaService.ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(nivelHierarquia.Id, tipoPessoa.Id, obra.Proposta.Segmentacao);

            var estaConfiguradoAprovCondicaoPagamento = 
                    aprovCondicaoPagamento == null 
                    ? false 
                    : (aprovCondicaoPagamento.MediaDiasDe > 0 && aprovCondicaoPagamento.MediaDiasAte > 0);

            if (!necessitaAprovacaoComercial && estaConfiguradoAprovCondicaoPagamento)
            {

                var condicaoPagamentoObra = _condicaoPagamentoService.ObterPeloId(obra.CondicaoPagamentoCodigo ?? 0, false);
                var isCondicaoPagamentoPadrao = _condicaoPagamentoService.CondicaoPagamentoPadraoUsinaTipoPessoa(obra.CondicaoPagamentoCodigo ?? 0, obra.UsinaEntregaCodigo, obra.Proposta.Data, tipoPessoa.Sigla);

                if (isCondicaoPagamentoPadrao)
                    necessitaAprovacaoComercial = false;
                else
                    necessitaAprovacaoComercial =
                        condicaoPagamentoObra.MediaDias > 0
                        ? condicaoPagamentoObra.MediaDias >= aprovCondicaoPagamento.MediaDiasDe && condicaoPagamentoObra.MediaDias <= aprovCondicaoPagamento.MediaDiasAte
                        : necessitaAprovacaoComercial;

            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteCondicaoPagamento = new AprovacaoComercialPendenteCondicaoPagamento()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.CondicoesPagamento.Add(pendenteCondicaoPagamento);
            }

            return pendente;


        }

        public AprovacaoComercialPendente ValidarComercialObraVolumeVersao(AprovacaoComercialPendente aprovPendente, ObraVersao obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicao = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id && x.TipoValor == EAprovacaoComercialHierarquiaValor.Volume).FirstOrDefault();

            if (!necessitaAprovacaoComercial && condicao != null)
            {
                var condicaoEstaZerada = condicao.PercentualDe == 0 && condicao.PercentualAte == 0 && condicao.ValorDe == 0 && condicao.ValorAte == 0;

                if (!condicaoEstaZerada)
                {

                    double volumeTotalObraTraco = obra.ObraTracos.Sum(x => x.M3Quantidade);

                    var valorAtende = volumeTotalObraTraco >= condicao.ValorDe && volumeTotalObraTraco <= condicao.ValorAte;
                    if (valorAtende)
                        necessitaAprovacaoComercial = true;

                }


            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteVolume = new AprovacaoComercialPendenteVolume()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Volumes.Add(pendenteVolume);
            }

            return pendente;


        }

        public AprovacaoComercialPendente ValidarComercialObraCondicaoPagamentoVersao(AprovacaoComercialPendente aprovPendente, ObraVersao obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var aprovCondicaoPagamento = _aprovacaoComercialHierarquiaService.ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(nivelHierarquia.Id, tipoPessoa.Id,obra.Proposta.Segmentacao);

            var estaConfiguradoAprovCondicaoPagamento = aprovCondicaoPagamento == null ? false : (aprovCondicaoPagamento.MediaDiasDe > 0 && aprovCondicaoPagamento.MediaDiasAte > 0);

            if (!necessitaAprovacaoComercial && estaConfiguradoAprovCondicaoPagamento)
            {

                var condicaoPagamentoObra = _condicaoPagamentoService.ObterPeloId(obra.CondicaoPagamentoCodigo ?? 0, false);
                var isCondicaoPagamentoPadrao = _condicaoPagamentoService.CondicaoPagamentoPadraoUsinaTipoPessoa(obra.CondicaoPagamentoCodigo ?? 0, obra.UsinaEntregaCodigo, obra.Proposta.Data, tipoPessoa.Sigla);

                if (isCondicaoPagamentoPadrao)
                    necessitaAprovacaoComercial = false;
                else
                    necessitaAprovacaoComercial =
                        condicaoPagamentoObra.MediaDias > 0
                        ? condicaoPagamentoObra.MediaDias >= aprovCondicaoPagamento.MediaDiasDe && condicaoPagamentoObra.MediaDias <= aprovCondicaoPagamento.MediaDiasAte
                        : necessitaAprovacaoComercial;


            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteCondicaoPagamento = new AprovacaoComercialPendenteCondicaoPagamento()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.CondicoesPagamento.Add(pendenteCondicaoPagamento);
            }

            return pendente;


        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaVolume(Proposta proposta, Obra obra)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesVolume = pendente.Volumes;

                    foreach (var pendenteVolume in pendentesVolume)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteVolume(pendenteVolume);
                    }

                    pendente.Volumes = new List<AprovacaoComercialPendenteVolume>();

                }

                pendente = ValidarComercialObraVolume(pendente, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }
                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPeloVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.VolumeNecessitaAprovacaoComercialScope();

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPeloVolume)
                        continue;

                    pendente = ValidarComercialObraVolume(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_volume", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaVolume", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaCondicaoPagamento(Proposta proposta, Obra obra)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesCondicaoPagamento = pendente.CondicoesPagamento;

                    foreach (var pendenteCondicaoPagamento in pendentesCondicaoPagamento)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteCondicaoPagamento(pendenteCondicaoPagamento);
                    }

                    pendente.CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>();


                }

                pendente = ValidarComercialObraCondicaoPagamento(pendente, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }
                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPeloCondicaoPagamento = pendentes.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPeloCondicaoPagamento)
                        continue;

                    pendente = ValidarComercialObraCondicaoPagamento(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_condicao_pagamento", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaCondicaPagamento", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        private AprovacaoComercialPendente ValidarComercialObraBomba(AprovacaoComercialPendente aprovPendente, ObraBomba obraBomba, Obra obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicoes = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id);

            if (!necessitaAprovacaoComercial)
            {

                foreach (var condicao in condicoes)
                {

                    if (condicao.PercentualDe == 0 && condicao.PercentualAte == 0 && condicao.ValorDe == 0 && condicao.ValorAte == 0)
                        continue;

                    double valor = 0;
                    double percentual = 0;
                    double valorComparacao = 0;
                    double valorComparacaoTaxaMinima = 0;

                    if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaBomba)
                    {
                        valor = obraBomba.M3PrecoProposto;
                        percentual = obraBomba.DescontoPercentual;
                        valorComparacao = obraBomba.M3PrecoTabela - obraBomba.M3PrecoProposto;
                        valorComparacaoTaxaMinima = obraBomba.TaxaMinimaPrecoTabela - obraBomba.TaxaMinimaPrecoProposto;
                        if (percentual == 0)
                        {
                            percentual = (valorComparacao * 100) / obraBomba.M3PrecoTabela;
                        }
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemMCC)
                    {
                        continue;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemTransporte)
                    {
                        continue;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.Ebtida)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraBomba.Ebitda;
                    }

                    var valorDeZeradoEValorComparacaoMenorZero = condicao.ValorDe == 0 && valorComparacao < 0;
                    var valorDeZeradoEValorComparacaoTaxaMinimaMenorZero = condicao.ValorDe == 0 && valorComparacaoTaxaMinima < 0;

                    var realizaComparacaoComValorZerado = !(condicao.ValorDe == 0 && condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaBomba);

                    var comparaValor = condicao.ValorDe > 0 && condicao.ValorAte > 0;
                    var comparaPercentual = condicao.PercentualDe > 0 || condicao.PercentualAte > 0;

                    var percentualAtende = percentual > 0
                                           && comparaPercentual
                                           && (condicao.PercentualDe <= percentual
                                           && condicao.PercentualAte >= percentual);

                    var valorAtende = realizaComparacaoComValorZerado
                                      && comparaValor
                                      && ((condicao.ValorDe <= valorComparacao || valorDeZeradoEValorComparacaoMenorZero) || (condicao.ValorDe <= valorComparacaoTaxaMinima || valorDeZeradoEValorComparacaoTaxaMinimaMenorZero))
                                      && (condicao.ValorAte >= valorComparacao || condicao.ValorAte >= valorComparacaoTaxaMinima);

                    if (percentualAtende || valorAtende)
                        necessitaAprovacaoComercial = true;

                }
            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteBomba = new AprovacaoComercialPendenteBomba()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,
                    ObraSeq = obraBomba.Sequencia,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Bombas.Add(pendenteBomba);
            }

            if (necessitaAprovacaoComercial)
            {
                obraBomba.AprovacaoOperacao = "G";
                obraBomba.AprovacaoVerbal = "S";
                obraBomba.AprovacaoObservacao = "";
            }

            return pendente;

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaVolumeVersao(PropostaVersao proposta, ObraVersao obra)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesVolume = pendente.Volumes;

                    foreach (var pendenteVolume in pendentesVolume)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteVolume(pendenteVolume);
                    }

                    pendente.Volumes = new List<AprovacaoComercialPendenteVolume>();

                }

                pendente = ValidarComercialObraVolumeVersao(pendente, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPeloVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.VolumeNecessitaAprovacaoComercialScope();

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPeloVolume)
                        continue;

                    pendente = ValidarComercialObraVolumeVersao(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_volume", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaVolumeVersao", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaCondicaoPagamentoVersao(PropostaVersao proposta, ObraVersao obra)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendenteCondicoesPagamento = pendente.CondicoesPagamento;

                    foreach (var pendenteCondicaoPagamento in pendenteCondicoesPagamento)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteCondicaoPagamento(pendenteCondicaoPagamento);
                    }

                    pendente.CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>();


                }

                pendente = ValidarComercialObraCondicaoPagamentoVersao(pendente, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarCondicaoPagamento = pendentes.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarCondicaoPagamento)
                        continue;

                    pendente = ValidarComercialObraCondicaoPagamentoVersao(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_condicao_pagamento", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaCondicaPagamentoVersao", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaTraco(Proposta proposta, Obra obra, ObraTraco newTraco)
        {

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList() ;
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach(var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesTraco = pendente.Tracos.Where(x => x.ObraSeq == newTraco.Sequencia);

                    foreach (var pendenteTraco in pendentesTraco)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteTraco(pendenteTraco);
                    }

                    pendente.Tracos = pendente.Tracos.Where(x => x.ObraSeq != newTraco.Sequencia).ToList();

                }

                pendente = ValidarComercialObraTraco(pendente, newTraco, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Tracos.Where(x => x.ObraSeq == newTraco.Sequencia && x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPeloTraco = pendentes.TracoNecessitaAprovacaoComercialScope(newTraco.Sequencia);

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.TracoNecessitaAprovacaoComercialScope(newTraco.Sequencia);

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPeloTraco && newTraco.StatusAprovacao == EStatusAprovacao.NaoNecessita)
                        continue;

                    pendente = ValidarComercialObraTraco(pendente, newTraco, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes,
                newTraco
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_traco", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTraco", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public void RemoverAprovacaoAlcadaTraco(Proposta proposta, ObraTraco oldTraco)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach(var pendente in pendentes)
            {

                var tracoPendentes = pendente.Tracos.Where(x => x.ObraSeq == oldTraco.Sequencia).ToList();

                foreach(var tracoPendente in tracoPendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteTraco(tracoPendente);
                }

            }

        }

        public void RemoverAprovacaoAlcadaVolume(Proposta proposta)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var volumePendentes = pendente.Volumes;

                foreach (var volumePendente in volumePendentes.ToList())
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteVolume(volumePendente);
                }

            }

        }

        public void RemoverAprovacaoAlcadaCondicaoPagamento(Proposta proposta)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var CondicaoPagamentoPendentes = pendente.CondicoesPagamento;

                foreach (var CondicaoPagamentoPendente in CondicaoPagamentoPendentes.ToList())
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteCondicaoPagamento(CondicaoPagamentoPendente);
                }

            }

        }

        public void RemoverAprovacaoAlcadaVolumeVersao(PropostaVersao proposta)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var volumePendentes = pendente.Volumes;

                foreach (var volumePendente in volumePendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteVolume(volumePendente);
                }

            }

        }

        public void RemoverAprovacaoAlcadaCondicaoPagamentoVersao(PropostaVersao proposta)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var CondicaoPagamentoPendentes = pendente.CondicoesPagamento;

                foreach (var CondicaoPagamentoPendente in CondicaoPagamentoPendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteCondicaoPagamento(CondicaoPagamentoPendente);
                }

            }

        }

        public void RevisarAprovacaoComercialPendente(Obra obra, List<AprovacaoComercialPendente> pendentes = null, Proposta proposta = null)
        {

            // Função destinada a revisar as aprovações e criar as aprovações referentes ao Workflow ou Nível Obrigatório de Aprovação

            var obraUsina = proposta == null ? obra.UsinaCodigo : proposta.Obra.UsinaCodigo;
            var obraNumero = proposta == null ? obra.Numero : proposta.Obra.Numero;
            var propostaIntervenienteTipo = proposta == null ? obra.Proposta.IntervenienteTipo : proposta.IntervenienteTipo;

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obraUsina, obraNumero);

            var necessarioSalvarPendentesNaFuncao = pendentes is null;

            if(necessarioSalvarPendentesNaFuncao)
                pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(obraUsina, obraNumero, ultimaVersao).ToList();

            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(propostaIntervenienteTipo);

            if (pendentes.Count > 0)
            {

                var niveisHierarquias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria); ;
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                _aprovacaoComercialPendenteRepository.RemoverVestigiosAprovacoesAnteriores(obraUsina, obraNumero, 0, nivelMax);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    niveisHierarquias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                foreach (var hierarquia in niveisHierarquias)
                {

                    AprovacaoComercialPendente pendente = pendentes.Where(x => x.NivelHierarquia == hierarquia.NivelAutoridade).FirstOrDefault();
                    var novoNivel = (pendente == null);

                    if (obra.ObraTracos != null)
                    {
                        foreach (var obraTraco in obra.ObraTracos)
                        {
                            obraTraco.AtualizaStatusAprovacao("");

                            var necessarioPassarPeloTraco = pendentes.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);
                            var jaExistePendenteTracoNesseNivel = pendente.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);

                            if (necessarioPassarPeloTraco && !jaExistePendenteTracoNesseNivel && !(obraTraco.StatusAprovacao == EStatusAprovacao.NaoNecessita))
                                pendente = ValidarComercialObraTraco(pendente, obraTraco, obra, ultimaVersao, tipoPessoa, hierarquia, true);
                        }
                    }
                    
                    if (obra.ObraBombas != null) {
                        foreach (var obraBomba in obra.ObraBombas)
                        {
                            obraBomba.AtualizaStatusAprovacao("");

                            var necessarioPassarPelaBomba = pendentes.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);
                            var jaExistePendenteBomba = pendente.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);

                            if (necessarioPassarPelaBomba && !jaExistePendenteBomba && !(obraBomba.StatusAprovacao == EStatusAprovacao.NaoNecessita))
                                pendente = ValidarComercialObraBomba(pendente, obraBomba, obra, ultimaVersao, tipoPessoa, hierarquia, true);
                        }
                    }

                    /* --- Validar Volume ----------------------------------------------------- */

                    var necessarioPassarPeloVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();
                    var jaExistePendenteVolume = pendente.VolumeNecessitaAprovacaoComercialScope();

                    if (necessarioPassarPeloVolume && !jaExistePendenteVolume)
                        pendente = ValidarComercialObraVolume(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    /* ------------------------------------------------------------------------ */

                    /* --- Validar Demais Aprovações ------------------------------------------ */

                    var necessarioPassarPeloCondicaoPagamento = pendentes.CondicaoPagamentoNecessitaAprovacaoComercialScope();
                    var jaExistePendenteCondicaoPagamento = pendente.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                    if (necessarioPassarPeloCondicaoPagamento && !jaExistePendenteCondicaoPagamento)
                        pendente = ValidarComercialObraCondicaoPagamento(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    /* ------------------------------------------------------------------------ */

                    if (novoNivel && pendente != null)
                        pendentes.Add(pendente);

                }

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow && pendentes.Count() > 0)
                    pendentes = FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

                try
                {
                    pendentes = RevisarQuantidadeAprovacoesNecessaria(pendentes, niveisHierarquia);
                }
                catch (Exception erro)
                {
                    var payLoad = new
                    {
                        obra,
                        pendentes,
                        erro
                    };

                    _aprovacaoComercialService.AdicionarLog(
                        new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "", "AprovacaoComercialPendenteService.RevisarQuantidadeAprovacoesNecessaria", PayloadHelper.ConvertToJson(payLoad))
                        );
                }

                if (necessarioSalvarPendentesNaFuncao)
                {
                    _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

                    var payload = new
                    {
                        obra,
                        pendentes
                    };

                    _aprovacaoComercialService.AdicionarLog(
                        new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "", "AprovacaoComercialPendenteService.RevisarAprovacaoComercialPendente", PayloadHelper.ConvertToJson(payload))
                        );
                }

                if (obra.ObraTracos != null)
                    foreach (var obraTracoLog in obra.ObraTracos)
                        _obraService.AdicionarLogPropostaItem(obraTracoLog, "AprovacaoComercialPendenteService.RevisarAprovacaoComercialPendente");

                _aprovacaoComercialPendenteRepository.SaveChanges();

            }

        }

        public void RevisarAprovacaoComercialPendenteVersao(ObraVersao obra, List<AprovacaoComercialPendente> pendentes = null, PropostaVersao proposta = null)
        {

            // Função destinada a revisar as aprovações e criar as aprovações referentes ao Workflow ou Nível Obrigatório de Aprovação

            var obraUsina = proposta == null ? obra.UsinaCodigo : proposta.Obra.UsinaCodigo;
            var obraNumero = proposta == null ? obra.Numero : proposta.Obra.Numero;
            var propostaIntervenienteTipo = proposta == null ? obra.Proposta.IntervenienteTipo : proposta.IntervenienteTipo;

            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obraUsina, obraNumero);

            var necessarioSalvarPendentesNaFuncao = pendentes is null;

            if (necessarioSalvarPendentesNaFuncao)
                pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(obraUsina, obraNumero, ultimaVersao).ToList();

            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(propostaIntervenienteTipo);

            if (pendentes.Count > 0)
            {

                var niveisHierarquias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria); ;
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                _aprovacaoComercialPendenteRepository.RemoverVestigiosAprovacoesAnteriores(obraUsina, obraNumero, ultimaVersao, nivelMax);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    niveisHierarquias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                foreach (var hierarquia in niveisHierarquias)
                {

                    AprovacaoComercialPendente pendente = pendentes.Where(x => x.NivelHierarquia == hierarquia.NivelAutoridade).FirstOrDefault();
                    var novoNivel = (pendente == null);

                    if(obra.ObraTracos != null)
                    {
                        foreach (var obraTraco in obra.ObraTracos)
                        {
                            obraTraco.AtualizaStatusAprovacao("");

                            var necessarioPassarPeloTraco = pendentes.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);
                            var jaExistePendenteTracoNesseNivel = pendente.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);

                            if (necessarioPassarPeloTraco && !jaExistePendenteTracoNesseNivel && !(obraTraco.StatusAprovacao == EStatusAprovacao.NaoNecessita))
                                pendente = ValidarComercialObraTracoVersao(pendente, obraTraco, obra, ultimaVersao, tipoPessoa, hierarquia, true);
                        }
                    }

                    if (obra.ObraBombas != null)
                    {
                        foreach (var obraBomba in obra.ObraBombas)
                        {
                            obraBomba.AtualizaStatusAprovacao("");

                            var necessarioPassarPelaBomba = pendentes.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);
                            var jaExistePendenteBomba = pendente.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);

                            if (necessarioPassarPelaBomba && !jaExistePendenteBomba && !(obraBomba.StatusAprovacao == EStatusAprovacao.NaoNecessita))
                                pendente = ValidarComercialObraBombaVersao(pendente, obraBomba, obra, ultimaVersao, tipoPessoa, hierarquia, true);
                        }
                    }

                    /* --- Validar Volume ----------------------------------------------------- */

                    var necessarioPassarPeloVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();
                    var jaExistePendenteVolume = pendente.VolumeNecessitaAprovacaoComercialScope();

                    if (necessarioPassarPeloVolume && !jaExistePendenteVolume)
                        pendente = ValidarComercialObraVolumeVersao(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    /* ------------------------------------------------------------------------ */

                    /* --- Validar Demais Aprovações ------------------------------------------ */

                    var necessarioPassarPeloCondicaoPagamento = pendentes.CondicaoPagamentoNecessitaAprovacaoComercialScope();
                    var jaExistePendenteCondicaoPagamento = pendente.CondicaoPagamentoNecessitaAprovacaoComercialScope();

                    if (necessarioPassarPeloCondicaoPagamento && !jaExistePendenteCondicaoPagamento)
                        pendente = ValidarComercialObraCondicaoPagamentoVersao(pendente, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    /* ------------------------------------------------------------------------ */

                    if (novoNivel && pendente != null)
                        pendentes.Add(pendente);

                }

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow && pendentes.Count() > 0)
                    pendentes = FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

                try
                {
                    pendentes = RevisarQuantidadeAprovacoesNecessaria(pendentes, niveisHierarquia);
                }
                catch (Exception erro)
                {
                    var payLoad = new
                    {
                        obra,
                        pendentes,
                        erro
                    };

                    _aprovacaoComercialService.AdicionarLog(
                        new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "", "AprovacaoComercialPendenteService.RevisarQuantidadeAprovacoesNecessaria", PayloadHelper.ConvertToJson(payLoad))
                        );
                }

                if (necessarioSalvarPendentesNaFuncao)
                {
                    _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

                    var payload = new
                    {
                        obra,
                        pendentes
                    };

                    _aprovacaoComercialService.AdicionarLog(
                        new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "", "AprovacaoComercialPendenteService.RevisarAprovacaoComercialPendente", PayloadHelper.ConvertToJson(payload))
                        );
                }

                if (obra.ObraTracos != null)
                    foreach (var obraTracoLog in obra.ObraTracos)
                        _obraService.AdicionarLogPropostaItem(obraTracoLog, "AprovacaoComercialPendenteService.RevisarAprovacaoComercialPendenteVersao");

                _aprovacaoComercialPendenteRepository.SaveChanges();

            }

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaBomba(Proposta proposta, Obra obra, ObraBomba newBomba)
        {
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesBomba = pendente.Bombas.Where(x => x.ObraSeq == newBomba.Sequencia);

                    foreach (var pendenteBomba in pendentesBomba)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteBomba(pendenteBomba);
                    }
                }

                pendente = ValidarComercialObraBomba(pendente, newBomba, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Bombas.Where(x => x.ObraSeq == newBomba.Sequencia && x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPelaBomba = pendentes.BombaNecessitaAprovacaoComercialScope(newBomba.Sequencia);

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.BombaNecessitaAprovacaoComercialScope(newBomba.Sequencia);

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPelaBomba && newBomba.StatusAprovacao == EStatusAprovacao.NaoNecessita)
                        continue;

                    pendente = ValidarComercialObraBomba(pendente, newBomba, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if(novoPendente)
                        pendentes.Add(pendente);

                }

            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes,
                newBomba
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_bomba", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBomba", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public void RemoverAprovacaoAlcadaBomba(Proposta proposta, ObraBomba oldBomba)
        {

            var ultimaVersao = _obraService.ObterUltimaVersaoObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var bombaPendentes = pendente.Bombas.Where(x => x.ObraSeq == oldBomba.Sequencia).ToList();

                foreach (var bombaPendente in bombaPendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteBomba(bombaPendente);
                }

            }

        }

        public void ValidaAprovacoesObra(string usuario, Obra obra)
        {

            var obraVersao = _obraService.ObterUltimaVersaoObra(obra.UsinaCodigo, obra.Numero);
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var hierarquiasComercial = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var aprovacoesPendentes = _aprovacaoComercialPendenteRepository.ListarTodasAprovacoesPorObraVersao(obra.UsinaCodigo, obra.Numero, obraVersao).ToList();

            if (aprovacoesPendentes.Count() == 0)
            {
                if(obra.StatusComercial == EObraStatusComercial.Aguardando)
                {
                    // Se não existe e não esta aprovado ele vai verificar se tem necessidade de criar
                    ValidarComercialObra(obra, usuario);
                }
            }

            ValidarAprovacoesObraTraco(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraBomba(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraVolume(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraCondicaoPagamento(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for(int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var aprovacaoPendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (aprovacaoPendente == null)
                    continue;

                if (aprovacaoPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                    continue;

                var possuiAprovacoesPendentes =
                    aprovacaoPendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() > 0;

                if (possuiAprovacoesPendentes)
                    continue;

                aprovacaoPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                aprovacaoPendente.AprovacaoData = DateTime.Now;
                
            }

            if(aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(aprovacoesPendentes);

            var payLoad = new
            {
                obra,
                aprovacoes = aprovacoesPendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, obraVersao, "", "AprovacaoComercialPendenteService.ValidaAprovacoesObra", "", PayloadHelper.ConvertToJson(payLoad)));
            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(aprovacoesPendentes);

        }

        public void ValidarAprovacoesObraTraco(Obra obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {
            var tracosFiltrados = obra.ObraTracos.Where(x => x.StatusAprovacao != EStatusAprovacao.NaoNecessita && x.StatusAprovacao != EStatusAprovacao.Aprovado);

            foreach (var traco in tracosFiltrados)
            {
                var aprovarTraco = true;

                var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
                var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

                for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
                {
                    var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                    if (pendente == null)
                        continue;

                    var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                    var numeroNotificacoes = pendente.Tracos.Where(x => x.ObraSeq == traco.Sequencia).Count();
                    var numeroAprovacoes = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.ObraSeq == traco.Sequencia).Count();
                    var numeroReprovacoes = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado && x.ObraSeq == traco.Sequencia).Count();

                    if (numeroNotificacoes == 0)
                        continue;

                    if (numeroReprovacoes > 0)
                    {

                        pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        pendente.AprovacaoData = DateTime.Now;

                        traco.ReprovarPorAlcada(usuario);

                    }

                    aprovarTraco = aprovarTraco && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

                }

                if (aprovarTraco)
                    traco.Aprovar(usuario);

            }
        }

        public void ValidarAprovacoesObraBomba(Obra obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {
            var bombasFiltradas = obra.ObraBombas.Where(x => x.StatusAprovacao != EStatusAprovacao.NaoNecessita && x.StatusAprovacao != EStatusAprovacao.Aprovado);

            foreach (var bomba in bombasFiltradas)
            {

                var aprovarBomba = true;

                var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
                var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

                for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
                {
                    var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                    if (pendente == null)
                        continue;

                    var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                    var numeroNotificacoes = pendente.Bombas.Where(x => x.ObraSeq == bomba.Sequencia).Count();
                    var numeroAprovacoes = pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.ObraSeq == bomba.Sequencia).Count();
                    var numeroReprovacoes = pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado && x.ObraSeq == bomba.Sequencia).Count();

                    if (numeroNotificacoes == 0)
                        continue;

                    if (numeroReprovacoes > 0)
                    {

                        pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        pendente.AprovacaoData = DateTime.Now;

                        bomba.ReprovarPorAlcada(usuario);

                    }

                    aprovarBomba = aprovarBomba && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

                }

                if (aprovarBomba)
                    bomba.Aprovar(usuario);

            }
        }

        public AprovacaoComercialPendente ObterAprovacaoPendentePorObraVersaoNivelHierarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia)
        {
            return _aprovacaoComercialPendenteRepository.ObterAprovacaoPendentePorObraVersaoNivelHierarquia(obraUsina, obraNumero, obraVersao, nivelHierarquia);
        }

        public AprovacaoComercialPendente ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia)
        {
            return _aprovacaoComercialPendenteRepository.ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(obraUsina, obraNumero, obraVersao, nivelHierarquia);
        }

        public bool UsuarioJaRealizouAprovacaoPendenteTracoDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaTraco, string usuarioId)
        {
            return _aprovacaoComercialPendenteRepository.UsuarioJaRealizouAprovacaoPendenteTracoDeObraVersao(obraUsina, obraNumero, obraVersao, nivelHierarquia, sequenciaTraco, usuarioId);
        }

        public bool UsuarioJaRealizouAprovacaoPendenteBombaDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaBomba, string usuarioId)
        {
            return _aprovacaoComercialPendenteRepository.UsuarioJaRealizouAprovacaoPendenteBombaDeObraVersao(obraUsina, obraNumero, obraVersao, nivelHierarquia, sequenciaBomba, usuarioId);
        }

        // ------------------------------------------------------------------------------- Código Replicado Para Versão de Contrato ---------------------------------------------

        public void ValidarComercialObraVersao(int obraUsina, int obraNumero, int obraVersao, string usuario)
        {

            var obra = _obraService.ObterPorIdAprovacaoComercial(obraUsina, obraNumero, obraVersao);
            ValidarComercialObraVersao(obra, usuario);

        }

        public void ValidarComercialObraVersao(ObraVersao obra, string usuario)
        {
            var utilizaAprovacaoComercialAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(obra.UsinaEntregaCodigo);
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);

            if (!utilizaAprovacaoComercialAlcada || aprovacaoComercialUsina == null)
                return;

            var obraVersao = obra.NumeroVersao;
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(obra.Proposta.IntervenienteTipo);
            var hierarquiasComercial = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo);
            var pendentes = new List<AprovacaoComercialPendente>();

            foreach (var hierarquia in hierarquiasComercial)
            {

                AprovacaoComercialPendente pendente = null;

                foreach (var traco in obra.ObraTracos)
                {
                    pendente = ValidarComercialObraTracoVersao(pendente, traco, obra, obraVersao, tipoPessoa, hierarquia);
                }

                foreach (var bomba in obra.ObraBombas)
                {
                    pendente = ValidarComercialObraBombaVersao(pendente, bomba, obra, obraVersao, tipoPessoa, hierarquia);
                }

                pendente = ValidarComercialObraVolumeVersao(pendente, obra, obraVersao, tipoPessoa, hierarquia);

                pendente = ValidarComercialObraCondicaoPagamentoVersao(pendente, obra, obraVersao, tipoPessoa, hierarquia);

                if (pendente != null)
                    pendentes.Add(pendente);

            }

            // Valida se o traço é pendente ou não
            foreach (var obraTraco in obra.ObraTracos)
            {
                var pendentesDoTraco = pendentes.TracoNecessitaAprovacaoComercialScope(obraTraco.Sequencia);

                if (pendentesDoTraco)
                {
                    obraTraco.AprovacaoOperacao = "G";
                    obraTraco.AprovacaoVerbal = "N";
                    obraTraco.AprovacaoObservacao = "";
                }
                else
                {
                    obraTraco.AprovacaoOperacao = "";
                    obraTraco.AprovacaoVerbal = "";
                    obraTraco.AprovacaoObservacao = "";
                }

                obraTraco.AtualizaStatusAprovacao(usuario);

            }

            foreach (var obraBomba in obra.ObraBombas)
            {

                var pendentesDoTraco = pendentes.BombaNecessitaAprovacaoComercialScope(obraBomba.Sequencia);

                if (pendentesDoTraco)
                {
                    obraBomba.AprovacaoOperacao = "G";
                    obraBomba.AprovacaoVerbal = "S";
                    obraBomba.AprovacaoObservacao = "";
                }
                else
                {
                    obraBomba.AprovacaoOperacao = "";
                    obraBomba.AprovacaoVerbal = "";
                    obraBomba.AprovacaoObservacao = "";
                }

                obraBomba.AtualizaStatusAprovacao(usuario);

            }

            var possuiPendentesVolume = pendentes.VolumeNecessitaAprovacaoComercialScope();
            if (possuiPendentesVolume)
            {
                obra.VolumeStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;
                obra.StatusComercial = EObraStatusComercial.Aguardando;
            }
            else
            {
                var novoStatus = EObraDemaisStatusComercial.NaoNecessita;

                if (obra.VolumeStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao)
                    novoStatus = EObraDemaisStatusComercial.Aprovado;

                obra.VolumeStatusComercial = novoStatus;
            }

            if (obra.ObraTracos != null)
                foreach (var obraTracoLog in obra.ObraTracos)
                    _obraService.AdicionarLogPropostaItem(obraTracoLog, "AprovacaoComercialPendenteService.ValidarComercialObraVersao");

            _aprovacaoComercialPendenteRepository.SaveChanges();

            RevisarAprovacaoComercialPendenteVersao(obra, pendentes);

            var payLoad = new
            {
                obra,
                aprovacoes = pendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, obraVersao, "", "AprovacaoComercialPendenteService.ValidarComercialObraVersao", "", PayloadHelper.ConvertToJson(payLoad)));
            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);
        }

        public List<AprovacaoComercialPendente> FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(List<AprovacaoComercialPendente> pendentes)
        {

            var aprovacaoPendentes = pendentes.Where(x => x.AprovacaoStatus != EAprovacaoComercialPendenteStatus.Aprovado);

            if (aprovacaoPendentes.Count() == 0)
                return pendentes;

            var menorNivelSemAprovacao = aprovacaoPendentes.Min(x => x.NivelHierarquia);
            var niveisOrdenados = pendentes.OrderByDescending(x => x.NivelHierarquia).ToList();

            foreach(var pendente in niveisOrdenados)
            {

                if (pendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacao 
                    && pendente.AprovacaoStatus != EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior)
                    continue;

                var novoStatus = menorNivelSemAprovacao == pendente.NivelHierarquia ? EAprovacaoComercialPendenteStatus.AguardandoAprovacao : EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

                pendente.AprovacaoStatus = novoStatus;

                foreach(var traco in pendente.Tracos)
                {

                    var statusAtualAguardandoAprovacao = traco.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao
                        || traco.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

                    if (statusAtualAguardandoAprovacao)
                        traco.AprovacaoStatus = novoStatus;
                }

                foreach (var bomba in pendente.Bombas)
                {

                    var statusAtualAguardandoAprovacao = bomba.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao
                        || bomba.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

                    if (statusAtualAguardandoAprovacao)
                        bomba.AprovacaoStatus = novoStatus;
                }

                foreach (var volume in pendente.Volumes)
                {

                    var statusAtualAguardandoAprovacao = volume.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao
                        || volume.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

                    if (statusAtualAguardandoAprovacao)
                        volume.AprovacaoStatus = novoStatus;
                }

                foreach (var CondicaoPagamento in pendente.CondicoesPagamento)
                {

                    var statusAtualAguardandoAprovacao = CondicaoPagamento.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao
                        || CondicaoPagamento.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

                    if (statusAtualAguardandoAprovacao)
                        CondicaoPagamento.AprovacaoStatus = novoStatus;
                }

            }

            return pendentes;

        }

        public List<AprovacaoComercialPendente> RevisarQuantidadeAprovacoesNecessaria(List<AprovacaoComercialPendente> pendentes, List<AprovacaoComercialHierarquia> hierarquias)
        {

            var result = pendentes;

            foreach(var pendente in result)
            {

                var hierarquia = hierarquias.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();
                if (hierarquia is null)
                    continue;

                // ==== VOLUME ====================================
                var aprovacoesExcedentes = pendente.Volumes.Count - hierarquia.QuantidadeAprovacoesNecessarias;
                for (int i = 1; i <= aprovacoesExcedentes; i++)
                {
                    var pendenteVolumeMaiorSequencia = pendente.Volumes.OrderByDescending(x => x.AprovacaoSequencia).FirstOrDefault();

                    if (pendenteVolumeMaiorSequencia is null)
                        break;

                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteVolume(pendenteVolumeMaiorSequencia);

                    pendente.Volumes.Remove(pendenteVolumeMaiorSequencia);
                }

                // ==== CONDICOES ====================================
                aprovacoesExcedentes = pendente.CondicoesPagamento.Count - hierarquia.QuantidadeAprovacoesNecessarias;
                for (int i = 1; i <= aprovacoesExcedentes; i++)
                {
                    var pendenteCondicoesMaiorSequencia = pendente.CondicoesPagamento.OrderByDescending(x => x.AprovacaoSequencia).FirstOrDefault();

                    if (pendenteCondicoesMaiorSequencia is null)
                        break;

                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteCondicaoPagamento(pendenteCondicoesMaiorSequencia);

                    pendente.CondicoesPagamento.Remove(pendenteCondicoesMaiorSequencia);
                }

                // ==== TRAÇOS ====================================
                var tracosSequencias = pendente.Tracos.Select(x => x.ObraSeq).Distinct().ToList();
                foreach (var tracoSequencia in tracosSequencias)
                {
                    var pendentesTraco = pendente.Tracos.Where(x => x.ObraSeq == tracoSequencia).ToList();
                    aprovacoesExcedentes = pendentesTraco.Count - hierarquia.QuantidadeAprovacoesNecessarias;
                    
                    for(int i = 1;i <= aprovacoesExcedentes; i++)
                    {
                        var pendenteTracoMaiorSequencia = pendentesTraco.OrderByDescending(x => x.AprovacaoSequencia).FirstOrDefault();

                        if (pendenteTracoMaiorSequencia is null)
                            break;

                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteTraco(pendenteTracoMaiorSequencia);

                        pendente.Tracos.Remove(pendenteTracoMaiorSequencia);
                        pendentesTraco = pendente.Tracos.Where(x => x.ObraSeq == tracoSequencia).ToList();
                    }

                }

                // ==== BOMBA ====================================
                var bombasSequencias = pendente.Bombas.Select(x => x.ObraSeq).Distinct().ToList();
                foreach (var bombaSequencia in bombasSequencias)
                {
                    var pendentesBomba = pendente.Bombas.Where(x => x.ObraSeq == bombaSequencia).ToList();
                    aprovacoesExcedentes = pendentesBomba.Count - hierarquia.QuantidadeAprovacoesNecessarias;

                    for (int i = 1; i <= aprovacoesExcedentes; i++)
                    {
                        var pendenteBombaMaiorSequencia = pendentesBomba.OrderByDescending(x => x.AprovacaoSequencia).FirstOrDefault();

                        if (pendenteBombaMaiorSequencia is null)
                            break;

                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteBomba(pendenteBombaMaiorSequencia);

                        pendente.Bombas.Remove(pendenteBombaMaiorSequencia);
                        pendentesBomba = pendente.Bombas.Where(x => x.ObraSeq == bombaSequencia).ToList();
                    }

                }

            }

            return result;

        }

        private AprovacaoComercialPendente ValidarComercialObraTracoVersao(AprovacaoComercialPendente aprovPendente, ObraTracoVersao obraTraco, ObraVersao obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicoes = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id);

            if (!necessitaAprovacaoComercial)
            {
                foreach (var condicao in condicoes)
                {

                    if ((condicao.PercentualDe == 0 && condicao.PercentualAte == 0) && (condicao.ValorDe == 0 && condicao.ValorAte == 0))
                        continue;

                    double valor = 0;
                    double percentual = 0;
                    double valorComparacao = 0;

                    if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaTracos)
                    {

                        valor = (obraTraco.PrecoReajustadoAtual != 0 ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);
                        percentual = obraTraco.DescontoPercentual;
                        valorComparacao = obraTraco.M3PrecoTabela - (obraTraco.PrecoReajustadoAtual != 0 ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemMCC)
                    {
                        var valorServicoAtual = obraTraco.CustoServicoReajustado == 0 ? obraTraco.ValorServico : obraTraco.CustoServicoReajustado;

                        valor = 0;
                        percentual = 0;
                        valorComparacao = valorServicoAtual - obraTraco.TotalImpostos;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemTransporte)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraTraco.MargemPosTransporte;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.Ebtida)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraTraco.Ebitda;
                    }

                    var valorDeZeradoEValorComparacaoMenorZero = condicao.ValorDe == 0 && valorComparacao < 0;

                    var realizaComparacaoComValorZerado = !(condicao.ValorDe == 0 && condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaTracos);

                    var comparaValor = condicao.ValorDe > 0 && condicao.ValorAte > 0;
                    var comparaPercentual = condicao.PercentualDe > 0 || condicao.PercentualAte > 0;

                    var percentualAtende = percentual > 0
                                           && comparaPercentual
                                           && condicao.PercentualDe <= percentual
                                           && condicao.PercentualAte >= percentual;

                    var valorAtende = realizaComparacaoComValorZerado
                                      && comparaValor
                                      && (condicao.ValorDe <= valorComparacao || valorDeZeradoEValorComparacaoMenorZero)
                                      && condicao.ValorAte >= valorComparacao;

                    if (percentualAtende || valorAtende)
                        necessitaAprovacaoComercial = true;

                }
            }


            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteTraco = new AprovacaoComercialPendenteTraco()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,
                    ObraSeq = obraTraco.Sequencia,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Tracos.Add(pendenteTraco);
            }

            return pendente;

        }

        private AprovacaoComercialPendente ValidarComercialObraBombaVersao(AprovacaoComercialPendente aprovPendente, ObraBombaVersao obraBomba, ObraVersao obra, int obraVersao, AprovacaoComercialTipoPessoa tipoPessoa, AprovacaoComercialHierarquia nivelHierarquia, bool forcarNotificacaoAprovacaoNivel = false)
        {
            var necessitaAprovacaoComercial = forcarNotificacaoAprovacaoNivel;
            var condicoes = nivelHierarquia.Condicoes.Where(x => x.TipoPessoaId == tipoPessoa.Id);

            if (!necessitaAprovacaoComercial)
            {

                foreach (var condicao in condicoes)
                {

                    if (condicao.PercentualDe == 0 && condicao.PercentualAte == 0 && condicao.ValorDe == 0 && condicao.ValorAte == 0)
                        continue;

                    double valor = 0;
                    double percentual = 0;
                    double valorComparacao = 0;
                    double valorComparacaoTaxaMinima = 0;

                    if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaBomba)
                    {
                        valor = obraBomba.M3PrecoProposto;
                        percentual = obraBomba.DescontoPercentual;
                        valorComparacao = obraBomba.M3PrecoTabela - obraBomba.M3PrecoProposto;
                        valorComparacaoTaxaMinima = obraBomba.TaxaMinimaPrecoTabela - obraBomba.TaxaMinimaPrecoProposto;
                        if (percentual == 0)
                        {
                            percentual = (valorComparacao * 100) / obraBomba.M3PrecoTabela;
                        }
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemMCC)
                    {
                        continue;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.MargemTransporte)
                    {
                        continue;
                    }
                    else if (condicao.TipoValor == EAprovacaoComercialHierarquiaValor.Ebtida)
                    {
                        valor = 0;
                        percentual = 0;
                        valorComparacao = obraBomba.Ebitda;
                    }

                    var valorDeZeradoEValorComparacaoMenorZero = condicao.ValorDe == 0 && valorComparacao < 0;
                    var valorDeZeradoEValorComparacaoTaxaMinimaMenorZero = condicao.ValorDe == 0 && valorComparacaoTaxaMinima < 0;

                    var realizaComparacaoComValorZerado = !(condicao.ValorDe == 0 && condicao.TipoValor == EAprovacaoComercialHierarquiaValor.ValorVendaBomba);

                    var comparaValor = condicao.ValorDe > 0 && condicao.ValorAte > 0;
                    var comparaPercentual = condicao.PercentualDe > 0 || condicao.PercentualAte > 0;

                    var percentualAtende = percentual > 0
                                           && comparaPercentual
                                           && (condicao.PercentualDe <= percentual
                                           && condicao.PercentualAte >= percentual);

                    var valorAtende = realizaComparacaoComValorZerado
                                      && comparaValor
                                      && ((condicao.ValorDe <= valorComparacao  || valorDeZeradoEValorComparacaoMenorZero) || (condicao.ValorDe <= valorComparacaoTaxaMinima || valorDeZeradoEValorComparacaoTaxaMinimaMenorZero))
                                      && (condicao.ValorAte >= valorComparacao || condicao.ValorAte >= valorComparacaoTaxaMinima);

                    if (percentualAtende || valorAtende)
                        necessitaAprovacaoComercial = true;

                }
            }

            if (!necessitaAprovacaoComercial)
                return aprovPendente;

            var pendente = aprovPendente;
            var novoPendente = (pendente == null);

            if (novoPendente)
            {
                pendente = new AprovacaoComercialPendente()
                {
                    Id = Guid.NewGuid(),
                    DataCriacao = DateTime.Now,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,

                    NivelHierarquia = nivelHierarquia.NivelAutoridade,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,
                    Tracos = new List<AprovacaoComercialPendenteTraco>(),
                    Bombas = new List<AprovacaoComercialPendenteBomba>(),
                    Volumes = new List<AprovacaoComercialPendenteVolume>(),
                    CondicoesPagamento = new List<AprovacaoComercialPendenteCondicaoPagamento>()
                };

            }

            for (int sequencia = 0; sequencia < nivelHierarquia.QuantidadeAprovacoesNecessarias; sequencia++)
            {
                var pendenteBomba = new AprovacaoComercialPendenteBomba()
                {
                    Id = Guid.NewGuid(),
                    IdAprovacao = pendente.Id,

                    ObraVersao = obraVersao,
                    ObraUsina = obra.UsinaCodigo,
                    ObraNumero = obra.Numero,
                    ObraSeq = obraBomba.Sequencia,

                    NivelHierarquia = pendente.NivelHierarquia,
                    AprovacaoStatus = EAprovacaoComercialPendenteStatus.AguardandoAprovacao,

                    AprovacaoSequencia = sequencia
                };

                pendente.Bombas.Add(pendenteBomba);
            }

            if (necessitaAprovacaoComercial)
            {
                obraBomba.AprovacaoOperacao = "G";
                obraBomba.AprovacaoVerbal = "S";
                obraBomba.AprovacaoObservacao = "";
            }

            return pendente;

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaTracoVersao(PropostaVersao proposta, ObraVersao obra, ObraTracoVersao newTraco)
        {

            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = proposta.Obra.NumeroVersao;
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesTraco = pendente.Tracos.Where(x => x.ObraSeq == newTraco.Sequencia);

                    foreach (var pendenteTraco in pendentesTraco)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteTraco(pendenteTraco);
                    }

                    pendente.Tracos = pendente.Tracos.Where(x => x.ObraSeq != newTraco.Sequencia).ToList();

                }

                pendente = ValidarComercialObraTracoVersao(pendente, newTraco, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Tracos.Where(x => x.ObraSeq == newTraco.Sequencia && x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                         && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                         && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade <= nivelMax);

                var necessarioPassarPeloTraco = pendentes.TracoNecessitaAprovacaoComercialScope(newTraco.Sequencia);

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;

                    var jaExistePendenteNivel = pendente.TracoNecessitaAprovacaoComercialScope(newTraco.Sequencia);

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPeloTraco && newTraco.StatusAprovacao == EStatusAprovacao.NaoNecessita)
                        continue;

                    pendente = ValidarComercialObraTracoVersao(pendente, newTraco, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }
            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes,
                newTraco
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_traco", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTracoVersao", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public void RemoverAprovacaoAlcadaTracoVersao(PropostaVersao proposta, ObraTracoVersao oldTraco)
        {

            var ultimaVersao = proposta.Obra.NumeroVersao;
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var tracoPendentes = pendente.Tracos.Where(x => x.ObraSeq == oldTraco.Sequencia).ToList();

                foreach (var tracoPendente in tracoPendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteTraco(tracoPendente);
                }

            }

        }

        public EStatusAprovacao AtualizarAprovacaoAlcadaBombaVersao(PropostaVersao proposta, ObraVersao obra, ObraBombaVersao newBomba)
        {

            var niveisHierarquia = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var ultimaVersao = proposta.Obra.NumeroVersao;
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao).ToList();
            var tipoPessoa = _aprovacaoComercialHierarquiaService.ObterTipoPessoaPorSigla(proposta.IntervenienteTipo);

            var necessitaAprovacao = false;

            foreach (var nivelHierarquia in niveisHierarquia)
            {

                var pendente = pendentes.Where(x => x.NivelHierarquia == nivelHierarquia.NivelAutoridade).FirstOrDefault();

                if (pendente != null)
                {
                    var pendentesBomba = pendente.Bombas.Where(x => x.ObraSeq == newBomba.Sequencia);

                    foreach (var pendenteBomba in pendentesBomba)
                    {
                        _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteBomba(pendenteBomba);
                    }

                    pendente.Bombas = pendente.Bombas.Where(x => x.ObraSeq != newBomba.Sequencia).ToList();
                }

                pendente = ValidarComercialObraBombaVersao(pendente, newBomba, proposta.Obra, ultimaVersao, tipoPessoa, nivelHierarquia, false);

                if (pendente != null)
                {

                    if (pendente.Bombas.Where(x => x.ObraSeq == newBomba.Sequencia && x.PendenteAprovacaoComercial()).Count() > 0)
                        necessitaAprovacao = true;

                    if (pendentes.Where(x => x.Id == pendente.Id).Count() == 0)
                        pendentes.Add(pendente);

                    if (pendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() == 0
                        && pendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() == 0)
                    {

                        var aprovarPendente = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;
                        aprovarPendente = aprovarPendente || pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count() > 0;

                        if (aprovarPendente)
                            pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                        else
                        {
                            _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendente(pendente);
                            pendentes = pendentes.Where(x => x.Id != pendente.Id).ToList();
                        }

                    }

                }

            }

            if (pendentes.Count > 0)
            {
                var hierarquiasObrigatorias = niveisHierarquia.Where(x => x.AprovacaoObrigatoria);
                var nivelMax = pendentes.Max(x => x.NivelHierarquia);

                if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                    hierarquiasObrigatorias = niveisHierarquia.Where(x => x.NivelAutoridade < nivelMax);

                var necessarioPassarPelaBomba = pendentes.BombaNecessitaAprovacaoComercialScope(newBomba.Sequencia);

                foreach (var hierarquia in hierarquiasObrigatorias)
                {

                    AprovacaoComercialPendente pendente = pendentes.FirstOrDefault(x => x.NivelHierarquia == hierarquia.NivelAutoridade);

                    var novoPendente = pendente == null;
                    var jaExistePendenteNivel = pendente.BombaNecessitaAprovacaoComercialScope(newBomba.Sequencia);

                    if (jaExistePendenteNivel)
                        continue;

                    if (!necessarioPassarPelaBomba && newBomba.StatusAprovacao == EStatusAprovacao.NaoNecessita)
                        continue;

                    pendente = ValidarComercialObraBombaVersao(pendente, newBomba, obra, ultimaVersao, tipoPessoa, hierarquia, true);

                    if (novoPendente)
                        pendentes.Add(pendente);

                }

            }

            if (pendentes.Count() > 0 && aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(pendentes);

            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(pendentes);

            var payload = new
            {
                necessitaAprovacao,
                obra,
                pendentes,
                newBomba
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, ultimaVersao, "con_aprovacao_comercial_pendente_bomba", "AprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBombaVersao", "", PayloadHelper.ConvertToJson(payload)));

            return necessitaAprovacao ? EStatusAprovacao.Pendente : EStatusAprovacao.NaoNecessita;

        }

        public void RemoverAprovacaoAlcadaBombaVersao(PropostaVersao proposta, ObraBombaVersao oldBomba)
        {

            var ultimaVersao = proposta.Obra.NumeroVersao;
            var pendentes = _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, ultimaVersao);

            foreach (var pendente in pendentes)
            {

                var bombaPendentes = pendente.Bombas.Where(x => x.ObraSeq == oldBomba.Sequencia).ToList();

                foreach (var bombaPendente in bombaPendentes)
                {
                    _aprovacaoComercialPendenteRepository.RemoverAprovacaoPendenteBomba(bombaPendente);
                }

            }

        }

        public void ValidaAprovacoesObraVersao(string usuario, ObraVersao obra)
        {

            var obraVersao = obra.NumeroVersao;
            var aprovacaoComercialUsina = _aprovacaoComercialService.ObterPorUsina(obra.UsinaEntregaCodigo);
            var hierarquiasComercial = _aprovacaoComercialHierarquiaService.ListarNivelHierarquiaPorUsina(obra.UsinaEntregaCodigo).ToList();
            var aprovacoesPendentes = _aprovacaoComercialPendenteRepository.ListarTodasAprovacoesPorObraVersao(obra.UsinaCodigo, obra.Numero, obraVersao).ToList();

            if (aprovacoesPendentes.Count() == 0)
            {
                if (obra.StatusComercial == EObraStatusComercial.Aguardando)
                {
                    // Se não existe e não esta aprovado ele vai verificar se tem necessidade de criar
                    ValidarComercialObraVersao(obra, usuario);
                }
            }

            ValidarAprovacoesObraTracoVersao(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraBombaVersao(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraVolumeVersao(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            ValidarAprovacoesObraCondicaoPagamentoVersao(obra, usuario, hierarquiasComercial, aprovacoesPendentes);

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var aprovacaoPendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (aprovacaoPendente == null)
                    continue;

                if (aprovacaoPendente.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado)
                    continue;

                var possuiAprovacoesPendentes =
                    aprovacaoPendente.Tracos.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.Bombas.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.Volumes.Where(x => x.PendenteAprovacaoComercial()).Count() > 0 ||
                    aprovacaoPendente.CondicoesPagamento.Where(x => x.PendenteAprovacaoComercial()).Count() > 0;

                if (possuiAprovacoesPendentes)
                    continue;

                aprovacaoPendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Aprovado;
                aprovacaoPendente.AprovacaoData = DateTime.Now;

            }

            if (aprovacaoComercialUsina.FluxoAprovacao == EAprovacaoComercialUsinaFluxoAprovacao.Workflow)
                FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(aprovacoesPendentes);

            var payLoad = new
            {
                obra,
                aprovacoes = aprovacoesPendentes
            };

            _aprovacaoComercialService.AdicionarLog(new AprovacaoComercialLog(obra.UsinaCodigo, obra.Numero, obraVersao, "", "AprovacaoComercialPendenteService.ValidaAprovacoesObra", "", PayloadHelper.ConvertToJson(payLoad)));
            _aprovacaoComercialPendenteRepository.AdicionarAprovacoesPendentes(aprovacoesPendentes);

        }

        public bool ValidarAprovacoesObraVolume(Obra obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {

            var aprovarVolume = true;

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (pendente == null)
                    continue;

                var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                var numeroNotificacoes = pendente.Volumes.Count();
                var numeroAprovacoes = pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count();
                var numeroReprovacoes = pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count();

                if (numeroNotificacoes == 0)
                    continue;

                if (numeroReprovacoes > 0)
                {

                    pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                    pendente.AprovacaoData = DateTime.Now;

                }

                aprovarVolume = aprovarVolume && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

            }

            if (aprovarVolume)
                obra.VolumeStatusComercial = EObraDemaisStatusComercial.Aprovado;

            return aprovarVolume;

        }

        public bool ValidarAprovacoesObraCondicaoPagamento(Obra obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {

            var aprovarCondicaoPagamento = true;

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (pendente == null)
                    continue;

                var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                var numeroNotificacoes = pendente.CondicoesPagamento.Count();
                var numeroAprovacoes = pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count();
                var numeroReprovacoes = pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count();

                if (numeroNotificacoes == 0)
                    continue;

                if (numeroReprovacoes > 0)
                {

                    pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                    pendente.AprovacaoData = DateTime.Now;

                }

                aprovarCondicaoPagamento = aprovarCondicaoPagamento && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

            }

            if (aprovarCondicaoPagamento)
                obra.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.Aprovado;

            return aprovarCondicaoPagamento;

        }

        private bool ValidarAprovacoesObraVolumeVersao(ObraVersao obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {

            var aprovarVolume = true;

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (pendente == null)
                    continue;

                var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                var numeroNotificacoes = pendente.Volumes.Count();
                var numeroAprovacoes = pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count();
                var numeroReprovacoes = pendente.Volumes.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count();

                if (numeroNotificacoes == 0)
                    continue;

                if (numeroReprovacoes > 0)
                {

                    pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                    pendente.AprovacaoData = DateTime.Now;

                }

                aprovarVolume = aprovarVolume && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

            }

            if (aprovarVolume)
                obra.VolumeStatusComercial = EObraDemaisStatusComercial.Aprovado;

            return aprovarVolume;

        }

        private bool ValidarAprovacoesObraCondicaoPagamentoVersao(ObraVersao obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {

            var aprovarCondicoesPagamento = true;

            var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
            var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

            for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
            {
                var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                if (pendente == null)
                    continue;

                var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                var numeroNotificacoes = pendente.CondicoesPagamento.Count();
                var numeroAprovacoes = pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado).Count();
                var numeroReprovacoes = pendente.CondicoesPagamento.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado).Count();

                if (numeroNotificacoes == 0)
                    continue;

                if (numeroReprovacoes > 0)
                {

                    pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                    pendente.AprovacaoData = DateTime.Now;

                }

                aprovarCondicoesPagamento = aprovarCondicoesPagamento && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

            }

            if (aprovarCondicoesPagamento)
                obra.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.Aprovado;

            return aprovarCondicoesPagamento;

        }

        public void ValidarAprovacoesObraTracoVersao(ObraVersao obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {
            var tracosFiltrados = obra.ObraTracos.Where(x => x.StatusAprovacao != EStatusAprovacao.NaoNecessita && x.StatusAprovacao != EStatusAprovacao.Aprovado);

            foreach (var traco in tracosFiltrados)
            {
                var aprovarTraco = true;

                var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
                var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

                for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
                {
                    var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                    if(pendente == null)
                        continue;

                    var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                    var numeroNotificacoes = pendente.Tracos.Where(x => x.ObraSeq == traco.Sequencia).Count();
                    var numeroAprovacoes = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.ObraSeq == traco.Sequencia).Count();
                    var numeroReprovacoes = pendente.Tracos.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado && x.ObraSeq == traco.Sequencia).Count();

                    if (numeroNotificacoes == 0)
                        continue;

                    if (numeroReprovacoes > 0)
                    {

                        pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        pendente.AprovacaoData = DateTime.Now;

                        traco.ReprovarPorAlcada(usuario);

                    }

                    aprovarTraco = aprovarTraco && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

                }

                if (aprovarTraco)
                    traco.Aprovar(usuario);

            }
        }

        public void ValidarAprovacoesObraBombaVersao(ObraVersao obra, string usuario, IEnumerable<AprovacaoComercialHierarquia> niveisHierarquia, IEnumerable<AprovacaoComercialPendente> aprovacoesPendentes)
        {
            var bombasFiltradas = obra.ObraBombas.Where(x => x.StatusAprovacao != EStatusAprovacao.NaoNecessita && x.StatusAprovacao != EStatusAprovacao.Aprovado);

            foreach (var bomba in bombasFiltradas)
            {

                var aprovarBomba = true;

                var nivelMin = aprovacoesPendentes.Min(x => x.NivelHierarquia);
                var nivelMax = aprovacoesPendentes.Max(x => x.NivelHierarquia);

                for (int nivel = nivelMin; nivel <= nivelMax; nivel++)
                {
                    var pendente = aprovacoesPendentes.Where(x => x.NivelHierarquia == nivel).FirstOrDefault();

                    var nivelHierarquia = niveisHierarquia.Where(x => x.NivelAutoridade == pendente.NivelHierarquia).FirstOrDefault();

                    var numeroNotificacoes = pendente.Bombas.Where(x => x.ObraSeq == bomba.Sequencia).Count();
                    var numeroAprovacoes = pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Aprovado && x.ObraSeq == bomba.Sequencia).Count();
                    var numeroReprovacoes = pendente.Bombas.Where(x => x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.Reprovado && x.ObraSeq == bomba.Sequencia).Count();

                    if (numeroNotificacoes == 0)
                        continue;

                    if (numeroReprovacoes > 0)
                    {

                        pendente.AprovacaoStatus = EAprovacaoComercialPendenteStatus.Reprovado;
                        pendente.AprovacaoData = DateTime.Now;

                        bomba.ReprovarPorAlcada(usuario);

                    }

                    aprovarBomba = aprovarBomba && (numeroReprovacoes == 0 && numeroAprovacoes >= numeroNotificacoes);

                }

                if (aprovarBomba)
                    bomba.Aprovar(usuario);

            }
        }

        public IEnumerable<AprovacaoComercialPendente> ListarTodasAprovacoesPorObraVersao(int obraUsina, int obraNumero, int obraVersao)
        {
            return _aprovacaoComercialPendenteRepository.ListarTodasAprovacoesPorObraVersao(obraUsina, obraNumero, obraVersao);
        }

        public IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentePorObraVersao(int obraUsina, int obraNumero, int obraVersao)
        {
            return _aprovacaoComercialPendenteRepository.ListarAprovacoesPendentePorObraVersao(obraUsina, obraNumero, obraVersao);
        }

        public void AdicionarAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente)
        {
            _aprovacaoComercialPendenteRepository.AdicionarAprovacaoPendenteTraco(pendente);
        }

        public void AdicionarAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente)
        {
            _aprovacaoComercialPendenteRepository.AdicionarAprovacaoPendenteBomba(pendente);
        }

        public void AdicionarAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente)
        {
            _aprovacaoComercialPendenteRepository.AdicionarAprovacaoPendenteVolume(pendente);
        }

        public void AdicionarAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente)
        {
            _aprovacaoComercialPendenteRepository.AdicionarAprovacaoPendenteCondicaoPagamento(pendente);
        }

        public void ForcarAprovacaoRegistrosAlcada(int obraUsina, int obraNumero, int obraVersao)
        {
            _aprovacaoComercialPendenteRepository.ForcarAprovacaoRegistrosAlcada(obraUsina, obraNumero, obraVersao);
        }

    }
}

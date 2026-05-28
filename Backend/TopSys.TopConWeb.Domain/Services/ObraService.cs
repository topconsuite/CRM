using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ObraService : ServiceBase<Obra>, IObraService
    {

        private readonly IObraRepository _obraRepository;
        private readonly IParametroFinanceiroRepository _parametroFinanceiroRepository;
        private readonly ICartaoTransacaoRepository _cartaoTransacaoRepository;
        private readonly IContasAReceberRepository _contasAReceberRepository;
        private readonly IMovimentoBancoRepository _movimentoBancoRepository;
        private readonly IContratoPagamentoRepository _contratoPagamentoRepository;
        private readonly IPropostaRepository _propostaRepository;
        private readonly IClicksignRepository _clicksignRepository;
        private readonly IContratoRepository _contratoRepository;
        
        private readonly IContasAReceberService _contasAReceberService;
        private readonly IObraTaxaService _obraTaxaService;
        private readonly IDemaisAprovacaoService _demaisAprovacaoService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly ITracoPrecoService _tracoPrecoService;
        private readonly ITracoCustoService _tracoCustoService;
        private readonly IBombaPrecoService _bombaPrecoService;
        private readonly IUsinaService _usinaService;
        private readonly IEnderecoService _enderecoService;
        private readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly IContratoService _contratoService;
        private readonly IUsinaDistanciaCepService _usinaDistanciaCepService;
        private readonly IParametroService _parametroService;
        private readonly ICustoServicoService _custoServicoService;
        private readonly ICadastroDiversoService _cadastroDiversoService;
        private readonly ICalculoImpostosService _calculoImpostosService;
        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly IParametroRepository _parametroRepository;
        private readonly IMercadoriaService _mercadoriaService;

        private readonly IContratoTracoReajusteService _contratoTracoReajusteService;
        private readonly IContratoBombaReajusteService _contratoBombaReajusteService;

        public ObraService(
            IObraRepository obraRepository,
            IParametroFinanceiroRepository parametroFinanceiroRepository,
            ICartaoTransacaoRepository cartaoTransacaoRepository,
            IContasAReceberRepository contasAReceberRepository,
            IMovimentoBancoRepository movimentoBancoRepository,
            IContratoPagamentoRepository contratoPagamentoRepository,
            IPropostaRepository propostaRepository,
            IClicksignRepository clicksignRepository,
            IContasAReceberService contasAReceberService,
            IObraTaxaService obraTaxaService,
            IDemaisAprovacaoService demaisAprovacaoService,
            IComercialLegacyService comercialLegacyService,
            IIntervenienteService intervenienteService,
            ITracoPrecoService tracoPrecoService,
            ITracoCustoService tracoCustoService,
            IBombaPrecoService bombaPrecoService,
            IUsinaService usinaService,
            IEnderecoService enderecoService,
            ICondicaoPagamentoService condicaoPagamentoService,
            IContratoService contratoService,
            IUsinaDistanciaCepService usinaDistanciaCepService,
            IParametroService parametroService,
            ICustoServicoService custoServicoService,
            ICadastroDiversoService cadastroDiversoService,
            IContratoRepository contratoRepository,
            ICalculoImpostosService calculoImpostosService,
            IAprovacaoComercialService aprovacaoComercialService,
            IParametroRepository parametroRepository,
            IContratoTracoReajusteService contratoTracoReajusteService,
            IContratoBombaReajusteService contratoBombaReajusteService,
            IMercadoriaService mercadoriaService
        ) : base(obraRepository)
        {
            _obraRepository = obraRepository;
            _parametroFinanceiroRepository = parametroFinanceiroRepository;
            _cartaoTransacaoRepository = cartaoTransacaoRepository;
            _contasAReceberRepository = contasAReceberRepository;
            _movimentoBancoRepository = movimentoBancoRepository;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _propostaRepository = propostaRepository;
            _clicksignRepository = clicksignRepository;

            _contasAReceberService = contasAReceberService;
            _obraTaxaService = obraTaxaService;
            _demaisAprovacaoService = demaisAprovacaoService;
            _comercialLegacyService = comercialLegacyService;
            _intervenienteService = intervenienteService;
            _tracoPrecoService = tracoPrecoService;
            _tracoCustoService = tracoCustoService;
            _bombaPrecoService = bombaPrecoService;
            _usinaService = usinaService;
            _enderecoService = enderecoService;
            _condicaoPagamentoService = condicaoPagamentoService;
            _contratoService = contratoService;
            _usinaDistanciaCepService = usinaDistanciaCepService;
            _parametroService = parametroService;
            _custoServicoService = custoServicoService;
            _cadastroDiversoService = cadastroDiversoService;
            _calculoImpostosService = calculoImpostosService;
            _aprovacaoComercialService = aprovacaoComercialService;

            _contratoRepository = contratoRepository;
            _parametroRepository = parametroRepository;

            _contratoTracoReajusteService = contratoTracoReajusteService;
            _contratoBombaReajusteService = contratoBombaReajusteService;
            _mercadoriaService = mercadoriaService;
        }

        public PagedList<Obra> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter)
        {
            return _obraRepository.ListarObraPorPaginaParaCarteira(pagina, porPagina, filter);
        }

        public void Adicionar(Obra obra, float valorExtras)
        {
            obra.VolumeEstimado = obra.ObraTracos.Sum(t => t.M3Quantidade);
            obra.Itinerante = "N";
            _obraRepository.Adicionar(obra, valorExtras);
        }

        public void Adicionar(ObraVersao obra, float valorExtras)
        {
            obra.VolumeEstimado = obra.ObraTracos.Sum(t => t.M3Quantidade);
            obra.Itinerante = "N";
            _obraRepository.Adicionar(obra, valorExtras);
        }

        public void Adicionar(DemaisAprovacao demaisAprovacao)
        {
            _obraRepository.Adicionar(demaisAprovacao);
        }

        public void Adicionar(AprovacaoScript aprovacaoScript)
        {
            _obraRepository.Adicionar(aprovacaoScript);
        }

        public IEnumerable<Obra> ListaPendentesDeAprovacao(string usuario)
        {

            Dictionary<string, Obra> listaObras = new Dictionary<string, Obra>();

            foreach (var obra in _obraRepository.ListarComTracoPendenteDeAprovacao(usuario))
                if (!listaObras.ContainsKey(obra.getChaveToString()))
                    listaObras.Add(obra.getChaveToString(), obra);

            foreach (var obra in _obraRepository.ListarComBombaPendenteDeAprovacao(usuario))
                if (!listaObras.ContainsKey(obra.getChaveToString()))
                    listaObras.Add(obra.getChaveToString(), obra);

            foreach (var obra in _obraRepository.ListarComTaxaExtraPendenteDeAprovacao())
                if (!listaObras.ContainsKey(obra.getChaveToString()))
                    listaObras.Add(obra.getChaveToString(), obra);

            foreach (var obra in _obraRepository.ListarComDemaisAprovacoesPendentes(usuario))
                if (!listaObras.ContainsKey(obra.getChaveToString()))
                    listaObras.Add(obra.getChaveToString(), obra);

            return new List<Obra>(listaObras.Values);
        }

        public bool TemAprovacaoPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            return _obraRepository.TemAprovacaoTracoPendente(usina, numero, anoChamada, noChamada)
                || _obraRepository.TemAprovacaoBombaPendente(usina, numero, anoChamada, noChamada)
                || _obraRepository.TemAprovacaoTaxaExtraPendente(usina, numero, anoChamada, noChamada)
                || _obraRepository.TemDemaisAprovacoesPendentes(usina, numero, anoChamada, noChamada);
        }

        public Obra ObtemPendentePorId(int usina, int numero, int anoChamada, int noChamada, string usuario)
        {
            var obra = _obraRepository.ObterPendentePorId(usina, numero, anoChamada, noChamada);

            //Recuperando as taxas extras referetnes à obra
            obra.ObraTaxas = _obraTaxaService.ListarByIdObra(obra.UsinaEntregaCodigo, obra.Numero);

            obra.DemaisAprovacoes = _demaisAprovacaoService.BuscarDemaisAprovacaoByIdObra(obra.UsinaCodigo, obra.Numero, usuario);

            foreach (var traco in obra.ObraTracos)
            {
                traco.AtualizaStatusAprovacao(usuario);
            }

            foreach (var bomba in obra.ObraBombas)
            {
                bomba.AtualizaStatusAprovacao(usuario);
            }

            return obra;
        }

        public ObraVersao ObtemPendentePorId(int numVersao, int usina, int numero, int anoChamada, int noChamada, string usuario)
        {
            var obra = _obraRepository.ObterPendentePorId(numVersao, usina, numero, anoChamada, noChamada);

            //Recuperando as taxas extras referetnes à obra
            obra.ObraTaxas = _obraTaxaService.ListarByIdObra(obra.UsinaEntregaCodigo, obra.Numero, numVersao);

            obra.DemaisAprovacoes = _demaisAprovacaoService.BuscarDemaisAprovacaoByIdObra(numVersao, obra.UsinaCodigo, obra.Numero, usuario);

            foreach (var traco in obra.ObraTracos)
            {
                traco.AtualizaStatusAprovacao(usuario);
            }

            foreach (var bomba in obra.ObraBombas)
            {
                bomba.AtualizaStatusAprovacao(usuario);
            }

            return obra;
        }

        public void AprovarObraPendente(string usuario, Obra obra)
        {

            //Recuperando a obra do banco
            var _obra = _obraRepository.ObterPendentePorId(obra.UsinaCodigo, obra.Numero, obra.AnoChamada, obra.NumChamada);

            //_obraRepository.AtualizarObraPendente(_obra);

            //TODO: Decupar em serviços específicos

            var sequencia_log = _obra.ObraLogs.Max(x => x.Sequencia);

            //Aprovação ObraTracos
            for (int i = 0; i < obra.ObraTracos.Count(); i++)
            {
                switch (obra.ObraTracos.ElementAt(i).StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        _obra.ObraTracos.ElementAt(i).Aprovar(usuario);
                        break;
                    case EStatusAprovacao.Reprovado:
                        var valorAdicional = ObterValoresAdicionaisObraTraco(_obra.ObraTracos.ElementAt(i), _obra.UsinaEntregaCodigo, _obra.Proposta?.Data ?? DateTime.Today,
                            _obra.EnderecoCep, _obra.DistanciaUsina, _obra.CondicaoPagamentoCodigo ?? 0, out TracoPreco tracoPreco, _obra);
                        _obra.ObraTracos.ElementAt(i).Reprovar(usuario, tracoPreco, valorAdicional);
                        break;
                    case EStatusAprovacao.Alterado:
                        float m3PrecoProposto = obra.ObraTracos.ElementAt(i).M3PrecoProposto;
                        _obra.ObraTracos.ElementAt(i).Alterar(usuario, m3PrecoProposto);
                        break;
                }

                if (!_obra.ObraTracos.ElementAt(i).Ativo.Equals(obra.ObraTracos.ElementAt(i).Ativo))
                {
                    _obra.ObraTracos.ElementAt(i).Ativo = obra.ObraTracos.ElementAt(i).Ativo;
                    sequencia_log += 1;
                    var sequenciaTraco = _obra.ObraTracos.ElementAt(i).Sequencia;
                    _obra.ObraLogs.Add(new ObraLog()
                    {
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = _obra.ObraTracos.ElementAt(i).Ativo == "S" ? "ALTERAÇÃO TRAÇO PARA ATIVO" : "ALTERAÇÃO TRAÇO PARA INATIVO",
                        AnoChamada = (int)_obra.AnoChamada,
                        NumChamada = (int)_obra.NumChamada,
                        Sequencia = sequencia_log,
                        Complemento = _obra.ObraTracos.ElementAt(i).Ativo == "S" ? $"Traço alterado de inativo para ativo. (Sequência {sequenciaTraco})" : $"Traço alterado de ativo para inativo. (Sequência {sequenciaTraco})",
                        Observacao = ""
                    });

                    var versaoAtual = ObterUltimaVersaoObra(_obra.UsinaCodigo, _obra.Numero);
                    if (versaoAtual != 0)
                    {
                        AtualizarTracoAtivoPropostaItemVersao(versaoAtual, _obra.UsinaCodigo, _obra.AnoChamada ?? 0, _obra.NumChamada ?? 0, _obra.ObraTracos.ElementAt(i).Sequencia, _obra.ObraTracos.ElementAt(i).Ativo);
                    }
                }
            }

            //Aprovacao ObraBombas
            for (int i = 0; i < obra.ObraBombas.Count(); i++)
            {
                switch (obra.ObraBombas.ElementAt(i).StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        _obra.ObraBombas.ElementAt(i).Aprovar(usuario);
                        break;
                    case EStatusAprovacao.Reprovado:
                        _obra.ObraBombas.ElementAt(i).Reprovar(usuario);
                        break;
                    case EStatusAprovacao.Alterado:
                        int m3PropostoAte = obra.ObraBombas.ElementAt(i).M3PropostoAte;
                        float taxaMinimaPrecoProposto = obra.ObraBombas.ElementAt(i).TaxaMinimaPrecoProposto;
                        float m3PrecoProposto = obra.ObraBombas.ElementAt(i).M3PrecoProposto;
                        _obra.ObraBombas.ElementAt(i).Alterar(usuario, m3PropostoAte, taxaMinimaPrecoProposto, m3PrecoProposto);
                        break;
                }

                if (!_obra.ObraBombas.ElementAt(i).Ativo.Equals(obra.ObraBombas.ElementAt(i).Ativo))
                {
                    _obra.ObraBombas.ElementAt(i).Ativo = obra.ObraBombas.ElementAt(i).Ativo;
                    sequencia_log += 1;
                    var sequenciaBomba = _obra.ObraBombas.ElementAt(i).Sequencia;
                    _obra.ObraLogs.Add(new ObraLog()
                    {
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = _obra.ObraBombas.ElementAt(i).Ativo == "S" ? "ALTERAÇÃO BOMBA PARA ATIVO" : "ALTERAÇÃO BOMBA PARA INATIVO",
                        AnoChamada = (int)_obra.AnoChamada,
                        NumChamada = (int)_obra.NumChamada,
                        Sequencia = sequencia_log,
                        Complemento = _obra.ObraBombas.ElementAt(i).Ativo == "S" ? $"Bomba alterado de inativo para ativo. (Sequência {sequenciaBomba})" : $"Bomba alterado de ativo para inativo. (Sequência {sequenciaBomba})",
                        Observacao = ""
                    });
                }

            }

            //Aprovação ObraTaxas
            if (obra.ObraTaxas != null)
                _obraTaxaService.AprovarTaxas(usuario, obra.ObraTaxas);

            foreach (var tracoLog in _obra.ObraTracos)
                _obraRepository.AdicionarLogPropostaItem(tracoLog, "ObraService.AprovarObraPendente");

        }

        public void AprovarObraPendente(string usuario, ObraVersao obra)
        {

            //Recuperando a obra do banco
            var _obra = _obraRepository.ObterPendentePorId(obra.NumeroVersao, obra.UsinaCodigo, obra.Numero, obra.AnoChamada, obra.NumChamada);
            _obra.NumeroVersao = obra.NumeroVersao;

            //_obraRepository.AtualizarObraPendente(_obra);

            //TODO: Decupar em serviços específicos

            var sequencia_log = _obra.ObraLogs.Max(x => x.Sequencia);

            //Aprovação ObraTracos
            for (int i = 0; i < obra.ObraTracos.Count(); i++)
            {
                switch (obra.ObraTracos.ElementAt(i).StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        _obra.ObraTracos.ElementAt(i).Aprovar(usuario);
                        break;
                    case EStatusAprovacao.Reprovado:
                        var valorAdicional = ObterValoresAdicionaisObraTraco(_obra.ObraTracos.ElementAt(i), _obra.UsinaEntregaCodigo, _obra.Proposta?.Data ?? DateTime.Today,
                            _obra.EnderecoCep, _obra.DistanciaUsina, _obra.CondicaoPagamentoCodigo ?? 0, out TracoPreco tracoPreco, _obra);
                        _obra.ObraTracos.ElementAt(i).Reprovar(usuario, tracoPreco, valorAdicional);
                        break;
                    case EStatusAprovacao.Alterado:
                        float m3PrecoProposto = obra.ObraTracos.ElementAt(i).M3PrecoProposto;
                        _obra.ObraTracos.ElementAt(i).Alterar(usuario, m3PrecoProposto);
                        break;
                }

                if (!_obra.ObraTracos.ElementAt(i).Ativo.Equals(obra.ObraTracos.ElementAt(i).Ativo))
                {
                    _obra.ObraTracos.ElementAt(i).Ativo = obra.ObraTracos.ElementAt(i).Ativo;
                    sequencia_log += 1;
                    var sequenciaTraco = _obra.ObraTracos.ElementAt(i).Sequencia;
                    _obra.ObraLogs.Add(new ObraLogVersao()
                    {
                        NumeroVersao = obra.NumeroVersao,
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = _obra.ObraTracos.ElementAt(i).Ativo == "S" ? "ALTERAÇÃO TRAÇO PARA ATIVO" : "ALTERAÇÃO TRAÇO PARA INATIVO",
                        AnoChamada = (int)_obra.AnoChamada,
                        NumChamada = (int)_obra.NumChamada,
                        Sequencia = sequencia_log,
                        Complemento = _obra.ObraTracos.ElementAt(i).Ativo == "S" ? $"Traço alterado de inativo para ativo. (Sequência {sequenciaTraco})" : $"Traço alterado de ativo para inativo. (Sequência {sequenciaTraco})",
                        Observacao = ""
                    });
                }
            }

            //Aprovacao ObraBombas
            for (int i = 0; i < obra.ObraBombas.Count(); i++)
            {
                switch (obra.ObraBombas.ElementAt(i).StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        _obra.ObraBombas.ElementAt(i).Aprovar(usuario);
                        break;
                    case EStatusAprovacao.Reprovado:
                        _obra.ObraBombas.ElementAt(i).Reprovar(usuario);
                        break;
                    case EStatusAprovacao.Alterado:
                        int m3PropostoAte = obra.ObraBombas.ElementAt(i).M3PropostoAte;
                        float taxaMinimaPrecoProposto = obra.ObraBombas.ElementAt(i).TaxaMinimaPrecoProposto;
                        float m3PrecoProposto = obra.ObraBombas.ElementAt(i).M3PrecoProposto;
                        _obra.ObraBombas.ElementAt(i).Alterar(usuario, m3PropostoAte, taxaMinimaPrecoProposto, m3PrecoProposto);
                        break;
                }

                if (!_obra.ObraBombas.ElementAt(i).Ativo.Equals(obra.ObraBombas.ElementAt(i).Ativo))
                {
                    _obra.ObraBombas.ElementAt(i).Ativo = obra.ObraBombas.ElementAt(i).Ativo;
                    sequencia_log += 1;
                    var sequenciaBomba = _obra.ObraBombas.ElementAt(i).Sequencia;
                    _obra.ObraLogs.Add(new ObraLogVersao()
                    {
                        NumeroVersao = obra.NumeroVersao,
                        UsinaCodigo = obra.UsinaCodigo,
                        ObraCodigo = obra.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = _obra.ObraBombas.ElementAt(i).Ativo == "S" ? "ALTERAÇÃO BOMBA PARA ATIVO" : "ALTERAÇÃO BOMBA PARA INATIVO",
                        AnoChamada = (int)_obra.AnoChamada,
                        NumChamada = (int)_obra.NumChamada,
                        Sequencia = sequencia_log,
                        Complemento = _obra.ObraBombas.ElementAt(i).Ativo == "S" ? $"Bomba alterado de inativo para ativo. (Sequência {sequenciaBomba})" : $"Bomba alterado de ativo para inativo. (Sequência {sequenciaBomba})",
                        Observacao = ""
                    });
                }

            }

            //Aprovação ObraTaxas
            if (obra.ObraTaxas != null)
                _obraTaxaService.AprovarTaxas(usuario, obra.ObraTaxas, obra.NumeroVersao);

            foreach (var tracoLog in _obra.ObraTracos)
                _obraRepository.AdicionarLogPropostaItem(tracoLog, "ObraService.AprovarObraPendente(Versão)");

        }

        public void FinalizarAprovacaoObraPendente(string usuario, Obra obra, bool logVolume = false, bool logDemaisCondicao = false)
        {
            //Definindo a coleção responsável por armazenar os logs
            List<ObraLogDado> logs = new List<ObraLogDado>();

            foreach (var obraTraco in obra.ObraTracos ?? new List<ObraTraco>())
            {

                var mercadoria = _mercadoriaService.ListarFiltrados(x => x.NumeracaoProduto == obraTraco.NumeracaoProduto).FirstOrDefault();
                var observacao = $"Sequência {obraTraco.Sequencia}{(mercadoria is null ? "" : $" - {mercadoria.Descricao}")} - R$ {obraTraco.M3PrecoProposto.ToString("n2")} ({obraTraco.DescontoPercentual.ToString("n2")}%)";

                switch (obraTraco.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "TRAÇO APROVADO", Observacao = observacao });
                        break;
                    case EStatusAprovacao.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "TRAÇO REPROVADO", Observacao = observacao });
                        break;
                    case EStatusAprovacao.Alterado:
                        logs.Add(new ObraLogDado() { Operacao = "V", Complemento = "TRAÇO ALTERADO", Observacao = observacao });
                        break;
                }
            }


            foreach (var obraBomba in obra.ObraBombas ?? new List<ObraBomba>())
            {

                var descricaoBomba = $"Sequência {obraBomba.Sequencia} - {obraBomba.BombaTipo.Descricao}";

                switch (obraBomba.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "BOMBA APROVADA", Observacao = descricaoBomba });
                        break;
                    case EStatusAprovacao.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "BOMBA REPROVADA", Observacao = descricaoBomba });
                        break;
                    case EStatusAprovacao.Alterado:
                        logs.Add(new ObraLogDado() { Operacao = "V", Complemento = "BOMBA ALTERADA", Observacao = descricaoBomba });
                        break;
                }

            }


            foreach (var obraTaxa in obra.ObraTaxas)
            {
                switch (obraTaxa.StatusAprovacao)
                {
                    case (EStatusAprovacao.Aprovado):
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "TAXA APROVADA", Observacao = obraTaxa.LogObservacao });
                        break;
                    case (EStatusAprovacao.Reprovado):
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "TAXA REPROVADA", Observacao = obraTaxa.LogObservacao });
                        break;
                }
            }

            /*  foreach (var demaisAprovacao in obra.DemaisAprovacoes)
              {
                  switch (demaisAprovacao.StatusAprovacao)
                  {
                      case EStatusAprovacao.Aprovado:
                          logs.Add(new ObraLogDado() { Operacao = "A", Complemento = "APROVADA", Observacao = demaisAprovacao.LogObservacao });
                          break;
                      case EStatusAprovacao.Reprovado:
                          logs.Add(new ObraLogDado() { Operacao = "R", Complemento = "REPROVADA", Observacao = demaisAprovacao.LogObservacao });
                          break;
                  }
              }*/

            if (logVolume)
            {
                switch (obra.VolumeStatusComercial)
                {
                    case EObraDemaisStatusComercial.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "VOLUME APROVADO", Observacao = $"{obra.ObraTracos.Sum(x => x.M3Quantidade).ToString("N1")} M3" });
                        break;
                    case EObraDemaisStatusComercial.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "VOLUME REPROVADO", Observacao = $"{obra.ObraTracos.Sum(x => x.M3Quantidade).ToString("N1")} M3" });
                        break;
                }
            }

            if (logDemaisCondicao)
            {

                var condicaoPagamento = obra.CondicaoPagamento is null ? _condicaoPagamentoService.ObterPeloId(obra.CondicaoPagamentoCodigo ?? 0) : obra.CondicaoPagamento;
                var descricao = $"Condição {obra.CondicaoPagamentoCodigo}{(condicaoPagamento is null ? "" : $" - {condicaoPagamento.Descricao}")} (Média de {condicaoPagamento.MediaDias} dias)";

                switch (obra.CondicaoPagamentoStatusComercial)
                {
                    case EObraDemaisStatusComercial.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "COND. DE PAGTO APROVADA", Observacao = descricao });
                        break;
                    case EObraDemaisStatusComercial.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "COND. DE PAGTO REPROVADA", Observacao = descricao });
                        break;
                }
            }

            _comercialLegacyService.FinalizarAprovacoesComerciais(usuario, obra.getChaveToString(), logs);
        }

        public void FinalizarAprovacaoObraPendente(string usuario, ObraVersao obra, bool logVolume = false, bool logDemaisCondicao = false)
        {
            //Definindo a coleção responsável por armazenar os logs
            List<ObraLogDado> logs = new List<ObraLogDado>();

            foreach (var obraTraco in obra.ObraTracos ?? new List<ObraTracoVersao>())
            {

                var mercadoria = _mercadoriaService.ListarFiltrados(x => x.NumeracaoProduto == obraTraco.NumeracaoProduto).FirstOrDefault();
                var observacao = $"Sequência {obraTraco.Sequencia}{(mercadoria is null ? "" : $" - {mercadoria.Descricao}")} - R$ {obraTraco.M3PrecoProposto.ToString("n2")} ({obraTraco.DescontoPercentual.ToString("n2")}%)";

                switch (obraTraco.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "TRAÇO APROVADO", Observacao = observacao });
                        break;
                    case EStatusAprovacao.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "TRAÇO REPROVADO", Observacao = observacao });
                        break;
                    case EStatusAprovacao.Alterado:
                        logs.Add(new ObraLogDado() { Operacao = "V", Complemento = "TRAÇO ALTERADO", Observacao = observacao });
                        break;
                }
            }


            foreach (var obraBomba in obra.ObraBombas ?? new List<ObraBombaVersao>())
            {

                var descricaoBomba = $"Sequência {obraBomba.Sequencia} - {obraBomba.BombaTipo.Descricao}";

                switch (obraBomba.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "BOMBA APROVADA", Observacao = descricaoBomba });
                        break;
                    case EStatusAprovacao.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "BOMBA REPROVADA", Observacao = descricaoBomba });
                        break;
                    case EStatusAprovacao.Alterado:
                        logs.Add(new ObraLogDado() { Operacao = "V", Complemento = "BOMBA ALTERADA", Observacao = descricaoBomba });
                        break;
                }

            }


            foreach (var obraTaxa in obra.ObraTaxas ?? new List<ObraTaxaVersao>())
            {
                switch (obraTaxa.StatusAprovacao)
                {
                    case (EStatusAprovacao.Aprovado):
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "TAXA APROVADA", Observacao = obraTaxa.LogObservacao });
                        break;
                    case (EStatusAprovacao.Reprovado):
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "TAXA REPROVADA", Observacao = obraTaxa.LogObservacao });
                        break;
                }
            }

            foreach (var demaisAprovacao in obra.DemaisAprovacoes ?? new List<DemaisAprovacao>())
            {
                switch (demaisAprovacao.StatusAprovacao)
                {
                    case EStatusAprovacao.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "A", Complemento = "APROVADA", Observacao = demaisAprovacao.LogObservacao });
                        break;
                    case EStatusAprovacao.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "R", Complemento = "REPROVADA", Observacao = demaisAprovacao.LogObservacao });
                        break;
                }
            }

            if(logVolume)
            {
                switch (obra.VolumeStatusComercial)
                {
                    case EObraDemaisStatusComercial.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "VOLUME APROVADO", Observacao = $"{obra.ObraTracos.Sum(x => x.M3Quantidade).ToString("N1")} M3"});
                        break;
                    case EObraDemaisStatusComercial.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "VOLUME REPROVADO", Observacao = $"{obra.ObraTracos.Sum(x => x.M3Quantidade).ToString("N1")} M3" });
                        break;
                }
            }

            if (logDemaisCondicao)
            {

                var condicaoPagamento = obra.CondicaoPagamento is null ? _condicaoPagamentoService.ObterPeloId(obra.CondicaoPagamentoCodigo ?? 0) : obra.CondicaoPagamento;
                var descricao = $"Condição {obra.CondicaoPagamentoCodigo}{(condicaoPagamento is null ? "" : $" - {condicaoPagamento.Descricao}")} (Média de {condicaoPagamento.MediaDias} dias)";

                switch (obra.CondicaoPagamentoStatusComercial)
                {
                    case EObraDemaisStatusComercial.Aprovado:
                        logs.Add(new ObraLogDado() { Operacao = "S", Complemento = "COND. DE PAGTO APROVADA", Observacao = descricao });
                        break;
                    case EObraDemaisStatusComercial.Reprovado:
                        logs.Add(new ObraLogDado() { Operacao = "X", Complemento = "COND. DE PAGTO REPROVADA", Observacao = descricao });
                        break;
                }
            }

            _comercialLegacyService.FinalizarAprovacoesComerciaisVersao(usuario, obra.getChaveVersaoToString(), logs);
        }

        public IEnumerable<ObraLog> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada)
        {
            return _obraRepository.ListarObraLogsPorId(usina, numero, anoChamada, noChamada);
        }

        public IEnumerable<ObraLogVersao> ListarObraLogsPorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada)
        {
            return _obraRepository.ListarObraLogsPorId(numVersao, usina, numero, anoChamada, noChamada);
        }

        public float ObterValoresAdicionaisObraTraco(ObraTraco obraTraco, int usinaEntregaCodigo, DateTime dataBase, string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, out TracoPreco tracoPreco, Obra obra)
        {
            tracoPreco = _tracoPrecoService
                    .ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    usinaEntregaCodigo, obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo,
                    obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, obra);

            //if (!obraTraco.TracoPrecoTabelaIsValid(tracoPreco)) return 0f;
            var m3Preco = obraTraco.M3PrecoTabela;
            if (tracoPreco != null)
                m3Preco = tracoPreco.M3Preco;

            var valorAdicionalM3PorVolume = _tracoPrecoService
                .ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(usinaEntregaCodigo, obraTraco.M3Quantidade, m3Preco);

            var valorAdicionalM3PorCep = _enderecoService
                .ObterValorAdicionalM3PorUsinaCep(usinaEntregaCodigo, obraCep) ?? 0f;

            var valorAdicionalM3PorDistancia = _usinaService
                .ObterValorAdicionalM3PorUsinaKm(usinaEntregaCodigo, distanciaUsina) ?? 0f;

            var valorAdicionalM3PorCondicaoPagamento = _condicaoPagamentoService
                .ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(condicaoPagamentoCodigo, usinaEntregaCodigo, m3Preco);

            return valorAdicionalM3PorVolume + valorAdicionalM3PorCep + valorAdicionalM3PorDistancia + valorAdicionalM3PorCondicaoPagamento;
        }

        public float ObterValoresAdicionaisObraTraco(ObraTracoVersao obraTraco, int usinaEntregaCodigo, DateTime dataBase, string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, out TracoPreco tracoPreco, ObraVersao obra)
        {
            tracoPreco = _tracoPrecoService
                    .ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    usinaEntregaCodigo, obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo,
                    obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, obra);

            //if (!obraTraco.TracoPrecoTabelaIsValid(tracoPreco)) return 0f;
            var m3Preco = obraTraco.M3PrecoTabela;
            if (tracoPreco != null)
                m3Preco = tracoPreco.M3Preco;

            var valorAdicionalM3PorVolume = _tracoPrecoService
                .ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(usinaEntregaCodigo, obraTraco.M3Quantidade, m3Preco);

            var valorAdicionalM3PorCep = _enderecoService
                .ObterValorAdicionalM3PorUsinaCep(usinaEntregaCodigo, obraCep) ?? 0f;

            var valorAdicionalM3PorDistancia = _usinaService
                .ObterValorAdicionalM3PorUsinaKm(usinaEntregaCodigo, distanciaUsina) ?? 0f;

            var valorAdicionalM3PorCondicaoPagamento = _condicaoPagamentoService
                .ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(condicaoPagamentoCodigo, usinaEntregaCodigo, m3Preco);

            return valorAdicionalM3PorVolume + valorAdicionalM3PorCep + valorAdicionalM3PorDistancia + valorAdicionalM3PorCondicaoPagamento;
        }

        public void ValidarObraTraco(string usuario, ObraTraco obraTraco, int usinaEntregaCodigo, DateTime dataBase,
            string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, float percentualDescontoLimite, Obra obra, ref bool aprovacaoComercialPendente, ref bool hasNotifications)
        {
            var valorAdicional = ObterValoresAdicionaisObraTraco(obraTraco, usinaEntregaCodigo, dataBase, obraCep, distanciaUsina, condicaoPagamentoCodigo, out TracoPreco tracoPreco, obra);

            var novaAprovacaoComerical = _aprovacaoComercialService.ObterPorUsina(usinaEntregaCodigo);
            var utilizaNovaAprovacaoComercial = novaAprovacaoComerical != null ? novaAprovacaoComerical.Ativo : false;

            //if (!obraTraco.TracoPrecoTabelaIsValid(tracoPreco)) return;

            //obraTraco.M3PrecoTabela = tracoPreco.M3Preco;
            var m3Preco = obraTraco.M3PrecoTabela;
            if (tracoPreco != null)
                m3Preco = tracoPreco.M3Preco;

            var valorSugerido = m3Preco + valorAdicional;

            //var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(usinaEntregaCodigo,
            //    obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo
            //    );

            obraTraco.DescontoPercentual = (1f - (obraTraco.M3PrecoProposto / (obraTraco.M3PrecoAjustado == 0 ? obraTraco.M3PrecoTabela : obraTraco.M3PrecoAjustado))) * 100f;

            obraTraco.ValorServico = (obraTraco.M3PrecoTabela - (tracoPreco?.CustoMaterial ?? 0f)) - (obraTraco.M3PrecoTabela - obraTraco.M3PrecoProposto);

            var necessitaAprovacao = !obraTraco.DescontoMaximoScopeIsValid(valorSugerido, percentualDescontoLimite);

            if (obraTraco.PrecoReajustadoAtual != 0 && !necessitaAprovacao)
            {
                var contratoReajuste = _contratoService.ListarFiltrados<ContratoTracoReajuste>(t => t.UsinaCodigo == obraTraco.Obra.UsinaCodigo
                       && t.ContratoAno == obraTraco.Obra.AnoContrato && t.ContratoNumero == obraTraco.Obra.NumContrato && t.ObraTracoSequencia == obraTraco.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();
                if (contratoReajuste != null)
                {
                    necessitaAprovacao = !obraTraco.DescontoMaximoPrecoReajustadoScopeIsValid(contratoReajuste.PrecoRecalculado, percentualDescontoLimite);
                }
            }

            if (utilizaNovaAprovacaoComercial)
            {
                obraTraco.DescontoSolicitante = usuario;
                hasNotifications = necessitaAprovacao && obraTraco.AprovacaoObservacao == "" && obraTraco.Justificativa == "";

                if (obraTraco.AprovacaoVerbal == "N" && !hasNotifications)
                {
                    aprovacaoComercialPendente = true;
                    obraTraco.DescontoSolicitante = usuario;
                }

                return;
            }

            if ( !necessitaAprovacao && obraTraco.AprovacaoObservacao == "")
            {
                obraTraco.Justificativa = "";
                obraTraco.AprovacaoTipo = "";
                obraTraco.AprovacaoVerbal = "";
                obraTraco.DescontoSolicitante = "";
            }
            else if (obraTraco.Justificativa != "" &&  obraTraco.AprovacaoObservacao == "")
            {
                obraTraco.AprovacaoTipo = "G";
                obraTraco.AprovacaoVerbal = "N";
                obraTraco.DescontoSolicitante = usuario;

                aprovacaoComercialPendente = true;
            }
            else if (obraTraco.AprovacaoObservacao == "")
            {
                hasNotifications = true;
            }

        }

        public void ValidarObraTraco(string usuario, ObraTracoVersao obraTraco, int usinaEntregaCodigo, DateTime dataBase,
            string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, float percentualDescontoLimite,
            ObraVersao obra, ref bool aprovacaoComercialPendente, ref bool hasNotifications)
        {
            var valorAdicional = ObterValoresAdicionaisObraTraco(obraTraco, usinaEntregaCodigo, dataBase, obraCep, distanciaUsina, condicaoPagamentoCodigo, out TracoPreco tracoPreco, obra);

            //if (!obraTraco.TracoPrecoTabelaIsValid(tracoPreco)) return;

            var novaAprovacaoComerical = _aprovacaoComercialService.ObterPorUsina(usinaEntregaCodigo);
            var utilizaNovaAprovacaoComercial = novaAprovacaoComerical != null ? novaAprovacaoComerical.Ativo : false;

            //obraTraco.M3PrecoTabela = tracoPreco.M3Preco;
            var m3Preco = obraTraco.M3PrecoTabela;
            if (tracoPreco != null)
                m3Preco = tracoPreco.M3Preco;

            var valorSugerido = m3Preco + valorAdicional;

            //var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(usinaEntregaCodigo,
            //    obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo
            //    );

            obraTraco.DescontoPercentual = (1f - (obraTraco.M3PrecoProposto / (obraTraco.M3PrecoAjustado == 0 ? obraTraco.M3PrecoTabela : obraTraco.M3PrecoAjustado))) * 100f;

            obraTraco.ValorServico = (obraTraco.M3PrecoTabela - (tracoPreco?.CustoMaterial ?? 0f)) - (obraTraco.M3PrecoTabela - obraTraco.M3PrecoProposto);

            var necessitaAprovacao = !obraTraco.DescontoMaximoScopeIsValid(valorSugerido, percentualDescontoLimite);

            if (obraTraco.PrecoReajustadoAtual != 0 && !necessitaAprovacao)
            {
                var contratoReajuste = _contratoService.ListarFiltrados<ContratoTracoReajusteVersao>(t => t.NumeroVersao == obraTraco.NumeroVersao && t.UsinaCodigo == obraTraco.Obra.UsinaCodigo
                       && t.ContratoAno == obraTraco.Obra.AnoContrato && t.ContratoNumero == obraTraco.Obra.NumContrato && t.ObraTracoSequencia == obraTraco.Sequencia)
                        .OrderByDescending(t => t.DataVigencia).FirstOrDefault();
                if (contratoReajuste != null)
                {
                    necessitaAprovacao = !obraTraco.DescontoMaximoPrecoReajustadoScopeIsValid(contratoReajuste.PrecoRecalculado, percentualDescontoLimite);
                }
            }

            if (utilizaNovaAprovacaoComercial)
            {
                obraTraco.DescontoSolicitante = usuario;
                hasNotifications = necessitaAprovacao && obraTraco.AprovacaoObservacao == "" && obraTraco.Justificativa == "";

                if (obraTraco.AprovacaoVerbal == "N" && !hasNotifications)
                {
                    aprovacaoComercialPendente = true;
                    obraTraco.DescontoSolicitante = usuario;
                }

                return;
            }

            if (!necessitaAprovacao && obraTraco.AprovacaoObservacao == "")
            {
                obraTraco.Justificativa = "";
                obraTraco.AprovacaoTipo = "";
                obraTraco.AprovacaoVerbal = "";
                obraTraco.DescontoSolicitante = "";
            }
            else if (obraTraco.Justificativa != "" && obraTraco.AprovacaoObservacao == "")
            {
                obraTraco.AprovacaoTipo = "G";
                obraTraco.AprovacaoVerbal = "N";
                obraTraco.DescontoSolicitante = usuario;

                aprovacaoComercialPendente = true;
            }
            else if (obraTraco.AprovacaoObservacao == "")
            {
                hasNotifications = true;
            }

            

        }

        public void ValidarObraBomba(string usuario, ObraBomba obraBomba, int usinaEntregaCodigo, DateTime dataBase, ref bool aprovacaoComercialPendente, ref bool hasNotifications)
        {
            const string NOME_INTERVENIENTE_TERCEIRO_DEFAULT = "BOMBA DE TERCEIRO";

            var novaAprovacaoComerical = _aprovacaoComercialService.ObterPorUsina(usinaEntregaCodigo);
            var utilizaNovaAprovacaoComercial = novaAprovacaoComerical != null ? novaAprovacaoComerical.Ativo : false;

            IBombaPreco bombaPreco;
            if (obraBomba.BombaPropria)
                bombaPreco = _bombaPrecoService.ObterPorUsinaBombaTipoData(usinaEntregaCodigo, obraBomba.BombaTipoCodigo ?? 0, dataBase);
            else
                bombaPreco = _bombaPrecoService.ObterPorBombistaBombaTipoData(obraBomba.TerceiroCodigo ?? 0, obraBomba.BombaTipoCodigo ?? 0, dataBase);

            if (obraBomba.BombaPropria || !obraBomba.FaturamentoDireto)
                if (!obraBomba.BombaPrecoTabelaIsValid(bombaPreco)) return;

            if (!obraBomba.BombaPropria && obraBomba.FaturamentoDireto)
            {
                var terceiroDefault = _intervenienteService.ObterPorNome(NOME_INTERVENIENTE_TERCEIRO_DEFAULT);
                obraBomba.TerceiroCodigo = terceiroDefault?.Codigo ?? 0;
            }

            if (obraBomba.BombaPropria)
                obraBomba.ValorAdicionalTubulacao = _bombaPrecoService.ObterValorAdicional(usinaEntregaCodigo, obraBomba.BombaTipoCodigo ?? 0, obraBomba.DistanciaTubulacao);
            else
                obraBomba.ValorAdicionalTubulacao = 0f;

            if (obraBomba.AprovacaoObservacao == "")
            {
                obraBomba.M3PrecoTabela = bombaPreco?.M3Preco ?? 0f;
                obraBomba.M3TabelaAte = bombaPreco?.M3Ate ?? 0;
                obraBomba.TaxaMinimaPrecoTabela = bombaPreco?.TaxaMinimaPreco ?? 0f;

                obraBomba.HoraPrecoTabela = bombaPreco?.HoraPreco ?? 0f;
                obraBomba.HoraTabelaAte = bombaPreco?.HoraAte ?? 0;
                obraBomba.HoraTaxaMinimaPrecoTabela = bombaPreco?.HoraTaxaMinimaPreco ?? 0f;
            }

            obraBomba.TipoCalculo = bombaPreco?.TipoCalculo ?? 0;
            if ((obraBomba.BombaPropria || !obraBomba.FaturamentoDireto) && obraBomba.TipoCalculo != EBombaM3CalculoTipo.SemCobranca)
            {

                var percentualDescontoTaxaMinima = (1 - (obraBomba.TaxaMinimaPrecoProposto / obraBomba.TaxaMinimaPrecoTabela)) * 100;
                var percentualDescontoM3 = (1 - (obraBomba.M3PrecoProposto / obraBomba.M3PrecoTabela)) * 100;

                obraBomba.DescontoPercentual = percentualDescontoM3 > percentualDescontoTaxaMinima ? percentualDescontoM3 : percentualDescontoTaxaMinima;

            } else
                obraBomba.DescontoPercentual = 0f;

            obraBomba.HoraTipoCalculo = bombaPreco?.HoraTipoCalculo ?? 0;
            if (obraBomba.BombaPropria && obraBomba.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca)
                obraBomba.HoraDescontoPercentual = (1 - (obraBomba.HoraTaxaMinimaPrecoProposto / obraBomba.HoraTaxaMinimaPrecoTabela)) * 100;
            else
                obraBomba.HoraDescontoPercentual = 0f;

            var necessitaAprovacao = !obraBomba.DescontoMaximoScopeIsValid(bombaPreco);

            if (utilizaNovaAprovacaoComercial)
            {
                obraBomba.DescontoSolicitante = usuario;
                hasNotifications = necessitaAprovacao && obraBomba.AprovacaoObservacao == "" && obraBomba.Justificativa == "";

                if (obraBomba.AprovacaoVerbal == "S" && !hasNotifications)
                {
                    aprovacaoComercialPendente = true;
                    obraBomba.DescontoSolicitante = usuario;
                }

                return;
            }

            if (!necessitaAprovacao && obraBomba.AprovacaoObservacao == "")
            {
                obraBomba.Justificativa = "";
                obraBomba.AprovacaoVerbal = "";
                obraBomba.DescontoSolicitante = "";
            }
            else if (obraBomba.Justificativa != "" && obraBomba.AprovacaoObservacao == "" && obraBomba.AprovacaoVerbal != "N")
            {
                obraBomba.AprovacaoVerbal = "S";
                obraBomba.DescontoSolicitante = usuario;

                aprovacaoComercialPendente = true;
            }
            else if (obraBomba.AprovacaoObservacao == "" && obraBomba.AprovacaoVerbal != "N")
            {
                hasNotifications = true;
            }
        }

        public void ValidarObraBomba(string usuario, ObraBombaVersao obraBomba, int usinaEntregaCodigo, DateTime dataBase, ref bool aprovacaoComercialPendente, ref bool hasNotifications)
        {
            const string NOME_INTERVENIENTE_TERCEIRO_DEFAULT = "BOMBA DE TERCEIRO";

            var novaAprovacaoComerical = _aprovacaoComercialService.ObterPorUsina(usinaEntregaCodigo);
            var utilizaNovaAprovacaoComercial = novaAprovacaoComerical != null ? novaAprovacaoComerical.Ativo : false;

            IBombaPreco bombaPreco;
            if (obraBomba.BombaPropria)
                bombaPreco = _bombaPrecoService.ObterPorUsinaBombaTipoData(usinaEntregaCodigo, obraBomba.BombaTipoCodigo ?? 0, dataBase);
            else
                bombaPreco = _bombaPrecoService.ObterPorBombistaBombaTipoData(obraBomba.TerceiroCodigo ?? 0, obraBomba.BombaTipoCodigo ?? 0, dataBase);

            if (obraBomba.BombaPropria || !obraBomba.FaturamentoDireto)
                if (!obraBomba.BombaPrecoTabelaIsValid(bombaPreco)) return;

            if (!obraBomba.BombaPropria && obraBomba.FaturamentoDireto)
            {
                var terceiroDefault = _intervenienteService.ObterPorNome(NOME_INTERVENIENTE_TERCEIRO_DEFAULT);
                obraBomba.TerceiroCodigo = terceiroDefault?.Codigo ?? 0;
            }

            if (obraBomba.BombaPropria)
                obraBomba.ValorAdicionalTubulacao = _bombaPrecoService.ObterValorAdicional(usinaEntregaCodigo, obraBomba.BombaTipoCodigo ?? 0, obraBomba.DistanciaTubulacao);
            else
                obraBomba.ValorAdicionalTubulacao = 0f;

            if (obraBomba.AprovacaoObservacao == "")
            {
                obraBomba.M3PrecoTabela = bombaPreco?.M3Preco ?? 0f;
                obraBomba.M3TabelaAte = bombaPreco?.M3Ate ?? 0;
                obraBomba.TaxaMinimaPrecoTabela = bombaPreco?.TaxaMinimaPreco ?? 0f;

                obraBomba.HoraPrecoTabela = bombaPreco?.HoraPreco ?? 0f;
                obraBomba.HoraTabelaAte = bombaPreco?.HoraAte ?? 0;
                obraBomba.HoraTaxaMinimaPrecoTabela = bombaPreco?.HoraTaxaMinimaPreco ?? 0f;
            }

            obraBomba.TipoCalculo = bombaPreco?.TipoCalculo ?? 0;
            if ((obraBomba.BombaPropria || !obraBomba.FaturamentoDireto) && obraBomba.TipoCalculo != EBombaM3CalculoTipo.SemCobranca) { 

                var percentualDescontoTaxaMinima = (1 - (obraBomba.TaxaMinimaPrecoProposto / obraBomba.TaxaMinimaPrecoTabela)) * 100;
                var percentualDescontoM3 = (1 - (obraBomba.M3PrecoProposto / obraBomba.M3PrecoTabela)) * 100;

                obraBomba.DescontoPercentual = percentualDescontoM3 > percentualDescontoTaxaMinima ? percentualDescontoM3 : percentualDescontoTaxaMinima;

            } else
                obraBomba.DescontoPercentual = 0f;

            obraBomba.HoraTipoCalculo = bombaPreco?.HoraTipoCalculo ?? 0;
            if (obraBomba.BombaPropria && obraBomba.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca)
                obraBomba.HoraDescontoPercentual = (1 - (obraBomba.HoraTaxaMinimaPrecoProposto / obraBomba.HoraTaxaMinimaPrecoTabela)) * 100;
            else
                obraBomba.HoraDescontoPercentual = 0f;

            var necessitaAprovacao = !obraBomba.DescontoMaximoScopeIsValid(bombaPreco);

            if (utilizaNovaAprovacaoComercial)
            {
                obraBomba.DescontoSolicitante = usuario;
                hasNotifications = necessitaAprovacao && obraBomba.AprovacaoObservacao == "" && obraBomba.Justificativa == "";

                if (obraBomba.AprovacaoVerbal == "S" && !hasNotifications)
                {
                    aprovacaoComercialPendente = true;
                    obraBomba.DescontoSolicitante = usuario;
                }

                return;
            }

            if (!necessitaAprovacao && obraBomba.AprovacaoObservacao == "")
            {
                obraBomba.Justificativa = "";
                obraBomba.AprovacaoVerbal = "";
                obraBomba.DescontoSolicitante = "";
            }
            else if (obraBomba.Justificativa != "" && obraBomba.AprovacaoObservacao == "" && obraBomba.AprovacaoVerbal != "N")
            {
                obraBomba.AprovacaoVerbal = "S";
                obraBomba.DescontoSolicitante = usuario;

                aprovacaoComercialPendente = true;
            }
            else if (obraBomba.AprovacaoObservacao == "" && obraBomba.AprovacaoVerbal != "N")
            {
                hasNotifications = true;
            }
        }

        public void ValidarObraTaxa(string usuario, ObraTaxa obraTaxa, ref bool aprovacaoComercialPendente)
        {
            if (obraTaxa.Nova && obraTaxa.Selecionada == "N") return;

            if (obraTaxa.Selecionada == "S" && obraTaxa.IsPersonalizada && obraTaxa.Tipo == "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
            {
                if (obraTaxa.Aprovada == "")
                {
                    obraTaxa.Aprovada = "N";
                    aprovacaoComercialPendente = true;
                }
                obraTaxa.AprovacaoSolicitante = usuario;
            }
            else if ((obraTaxa.Selecionada == "N" || obraTaxa.IsPersonalizada) && obraTaxa.Tipo != "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
            {
                if (obraTaxa.Aprovada == "")
                {
                    obraTaxa.Aprovada = "N";
                    aprovacaoComercialPendente = true;
                }
                obraTaxa.AprovacaoSolicitante = usuario;
            }
            else
            {
                if (obraTaxa.Selecionada == "S")
                {
                    obraTaxa.AprovacaoSolicitante = "";
                    obraTaxa.AprovacaoUsuario = "";
                    obraTaxa.AprovacaoCiente = "";
                }
            }
        }

        public void ValidarObraTaxa(string usuario, ObraTaxaVersao obraTaxa, ref bool aprovacaoComercialPendente)
        {
            if (obraTaxa.Nova && obraTaxa.Selecionada == "N") return;

            if (obraTaxa.Selecionada == "S" && obraTaxa.IsPersonalizada && obraTaxa.Tipo == "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
            {
                if (obraTaxa.Aprovada == "")
                {
                    obraTaxa.Aprovada = "N";
                    aprovacaoComercialPendente = true;
                }
                obraTaxa.AprovacaoSolicitante = usuario;
            }
            else if ((obraTaxa.Selecionada == "N" || obraTaxa.IsPersonalizada) && obraTaxa.Tipo != "ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO")
            {
                if (obraTaxa.Aprovada == "")
                {
                    obraTaxa.Aprovada = "N";
                    aprovacaoComercialPendente = true;
                }
                obraTaxa.AprovacaoSolicitante = usuario;
            }
            else
            {
                if (obraTaxa.Selecionada == "S")
                {
                    obraTaxa.AprovacaoSolicitante = "";
                    obraTaxa.AprovacaoUsuario = "";
                    obraTaxa.AprovacaoCiente = "";
                }
            }
        }

        public void AtualizarEnderecoProgramacoesFuturas(int usina, int obraNumero)
        {
            _obraRepository.AtualizarEnderecoProgramacoesFuturas(usina, obraNumero);
        }

        public void AtualizarStatusComercial(int usina, int obraNumero)
        {
            _obraRepository.AtualizarStatusComercial(usina, obraNumero);
        }

        public void AtualizarStatusComercial(int usina, int obraNumero, int numVersao)
        {
            _obraRepository.AtualizarStatusComercial(usina, obraNumero, numVersao);
        }

        public void AtualizarStatusComercial(Obra obra, bool utilizaAprovacaoPorAlcada = false)
        {

            var temReprovacao = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.VolumeStatusComercial == EObraDemaisStatusComercial.Reprovado);
            temReprovacao |= (obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Reprovado);

            var temPendente = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.VolumeStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao);
            temPendente |= (obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao);

            temPendente = temPendente && utilizaAprovacaoPorAlcada;

            var novoStatus = EObraStatusComercial.Aprovado;

            if (temReprovacao)
                novoStatus = EObraStatusComercial.Reprovado;
            else if (temPendente)
                novoStatus = EObraStatusComercial.Aguardando;

            _obraRepository.AtualizarStatusComercial(obra, novoStatus);
        }

        public void AtualizarStatusComercial(ObraVersao obra, bool utilizaAprovacaoPorAlcada = false)
        {
            var temReprovacao = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.VolumeStatusComercial == EObraDemaisStatusComercial.Reprovado);
            temReprovacao |= (obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Reprovado);

            var temPendente = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Pendente) ?? 0) > 0;
            temPendente |= (obra.VolumeStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao);
            temPendente |= (obra.CondicaoPagamentoStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao);

            temPendente = temPendente && utilizaAprovacaoPorAlcada;

            var novoStatus = EObraStatusComercial.Aprovado;

            if (temReprovacao)
                novoStatus = EObraStatusComercial.Reprovado;
            else if (temPendente)
                novoStatus = EObraStatusComercial.Aguardando;

            _obraRepository.AtualizarStatusComercial(obra, novoStatus);            
        }

        public IEnumerable<Obra> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao)
        {
            return _obraRepository.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, obraNumero, numeroCartao, autorizacao);
        }

        public Obra ListarObraPagamentos(int idUsina, int obraNumero)
        {
            var obra = _obraRepository.ListarObraPagamentos(idUsina, obraNumero);

            foreach (var pagamento in obra.ContratoPagamentos)
                pagamento.AtualizaStatusAprovacao();

            return obra;
        }

        public ObraVersao ListarObraPagamentos(int numVersao, int idUsina, int obraNumero)
        {
            var obra = _obraRepository.ListarObraPagamentos(idUsina, obraNumero, numVersao);

            foreach (var pagamento in obra.ContratoPagamentos)
                pagamento.AtualizaStatusAprovacao();

            return obra;
        }

        public void AprovarObraPagamentos(string usuario, Obra obra, IEnumerable<MovimentoBanco> movimentosBancoAVincular)
        {
            var obraAtual = _obraRepository.ListarObraPagamentos(obra.UsinaCodigo, obra.Numero);

            if (!obra.PagamentosModificadosIsValid(obraAtual))
                return;

            var mesAberto = _parametroFinanceiroRepository.ObterMesAbertoPorEmpresa(obraAtual.UsinaEntrega.EmpresaCodigo);
            var sequenciaLog = 1;

            for (var i = 0; i < obra.ContratoPagamentos?.Count; i++)
            {
                var pagamentoNovo = obra.ContratoPagamentos.ElementAt(i);
                var pagamentoAtual = obraAtual.ContratoPagamentos.ElementAt(i);

                if (!pagamentoNovo.PagamentoModificadoIsValid(pagamentoAtual))
                    return;

                var isValid = true;


                if (pagamentoNovo.StatusAprovacao == EStatusAprovacao.Aprovado)
                {
                    foreach (var detalhe in pagamentoNovo.Detalhes)
                    {
                        isValid &= detalhe.PagamentoDetalheDataFuturaIsValid();
                        isValid &= detalhe.PagamentoDetalheMesFechadoIsValid(mesAberto);

                        int portadorCod;
                        if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhe.GetType()))
                            portadorCod = (((ContratoPagamentoDetalheDeposito)detalhe).Portador ?? pagamentoNovo.TipoCobranca.Portador).Codigo;
                        else
                            portadorCod = pagamentoNovo.TipoCobranca.Portador.Codigo;

                        var PortadorParaVerificacao = _obraRepository.ObterPorId<Portador>(portadorCod);

                        if ((PortadorParaVerificacao?.ContaEmpresaCodigo ?? 0) != 0)
                        {
                            isValid &= obraAtual.EmpresaPortadorIsValid(PortadorParaVerificacao, pagamentoNovo);
                        }

                        // se gera movimento de banco
                        if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhe.GetType())
                            || typeof(ContratoPagamentoDetalheDinheiro).Equals(detalhe.GetType()))
                        {
                            Portador portador;
                            if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhe.GetType()))
                                portador = (((ContratoPagamentoDetalheDeposito)detalhe).Portador ?? pagamentoNovo.TipoCobranca.Portador);
                            else
                                portador = pagamentoNovo.TipoCobranca.Portador;

                            var conta = _obraRepository.ObterPorId<Conta>(portador?.ContaEmpresaCodigo ?? 0, portador?.ContaCodigo ?? 0);
                            isValid &= conta.ContaPortadorIsValid(pagamentoNovo);

                            isValid &= detalhe.PagamentoDetalheDataContaConciliadaIsValid(pagamentoNovo, obra.ContratoPagamentos, conta, movimentosBancoAVincular.Count() > 0);

                            if (movimentosBancoAVincular.Count() > 0)
                            {
                                List<MovimentoBanco> movimentosBancoAtual = new List<MovimentoBanco>();
                                var datasOperacao = movimentosBancoAVincular.Select(t => t.DataOperacao).Distinct();

                                foreach (var dataOperacao in datasOperacao)
                                {
                                    movimentosBancoAtual.AddRange(_movimentoBancoRepository.ListarNaoVinculadosComContasAReceber(portador?.ContaEmpresaCodigo ?? 0, portador?.ContaCodigo ?? 0, dataOperacao));
                                }

                                isValid &= detalhe.PagamentoDetalheVinculoMovimentoBancoIsValid(movimentosBancoAVincular, movimentosBancoAtual);
                            }
                        }

                        if (!isValid) continue;

                        if (typeof(ContratoPagamentoDetalheCartao).Equals(detalhe.GetType()))
                        {
                            var cartao = (ContratoPagamentoDetalheCartao)detalhe;

                            var transacao = _cartaoTransacaoRepository.ObterPorDataNumeroCartaoAutorizacao(cartao.DataTransacao, cartao.NumeroCartao, cartao.NumeroAutorizacao);

                            if (transacao == null)
                            {
                                var bandeira = _obraRepository.ObterPorId<CartaoBandeira>(cartao.BandeiraCodigo);

                                var produtoNome = "";
                                var subProdutoNome = "";

                                switch (pagamentoNovo.TipoCobranca.Forma)
                                {
                                    case "CC":
                                        produtoNome = "CREDITO";
                                        subProdutoNome = (cartao.QuantidadeParcelas > 1 ? "CREDITO PARCELADO LOJA" : "CREDITO A VISTA");
                                        break;
                                    case "CD":
                                        produtoNome = "DEBITO";
                                        subProdutoNome = "DEBITO A VISTA";
                                        break;
                                }

                                transacao = new CartaoTransacao
                                {
                                    TransacaoId = Guid.NewGuid(),
                                    Origem = "manual",
                                    PedidoId = Guid.NewGuid(),
                                    Status = "CONFIRMED",
                                    EstabelecimentoCod = bandeira.EstabelecimentoCod,
                                    AutorizacaoNumero = cartao.NumeroAutorizacao,
                                    TransacaoDataHora = cartao.DataTransacao,
                                    Valor = cartao.Valor,
                                    TransacaoTipo = "PAYMENT",
                                    ProdutoNome = produtoNome,
                                    SubProdutoNome = subProdutoNome,
                                    QuantidadeParcelas = cartao.QuantidadeParcelas,
                                    CartaoNumero = cartao.NumeroCartaoAsString,
                                    StatusProcesso = 0
                                };

                                _cartaoTransacaoRepository.Adicionar(transacao);
                            }

                            var car = _contasAReceberRepository
                                .ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(cartao.NumeroCartaoAsString, cartao.NumeroAutorizacao, cartao.DataTransacao.Year)
                                .Where(t => t.DocumentoTipo == ((int)EDocumentoTipo.ContasAReceberOperadora)
                                    && t.Alocado != ((int)EContasAReceberStatusAlocado.Vinculado))
                                .FirstOrDefault();

                            if (car == null)
                            {
                                if (!_contasAReceberService.GeraContasAReceberDaOperadora(transacao.TransacaoId.ToString()))
                                    return;
                            }

                            _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(obra.UsinaCodigo, obra.Numero, pagamentoNovo.Sequencia, usuario);
                        }
                        else if (typeof(ContratoPagamentoDetalheCheque).Equals(detalhe.GetType()))
                        {
                            var cheque = _contratoPagamentoRepository.ObterDetalheCheque(pagamentoAtual, detalhe.DetalheSequencia, true);
                            if (cheque.DataRecebimento == null)
                            {
                                cheque.DataRecebimento = DateTime.Today;
                                _obraRepository.SaveChanges();
                            }

                            var parametrosCheque = _parametroFinanceiroRepository.ObterParametroChequePorEmpresa(obraAtual.UsinaEntrega.EmpresaCodigo);

                            var carChequeTerceiro = new ContasAReceber(obraAtual, cheque, parametrosCheque, usuario);
                            _contasAReceberRepository.InsereContasAReceber(carChequeTerceiro);
                        }

                        if (obraAtual.UsinaEntrega.GeraCreditoClientePagamentoAntecipado
                            && !typeof(ContratoPagamentoDetalheBoleto).Equals(detalhe.GetType())
                            && !typeof(ContratoPagamentoDetalheCartao).Equals(detalhe.GetType()))
                        {
                            var operacaoCreditoCliente = _parametroFinanceiroRepository.ObterOperacaoRecebimentoDoClientePeloCodigoUsina(obraAtual.UsinaEntregaCodigo);
                            var carCreditoCliente = new ContasAReceber(obraAtual, pagamentoNovo, detalhe, operacaoCreditoCliente, usuario);
                            _contasAReceberRepository.InsereContasAReceber(carCreditoCliente);

                            if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhe.GetType())
                                || typeof(ContratoPagamentoDetalheDinheiro).Equals(detalhe.GetType()))
                            {
                                if (movimentosBancoAVincular.Count() == 0)
                                {
                                    var operacaoMovimentoBancoAdiantamento = _parametroFinanceiroRepository.ObterOperacaoMovimentoBancoAdiantamentoCliente(obraAtual.UsinaEntregaCodigo);

                                    var movimentoBanco = new MovimentoBanco(obraAtual, pagamentoNovo, detalhe, operacaoMovimentoBancoAdiantamento, usuario);
                                    if (!movimentoBanco.OperacaoScopeIsValid()) return;
                                    _movimentoBancoRepository.Adicionar(movimentoBanco);

                                    carCreditoCliente.IdMovimentoBanco = movimentoBanco.Id;
                                    _contasAReceberRepository.AtualizarIdMovimentoBanco(carCreditoCliente);

                                    _contasAReceberRepository.VincularContasAReceberComMovimentoBanco(carCreditoCliente, movimentoBanco, detalhe.Valor);
                                }
                                else
                                {
                                    var saldo = Math.Round(detalhe.Valor, 2);

                                    foreach (var movimentoBanco in movimentosBancoAVincular)
                                    {
                                        if (saldo == 0.00)
                                            break;

                                        var valor = Math.Round(Math.Min(saldo, movimentoBanco.Valor), 2);

                                        _contasAReceberRepository.VincularContasAReceberComMovimentoBanco(carCreditoCliente, movimentoBanco, (float)valor);

                                        saldo -= valor;
                                    }
                                }

                            }
                        }

                    }

                    var pagamento = _obraRepository.ObterPorId<ContratoPagamentoForSaving>(pagamentoAtual.UsinaCodigo, pagamentoAtual.ContratoAno, pagamentoAtual.ContratoNumero, pagamentoNovo.Sequencia);
                    pagamento.IdAprovacao = StringHelper.GetIDD(usuario);
                    var valorTotalCondicao = pagamentoNovo.Detalhes?.Sum(t => t.Valor) ?? 0;
                    if (valorTotalCondicao > 0) pagamento.Valor = valorTotalCondicao;
                    _obraRepository.SaveChanges();

                    var pagamentoProposta = _obraRepository.ObterPorId<PropostaPagamento>(obraAtual.UsinaCodigo, obraAtual.AnoChamada, obraAtual.NumChamada, obraAtual.Numero, pagamentoNovo.Sequencia);
                    if (pagamentoProposta != null)
                    {
                        pagamentoProposta.IdAprovacao = StringHelper.GetIDD(usuario);
                        if (valorTotalCondicao > 0) pagamentoProposta.Valor = valorTotalCondicao;
                        _obraRepository.SaveChanges();
                    }

                    _obraRepository.Adicionar(new ObraLog
                    {
                        UsinaCodigo = obraAtual.UsinaCodigo,
                        ObraCodigo = obraAtual.Numero,
                        AnoChamada = obraAtual.AnoChamada ?? 0,
                        NumChamada = obraAtual.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ACOMPANHAMENTO",
                        Complemento = $"Aprovação de pagamento",
                        Observacao = $"PAGAMENTO SEQUENCIA: {pagamentoNovo.Sequencia} / {pagamentoNovo.CondicaoPagamento.Descricao} - {pagamentoNovo.TipoCobranca.Descricao} / Valor: {pagamentoNovo.Valor} / Aprovado pelo Web",
                        Sequencia = sequenciaLog++
                    });
                    _obraRepository.SaveChanges();
                }
                else if (pagamentoNovo.StatusAprovacao == EStatusAprovacao.Reprovado)
                {
                    var pagamento = _obraRepository.ObterPorId<ContratoPagamentoForSaving>(pagamentoAtual.UsinaCodigo, pagamentoAtual.ContratoAno, pagamentoAtual.ContratoNumero, pagamentoNovo.Sequencia);
                    pagamento.NecessitaAprovacaoSimNao = "";
                    _obraRepository.SaveChanges();

                    _obraRepository.Adicionar(new ObraLog
                    {
                        UsinaCodigo = obraAtual.UsinaCodigo,
                        ObraCodigo = obraAtual.Numero,
                        AnoChamada = obraAtual.AnoChamada ?? 0,
                        NumChamada = obraAtual.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ACOMPANHAMENTO",
                        Complemento = $"Reprovação de pagamento",
                        Observacao = $"{pagamentoNovo.CondicaoPagamento.Descricao}: {(pagamentoNovo.LogObservacao ?? "")}",
                        Sequencia = sequenciaLog++
                    });
                    _obraRepository.SaveChanges();
                }
            }

            AtualizarStatusFinanceiro(obra, usuario);
        }

        public void AprovarObraPagamentos(string usuario, ObraVersao obra, IEnumerable<MovimentoBanco> movimentosBancoAVincular)
        {
            var obraAtual = _obraRepository.ListarObraPagamentos(obra.UsinaCodigo, obra.Numero, obra.NumeroVersao);

            if (!obra.PagamentosModificadosIsValid(obraAtual))
                return;

            var mesAberto = _parametroFinanceiroRepository.ObterMesAbertoPorEmpresa(obraAtual.UsinaEntrega.EmpresaCodigo);
            var sequenciaLog = 1;

            for (var i = 0; i < obra.ContratoPagamentos?.Count; i++)
            {
                var pagamentoNovo = obra.ContratoPagamentos.ElementAt(i);
                var pagamentoAtual = obraAtual.ContratoPagamentos.ElementAt(i);

                if (!pagamentoNovo.PagamentoModificadoIsValid(pagamentoAtual))
                    return;

                var isValid = true;



                if (pagamentoNovo.StatusAprovacao == EStatusAprovacao.Aprovado)
                {
                    foreach (var detalhe in pagamentoNovo.Detalhes)
                    {
                        isValid &= detalhe.PagamentoDetalheDataFuturaIsValid();
                        isValid &= detalhe.PagamentoDetalheMesFechadoIsValid(mesAberto);

                        int portadorCod;
                        if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhe.GetType()))
                            portadorCod = (((ContratoPagamentoDetalheDepositoVersao)detalhe).Portador ?? pagamentoNovo.TipoCobranca.Portador).Codigo;
                        else
                            portadorCod = pagamentoNovo.TipoCobranca.Portador.Codigo;

                        var PortadorParaVerificacao = _obraRepository.ObterPorId<Portador>(portadorCod);

                        if ((PortadorParaVerificacao?.ContaEmpresaCodigo ?? 0) != 0)
                        {
                            isValid &= obraAtual.EmpresaPortadorIsValid(PortadorParaVerificacao, pagamentoNovo);
                        }

                        // se gera movimento de banco
                        if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhe.GetType())
                            || typeof(ContratoPagamentoDetalheDinheiroVersao).Equals(detalhe.GetType()))
                        {
                            Portador portador;
                            if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhe.GetType()))
                                portador = (((ContratoPagamentoDetalheDepositoVersao)detalhe).Portador ?? pagamentoNovo.TipoCobranca.Portador);
                            else
                                portador = pagamentoNovo.TipoCobranca.Portador;

                            var conta = _obraRepository.ObterPorId<Conta>(portador?.ContaEmpresaCodigo ?? 0, portador?.ContaCodigo ?? 0);
                            isValid &= conta.ContaPortadorIsValid(pagamentoNovo);

                            isValid &= detalhe.PagamentoDetalheDataContaConciliadaIsValid(pagamentoNovo, obra.ContratoPagamentos, conta, movimentosBancoAVincular.Count() > 0);

                            if (movimentosBancoAVincular.Count() > 0)
                            {
                                List<MovimentoBanco> movimentosBancoAtual = new List<MovimentoBanco>();
                                var datasOperacao = movimentosBancoAVincular.Select(t => t.DataOperacao).Distinct();

                                foreach (var dataOperacao in datasOperacao)
                                {
                                    movimentosBancoAtual.AddRange(_movimentoBancoRepository.ListarNaoVinculadosComContasAReceber(portador?.ContaEmpresaCodigo ?? 0, portador?.ContaCodigo ?? 0, dataOperacao));
                                }

                                isValid &= detalhe.PagamentoDetalheVinculoMovimentoBancoIsValid(movimentosBancoAVincular, movimentosBancoAtual);
                            }
                        }

                        if (!isValid) continue;

                        if (typeof(ContratoPagamentoDetalheCartaoVersao).Equals(detalhe.GetType()))
                        {
                            var cartao = (ContratoPagamentoDetalheCartaoVersao)detalhe;

                            var transacao = _cartaoTransacaoRepository.ObterPorDataNumeroCartaoAutorizacao(cartao.DataTransacao, cartao.NumeroCartao, cartao.NumeroAutorizacao);

                            if (transacao == null)
                            {
                                var bandeira = _obraRepository.ObterPorId<CartaoBandeira>(cartao.BandeiraCodigo);

                                var produtoNome = "";
                                var subProdutoNome = "";

                                switch (pagamentoNovo.TipoCobranca.Forma)
                                {
                                    case "CC":
                                        produtoNome = "CREDITO";
                                        subProdutoNome = (cartao.QuantidadeParcelas > 1 ? "CREDITO PARCELADO LOJA" : "CREDITO A VISTA");
                                        break;
                                    case "CD":
                                        produtoNome = "DEBITO";
                                        subProdutoNome = "DEBITO A VISTA";
                                        break;
                                }

                                transacao = new CartaoTransacao
                                {
                                    TransacaoId = Guid.NewGuid(),
                                    Origem = "manual",
                                    PedidoId = Guid.NewGuid(),
                                    Status = "CONFIRMED",
                                    EstabelecimentoCod = bandeira.EstabelecimentoCod,
                                    AutorizacaoNumero = cartao.NumeroAutorizacao,
                                    TransacaoDataHora = cartao.DataTransacao,
                                    Valor = cartao.Valor,
                                    TransacaoTipo = "PAYMENT",
                                    ProdutoNome = produtoNome,
                                    SubProdutoNome = subProdutoNome,
                                    QuantidadeParcelas = cartao.QuantidadeParcelas,
                                    CartaoNumero = cartao.NumeroCartaoAsString,
                                    StatusProcesso = 0
                                };

                                _cartaoTransacaoRepository.Adicionar(transacao);
                            }

                            var car = _contasAReceberRepository
                                .ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(cartao.NumeroCartaoAsString, cartao.NumeroAutorizacao, cartao.DataTransacao.Year)
                                .Where(t => t.DocumentoTipo == ((int)EDocumentoTipo.ContasAReceberOperadora)
                                    && t.Alocado != ((int)EContasAReceberStatusAlocado.Vinculado))
                                .FirstOrDefault();

                            if (car == null)
                            {
                                if (!_contasAReceberService.GeraContasAReceberDaOperadora(transacao.TransacaoId.ToString()))
                                    return;
                            }

                            _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(obra.NumeroVersao, obra.UsinaCodigo, obra.Numero, pagamentoNovo.Sequencia, usuario);//**
                        }
                        else if (typeof(ContratoPagamentoDetalheChequeVersao).Equals(detalhe.GetType()))
                        {
                            var cheque = _contratoPagamentoRepository.ObterDetalheCheque(pagamentoAtual, detalhe.DetalheSequencia, true);
                            if (cheque.DataRecebimento == null)
                            {
                                cheque.DataRecebimento = DateTime.Today;
                                _obraRepository.SaveChanges();
                            }

                            var parametrosCheque = _parametroFinanceiroRepository.ObterParametroChequePorEmpresa(obraAtual.UsinaEntrega.EmpresaCodigo);

                            var carChequeTerceiro = new ContasAReceber(obraAtual, cheque, parametrosCheque, usuario);
                            _contasAReceberRepository.InsereContasAReceber(carChequeTerceiro);
                        }

                        if (obraAtual.UsinaEntrega.GeraCreditoClientePagamentoAntecipado
                            && !typeof(ContratoPagamentoDetalheBoletoVersao).Equals(detalhe.GetType())
                            && !typeof(ContratoPagamentoDetalheCartaoVersao).Equals(detalhe.GetType()))
                        {
                            var operacaoCreditoCliente = _parametroFinanceiroRepository.ObterOperacaoRecebimentoDoClientePeloCodigoUsina(obraAtual.UsinaEntregaCodigo);
                            var carCreditoCliente = new ContasAReceber(obraAtual, pagamentoNovo, detalhe, operacaoCreditoCliente, usuario);
                            _contasAReceberRepository.InsereContasAReceber(carCreditoCliente);

                            if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhe.GetType())
                                || typeof(ContratoPagamentoDetalheDinheiroVersao).Equals(detalhe.GetType()))
                            {
                                if (movimentosBancoAVincular.Count() == 0)
                                {
                                    var operacaoMovimentoBancoAdiantamento = _parametroFinanceiroRepository.ObterOperacaoMovimentoBancoAdiantamentoCliente(obraAtual.UsinaEntregaCodigo);
                                    //
                                    var movimentoBanco = new MovimentoBanco(obraAtual, pagamentoNovo, detalhe, operacaoMovimentoBancoAdiantamento, usuario);
                                    if (!movimentoBanco.OperacaoScopeIsValid()) return;
                                    _movimentoBancoRepository.Adicionar(movimentoBanco);

                                    carCreditoCliente.IdMovimentoBanco = movimentoBanco.Id;
                                    _contasAReceberRepository.AtualizarIdMovimentoBanco(carCreditoCliente);

                                    _contasAReceberRepository.VincularContasAReceberComMovimentoBanco(carCreditoCliente, movimentoBanco, detalhe.Valor);
                                }
                                else
                                {
                                    var saldo = Math.Round(detalhe.Valor, 2);

                                    foreach (var movimentoBanco in movimentosBancoAVincular)
                                    {
                                        if (saldo == 0.00)
                                            break;

                                        var valor = Math.Round(Math.Min(saldo, movimentoBanco.Valor), 2);

                                        _contasAReceberRepository.VincularContasAReceberComMovimentoBanco(carCreditoCliente, movimentoBanco, (float)valor);

                                        saldo -= valor;
                                    }
                                }

                            }
                        }

                    }

                    var pagamento = _obraRepository.ObterPorId<ContratoPagamentoForSavingVersao>(pagamentoAtual.NumeroVersao, pagamentoAtual.UsinaCodigo, pagamentoAtual.ContratoAno, pagamentoAtual.ContratoNumero, pagamentoNovo.Sequencia);
                    pagamento.IdAprovacao = StringHelper.GetIDD(usuario);
                    var valorTotalCondicao = pagamentoNovo.Detalhes?.Sum(t => t.Valor) ?? 0;
                    if (valorTotalCondicao > 0) pagamento.Valor = valorTotalCondicao;
                    _obraRepository.SaveChanges();

                    var pagamentoProposta = _obraRepository.ObterPorId<PropostaPagamentoVersao>(obraAtual.NumeroVersao, obraAtual.UsinaCodigo, obraAtual.AnoChamada, obraAtual.NumChamada, obraAtual.Numero, pagamentoNovo.Sequencia);
                    if (pagamentoProposta != null)
                    {
                        pagamentoProposta.IdAprovacao = StringHelper.GetIDD(usuario);
                        if (valorTotalCondicao > 0) pagamentoProposta.Valor = valorTotalCondicao;
                        _obraRepository.SaveChanges();
                    }

                    _obraRepository.Adicionar(new ObraLogVersao
                    {
                        NumeroVersao = obraAtual.NumeroVersao,
                        UsinaCodigo = obraAtual.UsinaCodigo,
                        ObraCodigo = obraAtual.Numero,
                        AnoChamada = obraAtual.AnoChamada ?? 0,
                        NumChamada = obraAtual.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ACOMPANHAMENTO",
                        Complemento = $"Aprovação de pagamento",
                        Observacao = $"PAGAMENTO SEQUENCIA: {pagamentoNovo.Sequencia} / {pagamentoNovo.CondicaoPagamento.Descricao} - {pagamentoNovo.TipoCobranca.Descricao} / Valor: {pagamentoNovo.Valor} / Aprovado pelo Web",
                        Sequencia = sequenciaLog++
                    });
                    _obraRepository.SaveChanges();
                }
                else if (pagamentoNovo.StatusAprovacao == EStatusAprovacao.Reprovado)
                {
                    var pagamento = _obraRepository.ObterPorId<ContratoPagamentoForSavingVersao>(pagamentoAtual.NumeroVersao, pagamentoAtual.UsinaCodigo, pagamentoAtual.ContratoAno, pagamentoAtual.ContratoNumero, pagamentoNovo.Sequencia);
                    pagamento.NecessitaAprovacaoSimNao = "";
                    _obraRepository.SaveChanges();

                    _obraRepository.Adicionar(new ObraLogVersao
                    {
                        NumeroVersao = obraAtual.NumeroVersao,
                        UsinaCodigo = obraAtual.UsinaCodigo,
                        ObraCodigo = obraAtual.Numero,
                        AnoChamada = obraAtual.AnoChamada ?? 0,
                        NumChamada = obraAtual.NumChamada ?? 0,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ACOMPANHAMENTO",
                        Complemento = $"Reprovação de pagamento",
                        Observacao = $"{pagamentoNovo.CondicaoPagamento.Descricao}: {(pagamentoNovo.LogObservacao ?? "")}",
                        Sequencia = sequenciaLog++
                    });
                    _obraRepository.SaveChanges();
                }
            }

            AtualizarStatusFinanceiro(obra, usuario);
        }

        public void AtualizarStatusFinanceiro(Obra obra, string usuario)
        {
            var parametroDesativarObrigatorioedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var obraAtual = _obraRepository.ListarObraPagamentos(obra.UsinaCodigo, obra.Numero);

            obraAtual.ContratoPagamentos = obraAtual.ContratoPagamentos.Where(t => t.Ativo).ToList();

            obraAtual.PropostaPagamentos = obraAtual.PropostaPagamentos.Where(t => t.Ativo).ToList();

            var aguardandoDadosPagamento = false;
            var aguardandoConfirmacaoPagamento = false;
            var aprovado = false;
            if (obraAtual.NumContrato != 0)
            {
                // cado não tem nenhum registro na con_contrato_pag
                aguardandoDadosPagamento = (obraAtual.ContratoPagamentos?.Count ?? 0) == 0
                    // ou existe pelo menos uma condição antecipada que não é 'BL'(boleto) nem 'CT'(carteira) que não foi informado detalhamento
                    || obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && (t.Detalhes?.Count ?? 0) == 0 && t.Forma != "CT" && t.Forma != "BL").Count() > 0
                    // ou existe pelo menos uma condição que foi reprovada
                    || obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "" && t.IdAprovacao == "").Count() > 0
                    // ou o valor total dos pagamentos é menor que o valor total do contrato
                    || Math.Round((float)obraAtual.Contrato.ValorTotalContrato - 10f, 2) > Math.Round(obraAtual.ContratoPagamentos.Sum(t => t.Valor), 2);

                aguardandoConfirmacaoPagamento = (obraAtual.ContratoPagamentos?.Count ?? 0) > 0
                    && obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao == "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;

                aprovado = (obraAtual.ContratoPagamentos?.Count ?? 0) > 0
                    && obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao != "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
            }
            else
            {
                // cado não tem nenhum registro na con_contrato_pag
                aguardandoDadosPagamento = (obraAtual.PropostaPagamentos?.Count ?? 0) == 0
                    // ou existe pelo menos uma condição antecipada que não é 'BL'(boleto) nem 'CT'(carteira) que não foi informado detalhamento
                    || obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && (t.Detalhes?.Count ?? 0) == 0 && t.Forma != "CT" && t.Forma != "BL").Count() > 0
                    // ou existe pelo menos uma condição que foi reprovada
                    || obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "" && t.IdAprovacao == "").Count() > 0
                    // ou o valor total dos pagamentos é menor que o valor total do contrato
                    || Math.Round((float)obraAtual.Proposta.ValorTotalContrato - 10f, 2) > Math.Round(obraAtual.PropostaPagamentos.Sum(t => t.Valor), 2);

                aguardandoConfirmacaoPagamento = (obraAtual.PropostaPagamentos?.Count ?? 0) > 0
                    && obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao == "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
                
                aprovado = (obraAtual.PropostaPagamentos?.Count ?? 0) > 0
                    && obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao != "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
            }
            
            var obraSalvar = _obraRepository.ObterPorId(obraAtual.UsinaCodigo, obraAtual.Numero);

            if (aguardandoConfirmacaoPagamento)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.AguardandoConfirmacao)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.AguardandoConfirmacao;
                    _obraRepository.SaveChanges();
                }
            }
            else if (aguardandoDadosPagamento)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.AguardandoDadosPagamento)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.AguardandoDadosPagamento;
                    _obraRepository.SaveChanges();
                }
            }
            else if (aprovado)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.Aprovado)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.Aprovado;
                    _obraRepository.SaveChanges();
                }

                var contrato = _contratoService.ObterPorId(obra.UsinaCodigo, obra.AnoContrato, obra.NumContrato);

                if (contrato.Status == EContratoStatus.AguardandoConfirmacaoPagamento || contrato.Status == EContratoStatus.AguardandoDadosPagamento)
                    TentarAprovarCadastroEContrato(contrato, usuario, false);
            }
            else
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.NaoNecessita)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.NaoNecessita;
                    _obraRepository.SaveChanges();
                }
            }

            var contratoAtual = _contratoService.ObterPorId(obra.UsinaCodigo, obra.AnoContrato, obra.NumContrato);
            if (contratoAtual != null)
                if (contratoAtual.Status == EContratoStatus.AguardandoDadosPagamento || contratoAtual.Status == EContratoStatus.AguardandoConfirmacaoPagamento || parametroDesativarObrigatorioedadeAprovacaoCadastro)
                    _comercialLegacyService.ValidarContrato(contratoAtual, usuario, out string mensagens, aprovContratoDirAuto);
        }

        public void AtualizarStatusFinanceiro(ObraVersao obra, string usuario)
        {
            var parametroDesativarObrigatorioedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var aprovContratoDirAuto = _parametroRepository.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");

            var obraAtual = _obraRepository.ListarObraPagamentos(obra.UsinaCodigo, obra.Numero, obra.NumeroVersao);

            obraAtual.ContratoPagamentos = obraAtual.ContratoPagamentos.Where(t => t.Ativo).ToList();

            obraAtual.PropostaPagamentos = obraAtual.PropostaPagamentos.Where(t => t.Ativo).ToList();

            var aguardandoDadosPagamento = false;
            var aguardandoConfirmacaoPagamento = false;
            var aprovado = false;
            if (obraAtual.NumContrato != 0)
            {
                // cado não tem nenhum registro na con_contrato_pag
                aguardandoDadosPagamento = (obraAtual.ContratoPagamentos?.Count ?? 0) == 0
                    // ou existe pelo menos uma condição antecipada que não é 'BL'(boleto) nem 'CT'(carteira) que não foi informado detalhamento
                    || obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && (t.Detalhes?.Count ?? 0) == 0 && t.Forma != "CT" && t.Forma != "BL").Count() > 0
                    // ou existe pelo menos uma condição que foi reprovada
                    || obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "" && t.IdAprovacao == "").Count() > 0
                    // ou o valor total dos pagamentos é menor que o valor total do contrato
                    || Math.Round((float)obraAtual.Contrato.ValorTotalContrato - 10f, 2) > Math.Round(obraAtual.ContratoPagamentos.Sum(t => t.Valor), 2);

                aguardandoConfirmacaoPagamento = (obraAtual.ContratoPagamentos?.Count ?? 0) > 0
                && obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao == "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;

                aprovado = (obraAtual.ContratoPagamentos?.Count ?? 0) > 0
                    && obraAtual.ContratoPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao != "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
            }
            else
            {
                // cado não tem nenhum registro na con_contrato_pag
                aguardandoDadosPagamento = (obraAtual.PropostaPagamentos?.Count ?? 0) == 0
                    // ou existe pelo menos uma condição antecipada que não é 'BL'(boleto) nem 'CT'(carteira) que não foi informado detalhamento
                    || obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && (t.Detalhes?.Count ?? 0) == 0 && t.Forma != "CT" && t.Forma != "BL").Count() > 0
                    // ou existe pelo menos uma condição que foi reprovada
                    || obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "" && t.IdAprovacao == "").Count() > 0
                    // ou o valor total dos pagamentos é menor que o valor total do contrato
                    || Math.Round((float)obraAtual.Proposta.ValorTotalContrato - 10f, 2) > Math.Round(obraAtual.PropostaPagamentos.Sum(t => t.Valor), 2);

                aguardandoConfirmacaoPagamento = (obraAtual.PropostaPagamentos?.Count ?? 0) > 0
                && obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao == "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;

                aprovado = (obraAtual.PropostaPagamentos?.Count ?? 0) > 0
                    && obraAtual.PropostaPagamentos.Where(t => t.NecessitaAprovacaoSimNao == "S" && t.IdAprovacao != "" && (t.Forma == "CT" || (t.Detalhes?.Count ?? 0) > 0)).Count() > 0;
            }

            var obraSalvar = _obraRepository.ObterPorId<ObraVersao>(obraAtual.NumeroVersao, obraAtual.UsinaCodigo, obraAtual.Numero);

            if (aguardandoConfirmacaoPagamento)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.AguardandoConfirmacao)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.AguardandoConfirmacao;
                    _obraRepository.SaveChanges();
                }
            }
            else if (aguardandoDadosPagamento)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.AguardandoDadosPagamento)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.AguardandoDadosPagamento;
                    _obraRepository.SaveChanges();
                }
            }
            else if (aprovado)
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.Aprovado)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.Aprovado;
                    _obraRepository.SaveChanges();
                }

                var contrato = _contratoService.ContratoVersaoObterPorId(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoContrato.Value, obra.NumContrato.Value);

                if (contrato.Status == EContratoStatus.AguardandoConfirmacaoPagamento || contrato.Status == EContratoStatus.AguardandoDadosPagamento)
                    TentarAprovarCadastroEContrato(contrato, usuario, false);
            }
            else
            {
                if (obraSalvar.StatusFinanceiro != EObraStatusFinanceiro.NaoNecessita)
                {
                    obraSalvar.StatusFinanceiro = EObraStatusFinanceiro.NaoNecessita;
                    _obraRepository.SaveChanges();
                }
            }

            var contratoAtual = _contratoService.ContratoVersaoObterPorId(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoContrato.Value, obra.NumContrato.Value);
            if (contratoAtual != null)
                if (contratoAtual.Status == EContratoStatus.AguardandoDadosPagamento || contratoAtual.Status == EContratoStatus.AguardandoConfirmacaoPagamento || parametroDesativarObrigatorioedadeAprovacaoCadastro)
                    _comercialLegacyService.ValidarContrato(contratoAtual, usuario, out string mensagens, aprovContratoDirAuto);
        }

        public Obra ListarObraTracos(int idUsina, int obraNumero, bool tracking = false)
        {
            return _obraRepository.ListarObraTracos(idUsina, obraNumero, tracking);
        }

        public ObraVersao ListarObraTracos(int numVersao, int idUsina, int obraNumero)
        {
            return _obraRepository.ListarObraTracos(numVersao, idUsina, obraNumero);
        }
        public Obra ListarObraBombas(int idUsina, int obraNumero, bool tracking = false)
        {
            return _obraRepository.ListarObraBombas(idUsina, obraNumero, tracking);
        }

        public ObraVersao ListarObraBombas(int numVersao, int idUsina, int obraNumero)
        {
            return _obraRepository.ListarObraBombas(numVersao, idUsina, obraNumero);
        }

        public void AprovarEngenharia(string usuario, Obra obra)
        {
            obra.Contrato = _contratoService.ObterPorId(obra.UsinaCodigo, obra.AnoContrato, obra.NumContrato);

            obra.Contrato?.AprovarEngenharia(usuario);
            obra.AprovarEngenharia();

            _obraRepository.Adicionar<ObraLog>(new ObraLog
            {
                UsinaCodigo = obra.UsinaCodigo,
                AnoChamada = obra.AnoChamada ?? 0,
                NumChamada = obra.NumChamada ?? 0,
                ObraCodigo = obra.Numero,
                DataHora = DateTime.Now,
                Usuario = usuario,
                Sequencia = 1,
                Evento = "APROVAÇÃO ENGENHARIA",
                Complemento = "Realizada aprovação de engenharia",
                Observacao = ""
            });
        }

        public void AprovarEngenharia(string usuario, ObraVersao obra)
        {
            obra.Contrato = _contratoService.ContratoVersaoObterPorId(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoContrato.Value, obra.NumContrato.Value);

            obra.Contrato?.AprovarEngenharia(usuario);
            obra.AprovarEngenharia();

            _obraRepository.Adicionar<ObraLogVersao>(new ObraLogVersao
            {
                NumeroVersao = obra.NumeroVersao,
                UsinaCodigo = obra.UsinaCodigo,
                AnoChamada = obra.AnoChamada ?? 0,
                NumChamada = obra.NumChamada ?? 0,
                ObraCodigo = obra.Numero,
                DataHora = DateTime.Now,
                Usuario = usuario,
                Sequencia = 1,
                Evento = "APROVAÇÃO ENGENHARIA",
                Complemento = "Realizada aprovação de engenharia",
                Observacao = ""
            });
        }

        public Obra ObterObraParaAnaliseCadastro(int idUsina, int obraNumero)
        {
            return _obraRepository.ObterObraParaAnaliseCadastro(idUsina, obraNumero);
        }

        public ObraVersao ObterObraParaAnaliseCadastro(int numVersao, int idUsina, int obraNumero)
        {
            return _obraRepository.ObterObraParaAnaliseCadastro(numVersao, idUsina, obraNumero);
        }

        public void AprovarDistanciaUsinaCep(string usuario, Obra obra)
        {
            obra.PendenteAprovacaoDistanciaUsinaCEP = false;
            var usinaDistancia = _usinaDistanciaCepService.ObterPorId(obra.UsinaEntregaCodigo, obra.EnderecoCep);
            usinaDistancia.AprovarNovaDistanciaUsinaCEP(usuario, obra.DistanciaUsina);
        }

        public void AlterarStatusCadastroEAnalista(Obra obra, string observacao, string usuario)
        {
            var _obra = _obraRepository.ObterPorId(obra.UsinaCodigo, obra.Numero);

            var proposta = _propostaRepository.ObterPorUsinaAnoNumero(obra.UsinaCodigo, (int)_obra.AnoChamada, (int)_obra.NumChamada, true);

            var alterouStatusCadastro = _obra.StatusCadastro != obra.StatusCadastro;
            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoCadastro)
            {
                alterouStatusCadastro = false;
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
            }

            var sequencia = 1;

            _obra.AlteraStatusCadastro(obra.StatusCadastro);
            _obraRepository.SaveChanges();

            _obra.Contrato = _contratoService.ObterPorId(obra.Contrato.Usina, obra.Contrato.Ano, obra.Contrato.Numero);

            if (_obra?.Contrato != null)
                _obra.Contrato.ClicksignEnvio = _clicksignRepository.ListarContratoClicksignEnvios(_obra.UsinaCodigo, _obra.AnoContrato ?? 0, _obra.NumContrato ?? 0);

            if (_obra.Contrato.ClicksignEnvio == null)
                _obra.Contrato.ClicksignEnvio = new List<ContratoClicksignEnvio>();

            var modeloDocumentoRemessaConcretoOld = _obra.Contrato.ModeloDocumentoRemessaConcreto;
            var modeloDocumentoRemessaBombaOld = _obra.Contrato.ModeloDocumentoRemessaBomba;
            var modeloItensDanfeERomaneioOld = _obra.Contrato.ModeloItensDanfeERomaneio;

            _obra.Contrato.ModeloDocumentoRemessaConcreto = obra.Contrato.ModeloDocumentoRemessaConcreto;
            _obra.Contrato.ModeloDocumentoRemessaBomba = obra.Contrato.ModeloDocumentoRemessaBomba;
            _obra.Contrato.ModeloItensDanfeERomaneio = obra.Contrato.ModeloItensDanfeERomaneio;

            _obra.Contrato.PercentualRetencaoContratual = obra.Contrato.PercentualRetencaoContratual;

            _obra.Contrato.MaoObraPropria = obra.Contrato.MaoObraPropria;
            _obra.Contrato.PercentualLocacao = obra.Contrato.PercentualLocacao;

            var alterouModeloDocumentoRemessaBomba = modeloDocumentoRemessaBombaOld != _obra.Contrato.ModeloDocumentoRemessaBomba;
            var alterouModeloDocumentoRemessaConcreto = modeloDocumentoRemessaConcretoOld != _obra.Contrato.ModeloDocumentoRemessaConcreto;
            var alterouModeloItensDanfeERomaneio = modeloItensDanfeERomaneioOld != _obra.Contrato.ModeloItensDanfeERomaneio;

            if (proposta != null && (alterouModeloDocumentoRemessaConcreto || alterouModeloDocumentoRemessaBomba || alterouModeloItensDanfeERomaneio))
            {
                proposta.ModeloDocumentoRemessaConcreto = _obra.Contrato.ModeloDocumentoRemessaConcreto;
                proposta.ModeloDocumentoRemessaBomba = _obra.Contrato.ModeloDocumentoRemessaBomba;
                proposta.ModeloItensDanfeERomaneio = _obra.Contrato.ModeloItensDanfeERomaneio;

                _propostaRepository.SaveChanges();
            }

            var alterouAnalista = (_obra.Contrato?.AnalistaCodigo ?? 0) != (obra.Contrato?.Analista?.Codigo ?? 0);

            _obra.Contrato.AlterarAnalista(obra.Contrato.Analista);
            _obraRepository.SaveChanges();

            var evento = "";
            var complemento = "";

            if (alterouStatusCadastro)
            {
                evento = "STATUS CADASTRO";
                complemento = $"Status cadastro: {obra.StatusCadastro}";
            }

            if (alterouAnalista)
            {
                if (!string.IsNullOrWhiteSpace(complemento))
                {
                    evento += " / ";
                    complemento += " / ";
                }

                evento += "ANALISTA";
                complemento += $"Analista: {obra.Contrato?.Analista?.Nome ?? ""}";
            }

            if (string.IsNullOrWhiteSpace(evento) && !string.IsNullOrWhiteSpace(observacao))
                evento = "ACOMPANHAMENTO";

            if (!string.IsNullOrWhiteSpace(evento))
            {
                _obraRepository.Adicionar(new ObraLog
                {
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = evento,
                    Complemento = complemento,
                    Observacao = observacao
                });

                _obraRepository.SaveChanges();
            }

            if (modeloDocumentoRemessaBombaOld != _obra.Contrato.ModeloDocumentoRemessaBomba)
            {

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaBombaOld.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(_obra.Contrato.ModeloDocumentoRemessaBomba.ToString())).FirstOrDefault();

                var descricao = $"De: {modeloDocumentoRemessaBombaOld} Para: {_obra.Contrato.ModeloDocumentoRemessaBomba}";

                if (modeloOld != null && modeloNew != null)
                    descricao = $"De: {modeloOld.Descricao} Para: {modeloNew.Descricao}";

                _obraRepository.Adicionar(new ObraLog
                {
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricao,
                    Observacao = "Alteração de modelo de documento para remessa de bomba"
                });

                _obraRepository.SaveChanges();

            }

            if (modeloDocumentoRemessaConcretoOld != _obra.Contrato.ModeloDocumentoRemessaConcreto)
            {

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaConcretoOld.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(_obra.Contrato.ModeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();

                var descricao = $"De: {modeloDocumentoRemessaConcretoOld} Para: {_obra.Contrato.ModeloDocumentoRemessaConcreto}";

                if (modeloOld != null && modeloNew != null)
                    descricao = $"De: {modeloOld.Descricao} Para: {modeloNew.Descricao}";

                _obraRepository.Adicionar(new ObraLog
                {
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricao,
                    Observacao = "Alteração de modelo de documento para remessa de concreto"
                });

                _obraRepository.SaveChanges();

            }

            if (obra.StatusCadastro == EObraStatusCadastro.Aprovado)
            {
                TentarAprovarCadastroEContrato(_obra.Contrato, usuario, true);
            }
            else
            {
                switch (obra.StatusCadastro)
                {
                    case EObraStatusCadastro.PreCadastro:
                        _obra.Contrato.AlterarStatus(EContratoStatus.PreAnalise);
                        break;
                    case EObraStatusCadastro.EmAnalise:
                        _obra.Contrato.AlterarStatus(EContratoStatus.EmAnalise);
                        break;
                    case EObraStatusCadastro.Reprovado:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Reprovado);
                        break;
                    case EObraStatusCadastro.Revalidacao:
                        _obra.Contrato.AlterarStatus(EContratoStatus.RevalidacaoCadastro);
                        break;
                    case EObraStatusCadastro.Pendente:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Pendente);
                        break;
                    case EObraStatusCadastro.AguardandoProgramacao:
                        _obra.Contrato.AlterarStatus(EContratoStatus.AguardandoDataProgramacao);
                        break;
                    case EObraStatusCadastro.Cancelado:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Cancelado);
                        break;
                }
                _obraRepository.SaveChanges();
            }
        }

        public void AlterarStatusCadastroEAnalista(ObraVersao obra, string observacao, string usuario)
        {
            var _obra = _obraRepository.ObterPorId<ObraVersao>(obra.NumeroVersao, obra.UsinaCodigo, obra.Numero);

            var proposta = _propostaRepository.ObterPorUsinaAnoNumero(obra.NumeroVersao, obra.UsinaCodigo, (int)_obra.AnoChamada, (int)_obra.NumChamada, true);

            var alterouStatusCadastro = _obra.StatusCadastro != obra.StatusCadastro;
            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoCadastro)
            {
                alterouStatusCadastro = false;
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
            }

            var sequencia = 1;

            _obra.AlteraStatusCadastro(obra.StatusCadastro);
            _obraRepository.SaveChanges();

            _obra.Contrato = _contratoService.ContratoVersaoObterPorId(obra.NumeroVersao, obra.Contrato.Usina, obra.Contrato.Ano, obra.Contrato.Numero);

            //*** Verificar ClickSign
           /* if (_obra?.Contrato != null)
                _obra.Contrato.ClicksignEnvio = _clicksignRepository.ListarContratoClicksignEnvios(_obra.UsinaCodigo, _obra.AnoContrato ?? 0, _obra.NumContrato ?? 0);

            if (_obra.Contrato.ClicksignEnvio == null)
                _obra.Contrato.ClicksignEnvio = new List<ContratoClicksignEnvio>();*/
            //****

            var modeloDocumentoRemessaConcretoOld = _obra.Contrato.ModeloDocumentoRemessaConcreto;
            var modeloDocumentoRemessaBombaOld = _obra.Contrato.ModeloDocumentoRemessaBomba;
            var modeloItensDanfeERomaneioOld = _obra.Contrato.ModeloItensDanfeERomaneio;

            _obra.Contrato.ModeloDocumentoRemessaConcreto = obra.Contrato.ModeloDocumentoRemessaConcreto;
            _obra.Contrato.ModeloDocumentoRemessaBomba = obra.Contrato.ModeloDocumentoRemessaBomba;
            _obra.Contrato.ModeloItensDanfeERomaneio = obra.Contrato.ModeloItensDanfeERomaneio;

            _obra.Contrato.PercentualRetencaoContratual = obra.Contrato.PercentualRetencaoContratual;

            _obra.Contrato.MaoObraPropria = obra.Contrato.MaoObraPropria;
            _obra.Contrato.PercentualLocacao = obra.Contrato.PercentualLocacao;

            var alterouModeloDocumentoRemessaBomba = modeloDocumentoRemessaBombaOld != _obra.Contrato.ModeloDocumentoRemessaBomba;
            var alterouModeloDocumentoRemessaConcreto = modeloDocumentoRemessaConcretoOld != _obra.Contrato.ModeloDocumentoRemessaConcreto;
            var alterouModeloItensDanfeRomaneio = modeloItensDanfeERomaneioOld != _obra.Contrato.ModeloItensDanfeERomaneio;

            if (proposta != null && (alterouModeloDocumentoRemessaConcreto || alterouModeloDocumentoRemessaBomba || alterouModeloItensDanfeRomaneio))
            {
                proposta.ModeloDocumentoRemessaConcreto = _obra.Contrato.ModeloDocumentoRemessaConcreto;
                proposta.ModeloDocumentoRemessaBomba = _obra.Contrato.ModeloDocumentoRemessaBomba;
                proposta.ModeloItensDanfeERomaneio = _obra.Contrato.ModeloItensDanfeERomaneio;

                _propostaRepository.SaveChanges();
            }

            var alterouAnalista = (_obra.Contrato?.AnalistaCodigo ?? 0) != (obra.Contrato?.Analista?.Codigo ?? 0);

            _obra.Contrato.AlterarAnalista(obra.Contrato.Analista);
            _obraRepository.SaveChanges();

            var evento = "";
            var complemento = "";

            if (alterouStatusCadastro)
            {
                evento = "STATUS CADASTRO";
                complemento = $"Status cadastro: {obra.StatusCadastro}";
            }

            if (alterouAnalista)
            {
                if (!string.IsNullOrWhiteSpace(complemento))
                {
                    evento += " / ";
                    complemento += " / ";
                }

                evento += "ANALISTA";
                complemento += $"Analista: {obra.Contrato?.Analista?.Nome ?? ""}";
            }

            if (string.IsNullOrWhiteSpace(evento) && !string.IsNullOrWhiteSpace(observacao))
                evento = "ACOMPANHAMENTO";

            if (!string.IsNullOrWhiteSpace(evento))
            {
                _obraRepository.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = _obra.NumeroVersao,
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = evento,
                    Complemento = complemento,
                    Observacao = observacao
                });

                _obraRepository.SaveChanges();
            }

            if (modeloDocumentoRemessaBombaOld != _obra.Contrato.ModeloDocumentoRemessaBomba)
            {

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaBombaOld.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(_obra.Contrato.ModeloDocumentoRemessaBomba.ToString())).FirstOrDefault();

                var descricao = $"De: {modeloDocumentoRemessaBombaOld} Para: {_obra.Contrato.ModeloDocumentoRemessaBomba}";

                if (modeloOld != null && modeloNew != null)
                    descricao = $"De: {modeloOld.Descricao} Para: {modeloNew.Descricao}";

                _obraRepository.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = _obra.NumeroVersao,
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricao,
                    Observacao = "Alteração de modelo de documento para remessa de bomba"
                });

                _obraRepository.SaveChanges();

            }

            if (modeloDocumentoRemessaConcretoOld != _obra.Contrato.ModeloDocumentoRemessaConcreto)
            {

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaConcretoOld.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(_obra.Contrato.ModeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();

                var descricao = $"De: {modeloDocumentoRemessaConcretoOld} Para: {_obra.Contrato.ModeloDocumentoRemessaConcreto}";

                if (modeloOld != null && modeloNew != null)
                    descricao = $"De: {modeloOld.Descricao} Para: {modeloNew.Descricao}";

                _obraRepository.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = _obra.NumeroVersao,
                    UsinaCodigo = _obra.UsinaCodigo,
                    AnoChamada = _obra.AnoChamada ?? 0,
                    NumChamada = _obra.NumChamada ?? 0,
                    ObraCodigo = _obra.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Sequencia = sequencia++,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricao,
                    Observacao = "Alteração de modelo de documento para remessa de concreto"
                });

                _obraRepository.SaveChanges();

            }

            if (obra.StatusCadastro != EObraStatusCadastro.Aprovado)
            {
                switch (obra.StatusCadastro)
                {
                    case EObraStatusCadastro.PreCadastro:
                        _obra.Contrato.AlterarStatus(EContratoStatus.PreAnalise);
                        break;
                    case EObraStatusCadastro.EmAnalise:
                        _obra.Contrato.AlterarStatus(EContratoStatus.EmAnalise);
                        break;
                    case EObraStatusCadastro.Reprovado:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Reprovado);
                        break;
                    case EObraStatusCadastro.Revalidacao:
                        _obra.Contrato.AlterarStatus(EContratoStatus.RevalidacaoCadastro);
                        break;
                    case EObraStatusCadastro.Pendente:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Pendente);
                        break;
                    case EObraStatusCadastro.AguardandoProgramacao:
                        _obra.Contrato.AlterarStatus(EContratoStatus.AguardandoDataProgramacao);
                        break;
                    case EObraStatusCadastro.Cancelado:
                        _obra.Contrato.AlterarStatus(EContratoStatus.Cancelado);
                        break;
                }
                _obraRepository.SaveChanges();
            }
        }

        public void TentarAprovarCadastroEContrato(Contrato contrato, string usuario, bool adicionarNotificacaoCadastro, bool adicionarNotificacaoContrato = false)
        {
            // TODO: isolar em duas funções separadas

            var obra = _obraRepository
                .ListarFiltradosTracking<Obra>(t => t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoCadastro)
            {
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
                _obraRepository.SaveChanges();

                var aprovarContratoAutomaticamente = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Trim() == "1";

                if (_comercialLegacyService.ValidarContrato(contrato, usuario, out string mensagem, aprovarContratoAutomaticamente))
                {
                    if (adicionarNotificacaoContrato)
                        AssertionConcern.Notify("statusCadastro", mensagem);
                }

                return;
            }

            if (_comercialLegacyService.ValidarAprovacaoCadastro(contrato, out string mensagens))
            {
                contrato.LimparAprovacaoCadastro();
                _obraRepository.SaveChanges();

                if (obra.StatusCadastro == EObraStatusCadastro.Aprovado)
                {
                    obra.StatusCadastro = EObraStatusCadastro.EmAnalise;
                    _obraRepository.SaveChanges();
                }

                if (adicionarNotificacaoCadastro)
                    AssertionConcern.Notify("statusCadastro", mensagens);

                return;
            }

            if (obra.StatusCadastro != EObraStatusCadastro.Aprovado)
            {
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
                _obraRepository.SaveChanges();
            }

            if (!contrato.IsCadastroAprovado())
            {
                contrato.AprovarCadastro(usuario);
                _obraRepository.SaveChanges();
            }

            var aprovarAutomaticamente = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Trim() == "1";

            

            if (_comercialLegacyService.ValidarContrato(contrato, usuario, out mensagens, aprovarAutomaticamente))
            {
                if (adicionarNotificacaoContrato)
                    AssertionConcern.Notify("statusCadastro", mensagens);

                return;
            }
        }

        public void TentarAprovarCadastroEContrato(ContratoVersao contrato, string usuario, bool adicionarNotificacaoCadastro, bool adicionarNotificacaoContrato = false)
        {
            // TODO: isolar em duas funções separadas

            var obra = _obraRepository
                .ListarFiltradosTracking<ObraVersao>(t => t.NumeroVersao == contrato.NumeroVersao && t.UsinaCodigo == contrato.Usina && t.AnoContrato == contrato.Ano && t.NumContrato == contrato.Numero)
                .FirstOrDefault();

            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";

            if (parametroDesativarObrigatoriedadeAprovacaoCadastro)
            {
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
                _obraRepository.SaveChanges();

                var aprovarContratoAutomaticamente = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Trim() == "1";

                if (_comercialLegacyService.ValidarContrato(contrato, usuario, out string mensagem, aprovarContratoAutomaticamente))
                {
                    if (adicionarNotificacaoContrato)
                        AssertionConcern.Notify("statusCadastro", mensagem);
                }

                return;
            }

            if (_comercialLegacyService.ValidarAprovacaoCadastro(contrato, out string mensagens))
            {
                contrato.LimparAprovacaoCadastro();
                _obraRepository.SaveChanges();

                if (obra.StatusCadastro == EObraStatusCadastro.Aprovado)
                {
                    obra.StatusCadastro = EObraStatusCadastro.EmAnalise;
                    _obraRepository.SaveChanges();
                }

                if (adicionarNotificacaoCadastro)
                    AssertionConcern.Notify("statusCadastro", mensagens);

                return;
            }

            if (obra.StatusCadastro != EObraStatusCadastro.Aprovado)
            {
                obra.StatusCadastro = EObraStatusCadastro.Aprovado;
                _obraRepository.SaveChanges();
            }

            if (!contrato.IsCadastroAprovado())
            {
                contrato.AprovarCadastro(usuario);
                _obraRepository.SaveChanges();
            }

            var aprovarAutomaticamente = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Trim() == "1";

            if (_comercialLegacyService.ValidarContrato(contrato, usuario, out mensagens, aprovarAutomaticamente))
            {
                if (adicionarNotificacaoContrato)
                    AssertionConcern.Notify("statusCadastro", mensagens);

                return;
            }
        }

        public void AtualizarValoresProgramacoesFuturas(int usina, int obraNumero)
        {
            var programacoesFuturas = _obraRepository
                .ListarFiltrados<Programacao>(t => t.UsinaCodigo == usina
                    && t.ObraNumero == obraNumero
                    && t.DataConcretagem >= DateTime.Today);

            foreach (var programacao in programacoesFuturas)
                _comercialLegacyService.TotalizarValoresProgramacao(programacao);
        }

        public void AtualizarStatusEngenharia(Obra obra)
        {
            var _obra = _obraRepository.ObterPorId(obra.UsinaCodigo, obra.Numero);
            _obra.Contrato = _contratoService.ObterPorId(obra.UsinaCodigo, obra.AnoContrato, obra.NumContrato);

            if (_obra.Contrato == null)
                return;

            var aprovaEngenharia = "";
            foreach (var traco in _obra.ObraTracos)
            {
                if (aprovaEngenharia == "S")
                    break;

                aprovaEngenharia = (_comercialLegacyService.VerificaRegrasAprovacaoEngenharia(traco, _obra.Contrato?.IntervenienteCodigo ?? 0) ? "S" : "N");
            }

            _obra.Contrato?.AlterarNecessitaAprovacao(aprovaEngenharia, "");
        }

        public void AtualizarStatusEngenharia(ObraVersao obra)
        {
            var _obra = _obraRepository.ObterPorId<ObraVersao>(obra.NumeroVersao, obra.UsinaCodigo, obra.Numero);
            _obra.Contrato = _contratoRepository.ContratoVersaoObterPorId(obra.NumeroVersao, obra.UsinaCodigo, obra.AnoContrato??0, obra.NumContrato??0);

            if (_obra.Contrato == null)
                return;

            var aprovaEngenharia = "";
            foreach (var traco in _obra.ObraTracos)
            {
                if (aprovaEngenharia == "S")
                    break;

                aprovaEngenharia = (_comercialLegacyService.VerificaRegrasAprovacaoEngenharia(traco, _obra.Contrato?.IntervenienteCodigo ?? 0) ? "S" : "N");
            }

            _obra.Contrato?.AlterarNecessitaAprovacao(aprovaEngenharia, "");
        }

        public void AprovarAutomaticamentePagamentosDaCieloLio(Obra obra)
        {
            var _obra = _obraRepository.ListarObraPagamentos(obra.UsinaCodigo, obra.Numero);

            var contratoPagamentos = _obra?.ContratoPagamentos;
            if (contratoPagamentos == null)
                return;

            foreach (var contratoPagamento in contratoPagamentos)
            {
                if (typeof(ContratoPagamento).Equals(contratoPagamento.GetType()))
                    AprovarAutomaticamenteContratoPagamentosDaCieloLio((ContratoPagamento)contratoPagamento, _obra);
            }
        }

        public void AprovarAutomaticamenteContratoPagamentosDaCieloLio(ContratoPagamento contratoPagamento, Obra obra)
        {
            if (contratoPagamento?.Detalhes == null)
                return;

            foreach (var detalhe in contratoPagamento.Detalhes)
            {
                if (!(typeof(ContratoPagamentoDetalheCartao).Equals(detalhe.GetType())))
                    continue;

                var cartao = (ContratoPagamentoDetalheCartao)detalhe;

                var cartaotransacao = _cartaoTransacaoRepository.ObterPorDataNumeroCartaoAutorizacaoValorQuantidadeParcelas(cartao.DataTransacao, cartao.NumeroCartao, cartao.NumeroAutorizacao, cartao.Valor, cartao.QuantidadeParcelas);

                if (cartaotransacao == null)
                    continue;

                if (!cartaotransacao.OrigemCieLio() || cartaotransacao.JaVinculado())
                    continue;
                //Está como admin porque pelo serviço também coloca como ADMIN.
                if (_contasAReceberService.GeraContasAReceberDaOperadora(cartaotransacao.TransacaoId.ToString(), "ADMIN"))
                {
                    _contasAReceberService.AprovaPagamentoAntecipadoCartaoDeCredito(obra.UsinaCodigo, obra.Numero, contratoPagamento.Sequencia, "ADMIN");
                }
            }
        }

        public TDetalhe BuscarDetalhes<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            return _contratoPagamentoRepository.BuscarDetalhes<TDetalhe>(forma, contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, detalheSequencia, obraNumero, tracking);
        }

        public TDetalhe BuscarDetalhesVersao<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, int numVersao = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            return _contratoPagamentoRepository.BuscarDetalhesVersao<TDetalhe>(forma, contratoUsina, contratoAno, contratoNumero, pagamentoSequencia, detalheSequencia, obraNumero, numVersao, tracking);
        }

        public IEnumerable<TDetalhe> ListarDetalhes<TDetalhe>(string forma, Expression<Func<TDetalhe, bool>> filter, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe
        {
            return _contratoPagamentoRepository.ListarDetalhes<TDetalhe>(forma, filter, tracking);
        }

        public float CalcularImpostosAplicadoEstadual(ObraTraco obraTraco, CustoServico custoServico, Obra obra)
        {
            var tracoPreco = _tracoPrecoService
              .ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(obra.UsinaEntregaCodigo, obraTraco.UsoCodigo,
              obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, obra);

            return (obraTraco.M3PrecoProposto - tracoPreco.M3Preco) * (custoServico.ImpostoEstadual / 100);
        }

        public float CalcularCustoValorBombaPorM3(ObraTraco obraTraco, Obra obra)
        {
            if (obraTraco.M3QuantidadeBombeada == 0) return 0;

            var bombaParaCalculo = obra.ObraBombas?.FirstOrDefault();
            if (bombaParaCalculo == null) return 0;
            return bombaParaCalculo.CalculaValorBomba(obraTraco.M3QuantidadeBombeada) / obraTraco.M3QuantidadeBombeada;
        }

        public void CalcularEbitdaObraTraco(ObraTraco obraTraco, Obra obra)
        {
            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == obra.UsinaEntregaCodigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            var tracoCusto = _tracoCustoService
              .ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(obra.UsinaEntregaCodigo, obraTraco.UsoCodigo,
              obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, DateTime.Today);

            if (custoServicoVigente != null)
            {
                var utilizaCalculoPrecoTabelaPorUsina = _parametroRepository.ObterParametroN("web", "UtilizaCalculoPrecoTabelaPorUsina").Equals("1");

                var usinaEntrega = _usinaService.ObterPorId<Usina>(obra.UsinaEntregaCodigo);
                obra.CalcularTempoDeCiclo(usinaEntrega);

                obraTraco.ValorServico = obraTraco.CustoServicoReajustado > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.CustoServicoReajustado : (obraTraco.M3PrecoTabela - (tracoCusto?.CustoAjustado ?? 0f)) - (obraTraco.M3PrecoTabela - obraTraco.M3PrecoProposto);

                var volumePorCarga = obra.VolumePorCarga == 0 ? 8 : obra.VolumePorCarga;
                var quantidadeViagensPrevista = (int)Math.Ceiling(obraTraco.M3Quantidade / volumePorCarga);

                if (utilizaCalculoPrecoTabelaPorUsina)
					obraTraco.CustoProjetadoTransporte = (custoServicoVigente.CustoMedioHoraTransporte * (obra.TempoCicloPrevisto / 60f)) / volumePorCarga;
                else
                    obraTraco.CustoProjetadoTransporte = (custoServicoVigente.CustoMedioHoraTransporte * (obra.TempoCicloPrevisto / 60f) * quantidadeViagensPrevista) / obraTraco.M3Quantidade;

                obraTraco.MargemPosTransporte = (obraTraco.CustoServicoReajustado > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.CustoServicoReajustado : obraTraco.ValorServico) - obraTraco.CustoProjetadoTransporte;

                var valorM3 = (obraTraco.PrecoReajustadoAtual > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);
                
                //Necessário Refatorar depois TC-4205
                if (utilizaCalculoPrecoTabelaPorUsina)
                {
                    var empresa = _usinaService.ObterPorId<Empresa>(usinaEntrega.EmpresaCodigo);
                    var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

                    var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : municipio.AliquotaIss;

                    obraTraco.ImpostoAplicadoEstadual = 0;
                    obraTraco.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, (custoServicoVigente.ImpostoEstadual + custoServicoVigente.ImpostoFederal + aliquota));
                    obraTraco.IssDedutivel = 0;
                } else
                {
                    obraTraco.ImpostoAplicadoEstadual = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, custoServicoVigente.ImpostoEstadual);
                    obraTraco.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, custoServicoVigente.ImpostoFederal);
                    obraTraco.IssDedutivel = _calculoImpostosService.CalcularIss(obra, obraTraco.ValorM3, obraTraco.ValorMaterial(tracoCusto));
                }
                obraTraco.MargemPosTransporte -= (obraTraco.ImpostoAplicadoEstadual + obraTraco.ImpostoAplicadoFederal + obraTraco.IssDedutivel);
                
                
                obraTraco.CustoBombagem = obraTraco.M3QuantidadeBombeada > 0 ? custoServicoVigente.CustoMedioBombagem : 0;

                obraTraco.Ebitda = obraTraco.MargemPosTransporte - (custoServicoVigente.CustoMedioProducao + custoServicoVigente.CustoMedioLaboratorio + custoServicoVigente.CustoMedioComercial + custoServicoVigente.CustoMedioAdministrativo);
            }

            _obraRepository.AdicionarLogPropostaItem(obraTraco, "ObraService.CalcularEbitdaObraTraco");
            _obraRepository.SaveChanges();
        }

        public void CalcularEbitdaObraTraco(ObraTracoVersao obraTraco, ObraVersao obra)
        {
            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == obra.UsinaEntregaCodigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            var tracoCusto = _tracoCustoService
              .ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(obra.UsinaEntregaCodigo, obraTraco.UsoCodigo,
              obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, DateTime.Today);

            if (custoServicoVigente != null)
            {
                var utilizaCalculoPrecoTabelaPorUsina = _parametroRepository.ObterParametroN("web", "UtilizaCalculoPrecoTabelaPorUsina").Equals("1");

                var usinaEntrega = _usinaService.ObterPorId<Usina>(obra.UsinaEntregaCodigo);
                obra.CalcularTempoDeCiclo(usinaEntrega);

                obraTraco.ValorServico = obraTraco.CustoServicoReajustado > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.CustoServicoReajustado : (obraTraco.M3PrecoTabela - (tracoCusto?.CustoAjustado ?? 0f)) - (obraTraco.M3PrecoTabela - obraTraco.M3PrecoProposto);
                
                var volumePorCarga = obra.VolumePorCarga == 0 ? 8 : obra.VolumePorCarga;
                var quantidadeViagensPrevista = (int)Math.Ceiling(obraTraco.M3Quantidade / volumePorCarga);

                if (utilizaCalculoPrecoTabelaPorUsina)
                    obraTraco.CustoProjetadoTransporte = (custoServicoVigente.CustoMedioHoraTransporte * (obra.TempoCicloPrevisto / 60f)) / volumePorCarga;
                else
                    obraTraco.CustoProjetadoTransporte = (custoServicoVigente.CustoMedioHoraTransporte * (obra.TempoCicloPrevisto / 60f) * quantidadeViagensPrevista) / obraTraco.M3Quantidade;

                obraTraco.MargemPosTransporte = (obraTraco.CustoServicoReajustado > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.CustoServicoReajustado : obraTraco.ValorServico) - obraTraco.CustoProjetadoTransporte;

                var valorM3 = (obraTraco.PrecoReajustadoAtual > 0 && obraTraco.DataUltimoReajuste <= DateTime.Now.Date ? obraTraco.PrecoReajustadoAtual : obraTraco.M3PrecoProposto);

                //Necessário Refatorar depois TC-4205
                if (utilizaCalculoPrecoTabelaPorUsina)
                {
                    var empresa = _usinaService.ObterPorId<Empresa>(usinaEntrega.EmpresaCodigo);
                    var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

                    var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : municipio.AliquotaIss;

                    obraTraco.ImpostoAplicadoEstadual = 0;
                    obraTraco.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, (custoServicoVigente.ImpostoEstadual + custoServicoVigente.ImpostoFederal + aliquota));
                    obraTraco.IssDedutivel = 0;
                }
                else
                {
                    obraTraco.ImpostoAplicadoEstadual = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, custoServicoVigente.ImpostoEstadual);
                    obraTraco.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(valorM3, custoServicoVigente.ImpostoFederal);
                    obraTraco.IssDedutivel = _calculoImpostosService.CalcularIss(obra, obraTraco.ValorM3, obraTraco.ValorMaterial(tracoCusto));
                }

                obraTraco.MargemPosTransporte -= (obraTraco.ImpostoAplicadoEstadual + obraTraco.ImpostoAplicadoFederal + obraTraco.IssDedutivel);

                obraTraco.CustoBombagem = obraTraco.M3QuantidadeBombeada > 0 ? custoServicoVigente.CustoMedioBombagem : 0;

                obraTraco.Ebitda = obraTraco.MargemPosTransporte - (custoServicoVigente.CustoMedioProducao + custoServicoVigente.CustoMedioLaboratorio + custoServicoVigente.CustoMedioComercial + custoServicoVigente.CustoMedioAdministrativo);
            }

            _obraRepository.AdicionarLogPropostaItem(obraTraco, "ObraService.CalcularEbitdaObraTraco");
            _obraRepository.SaveChanges();
        }

        public void CalcularEbitdaObraBomba(ObraBomba obraBomba, Obra obra)
        {
            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == obra.UsinaEntregaCodigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            if (custoServicoVigente == null) return;

            if ((obra?.ObraTracos?.Count() ?? 0) == 0) return;

            var quantidadeBombeada = obra.ObraTracos.Sum(t => t.M3QuantidadeBombeada);

            if (quantidadeBombeada == 0) return;

            var quantidadeBombeadaM3 = obraBomba.CalculaValorBomba(quantidadeBombeada) / quantidadeBombeada;

            obraBomba.ImpostoAplicadoEstadual = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(quantidadeBombeadaM3, custoServicoVigente.ImpostoEstadual);

            obraBomba.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(quantidadeBombeadaM3, custoServicoVigente.ImpostoFederal);

            obraBomba.IssDedutivel = _calculoImpostosService.CalcularIss(obra, quantidadeBombeadaM3, 0, true);

            obraBomba.Ebitda = quantidadeBombeadaM3 - custoServicoVigente.CustoMedioBombagem - (obraBomba.ImpostoAplicadoEstadual + obraBomba.ImpostoAplicadoFederal + obraBomba.IssDedutivel);

            _obraRepository.SaveChanges();

        }

        public void CalcularEbitdaObraBomba(ObraBombaVersao obraBomba, ObraVersao obra)
        {
            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == obra.UsinaEntregaCodigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            if (custoServicoVigente == null) return;

            if ((obra?.ObraTracos?.Count() ?? 0) == 0) return;

            var quantidadeBombeada = obra.ObraTracos.Sum(t => t.M3QuantidadeBombeada);

            if (quantidadeBombeada == 0) return;

            var quantidadeBombeadaM3 = obraBomba.CalculaValorBomba(quantidadeBombeada) / quantidadeBombeada;

            obraBomba.ImpostoAplicadoEstadual = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(quantidadeBombeadaM3, custoServicoVigente.ImpostoEstadual);

            obraBomba.ImpostoAplicadoFederal = _calculoImpostosService.CalcularImpostoSomenteSobreAliquota(quantidadeBombeadaM3, custoServicoVigente.ImpostoFederal);

            obraBomba.IssDedutivel = _calculoImpostosService.CalcularIss(obra, quantidadeBombeadaM3, 0, true);

            obraBomba.Ebitda = quantidadeBombeadaM3 - custoServicoVigente.CustoMedioBombagem - (obraBomba.ImpostoAplicadoEstadual + obraBomba.ImpostoAplicadoFederal + obraBomba.IssDedutivel);

            _obraRepository.SaveChanges();

        }

        public float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato)
        {
            var consumo = _obraRepository.ObterConsumoTracoPorContrato(usinaContrato, numeroContrato, anoContrato, sequenciaTracoContrato);

            return consumo;
        }

        public float? ObterConsumoPorTraco(int numeroContrato, int anoContrato, string traco, int interveniente)
        {
            var consumo = _obraRepository.ObterConsumoPorTraco(numeroContrato, anoContrato, traco, interveniente);

            return consumo;
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            _obraRepository.AdicionarVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao, usinaProposta, anoProposta, numeroProposta, numObra);

            var contratoTracoReajustes = _contratoTracoReajusteService
                .ListarFiltradosTracking(t => t.UsinaCodigo == codUsina && t.ContratoAno == anoContrato && t.ContratoNumero == numeroContrato)
                .GroupBy(t => t.ObraTracoSequencia)
                .Select(g => g.OrderByDescending(t => t.DataVigencia).First())
                .ToList();

            foreach (var reajuste in contratoTracoReajustes)
            {
                var obra = _obraRepository.ObterPorId(codUsina, numObra);

                _contratoTracoReajusteService.AtualizarObraTracoReajustes(obra, reajuste.ObraTracoSequencia, numVersao, numVersao == 1 ? false : true);

                var obraVersao = _contratoTracoReajusteService.ListarFiltradosTracking<ObraVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.Numero == obra.Numero && t.NumeroVersao == numVersao).FirstOrDefault();

                var obraTracoVersao = _contratoTracoReajusteService.ListarFiltradosTracking<ObraTracoVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero && t.NumeroVersao == numVersao && t.Sequencia == reajuste.ObraTracoSequencia).FirstOrDefault();
                if (obraTracoVersao != null)
                    CalcularEbitdaObraTraco(obraTracoVersao, obraVersao);
            }

            var contratoBombaReajustes = _contratoBombaReajusteService
                .ListarFiltradosTracking(t => t.UsinaCodigo == codUsina && t.ContratoAno == anoContrato && t.ContratoNumero == numeroContrato)
                .GroupBy(t => t.ObraBombaReajusteSequencia)
                .Select(g => g.OrderByDescending(t => t.DataVigencia).First())
                .ToList();

            foreach (var reajuste in contratoBombaReajustes)
            {
                var obra = _obraRepository.ObterPorId(codUsina, numObra);

                _contratoBombaReajusteService.AtualizarObraBombaReajustes(obra, reajuste.ObraBombaReajusteSequencia, numVersao, numVersao == 1 ? false : true);

                var obraVersao = _contratoBombaReajusteService.ListarFiltradosTracking<ObraVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.Numero == obra.Numero && t.NumeroVersao == numVersao).FirstOrDefault();
                obraVersao.ObraTracos = _contratoBombaReajusteService.ListarFiltradosTracking<ObraTracoVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero && t.NumeroVersao == numVersao).ToList();

                var obraBombaVersao = _contratoBombaReajusteService.ListarFiltradosTracking<ObraBombaVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero && t.NumeroVersao == numVersao && t.Sequencia == reajuste.ObraBombaReajusteSequencia).FirstOrDefault();

                if (obraBombaVersao != null) CalcularEbitdaObraBomba(obraBombaVersao, obraVersao);
        }
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            _obraRepository.ExcluirVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao, usinaProposta, anoProposta, numeroProposta, numObra);
        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            _obraRepository.AdicionarContrato(codUsina, anoContrato, numeroContrato, numVersao, usinaProposta, anoProposta, numeroProposta, numObra);
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            _obraRepository.ExcluirContrato(codUsina, anoContrato, numeroContrato, usinaProposta, anoProposta, numeroProposta, numObra);
        }
        public int ObterUltimaVersaoObra(int obraUsina, int obraNumero)
        {

            return _obraRepository.ObterUltimaVersaoObra(obraUsina, obraNumero);

        }

        public Obra ObterPorIdAprovacaoComercial(int usina, int numero)
        {
            return _obraRepository.ObterPorIdAprovacaoComercial(usina, numero, true);
        }

        public ObraVersao ObterPorIdAprovacaoComercial(int usina, int numero, int versao)
        {
            return _obraRepository.ObterPorIdAprovacaoComercial(usina, numero, versao, true);
        }

        public void AlterarMensagemObraReajuste(int codUsina, int codObra, string mensagem)
        {
            _obraRepository.AlterarMensagemObraReajuste(codUsina, codObra, mensagem);
        }

        public void AlterarMensagemObraReajusteVersao(int numVersao, int codUsina, int codObra, string mensagem)
        {
            _obraRepository.AlterarMensagemObraReajusteVersao(numVersao, codUsina, codObra, mensagem);
        }

        public Obra ObterObraPorContrato(int codUsina, int anoContrato, int numeroContrato, bool tracking = false)
        {
            return _obraRepository.ObterObraPorContrato(codUsina, anoContrato, numeroContrato, tracking);
        }

        public bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero)
        {
            return _obraRepository.VerificarStatusComercialEstaReprovada(obraUsina, obraNumero);
        }

        public bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero, int obraVersao)
        {
            return _obraRepository.VerificarStatusComercialEstaReprovada(obraUsina, obraNumero, obraVersao);
        }

        public void AtualizaObraTracoReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, float m3PrecoProposto, float valorServico, float descontoPercentual, ContratoTracoReajuste tracoReajuste)
        {
            _obraRepository.AtualizaObraTracoReajuste(usina, obraNumero, sequencia, dataUltimoReajuste, m3PrecoProposto, valorServico, descontoPercentual, tracoReajuste);
        }

        public void AtualizaObraBombaReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, int m3PropostoAte, float taxaMinimaPrecoProposto, float m3PrecoProposto, float descontoPercentual, ContratoBombaReajuste bombaReajuste)
        {
            _obraRepository.AtualizaObraBombaReajuste(usina, obraNumero, sequencia, dataUltimoReajuste, m3PropostoAte, taxaMinimaPrecoProposto, m3PrecoProposto, descontoPercentual, bombaReajuste);
        }

        public IEnumerable<ObraProjecao> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada)
        {
            return _obraRepository.ListarProjecaoPorObra(usina, numero, anoChamada, noChamada);
        }

        public float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraRepository.ObterConsumoPorContrato(usinaContrato, numeroContrato, anoContrato);

            return consumo;
        }

        public float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var volume = _obraRepository.ObterVolumePorContrato(usina, noObra, anoChamada, noChamada);

            return volume;
        }

        public float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraRepository.ObterConsumoAcumuladoPorContrato(usinaContrato, numeroContrato, anoContrato);

            return consumo;
        }

        public float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            var consumo = _obraRepository.ObterConsumoMesAtualPorContrato(usinaContrato, numeroContrato, anoContrato);

            return consumo;
        }

        public void AtualizarDadosReajuste(ObraTraco obraTraco)
        {
            _obraRepository.AtualizarDadosReajuste(obraTraco);
		}
        public void GarantirPersistenciaObraPropostaContrato(string usuario, int usina, int numero)
        {

            var featureFlag = _parametroRepository.ObterParametroN("FeatureFlags", "GarantePersistenciaStatus").Equals("1");
            if (!featureFlag)
                return;

            var ultimaVersao = _obraRepository.ObterUltimaVersaoObra(usina, numero);

            var statusCadastro = 0;
            var statusComercial = 0;
            var statusContrato = 0;

            _obraRepository.ObterStatusObra(usina, numero, ultimaVersao, out statusCadastro, out statusComercial, out statusContrato);

            if (statusCadastro == 0 && statusComercial == 0 && statusContrato == 0)
                return;

            var statusContratoAlterado = false;
            var finalStatusContrato = statusContrato;

            var obraRevalidacao = (statusCadastro == (int)EObraStatusCadastro.Revalidacao && statusContrato == (int)EContratoStatus.RevalidacaoCadastro);
            var obraPreCadastro = (statusCadastro == (int)EObraStatusCadastro.PreCadastro && statusContrato == (int)EContratoStatus.PreAnalise);
            var obraEmAnalise = (statusCadastro == (int)EObraStatusCadastro.EmAnalise && statusContrato == (int)EContratoStatus.EmAnalise);

            var obraCadastroPendenteAndOK = obraRevalidacao || obraPreCadastro || obraEmAnalise;

            if (statusCadastro == (int)EObraStatusCadastro.Revalidacao && !obraRevalidacao)
                finalStatusContrato = (int)EContratoStatus.RevalidacaoCadastro;
            else if (statusCadastro == (int)EObraStatusCadastro.PreCadastro && !obraPreCadastro)
                finalStatusContrato = (int)EContratoStatus.PreAnalise;
            else if (statusCadastro == (int)EObraStatusCadastro.EmAnalise && !obraEmAnalise)
                finalStatusContrato = (int)EContratoStatus.EmAnalise;

            statusContratoAlterado = finalStatusContrato != statusContrato;

            if (!obraCadastroPendenteAndOK && !statusContratoAlterado)
            {

                var contratoPendente = (statusComercial == (int)EObraStatusComercial.Aguardando && statusContrato == (int)EContratoStatus.AguardandoAprovacaoComercial);

                if (statusComercial == (int)EObraStatusComercial.Aguardando && !contratoPendente)
                    finalStatusContrato = (int)EContratoStatus.AguardandoAprovacaoComercial;
                else if (statusComercial == (int)EObraStatusComercial.Aprovado && statusContrato == (int)EContratoStatus.AguardandoAprovacaoComercial)
                {

                    if (ultimaVersao > 0)
                        _comercialLegacyService.FinalizarAprovacoesComerciaisVersao(usuario, $"{ultimaVersao}-{usina}-{numero}", new List<ObraLogDado>());
                    else
                        _comercialLegacyService.FinalizarAprovacoesComerciais(usuario, $"{usina}-{numero}", new List<ObraLogDado>());

                }

                statusContratoAlterado = finalStatusContrato != statusContrato;

                if (statusContratoAlterado)
                    _obraRepository.alterarStatusContratoPelaObra(usina, numero, ultimaVersao, finalStatusContrato);
            }
        }

        public void AtualizarDadosReajuste(ObraBomba obraBomba)
        {
            _obraRepository.AtualizarDadosReajuste(obraBomba);
        }

        public void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, float valorReajustado, float valorServico, float descontoPercentual)
        {
            _obraRepository.AtualizarValorReajustePropostaItemVersao(numVersao, usina, anoProposta, numProposta, sequencia, valorReajustado, valorServico, descontoPercentual);
        }

        public void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, int m3ReajustadoAteAtual, float taxaMinimaReajustadaAtual, float m3PrecoReajustadoAtual, float descontoPercentual)
        {
            _obraRepository.AtualizarValorReajustePropostaBombaVersao(numVersao, usina, anoProposta, numProposta, sequencia, m3ReajustadoAteAtual, taxaMinimaReajustadaAtual, m3PrecoReajustadoAtual, descontoPercentual);
        }

        public void AtualizarTracoAtivoPropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, string ativo)
        {
            _obraRepository.AtualizarTracoAtivoPropostaItemVersao(numVersao, usina, anoProposta, numProposta, sequencia, ativo);
        }

        public int ObterTempoDescarga(int idUsina)
        {
            return _obraRepository.ObterTempoDescarga(idUsina);
        }

        public void AdicionarLogPropostaItem(PropostaItemLog log)
        {
            _obraRepository.AdicionarLogPropostaItem(log);
        }

        public void AdicionarLogPropostaItem(ObraTraco obraTraco, string source)
        {
            _obraRepository.AdicionarLogPropostaItem(obraTraco, source);
        }

        public void AdicionarLogPropostaItem(ObraTracoVersao obraTracoVersao, string source)
        {
            _obraRepository.AdicionarLogPropostaItem(obraTracoVersao, source);
        }
    }
}
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.ContratoReajuste;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoTracoReajuste.ContratoTracoReajusteVigenciasResponse;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class ContratoBombaReajusteApplicationService : ApplicationServiceBase<ContratoBombaReajuste>, IContratoBombaReajusteApplicationService
    {
        private readonly IContratoBombaReajusteService _contratoBombaReajusteService;
        private readonly IUsinaService _usinaService;
        private readonly IParametroService _parametroService;
        private readonly IPropostaService _propostaService;
        private readonly IContratoService _contratoService;
        private readonly IObraService _obraService;
        private readonly IObraTaxaService _obraTaxaService;
        private readonly IDemaisServicosService _demaisServicosService;
        private readonly IContratoApplicationService _contratoApplicationService;
        private readonly IContratoVersaoService _contratoVersaoService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IProgramacaoService _programacaoService;

        public ContratoBombaReajusteApplicationService(IContratoBombaReajusteService contratoBombaReajusteService, IUsinaService usinaService,
                                                       IParametroService parametroService, IPropostaService propostaService, IContratoService contratoService,
                                                       IObraService obraService, IObraTaxaService obraTaxaService, IDemaisServicosService demaisServicosService,
                                                       IContratoApplicationService contratoApplicationService, IContratoVersaoService contratoVersaoService,
                                                       IComercialLegacyService comercialLegacyService, IProgramacaoService programacaoService, IUnitOfWork unitOfWork) : base(contratoBombaReajusteService, unitOfWork)
        {
            _contratoBombaReajusteService = contratoBombaReajusteService;
            _usinaService = usinaService;
            _parametroService = parametroService;
            _propostaService = propostaService;
            _contratoService = contratoService;
            _obraService = obraService;
            _obraTaxaService = obraTaxaService;
            _demaisServicosService = demaisServicosService;
            _contratoApplicationService = contratoApplicationService;
            _contratoVersaoService = contratoVersaoService;
            _comercialLegacyService = comercialLegacyService;
            _programacaoService = programacaoService;
        }

        public PagedList<ContratoReajusteResponse> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter)
        {
            var result = AutoMapper.Mapper.Map<PagedList<ContratoBombaReajuste>, PagedList<ContratoReajusteResponse>>(
                    _contratoBombaReajusteService.ListarContratoReajusteBombaPorPagina(pagina, porPagina, filter),
                    new PagedList<ContratoReajusteResponse>());

            foreach (var reajuste in result.Records)
            {
                reajuste.UsinaEntrega = AutoMapper.Mapper.Map<Usina, UsinaDTO>(_usinaService.ObterPorId(reajuste.Contrato.UsinaEntrega), new UsinaDTO());

                reajuste.Bombas = AutoMapper.Mapper.Map<IEnumerable<ContratoBombaReajuste>, IEnumerable<ObraBombaReajusteResponse>>(_contratoBombaReajusteService.ListarContratoReajusteBombaPorContrato(reajuste.UsinaCodigo, reajuste.ContratoAno, reajuste.ContratoNumero, reajuste.DataVigencia), new List<ObraBombaReajusteResponse>());
            }

            return result;
        }

        public IEnumerable<ContratoReajusteVigenciasResponse> ObterVigencias()
        {
            var listaVigencias = _contratoBombaReajusteService.ObterVigencias();

            var listaContratoVigencias = new List<ContratoReajusteVigenciasResponse>();
            foreach (var vigencia in listaVigencias)
            {
                var contratoVigencia = new ContratoReajusteVigenciasResponse
                {
                    Vigencia = vigencia
                };

                listaContratoVigencias.Add(contratoVigencia);
            }

            return listaContratoVigencias.OrderBy(t => t.Vigencia);
        }

        public void AprovarReajuste(ContratoReajusteAlteracaoRequest contratoReajusteAlteracao, string usuario)
        {
            if (contratoReajusteAlteracao.DataConfirmacao == new DateTime(1, 1, 1) && contratoReajusteAlteracao.IdReprovacao.Equals(""))
            {
                using (var scope = new TransactionScope())
                {
                    var versionamentoReajusteContrato = _parametroService.ObterParametroN("web", "VersionamentoReajusteContrato").Contains("true");

                    if (_contratoVersaoService.ExisteVersaoEmAberto(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero))
                    {
                        AssertionConcern.Notify("Alteracao", "Não é possível realizar a aprovação, pois existe uma versão do contrato ainda não aprovada.");
                        return;
                    }

                    var obra = _obraService.ObterObraPorContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero);

                    obra.ObraTracos = _obraService.ListarObraTracos(obra.UsinaCodigo, obra.Numero).ObraTracos.ToList();

                    var contratoReajustes = _contratoBombaReajusteService.ListarContratoReajusteBombaPorContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, contratoReajusteAlteracao.DataVigencia).ToList();

                    foreach (var contratoReajuste in contratoReajustes)
                    {
                        contratoReajuste.DataConfirmacao = DateTime.Today;
                        contratoReajuste.IdAtualizacao = StringHelper.GetIDD(usuario);

                        if (versionamentoReajusteContrato) contratoReajuste.IdAprovacaoVersao = StringHelper.GetIDD(usuario);

                        _contratoBombaReajusteService.Atualizar(contratoReajuste);

                        var obraBomba = _obraService.ListarObraBombas(obra.UsinaCodigo, obra.Numero, true).ObraBombas.Where(t => t.Sequencia == contratoReajuste.ObraBombaReajusteSequencia).FirstOrDefault();

                        obraBomba.TaxaMinimaReajustadaAnterior = contratoReajuste.ValorVigente;
                        obraBomba.M3ReajustadoAteAnterior = contratoReajuste.VigenteAteM3;
                        obraBomba.M3PrecoReajustadoAnterior = contratoReajuste.M3ExcedenteVigente;
                        obraBomba.TaxaMinimaReajustadaAtual = contratoReajuste.ValorReajustado;
                        obraBomba.M3ReajustadoAteAtual = contratoReajuste.ReajustadoAteM3;
                        obraBomba.M3PrecoReajustadoAtual = contratoReajuste.M3ExcedenteReajustado;
                        obraBomba.DataUltimoReajuste = contratoReajuste.DataVigencia;

                        _obraService.AtualizarDadosReajuste(obraBomba);

                        Commit();

                        _obraService.CalcularEbitdaObraBomba(obraBomba, obra);

                        Commit();
                    }

                    var programacoesReajustadas = _programacaoService.ListarFiltrados(t => t.UsinaCodigo == contratoReajusteAlteracao.UsinaCodigo &&
                                                                          t.ContratoAno == contratoReajusteAlteracao.ContratoAno &&
                                                                          t.ContratoNumero == contratoReajusteAlteracao.ContratoNumero &&
                                                                          t.DataConcretagem >= contratoReajusteAlteracao.DataVigencia);
                    foreach (var programacao in programacoesReajustadas)
                    {
                        _comercialLegacyService.TotalizarValoresProgramacao(programacao);
                    }

                    Commit();

                    var logs = _contratoBombaReajusteService.ListarFiltrados<ContratoReajusteLog>(t => t.Usina == contratoReajusteAlteracao.UsinaCodigo && t.ContratoAno == contratoReajusteAlteracao.ContratoAno && t.ContratoNumero == contratoReajusteAlteracao.ContratoNumero && t.DataVigencia == contratoReajusteAlteracao.DataVigencia && t.Tipo == "bomba");
                    var sequenciaReajusteLog = logs.Count();

                    var numProximaVersao = _contratoService.GetUltimaVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero) + 1;

                    if (versionamentoReajusteContrato)
                    {
                        _propostaService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, numProximaVersao);
                        _contratoService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numProximaVersao);
                        _obraService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numProximaVersao, obra.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, obra.Numero);
                        _obraTaxaService.AdicionarVersaoContrato(obra.UsinaEntregaCodigo, numProximaVersao, obra.Numero);
                        _demaisServicosService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, numProximaVersao, obra.Numero);

                        if (numProximaVersao == 1)
                        {
                            _contratoApplicationService.SalvarPDFContratoVersao(1, contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero);

                            numProximaVersao = numProximaVersao + 1;
                            _propostaService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, numProximaVersao);
                            _contratoService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numProximaVersao);
                            _obraService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numProximaVersao, obra.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, obra.Numero);
                            _obraTaxaService.AdicionarVersaoContrato(obra.UsinaEntregaCodigo, numProximaVersao, obra.Numero);
                            _demaisServicosService.AdicionarVersaoContrato(obra.UsinaCodigo, numProximaVersao, obra.Numero);
                        }
                        else
                        {
                            _contratoApplicationService.SalvarPDFContratoVersao(numProximaVersao - 1, contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero);
                        }

                        _contratoBombaReajusteService.Adicionar<ContratoReajusteVersao>(new ContratoReajusteVersao
                        {
                            NumeroVersao = numProximaVersao,
                            Usina = contratoReajusteAlteracao.UsinaCodigo,
                            ContratoAno = contratoReajusteAlteracao.ContratoAno,
                            ContratoNumero = contratoReajusteAlteracao.ContratoNumero,
                            DataVigencia = contratoReajusteAlteracao.DataVigencia,
                            Tipo = "bomba"
                        });

                        Commit();

                        sequenciaReajusteLog++;
                        _contratoBombaReajusteService.Adicionar<ContratoReajusteLog>(new ContratoReajusteLog
                        {
                            Usina = contratoReajusteAlteracao.UsinaCodigo,
                            ContratoAno = contratoReajusteAlteracao.ContratoAno,
                            ContratoNumero = contratoReajusteAlteracao.ContratoNumero,
                            DataVigencia = contratoReajusteAlteracao.DataVigencia,
                            DataHoraEvento = DateTime.Now,
                            Tipo = "bomba",
                            Sequencia = sequenciaReajusteLog,
                            Usuario = usuario,
                            Evento = "APROVAÇÃO REAJUSTE",
                            Complemento = $"Versão de contrato gerada: {numProximaVersao}"
                        });

                        Commit();
                    }
                    else
                    {
                        var numVersao = _contratoService.GetUltimaVersaoContratoAprovado(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero);

                        if (numVersao > 0)
                        {
                            _propostaService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, numVersao);
                            _contratoService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numVersao);
                            _obraService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, numVersao, obra.UsinaCodigo, obra.AnoChamada.Value, obra.NumChamada.Value, obra.Numero);
                            _obraTaxaService.AdicionarVersaoContrato(obra.UsinaEntregaCodigo, numVersao, obra.Numero);
                            _demaisServicosService.AdicionarVersaoContrato(contratoReajusteAlteracao.UsinaCodigo, numVersao, obra.Numero);
                        }

                        sequenciaReajusteLog++;
                        _contratoBombaReajusteService.Adicionar<ContratoReajusteLog>(new ContratoReajusteLog
                        {
                            Usina = contratoReajusteAlteracao.UsinaCodigo,
                            ContratoAno = contratoReajusteAlteracao.ContratoAno,
                            ContratoNumero = contratoReajusteAlteracao.ContratoNumero,
                            DataVigencia = contratoReajusteAlteracao.DataVigencia,
                            DataHoraEvento = DateTime.Now,
                            Tipo = "bomba",
                            Sequencia = sequenciaReajusteLog,
                            Usuario = usuario,
                            Evento = "APROVAÇÃO REAJUSTE",
                            Complemento = ""
                        });

                        Commit();
                    }

                    scope.Complete();
                }
            }
        }

        public void AprovarTodos(IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajustes, string usuario)
        {
            var versionamentoReajusteContrato = _parametroService.ObterParametroN("web", "VersionamentoReajusteContrato").Contains("true");

            foreach (var reajuste in contratoReajustes)
            {
                if (_contratoVersaoService.ExisteVersaoEmAberto(reajuste.UsinaCodigo, reajuste.ContratoAno, reajuste.ContratoNumero))
                {
                    AssertionConcern.Notify("Alteracao", "Não é possível realizar a aprovação, pois existe(m) uma versão(ões) do(s) contrato(s) ainda não aprovada(s).");
                    return;
                }
            }

            foreach (var reajuste in contratoReajustes)
            {
                AprovarReajuste(reajuste, usuario);
            }
        }

        public void ReprovarReajuste(ContratoReajusteAlteracaoRequest contratoReajusteAlteracao, string usuario)
        {
            if (contratoReajusteAlteracao.DataConfirmacao == new DateTime(1, 1, 1) && contratoReajusteAlteracao.IdReprovacao.Equals(""))
            {
                using (var scope = new TransactionScope())
                {
                    var logs = _contratoBombaReajusteService.ListarFiltrados<ContratoReajusteLog>(t => t.Usina == contratoReajusteAlteracao.UsinaCodigo && t.ContratoAno == contratoReajusteAlteracao.ContratoAno && t.ContratoNumero == contratoReajusteAlteracao.ContratoNumero && t.DataVigencia == contratoReajusteAlteracao.DataVigencia && t.Tipo == "bomba");

                    var sequenciaReajusteLog = logs.Count();

                    sequenciaReajusteLog++;
                    _contratoBombaReajusteService.Adicionar<ContratoReajusteLog>(new ContratoReajusteLog
                    {
                        Usina = contratoReajusteAlteracao.UsinaCodigo,
                        ContratoAno = contratoReajusteAlteracao.ContratoAno,
                        ContratoNumero = contratoReajusteAlteracao.ContratoNumero,
                        DataVigencia = contratoReajusteAlteracao.DataVigencia,
                        DataHoraEvento = DateTime.Now,
                        Tipo = "bomba",
                        Sequencia = sequenciaReajusteLog,
                        Usuario = usuario,
                        Evento = "REPROVAÇÃO REAJUSTE",
                        Complemento = ""
                    });

                    var contratoReajustes = _contratoBombaReajusteService.ListarContratoReajusteBombaPorContrato(contratoReajusteAlteracao.UsinaCodigo, contratoReajusteAlteracao.ContratoAno, contratoReajusteAlteracao.ContratoNumero, contratoReajusteAlteracao.DataVigencia).ToList();
                    foreach (var contratoReajuste in contratoReajustes)
                    {
                        contratoReajuste.IdAtualizacao = StringHelper.GetIDD(usuario);
                        contratoReajuste.IdReprovacao = StringHelper.GetIDD(usuario);

                        _contratoBombaReajusteService.Atualizar(contratoReajuste);
                    }

                    Commit();

                    scope.Complete();
                }
            }
        }

        public void ReprovarTodos(IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajustes, string usuario)
        {
            foreach (var reajuste in contratoReajustes)
            {
                ReprovarReajuste(reajuste, usuario);
            }
        }

        public IEnumerable<ContratoReajusteLogResponse> ListaReajusteLogs(int usina, int contratoAno, int contratoNumero, DateTime dataVigencia)
        {
            var logs = _contratoBombaReajusteService.ListarFiltrados<ContratoReajusteLog>(t => t.Usina == usina && t.ContratoAno == contratoAno && t.ContratoNumero == contratoNumero && t.DataVigencia == dataVigencia && t.Tipo == "bomba");

            return AutoMapper.Mapper.Map(logs, new List<ContratoReajusteLogResponse>());
        }
    }
}

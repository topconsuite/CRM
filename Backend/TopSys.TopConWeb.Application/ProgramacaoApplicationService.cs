using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoDetalhadaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoHoraResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoIntegracao;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoLogResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class ProgramacaoApplicationService : ApplicationServiceBase<Programacao>, IProgramacaoApplicationService
    {
        public readonly IProgramacaoService _programacaoService;
        public readonly IObraService _obraService;
        public readonly IContratoService _contratoService;
        public readonly IComercialLegacyService _comercialLegacyService;
        public readonly IUsinaService _usinaService;
        private readonly IHeaderProvider _headerProvider;

        public ProgramacaoApplicationService(
            IProgramacaoService programacaoService,
            IObraService obraService,
            IContratoService contratoService,
            IComercialLegacyService comercialLegacyService,
            IUsinaService usinaService,
            IUnitOfWork unityOfWork, IHeaderProvider headerProvider) :
            base(programacaoService, unityOfWork)
        {
            _programacaoService = programacaoService;
            _obraService = obraService;
            _contratoService = contratoService;
            _comercialLegacyService = comercialLegacyService;
            _usinaService = usinaService;
            _headerProvider = headerProvider;
        }

        public void Adicionar(string usuario, ProgramacaoInclusaoRequest programacao)
        {
            var programacaoNew = AutoMapper.Mapper.Map(programacao, new Programacao());
            var usinaEntrega = _usinaService.ObterPorId<Usina>(programacaoNew.UsinaEntregaCodigo);

            if (usinaEntrega.MoldagemRemota == "S" && programacaoNew.CorpoDeProvaMoldagemRemota == "")
            {
                programacaoNew.CorpoDeProvaMoldagemRemota = "Padrao da Central";
            }

            programacaoNew.FaxSimNao = "N";
            programacaoNew.DataHoraSolicitacao = DateTime.Now.ToString("dd/MM/yy HH:mm");
            programacaoNew.IdCadastro = StringHelper.GetIDD(usuario);

            var obraTracoOld = _obraService.ObterPorId<ObraTraco>(programacaoNew.UsinaCodigo, programacaoNew.ObraNumero, programacaoNew.ObraTracoSequencia);

            using (var scope = new TransactionScope())
            {

                programacaoNew.TracoProgramacaoScopeIsValid(obraTracoOld);

                if (programacaoNew.ObraTracoSequencia >= 0)
                {

                    var obraOld = _obraService.ListarFiltrados(t => t.UsinaCodigo == programacaoNew.UsinaCodigo && t.Numero == programacaoNew.ObraNumero, t => t.TipoCobranca).FirstOrDefault();

                    var volumeOutrasProgramacoes = _programacaoService
                            .ListarFiltrados(t => t.UsinaCodigo == programacaoNew.UsinaCodigo
                                && t.ObraNumero == programacaoNew.ObraNumero
                                && t.ObraTracoSequencia == programacaoNew.ObraTracoSequencia
                                && t.Sequencia != programacaoNew.Sequencia
                                && t.Status != EProgramacaoStatus.Cancelada)
                            .Sum(t => t.VolumeTotal);

                    
                }

                if (programacaoNew.ContratoNumero == null || programacaoNew.ContratoNumero == 0)
                {
                    var obra = _obraService.ObterPorId(programacaoNew.UsinaCodigo, programacaoNew.ObraNumero);
                    programacaoNew.ContratoAno = obra?.AnoContrato ?? 0;
                    programacaoNew.ContratoNumero = obra?.NumContrato ?? 0;
                } 

                _programacaoService.Adicionar(programacaoNew);

                foreach (var demaisServico in programacaoNew.DemaisServicos)
                {
                    demaisServico.UsinaCodigo = programacaoNew.UsinaCodigo;
                    demaisServico.ObraNumero = programacaoNew.ObraNumero;
                    demaisServico.ProgramacaoSequencia = programacaoNew.Sequencia;

                    _programacaoService.Adicionar(demaisServico);
                }

                _programacaoService.Adicionar(new ProgramacaoLog() {
                    UsinaCodigo = programacaoNew.UsinaCodigo,
                    ObraCodigo = programacaoNew.ObraNumero,
                    ProgramacaoSequencia = programacaoNew.Sequencia,
                    PropostaAno = programacaoNew.PropostaAno,
                    PropostaNumero = programacaoNew.PropostaNumero,
                    ContratoAno = programacaoNew.ContratoAno,
                    ContratoNumero = programacaoNew.ContratoNumero,
                    DataHora = DateTime.Now,
                    Horario = "",
                    Usuario = usuario,
                    Evento = "Inserção Prog",
                    Complemento = "AGUARDANDO CONFIRMAÇÃO",
                    Descricao = "",
                    Sequencia = 1
                });
                Commit();

                var contrato = programacaoNew.ContratoNumero > 0 ? _contratoService.ObterPorId(programacaoNew.UsinaCodigo, programacaoNew.ContratoAno, programacaoNew.ContratoNumero) : null;

                if (contrato != null)
                {
                    var contratoStatusAnterior = contrato.Status;

                    if (contrato.Status == EContratoStatus.AguardandoDataProgramacao)
                    {
                        contrato.Status = EContratoStatus.EmAnalise;
                        Commit();

                        _obraService.Adicionar(new ObraLog()
                        {
                            UsinaCodigo = programacaoNew.UsinaCodigo,
                            ObraCodigo = programacaoNew.ObraNumero,
                            AnoChamada = programacaoNew.PropostaAno ?? 0,
                            NumChamada = programacaoNew.PropostaNumero ?? 0,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "CONTRATO EM ANÁLISE",
                            Complemento = $"{programacaoNew.UsinaCodigo}/{contrato.Numero.ToString().PadLeft(6, '0')}-{contrato.Ano}",
                            Observacao = "Programação Inserida",
                            Sequencia = 1
                        });
                        Commit();

                    }
                }

                if (!_notifications.HasNotifications())
                    scope.Complete();
            }
                
        }

        public ObraBomba ObterBombaDaProgramacao(int idUsina, int obraNumero, int sequencia)
        {

            var programacao = _programacaoService
                .ListarFiltrados<Programacao>(t => 
                    t.UsinaCodigo == idUsina && 
                    t.ObraNumero == obraNumero && 
                    t.Sequencia == sequencia).FirstOrDefault();

            var bombaSequencia = programacao.ObraBombaSequencia;

            if (bombaSequencia == 0)
                return null;

            var bomba = _programacaoService.ListarFiltrados<ObraBomba>(
                x => x.UsinaCodigo == idUsina
                && x.ObraCodigo == obraNumero
                && x.Sequencia == bombaSequencia, x => x.BombaTipo).FirstOrDefault();

            if (bomba != null)
                return bomba;

            var bombaVersao = _programacaoService.ListarFiltrados<ObraBombaVersao>(
                x => x.UsinaCodigo == idUsina
                && x.ObraCodigo == obraNumero
                && x.Sequencia == bombaSequencia, x => x.BombaTipo)
                .OrderByDescending(x => x.NumeroVersao).FirstOrDefault();

            if (bombaVersao is null)
                return null;

            return AutoMapper.Mapper.Map(bombaVersao, new ObraBomba());


        }

        public void Atualizar(string usuario, ProgramacaoAlteracaoRequest programacao)
        {
            var programacaoOld = _programacaoService.ObterPorId(programacao.Usina.Codigo, programacao.ObraNumero, programacao.Sequencia); 

            var usinaEntregaAnterior = programacaoOld.UsinaEntregaCodigo;
            var dataAnterior = programacaoOld.DataConcretagem;
            var horaAnterior = programacaoOld.Horario;
            var necessitaConfirmacaoAnterior = programacaoOld.NecessitaConfirmacao;
            var intervaloAnterior = programacaoOld.IntervaloEmMinutosEntreCargas;
            var solicitanteAnterior = programacaoOld.Solicitante;
            var observacaoAnterior = programacaoOld.Observacao;

            var pecaConcretarAnterior = programacaoOld.PecaConcretar;
            var andarAnterior = programacaoOld.Andar;
            var tracoAnterior = programacaoOld.ObraTracoSequencia;
            var volumeTotalAnterior = programacaoOld.VolumeTotal;
            var volumePorCargaAnterior = programacaoOld.VolumePorCarga;

            var bombaAnterior = programacaoOld.ObraBombaSequencia;
            var distanciaTubulacaoAnterior = programacaoOld.DistanciaTubulacao;
            var horarioBombaAnterior = programacaoOld.HorarioBomba;

            var vibradorQuantidadeAnterior = programacaoOld.VibradorQuantidade;
            var vibradorValorUnitarioAnterior = programacaoOld.VibradorValorUnitario;
            var vibradorVendedorCodigoAnterior = programacaoOld.VibradorVendedorCodigo;

            using (var scope = new TransactionScope())
            {
                var programacaoNew = AutoMapper.Mapper.Map(programacao, programacaoOld);

                if (_programacaoService.Validar(programacaoNew))
                {
                    var obraTracoOld = _obraService.ObterPorId<ObraTraco>(programacaoNew.UsinaCodigo, programacaoNew.ObraNumero, programacaoNew.ObraTracoSequencia);
                    programacaoNew.TracoProgramacaoScopeIsValid(obraTracoOld);

                    if (programacaoNew.ObraTracoSequencia >= 0 && volumeTotalAnterior < programacaoNew.VolumeTotal)
                    {
                        var obraOld = _obraService.ListarFiltrados(t => t.UsinaCodigo == programacaoNew.UsinaCodigo && t.Numero == programacaoNew.ObraNumero, t => t.TipoCobranca).FirstOrDefault();

                        var volumeOutrasProgramacoes = _programacaoService
                            .ListarFiltrados(t => t.UsinaCodigo == programacaoNew.UsinaCodigo
                                && t.ObraNumero == programacaoNew.ObraNumero
                                && t.ObraTracoSequencia == programacaoNew.ObraTracoSequencia
                                && t.Sequencia != programacaoNew.Sequencia
                                && t.Status != EProgramacaoStatus.Cancelada)
                            .Sum(t => t.VolumeTotal);
                        
                    }

                    // ObraDemaisServicos
                    var sequencias = new List<int>();
                    foreach (var tDto in programacao.DemaisServicos)
                    {
                        sequencias.Add(tDto.Sequencia);
                        tDto.UsinaCodigo = programacaoNew.UsinaCodigo;
                        tDto.ObraNumero = programacaoNew.ObraNumero;
                        tDto.ProgramacaoSequencia = programacaoNew.Sequencia;

                        var demaisServicosOld = _programacaoService.ObterPorId<ProgramacaoDemaisServicos>
                            (tDto.UsinaCodigo, tDto.ObraNumero, tDto.ProgramacaoSequencia, tDto.Sequencia);

                        if (demaisServicosOld != null)
                        {
                            demaisServicosOld = AutoMapper.Mapper.Map(tDto, demaisServicosOld);
                        }
                        else
                        {
                            demaisServicosOld = AutoMapper.Mapper.Map<ProgramacaoDemaisServicos>(tDto);
                            _programacaoService.Adicionar(demaisServicosOld);
                        }
                        Commit();
                    }

                    var seqs = sequencias.ToArray();
                    var demaisServicosExcluidos = _programacaoService.ListarFiltradosTracking<ProgramacaoDemaisServicos>
                        (t => t.UsinaCodigo == programacaoNew.UsinaCodigo
                            && t.ObraNumero == programacaoNew.ObraNumero
                            && t.ProgramacaoSequencia == programacaoNew.Sequencia
                            && !seqs.Contains(t.Sequencia));

                    foreach (var t in demaisServicosExcluidos)
                    {
                        _programacaoService.Remover(t);
                        Commit();
                    }

                    if (!_notifications.HasNotifications())
                    {
                        string dadosAlteradosProgramacao = "";
                        string dadosAlteradosTraco = "";
                        string dadosAlteradosBomba = "";
                        string dadosAlteradosVibrador = "";

                        if (programacaoNew.UsinaEntregaCodigo != usinaEntregaAnterior)
                            dadosAlteradosProgramacao += $"Usina Entrega: {usinaEntregaAnterior} -> {programacaoNew.UsinaEntregaCodigo} ";
                        if (programacaoNew.DataConcretagem != dataAnterior)
                            dadosAlteradosProgramacao += $"Data: {dataAnterior.ToShortDateString()} -> {programacaoNew.DataConcretagem.ToShortDateString()} ";
                        if (programacaoNew.Horario != horaAnterior)
                            dadosAlteradosProgramacao += $"Hora: {horaAnterior} -> {programacaoNew.Horario} ";
                        if (programacaoNew.NecessitaConfirmacao != necessitaConfirmacaoAnterior)
                            dadosAlteradosProgramacao += $"A confirmar: {necessitaConfirmacaoAnterior} -> {programacaoNew.NecessitaConfirmacao} ";
                        if (programacaoNew.IntervaloEmMinutosEntreCargas != intervaloAnterior)
                            dadosAlteradosProgramacao += $"Intervalo: {intervaloAnterior} -> {programacaoNew.IntervaloEmMinutosEntreCargas} ";
                        if (programacaoNew.Solicitante != solicitanteAnterior)
                            dadosAlteradosProgramacao += $"Solicitante: {solicitanteAnterior} -> {programacaoNew.Solicitante} ";
                        if (programacaoNew.Observacao != observacaoAnterior)
                            dadosAlteradosProgramacao += $"Observacao: {observacaoAnterior} -> {programacaoNew.Observacao} ";

                        if (programacaoNew.PecaConcretar != pecaConcretarAnterior)
                            dadosAlteradosTraco += $"Peça: {pecaConcretarAnterior} -> {programacaoNew.PecaConcretar} ";
                        if (programacaoNew.Andar != andarAnterior)
                            dadosAlteradosTraco += $"Andar: {andarAnterior} -> {programacaoNew.Andar} ";
                        if (programacaoNew.ObraTracoSequencia != tracoAnterior)
                            dadosAlteradosTraco += $"Traço: {tracoAnterior} -> {programacaoNew.ObraTracoSequencia} ";
                        if (programacaoNew.VolumeTotal != volumeTotalAnterior)
                            dadosAlteradosTraco += $"Qtde.M3: {volumeTotalAnterior} -> {programacaoNew.VolumeTotal} ";
                        if (programacaoNew.VolumePorCarga != volumePorCargaAnterior)
                            dadosAlteradosTraco += $"Qtde/BT: {volumePorCargaAnterior} -> {programacaoNew.VolumePorCarga} ";

                        if (programacaoNew.ObraBombaSequencia != bombaAnterior)
                            dadosAlteradosBomba += $"Bomba: {bombaAnterior} -> {programacaoNew.ObraBombaSequencia} ";
                        if (programacaoNew.DistanciaTubulacao != distanciaTubulacaoAnterior)
                            dadosAlteradosBomba += $"Dist.Tub.: {distanciaTubulacaoAnterior} -> {programacaoNew.DistanciaTubulacao} ";
                        if (programacaoNew.HorarioBomba != horarioBombaAnterior)
                            dadosAlteradosBomba += $"Horário bomba: {horarioBombaAnterior} -> {programacaoNew.HorarioBomba} ";

                        if (programacaoNew.VibradorQuantidade != vibradorQuantidadeAnterior)
                            dadosAlteradosVibrador += $"Vibr.Qtde.: {vibradorQuantidadeAnterior} -> {programacaoNew.VibradorQuantidade} ";
                        if (programacaoNew.VibradorValorUnitario != vibradorValorUnitarioAnterior)
                            dadosAlteradosVibrador += $"Vibr.Vlr.Unit.: {vibradorValorUnitarioAnterior} -> {programacaoNew.VibradorValorUnitario} ";
                        if (programacaoNew.VibradorVendedorCodigo != vibradorVendedorCodigoAnterior)
                            dadosAlteradosVibrador += $"Vibr.Vendedor: {vibradorVendedorCodigoAnterior} -> {programacaoNew.VibradorVendedorCodigo} ";

                        var i = 1;
                        if (dadosAlteradosProgramacao != "")
                        {
                            _programacaoService.Adicionar(new ProgramacaoLog()
                            {
                                UsinaCodigo = programacaoNew.UsinaCodigo,
                                ObraCodigo = programacaoNew.ObraNumero,
                                ProgramacaoSequencia = programacaoNew.Sequencia,
                                PropostaAno = programacaoNew.PropostaAno,
                                PropostaNumero = programacaoNew.PropostaNumero,
                                ContratoAno = programacaoNew.ContratoAno,
                                ContratoNumero = programacaoNew.ContratoNumero,
                                DataHora = DateTime.Now,
                                Horario = "",
                                Usuario = usuario,
                                Evento = "Alteração Dados",
                                Complemento = "Dados programação",
                                Descricao = dadosAlteradosProgramacao,
                                Sequencia = i++
                            });
                        }

                        if (dadosAlteradosTraco != "")
                        {
                            _programacaoService.Adicionar(new ProgramacaoLog()
                            {
                                UsinaCodigo = programacaoNew.UsinaCodigo,
                                ObraCodigo = programacaoNew.ObraNumero,
                                ProgramacaoSequencia = programacaoNew.Sequencia,
                                PropostaAno = programacaoNew.PropostaAno,
                                PropostaNumero = programacaoNew.PropostaNumero,
                                ContratoAno = programacaoNew.ContratoAno,
                                ContratoNumero = programacaoNew.ContratoNumero,
                                DataHora = DateTime.Now,
                                Horario = "",
                                Usuario = usuario,
                                Evento = "Alteração Dados",
                                Complemento = "Dados traço",
                                Descricao = dadosAlteradosTraco,
                                Sequencia = i++
                            });
                        }

                        if (dadosAlteradosBomba != "")
                        {
                            _programacaoService.Adicionar(new ProgramacaoLog()
                            {
                                UsinaCodigo = programacaoNew.UsinaCodigo,
                                ObraCodigo = programacaoNew.ObraNumero,
                                ProgramacaoSequencia = programacaoNew.Sequencia,
                                PropostaAno = programacaoNew.PropostaAno,
                                PropostaNumero = programacaoNew.PropostaNumero,
                                ContratoAno = programacaoNew.ContratoAno,
                                ContratoNumero = programacaoNew.ContratoNumero,
                                DataHora = DateTime.Now,
                                Horario = "",
                                Usuario = usuario,
                                Evento = "Alteração Dados",
                                Complemento = "Dados bomba",
                                Descricao = dadosAlteradosBomba,
                                Sequencia = i++
                            });
                        }

                        if (dadosAlteradosVibrador != "")
                        {
                            _programacaoService.Adicionar(new ProgramacaoLog()
                            {
                                UsinaCodigo = programacaoNew.UsinaCodigo,
                                ObraCodigo = programacaoNew.ObraNumero,
                                ProgramacaoSequencia = programacaoNew.Sequencia,
                                PropostaAno = programacaoNew.PropostaAno,
                                PropostaNumero = programacaoNew.PropostaNumero,
                                ContratoAno = programacaoNew.ContratoAno,
                                ContratoNumero = programacaoNew.ContratoNumero,
                                DataHora = DateTime.Now,
                                Horario = "",
                                Usuario = usuario,
                                Evento = "Alteração Dados",
                                Complemento = "Dados vibrador",
                                Descricao = dadosAlteradosVibrador,
                                Sequencia = i++
                            });
                        }

                        if (dadosAlteradosProgramacao != "" || dadosAlteradosTraco != "" || dadosAlteradosBomba!= "" || dadosAlteradosVibrador != "")
                        {
                            programacaoOld.Status = programacaoOld.Status == EProgramacaoStatus.Programado ? EProgramacaoStatus.Revalidacao : EProgramacaoStatus.AguardandoConfirmacao;
                        }

                        Commit();

                        _comercialLegacyService.TotalizarValoresProgramacao(programacaoNew);

                        scope.Complete();
                    }
                }
                
            }
                
        }

        public void CancelarPorId(string usuario, int idUsina, int obraNumero, int sequencia, string observacao)
        {
            var programacao = _programacaoService.ObterDetalhadaPorId(idUsina, obraNumero, sequencia, true);

            programacao.CancelamentoScopeIsValid();

            programacao.Status = EProgramacaoStatus.Revalidacao;
            programacao.EquipamentoBombaCodigo = "";

            if (Commit())
            {
                _programacaoService.Adicionar(new ProgramacaoLog()
                {
                    UsinaCodigo = programacao.UsinaCodigo,
                    ObraCodigo = programacao.ObraNumero,
                    ProgramacaoSequencia = programacao.Sequencia,
                    PropostaAno = programacao.PropostaAno,
                    PropostaNumero = programacao.PropostaNumero,
                    ContratoAno = programacao.ContratoAno,
                    ContratoNumero = programacao.ContratoNumero,
                    DataHora = DateTime.Now,
                    Horario = "",
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO STATUS",
                    Complemento = "CANCELADA",
                    Descricao = observacao,
                    Sequencia = 1
                });
                Commit();
            }
        }

        public IEnumerable<ProgramacaoSimplesResponse> ListarComPropostaContrato()
        {
            var programacoes = AutoMapper.Mapper.Map(_programacaoService.ListarComPropostaContrato(), new List<ProgramacaoSimplesResponse>());
            return programacoes;
        }

        public PagedList<ProgramacaoSimplesResponse> ListarComPropostaContratoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            var programacoes = _programacaoService.ListarComPropostaContratoEmOrdemDescrescente(pagina, porPagina, filter);

            var programacoesDto = AutoMapper.Mapper.Map(programacoes, new PagedList<ProgramacaoSimplesResponse>());

            return programacoesDto;
        }

        public PagedList<ProgramacaoSimplesResponse> ListarComPropostaContratoEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Programacao, bool>> filter)
        {
            var programacoes = _programacaoService.ListarComPropostaContratoEmOrdemCrescente(pagina, porPagina, filter);

            var programacoesDto = AutoMapper.Mapper.Map(programacoes, new PagedList<ProgramacaoSimplesResponse>());

            return programacoesDto;
        }

        public IEnumerable<ProgramacaoLogResponse> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia)
        {
            var programacaoLogs = AutoMapper.Mapper.Map(_programacaoService.ListarProgramacaoLogsPorId(idUsina, obraNumero, sequencia), new List<ProgramacaoLogResponse>());

            return programacaoLogs;
        }

        public ProgramacaoDetalhadaResponse ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia)
        {
            var programacao = AutoMapper.Mapper.Map(_programacaoService.ObterDetalhadaPorId(idUsina, obraNumero, sequencia), new ProgramacaoDetalhadaResponse());

            return programacao;
        }

        public bool TemNotaFiscalEmitida(int idUsina, int obraNumero, int sequencia)
        {
            return _programacaoService.TemNotaFiscalEmitida(idUsina, obraNumero, sequencia);
        }

        public bool TemComplexidadeBombeado(int idUsina, int obraNumero, int sequencia)
        {
            var programacao = _programacaoService.ObterDetalhadaPorId(idUsina, obraNumero, sequencia);
            return _comercialLegacyService.TemComplexidadeBombeado(programacao);
        }

        public string VerificaContinuidade(int idUsina, int obraNumero, int sequencia)
        {
            var programacao = _programacaoService.ObterDetalhadaPorId(idUsina, obraNumero, sequencia);
            return _comercialLegacyService.VerificaContinuidade(programacao);
        }

        public float ObterVolumeTotalProgramado(int idUsina, int obraNumero)
        {
            return _programacaoService.ObterVolumeTotalProgramado(idUsina, obraNumero);
        }

        public IEnumerable<ProgramacaoHoraResponse> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            var horarios = AutoMapper.Mapper.Map(_programacaoService.ListarHorarios(idUsina, contratoAno, contratoNumero, sequencia), new List<ProgramacaoHoraResponse>());

            return horarios;
        }

        public bool GeraProgramacao(int idUsina, int obraNumero, int sequencia, bool atualizaComplexidadeBombeado, bool gravaContinuidadeProgramacao, string usuario)
        {
            return _programacaoService.GeraProgramacao(idUsina, obraNumero, sequencia, atualizaComplexidadeBombeado, gravaContinuidadeProgramacao, usuario);
        }

        public bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, string observacao, string usuario)
        {
            var programacao = _programacaoService.ObterPorId(idUsina, obraNumero, sequencia);

            var result = _programacaoService.RejeitaProgramacao(idUsina, obraNumero, sequencia, observacao, usuario);

            if (Commit())
            {
                _programacaoService.Adicionar(new ProgramacaoLog()
                {
                    UsinaCodigo = programacao.UsinaCodigo,
                    ObraCodigo = programacao.ObraNumero,
                    ProgramacaoSequencia = programacao.Sequencia,
                    PropostaAno = programacao.PropostaAno,
                    PropostaNumero = programacao.PropostaNumero,
                    ContratoAno = programacao.ContratoAno,
                    ContratoNumero = programacao.ContratoNumero,
                    DataHora = DateTime.Now,
                    Horario = "",
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO STATUS",
                    Complemento = "REPROVADO",
                    Descricao = observacao
                });
                Commit();
            }

            return result;
        }

        public void AprovaFinanceiro(int idUsina, int obraNumero, int sequencia, string usuario)
        {
            var programacao = _programacaoService.ObterPorId(idUsina, obraNumero, sequencia);

            _programacaoService.AprovaFinanceiro(idUsina, obraNumero, sequencia, usuario);

            if (Commit())
            {
                _programacaoService.Adicionar(new ProgramacaoLog()
                {

                    UsinaCodigo = programacao.UsinaCodigo,
                    ObraCodigo = programacao.ObraNumero,
                    ProgramacaoSequencia = programacao.Sequencia,
                    PropostaAno = programacao.PropostaAno,
                    PropostaNumero = programacao.PropostaNumero,
                    ContratoAno = programacao.ContratoAno,
                    ContratoNumero = programacao.ContratoNumero,
                    DataHora = DateTime.Now,
                    Horario = "",
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO STATUS",
                    Complemento = "APROVADO",
                    Descricao = ""
                });
                Commit();
            }
        }

        // integracao

        public ResultDTO<ProgramacaoAdicionarResponse> Adicionar(ProgramacaoAdicionarRequest[] request)
        {
            using (var scope = new TransactionScope())
            {
                int i = 0;
                try
                {
                    // == Validar Request =====================================================================

                    var errors = new List<Error>();
                    var idioma = _headerProvider.GetAcceptLanguage();

                    for (i = 0; i < request.Length; i++)
                    {
                        var programacao = request[i];

                        var usinaValida = _programacaoService.ObterPorId<Usina>(programacao.UsinaCodigo) != null;
                        var usinaEntregaValida = _programacaoService.ObterPorId<Usina>(programacao.UsinaEntregaCodigo) != null;
                        var enderecoMunicipioValido = programacao.EnderecoMunicipioCodigo == 0 ? true : _programacaoService.ObterPorId<Municipio>(programacao.EnderecoMunicipioCodigo) != null;
                        var pedraValida = programacao.PedraCodigo == 0 ? true : _programacaoService.ObterPorId<Pedra>(programacao.PedraCodigo) != null;
                        var usoValido = programacao.UsoCodigo == 0 ? true : _programacaoService.ObterPorId<Uso>(programacao.UsoCodigo) != null;
                        var equipamentoBombaValido = string.IsNullOrEmpty(programacao.EquipamentoBombaCodigo) ? true : _programacaoService.ObterPorId<Equipamento>(programacao.EquipamentoBombaCodigo) != null;
                        var resistenciaTipoValido = programacao.ResistenciaTipoCodigo == 0 ? true : _programacaoService.ObterPorId<ResistenciaTipo>(programacao.ResistenciaTipoCodigo) != null;
                        var slumpValido = programacao.SlumpCodigo == 0 ? true : _programacaoService.ObterPorId<Slump>(programacao.SlumpCodigo) != null;
                        var contratoValido = _programacaoService.ObterPorId<Contrato>(programacao.UsinaCodigo, programacao.ContratoAno, programacao.ContratoNumero) != null;
                        var itemProposta = _programacaoService.ObterPorId<ObraTraco>(programacao.UsinaCodigo, programacao.ObraNumero, programacao.ObraTracoSequencia);
                        var itemPropostaValido = programacao.ObraTracoSequencia == 0 ? true : itemProposta != null;
                        var tracoValido = programacao.ObraTracoSequencia == 0  || !itemPropostaValido ? true : itemProposta.Consumo == programacao.Consumo && itemProposta.Fck == programacao.Mpa &&
                            itemProposta.PedraCodigo == programacao.PedraCodigo && itemProposta.UsoCodigo == programacao.UsoCodigo && itemProposta.SlumpCodigo == programacao.SlumpCodigo
                            && itemProposta.ResistenciaTipoCodigo == programacao.ResistenciaTipoCodigo;
                        var validaQuantidadeProgramada = _programacaoService.ValidaQuantidadeProgramada();
                        var quantidadeValida = !validaQuantidadeProgramada ? true : programacao.VolumeTotal <= itemProposta.M3Quantidade;
                        var programacaoJaExiste = _programacaoService.ListarFiltrados(f => f.UsinaCodigo == programacao.UsinaCodigo && f.ContratoAno == programacao.ContratoAno && f.ContratoNumero == programacao.ContratoNumero && f.Sequencia == programacao.Sequencia).FirstOrDefault() != null;


                        if (!usinaValida)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393632.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393632.GetResourceMessage(idioma),
                                i));

                        if (!usinaEntregaValida)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393633.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393633.GetResourceMessage(idioma),
                                i));

                        if (!enderecoMunicipioValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393634.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393634.GetResourceMessage(idioma),
                                i));

                        if (!pedraValida)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393635.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393635.GetResourceMessage(idioma),
                                i));

                        if (!usoValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393636.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393636.GetResourceMessage(idioma),
                                i));

                        if (!equipamentoBombaValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393637.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393637.GetResourceMessage(idioma),
                                i));

                        if (!resistenciaTipoValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393638.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393638.GetResourceMessage(idioma),
                                i));

                        if (!slumpValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393639.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393639.GetResourceMessage(idioma),
                                i));

                        if (!contratoValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363130.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363130.GetResourceMessage(idioma),
                                i));

                        if (!itemPropostaValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363131.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363131.GetResourceMessage(idioma),
                                i));

                        if (!tracoValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363132.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363132.GetResourceMessage(idioma),
                                i));


                        if (!quantidadeValida)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363133.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363133.GetResourceMessage(idioma),
                                i));

                        if (programacaoJaExiste)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363138.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363138.GetResourceMessage(idioma),
                                i));
                    }

                    if (errors.Count > 0)
                        return new ResultDTO<ProgramacaoAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

                    // Inserção =================================================================================

                    for (i = 0; i < request.Length; i++)
                    {
                        var programacao = new Programacao()
                        {
                            FaxSimNao = "N",
                            DataHoraSolicitacao = DateTime.Now.ToString("dd/MM/yy HH:mm"),
                            IdCadastro = StringHelper.GetIDD("APICRM"),
                            IdAtual = ""
                        };

                        AutoMapper.Mapper.Map(request[i], programacao);

                        _programacaoService.Adicionar(programacao);

                        var programacaoLog = new ProgramacaoLog()
                        {
                            UsinaCodigo = programacao.UsinaCodigo,
                            ObraCodigo = programacao.ObraNumero,
                            ProgramacaoSequencia = programacao.Sequencia,
                            PropostaAno = programacao.PropostaAno,
                            PropostaNumero = programacao.PropostaNumero,
                            ContratoAno = programacao.ContratoAno,
                            ContratoNumero = programacao.ContratoNumero,
                            DataHora = DateTime.Now,
                            Horario = "",
                            Usuario = "",
                            Evento = "Inserção Prog",
                            Complemento = "AGUARDANDO CONFIRMAÇÃO",
                            Descricao = "Inserção via API",
                            Sequencia = 1
                        };

                        _programacaoService.Adicionar(programacaoLog);
                    }

                    Commit();
                    scope.Complete();

                    var result = new ProgramacaoAdicionarResponse(request.Length);

                    return new ResultDTO<ProgramacaoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting equipaments.", result, "");
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetResourceMessage(_headerProvider.GetAcceptLanguage(), i-1);
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetMessageCode();
                    return new ResultDTO<ProgramacaoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ProgramacaoResponse> AtualizarPorExternalId(string externalId, ProgramacaoAtualizarRequest request)
        {
            var old = _programacaoService.ListarFiltradosTracking(f => f.ExternalId == externalId).FirstOrDefault();

            if (old == null)
                return new ResultDTO<ProgramacaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<ProgramacaoResponse> AtualizarPorId(int idUsina, int contratoAno, int contratoNumero, int sequencia, ProgramacaoAtualizarRequest request)
        {
            var old = _programacaoService.ListarFiltradosTracking(f => f.UsinaCodigo == idUsina && f.ContratoAno == contratoAno && f.ContratoNumero == contratoNumero && f.Sequencia == sequencia).FirstOrDefault();

            if (old == null)
                return new ResultDTO<ProgramacaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<ProgramacaoResponse> Atualizar(Programacao programacao, ProgramacaoAtualizarRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var errors = new List<Error>();
                    var idioma = _headerProvider.GetAcceptLanguage();

                    var usinaEntregaValida = request.UsinaEntregaCodigo is null ? true : _programacaoService.ObterPorId<Usina>(request.UsinaEntregaCodigo) != null;
                    var enderecoMunicipioValido = request.EnderecoMunicipioCodigo is null ? true : _programacaoService.ObterPorId<Municipio>(request.EnderecoMunicipioCodigo) != null;
                    var pedraValida = request.PedraCodigo is null ? true : _programacaoService.ObterPorId<Pedra>(request.PedraCodigo) != null;
                    var usoValido = request.UsoCodigo is null ? true : _programacaoService.ObterPorId<Uso>(request.UsoCodigo) != null;
                    var equipamentoBombaValido = string.IsNullOrEmpty(request.EquipamentoBombaCodigo) ? true : _programacaoService.ObterPorId<Equipamento>(request.EquipamentoBombaCodigo) != null;
                    var resistenciaTipoValido = request.ResistenciaTipoCodigo is null ? true : _programacaoService.ObterPorId<ResistenciaTipo>(request.ResistenciaTipoCodigo) != null;
                    var slumpValido = request.SlumpCodigo is null ? true : _programacaoService.ObterPorId<Slump>(request.SlumpCodigo) != null;

                    if (!usinaEntregaValida)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393633.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393633.GetResourceMessage(idioma)));

                    if (!enderecoMunicipioValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393634.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393634.GetResourceMessage(idioma)));

                    if (!pedraValida)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393635.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393635.GetResourceMessage(idioma)));

                    if (!usoValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393636.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393636.GetResourceMessage(idioma)));

                    if (!equipamentoBombaValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393637.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393637.GetResourceMessage(idioma)));

                    if (!resistenciaTipoValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393638.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393638.GetResourceMessage(idioma)));

                    if (!slumpValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393639.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393639.GetResourceMessage(idioma)));

                    programacao = AutoMapper.Mapper.Map(request, programacao);

                    if (request.ObraNumero != null && request.ObraTracoSequencia != null)
                    {
                        var itemProposta = _programacaoService.ObterPorId<ObraTraco>(programacao.UsinaCodigo, programacao.ObraNumero, programacao.ObraTracoSequencia);
                        var itemPropostaValido = itemProposta != null;

                        var tracoValido = programacao.ObraTracoSequencia == 0 ? true
                            : itemProposta.Consumo == programacao.Consumo && itemProposta.Fck == programacao.Mpa &&
                            itemProposta.PedraCodigo == programacao.PedraCodigo && itemProposta.UsoCodigo == programacao.UsoCodigo && itemProposta.SlumpCodigo == programacao.SlumpCodigo
                            && itemProposta.ResistenciaTipoCodigo == programacao.ResistenciaTipoCodigo;

                        var validaQuantidadeProgramada = _programacaoService.ValidaQuantidadeProgramada();
                        var quantidadeValida = !validaQuantidadeProgramada ? true : programacao.VolumeTotal <= itemProposta.M3Quantidade;

                        if (!itemPropostaValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363131.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363131.GetResourceMessage(idioma)));

                        if (!tracoValido)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363132.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363132.GetResourceMessage(idioma)));

                        if (!quantidadeValida)
                            errors.Add(new Error(
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363133.GetMessageCode(),
                                EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363133.GetResourceMessage(idioma)));
                    }

                    var volumeLiberadoValido = programacao.NecessitaConfirmacao != "P" ? true : programacao.VolumeLiberado != 0;
                    if (!volumeLiberadoValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363134.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363134.GetResourceMessage(idioma)));

                    var horarioBombaValido = programacao.ObraBombaSequencia == 0 ? true : !string.IsNullOrEmpty(programacao.HorarioBomba) & this.ValidaHorario1MenorQueHorario2(programacao.HorarioBomba, programacao.Horario);
                    if (!horarioBombaValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363135.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363135.GetResourceMessage(idioma)));

                    var distanciaTubulacaoValido = programacao.ObraBombaSequencia == 0 ? true : programacao.DistanciaTubulacao != 0;
                    if (!distanciaTubulacaoValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363136.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363136.GetResourceMessage(idioma)));

                    var codigoBombaValido = programacao.ObraBombaSequencia == 0 ? true : !string.IsNullOrEmpty(programacao.EquipamentoBombaCodigo);
                    if (!codigoBombaValido)
                        errors.Add(new Error(
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363137.GetMessageCode(),
                            EResourcesProgramacao.PROGRAMACAO_ERROR_TCON39363137.GetResourceMessage(idioma)));

                    if (errors.Count > 0)
                        return new ResultDTO<ProgramacaoResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

                    ProgramacaoLog programacaoLog = null;
                    if (request.Status != null && request.Status != programacao.Status)
                    {
                        programacaoLog = new ProgramacaoLog()
                        {
                            UsinaCodigo = programacao.UsinaCodigo,
                            ObraCodigo = programacao.ObraNumero,
                            ProgramacaoSequencia = programacao.Sequencia,
                            PropostaAno = programacao.PropostaAno,
                            PropostaNumero = programacao.PropostaNumero,
                            ContratoAno = programacao.ContratoAno,
                            ContratoNumero = programacao.ContratoNumero,
                            DataHora = DateTime.Now,
                            Horario = "",
                            Usuario = "",
                            Evento = "Inserção Prog",
                            Complemento = "AGUARDANDO CONFIRMAÇÃO",
                            Descricao = "Inserção via API",
                            Sequencia = 1
                        };
                    }

                    programacao.IdAtual = StringHelper.GetIDD("APICRM");

                    if (programacaoLog != null) _programacaoService.Adicionar(programacaoLog);

                    Commit();

                    scope.Complete();
                    
                    var result = AutoMapper.Mapper.Map(programacao, new ProgramacaoResponse());
                    var contrato = _contratoService.ObterPorId(programacao.UsinaCodigo, programacao.ContratoAno, programacao.ContratoNumero);
                    if (contrato != null)
                        result.NumeroContratoAnterior = contrato.NumeroContratoAnterior;

                    return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Success, "Successfully updated", result);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<List<ProgramacaoResponse>> ObterPorUsinaEPeriodo(int usina, DateTime dataInicio, DateTime? dataFim)
        {
            try
            {
                var programacoes = dataFim is null
                    ? _programacaoService.ListarFiltrados(x => x.UsinaCodigo == usina && x.DataConcretagem >= dataInicio)
                    : _programacaoService.ListarFiltrados(x => x.UsinaCodigo == usina && x.DataConcretagem >= dataInicio && x.DataConcretagem <= dataFim);
                var result = AutoMapper.Mapper.Map(programacoes, new List<ProgramacaoResponse>());

                foreach (var programacao in result)
                {
                    var resistenciaTipo = _programacaoService.ObterPorId<ResistenciaTipo>(programacao.ResistenciaTipoCodigo);

                    var codigoMercadoria = Mercadoria.GerarCodigoMercadoriaTraco(programacao.UsoCodigo, programacao.PedraCodigo, programacao.SlumpCodigo, resistenciaTipo, programacao.Mpa, programacao.Consumo);
                    var contrato = _contratoService.ObterPorId(programacao.UsinaCodigo, programacao.ContratoAno, programacao.ContratoNumero);
                    if (contrato != null)
                        programacao.NumeroContratoAnterior = contrato.NumeroContratoAnterior;

                    var mercadoria = _programacaoService.ObterPorId<Mercadoria>(codigoMercadoria);


                    // Essa validação adiciona ,0 a primeira posição do código da mercadoria, isso resolve um problema de mascara do Tech
                    if(mercadoria is null)
                    {
                        var codigoMercadoriaArray = codigoMercadoria.Split('/');

                        if (!codigoMercadoriaArray[0].Contains(",") && codigoMercadoriaArray.Length > 1)
                        {
                            codigoMercadoria = $"{codigoMercadoriaArray[0]},0{codigoMercadoria.Substring(codigoMercadoriaArray[0].Length)}";
                            mercadoria = _programacaoService.ObterPorId<Mercadoria>(codigoMercadoria);
                        }
                    }

                    if(mercadoria != null)
                        programacao.NumeracaoProduto = mercadoria.NumeracaoProduto;

                    var programacaoBanco = _programacaoService.ListarFiltrados(t => t.UsinaCodigo == programacao.UsinaCodigo && t.ContratoNumero == programacao.ContratoNumero && t.ContratoAno == programacao.ContratoAno && t.Sequencia == programacao.Sequencia).FirstOrDefault();
                    if (programacaoBanco.CanceladoPor == ECanceladoPor.Usina)
                        programacao.CanceladoPorString = "Concrete Batching Plant";
                    else if (programacaoBanco.CanceladoPor == ECanceladoPor.Cliente)
                        programacao.CanceladoPorString = "Intervener";
                }

                return new ResultDTO<List<ProgramacaoResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<ProgramacaoResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<ProgramacaoResponse> ObterPorId(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            try
            {
                var programacaoBanco = _programacaoService.ListarFiltrados(f => f.UsinaCodigo == idUsina && f.ContratoAno == contratoAno && f.ContratoNumero == contratoNumero && f.Sequencia == sequencia).FirstOrDefault();
                var programacao = AutoMapper.Mapper.Map(programacaoBanco, new ProgramacaoResponse());

                if (programacao == null)
                    return new ResultDTO<ProgramacaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetMessageCode());

                var contrato = _contratoService.ObterPorId(programacao.UsinaCodigo, programacao.ContratoAno, programacao.ContratoNumero);
                if (contrato != null)
                    programacao.NumeroContratoAnterior = contrato.NumeroContratoAnterior;

                var resistenciaTipo = _programacaoService.ObterPorId<ResistenciaTipo>(programacao.ResistenciaTipoCodigo);

                var codigoMercadoria = Mercadoria.GerarCodigoMercadoriaTraco(programacao.UsoCodigo, programacao.PedraCodigo, programacao.SlumpCodigo, resistenciaTipo, programacao.Mpa, programacao.Consumo);

                programacao.NumeracaoProduto = _programacaoService.ObterPorId<Mercadoria>(codigoMercadoria).NumeracaoProduto;

                if (programacaoBanco.CanceladoPor == ECanceladoPor.Usina)
                    programacao.CanceladoPorString = "Concrete Batching Plant";
                else if (programacaoBanco.CanceladoPor == ECanceladoPor.Cliente)
                    programacao.CanceladoPorString = "Intervener";

                return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Success, "", programacao);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<ProgramacaoResponse> ObterPorExternalId(string externalId)
        {
            try
            {
                var programacao = AutoMapper.Mapper.Map(_programacaoService.ListarFiltrados(f => f.ExternalId == externalId).FirstOrDefault(), new ProgramacaoResponse());

                if (programacao == null)
                    return new ResultDTO<ProgramacaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesProgramacao.PROGRAMACAO_ERROR_TCON393631.GetMessageCode());

                var resistenciaTipo = _programacaoService.ObterPorId<ResistenciaTipo>(programacao.ResistenciaTipoCodigo);

                var codigoMercadoria = Mercadoria.GerarCodigoMercadoriaTraco(programacao.UsoCodigo, programacao.PedraCodigo, programacao.SlumpCodigo, resistenciaTipo, programacao.Mpa, programacao.Consumo);

                var contrato = _contratoService.ObterPorId(programacao.UsinaCodigo, programacao.ContratoAno, programacao.ContratoNumero);
                if (contrato != null)
                    programacao.NumeroContratoAnterior = contrato.NumeroContratoAnterior;

                programacao.NumeracaoProduto = _programacaoService.ObterPorId<Mercadoria>(codigoMercadoria).NumeracaoProduto;

                var programacaoBanco = _programacaoService.ListarFiltrados(t => t.UsinaCodigo == programacao.UsinaCodigo && t.ContratoNumero == programacao.ContratoNumero && t.ContratoAno == programacao.ContratoAno && t.Sequencia == programacao.Sequencia).FirstOrDefault();
                if (programacaoBanco.CanceladoPor == ECanceladoPor.Usina)
                    programacao.CanceladoPorString = "Concrete Batching Plant";
                else if (programacaoBanco.CanceladoPor == ECanceladoPor.Cliente)
                    programacao.CanceladoPorString = "Intervener";

                return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Success, "", programacao);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<ProgramacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        private bool ValidaHorario1MenorQueHorario2(string horario1, string horario2)
        {
            var validaHorario = new TimeLessThanAttribute("CampoB");

            var contextoValidacao = new ValidationContext(new { CampoA = horario1, CampoB = horario2 });
            var resultadoValidacao = validaHorario.GetValidationResult(horario1, contextoValidacao);

            return resultadoValidacao == ValidationResult.Success;
        }
    }
}

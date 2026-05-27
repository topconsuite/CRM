using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso;
using TopSys.TopConWeb.Application.DTOS.Response.LiberacaoAcesso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class LiberacaoAcessoApplicationService : ApplicationServiceBase<GrupoAcesso>, ILiberacaoAcessoApplicationService
    {
        private readonly ILiberacaoAcessoService _liberacaoAcessoService;
        private readonly IUsuarioService _usuarioService;
        private object grupoAcessoRequest;

        public LiberacaoAcessoApplicationService(ILiberacaoAcessoService liberacaoAcessoService, IUsuarioService usuarioService, IUnitOfWork unityOfWork)
            : base(liberacaoAcessoService, unityOfWork)
        {
            _liberacaoAcessoService = liberacaoAcessoService;
            _usuarioService = usuarioService;
        }

        public void Adicionar(GrupoAcessoInclusaoRequest grupoAcessoRequest, string userRequest)
        {
            var grupoAcesso = AutoMapper.Mapper.Map(grupoAcessoRequest, new GrupoAcesso());

            var grupoAcessoIgual = _liberacaoAcessoService.ListarFiltradosTracking<GrupoAcesso>(x => x.Descricao == grupoAcesso.Descricao).FirstOrDefault();

            if (!(grupoAcessoIgual is null))
            {
                AssertionConcern.Notify("Adicionar", "A descrição fornecida já está vinculada a um Grupo existente!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                grupoAcesso.CriadoEm = DateTime.Now;
                _liberacaoAcessoService.Adicionar(grupoAcesso);

                _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                {
                    TipoLiberacao = TipoLiberacao.APROVACOES,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    UsuarioModificado = "",
                    UsinaGrupo = grupoAcessoRequest.Usina,
                    DescricaoGrupo = grupoAcessoRequest.Descricao,
                    Evento = "INSERÇÃO NOVO GRUPO",
                    Complemento = $"Central: {grupoAcessoRequest.Usina} Grupo: {grupoAcessoRequest.Descricao}" 
                });

                foreach (var liberacaoAcesso in grupoAcesso.LiberacoesAcessos)
                {
                    liberacaoAcesso.Grupo = grupoAcesso.Codigo;
                    liberacaoAcesso.TipoLiberacao = TipoLiberacao.APROVACOES;
                    liberacaoAcesso.CriadoEm = DateTime.Now;
                    _liberacaoAcessoService.Adicionar(liberacaoAcesso);

                    _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                    {
                        TipoLiberacao = TipoLiberacao.APROVACOES,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        UsuarioModificado = "",
                        UsinaGrupo = grupoAcessoRequest.Usina,
                        DescricaoGrupo = grupoAcessoRequest.Descricao,
                        Evento = $"INSERÇÃO LIBERAÇÃO {liberacaoAcesso.DiaSemana} {liberacaoAcesso.Turno}",
                        Complemento = $"Turno: {liberacaoAcesso.Turno} Horário: {liberacaoAcesso.HoraEntrada}-{liberacaoAcesso.HoraSaida} Bloquear: {(liberacaoAcesso.Bloquear ? "Sim" : "Não")}"
                    });
                }

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(GrupoAcessoAlteracaoRequest grupoAcessoRequest, bool alteraUsuarios, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var grupoAcessoAtual = _liberacaoAcessoService.ObterPorId(grupoAcessoRequest.Codigo);

                if (grupoAcessoAtual is null)
                {
                    AssertionConcern.Notify("Atualizar", "Grupo não encontrado!");
                    return;
                }

                var grupoAcessoIgual = _liberacaoAcessoService.ListarFiltradosTracking<GrupoAcesso>(x => x.Codigo != grupoAcessoRequest.Codigo && x.Descricao == grupoAcessoRequest.Descricao).FirstOrDefault();
                if (!(grupoAcessoIgual is null))
                {
                    AssertionConcern.Notify("Atualizar", "A descrição fornecida já está vinculada a um Grupo existente!");
                    return;
                }

                if (grupoAcessoAtual.Descricao != grupoAcessoRequest.Descricao || grupoAcessoAtual.Usina != grupoAcessoRequest.Usina)
                {
                    _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                    {
                        TipoLiberacao = TipoLiberacao.APROVACOES,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        UsuarioModificado = "",
                        UsinaGrupo = grupoAcessoRequest.Usina,
                        DescricaoGrupo = grupoAcessoRequest.Descricao,
                        Evento = $"ALTERAÇÃO GRUPO",
                        Complemento = $"De: Central: {grupoAcessoAtual.Usina} Grupo: {grupoAcessoAtual.Descricao} Para: {grupoAcessoRequest.Usina} Grupo: {grupoAcessoRequest.Descricao}"
                    });

                    AutoMapper.Mapper.Map(grupoAcessoRequest, grupoAcessoAtual);
                }

                var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Grupo == grupoAcessoRequest.Codigo && x.Usuario == null && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

                var usuariosLiberacoes = _liberacaoAcessoService.ListarUsuariosPorGrupoAcesso(grupoAcessoRequest.Codigo).ToList();

                foreach (var liberacaoAcesso in liberacoesAcessos)
                {
                    var request = grupoAcessoRequest.LiberacoesAcessos.Where(x => x.Codigo == liberacaoAcesso.Codigo).FirstOrDefault();

                    if (request == null) continue;

                    if (liberacaoAcesso.HoraEntrada == request.HoraEntrada && liberacaoAcesso.HoraSaida == request.HoraSaida && liberacaoAcesso.Bloquear == request.Bloquear) continue;

                    _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                    {
                        TipoLiberacao = TipoLiberacao.APROVACOES,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        UsuarioModificado = "",
                        UsinaGrupo = grupoAcessoRequest.Usina,
                        DescricaoGrupo = grupoAcessoRequest.Descricao,
                        Evento = $"ALTERAÇÃO {liberacaoAcesso.DiaSemana} {liberacaoAcesso.Turno}",
                        Complemento = $"De: {liberacaoAcesso.Turno} Turno {liberacaoAcesso.HoraEntrada}-{liberacaoAcesso.HoraSaida} Bloquear: {(liberacaoAcesso.Bloquear ? "Sim" : "Não")} Para: {request.Turno} Turno {request.HoraEntrada}-{request.HoraSaida} Bloquear: {(request.Bloquear ? "Sim" : "Não")}"
                    });

                    AutoMapper.Mapper.Map(request, liberacaoAcesso);

                    if (!alteraUsuarios) continue;
                            
                    foreach (var usuarioLiberacao in usuariosLiberacoes)
                    {
                        if(usuarioLiberacao.DiaSemana == liberacaoAcesso.DiaSemana && usuarioLiberacao.Turno == liberacaoAcesso.Turno)
                        {
                            _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                            {
                                TipoLiberacao = TipoLiberacao.APROVACOES,
                                DataHoraEvento = DateTime.Now,
                                Usuario = userRequest,
                                UsuarioModificado = usuarioLiberacao.Usuario,
                                UsinaGrupo = 0,
                                DescricaoGrupo = "",
                                Evento = $"ALTERAÇÃO {liberacaoAcesso.DiaSemana} {liberacaoAcesso.Turno} - {usuarioLiberacao.Usuario}",
                                Complemento = $"De: {usuarioLiberacao.Turno} Turno {usuarioLiberacao.HoraEntrada}-{usuarioLiberacao.HoraSaida} Bloquear: {(usuarioLiberacao.Bloquear ? "Sim" : "Não")} Para: {request.Turno} Turno {request.HoraEntrada}-{request.HoraSaida} Bloquear: {(request.Bloquear ? "Sim" : "Não")}"
                            });

                            usuarioLiberacao.HoraEntrada = usuarioLiberacao.HoraEntrada != request.HoraEntrada ? usuarioLiberacao.HoraEntrada = request.HoraEntrada : usuarioLiberacao.HoraEntrada = usuarioLiberacao.HoraEntrada;

                            usuarioLiberacao.HoraSaida = usuarioLiberacao.HoraSaida != request.HoraSaida ? usuarioLiberacao.HoraSaida = request.HoraSaida : usuarioLiberacao.HoraSaida = usuarioLiberacao.HoraSaida;

                            usuarioLiberacao.Bloquear = usuarioLiberacao.Bloquear != request.Bloquear ? usuarioLiberacao.Bloquear = request.Bloquear : usuarioLiberacao.Bloquear = usuarioLiberacao.Bloquear;

                            _liberacaoAcessoService.AtualizarLiberacaoAcessoUsuario(usuarioLiberacao);
                        }
                    }
                            
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int idGrupo, string userRequest)
        {
            var grupoAcesso = _liberacaoAcessoService.ObterPorId<GrupoAcesso>(idGrupo);

            if (grupoAcesso is null)
            {
                AssertionConcern.Notify("Deletar", "Grupo não encontrado!");
                return;
            }

            var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Grupo == grupoAcesso.Codigo && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

            var usuariosVinculados = liberacoesAcessos
                .Where(x => liberacoesAcessos.Any(y => y.Usuario != null))
                .ToList();
            if (usuariosVinculados.Count > 0)
            {
                AssertionConcern.Notify("Deletar", "Grupo contêm usuários vinculados e não poderá ser excluído");
                return;
            }

            _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
            {
                TipoLiberacao = TipoLiberacao.APROVACOES,
                DataHoraEvento = DateTime.Now,
                Usuario = userRequest,
                UsuarioModificado = "",
                UsinaGrupo = grupoAcesso.Usina,
                DescricaoGrupo = grupoAcesso.Descricao,
                Evento = $"EXCLUSÃO DE GRUPO",
                Complemento = $"Grupo: {grupoAcesso.Descricao} Usina: {grupoAcesso.Usina}"
            });

            foreach (var liberacaoAcesso in liberacoesAcessos)
            {
                _liberacaoAcessoService.Remover(liberacaoAcesso);
            }

            _liberacaoAcessoService.Remover(grupoAcesso);

            Commit();
        }

        public GrupoAcessoResponse ObterGrupoPorCodigo(int grupoAcessoCodigo)
        {
            return AutoMapper.Mapper.Map(_liberacaoAcessoService.ObterPorId(grupoAcessoCodigo), new GrupoAcessoResponse());
        }

        public PagedList<GrupoAcessoResponse> Listar(int pagina, int porPagina, Expression<Func<GrupoAcesso, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_liberacaoAcessoService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<GrupoAcessoResponse>());
        }

        // ------------------------- Usuário -----------------------------------------------------------------------------------------

        public IEnumerable<LiberacaoAcessoResponse> ListarUsuariosPorGrupoAcesso(int grupoAcessoCodigo)
        {
            var result = _liberacaoAcessoService.ListarUsuariosPorGrupoAcesso(grupoAcessoCodigo).ToList();

            return AutoMapper.Mapper.Map(result.ToList(), new List<LiberacaoAcessoResponse>());
        }

        public IEnumerable<PeriodoAusenciaUsuarioResponse> ListarPeriodosAusenciaPorUsuario(string usuario)
        {
            var result = _liberacaoAcessoService.ListarPeriodosAusenciaPorUsuario(usuario).ToList();

            return AutoMapper.Mapper.Map(result.ToList(), new List<PeriodoAusenciaUsuarioResponse>());
        }

        public IEnumerable<LiberacaoAcessoUsuarioResponse> ListarUsuarios()
        {
            return AutoMapper.Mapper.Map(
                _usuarioService.ObterNomeUsuariosVerificados().ToList(),
                new List<LiberacaoAcessoUsuarioResponse>());
        }

        public IEnumerable<LiberacaoAcessoResponse> AdicionarUsuario(IEnumerable<LiberacaoAcessoInclusaoRequest> liberacaoAcessoUsuario, string userRequest)
        {

            var listaLiberacoesAcessosUsuario = new List<LiberacaoAcessoResponse>();

            var codigoUsuario = liberacaoAcessoUsuario.FirstOrDefault()?.Usuario;
            var codigoGrupo = liberacaoAcessoUsuario.FirstOrDefault()?.Grupo;

            var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Usuario == codigoUsuario && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

            if (liberacoesAcessos.Count() > 0)
            {
                AssertionConcern.Notify("Adicionar","Usuário já está vinculado à um grupo");
                return null;
            }

            var liberacoesAcessosGrupo = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Usuario == null && x.Grupo == codigoGrupo && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

            foreach (var liberacao in liberacaoAcessoUsuario)
            {
                liberacao.TipoLiberacao = TipoLiberacao.APROVACOES;
                liberacao.CriadoEm = DateTime.Now;

                var liberacaoGrupo = liberacoesAcessosGrupo.FirstOrDefault(g => g.DiaSemana == liberacao.DiaSemana && g.Turno == liberacao.Turno);
                if (liberacaoGrupo != null)
                {
                    liberacao.HoraEntrada = liberacaoGrupo.HoraEntrada ;
                    liberacao.HoraSaida = liberacaoGrupo.HoraSaida;
                    liberacao.Bloquear = liberacaoGrupo.Bloquear;
                }

                _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                {
                    TipoLiberacao = TipoLiberacao.APROVACOES,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    UsuarioModificado = liberacao.Usuario,
                    UsinaGrupo = 0,
                    DescricaoGrupo = "",
                    Evento = $"INSERÇÃO LIBERAÇÃO {liberacao.DiaSemana} {liberacao.Turno} - {userRequest}",
                    Complemento = $"Turno: {liberacao.Turno} Horário: {liberacao.HoraEntrada}-{liberacao.HoraSaida} Bloquear: {(liberacao.Bloquear ? "Sim" : "Não")}"
                });

                var usuario = AutoMapper.Mapper.Map<LiberacaoAcesso>(liberacao);

                _liberacaoAcessoService.Adicionar(usuario);

                var usuarioResponse = AutoMapper.Mapper.Map<LiberacaoAcessoResponse>(usuario);

                listaLiberacoesAcessosUsuario.Add(usuarioResponse);
            }

            Commit();

            return listaLiberacoesAcessosUsuario;
        }

        public void AtualizarPeriodoAusenciaUsuario(IEnumerable<PeriodoAusenciaUsuarioAlteracaoRequest> periodosAusenciasUsuario, string userRequest)
        {
            var tiposAusencia = new Dictionary<string, string> {{ "AFASTAMENTO", "AUSENCIA" },{ "AUSENCIA", "AFASTAMENTO" }};

            using (var scope = new TransactionScope())
            {
                foreach (var periodoAusenciaUsuario in periodosAusenciasUsuario)
                {
                    if (!periodoAusenciaUsuario.Checked)
                    {
                        var periodoAusenciaAtual = _liberacaoAcessoService.ObterPorId<PeriodoAusenciaUsuario>(periodoAusenciaUsuario.Codigo);
                        _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                        {
                            TipoLiberacao = TipoLiberacao.APROVACOES,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            UsuarioModificado = periodoAusenciaUsuario.Usuario,
                            UsinaGrupo = 0,
                            DescricaoGrupo = "",
                            Evento = $"REMOÇÃO {periodoAusenciaUsuario.TipoAusencia}",
                            Complemento = $"{periodoAusenciaUsuario.TipoAusencia} {periodoAusenciaUsuario.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaUsuario.FimPeriodo.ToString("dd/MM/yyyy")}"
                        });
                        _liberacaoAcessoService.Remover(periodoAusenciaAtual);
                        continue;
                    }

                    if (tiposAusencia.ContainsKey(periodoAusenciaUsuario.TipoAusencia))
                    {
                        var tipoParaRemover = tiposAusencia[periodoAusenciaUsuario.TipoAusencia];
                        var periodoAusenciaRemover = _liberacaoAcessoService.ListarFiltradosTracking<PeriodoAusenciaUsuario>(x => x.Usuario == periodoAusenciaUsuario.Usuario && x.TipoAusencia == tipoParaRemover).FirstOrDefault();

                        if (periodoAusenciaRemover != null)
                        {
                            _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                            {
                                TipoLiberacao = TipoLiberacao.APROVACOES,
                                DataHoraEvento = DateTime.Now,
                                Usuario = userRequest,
                                UsuarioModificado = periodoAusenciaRemover.Usuario,
                                UsinaGrupo = 0,
                                DescricaoGrupo = "",
                                Evento = $"REMOÇÃO {periodoAusenciaRemover.TipoAusencia}",
                                Complemento = $"{periodoAusenciaRemover.TipoAusencia} {periodoAusenciaRemover.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaRemover.FimPeriodo.ToString("dd/MM/yyyy")}"
                            });
                            _liberacaoAcessoService.Remover(periodoAusenciaRemover);
                        }
                    };

                    if (periodoAusenciaUsuario.Codigo == 0) {

                        _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                        {
                            TipoLiberacao = TipoLiberacao.APROVACOES,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            UsuarioModificado = periodoAusenciaUsuario.Usuario,
                            UsinaGrupo = 0,
                            DescricaoGrupo = "",
                            Evento = $"INSERÇÃO {periodoAusenciaUsuario.TipoAusencia}",
                            Complemento = $"{periodoAusenciaUsuario.TipoAusencia} {periodoAusenciaUsuario.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaUsuario.FimPeriodo.ToString("dd/MM/yyyy")}"
                        });

                        var periodoAusencia = AutoMapper.Mapper.Map(periodoAusenciaUsuario, new PeriodoAusenciaUsuario());
                        periodoAusencia.CriadoEm = DateTime.Now;
                        _liberacaoAcessoService.Adicionar(periodoAusencia);
                        continue;
                    }

                    var periodoAusenciaOld = _liberacaoAcessoService.ObterPorId<PeriodoAusenciaUsuario>(periodoAusenciaUsuario.Codigo);

                    var dataModificada = periodoAusenciaUsuario.InicioPeriodo != periodoAusenciaOld.InicioPeriodo ||
                                           periodoAusenciaUsuario.FimPeriodo != periodoAusenciaOld.FimPeriodo;

                    if (!dataModificada) continue;

                    if (periodoAusenciaUsuario.TipoAusencia == "FERIAS" && periodoAusenciaOld.FimPeriodo <= DateTime.Now)
                    {
                        _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                        {
                            TipoLiberacao = TipoLiberacao.APROVACOES,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            UsuarioModificado = periodoAusenciaUsuario.Usuario,
                            UsinaGrupo = 0,
                            DescricaoGrupo = "",
                            Evento = $"INSERÇÃO {periodoAusenciaUsuario.TipoAusencia}",
                            Complemento = $"{periodoAusenciaUsuario.TipoAusencia} {periodoAusenciaUsuario.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaUsuario.FimPeriodo.ToString("dd/MM/yyyy")}"
                        });
                        var periodoAusenciaNew = AutoMapper.Mapper.Map(periodoAusenciaUsuario, new PeriodoAusenciaUsuario());
                        periodoAusenciaNew.CriadoEm = DateTime.Now;
                        _liberacaoAcessoService.Adicionar(periodoAusenciaNew);
                        continue;
                    }
                    
                    _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                    {
                        TipoLiberacao = TipoLiberacao.APROVACOES,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        UsuarioModificado = periodoAusenciaUsuario.Usuario,
                        UsinaGrupo = 0,
                        DescricaoGrupo = "",
                        Evento = $"ALTERAÇÃO {periodoAusenciaUsuario.TipoAusencia}",
                        Complemento = $"De: {periodoAusenciaUsuario.TipoAusencia} {periodoAusenciaOld.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaOld.FimPeriodo.ToString("dd/MM/yyyy")} " +
                                        $"Para: {periodoAusenciaUsuario.TipoAusencia} {periodoAusenciaUsuario.InicioPeriodo.ToString("dd/MM/yyyy")}-{periodoAusenciaUsuario.FimPeriodo.ToString("dd/MM/yyyy")}"
                    });
                    AutoMapper.Mapper.Map(periodoAusenciaUsuario, periodoAusenciaOld);               
                }
                Commit();
                scope.Complete();
            }
        }

        public void AtualizarUsuario(IEnumerable<LiberacaoAcessoAlteracaoRequest> liberacoesAcessosRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var codigoGrupo = liberacoesAcessosRequest.FirstOrDefault()?.Grupo;
                if (codigoGrupo == null) return;

                var codigoUsuario = liberacoesAcessosRequest.FirstOrDefault()?.Usuario;
                if (codigoUsuario == null) return;

                var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Grupo == codigoGrupo && x.Usuario == codigoUsuario && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

                foreach (var liberacaoAcesso in liberacoesAcessos)
                {
                    var request = liberacoesAcessosRequest.Where(x => x.Codigo == liberacaoAcesso.Codigo).FirstOrDefault();
                    if (request == null) continue;

                    if (liberacaoAcesso.HoraEntrada == request.HoraEntrada && liberacaoAcesso.HoraSaida == request.HoraSaida && liberacaoAcesso.Bloquear == request.Bloquear) continue;

                    _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                    {
                        TipoLiberacao = TipoLiberacao.APROVACOES,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        UsuarioModificado = request.Usuario,
                        UsinaGrupo = 0,
                        DescricaoGrupo = "",
                        Evento = $"ALTERAÇÃO {liberacaoAcesso.DiaSemana} {liberacaoAcesso.Turno} - {userRequest}",
                        Complemento = $"De: {liberacaoAcesso.Turno} Turno {liberacaoAcesso.HoraEntrada}-{liberacaoAcesso.HoraSaida} Bloquear: {(liberacaoAcesso.Bloquear ? "Sim" : "Não")} Para: {request.Turno} Turno {request.HoraEntrada}-{request.HoraSaida} Bloquear: {(request.Bloquear ? "Sim" : "Não")}"
                    });

                    AutoMapper.Mapper.Map(request, liberacaoAcesso);
                }

                Commit();
                scope.Complete();
            }
        }

        public void RemoverUsuario(string codigoUsuario, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var usuario = "";
                var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Usuario == codigoUsuario && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

                foreach (var liberacaoAcesso in liberacoesAcessos)
                {
                    usuario = liberacaoAcesso.Usuario;
                    _liberacaoAcessoService.Remover(liberacaoAcesso);
                }

                var periodoAusencias = _liberacaoAcessoService.ListarFiltradosTracking<PeriodoAusenciaUsuario>(x => x.Usuario == usuario && x.TipoLiberacao == TipoLiberacao.APROVACOES).ToList();

                foreach (var periodoAusencia in periodoAusencias)
                {
                    _liberacaoAcessoService.Remover(periodoAusencia);
                }

                _liberacaoAcessoService.Adicionar(new LiberacaoAcessoLog
                {
                    TipoLiberacao = TipoLiberacao.APROVACOES,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    UsuarioModificado = usuario,
                    UsinaGrupo = 0,
                    DescricaoGrupo = "",
                    Evento = $"EXCLUSÃO DE USUARIO",
                    Complemento = $"Usuário: {usuario}"
                });

                Commit();
                scope.Complete();
            }
        }

        public bool ObterLiberacaoAcessoUsuario(string usuario)
        {
            var diaSemanaAtual = ObterDiaSemanaAtual();
            var horarioAtual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

            var possuiRestricaoAcesso = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Usuario == usuario && x.TipoLiberacao == TipoLiberacao.APROVACOES).FirstOrDefault();
            if(possuiRestricaoAcesso is null)
                return true;

            var liberacoesAcessos = _liberacaoAcessoService.ListarFiltradosTracking<LiberacaoAcesso>(x => x.Usuario == usuario && x.TipoLiberacao == TipoLiberacao.APROVACOES 
                                    && x.DiaSemana == diaSemanaAtual && (!x.Bloquear && ((horarioAtual >= x.HoraEntrada && horarioAtual <= x.HoraSaida) || (x.HoraEntrada == TimeSpan.Zero && x.HoraSaida == TimeSpan.Zero)))).ToList();

            if (liberacoesAcessos.Count() == 0 || (liberacoesAcessos.Count == 1 && liberacoesAcessos.Any(x => x.HoraEntrada == TimeSpan.Zero && x.HoraSaida == TimeSpan.Zero)))
                return false;

            var ausencias = _liberacaoAcessoService.ListarPeriodosAusenciaPorUsuario(usuario).ToList();

            foreach (var ausencia in ausencias)
            {
                if (DateTime.Now.Date >= ausencia.InicioPeriodo && DateTime.Now.Date <= ausencia.FimPeriodo)
                {
                    return false;
                }
            }

            return true;
        }

        private string ObterDiaSemanaAtual()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "SEGUNDA";
                case DayOfWeek.Tuesday:
                    return "TERCA";
                case DayOfWeek.Wednesday:
                    return "QUARTA";
                case DayOfWeek.Thursday:
                    return "QUINTA";
                case DayOfWeek.Friday:
                    return "SEXTA";
                case DayOfWeek.Saturday:
                    return "SABADO";
                case DayOfWeek.Sunday:
                    return "DOMINGO";
                default:
                    return string.Empty;
            }
        }
    }
}

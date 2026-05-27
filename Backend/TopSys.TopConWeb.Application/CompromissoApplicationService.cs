using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Compromisso;
using TopSys.TopConWeb.Application.DTOS.Response.Compromisso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class CompromissoApplicationService : ApplicationServiceBase<Compromisso>, ICompromissoApplicationService
    {
        private readonly ICompromissoService _compromissoService;
        public CompromissoApplicationService(ICompromissoService compromissoService, IUnitOfWork unityOfWork) : base(compromissoService, unityOfWork)
        {
            _compromissoService = compromissoService;
        }

        public void AdicionarGrupo(CompromissoInclusaoRequest[] compromissoInclusaoRequest, string userRequest)
        {

            var compromissos = AutoMapper.Mapper.Map(compromissoInclusaoRequest.ToList(), new List<Compromisso>());
            var idAgrupamento = Guid.NewGuid().ToString();

            using (var scope = new TransactionScope())
            {
                foreach (var compromisso in compromissos)
                {
                    compromisso.IdAgrupamento = idAgrupamento;

                    _compromissoService.Adicionar(compromisso);

                    _compromissoService.Adicionar(new CompromissoLog
                    {
                        CodigoCompromisso = compromisso.Codigo,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        Descricao = compromisso.Descricao,
                        Usina = compromisso.UsinaCodigo,
                        AnoVisita = compromisso.AnoVisita,
                        NumeroVisita = compromisso.NumeroVisita,
                        AnoLead = compromisso.AnoLead,
                        NumeroLead = compromisso.NumeroLead,
                        AnoOportunidade = compromisso.AnoOportunidade,
                        NumeroOportunidade = compromisso.NumeroOportunidade,
                        Evento = "INSERÇÃO NOVO COMPROMISSO EM GRUPO",
                        Complemento = $"Data Inicio: {compromisso.DataInicio:dd/MM/yyyy} Data Fim: {compromisso.DataFim:dd/MM/yyyy} " +
                    $"Grupo: {idAgrupamento} {(compromisso.DiaInteiro ? "Hora: Dia Inteiro" : $"Hora Inicio: {compromisso.HoraFim.Value.ToString(@"hh\:mm")} Hora Fim: {compromisso.HoraFim.Value.ToString(@"hh\:mm")}")}"
                    });

                }

                Commit();
                scope.Complete();

            }

        }

        public void Adicionar(CompromissoInclusaoRequest compromissoInclusaoRequest, string userRequest)
        {
            var compromisso = AutoMapper.Mapper.Map(compromissoInclusaoRequest, new Compromisso());

            if (!compromisso.AdicionarCompromissoScopeIsValid()) return;

            using (var scope = new TransactionScope())
            {
                _compromissoService.Adicionar(compromisso);

                _compromissoService.Adicionar(new CompromissoLog
                {
                    CodigoCompromisso = compromisso.Codigo,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    Descricao = compromisso.Descricao,
                    Usina = compromisso.UsinaCodigo,
                    AnoVisita = compromisso.AnoVisita,
                    NumeroVisita = compromisso.NumeroVisita,
                    AnoLead = compromisso.AnoLead,
                    NumeroLead = compromisso.NumeroLead,
                    AnoOportunidade = compromisso.AnoOportunidade,
                    NumeroOportunidade = compromisso.NumeroOportunidade,
                    Evento = "INSERÇÃO NOVO COMPROMISSO",
                    Complemento = $"Data Inicio: {compromisso.DataInicio:dd/MM/yyyy} Data Fim: {compromisso.DataFim:dd/MM/yyyy} " +
                    $"{(compromisso.DiaInteiro ? "Hora: Dia Inteiro" : $"Hora Inicio: {compromisso.HoraFim.Value.ToString(@"hh\:mm")} Hora Fim: {compromisso.HoraFim.Value.ToString(@"hh\:mm")}")}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(CompromissoAlteracaoRequest compromissoAlterecaoRequest, string userRequest)
        {
            var compromissoOld = _compromissoService.ObterPorId(compromissoAlterecaoRequest.Codigo);
            var compromissoAgrupamento = new List<Compromisso>();
            compromissoAgrupamento.Add(compromissoOld);

            if (!string.IsNullOrEmpty(compromissoOld.IdAgrupamento)) // Buscando outros compromissos do grupo
                compromissoAgrupamento.AddRange(_compromissoService.ListarFiltradosTracking(x => x.IdAgrupamento == compromissoOld.IdAgrupamento && x.Codigo != compromissoOld.Codigo).ToList());
            
            if (compromissoOld is null)
            {
                AssertionConcern.Notify("Atualizar", "Compromisso não encontrado!");
                return;
            }



            using (var scope = new TransactionScope())
            {
               
                foreach(var compromissoAtual in compromissoAgrupamento)
                {

                    if (compromissoAlterecaoRequest.Descricao != compromissoAtual.Descricao)
                    {
                        _compromissoService.Adicionar(new CompromissoLog
                        {
                            CodigoCompromisso = compromissoAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = compromissoAlterecaoRequest.Descricao,
                            Usina = compromissoAlterecaoRequest.UsinaCodigo,
                            AnoVisita = compromissoAlterecaoRequest.AnoVisita,
                            NumeroVisita = compromissoAlterecaoRequest.NumeroVisita,
                            AnoLead = compromissoAlterecaoRequest.AnoLead,
                            NumeroLead = compromissoAlterecaoRequest.NumeroLead,
                            AnoOportunidade = compromissoAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = compromissoAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO DESCRICAO COMPROMISSO",
                            Complemento = $"DE: {compromissoAtual.Descricao} - PARA: {compromissoAlterecaoRequest.Descricao}"
                        });
                    }
                    if ((compromissoAlterecaoRequest.DataInicio != compromissoAtual.DataInicio) || (compromissoAlterecaoRequest.DataFim != compromissoAtual.DataFim)
                        || (compromissoAlterecaoRequest.HoraInicio != compromissoAtual.HoraInicio) || (compromissoAlterecaoRequest.HoraFim != compromissoAtual.HoraFim))
                    {
                        _compromissoService.Adicionar(new CompromissoLog
                        {
                            CodigoCompromisso = compromissoAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = compromissoAlterecaoRequest.Descricao,
                            Usina = compromissoAlterecaoRequest.UsinaCodigo,
                            AnoVisita = compromissoAlterecaoRequest.AnoVisita,
                            NumeroVisita = compromissoAlterecaoRequest.NumeroVisita,
                            AnoLead = compromissoAlterecaoRequest.AnoLead,
                            NumeroLead = compromissoAlterecaoRequest.NumeroLead,
                            AnoOportunidade = compromissoAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = compromissoAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO DATA E HORARIO COMPROMISSO",
                            Complemento = $"DE: Data Inicio: {compromissoAtual.DataInicio:dd/MM/yyyy} Data Fim: {compromissoAtual.DataFim:dd/MM/yyyy} " +
                            $"{(compromissoAtual.DiaInteiro ? "Hora: Dia Inteiro" : $"Hora Inicio: {compromissoAtual.HoraFim.Value.ToString(@"hh\:mm")} Hora Fim: {compromissoAtual.HoraFim.Value.ToString(@"hh\:mm")}")} " +
                            $"- PARA: Data Inicio: {compromissoAlterecaoRequest.DataInicio:dd/MM/yyyy} Data Fim: {compromissoAlterecaoRequest.DataFim:dd/MM/yyyy} " +
                            $"{(compromissoAlterecaoRequest.DiaInteiro ? "Hora: Dia Inteiro" : $"Hora Inicio: {compromissoAlterecaoRequest.HoraFim.Value.ToString(@"hh\:mm")} Hora Fim: {compromissoAlterecaoRequest.HoraFim.Value.ToString(@"hh\:mm")}")}"
                        });
                    }
                    if (compromissoAlterecaoRequest.Providencia != compromissoAtual.Providencia)
                    {
                        _compromissoService.Adicionar(new CompromissoLog
                        {
                            CodigoCompromisso = compromissoAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = compromissoAlterecaoRequest.Descricao,
                            Usina = compromissoAlterecaoRequest.UsinaCodigo,
                            AnoVisita = compromissoAlterecaoRequest.AnoVisita,
                            NumeroVisita = compromissoAlterecaoRequest.NumeroVisita,
                            AnoLead = compromissoAlterecaoRequest.AnoLead,
                            NumeroLead = compromissoAlterecaoRequest.NumeroLead,
                            AnoOportunidade = compromissoAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = compromissoAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO PROVIDENCIA COMPROMISSO",
                            Complemento = $"DE: {compromissoAtual.Providencia} - PARA: {compromissoAlterecaoRequest.Providencia}"
                        });
                    }
                    if (compromissoAlterecaoRequest.Conclusao != compromissoAtual.Conclusao)
                    {
                        _compromissoService.Adicionar(new CompromissoLog
                        {
                            CodigoCompromisso = compromissoAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = compromissoAlterecaoRequest.Descricao,
                            Usina = compromissoAlterecaoRequest.UsinaCodigo,
                            AnoVisita = compromissoAlterecaoRequest.AnoVisita,
                            NumeroVisita = compromissoAlterecaoRequest.NumeroVisita,
                            AnoLead = compromissoAlterecaoRequest.AnoLead,
                            NumeroLead = compromissoAlterecaoRequest.NumeroLead,
                            AnoOportunidade = compromissoAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = compromissoAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO CONCLUSAO COMPROMISSO",
                            Complemento = $"DE: {compromissoAtual.Conclusao} - PARA: {compromissoAlterecaoRequest.Conclusao}"
                        });
                    }

                    var compromisso = AutoMapper.Mapper.Map(compromissoAlterecaoRequest, compromissoAtual);

                    if (!compromisso.AtualizarCompromissoScopeIsValid()) return;

                    _compromissoService.Atualizar(compromisso);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var compromissoOld = _compromissoService.ObterPorId(codigo);
            var compromissoAgrupamento = new List<Compromisso>();
            compromissoAgrupamento.Add(compromissoOld);

            if (!string.IsNullOrEmpty(compromissoOld.IdAgrupamento)) // Buscando outros compromissos do grupo
                compromissoAgrupamento.AddRange(_compromissoService.ListarFiltradosTracking(x => x.IdAgrupamento == compromissoOld.IdAgrupamento && x.Codigo != compromissoOld.Codigo).ToList());

            if (compromissoOld is null)
            {
                AssertionConcern.Notify("Deletar", "Compromisso não encontrado!");
                return;
            }

            foreach(var compromisso in compromissoAgrupamento)
            {

                _compromissoService.Remover(compromisso);

                _compromissoService.Adicionar(new CompromissoLog
                {
                    CodigoCompromisso = compromisso.Codigo,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    Descricao = compromisso.Descricao,
                    Usina = compromisso.UsinaCodigo,
                    AnoVisita = compromisso.AnoVisita,
                    NumeroVisita = compromisso.NumeroVisita,
                    AnoLead = compromisso.AnoLead,
                    NumeroLead = compromisso.NumeroLead,
                    AnoOportunidade = compromisso.AnoOportunidade,
                    NumeroOportunidade = compromisso.NumeroOportunidade,
                    Evento = $"EXCLUSÃO COMPROMISSO{(!string.IsNullOrEmpty(compromissoOld.IdAgrupamento) ? "" : " EM GRUPO")}",
                    Complemento = $"{(!string.IsNullOrEmpty(compromissoOld.IdAgrupamento) ? "" : $"GRUPO: {compromissoOld.IdAgrupamento} ")}Data Inicio: {compromisso.DataInicio:dd/MM/yyyy} Data Fim: {compromisso.DataFim:dd/MM/yyyy} {(compromisso.DiaInteiro ? "Hora: Dia Inteiro" : $"Hora Inicio: {compromisso.HoraFim.Value.ToString(@"hh\:mm")} Hora Fim: {compromisso.HoraFim.Value.ToString(@"hh\:mm")}")}"
                });

            }

            Commit();
        }

        public PagedList<CompromissoResponse> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_compromissoService.ListarEmOrdemDecrescentePorHorario(pagina, porPagina, filter), new PagedList<CompromissoResponse>());
        }

        public CompromissoResponse ObterPorId(int codigo)
        {
            return AutoMapper.Mapper.Map(_compromissoService.ObterPorId(codigo), new CompromissoResponse());
        }

        public Dictionary<string, string> ListarGrupoUsuario()
        {
            return _compromissoService.ListarGrupoUsuario();
        }

        public Dictionary<string, string> UsuariosLigadosAgrupamento(string idAgrupamento)
        {

            var lista = _compromissoService.ListarFiltrados<Compromisso>(x => x.IdAgrupamento == idAgrupamento).ToList();
            var todosUsuarios = _compromissoService.ListarGrupoUsuario();

            var usuariosVinculados = new Dictionary<string, string>();

            foreach (var item in lista) {

                if (!todosUsuarios.ContainsKey(item.Usuario))
                    continue;

                usuariosVinculados.Add(item.Usuario, todosUsuarios[item.Usuario]);

            }

            return usuariosVinculados;

        }
    }
}

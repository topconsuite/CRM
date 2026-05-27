using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Tarefa;
using TopSys.TopConWeb.Application.DTOS.Response.Tarefa;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class TarefaApplicationService : ApplicationServiceBase<Tarefa>, ITarefaApplicationService
    {
        private readonly ITarefaService _tarefaService;
        private readonly ICompromissoService _compromissoService;
        public TarefaApplicationService(ITarefaService tarefaService, ICompromissoService compromissoService, IUnitOfWork unityOfWork) : base(tarefaService, unityOfWork)
        {
            _tarefaService = tarefaService;
            _compromissoService = compromissoService;
        }

        public void AdicionarGrupo(TarefaInclusaoRequest[] tarefasInclusaoRequest, string userRequest)
        {

            var tarefas = AutoMapper.Mapper.Map(tarefasInclusaoRequest.ToList(), new List<Tarefa>());
            var idAgrupamento = Guid.NewGuid().ToString();

            using (var scope = new TransactionScope())
            {
                foreach (var tarefa in tarefas)
                {
                    tarefa.IdAgrupamento = idAgrupamento;

                    _tarefaService.Adicionar(tarefa);

                    _tarefaService.Adicionar(new TarefaLog
                    {
                        CodigoTarefa = tarefa.Codigo,
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest,
                        Descricao = tarefa.Descricao,
                        Usina = tarefa.UsinaCodigo,
                        AnoVisita = tarefa.AnoVisita,
                        NumeroVisita = tarefa.NumeroVisita,
                        AnoLead = tarefa.AnoLead,
                        NumeroLead = tarefa.NumeroLead,
                        AnoOportunidade = tarefa.AnoOportunidade,
                        NumeroOportunidade = tarefa.NumeroOportunidade,
                        Evento = "INSERÇÃO NOVA TAREFA EM GRUPO",
                        Complemento = $"Grupo: {idAgrupamento} Data: {tarefa.Data:dd/MM/yyyy} Horário: {(tarefa.DiaInteiro ? "Dia Inteiro" : tarefa.Horario.Value.ToString(@"hh\:mm"))}"
                    });

                }

                Commit();
                scope.Complete();

            }

                

        }

        public void Adicionar(TarefaInclusaoRequest tarefaInclusaoRequest, string userRequest)
        {
            var tarefa = AutoMapper.Mapper.Map(tarefaInclusaoRequest, new Tarefa());

            if (!tarefa.AdicionarTarefaScopeIsValid()) return;

            using (var scope = new TransactionScope())
            {
                _tarefaService.Adicionar(tarefa);

                _tarefaService.Adicionar(new TarefaLog
                {
                    CodigoTarefa = tarefa.Codigo,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    Descricao = tarefa.Descricao,
                    Usina = tarefa.UsinaCodigo,
                    AnoVisita = tarefa.AnoVisita,
                    NumeroVisita = tarefa.NumeroVisita,
                    AnoLead = tarefa.AnoLead,
                    NumeroLead = tarefa.NumeroLead,
                    AnoOportunidade = tarefa.AnoOportunidade,
                    NumeroOportunidade = tarefa.NumeroOportunidade,
                    Evento = "INSERÇÃO NOVA TAREFA",
                    Complemento = $"Data: {tarefa.Data:dd/MM/yyyy} Horário: {(tarefa.DiaInteiro ? "Dia Inteiro" : tarefa.Horario.Value.ToString(@"hh\:mm"))}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(TarefaAlteracaoRequest tarefaAlterecaoRequest, string userRequest)
        {
            var tarefaOld = _tarefaService.ObterPorId(tarefaAlterecaoRequest.Codigo);
            var tarefasAgrupamento = new List<Tarefa>();
            tarefasAgrupamento.Add(tarefaOld);

            if (!string.IsNullOrEmpty(tarefaOld.IdAgrupamento))
                tarefasAgrupamento.AddRange(_tarefaService.ListarFiltradosTracking(x => x.IdAgrupamento == tarefaOld.IdAgrupamento && x.Codigo != tarefaOld.Codigo).ToList());
                

            if (tarefaOld is null)
            {
                AssertionConcern.Notify("Atualizar", "Tarefa(s) não encontrada(s)!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                foreach(var tarefaAtual in tarefasAgrupamento)
                {


                    if (tarefaAlterecaoRequest.Finalizado != tarefaAtual.Finalizado)
                    {
                        _tarefaService.Adicionar(new TarefaLog
                        {
                            CodigoTarefa = tarefaAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = tarefaAlterecaoRequest.Descricao,
                            Usina = tarefaAlterecaoRequest.UsinaCodigo,
                            AnoVisita = tarefaAlterecaoRequest.AnoVisita,
                            NumeroVisita = tarefaAlterecaoRequest.NumeroVisita,
                            AnoLead = tarefaAlterecaoRequest.AnoLead,
                            NumeroLead = tarefaAlterecaoRequest.NumeroLead,
                            AnoOportunidade = tarefaAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = tarefaAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO FINALIZAÇÃO TAREFA",
                            Complemento = $"DE: {(tarefaAtual.Finalizado ? "Sim" : "Não")} - PARA: {(tarefaAlterecaoRequest.Finalizado ? "Sim" : "Não")}"
                        });
                    }
                    if (tarefaAlterecaoRequest.Descricao != tarefaAtual.Descricao)
                    {
                        _tarefaService.Adicionar(new TarefaLog
                        {
                            CodigoTarefa = tarefaAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = tarefaAlterecaoRequest.Descricao,
                            Usina = tarefaAlterecaoRequest.UsinaCodigo,
                            AnoVisita = tarefaAlterecaoRequest.AnoVisita,
                            NumeroVisita = tarefaAlterecaoRequest.NumeroVisita,
                            AnoLead = tarefaAlterecaoRequest.AnoLead,
                            NumeroLead = tarefaAlterecaoRequest.NumeroLead,
                            AnoOportunidade = tarefaAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = tarefaAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO DESCRICAO TAREFA",
                            Complemento = $"DE: {tarefaAtual.Descricao} - PARA: {tarefaAlterecaoRequest.Descricao}"
                        });
                    }
                    if ((tarefaAlterecaoRequest.Data != tarefaAtual.Data) || (tarefaAlterecaoRequest.Horario != tarefaAtual.Horario))
                    {
                        _tarefaService.Adicionar(new TarefaLog
                        {
                            CodigoTarefa = tarefaAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = tarefaAlterecaoRequest.Descricao,
                            Usina = tarefaAlterecaoRequest.UsinaCodigo,
                            AnoVisita = tarefaAlterecaoRequest.AnoVisita,
                            NumeroVisita = tarefaAlterecaoRequest.NumeroVisita,
                            AnoLead = tarefaAlterecaoRequest.AnoLead,
                            NumeroLead = tarefaAlterecaoRequest.NumeroLead,
                            AnoOportunidade = tarefaAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = tarefaAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO DATA E HORARIO TAREFA",
                            Complemento = $"DE: Data:{tarefaAtual.Data:dd/MM/yyyy} Horário: {(tarefaAtual.DiaInteiro ? "Dia Inteiro" : tarefaAtual.Horario.Value.ToString(@"hh\:mm"))} " +
                            $"- PARA: Data: {tarefaAlterecaoRequest.Data:dd/MM/yyyy} Horário: {(tarefaAlterecaoRequest.DiaInteiro ? "Dia Inteiro" : tarefaAlterecaoRequest.Horario.ToString(@"hh\:mm"))}"
                        });
                    }
                    if (tarefaAlterecaoRequest.Providencia != tarefaAtual.Providencia)
                    {
                        _tarefaService.Adicionar(new TarefaLog
                        {
                            CodigoTarefa = tarefaAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = tarefaAlterecaoRequest.Descricao,
                            Usina = tarefaAlterecaoRequest.UsinaCodigo,
                            AnoVisita = tarefaAlterecaoRequest.AnoVisita,
                            NumeroVisita = tarefaAlterecaoRequest.NumeroVisita,
                            AnoLead = tarefaAlterecaoRequest.AnoLead,
                            NumeroLead = tarefaAlterecaoRequest.NumeroLead,
                            AnoOportunidade = tarefaAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = tarefaAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO PROVIDENCIA TAREFA",
                            Complemento = $"DE: {tarefaAtual.Providencia} - PARA: {tarefaAlterecaoRequest.Providencia}"
                        });
                    }
                    if (tarefaAlterecaoRequest.Conclusao != tarefaAtual.Conclusao)
                    {
                        _tarefaService.Adicionar(new TarefaLog
                        {
                            CodigoTarefa = tarefaAtual.Codigo,
                            DataHoraEvento = DateTime.Now,
                            Usuario = userRequest,
                            Descricao = tarefaAlterecaoRequest.Descricao,
                            Usina = tarefaAlterecaoRequest.UsinaCodigo,
                            AnoVisita = tarefaAlterecaoRequest.AnoVisita,
                            NumeroVisita = tarefaAlterecaoRequest.NumeroVisita,
                            AnoLead = tarefaAlterecaoRequest.AnoLead,
                            NumeroLead = tarefaAlterecaoRequest.NumeroLead,
                            AnoOportunidade = tarefaAlterecaoRequest.AnoOportunidade,
                            NumeroOportunidade = tarefaAlterecaoRequest.NumeroOportunidade,
                            Evento = "ALTERAÇÃO CONCLUSAO TAREFA",
                            Complemento = $"DE: {tarefaAtual.Conclusao} - PARA: {tarefaAlterecaoRequest.Conclusao}"
                        });
                    }

                    var tarefa = AutoMapper.Mapper.Map(tarefaAlterecaoRequest, tarefaAtual);

                    if (!tarefa.AtualizarTarefaScopeIsValid()) return;

                    _tarefaService.Atualizar(tarefa);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var tarefaOld = _tarefaService.ObterPorId<Tarefa>(codigo);
            var tarefas = new List<Tarefa>();
            tarefas.Add(tarefaOld);

            if (!string.IsNullOrEmpty(tarefaOld.IdAgrupamento))
                tarefas.AddRange(_tarefaService.ListarFiltradosTracking(x => x.IdAgrupamento == tarefaOld.IdAgrupamento && x.Codigo != tarefaOld.Codigo));

            if (tarefaOld is null)
            {
                AssertionConcern.Notify("Deletar", "Tarefa não encontrado!");
                return;
            }

            foreach(var tarefa in tarefas)
            {
                _tarefaService.Adicionar(new TarefaLog
                {
                    CodigoTarefa = tarefa.Codigo,
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest,
                    Descricao = tarefa.Descricao,
                    Usina = tarefa.UsinaCodigo,
                    AnoVisita = tarefa.AnoVisita,
                    NumeroVisita = tarefa.NumeroVisita,
                    AnoLead = tarefa.AnoLead,
                    NumeroLead = tarefa.NumeroLead,
                    AnoOportunidade = tarefa.AnoOportunidade,
                    NumeroOportunidade = tarefa.NumeroOportunidade,
                    Evento = $"EXCLUSÃO TAREFA{(string.IsNullOrEmpty(tarefa.IdAgrupamento) ? "" : " EM GRUPO")}",
                    Complemento = $"{(string.IsNullOrEmpty(tarefa.IdAgrupamento) ? "" : $"GRUPO: {tarefa.IdAgrupamento} ")}Data: {tarefa.Data:dd/MM/yyyy} Horário: {(tarefa.DiaInteiro ? "Dia Inteiro" : tarefa.Horario.Value.ToString(@"hh\:mm"))}"
                });

                _tarefaService.Remover(tarefa);
            }

            Commit();
        }

        public PagedList<TarefaResponse> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_tarefaService.ListarEmOrdemDecrescentePorHorario(pagina, porPagina, filter), new PagedList<TarefaResponse>());
        }

        public TarefaResponse ObterPorId(int codigo)
        {
            return AutoMapper.Mapper.Map(_tarefaService.ObterPorId(codigo), new TarefaResponse());
        }

        public Dictionary<string, string> UsuariosLigadosAgrupamento(string idAgrupamento)
        {

            var lista = _tarefaService.ListarFiltrados<Tarefa>(x => x.IdAgrupamento == idAgrupamento).ToList();
            var todosUsuarios = _compromissoService.ListarGrupoUsuario();

            var usuariosVinculados = new Dictionary<string, string>();

            foreach (var item in lista)
            {

                if (!todosUsuarios.ContainsKey(item.Usuario))
                    continue;

                usuariosVinculados.Add(item.Usuario, todosUsuarios[item.Usuario]);

            }

            return usuariosVinculados;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Application.DTOS.Response.Visita;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class VisitaTipoApplicationService : ApplicationServiceBase<VisitaTipo>, IVisitaTipoApplicationService
    {
        private readonly IVisitaTipoService _tipoVisitaService;

        public VisitaTipoApplicationService(IVisitaTipoService tipoVisitaService, IUnitOfWork unityOfWork)
            : base(tipoVisitaService, unityOfWork)
        {
            _tipoVisitaService = tipoVisitaService;
        }

        public IEnumerable<VisitaTipoResponse> ListarTipoVisitasAtivas()
        {

            var tipoVisitas = _tipoVisitaService.ListarFiltrados(x => x.Ativo == true);
            var response = AutoMapper.Mapper.Map(tipoVisitas, new List<VisitaTipoResponse>());

            return response;

        }

        public void Adicionar(VisitaTipoInclusaoRequest tipoVisitaRequest, string userRequest)
        {
            var tipoVisita = AutoMapper.Mapper.Map(tipoVisitaRequest, new VisitaTipo());

            var tipoVisitaIgual = _tipoVisitaService.ListarFiltradosTracking(x => x.Descricao == tipoVisitaRequest.Descricao).FirstOrDefault();

            if (!(tipoVisitaIgual is null))
            {
                AssertionConcern.Notify("Adicionar", "A descrição fornecida já está vinculada a um Tipo de Visita existente!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                tipoVisita.IdCadastro = userRequest;
                _tipoVisitaService.Adicionar(tipoVisita);

                _tipoVisitaService.Adicionar(new VisitaLog
                {
                    Tipo = "TIPOS VISITA",
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest.Split(' ')[0],
                    Evento = "INSERÇÃO NOVO TIPO DE VISITA",
                    Complemento = $"Descricao: {tipoVisita.Descricao} Ativo: {(tipoVisita.Ativo ? "Sim" : "Nao")}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(VisitaTipoAlteracaoRequest tipoVisitaRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var tipoVisitaAtual = _tipoVisitaService.ObterPorId(tipoVisitaRequest.Codigo);

                if (tipoVisitaAtual is null)
                {
                    AssertionConcern.Notify("Atualizar", "Tipo de Visita não encontrado!");
                    return;
                }

                var tipoVisitaIgual = _tipoVisitaService.ListarFiltradosTracking(x => x.Descricao == tipoVisitaRequest.Descricao && x.Codigo != tipoVisitaRequest.Codigo).FirstOrDefault();
                if (!(tipoVisitaIgual is null))
                {
                    AssertionConcern.Notify("Atualizar", "A descrição fornecida já está vinculada a um Tipo de Visita existente!");
                    return;
                }

                if (tipoVisitaAtual.Descricao != tipoVisitaRequest.Descricao || tipoVisitaAtual.Ativo != tipoVisitaRequest.Ativo)
                {
                    _tipoVisitaService.Adicionar(new VisitaLog
                    {
                        Tipo = "TIPOS VISITA",
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest.Split(' ')[0],
                        Evento = $"ALTERAÇÃO TIPO DE VISITA",
                        Complemento = $"Codigo: {tipoVisitaAtual.Codigo} - DE: Descrição: {tipoVisitaAtual.Descricao} Ativo: {(tipoVisitaAtual.Ativo ? "Sim" : "Nao")} - PARA: Descrição: {tipoVisitaRequest.Descricao} Ativo: {(tipoVisitaRequest.Ativo ? "Sim" : "Nao")}"
                    });

                    tipoVisitaAtual.IdAtualizacao = userRequest;
                    AutoMapper.Mapper.Map(tipoVisitaRequest, tipoVisitaAtual);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var tipoVisita = _tipoVisitaService.ObterPorId<VisitaTipo>(codigo);

            if (tipoVisita is null)
            {
                AssertionConcern.Notify("Deletar", "Tipo de Visita não encontrado!");
                return;
            }

            var visitaComTipoDeVisita = _tipoVisitaService.ListarFiltradosTracking<Visita>(x => x.VisitaTipoCodigo == codigo).FirstOrDefault();

            if (!(visitaComTipoDeVisita is null))
            {
                AssertionConcern.Notify("Deletar", "Tipo de visita não pode ser excluída, pois já está sendo utilizada!");
                return;
            }

            _tipoVisitaService.Remover(tipoVisita);

            _tipoVisitaService.Adicionar(new VisitaLog
            {
                Tipo = "TIPOS VISITA",
                DataHoraEvento = DateTime.Now,
                Usuario = userRequest.Split(' ')[0],
                Evento = $"EXCLUSÃO TIPO DE VISITA",
                Complemento = $"Codigo: {tipoVisita.Codigo} Descrição: {tipoVisita.Descricao} Ativo: {(tipoVisita.Ativo ? "Sim" : "Nao")}"
            });

            Commit();
        }

        public VisitaTipoResponse ObterPorCodigo(int codigo)
        {
            return AutoMapper.Mapper.Map(_tipoVisitaService.ObterPorId(codigo), new VisitaTipoResponse());
        }

        public PagedList<VisitaTipoResponse> Listar(int pagina, int porPagina, Expression<Func<VisitaTipo, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_tipoVisitaService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<VisitaTipoResponse>());
        }
    }
}

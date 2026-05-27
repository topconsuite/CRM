using System;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class ConcorrenteApplicationService : ApplicationServiceBase<Concorrente>, IConcorrenteApplicationService
    {
        private readonly IConcorrenteService _concorrenteService;

        public ConcorrenteApplicationService(IConcorrenteService concorrenteService, IUnitOfWork unityOfWork) : base(concorrenteService, unityOfWork)
        {
            _concorrenteService = concorrenteService;
        }

        public void Adicionar(ConcorrenteInclusaoRequest concorrenteRequest, string userRequest)
        {
            var concorrente = AutoMapper.Mapper.Map(concorrenteRequest, new Concorrente());

            var concorrenteIgual = _concorrenteService.ListarFiltradosTracking(x => x.Descricao == concorrenteRequest.Descricao).FirstOrDefault();

            if (!(concorrenteIgual is null))
            {
                AssertionConcern.Notify("Adicionar", "A descrição fornecida já está vinculada a um Concorrente existente!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                concorrente.IdCadastro = userRequest;
                _concorrenteService.Adicionar(concorrente);

                _concorrenteService.Adicionar(new OportunidadeLog
                {
                    Tipo = "CONCORRENTE",
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest.Split(' ')[0],
                    Evento = "INSERÇÃO NOVO CONCORRENTE",
                    Complemento = $"Descricao: {concorrente.Descricao} Ativo: {(concorrente.Ativo ? "Sim" : "Nao")}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(ConcorrenteAlteracaoRequest concorrenteRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var concorrenteAtual = _concorrenteService.ObterPorId(concorrenteRequest.Codigo);

                if (concorrenteAtual is null)
                {
                    AssertionConcern.Notify("Atualizar", "Concorrente não encontrado!");
                    return;
                }

                var concorrenteIgual = _concorrenteService.ListarFiltradosTracking(x => x.Descricao == concorrenteRequest.Descricao && x.Codigo != concorrenteRequest.Codigo).FirstOrDefault();
                if (!(concorrenteIgual is null))
                {
                    AssertionConcern.Notify("Atualizar", "A descrição fornecida já está vinculada a um Concorrente existente!");
                    return;
                }

                if (concorrenteAtual.Descricao != concorrenteRequest.Descricao || concorrenteAtual.Ativo != concorrenteRequest.Ativo)
                {
                    _concorrenteService.Adicionar(new OportunidadeLog
                    {
                        Tipo = "CONCORRENTE",
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest.Split(' ')[0],
                        Evento = $"ALTERAÇÃO CONCORRENTE",
                        Complemento = $"Codigo: {concorrenteAtual.Codigo} - DE: Descrição: {concorrenteAtual.Descricao} Ativo: {(concorrenteAtual.Ativo ? "Sim" : "Nao")} - PARA: Descrição: {concorrenteRequest.Descricao} Ativo: {(concorrenteRequest.Ativo ? "Sim" : "Nao")}"
                    });

                    concorrenteAtual.IdAtualizacao = userRequest;
                    AutoMapper.Mapper.Map(concorrenteRequest, concorrenteAtual);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var concorrente = _concorrenteService.ObterPorId<Concorrente>(codigo);

            if (concorrente is null)
            {
                AssertionConcern.Notify("Deletar", "Concorrente não encontrado!");
                return;
            }

            var oportunidadeComConcorrente = _concorrenteService.ListarFiltradosTracking<Oportunidade>(x => x.ConcorrenteCodigo == codigo).FirstOrDefault();

            if (!(oportunidadeComConcorrente is null))
            {
                AssertionConcern.Notify("Deletar", "Concorrente não pode ser excluído, pois já está sendo utilizado!");
                return;
            }

            _concorrenteService.Remover(concorrente);

            _concorrenteService.Adicionar(new OportunidadeLog
            {
                Tipo = "CONCORRENTE",
                DataHoraEvento = DateTime.Now,
                Usuario = userRequest.Split(' ')[0],
                Evento = $"EXCLUSÃO CONCORRENTE",
                Complemento = $"Codigo: {concorrente.Codigo} Descrição: {concorrente.Descricao} Ativo: {(concorrente.Ativo ? "Sim" : "Nao")}"
            });

            Commit();
        }

        public ConcorrenteResponse ObterPorCodigo(int codigo)
        {
            return AutoMapper.Mapper.Map(_concorrenteService.ObterPorId(codigo), new ConcorrenteResponse());
        }

        public PagedList<ConcorrenteResponse> Listar(int pagina, int porPagina, Expression<Func<Concorrente, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_concorrenteService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<ConcorrenteResponse>());
        }
    }
}

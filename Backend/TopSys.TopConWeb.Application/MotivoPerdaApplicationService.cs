using System;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.MotivoPerda;
using TopSys.TopConWeb.Application.DTOS.Response.MotivoPerda;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class MotivoPerdaApplicationService : ApplicationServiceBase<MotivoPerda>, IMotivoPerdaApplicationService
    {
        private readonly IMotivoPerdaService _motivoPerdaService;

        public MotivoPerdaApplicationService(IMotivoPerdaService motivoPerdaService, IUnitOfWork unityOfWork) : base(motivoPerdaService, unityOfWork)
        {
            _motivoPerdaService = motivoPerdaService;
        }

        public void Adicionar(MotivoPerdaInclusaoRequest motivoPerdaRequest, string userRequest)
        {
            var motivoPerda = AutoMapper.Mapper.Map(motivoPerdaRequest, new MotivoPerda());

            var motivoPerdaIgual = _motivoPerdaService.ListarFiltradosTracking(x => x.Descricao == motivoPerdaRequest.Descricao).FirstOrDefault();

            if (!(motivoPerdaIgual is null))
            {
                AssertionConcern.Notify("Adicionar", "A descrição fornecida já está vinculada a um Motivo da Perda existente!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                motivoPerda.IdCadastro = userRequest;
                _motivoPerdaService.Adicionar(motivoPerda);

                _motivoPerdaService.Adicionar(new MotivoPerdaLog
                {
                    Tipo = "MOTIVO DA PERDA",
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest.Split(' ')[0],
                    Evento = "INSERÇÃO NOVO MOTIVO DA PERDA",
                    Complemento = $"Descricao: {motivoPerda.Descricao} Ativo: {(motivoPerda.Ativo ? "Sim" : "Nao")}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(MotivoPerdaAlteracaoRequest motivoPerdaRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var motivoPerdaAtual = _motivoPerdaService.ObterPorId(motivoPerdaRequest.Codigo);

                if (motivoPerdaAtual is null)
                {
                    AssertionConcern.Notify("Atualizar", "Motivo da Perda não encontrado!");
                    return;
                }

                var motivoPerdaIgual = _motivoPerdaService.ListarFiltradosTracking(x => x.Descricao == motivoPerdaRequest.Descricao && x.Codigo != motivoPerdaRequest.Codigo).FirstOrDefault();
                if (!(motivoPerdaIgual is null))
                {
                    AssertionConcern.Notify("Atualizar", "A descrição fornecida já está vinculada a um Motivo da Perda existente!");
                    return;
                }

                if (motivoPerdaAtual.Descricao != motivoPerdaRequest.Descricao || motivoPerdaAtual.Ativo != motivoPerdaRequest.Ativo)
                {
                    _motivoPerdaService.Adicionar(new MotivoPerdaLog
                    {
                        Tipo = "MOTIVO DA PERDA",
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest.Split(' ')[0],
                        Evento = $"ALTERAÇÃO MOTIVO DA PERDA",
                        Complemento = $"Codigo: {motivoPerdaAtual.Codigo} - DE: Descrição: {motivoPerdaAtual.Descricao} Ativo: {(motivoPerdaAtual.Ativo ? "Sim" : "Nao")} - PARA: Descrição: {motivoPerdaRequest.Descricao} Ativo: {(motivoPerdaRequest.Ativo ? "Sim" : "Nao")}"
                    });

                    motivoPerdaAtual.IdAtualizacao = userRequest;
                    AutoMapper.Mapper.Map(motivoPerdaRequest, motivoPerdaAtual);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var motivoPerda = _motivoPerdaService.ObterPorId<MotivoPerda>(codigo);

            if (motivoPerda is null)
            {
                AssertionConcern.Notify("Deletar", "Motivo da Perda não encontrado!");
                return;
            }

            var leadComMotivoDePerda = _motivoPerdaService.ListarFiltradosTracking<Lead>(x => x.MotivoPerdaCodigo == codigo).FirstOrDefault();
            var oportunidadeComMotivoDePerda = _motivoPerdaService.ListarFiltradosTracking<Oportunidade>(x => x.MotivoPerdaCodigo == codigo).FirstOrDefault();

            if (!(leadComMotivoDePerda is null) || !(oportunidadeComMotivoDePerda is null))
            {
                AssertionConcern.Notify("Deletar", "Motivo da perda não pode ser excluído, pois já está sendo utilizado!");
                return;
            }

            _motivoPerdaService.Remover(motivoPerda);

            _motivoPerdaService.Adicionar(new MotivoPerdaLog
            {
                Tipo = "MOTIVO DA PERDA",
                DataHoraEvento = DateTime.Now,
                Usuario = userRequest.Split(' ')[0],
                Evento = $"EXCLUSÃO MOTIVO DA PERDA",
                Complemento = $"Codigo: {motivoPerda.Codigo} Descrição: {motivoPerda.Descricao} Ativo: {(motivoPerda.Ativo ? "Sim" : "Nao")}"
            });

            Commit();
        }

        public MotivoPerdaResponse ObterPorCodigo(int codigo)
        {
            return AutoMapper.Mapper.Map(_motivoPerdaService.ObterPorId(codigo), new MotivoPerdaResponse());
        }

        public PagedList<MotivoPerdaResponse> Listar(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_motivoPerdaService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<MotivoPerdaResponse>());
        }
    }
}

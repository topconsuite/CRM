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
    public class OportunidadeTipoApplicationService : ApplicationServiceBase<OportunidadeTipo>, IOportunidadeTipoApplicationService
    {
        private readonly IOportunidadeTipoService _oportunidadeTipoService;

        public OportunidadeTipoApplicationService(IOportunidadeTipoService oportunidadeTipoService, IUnitOfWork unityOfWork) : base(oportunidadeTipoService, unityOfWork)
        {
            _oportunidadeTipoService = oportunidadeTipoService;
        }

        public void Adicionar(OportunidadeTipoInclusaoRequest oportunidadeTipoRequest, string userRequest)
        {
            var oportunidadeTipo = AutoMapper.Mapper.Map(oportunidadeTipoRequest, new OportunidadeTipo());

            var oportunidadeTipoIgual = _oportunidadeTipoService.ListarFiltradosTracking(x => x.Descricao == oportunidadeTipoRequest.Descricao).FirstOrDefault();

            if (!(oportunidadeTipoIgual is null))
            {
                AssertionConcern.Notify("Adicionar", "A descrição fornecida já está vinculada a um Tipo de Oportunidade existente!");
                return;
            }

            using (var scope = new TransactionScope())
            {
                oportunidadeTipo.IdCadastro = userRequest;
                _oportunidadeTipoService.Adicionar(oportunidadeTipo);

                _oportunidadeTipoService.Adicionar(new OportunidadeLog
                {
                    Tipo = "TIPO DE OPORTUNIDADE",
                    DataHoraEvento = DateTime.Now,
                    Usuario = userRequest.Split(' ')[0],
                    Evento = "INSERÇÃO NOVO TIPO DE OPORTUNIDADE",
                    Complemento = $"Descricao: {oportunidadeTipo.Descricao} Ativo: {(oportunidadeTipo.Ativo ? "Sim" : "Nao")}"
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(OportunidadeTipoAlteracaoRequest oportunidadeTipoRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var oportunidadeTipoAtual = _oportunidadeTipoService.ObterPorId(oportunidadeTipoRequest.Codigo);

                if (oportunidadeTipoAtual is null)
                {
                    AssertionConcern.Notify("Atualizar", "Tipo de Oportunidade não encontrado!");
                    return;
                }

                var oportunidadeTipoIgual = _oportunidadeTipoService.ListarFiltradosTracking(x => x.Descricao == oportunidadeTipoRequest.Descricao && x.Codigo != oportunidadeTipoRequest.Codigo).FirstOrDefault();
                if (!(oportunidadeTipoIgual is null))
                {
                    AssertionConcern.Notify("Atualizar", "A descrição fornecida já está vinculada a um Tipo de Oportunidade existente!");
                    return;
                }

                if (oportunidadeTipoAtual.Descricao != oportunidadeTipoRequest.Descricao || oportunidadeTipoAtual.Ativo != oportunidadeTipoRequest.Ativo)
                {
                    _oportunidadeTipoService.Adicionar(new OportunidadeLog
                    {
                        Tipo = "TIPO DE OPORTUNIDADE",
                        DataHoraEvento = DateTime.Now,
                        Usuario = userRequest.Split(' ')[0],
                        Evento = $"ALTERAÇÃO TIPO DE OPORTUNIDADE",
                        Complemento = $"Codigo: {oportunidadeTipoAtual.Codigo} - DE: Descrição: {oportunidadeTipoAtual.Descricao} Ativo: {(oportunidadeTipoAtual.Ativo ? "Sim" : "Nao")} - PARA: Descrição: {oportunidadeTipoRequest.Descricao} Ativo: {(oportunidadeTipoRequest.Ativo ? "Sim" : "Nao")}"
                    });

                    oportunidadeTipoAtual.IdAtualizacao = userRequest;
                    AutoMapper.Mapper.Map(oportunidadeTipoRequest, oportunidadeTipoAtual);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int codigo, string userRequest)
        {
            var oportunidadeTipo = _oportunidadeTipoService.ObterPorId<OportunidadeTipo>(codigo);

            if (oportunidadeTipo is null)
            {
                AssertionConcern.Notify("Deletar", "Tipo de Oportunidade não encontrado!");
                return;
            }

            var oportunidadeComOportunidadeTipo = _oportunidadeTipoService.ListarFiltradosTracking<Oportunidade>(x => x.OportunidadeTipoCodigo == codigo).FirstOrDefault();

            if (!(oportunidadeComOportunidadeTipo is null))
            {
                AssertionConcern.Notify("Deletar", "Tipo de Oportunidade não pode ser excluída, pois já está sendo utilizada!");
                return;
            }

            _oportunidadeTipoService.Remover(oportunidadeTipo);

            _oportunidadeTipoService.Adicionar(new OportunidadeLog
            {
                Tipo = "TIPO DE OPORTUNIDADE",
                DataHoraEvento = DateTime.Now,
                Usuario = userRequest.Split(' ')[0],
                Evento = $"EXCLUSÃO TIPO DE OPORTUNIDADE",
                Complemento = $"Codigo: {oportunidadeTipo.Codigo} Descrição: {oportunidadeTipo.Descricao} Ativo: {(oportunidadeTipo.Ativo ? "Sim" : "Nao")}"
            });

            Commit();
        }

        public OportunidadeTipoResponse ObterPorCodigo(int codigo)
        {
            return AutoMapper.Mapper.Map(_oportunidadeTipoService.ObterPorId(codigo), new OportunidadeTipoResponse());
        }

        public PagedList<OportunidadeTipoResponse> Listar(int pagina, int porPagina, Expression<Func<OportunidadeTipo, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_oportunidadeTipoService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<OportunidadeTipoResponse>());
        }
    }
}

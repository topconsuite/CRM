using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.GrupoEconomico;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class GrupoEconomicoApplicationService : ApplicationServiceBase<GrupoEconomico>, IGrupoEconomicoApplicationService
    {
        private readonly IGrupoEconomicoService _grupoEconomicoService;
        private readonly IIntervenienteService _intervenienteService;

        public GrupoEconomicoApplicationService(IGrupoEconomicoService grupoEconomicoService, IIntervenienteService intervenienteService, IUnitOfWork unityOfWork)
            :base(grupoEconomicoService, unityOfWork)
        {
            _grupoEconomicoService = grupoEconomicoService;
            _intervenienteService = intervenienteService;
        }

        public void Adicionar(GrupoEconomicoInclusaoRequest grupoEconomicoRequest, string userRequest)
        {
            var grupoEconomico = AutoMapper.Mapper.Map(grupoEconomicoRequest, new GrupoEconomico());

            var gruposEconomicos = AutoMapper.Mapper.Map(_grupoEconomicoService.ListarEmOrdemCrescente().ToList(), new List<GrupoEconomico>());

            if (gruposEconomicos.Any(g => g.Descricao == grupoEconomico.Descricao))
                throw new Exception("A descrição fornecida já está vinculada a um Grupo Econômico existente.");

            using (var scope = new TransactionScope())
            {
                grupoEconomico.IdCadastro = StringHelper.GetIDD(userRequest);
                _grupoEconomicoService.Adicionar(grupoEconomico);

                foreach (var cliente in grupoEconomicoRequest.Clientes)
                {
                    AdicionarClienteNoGrupoEconomico(cliente.Codigo, grupoEconomico.Codigo, userRequest);
                }

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(GrupoEconomicoAlteracaoRequest grupoEconomicoRequest, string userRequest)
        {
            using (var scope = new TransactionScope())
            {
                var grupoEconomicoAtual = _grupoEconomicoService.ObterPorId(grupoEconomicoRequest.Codigo);

                if (grupoEconomicoAtual is null)
                    throw new Exception("Grupo Econômico não encontrado!");

                var grupoEconomicoAtualizado = AutoMapper.Mapper.Map(grupoEconomicoRequest, grupoEconomicoAtual);
                grupoEconomicoAtualizado.IdAtualizacao = StringHelper.GetIDD(userRequest);

                grupoEconomicoAtualizado.BloqueioMotivoCodigo = 0;
                if (grupoEconomicoAtualizado.BloqueioMotivo != null)
                {
                    grupoEconomicoAtualizado.BloqueioMotivoCodigo = grupoEconomicoAtualizado.BloqueioMotivo.Codigo;
                    grupoEconomicoAtualizado.BloqueioMotivo = null;
                }

                if (grupoEconomicoAtualizado.BloqueioMotivoCodigo == 0) grupoEconomicoAtualizado.BloqueioObservacao = "";

                _grupoEconomicoService.Atualizar(grupoEconomicoAtualizado);

                if (grupoEconomicoRequest.Clientes != null)
                {
                    var clientesRemovidos = grupoEconomicoAtual.Clientes
                        .Where(x => !grupoEconomicoRequest.Clientes.Any(y => y.Codigo == x.Codigo))
                        .ToList();

                    var clientesAdicionados = grupoEconomicoRequest.Clientes
                        .Where(x => !grupoEconomicoAtual.Clientes.Any(y => y.Codigo == x.Codigo))
                        .ToList();

                    foreach (var cliente in clientesRemovidos)
                    {
                        RemoverGrupoEconomicoDoCliente(cliente, userRequest);
                    }

                    foreach (var cliente in clientesAdicionados)
                    {
                        AdicionarClienteNoGrupoEconomico(cliente.Codigo, grupoEconomicoAtual.Codigo, userRequest);
                    }
                } 

                Commit();
                scope.Complete();
            }
        }

        public void Deletar(int idServico)
        {
            var grupoEconomico = _grupoEconomicoService.ObterPorId(idServico);

            if (grupoEconomico is null)
                throw new Exception("Grupo Econômico não encontrado!");

            var clientesVinculados = grupoEconomico.Clientes;
            if (clientesVinculados.Count > 0)
                throw new Exception("Grupo Econômico contêm clientes vinculados e não poderá ser excluído");

            _grupoEconomicoService.Remover(grupoEconomico);

            Commit();
        }

        public GrupoEconomicoResponse ObterPorCodigo(int grupoEconomicoCodigo)
        {
            return AutoMapper.Mapper.Map(_grupoEconomicoService.ObterPorId(grupoEconomicoCodigo), new GrupoEconomicoResponse());
        }

        public PagedList<GrupoEconomicoResponse> Listar(int pagina, int porPagina, IFilter filter)
        {
            return AutoMapper.Mapper.Map(_grupoEconomicoService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<GrupoEconomicoResponse>());
        }

        public IEnumerable<GrupoEconomicoResponse> Listar()
        {
            return AutoMapper.Mapper.Map(_grupoEconomicoService.ListarEmOrdemCrescente(), new List<GrupoEconomicoResponse>());
        }

        private void AdicionarClienteNoGrupoEconomico(int codigoCliente, int codigoGrupoEconomico, string userRequest)
        {
            var interveniente = _intervenienteService.ObterPorId(codigoCliente);

            if (interveniente is null)
                throw new Exception("Cliente não encontrado!");

            if (interveniente.Cliente == "N")
                throw new Exception("Apenas clientes podem ser vinculados em grupos econômicos!");

            if (interveniente.GrupoEconomicoCodigo != 0 && interveniente.GrupoEconomicoCodigo != null)
                throw new Exception("Cliente já vinculado a um Grupo Econômico!");

            interveniente.GrupoEconomicoCodigo = codigoGrupoEconomico;
            interveniente.IdAtualizacao = StringHelper.GetIDD(userRequest);
            _intervenienteService.Atualizar(interveniente);
        }

        private void RemoverGrupoEconomicoDoCliente(Interveniente cliente, string userRequest)
        {
            cliente.GrupoEconomicoCodigo = null;
            cliente.GrupoEconomico = null;
            cliente.IdAtualizacao = StringHelper.GetIDD(userRequest);
            _intervenienteService.Atualizar(cliente);
        }
    }
}

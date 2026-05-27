using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Application
{
    public class PropostaPropagandaApplicationService : IPropostaPropagandaApplicationService
    {

        private readonly IPropostaPropagandaService _propostaPropagandaService;

        public PropostaPropagandaApplicationService(IPropostaPropagandaService propostaPropagandaService)
        {
            _propostaPropagandaService = propostaPropagandaService;
        }

        public void Adicionar(PropostaPropagandaAdicionarRequest request, string usuario, out string mensagem)
        {

            mensagem = "";

            if (request == null)
            {
                mensagem = $"Arquivos maiores que 10 MB não são suportados. Por favor, envie um arquivo com tamanho inferior.";
                return;
            }

            var propaganda = AutoMapper.Mapper.Map(request, new PropostaPropaganda());
            propaganda.Usuario = usuario;

            var propagandas = ListarTodos();
            if (propagandas.Count == 0)
                propaganda.Ativa = true;

            _propostaPropagandaService.Adicionar(propaganda, request.Arquivo);
        }

        public void Atualizar(PropostaPropagandaAtualizarRequest request)
        {
            var propagandasAtivas = _propostaPropagandaService.ListarTodos().Where(t => t.Ativa && t.Id.ToString() != request.Id.ToString()).Count();

            if (propagandasAtivas > 0)
            {
                AssertionConcern.Notify("Propaganda", "Já existe uma propaganda ativa!");
                return;
            }

            var propaganda = AutoMapper.Mapper.Map(request, new PropostaPropaganda());

            _propostaPropagandaService.Atualizar(propaganda);
        }

        public List<PropostaPropagandaResponse> ListarTodos()
        {
            return AutoMapper.Mapper.Map(_propostaPropagandaService.ListarTodos(), new List<PropostaPropagandaResponse>());
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _propostaPropagandaService.ObterAnexo(id);
        }

        public PropostaPropagandaResponse ObterPorId(Guid id)
        {
            return AutoMapper.Mapper.Map(_propostaPropagandaService.ObterPorId(id), new PropostaPropagandaResponse());
        }

        public void Remover(Guid id)
        {
            _propostaPropagandaService.Remover(id);
        }
    }
}

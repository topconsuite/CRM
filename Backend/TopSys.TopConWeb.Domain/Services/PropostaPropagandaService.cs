using System;
using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class PropostaPropagandaService : IPropostaPropagandaService
    {

        private readonly IPropostaPropagandaRepository _propostaPropagandaRepository;

        public PropostaPropagandaService(IPropostaPropagandaRepository propostaPropagandaRepository)
        {
            _propostaPropagandaRepository = propostaPropagandaRepository;
        }

        public void Adicionar(PropostaPropaganda propaganda, string anexo)
        {
            _propostaPropagandaRepository.Adicionar(propaganda, anexo);
        }

        public void Atualizar(PropostaPropaganda propaganda)
        {
            _propostaPropagandaRepository.Atualizar(propaganda);
        }

        public IEnumerable<PropostaPropaganda> ListarTodos()
        {
            return _propostaPropagandaRepository.ListarTodos();
        }

        public byte[] ObterAnexo(Guid id)
        {
            return _propostaPropagandaRepository.ObterAnexo(id);
        }

        public PropostaPropaganda ObterPorId(Guid id)
        {
            return _propostaPropagandaRepository.ObterPorId(id);
        }

        public void Remover(Guid id)
        {
            _propostaPropagandaRepository.Remover(id);
        }

    }
}

using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IPropostaPropagandaRepository
    {

        IEnumerable<PropostaPropaganda> ListarTodos();
        byte[] ObterAnexo(Guid id);
        PropostaPropaganda ObterPorId(Guid id);
        void Adicionar(PropostaPropaganda propaganda, string anexo);
        void Atualizar(PropostaPropaganda propaganda);
        void Remover(Guid id);


    }
}

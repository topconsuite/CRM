using System;
using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IPropostaPropagandaService
    {

        IEnumerable<PropostaPropaganda> ListarTodos();
        byte[] ObterAnexo(Guid id);
        PropostaPropaganda ObterPorId(Guid id);
        void Adicionar(PropostaPropaganda propaganda, string anexo);
        void Atualizar(PropostaPropaganda propaganda);
        void Remover(Guid id);

    }
}

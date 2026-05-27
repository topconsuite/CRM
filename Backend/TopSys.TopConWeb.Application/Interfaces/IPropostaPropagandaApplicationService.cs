using System;
using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IPropostaPropagandaApplicationService
    {

        List<PropostaPropagandaResponse> ListarTodos();
        byte[] ObterAnexo(Guid id);
        PropostaPropagandaResponse ObterPorId(Guid id);
        void Adicionar(PropostaPropagandaAdicionarRequest propaganda, string usuario, out string mensagem);
        void Atualizar(PropostaPropagandaAtualizarRequest propaganda);
        void Remover(Guid id);

    }
}

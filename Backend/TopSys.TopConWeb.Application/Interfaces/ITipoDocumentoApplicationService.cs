using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDocumento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TipoDocumento;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITipoDocumentoApplicationService
    {
        ResultDTO<TipoDocumentoAdicionarResponse> Adicionar(TipoDocumentoAdicionarRequest[] request);
        ResultDTO<TipoDocumentoResponse> AtualizarPorId(int id, TipoDocumentoAtualizarRequest request);
        ResultDTO<ICollection<TipoDocumentoResponse>> Listar();
        ResultDTO<TipoDocumentoResponse> ObterPorId(int id);
    }
}
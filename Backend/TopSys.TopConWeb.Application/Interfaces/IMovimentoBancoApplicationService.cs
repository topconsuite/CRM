using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.MovimentoBanco;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IMovimentoBancoApplicationService
    {
        IEnumerable<MovimentoBancoNaoVinvuladoComContasAReceberResponse> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao);
    }
}

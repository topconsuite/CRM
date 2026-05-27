using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IMovimentoBancoService
    {
        IEnumerable<MovimentoBanco> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao);
    }
}

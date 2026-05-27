using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.MovimentoBanco;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Application
{
    public class MovimentoBancoApplicationService : IMovimentoBancoApplicationService
    {
        private readonly IMovimentoBancoService _movimentoBancoService;

        public MovimentoBancoApplicationService(IMovimentoBancoService movimentoBancoService)
        {
            _movimentoBancoService = movimentoBancoService;
        }

        public IEnumerable<MovimentoBancoNaoVinvuladoComContasAReceberResponse> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao)
        {
            return AutoMapper.Mapper.Map(_movimentoBancoService.ListarNaoVinculadosComContasAReceber(empresaCodigo, contaCodigo, dataOperacao), new List<MovimentoBancoNaoVinvuladoComContasAReceberResponse>());
        }
    }
}

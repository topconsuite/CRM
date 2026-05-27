using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class MovimentoBancoService : IMovimentoBancoService
    {
        private readonly IMovimentoBancoRepository _movimentoBancoRepository;

        public MovimentoBancoService(IMovimentoBancoRepository movimentoBancoRepository)
        {
            _movimentoBancoRepository = movimentoBancoRepository;
        }

        public IEnumerable<MovimentoBanco> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao)
        {
            return _movimentoBancoRepository.ListarNaoVinculadosComContasAReceber(empresaCodigo, contaCodigo, dataOperacao);
        }
    }
}

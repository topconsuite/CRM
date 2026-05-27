using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class OperacaoFinanceiraService : ServiceBase<OperacaoFinanceira>, IOperacaoFinanceiraService
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;
        
        public OperacaoFinanceiraService(IOperacaoFinanceiraRepository operacaoFinanceiraRepository) : base(operacaoFinanceiraRepository)
        {
            _operacaoFinanceiraRepository = operacaoFinanceiraRepository;
        }

        public ICollection<OperacaoFinanceira> Listar()
        {
            return _operacaoFinanceiraRepository.ListarOperacaoFinanceira();
        }

        public OperacaoFinanceira ObterPorId(int id, bool tracking = false)
        {
            return _operacaoFinanceiraRepository.ObterPorIdOperacaoFinanceira(id, tracking);
        }

        public bool EstaEmUsoOperacaoFinanceira(int id)
        {
            return _operacaoFinanceiraRepository.EstaEmUsoOperacaoFinanceira(id);
        }
    }
}
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class SituacaoFinanceiraService : ServiceBase<SituacaoFinanceira>, ISituacaoFinanceiraService
    {
        private readonly ISituacaoFinanceiraRepository _situacaoFinanceiraRepository;

        public SituacaoFinanceiraService(ISituacaoFinanceiraRepository situacaoFinanceiraRepository) : base(situacaoFinanceiraRepository)
        {
            _situacaoFinanceiraRepository = situacaoFinanceiraRepository;
        }

        public ICollection<SituacaoFinanceira> Listar()
        {
            return _situacaoFinanceiraRepository.ListarSituacaoFinanceira();
        }

        public SituacaoFinanceira ObterPorId(int id, bool tracking = false)
        {
            return _situacaoFinanceiraRepository.ObterPorIdSituacaoFinanceira(id, tracking);
        }

        public bool EstaEmUsoSituacaoFinanceira(int id)
        {
            return _situacaoFinanceiraRepository.EstaEmUsoSituacaoFinanceira(id);
        }

        public bool VerificaSeExisteOperacao(int codigoOperacao)
        {
            return _situacaoFinanceiraRepository.IsOnTable(codigoOperacao.ToString(), "cod", "fin_operacao");
        }

        public bool ValidaOperacaoBaixa(int codigoOperacao)
        {
            return _situacaoFinanceiraRepository.ValidaOperacaoBaixa(codigoOperacao);
        }
    }
}

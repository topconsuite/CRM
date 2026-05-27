using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ISituacaoFinanceiraService : IServiceBase<SituacaoFinanceira>
    {
        ICollection<SituacaoFinanceira> Listar();
        SituacaoFinanceira ObterPorId(int id, bool tracking = false);
        bool EstaEmUsoSituacaoFinanceira(int id);
        bool VerificaSeExisteOperacao(int codigoOperacao);
        bool ValidaOperacaoBaixa(int codigoOperacao);
    }
}

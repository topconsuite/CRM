using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICondicaoPagamentoParcelaRepository : IRepositoryBase<CondicaoPagamentoParcela>
    {
        new void Adicionar(CondicaoPagamentoParcela condicaoPagamentoParcela);
        new void Remover(int conditionPayment);
    }
}

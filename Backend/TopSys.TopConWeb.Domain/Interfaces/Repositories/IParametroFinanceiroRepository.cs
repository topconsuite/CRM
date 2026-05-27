using System;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IParametroFinanceiroRepository
    {
        int ObterOperacaoCartaoPeloCodigoDaEmpresa(int empresa);
        int ObterOperacaoRecebimentoDoClientePeloCodigoUsina(int codigoUsinaObra);
        int ObterOperacaoMovimentoBancoAdiantamentoCliente(int codigoUsinaObra);
        DateTime? ObterMesAbertoPorEmpresa(int empresa);
        ParametroFinanceiroCheque ObterParametroChequePorEmpresa(int empresa);
    }
}

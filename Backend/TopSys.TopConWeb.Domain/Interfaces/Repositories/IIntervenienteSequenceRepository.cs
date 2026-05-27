using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IIntervenienteSequenceRepository : IRepositoryBase<IntervenienteSequence>
    {
        void CriaOuIgnoraTabela();
        int AtualizarEObterProximaSequencia(int faixaInicial, int faixaFinal);
        void SincronizarFaixa(IntervenienteSequence intervenienteSequence);
        int ObterFaixa(int faixaInicial, int faixaFinal);
        bool ValidaFaixaTotalmenteUtilizada(int faixaInicial, int faixaFinal);
        bool ValidaProbabilidadeDeDuplicidadeFaixaUtilizada(int faixaInicial, int faixaFinal);
        bool ValidaDuplicacoesDeFaixasIniciais();
        bool ValidaDuplicacoesDeFaixasFinais();
        bool ValidaCodigosForaDeFaixa();

    }
}

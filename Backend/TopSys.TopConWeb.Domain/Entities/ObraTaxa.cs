using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ObraTaxa : ObraTaxaBase<TaxaExtra>
    {
        protected ObraTaxa() : base() { }

        public ObraTaxa(int usinaCodigo, int obraCodigo, int sequencia, string selecionada, string aprovacaoSolicitante, string aprovada, string aprovacaoUsuario, string aprovacaoCiente) :
            base(usinaCodigo, obraCodigo, sequencia, selecionada, aprovacaoSolicitante, aprovada, aprovacaoUsuario, aprovacaoCiente)
        {
        }

        public ObraTaxa(TaxaExtra taxa, int obraCodigo, string aprovacaoSolicitante, string aprovada, string selecionada, bool isPersonalizada) :
            base(taxa, obraCodigo, aprovacaoSolicitante, aprovada, selecionada, isPersonalizada)
        {
        }
    }
}

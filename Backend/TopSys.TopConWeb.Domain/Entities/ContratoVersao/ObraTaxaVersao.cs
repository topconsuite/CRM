using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ObraTaxaVersao : ObraTaxaBase<TaxaExtraVersao>
    {
        public int NumeroVersao { get; set; }
        protected ObraTaxaVersao() : base() { }

        public ObraTaxaVersao(int usinaCodigo, int obraCodigo, int sequencia, string selecionada, string aprovacaoSolicitante, string aprovada, string aprovacaoUsuario, string aprovacaoCiente) :
            base(usinaCodigo, obraCodigo, sequencia, selecionada, aprovacaoSolicitante, aprovada, aprovacaoUsuario, aprovacaoCiente)
        {
        }

        public ObraTaxaVersao(TaxaExtraVersao taxa, int obraCodigo, string aprovacaoSolicitante, string aprovada, string selecionada, bool isPersonalizada, int numVersao) :
            base(taxa, obraCodigo, aprovacaoSolicitante, aprovada, selecionada, isPersonalizada)
        {
            NumeroVersao = numVersao;
        }

    }
}

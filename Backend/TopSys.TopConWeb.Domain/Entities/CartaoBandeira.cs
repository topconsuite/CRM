using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CartaoBandeira
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string TipoIntegracao { get; set; }
        public int? IntervenienteCodigo { get; set; } = 0;
        public virtual Interveniente Interveniente { get; set; }
        public int? PortadorCodigo { get; set; } = 0;
        public virtual Portador Portador { get; set; }
        public string EstabelecimentoCod { get; set; }
        public int EmpresaCod { get; set; }
        public int BancoCod { get; set; }
        public int CentroDeCusto { get; set; }
        public int Situacao { get; set; }
        public int DiasPrimeiraParcela { get; set; }
        public int DiasSegundaParcela { get; set; }
        public int DiasTerceiraParcela { get; set; }
        public int DiasQuartaParcela { get; set; }
        public int DiasQuintaParcela { get; set; }
        public int DiasSextaParcela { get; set; }
        public int DiasSetimaParcela { get; set; }
        public int DiasOitavaParcela { get; set; }
        public int DiasNonaParcela { get; set; }
        public int DiasDecimaParcela { get; set; }
        public int DiasDecimaPrimeiraParcela { get; set; }
        public int DiasDecimaSegundaParcela { get; set; }
        public int DiasFloatDebito { get; set; }
        public string Ativo { get; set; }

        public IList<int> DatasParcelas
        {
            get
            {
                var datasParcelas = new List<int>();
                datasParcelas.Add(DiasPrimeiraParcela);
                datasParcelas.Add(DiasSegundaParcela);
                datasParcelas.Add(DiasTerceiraParcela);
                datasParcelas.Add(DiasQuartaParcela);
                datasParcelas.Add(DiasQuintaParcela);
                datasParcelas.Add(DiasSextaParcela);
                datasParcelas.Add(DiasSetimaParcela);
                datasParcelas.Add(DiasOitavaParcela);
                datasParcelas.Add(DiasNonaParcela);
                datasParcelas.Add(DiasDecimaParcela);
                datasParcelas.Add(DiasDecimaPrimeiraParcela);
                datasParcelas.Add(DiasDecimaSegundaParcela);
                return datasParcelas;
            }   
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalFisicaIndicadorPontos : IQueryResult
    {
        public long Numero { get; set; }

        public string Serie { get; set; }

        public DateTime? DataRemessa { get; set; }

        public int UsinaPesagem { get; set; }

        public string IntervenienteNome { get; set; } = "";

        public string IntervenienteCpfCnpj { get; set; }

        public float TracoValorTotal { get; set; }

        public int IndicadorTipo { get; set; }

        public string IndicadorCpfCnpj { get; set; }

        public string IndicadorNome { get; set; }

        public string IndicadorPontos { get; set; }

    }
}

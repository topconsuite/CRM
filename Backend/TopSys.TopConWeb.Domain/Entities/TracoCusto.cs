using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TracoCusto
    {
        public int UsinaCodigo { get; set; }

        public string TracoEspecificacao { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public int UsoCodigo { get; set; }

        public int ResistenciaTipoCodigo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public int PedraCodigo { get; set; }

        public int SlumpCodigo { get; set; }

        public float CustoPuro { get; set; }

        public float CustoAjustado { get; set; }

        public DateTime? DataCalculoCusto { get; set; }

        public float CustoRecalculado { get; set; }

        public DateTime? DataRecalculoCusto { get; set; }

        public float PercentalVariacao { get; set; }

        public string Ativo { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }

        public float ValorServico { get; set; }

        public float ValorOutrosCustos { get; set; }

        public float ValorMarkup { get; set; }
    }
}

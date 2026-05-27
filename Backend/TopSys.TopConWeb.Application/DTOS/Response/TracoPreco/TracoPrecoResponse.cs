using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Response.TracoPreco
{
    public class TracoPrecoResponse
    {
        public int NumeroTabela { get; set; }
        
        public virtual UsinaDTO UsinaBase { get; set; }
        
        public virtual VendedorDTO VendedorRepresentante { get; set; }
        
        public virtual UsoDTO Uso { get; set; }
        
        public virtual ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }
        
        public virtual PedraDTO Pedra { get; set; }
        
        public virtual SlumpDTO Slump { get; set; }

        public float M3Preco { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public DateTime? DataFinalVigencia { get; set; }

        public float M3PrecoRecalculo { get; set; }

        public float PercentualVariacao { get; set; }

        public DateTime? DataRecalculo { get; set; }

       public float CustoMaterial { get; set; }

        public float Markup { get; set; }

        public string TracoEspecificacao { get; set; }
        
        public virtual UsinaDTO UsinaReferencia { get; set; }

        public int NumeracaoProduto { get; set; }

        public int StatusTraco { get; set; }
    }
}

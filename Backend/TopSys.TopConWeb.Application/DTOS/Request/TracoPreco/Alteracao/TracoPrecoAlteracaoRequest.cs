using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.TracoPreco.Alteracao
{
    public class TracoPrecoAlteracaoRequest
    {

        public int NumeroTabela { get; set; }

        public virtual Usina UsinaBase { get; set; }

        public virtual Uso Uso { get; set; }

        public virtual ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public virtual Pedra Pedra { get; set; }

        public virtual SlumpReal Slump { get; set; }

        public float M3Preco { get; set; }

        public float M3PrecoRecalculo { get; set; }
        public float CustoMaterial { get; set; }

        public float Markup { get; set; }


    }
}

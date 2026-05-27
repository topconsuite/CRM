using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoTracoReajusteBase<TContrato>
    {
        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }

        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public DateTime DataVigencia { get; set; }

        public int ObraTracoSequencia { get; set; }

        //tp_resist
        public int? ResistenciaTipoCodigo { get; set; } = 0;

        public virtual ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        //uso
        public int? UsoCodigo { get; set; } = 0;

        public virtual Uso Uso { get; set; }

        //pedra
        public int? PedraCodigo { get; set; } = 0;

        public virtual Pedra Pedra { get; set; }

        //slump
        public int? SlumpCodigo { get; set; } = 0;

        public virtual Slump Slump { get; set; }

        public float PrecoVigente { get; set; }

        public float CustoVigente { get; set; }

        public float ValorServicoVigente { get; set; }

        public float PrecoRecalculado { get; set; }

        public float CustoRecalculado { get; set; }

        public float ValorServicoRecalculado { get; set; }

        public float PorcentagemReajuste { get; set; }

        public string EmiteCartaSimNao { get; set; }

        public int UsinaEntregaCodigo { get; set; }
        public virtual Usina UsinaEntrega { get; set; }

        public DateTime? DataCarta { get; set; }

        //id_cadast
        public string IdCadastro { get; set; }

        //id_atual
        public string IdAtualizacao { get; set; }

        public DateTime? DataConfirmacao { get; set; }

        public DateTime? DataCalculo { get; set; }

        public int NumeroTabela { get; set; }

        public virtual TContrato Contrato { get; set; }
        public virtual Obra Obra { get; set; }
        public string IdAprovacaoVersao { get; set; }
        public string IdReprovacao { get; set; }
    }
}

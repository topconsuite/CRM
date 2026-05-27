using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoBombaReajusteBase<TContrato>
    {
        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }

        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public DateTime DataVigencia { get; set; }

        public int ObraBombaReajusteSequencia { get; set; }
        public CadastroGeral BombaTipo { get; set; }
        public int? BombaTipoCodigo { get; set; } = 0;

        public float ValorVigente { get; set; }

        public int VigenteAteM3 { get; set; }

        public float M3ExcedenteVigente { get; set; }

        public float ValorReajustado { get; set; }

        public int ReajustadoAteM3 { get; set; }

        public float M3ExcedenteReajustado { get; set; }
        public DateTime? DataCarta { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public string EmiteCartaSimNao { get; set; }
        public virtual TContrato Contrato { get; set; }
        public virtual Obra Obra { get; set; }
        public string IdAtualizacao { get; set; }
        public string IdAprovacaoVersao { get; set; }
        public string IdReprovacao { get; set; }
    }
}

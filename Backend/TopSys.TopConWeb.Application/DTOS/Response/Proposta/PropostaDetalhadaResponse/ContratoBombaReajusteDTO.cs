using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class ContratoBombaReajusteDTO
    {
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public DateTime DataVigencia { get; set; }

        public int ObraBombaReajusteSequencia { get; set; }

        public int? BombaTipoCodigo { get; set; } = 0;

        public float ValorReajustado { get; set; }

        public float ReajustadoAteM3 { get; set; }

        public float M3ExcedenteReajustado { get; set; }
    }
}

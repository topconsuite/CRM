using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaImportacaoSimplesResponse
{
    public class PropostaImportacaoSimplesResponse
    {
        public UsinaDTO Usina;
        public int Ano;
        public int Numero;
        public DateTime Data;
        public ObraDTO Obra;

        public int IntervenienteCodigo { get; set; }
        public string IntervenienteRazao { get; set; }

        public float ValorTotalContrato { get; set; }

        public float VolumeTotal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class CondicaoPagamentoDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public float MediaDias { get; set; }
    }
}

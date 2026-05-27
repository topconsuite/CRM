using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento
{
    public class CondicaoPagamentoParcelaDTO
    {
        public int CondicaoPagamentoCodigo { get; set; }
        public int Dias { get; set; }
        public float Percentual { get; set; }
    }
}

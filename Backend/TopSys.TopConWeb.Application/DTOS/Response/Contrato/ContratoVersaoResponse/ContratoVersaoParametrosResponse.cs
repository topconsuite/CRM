using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato
{
    public class ContratoVersaoParametrosResponse
    {
        public bool VersionamentoTraco { get; set; }
        public bool VersionamentoBomba { get; set; }
        public bool VersionamentoTaxaExtra { get; set; }
        public bool VersionamentoCondicaoPagamento { get; set; }
        public bool VersionamentoEnderecoObra { get; set; }
        public bool VersionamentoDemaisServicos { get; set; }
        public bool VersionamentoReajusteContrato { get; set; }
    }
}

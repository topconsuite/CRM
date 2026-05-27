using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Filial
{
    public class FilialResponse
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        public string PermiteDocumentoDiferentePadraoRemessa { get; set; }

        public string PermiteDocumentoDiferentePadraoBomba { get; set; }
    }
}

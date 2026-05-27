using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.TipoCobranca
{
    public class ContaDTO
    {
        public int EmpresaCodigo { get; set; }

        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Razao { get; set; }

        public int BancoCodigoOficial { get; set; }

        public int NumeroAgencia { get; set; }

        public long NumeroConta { get; set; }

        public string DvAgencia { get; set; }

        public string DvConta { get; set; }
    }
}

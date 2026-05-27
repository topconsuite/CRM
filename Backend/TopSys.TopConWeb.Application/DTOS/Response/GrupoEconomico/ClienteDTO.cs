using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.GrupoEconomico
{
    public class ClienteDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }
        public string CpfCnpj { get; set; }
    }
}

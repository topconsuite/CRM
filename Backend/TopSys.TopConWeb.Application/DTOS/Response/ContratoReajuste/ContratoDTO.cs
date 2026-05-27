using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste
{
    public class ContratoDTO
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public int IntervenienteCodigo { get; set; }
        public IntervenienteDTO Interveniente { get; set; }
        public int UsinaEntrega { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracosResponse
{
    public class ContratoDTO
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string FechadoSimNao { get; set; }
        public string IdAprovacaoEngenharia { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public string AprovaEngenharia { get; set; }
        public IntervenienteDTO Interveniente { get; set; }
    }
}

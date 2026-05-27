using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Contrato.AprovarCoincidenciasCadastraisRequest
{
    public class AprovarCoincidenciasCadastraisRequest
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string AguardandoAprovacao { get; set; }
    }
}

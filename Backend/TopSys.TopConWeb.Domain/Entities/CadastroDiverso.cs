using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CadastroDiverso
    {
        public string Aplicativo { get; set; }

        public int ProgramaNumero { get; set; }

        public string ProgramaCampo { get; set; }

        public string Codigo { get; set; }

        public string Descricao { get; set; }
    }
}

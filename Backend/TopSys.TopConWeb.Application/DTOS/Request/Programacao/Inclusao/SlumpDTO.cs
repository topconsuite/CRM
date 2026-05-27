using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Programacao.Inclusao
{
    public class SlumpDTO
    {
        public int Codigo { get; set; }

        public int Variacao { get; set; }

        public int AmplitudeDe { get; set; }

        public string Descricao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class SlumpReal
    {
        public int Codigo { get; set; }

        public int Variacao { get; set; }

        public int AmplitudeDe { get; set; }

        public string Descricao
        {
            get
            {
                return Codigo.ToString() + '±' + Variacao.ToString();
            }
        }
    }
}

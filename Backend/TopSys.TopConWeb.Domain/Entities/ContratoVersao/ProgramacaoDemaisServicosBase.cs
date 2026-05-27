using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ProgramacaoDemaisServicosBase
    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int ProgramacaoSequencia { get; set; }

        public int Sequencia { get; set; }

        public float Quantidade { get; set; }

        public float ValorTotal { get; set; }

        public float ValorCobrado { get; set; }
    }
}

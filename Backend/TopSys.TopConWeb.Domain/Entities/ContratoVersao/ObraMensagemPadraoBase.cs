using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraMensagemPadraoBase
    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int MensagemPadraoCodigo { get; set; }
        public virtual MensagemPadrao MensagemPadrao { get; set; }

        public string SelecionadoSimNao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraReajusteBase<TObra>
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public string MensagemReajuste { get; set; }

        public TObra Obra { get; set; }
    }
}

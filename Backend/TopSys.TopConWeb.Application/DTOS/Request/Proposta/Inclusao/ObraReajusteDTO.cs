using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao
{
    public class ObraReajusteDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public string MensagemReajuste { get; set; }
    }
}

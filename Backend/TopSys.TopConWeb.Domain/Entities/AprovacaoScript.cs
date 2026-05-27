using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class AprovacaoScript
    {
        public string Chave { get; set; }
        
        public string OperacaoTipo { get; set; }

        public string Script { get; set; }

        public string Status { get; set; }

        public void Executar()
        {
            this.Status = "E";
        }
    }
}

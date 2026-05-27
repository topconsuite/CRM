using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class LogGeral
    {
        public long Id { get; set; }

        public int Local { get; set; }

        public DateTime Data { get; set; }

        public DateTime? Hora { get; set; }

        public string Usuario { get; set; }

        public string Tabela { get; set; }

        public string Script { get; set; }

        public int AtualizouServidor { get; set; }
    }
}

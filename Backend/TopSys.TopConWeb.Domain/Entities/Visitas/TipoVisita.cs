using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TipoVisita
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public bool Ativo{ get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }
    }
}

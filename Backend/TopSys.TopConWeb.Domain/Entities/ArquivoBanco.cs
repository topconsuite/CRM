using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ArquivoBanco
    {

        public Guid Id { get; set; }
        
        public string Aplicacao { get; set; }

        public int Programa { get; set; }

        public string Chave { get; set; }
        public int Sequencia { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }

    }
}

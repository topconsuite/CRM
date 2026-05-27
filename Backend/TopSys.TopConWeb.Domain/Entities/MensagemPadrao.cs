using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class MensagemPadrao
    {
        public int Codigo { get; set; }

        public int ProdutoCodigo { get; set; }

        public string Mensagem { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }
    }
}

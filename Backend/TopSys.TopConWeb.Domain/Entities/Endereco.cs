using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Endereco
    {
        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public int MunicipioCodigo { get; set; }

        public virtual Municipio Municipio { get; set; }

        public bool IsConfiavel { get; set; }
    }
}

using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs
{
    public class ViaCepEndereco
    {
        public bool Erro { get; set; }

        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        [DeserializeAs(Name = "localidade")]
        public string MunicipioNome { get; set; }

        [DeserializeAs(Name = "uf")]
        public string MunicipioUf { get; set; }

        public string Unidade { get; set; }

        [DeserializeAs(Name = "ibge")]
        public int MunicipioIbgeCodigo { get; set; }

        public string Gia { get; set; }
    }
}

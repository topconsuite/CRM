using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse
{
    public class VendedorDTO
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        [JsonProperty(PropertyName = "telefoneDdd")]
        public int DDDCelular { get; set; }

        [JsonProperty(PropertyName = "telefoneNumero")]
        public int Celular { get; set; }

        public string CpfCnpj { get; set; }
    }
}

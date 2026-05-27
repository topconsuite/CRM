using TopSys.TopConWeb.Application.DTOS.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Endereco
{
    public class EnderecoResponse
    {
        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public virtual MunicipioDTO Municipio { get; set; }
    }
}

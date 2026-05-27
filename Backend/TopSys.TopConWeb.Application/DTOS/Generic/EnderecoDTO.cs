namespace TopSys.TopConWeb.Application.DTOS.Generic
{
    public class EnderecoDTO
    {
        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public virtual MunicipioDTO Municipio { get; set; }
    }
}

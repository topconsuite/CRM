using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraFrenteResponse
{
    public class ObraFrenteResponse
    {
        
        public Guid ID { get; set; }

        public int UsinaCodigo { get; set; }
        public int ObraCodigo { get; set; }
        public int ObraSequencia { get; set; }

        public string EnderecoNome { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }

    }
}

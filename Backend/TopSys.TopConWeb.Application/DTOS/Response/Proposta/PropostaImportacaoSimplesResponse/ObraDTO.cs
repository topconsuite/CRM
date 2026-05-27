using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaImportacaoSimplesResponse
{
    public class ObraDTO : IHasEnderecoDTO
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public string Nome { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }

        public TipoCobrancaDTO TipoCobranca { get; set; }

        public ICollection<ObraTributacaoMunicipalDTO> ObraTributacoesMunicipais { get; set; }
    }
}

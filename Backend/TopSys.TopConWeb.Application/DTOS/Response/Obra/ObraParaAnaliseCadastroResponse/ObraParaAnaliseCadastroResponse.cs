using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse
{
    public class ObraParaAnaliseCadastroResponse : IHasEnderecoDTO
    {
        public int UsinaCodigo { get; set; }
        public UsinaDTO UsinaEntrega { get; set; }
        public int Numero { get; set; }
        public bool PendenteAprovacaoDistanciaUsinaCEP { get; set; }
        public int DistanciaUsina { get; set; }
        public ContratoDTO Contrato { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public TipoCobrancaDTO TipoCobranca { get; set; }
        public EObraStatusCadastro StatusCadastro { get; set; }

        public string ObservacaoNf { get; set; }
        public string Cei { get; set; }
        public ICollection<ObraTributacaoMunicipalDTO> ObraTributacoesMunicipais { get; set; }
        public ICollection<ObraBombaDTO> ObraBombas { get; set; }
        public string NecessitaAprovacaoZMRC { get; set; }
    }
}

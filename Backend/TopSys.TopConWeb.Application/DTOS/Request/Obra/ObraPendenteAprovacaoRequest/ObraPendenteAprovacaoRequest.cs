using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest
{
    public class ObraPendenteAprovacaoRequest
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public string Nome { get; set; }
        
        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        public string EnderecoCep { get; set; }

        public string EnderecoLogradouro { get; set; }

        public int EnderecoNumero { get; set; }

        public string EnderecoComplemento { get; set; }

        public string EnderecoBairro { get; set; }
        
        public MunicipioDTO EnderecoMunicipio { get; set; }
       
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }

        public CadastroGeralDTO TipoCobranca { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public ICollection<ObraTracoDTO> ObraTracos { get; set; }

        public ICollection<ObraBombaDTO> ObraBombas { get; set; }

        public ICollection<ObraTaxaDTO> ObraTaxas { get; set; }

        public ICollection<DemaisAprovacaoDTO> DemaisAprovacoes { get; set; }
        
        public ICollection<ObraLogDTO> ObraLogs { get; set; }
        public EObraDemaisStatusComercial VolumeStatusComercial { get; set; }
        public EObraDemaisStatusComercial CondicaoPagamentoStatusComercial { get; set; }

    }
}

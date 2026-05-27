using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Oportunidade
{
    public class OportunidadeResponse : IHasEnderecoDTO
    {

        public virtual UsinaDTO Usina { get; set; }
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }

        public int NumeroLead { get; set; }
        public int AnoLead { get; set; }

        public int NumeroVisita { get; set; }
        public int AnoVisita { get; set; }

        public DateTime Data { get; set; }
        public string OportunidadeNome { get; set; }
        public int IntervenienteCodigo { get; set; }
        public string Cliente { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public VendedorDTO Vendedor { get; set; }
        public int VendedorCodigo { get; set; }
        public SegmentacaoDTO Segmentacao { get; set; }
        public int SegmentacaoCodigo { get; set; }
        public OportunidadeTipoDTO OportunidadeTipo { get; set; }
        public int OportunidadeTipoCodigo { get; set; }
        public CadastroGeralDTO ViaCaptacao { get; set; }
        public int ViaCaptacaoCodigo { get; set; }
        public OportunidadeFaseDTO Fase { get; set; }
        public int FaseCodigo { get; set; }
        public EClassificacaoTemperatura Classificacao { get; set; }
        public string ProximaEtapa { get; set; }
        public DateTime? PrevisaoFechamento { get; set; }
        public MotivoPerdaDTO MotivoPerda { get; set; }
        public int MotivoPerdaCodigo { get; set; }
        public ConcorrenteDTO Concorrente { get; set; }
        public int ConcorrenteCodigo { get; set; }
        public string ObservacaoInterna { get; set; }
        public string ObraNome { get; set; }
        public virtual CadastroGeralDTO PorteObra { get; set; }
        public int? PorteObraCodigo { get; set; } = 0;
        public EObraFase ObraFase { get; set; }
        public float VolumeEstimadoObra { get; set; }
        public float ValorEstimadoObra { get; set; }
        public DateTime? PrevisaoInicio { get; set; }
        public DateTime? PrevisaoTermino { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public UsinaDTO UsinaEntrega { get; set; }
        public int UsinaEntregaCodigo { get; set; }
        public float DistanciaUsina { get; set; }
        public string ReferenciaAcesso { get; set; }
        public OportunidadeContatoDTO ContatoPrincipal { get; set; }
        public OportunidadeContatoDTO ContatoSecundario { get; set; }
        public ICollection<OportunidadeLogDTO> Logs { get; set; }

        public ICollection<PropostaDTO> Propostas { get; set; }

    }
}

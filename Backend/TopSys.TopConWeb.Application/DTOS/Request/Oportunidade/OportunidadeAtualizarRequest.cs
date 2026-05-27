using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Oportunidade
{
    public class OportunidadeAtualizarRequest
    {

        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public int Ano { get; set; }

        public string OportunidadeNome { get; set; }
        public int IntervenienteCodigo { get; set; }
        public string Cliente { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public int VendedorCodigo { get; set; }
        public int SegmentacaoCodigo { get; set; }
        public int OportunidadeTipoCodigo { get; set; }
        public int ViaCaptacaoCodigo { get; set; }
        public int FaseCodigo { get; set; }
        public EClassificacaoTemperatura Classificacao { get; set; }
        public string ProximaEtapa { get; set; }
        public DateTime? PrevisaoFechamento { get; set; }
        public int MotivoPerdaCodigo { get; set; }
        public int ConcorrenteCodigo { get; set; }
        public string ObservacaoInterna { get; set; }
        public string ObraNome { get; set; }
        public int? PorteObraCodigo { get; set; } = 0;
        public EObraFase ObraFase { get; set; }
        public float VolumeEstimadoObra { get; set; }
        public float ValorEstimadoObra { get; set; }
        public DateTime? PrevisaoInicio { get; set; }
        public DateTime? PrevisaoTermino { get; set; }
        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; }
        public int UsinaEntregaCodigo { get; set; }
        public int DistanciaUsina { get; set; }
        public string ReferenciaAcesso { get; set; }
        public OportunidadeContatoAtualizarRequest ContatoPrincipal { get; set; }
        public OportunidadeContatoAtualizarRequest ContatoSecundario { get; set; }

    }
}

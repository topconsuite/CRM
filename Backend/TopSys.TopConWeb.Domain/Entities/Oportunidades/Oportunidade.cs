using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities.Oportunidades
{
    public class Oportunidade : IHasEndereco
    {
        public virtual Usina Usina { get; set; }
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
        public Vendedor Vendedor { get; set; }
        public int VendedorCodigo { get; set; }
        public Segmentacao Segmentacao { get; set; }
        public int SegmentacaoCodigo { get; set; }
        public OportunidadeTipo OportunidadeTipo { get; set; }
        public int OportunidadeTipoCodigo { get; set; }
        public CadastroGeral ViaCaptacao { get; set; }
        public int ViaCaptacaoCodigo { get; set; }
        public OportunidadeFase Fase { get; set; }
        public int FaseCodigo { get; set; }
        public EClassificacaoTemperatura Classificacao { get; set; }
        public string ProximaEtapa { get; set; }
        public DateTime? PrevisaoFechamento { get; set; }
        public MotivoPerda MotivoPerda { get; set; }
        public int? MotivoPerdaCodigo { get; set; }
        public Concorrente Concorrente { get; set; }
        public int? ConcorrenteCodigo { get; set; }
        public string ObservacaoInterna { get; set; }
        public string ObraNome { get; set; }
        public virtual CadastroGeral PorteObra { get; set; }
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
        public Municipio EnderecoMunicipio { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; }
        public Usina UsinaEntrega { get; set; }
        public int? UsinaEntregaCodigo { get; set; }
        public int DistanciaUsina { get; set; }
        public string ReferenciaAcesso { get; set; }
        public OportunidadeContato ContatoPrincipal { get; set; }
        public OportunidadeContato ContatoSecundario { get; set; }

        public ICollection<OportunidadeLog> Logs { get; set; }
        public ICollection<Proposta> Propostas { get; set; }

    }
}

using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraFrenteDTO;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecaoDTO;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class ObraDTO : IHasEnderecoDTO
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public int? AnoContrato { get; set; } = 0;
        public int? NumContrato { get; set; } = 0;

        public string Nome { get; set; }

        public int DistanciaUsina { get; set; }

        public int DistanciaUsinaGoogleApi { get; set; } = 0;

        public DateTime? PrevisaoInicio { get; set; }

        public DateTime? PrevisaoTermino { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }

        public TipoCobrancaDTO TipoCobranca { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public ICollection<ObraFrenteDTO> ObraFrentes { get; set; }

        public ICollection<ObraProjecaoDTO> ObraProjecao { get; set; }

        public ICollection<ObraTracoDTO> ObraTracos { get; set; }

        public ICollection<ObraBombaDTO> ObraBombas { get; set; }

        public ICollection<ObraTaxaDTO> ObraTaxas { get; set; }

        public ICollection<ObraLogDTO> ObraLogs { get; set; }

        public ICollection<ObraPagamentoDTO> ObraPagamentos { get; set; }

        public ICollection<ObraTributacaoMunicipalDTO> ObraTributacoesMunicipais { get; set; }

        public ICollection<ObraDemaisServicosDTO> ObraDemaisServicos { get; set; }

        public ObraReajusteDTO ObraReajuste { get; set; }

        public string RadioNextel { get; set; }

        public string ContatoPrincipalNome { get; set; }
        public CadastroGeralDTO ContatoPrincipalFuncao { get; set; }
        public int ContatoPrincipalTelefoneDdd { get; set; }
        public int ContatoPrincipalTelefoneNumero { get; set; }
        public int ContatoPrincipalCelularDdd { get; set; }
        public int ContatoPrincipalCelularNumero { get; set; }
        public ObraIndicadorDTO Indicador { get; set; }

        public string ContatoSecundarioNome { get; set; }
        public CadastroGeralDTO ContatoSecundarioFuncao { get; set; }
        public int ContatoSecundarioTelefoneDdd { get; set; }
        public int ContatoSecundarioTelefoneNumero { get; set; }
        public int ContatoSecundarioCelularDdd { get; set; }
        public int ContatoSecundarioCelularNumero { get; set; }

        public CadastroGeralDTO ViaCaptacao { get; set; }

        public CadastroGeralDTO TipoObra { get; set; }

        public CadastroGeralDTO PorteObra { get; set; }

        public string Email { get; set; }

        public string ObservacaoNf { get; set; }

        public string Cei { get; set; }

        public string CodigoCib { get; set; }

        public EConstrucaoCivilTipoAlvara ConstrucaoCivilTipoAlvara { get; set; }

        public TributacaoPisCofinsDTO TributacaoPisCofins { get; set; }

        public TributacaoReformaDTO TributacaoIS { get; set; }
        public TributacaoReformaDTO TributacaoIBS { get; set; }
        public TributacaoReformaDTO TributacaoCBS { get; set; }

        public string ReferenciaAcesso { get; set; }

        public float VolumeEstimado { get; set; }

        public float VolumePorCarga { get; set; }

        public int VibradorQuantidade { get; set; }

        public float VibradorValorUnitario { get; set; }

        public string ObservacaoInterna { get; set; }

        public int TempoBtNaObra { get; set; }

        public int TempoAteAObra { get; set; }

        public int TempoDescarga { get; set; }

        public string CodigoBeneficioFiscal { get; set; }

        public string EmailResponsavelTecnico { get; set; }

        public ContratoDTO Contrato { get; set; }

        public string UsaAdicionalZMRC { get; set; }

        public string NecessitaAprovacaoZMRC { get; set; }
    }
}

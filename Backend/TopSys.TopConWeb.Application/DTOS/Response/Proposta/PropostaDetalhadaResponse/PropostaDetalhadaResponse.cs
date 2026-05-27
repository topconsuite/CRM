using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Response.Segmentacao;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class PropostaDetalhadaResponse : IHasEnderecoDTO
    {
        public UsinaDTO Usina;
        public int Ano;
        public int Numero;
        public DateTime Data;
        public VendedorDTO Vendedor;
        public VendedorDTO VendedorPadrinho;
        public IntervenienteDTO Interveniente;
        public ObraDTO Obra;

        public int IntervenienteCodigo { get; set; }

        public string IntervenienteRazao { get; set; }

        public int TracoPrecoNumeroTabela { get; set; }

        public string Contato { get; set; }

        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }

        public int Ramal { get; set; }

        public int CelularDdd { get; set; }
        public int CelularNumero { get; set; }
        
        public EnderecoDTO Endereco { get; set; }

        public string Email { get; set; }
        public string EmailCobranca { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }

        public string Observacao { get; set; }

        public string IntervenienteTipo { get; set; }

        public float ValorConcreto { get; set; }

        public float ValorBomba { get; set; }

        public float ValorExtras { get; set; }

        public float ValorTotalContrato { get; set; }

        public float VolumeTotal { get; set; }

        public string IdEmissao { get; set; }

        public string NomeMae { get; set; }

        public string NomeConjuge { get; set; }

        public EPropostaStatus Status { get; set; }

        public string CpfCnpj { get; set; }

        public string IntervenienteNome { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoMunicipal { get; set; }

        public string Rg { get; set; }

        public string OrgaoExpedidor { get; set; }

        public string Profissao { get; set; }

        public string EmpresaTrabalho { get; set; }

        public int TelefoneComercialDdd { get; set; }

        public int TelefoneComercialNumero { get; set; }

        public string CodigoObraPrefeitura { get; set; }

        public string FaturamentoAosCuidados { get; set; }

        public string IntervenienteIdExterno { get; set; }

        public int ModeloDocumentoRemessaConcreto { get; set; }

        public int ModeloDocumentoRemessaBomba { get; set; }
        public int ModeloItensDanfeERomaneio { get; set; }

        public PropostaCobrancaDTO Cobranca { get; set; }
        public PropostaFaturamentoDTO Faturamento { get; set; }
        public PropostaResponsavelSolidarioDTO ResponsavelSolidario { get; set; }

        public bool UtilizaResponsavelSolidario { get; set; }

        public bool UtilizaDadosFaturamento { get; set; }
        public bool UtilizaEnderecoFaturamento { get; set; }

        public bool UtilizaDadosCobranca { get; set; }
        public bool UtilizaEnderecoCobranca { get; set; }

        public int OrigemUsinaCodigo { get; set; }
        public int OrigemObraCodigo { get; set; }

        public DateTime? DataEncerramentoContrato { get; set; }

        public bool IsContratoEncerrado { get; set; }

        public bool IsContratoFechado { get; set; }

        public int? Segmentacao { get; set; }
        public SegmentacaoResponse Segmento { get; set; }

        public EPropostaStatus StatusAnterior { get; set; }

        public EPropostaStatusCliente StatusProposta { get; set; }
        public EObraStatusComercial StatusComercial { get; set; }
        public EContratoStatus StatusContrato { get; set; }
        public string CondicoesGerais { get; set; }
        public DateTime? ValidadeProposta { get; set; }
        public DateTime? DataUltimaVersaoGerada { get; set; }

        public int AnoVisita { get; set; }

        public int NumeroVisita { get; set; }

        public int AnoLead { get; set; }

        public int NumeroLead { get; set; }

        public int AnoOportunidade { get; set; }

        public int NumeroOportunidade { get; set; }

        public EContratoFinalidade ContratoFinalidade { get; set; }

        public string AprovacaoMedicao { get; set; }
        public int TempoAprovacaoMedicao { get; set; }
    }
}

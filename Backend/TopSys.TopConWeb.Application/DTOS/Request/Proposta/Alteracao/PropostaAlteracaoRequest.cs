using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class PropostaAlteracaoRequest : IHasEnderecoDTO
    {
        public UsinaDTO Usina { get; set; }

        public int Ano { get; set; }

        public int Numero { get; set; }

        public DateTime Data { get; set; }

        public VendedorDTO Vendedor { get; set; }

        public VendedorDTO VendedorPadrinho { get; set; }

        public IntervenienteDTO Interveniente { get; set; }

        public ObraDTO Obra { get; set; }

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

        public bool UtilizaResponsavelSolidario { get; set; }

        public bool UtilizaDadosCobranca { get; set; }

        public bool UtilizaDadosFaturamento { get; set; }

        public bool UtilizaEnderecoCobranca { get; set; }

        public bool UtilizaEnderecoFaturamento { get; set; }

        public int ModeloDocumentoRemessaConcreto { get; set; }

        public int ModeloDocumentoRemessaBomba { get; set; }
        public int ModeloItensDanfeERomaneio { get; set; }

        public PropostaCobrancaDTO Cobranca { get; set; }
        public PropostaFaturamentoDTO Faturamento { get; set; }
        public PropostaResponsavelSolidarioDTO ResponsavelSolidario { get; set; }

        public EPropostaStatusCliente StatusProposta { get; set; }
        public EObraStatusComercial StatusComercial { get; set; }
        public EContratoStatus StatusContrato { get; set; }

        public string CondicoesGerais { get; set; }
        public DateTime? ValidadeProposta { get; set; }

        public EContratoFinalidade ContratoFinalidade { get; set; }

        public int Segmentacao { get; set; }

        public string Origem { get; set; }

        public string AprovacaoMedicao { get; set; }
        public int TempoAprovacaoMedicao { get; set; }
    }
}

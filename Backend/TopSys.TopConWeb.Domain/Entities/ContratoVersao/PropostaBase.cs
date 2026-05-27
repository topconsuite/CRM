using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class  PropostaBase<TProposta, TObra, TContrato, TPropostaCobranca, TPropostaFaturamento, TPropostaResponsavelSolidario, TObraTraco, TObraBomba, TContratoTracoReajuste, TContratoBombaReajuste> : IHasEndereco, IHasInterveniente, IHasIntervenienteRazao, IHasVendedor
        where TPropostaCobranca : IPropostaDadosPessoaisBase<TProposta>
        where TPropostaFaturamento : IPropostaDadosPessoaisBase<TProposta>
        where TPropostaResponsavelSolidario : IPropostaDadosPessoaisBase<TProposta>
        where TObra : IObra<TObra, TContrato, TObraTraco, TObraBomba>
        where TObraTraco : ObraTracoBase<TObra>
        where TObraBomba : ObraBombaBase<TObra>
        where TContrato : ContratoBase<TContratoTracoReajuste, TContratoBombaReajuste>
    {
        public virtual Usina Usina { get; set; }

        public int UsinaCodigo { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public int? IntervenienteCodigo { get; set; } = 0;

        public virtual Interveniente Interveniente { get; set; }

        public int? VendedorCodigo { get; set; } = 0;

        public virtual Vendedor Vendedor { get; set; }

        public string IntervenienteRazao { get; set; }

        public int ObraCodigo { get; set; }
        public virtual TObra Obra { get; set; }

        public DateTime Data { get; set; }

        public int Hora { get; set; }

        public int? RepresentanteCodigo { get; set; } = 0;

        public int? VendedorPadrinhoCodigo { get; set; } = 0;
        public virtual Vendedor VendedorPadrinho { get; set; }

        public int TracoPrecoNumeroTabela { get; set; }

        public string Contato { get; set; }

        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }

        public int Ramal { get; set; }

        public int CelularDdd { get; set; }
        public int CelularNumero { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public Municipio EnderecoMunicipio { get; set; }

        public string Email { get; set; }
        public string EmailCobranca { get; set; }

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }

        public string Observacao { get; set; }

        public string IntervenienteTipo { get; set; }

        public decimal ValorConcreto { get; set; }

        public decimal ValorBomba { get; set; }

        public float ValorExtras { get; set; }

        public decimal ValorTotalContrato { get; set; }

        public float VolumeTotal { get; set; }

        public string IdEmissao { get; set; }

        public string NomeMae { get; set; }

        public int ModeloDocumentoRemessaConcreto { get; set; }

        public int ModeloDocumentoRemessaBomba { get; set; }
        public int ModeloItensDanfeERomaneio { get; set; }

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

        public int Segmentacao { get; set; }

        public virtual Segmentacao Segmento { get; set; }

        public int TelefoneComercialDdd { get; set; }

        public int TelefoneComercialNumero { get; set; }

        public string CodigoObraPrefeitura { get; set; }

        public string FaturamentoAosCuidados { get; set; }

        public DateTime? DataUltimaVersaoGerada { get; set; }

        public int AnoVisita { get; set; }

        public int NumeroVisita { get; set; }

        public int AnoLead { get; set; }

        public int NumeroLead { get; set; }

        public int AnoOportunidade { get; set; }

        public int NumeroOportunidade { get; set; }

        public virtual TPropostaCobranca Cobranca { get; set; }
        public virtual TPropostaFaturamento Faturamento { get; set; }
        public virtual TPropostaResponsavelSolidario ResponsavelSolidario { get; set; }

        public string CondicoesGerais { get; set; }

        public DateTime? ValidadeProposta { get; set; }

        public bool UtilizaResponsavelSolidario
        {
            get { return ResponsavelSolidario != null && !ResponsavelSolidario.CpfCnpj.Equals(CpfCnpj); }
        }

        public bool UtilizaDadosFaturamento
        {
            get { return !$"{Faturamento?.CpfCnpj}".Equals("") && !$"{Faturamento?.CpfCnpj}".Equals(CpfCnpj); }
        }
        public bool UtilizaDadosCobranca
        {
            get { return !$"{Cobranca?.CpfCnpj}".Equals("") && !$"{Cobranca?.CpfCnpj}".Equals(CpfCnpj); }
        }

        private string _enderecoString
        {
            get { return $"{EnderecoCep}{EnderecoLogradouro}{EnderecoNumero}{EnderecoComplemento}"; }
        }
        private string _enderecoFaturamentoString
        {
            get { return $"{Faturamento?.EnderecoCep}{Faturamento?.EnderecoLogradouro}{Faturamento?.EnderecoNumero}{Faturamento?.EnderecoComplemento}"; }
        }
        private string _enderecoCobrancaString
        {
            get { return $"{Cobranca?.EnderecoCep}{Cobranca?.EnderecoLogradouro}{Cobranca?.EnderecoNumero}{Cobranca?.EnderecoComplemento}"; }
        }
        private string _enderecoResponsavelSolidario
        {
            get { return $"{ResponsavelSolidario?.EnderecoCep}{ResponsavelSolidario?.EnderecoLogradouro}{ResponsavelSolidario?.EnderecoNumero}{ResponsavelSolidario?.EnderecoComplemento}"; }
        }
        public bool UtilizaEnderecoFaturamento
        {
            get { return !_enderecoFaturamentoString.Equals("") && !_enderecoFaturamentoString.Equals("0") && !_enderecoFaturamentoString.Equals(_enderecoString); }
        }
        public bool UtilizaEnderecoCobranca
        {
            get { return !_enderecoCobrancaString.Equals("") && !_enderecoCobrancaString.Equals("0") && !_enderecoCobrancaString.Equals(_enderecoString); }
        }

        public EPropostaStatus StatusAnterior { get; set; }

        public int OrigemUsinaCodigo { get; set; }

        public int OrigemObraCodigo { get; set; }

        public int ProdutoTipoCodigo { get; set; }

        public string TemObras { get; set; }

        public string EmissaoPropostaAprovada { get; set; }

        public EContratoFinalidade ContratoFinalidade { get; set; }

        public virtual DateTime? DataEncerramentoContrato
        {
            get
            {
                return Obra?.Contrato?.DataEncerramento;
            }
        }

        public virtual DateTime? FimVigenciaContrato
        {
            get
            {
                return Obra?.Contrato?.FimVigencia;
            }
        }

        public virtual bool IsContratoEncerrado
        {
            get
            {
                return Obra?.Contrato?.DataEncerramento != null;
            }
        }

        public virtual bool IsContratoFechado
        {
            get
            {
                return Obra?.Contrato?.Fechado ?? false;
            }
        }

        /* O status da proposta tá sendo alterado automaticamente */
        public virtual EPropostaStatusCliente StatusProposta
        {
            get
            {
                if (Status == EPropostaStatus.AprovadaPeloCliente || Status == EPropostaStatus.ContratoGerado)
                    return EPropostaStatusCliente.Aprovado;
                if ((Status == EPropostaStatus.AguardandoAprovacaoComercial || Status == EPropostaStatus.AguardandoAprovacaoEngenharia || Status == EPropostaStatus.ReprovadaComercialmente)
                    && (StatusAnterior == EPropostaStatus.AprovadaPeloCliente || StatusAnterior == EPropostaStatus.ContratoGerado))
                    return EPropostaStatusCliente.Aprovado;
                if (Status == EPropostaStatus.Perdida || Status == EPropostaStatus.Reprovada)
                    return EPropostaStatusCliente.Perdido;
                return EPropostaStatusCliente.EmNegociacao;
            }
            set
            {
                if (StatusProposta == value && Status != 0)
                    return;

                switch (value)
                {
                    case EPropostaStatusCliente.EmNegociacao:
                        Status = EPropostaStatus.Andamento;
                        break;
                    case EPropostaStatusCliente.Aprovado:
                        Status = EPropostaStatus.AprovadaPeloCliente;
                        break;
                    case EPropostaStatusCliente.Perdido:
                        Status = EPropostaStatus.Perdida;
                        break;
                    default:
                        break;
                }
            }
        }
        public virtual EObraStatusComercial StatusComercial
        {
            get
            {
                var statusContrato = Obra?.Contrato?.Status ?? EContratoStatus.Inexistente;
                if (Status == EPropostaStatus.AguardandoAprovacaoComercial || statusContrato == EContratoStatus.AguardandoAprovacaoComercial) return EObraStatusComercial.Aguardando;
                if (Status == EPropostaStatus.ReprovadaComercialmente) return EObraStatusComercial.Reprovado;
                return Obra?.StatusComercial ?? EObraStatusComercial.NaoNecessita;
            }
        }
        public virtual EContratoStatus StatusContrato
        {
            get
            {
                return Obra?.Contrato?.Status ?? EContratoStatus.Inexistente;
            }
        }

        public string AprovacaoMedicao { get; set; }
        public int TempoAprovacaoMedicao { get; set; }
    }
}

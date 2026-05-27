using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.Interveniente
{
    public class PublicoIntervenienteResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }

        [JsonProperty(PropertyName = "trade_name")]
        public string Nome { get; set; } = "";

        [JsonProperty(PropertyName = "company_name")]
        public string Razao { get; set; } = "";

        [JsonProperty(PropertyName = "client")]
        public bool Cliente { get; set; } = true;

        [JsonProperty(PropertyName = "supplier")]
        public bool Fornecedor { get; set; } = true;

        [JsonProperty(PropertyName = "carrier")]
        public bool Transportador { get; set; } = false;

        [JsonProperty(PropertyName = "service_provider")]
        public bool PrestadorServico { get; set; } = false;

        [JsonProperty(PropertyName = "public_agency")]
        public bool OrgaoPublico { get; set; } = false;

        [JsonProperty(PropertyName = "others")]
        public bool Outros { get; set; } = false;

        [JsonProperty(PropertyName = "cep_code")]
        public string EnderecoCep { get; set; } = "";

        [JsonProperty(PropertyName = "street_address")]
        public string EnderecoLogradouro { get; set; } = "";

        [JsonProperty(PropertyName = "number")]
        public int? EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "complement")]
        public string EnderecoComplemento { get; set; } = "";

        [JsonProperty(PropertyName = "neighborhood_address")]
        public string EnderecoBairro { get; set; } = "";

        [JsonProperty(PropertyName = "internal_municipal_code")]
        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        [JsonProperty(PropertyName = "cnpj_cpf")]
        public string CpfCnpj { get; set; } = "";

        [JsonProperty(PropertyName = "state_registration")]
        public string InscricaoEstadual { get; set; } = "";

        [JsonProperty(PropertyName = "rg")]
        public string Rg { get; set; } = "";

        [JsonProperty(PropertyName = "municipal_registration")]
        public string InscricaoMunicipal { get; set; } = "";

        [JsonProperty(PropertyName = "cei_number")]
        public string Cei { get; set; } = "";

        [JsonProperty(PropertyName = "ddd_telephone")]
        public int? TelefoneDdd { get; set; } = 0;

        [JsonProperty(PropertyName = "telephone_number")]
        public int? TelefoneNumero { get; set; } = 0;

        [JsonProperty(PropertyName = "telephone_extension")]
        public int? Ramal { get; set; } = 0;

        [JsonProperty(PropertyName = "ddd_cell")]
        public int? CelularDdd { get; set; } = 0;

        [JsonProperty(PropertyName = "cell_number")]
        public int? CelularNumero { get; set; } = 0;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = "";

        [JsonProperty(PropertyName = "billing_email")]
        public string EmailCobranca { get; set; } = "";

        [JsonProperty(PropertyName = "contact")]
        public string Contato { get; set; } = "";

        [JsonProperty(PropertyName = "activity")]
        public int Atividade { get; set; } = 0;

        [JsonProperty(PropertyName = "type_of_billing")]
        public int? TipoCobranca { get; set; } = 0;

        [JsonProperty(PropertyName = "seller")]
        public string VendedorCodigo { get; set; } = "";

        [JsonProperty(PropertyName = "blocked")]
        public int? BloqueioMotivoCodigo { get; set; } = 0;

        [JsonProperty(PropertyName = "credit_limit")]
        public float? LimiteValor { get; set; } = 0;

        [JsonProperty(PropertyName = "discount_allowed")]
        public float? PorcentagemDesconto { get; set; } = 0;

        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; } = "";

        [JsonProperty(PropertyName = "in86")]
        public bool? In86 { get; set; } = false;

        [JsonProperty(PropertyName = "accounting_account_number")]
        public long? ContaContabil { get; set; } = 0;

        [JsonProperty(PropertyName = "pumper")]
        public bool? bombista { get; set; }

        [JsonProperty(PropertyName = "mp_supplier")]
        public bool? FornecedorMp { get; set; }

        [JsonProperty(PropertyName = "region")]
        public int? Regiao { get; set; } = 0;

        [JsonProperty(PropertyName = "route")]
        public int? Rota { get; set; } = 0;

        [JsonProperty(PropertyName = "route_sequence")]
        public int? RotaSequencia { get; set; } = 0;

        [JsonProperty(PropertyName = "transporter")]
        public int? Transp { get; set; } = 0;

        [JsonProperty(PropertyName = "client_type")]
        public string IntervenienteTipo { get; set; } = "J";

        [JsonProperty(PropertyName = "withhold_iss")]
        public int? RetemIss { get; set; } = 0;

        [JsonProperty(PropertyName = "delivery_location")]
        public bool? LocalEntrega { get; set; } = false;

        [JsonProperty(PropertyName = "specification")]
        public string Especificacao { get; set; } = null;

        [JsonProperty(PropertyName = "mother_name")]
        public string NomeMae { get; set; } = "";

        [JsonProperty(PropertyName = "spouse")]
        public string NomeConjuge { get; set; } = "";

        [JsonProperty(PropertyName = "bearer_billing")]
        public int? PortadorCobranca { get; set; } = 0;

        [JsonProperty(PropertyName = "employee")]
        public bool? Funcionario { get; set; } = false;

        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; } = "";

        [JsonProperty(PropertyName = "engineering_approval")]
        public bool? AprovacaoEngenharia { get; set; } = false;

        [JsonProperty(PropertyName = "ddd_commercial")]
        public int? TelefoneComercialDdd { get; set; } = 0;

        [JsonProperty(PropertyName = "telephone_commercial")]
        public int? TelefoneComercialNumero { get; set; } = 0;

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "due_date_credit_limit")]
        public DateTime? LimiteData { get; set; } = null;

        [JsonProperty(PropertyName = "simple_national")]
        public bool? SimplesNacional { get; set; } = false;

        [JsonProperty(PropertyName = "inss_withholding")]
        public bool? RetemInss { get; set; } = false;

        [JsonProperty(PropertyName = "icms_taxpayer")]
        public int? ContribuiIcms { get; set; } = 0;

        [JsonProperty(PropertyName = "inactive")]
        public bool? Inativo { get; set; } = false;

        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; } = "";

        [JsonProperty(PropertyName = "irrf_withholding")]
        public bool? RetemIrrf { get; set; } = false;

        [JsonProperty(PropertyName = "cofins_withholding")]
        public bool? RetemCofins { get; set; } = false;

        [JsonProperty(PropertyName = "pis_withholding")]
        public bool? RetemPis { get; set; } = false;

        [JsonProperty(PropertyName = "csll_withholding")]
        public bool? RetemCsll { get; set; } = false;

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "update_date")]
        public DateTime? DataAtualizacao { get; set; } = null;
    }
}

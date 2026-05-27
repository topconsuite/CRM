using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Interveniente
{
    [AtLeastOnePropertyRequired]
    public class IntervenienteAtualizarRequest
    {

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Trade_Name" + "::20")]
        [JsonProperty(PropertyName = "trade_name")]
        public string Nome { get; set; } 

        [StringLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Company_Name" + "::100")]
        [JsonProperty(PropertyName = "company_name")]
        public string Razao { get; set; }

        [JsonProperty(PropertyName = "client")]
        public bool? Cliente { get; set; } = null;

        [JsonProperty(PropertyName = "supplier")]
        public bool? Fornecedor { get; set; } = null;

        [JsonProperty(PropertyName = "carrier")]
        public bool? Transportador { get; set; } = null;

        [JsonProperty(PropertyName = "service_provider")]
        public bool? PrestadorServico { get; set; } = null;

        [JsonProperty(PropertyName = "public_agency")]
        public bool? OrgaoPublico { get; set; } = null;

        [JsonProperty(PropertyName = "others")]
        public bool? Outros { get; set; } = null;

        [StringLength(8, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Cep_Code" + "::8")]
        [JsonProperty(PropertyName = "cep_code")]
        public string EnderecoCep { get; set; } 

        [StringLength(40, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Street_Address" + "::40")]
        [JsonProperty(PropertyName = "street_address")]
        public string EnderecoLogradouro { get; set; } 

        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Number" + "::6")]
        [JsonProperty(PropertyName = "number")]
        public int? EnderecoNumero { get; set; }

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Complement" + "::20")]
        [JsonProperty(PropertyName = "complement")]
        public string EnderecoComplemento { get; set; } 

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Neighborhood_Address" + "::20")]
        [JsonProperty(PropertyName = "neighborhood_address")]
        public string EnderecoBairro { get; set; }

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Internal_Municipal_Code" + "::4")]
        [JsonProperty(PropertyName = "internal_municipal_code")]
        public int? EnderecoMunicipioCodigo { get; set; } = null;

        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = nameof(EResourcesInterveniente.INTERVENIENTE_ERROR_TCON385F315F32))]
        [JsonProperty(PropertyName = "cnpj_cpf")]
        public string CpfCnpj { get; set; } 

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::State_Registration" + "::20")]
        [JsonProperty(PropertyName = "state_registration")]
        public string InscricaoEstadual { get; set; }

        [StringLength(15, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Rg" + "::15")]
        [JsonProperty(PropertyName = "rg")]
        public string Rg { get; set; } 

        [StringLength(15, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Municipal_Registration" + "::15")]
        [JsonProperty(PropertyName = "municipal_registration")]
        public string InscricaoMunicipal { get; set; } 

        [StringLength(18, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Cei_Number" + "::18")]
        [JsonProperty(PropertyName = "cei_number")]
        public string Cei { get; set; } 

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Ddd_Telephone" + "::2")]
        [JsonProperty(PropertyName = "ddd_telephone")]
        public int? TelefoneDdd { get; set; } = null;

        [Range(0, 999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Telephone_Number" + "::9")]
        [JsonProperty(PropertyName = "telephone_number")]
        public int? TelefoneNumero { get; set; } = null;

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Telephone_Extension" + "::4")]
        [JsonProperty(PropertyName = "telephone_extension")]
        public int? Ramal { get; set; } = null;

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Ddd_Cell" + "::2")]
        [JsonProperty(PropertyName = "ddd_cell")]
        public int? CelularDdd { get; set; } = null;

        [Range(0, 999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Internal_Municipal_Code" + "::9")]
        [JsonProperty(PropertyName = "cell_number")]
        public int? CelularNumero { get; set; } = null;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = null;

        [JsonProperty(PropertyName = "billing_email")]
        public string EmailCobranca { get; set; } 

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Contact" + "::20")]
        [JsonProperty(PropertyName = "contact")]
        public string Contato { get; set; } 

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Activity" + "::4")]
        [JsonProperty(PropertyName = "activity")]
        public int? Atividade { get; set; } 

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Type_Of_Billing" + "::4")]
        [JsonProperty(PropertyName = "type_of_billing")]
        public int? TipoCobranca { get; set; } = null;

        [StringLength(4, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Seller" + "::4")]
        [JsonProperty(PropertyName = "seller")]
        public string VendedorCodigo { get; set; } 

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Blocked" + "::4")]
        [JsonProperty(PropertyName = "blocked")]
        public int? BloqueioMotivoCodigo { get; set; } = null;

        [Range(-99999999.99, 99999999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Credit_Limit" + "::8")]
        [JsonProperty(PropertyName = "credit_limit")]
        public float? LimiteValor { get; set; } = null;

        [Range(0, 999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Discount_Allowed" + "::3")]
        [JsonProperty(PropertyName = "discount_allowed")]
        public float? PorcentagemDesconto { get; set; } = null;

        [StringLength(50, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Observation" + "::50")]
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; } 

        [JsonProperty(PropertyName = "in86")]
        public bool? In86 { get; set; } = null;

        [Range(0, 999999999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Accounting_Account_Number" + "::15")]
        [JsonProperty(PropertyName = "accounting_account_number")]
        public long? ContaContabil { get; set; } 

        [JsonProperty(PropertyName = "pumper")]
        public bool? bombista { get; set; } = null;

        [JsonProperty(PropertyName = "mp_supplier")]
        public bool? FornecedorMp { get; set; } = null;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Region" + "::3")]
        [JsonProperty(PropertyName = "region")]
        public int? Regiao { get; set; } = null;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Route" + "::3")]
        [JsonProperty(PropertyName = "route")]
        public int? Rota { get; set; } = null;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Route_Sequence" + "::3")]
        [JsonProperty(PropertyName = "route_sequence")]
        public int? RotaSequencia { get; set; } = null;

        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Transporter" + "::6")]
        [JsonProperty(PropertyName = "transporter")]
        public int? Transp { get; set; } = null;

        [RegularExpression("^[C-F-J-P]$", ErrorMessage = nameof(EResourcesInterveniente.INTERVENIENTE_ERROR_TCON385F315F33))]
        [StringLength(1, ErrorMessage = nameof(EResourcesInterveniente.INTERVENIENTE_ERROR_TCON385F315F33))]
        [JsonProperty(PropertyName = "client_type")]
        public string IntervenienteTipo { get; set; } = null;

        [Range(0, 3, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Withhold_Iss" + "::1")]
        [JsonProperty(PropertyName = "withhold_iss")]
        public int? RetemIss { get; set; } = null;

        [JsonProperty(PropertyName = "delivery_location")]
        public bool? LocalEntrega { get; set; } = null;

        [JsonProperty(PropertyName = "specification")]
        public string Especificacao { get; set; }

        [StringLength(50, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Mother_Name" + "::50")]
        [JsonProperty(PropertyName = "mother_name")]
        public string NomeMae { get; set; } 

        [StringLength(40, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Spouse" + "::40")]
        [JsonProperty(PropertyName = "spouse")]
        public string NomeConjuge { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Bearer_Billing" + "::3")]
        [JsonProperty(PropertyName = "bearer_billing")]
        public int? PortadorCobranca { get; set; } = null;

        [JsonProperty(PropertyName = "employee")]
        public bool? Funcionario { get; set; } = null;

        [StringLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Site" + "::100")]
        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; }

        [JsonProperty(PropertyName = "engineering_approval")]
        public bool? AprovacaoEngenharia { get; set; } = null;

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Ddd_Commercial" + "::2")]
        [JsonProperty(PropertyName = "ddd_commercial")]
        public int? TelefoneComercialDdd { get; set; } = null;

        [Range(0, 999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Telephone_Commercial" + "::9")]
        [JsonProperty(PropertyName = "telephone_commercial")]
        public int? TelefoneComercialNumero { get; set; } = null;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "due_date_credit_limit")]
        public DateTime? LimiteData { get; set; } = null;

        [JsonProperty(PropertyName = "simple_national")]
        public bool? SimplesNacional { get; set; } = null;

        [JsonProperty(PropertyName = "inss_withholding")]
        public bool? RetemInss { get; set; } = null;

        [Range(0, 3, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Icms_Taxpayer" + "::1")]
        [JsonProperty(PropertyName = "icms_taxpayer")]
        public int? ContribuiIcms { get; set; } = null;

        [JsonProperty(PropertyName = "inactive")]
        public bool? Inativo { get; set; } = null;

        [StringLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::External_Id" + "::20")]
        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; } 

        [JsonProperty(PropertyName = "irrf_withholding")]
        public bool? RetemIrrf { get; set; } = null;

        [JsonProperty(PropertyName = "cofins_withholding")]
        public bool? RetemCofins { get; set; } = null;

        [JsonProperty(PropertyName = "pis_withholding")]
        public bool? RetemPis { get; set; } = null;

        [JsonProperty(PropertyName = "csll_withholding")]
        public bool? RetemCsll { get; set; } = null;
    }
}

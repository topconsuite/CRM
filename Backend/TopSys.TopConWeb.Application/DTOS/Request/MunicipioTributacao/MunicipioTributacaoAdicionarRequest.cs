using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao
{
    public class MunicipioTributacaoAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Municipality_Name")]
        [MaxLength(30, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Municipality_Name" + "::30")]
        [JsonProperty(PropertyName = "municipality_name")]
        public string Nome { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Uf_Acronym")]
        [MaxLength(2, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Uf_Acronym" + "::2")]
        [JsonProperty(PropertyName = "uf_acronym")]
        public string Uf { get; set; }

        [MaxLength(15, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Shorter_Municipality_Name" + "::15")]
        [JsonProperty(PropertyName = "shorter_municipality_name")]
        public string NomeReduzido { get; set; } = "";

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Iss_Tax" + "::4")]
        [JsonProperty(PropertyName = "iss_tax")]
        public int TributacaoIss { get; set; } = 0;

        [Range(0, 99.9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Iss_Tax_Rate_Percentage" + "::2")]
        [JsonProperty(PropertyName = "iss_tax_rate_percentage")]
        public float AliquotaIss { get; set; } = 0;

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Iss_Tax_Base" + "::2")]
        [JsonProperty(PropertyName = "iss_tax_base")]
        public int BaseCalculo { get; set; } = 0;

        [Range(0, 99.9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Iss_Tax_Liability_Percentage" + "::2")]
        [JsonProperty(PropertyName = "iss_tax_liability_percentage")]
        public float PercentualIssRetido { get; set; } = 0;

        [Range(0, 99.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Service_Percentage" + "::2")]
        [JsonProperty(PropertyName = "service_percentage")]
        public float PorcentagemServico { get; set; } = 0;

        [JsonProperty(PropertyName = "iss_withheld")]
        public bool IssRetido { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Ibge_Code")]
        [Range(0, 9999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Ibge_Code" + "::7")]
        [JsonProperty(PropertyName = "ibge_code")]
        public int IbgeCodigo { get; set; } = 0;

        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Municipal_Tax_Retention_Interv_Code" + "::6")]
        [JsonProperty(PropertyName = "municipal_tax_retention_interv_code")]
        public int IntervPrefeituraRetencao { get; set; } = 0;

        [Range(1, 99999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Country_Code" + "::3")]
        [JsonProperty(PropertyName = "country_code")]
        public int Pais { get; set; } = 1058;

        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Siafi_Code" + "::4")]
        [JsonProperty(PropertyName = "siafi_code")]
        public int CodigoSiafi { get; set; } = 0;

        [JsonProperty(PropertyName = "iss_pump_tax")]
        public bool TributacaoIntegralBomba { get; set; }

        [MaxLength(200, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Fiscal_Message" + "::200")]
        [JsonProperty(PropertyName = "fiscal_message")]
        public string MensagemFiscal { get; set; } = "";

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::External_Id")]
        [MaxLength(10, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::External_Id" + "::10")]
        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; } = "";

        [Range(0, 99.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Material_Deduction" + "::2")]
        [JsonProperty(PropertyName = "material_deduction")]
        public float PorcentagemDeducaoMaterial { get; set; } = 0;

        [JsonProperty(PropertyName = "other_taxed_services")]
        public bool TributacaoIntegralDemaisServicos { get; set; }

        [JsonProperty(PropertyName = "fully_taxed_rates")]
        public bool TaxasTributadasIntegralmente { get; set; }

        [Range(0, 9999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Min_Iss_Withholding_Amount" + "::4")]
        [JsonProperty(PropertyName = "min_iss_withholding_amount")]
        public float ValorMinimoRetencaoIss { get; set; } = 0;
    }
}

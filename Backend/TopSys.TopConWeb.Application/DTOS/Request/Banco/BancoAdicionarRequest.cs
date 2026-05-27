using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Banco
{
    public class BancoAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Code")]
        [Range(1, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Code" + "::3")]
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Company")]
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Company" + "::2")]
        [JsonProperty(PropertyName = "company")]
        public int EmpresaCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Bank_Trade_Name")]
        [MaxLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Bank_Trade_Name" + "::20")]
        [JsonProperty(PropertyName = "bank_trade_name")]
        public string Nome { get; set; }

        [MaxLength(40, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Bank_Company_Name" + "::40")]
        [JsonProperty(PropertyName = "bank_company_name")]
        public string Razao { get; set; } = "";

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Bank_Official_Code")]
        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Bank_Official_Code" + "::3")]
        [JsonProperty(PropertyName = "bank_official_code")]
        public int BancoCodigoOficial { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Agency_Number")]
        [Range(0, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Agency_Number" + "::4")]
        [JsonProperty(PropertyName = "agency_number")]
        public int NumeroAgencia { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Agency_Verification_Digit")]
        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Agency_Verification_Digit" + "::2")]
        [JsonProperty(PropertyName = "agency_verification_digit")]
        public int? DvAgencia { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Account_Number")]
        [Range(0, 9999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Account_Number" + "::10")]
        [JsonProperty(PropertyName = "account_number")]
        public long NumeroConta { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Account_Verification_Digit")]
        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Account_Verification_Digit" + "::2")]
        [JsonProperty(PropertyName = "account_verification_digit")]
        public int? DvConta { get; set; }

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Owner_Company" + "::2")]
        [JsonProperty(PropertyName = "owner_company")]
        public int EmpresaProprietaria { get; set; } = 0;
    }
}

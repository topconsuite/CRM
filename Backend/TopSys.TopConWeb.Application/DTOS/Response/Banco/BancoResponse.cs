using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.Banco
{
    public class BancoResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "company")]
        public int EmpresaCodigo { get; set; }

        [JsonProperty(PropertyName = "bank_trade_name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "bank_company_name")]
        public string Razao { get; set; }

        [JsonProperty(PropertyName = "bank_official_code")]
        public int BancoCodigoOficial { get; set; }

        [JsonProperty(PropertyName = "agency_number")]
        public int NumeroAgencia { get; set; }

        [JsonProperty(PropertyName = "agency_verification_digit")]
        public int DvAgencia { get; set; }

        [JsonProperty(PropertyName = "account_number")]
        public long NumeroConta { get; set; }

        [JsonProperty(PropertyName = "account_verification_digit")]
        public int DvConta { get; set; }

        [JsonProperty(PropertyName = "owner_company")]
        public int EmpresaProprietaria { get; set; }
    }
}

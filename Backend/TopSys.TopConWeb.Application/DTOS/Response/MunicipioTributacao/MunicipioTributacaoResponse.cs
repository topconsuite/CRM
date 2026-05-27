
using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.MunicipioTributacao
{
    public class MunicipioTributacaoResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "municipality_name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "uf_acronym")]
        public string Uf { get; set; }

        [JsonProperty(PropertyName = "shorter_municipality_name")]
        public string NomeReduzido { get; set; }

        [JsonProperty(PropertyName = "iss_tax")]
        public int TributacaoIss { get; set; }

        [JsonProperty(PropertyName = "iss_tax_rate_percentage")]
        public float AliquotaIss { get; set; }

        [JsonProperty(PropertyName = "iss_tax_base")]
        public int BaseCalculo { get; set; }

        [JsonProperty(PropertyName = "iss_tax_liability_percentage")]
        public float PercentualIssRetido { get; set; }

        [JsonProperty(PropertyName = "service_percentage")]
        public float PorcentagemServico { get; set; }

        [JsonProperty(PropertyName = "iss_withheld")]
        public bool IssRetido { get; set; }

        [JsonProperty(PropertyName = "ibge_code")]
        public int IbgeCodigo { get; set; }

        [JsonProperty(PropertyName = "municipal_tax_retention_interv_code")]
        public int IntervPrefeituraRetencao { get; set; }

        [JsonProperty(PropertyName = "country_code")]
        public int Pais { get; set; }

        [JsonProperty(PropertyName = "siafi_code")]
        public int CodigoSiafi { get; set; }

        [JsonProperty(PropertyName = "iss_pump_tax")]
        public bool TributacaoIntegralBomba { get; set; }

        [JsonProperty(PropertyName = "fiscal_message")]
        public string MensagemFiscal { get; set; }

        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; }

        [JsonProperty(PropertyName = "material_deduction")]
        public float PorcentagemDeducaoMaterial { get; set; }

        [JsonProperty(PropertyName = "other_taxed_services")]
        public bool TributacaoIntegralDemaisServicos { get; set; }

        [JsonProperty(PropertyName = "fully_taxed_rates")]
        public bool TaxasTributadasIntegralmente { get; set; }

        [JsonProperty(PropertyName = "min_iss_withholding_amount")]
        public float ValorMinimoRetencaoIss { get; set; }
    }
}

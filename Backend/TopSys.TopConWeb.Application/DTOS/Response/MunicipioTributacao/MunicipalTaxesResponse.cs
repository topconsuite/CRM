
namespace TopSys.TopConWeb.Application.DTOS.Response.MunicipioTributacao
{
    public class MunicipalTaxesResponse
    {
        public int Code { get; set; }
        public string MunicipalityName { get; set; }
        public string UfAcronym { get; set; }
        public string ShorterMunicipalityName { get; set; }
        public int IssTax { get; set; }
        public double IssTaxRatePercentage { get; set; }
        public int IssTaxBase { get; set; }
        public double IssTaxLiabilityPercentage { get; set; }
        public double ServicePercentage { get; set; }
        public bool IssWithheld { get; set; }
        public int IbgeCode { get; set; }
        public int MunicipalTaxRetentionIntervCode { get; set; }
        public int CountryCode { get; set; }
        public int SiafiCode { get; set; }
        public string RegisterId { get; set; }
        public bool IssPumpTax { get; set; }
        public string FiscalMessage { get; set; }
        public string ExternalId { get; set; }
        public double MaterialDeduction { get; set; }
        public bool OtherTaxedServices { get; set; }
        public bool FullyTaxedRates { get; set; }
        public double MinIssWithholdingAmount { get; set; }
    }
}

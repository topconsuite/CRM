using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao.Inclusao
{
    public class MunicipalTaxesInclusion
    {
        [Required]
        public string MunicipalityName { get; set; }
        [Required]
        public string UfAcronym { get; set; }
        public string ShorterMunicipalityName { get; set; } = string.Empty;
        public int IssTax { get; set; } = 0;
        public double IssTaxRatePercentage { get; set; } = 0.0000;
        public int IssTaxBase { get; set; } = 0;
        public double IssTaxLiabilityPercentage { get; set; } = 0.0000;
        public double ServicePercentage { get; set; } = 0.00;
        public bool IssWithheld { get; set; } = false;
        public int IbgeCode { get; set; } = 0;
        public int MunicipalTaxRetentionIntervCode { get; set; } = 0;
        public int CountryCode { get; set; } = 1058;
        public int SiafiCode { get; set; } = 0;
        public bool IssPumpTax { get; set; } = false;
        public string FiscalMessage { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public double MaterialDeduction { get; set; } = 0.00;
        public bool OtherTaxedServices { get; set; } = false;
        public bool FullyTaxedRates { get; set; } = false;
        public double MinIssWithholdingAmount { get; set; } = 0.00;
    }
}

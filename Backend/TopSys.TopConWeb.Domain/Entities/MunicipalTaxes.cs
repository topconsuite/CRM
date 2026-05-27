using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class MunicipalTaxes
    {
        public int Code { get; set; }
        public string MunicipalityName { get; set; }
        public string UfAcronym { get; set; }
        public string ShorterMunicipalityName { get; set; }
        public int? IssTax { get; set; }
        private double? issTaxRatePercentage;
        public double? IssTaxRatePercentage
        {
            get => issTaxRatePercentage.HasValue ? issTaxRatePercentage.Value : (double?)null;
            set => issTaxRatePercentage = Math.Round((double)value, 4);
        }
        public int? IssTaxBase { get; set; }
        private double? issTaxLiabilityPercentage { get; set; }
        public double? IssTaxLiabilityPercentage
        {
            get => issTaxLiabilityPercentage.HasValue ? issTaxLiabilityPercentage.Value : (double?)null;
            set => issTaxLiabilityPercentage = Math.Round((double)value, 4);
        }
        private double? servicePercentage { get; set; }
        public double? ServicePercentage
        {
            get => servicePercentage.HasValue ? Math.Round(servicePercentage.Value, 2) : (double?)null;
            set => servicePercentage = Math.Round((double)value, 2);
        }
        public bool? IssWithheld { get; set; }
        public int? IbgeCode { get; set; }
        public int? MunicipalTaxRetentionIntervCode { get; set; }
        public int? CountryCode { get; set; }
        public int? SiafiCode { get; set; }
        public string RegisterId { get; set; }
        public bool? IssPumpTax { get; set; }
        public string FiscalMessage { get; set; }
        public string ExternalId { get; set; }
        private double? materialDeduction { get; set; }
        public double? MaterialDeduction
        {
            get => materialDeduction.HasValue ? materialDeduction.Value : (double?)null;
            set => materialDeduction = Math.Round((double)value, 2);
        }
        public bool? OtherTaxedServices { get; set; }
        public bool? FullyTaxedRates { get; set; }
        private double? minIssWithholdingAmount { get; set; }
        public double? MinIssWithholdingAmount
        {
            get => minIssWithholdingAmount.HasValue ? minIssWithholdingAmount.Value : (double?)null;
            set => minIssWithholdingAmount = Math.Round((double)value, 2);
        }
    }
}

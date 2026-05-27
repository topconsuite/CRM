using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Domain.Enums;


namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao
{
    public class MunicipalTaxesAlterationRequest
    {
        [Required]
        public string MunicipalityName { get; set; }
        [Required]
        public string UfAcronym { get; set; }
        public string ShorterMunicipalityName { get; set; } = null;
        public int? IssTax { get; set; } = null;
        public double? IssTaxRatePercentage { get; set; } = null;
        public int? IssTaxBase { get; set; } = null;
        public double? IssTaxLiabilityPercentage { get; set; } = null;
        public double? ServicePercentage { get; set; } = null;
        public bool? IssWithheld { get; set; } = null;
        public int? IbgeCode { get; set; } = null;
        public int? MunicipalTaxRetentionIntervCode { get; set; } = null;
        public int? CountryCode { get; set; } = null;
        public int? SiafiCode { get; set; } = null;
        public bool? IssPumpTax { get; set; } = null;
        public string FiscalMessage { get; set; } = null;
        public string ExternalId { get; set; } = null;
        public double? MaterialDeduction { get; set; } = null;
        public bool? OtherTaxedServices { get; set; } = null;
        public bool? FullyTaxedRates { get; set; } = null;
        public double? MinIssWithholdingAmount { get; set; } = null;
    }
}

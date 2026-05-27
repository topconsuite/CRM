using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceberPublica
{
    public class AccountsReceivableResponse
    {
        public int Company { get; set; }
        public int DocumentType { get; set; }
        public string DocumentSerie { get; set; }
        public long DocumentNumber { get; set; }
        public int Sequence { get; set; }
        public int Splitting { get; set; }
        public int Client { get; set; }
        public DateTime? EmissionDate { get; set; }
        public DateTime? OperationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal Value { get; set; }
        public decimal SumReceived { get; set; }
        public decimal BalanceReceivable { get; set; }
        public int Operation { get; set; }
        public int CostCenter { get; set; }
        public int FinancialFlow { get; set; }
        public int Situation { get; set; }
        public DateTime? SituationDate { get; set; }
        public int BearerBank { get; set; }
        public string Observation { get; set; }
        public DateTime? LiquidationDate { get; set; }
        public int OperationLiquidate { get; set; }
        public string RegisterId { get; set; }
        public decimal FeesLiquidation { get; set; }
        public decimal DiscountLiquidation { get; set; }
        public bool ExpensesLiquidation { get; set; }
        public decimal AmountReceivedLiquidation { get; set; }
        public int BankLiquidation { get; set; }

    }

    
}

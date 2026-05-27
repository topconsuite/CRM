using System;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.CartaoPagamentoIntegracao
{
    [Serializable]
    public class CieloLioOrderRequest
    {
        public string number { get; set; } = Guid.NewGuid().ToString();
        public string reference { get; set; } = "";
        public string status { get; set; } = "";
        public IEnumerable<CieloLioOrderItemDTO> items { get; set; }
        public string notes { get; set; } = "nota teste";
        public int price { get; set; } = 0;
    }

    [Serializable]
    public class CieloLioOrderItemDTO
    {
        public string sku { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; } = "";
        public int unit_price { get; set; } = 0;
        public int quantity { get; set; } = 0;
        public string unit_of_measure { get; set; } = "EACH";
        public string description { get; set; } = "descrição";
        public string details { get; set; } = "detalhes";
    }
}

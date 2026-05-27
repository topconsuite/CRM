using Newtonsoft.Json;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{

    public class ContratoPagamentosWebHookDTO
    {
        [JsonProperty("ContratYear")]
        public int ContratoAno { get; set; }

        [JsonProperty("ContractNumber")]
        public int ContratoNumero { get; set; }

        [JsonProperty("ConcreteBatchingPlantExternalId")]
        public string ExternalIdUsina { get; set; }

        [JsonProperty("ContractConcreteBatchingPlant")]
        public int ContratoUsina { get; set; }

        [JsonProperty("Client")]
        public IntervenienteContratoPagamentoWebHookDTO Interveniente { get; set; }
        

        [JsonProperty("TotalValueContract")]
        public decimal ValorTotal { get; set; }

        [JsonProperty("Payments")]
        public List<ContratoPagamentoWebHookDTO> Pagamentos { get; set; }

    }
    
    public class ContratoPagamentoWebHookDTO
    {

        [JsonProperty("Sequence")]
        public int Sequencia { get; set; }

        [JsonProperty("Method")]
        public string Forma { get; set; }

        [JsonProperty("PaymentCondition")]
        public CondicaoPagamentoWebHookDTO CondicaoPagamento { get; set; }

        [JsonProperty("Value")]
        public float Valor { get; set; }

        [JsonProperty("Details")]
        public IEnumerable<DetalhesCondicaoPagamentoWebHookDTO> Detalhes { get; set; }

    }

    public class CondicaoPagamentoWebHookDTO
    {

        public CondicaoPagamentoWebHookDTO()
        {

        }

        public CondicaoPagamentoWebHookDTO(int codigo, string descricao, string idExterno)
        {
            Codigo = codigo;
            Descricao = descricao;
            IdExterno = idExterno;
        }

        [JsonProperty("Code")]
        public int Codigo { get; set; }

        [JsonProperty("Description")]
        public string Descricao { get; set; }

        [JsonProperty("ExternalId")]
        public string IdExterno { get; set; }

    }

    public class TipoCobrancaWebhookDTO
    {
        public TipoCobrancaWebhookDTO(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        [JsonProperty("Code")]
        public int Codigo { get; set; }

        [JsonProperty("Description")]
        public string Descricao { get; set; }

    }

    public class DetalhesCondicaoPagamentoWebHookDTO
    {
        [JsonProperty(PropertyName = "Sequence")]
        public int DetalheSequencia { get; set; }

        [JsonProperty(PropertyName = "Value")]
        public float Valor { get; set; }
    }

    public class IntervenienteContratoPagamentoWebHookDTO
    {


        [JsonProperty("Name")]
        public string Razao { get; set; }

        [JsonProperty("IdentificationDocument")]
        public string CpfCnpj { get; set; }

        [JsonProperty("ExternalId")]
        public string IdExterno { get; set; }

    }

}

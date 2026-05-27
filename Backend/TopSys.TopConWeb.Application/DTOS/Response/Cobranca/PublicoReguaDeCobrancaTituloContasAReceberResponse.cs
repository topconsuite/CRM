using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class PublicoReguaDeCobrancaTituloContasAReceberResponse
    {
        [JsonProperty(PropertyName = "account_id")]
        public int ContaNumero { get; set; }

        [JsonProperty(PropertyName ="account_name")]
        public string ContaNome { get; set; }

        [JsonProperty(PropertyName ="chave_nfe")]
        public string ChaveNfe { get; set; }

        [JsonProperty(PropertyName ="cnpj_cpf")]
        public string CpfCnpj { get; set; }

        [JsonProperty(PropertyName ="cod_cliente")]
        public string OrganizacaoCodigo { get; set; }

        [JsonProperty(PropertyName ="customer")]
        public string Cliente { get; set; }

        [JsonProperty(PropertyName ="customer_type")]
        public string ClienteTipo { get; set; }

        [JsonProperty(PropertyName ="description")]
        public string Descricao { get; set; }

        [JsonIgnore]
        public DateTime Vencimento { get; set; }

        [JsonProperty(PropertyName = "due_date")]
        public string VencimentoFormatted { get => Vencimento.ToString("yyyy-MM-dd"); }

        [JsonIgnore]
        public DateTime Emissao { get; set; }

        [JsonProperty(PropertyName = "issue_date")]
        public string EmissaoFormatted { get => Emissao.ToString("yyyy-MM-dd"); }

        [JsonIgnore]
        public DateTime? Pagamento { get; set; }

        [JsonProperty(PropertyName = "paid_at")]
        public string PagamentoFormatted { get => Pagamento?.ToString("yyyy-MM-dd"); }

        [JsonProperty(PropertyName ="original_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName ="paid_value")]
        public double PagamentoValor { get; set; }

        [JsonProperty(PropertyName ="net_value")]
        public double Saldo { get; set; }

        [JsonProperty(PropertyName ="payment_method")]
        public string PagamentoMetodo { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName ="to_email")]
        public string ClienteEmail { get; set; }

        [JsonProperty(PropertyName ="to_name")]
        public string ClienteNome { get; set; }

        [JsonProperty(PropertyName ="to_nome_fantasia")]
        public string ClienteNomeFantasia { get; set; }

        [JsonProperty(PropertyName ="to_razao_social")]
        public string ClienteRazaoSocial { get; set; }

        [JsonProperty(PropertyName ="to_sms")]
        public string ClienteTelefone { get; set; }

        [JsonProperty(PropertyName ="value")]
        public double Valor { get; set; }

        [JsonProperty(PropertyName = "num_boleto")]
        public string NumeroBoleto { get; set; }

        [JsonProperty(PropertyName ="barcode")]
        public string Barcode { get; set; }

        [JsonProperty(PropertyName = "view_link")]
        public string LinkSegundaViaBoleto { get; set; }
    }

}

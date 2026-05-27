using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Remessa
{
    public class RemessaPontosResponse
    {

        [JsonProperty(PropertyName = "invoice_number_packing_list")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "date_packing_list")]
        public DateTime? DataRemessa { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_batching_plant")]
        public int UsinaPesagem { get; set; }

        [JsonProperty(PropertyName = "client_name")]
        public string IntervenienteNome { get; set; } = "";

        [JsonProperty(PropertyName = "client_cnpj_cpf")]
        public string IntervenienteCpfCnpj { get; set; }

        [JsonProperty(PropertyName = "total_sale_value")]
        public float TracoValorTotal { get; set; }

        [JsonProperty(PropertyName = "referrer_type")]
        public int IndicadorTipo { get; set; }

        [JsonProperty(PropertyName = "referrer_cnpj_cpf")]
        public string IndicadorCpfCnpj { get; set; }

        [JsonProperty(PropertyName = "referrer_name")]
        public string IndicadorNome { get; set; }

        [JsonProperty(PropertyName = "points")]
        public string IndicadorPontos { get; set; }

    }
}

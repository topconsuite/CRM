using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral
{
    public class CadastroGeralIntegracaoResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "short_description")]
        public string DescricaoReduzida { get; set; }

        [JsonProperty(PropertyName = "id_registration")]
        public string IdCadastro { get; set; }

        [JsonProperty(PropertyName = "id_update")]
        public string IdAtualizacao { get; set; }

        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }
    }
}

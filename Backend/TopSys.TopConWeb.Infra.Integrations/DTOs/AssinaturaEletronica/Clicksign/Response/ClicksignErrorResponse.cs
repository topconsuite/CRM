using Newtonsoft.Json;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Response
{
    public class ClicksignErrorResponse
    {
        [JsonProperty(PropertyName = "errors")]
        public IEnumerable<string> Erros { get; set; }
    }
}

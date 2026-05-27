using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request
{
    public class RequestSignatureRequest
    {
        public RequestSignatureRequest(Guid signerKey, string message)
        {
            SignerKey = signerKey;
            Message = message;
        }

        public RequestSignatureRequest(Guid signerKey)
        {
            SignerKey = signerKey;
        }

        [JsonProperty(PropertyName = "request_signature_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Response
{
    public class AddSignerToDocumentResponse
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerDTO Signer { get; set; }
    }

    public class AddSignerDTO
    {
        [JsonProperty(PropertyName = "request_signature_key")]
        public Guid RequestSignatureKey { get; set; }

    }
}

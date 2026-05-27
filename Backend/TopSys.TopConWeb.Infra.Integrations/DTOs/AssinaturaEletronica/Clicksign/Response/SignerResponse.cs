using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Response
{
    public class SignerResponse
    {
        [JsonProperty(PropertyName = "signer")]
        public SignerDTO Signer { get; set; }
    }

    public class SignerDTO
    {
        [JsonProperty(PropertyName = "key")]
        public Guid Id { get; set; }

    }
}

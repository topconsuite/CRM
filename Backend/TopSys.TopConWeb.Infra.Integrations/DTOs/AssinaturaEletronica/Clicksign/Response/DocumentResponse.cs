using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Response
{
    public class DocumentResponse
    {
        [JsonProperty(PropertyName = "document")]
        public DocumentDTO Document { get; set; }
    }

    public class DocumentDTO
    {

        [JsonProperty(PropertyName = "key")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "content_base64")]
        public string File { get; set; }
        [JsonProperty(PropertyName = "request_signature_key")]
        public Guid RequestSignatureKey { get; set; }
        [JsonProperty(PropertyName = "downloads")]
        public DownloadDTO DownloadDocument { get; set; }
    }

    public class DownloadDTO
    {
        [JsonProperty(PropertyName = "signed_file_url")]
        public string SignedFileUrl { get; set; }
    }

}

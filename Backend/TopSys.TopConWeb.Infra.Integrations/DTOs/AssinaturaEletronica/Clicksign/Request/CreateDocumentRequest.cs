using Newtonsoft.Json;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request
{
    public class CreateDocumentRequest
    {

        [JsonProperty(PropertyName = "document")]
        public DocumentDTO Document { get; set; }
    }

    public class DocumentDTO
    {
        public DocumentDTO(string path, string file, DateTime deadline)
        {
            Path = path;
            File = file;
            Deadline = deadline;
        }
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
        [JsonProperty(PropertyName = "content_base64")]
        public string File { get; set; }
        [JsonProperty(PropertyName = "deadline_at")]
        public DateTime Deadline { get; set; }
    }
}

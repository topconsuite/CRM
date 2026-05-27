using Newtonsoft.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request
{
    public class AddSignerToDocumentRequest
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerToDocumentDTO List { get; set; }
    }

    public class AddSignerHiredToDocumentRequest
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerHiredToDocumentDTO List { get; set; }
    }

    public class AddSignerCoResponsibleToDocumentRequest
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerCoResponsibleToDocumentDTO List { get; set; }
    }

    public class AddSignerWitnessToDocumentRequest
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerWitnessToDocumentDTO List { get; set; }
    }

    public class AddSignerSellerToDocumentRequest
    {
        [JsonProperty(PropertyName = "list")]
        public AddSignerSellerToDocumentDTO List { get; set; }
    }

    public class AddSignerToDocumentDTO
    {
        public AddSignerToDocumentDTO(Guid documentKey, Guid signerKey)
        {
            DocumentKey = documentKey;
            SignerKey = signerKey;
        }
        [JsonProperty(PropertyName = "document_key")]
        public Guid DocumentKey { get; set; }
        [JsonProperty(PropertyName = "signer_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "sign_as")]
        public string SignAs { get; set; } = "contractor";
    }

    public class AddSignerHiredToDocumentDTO
    {
        public AddSignerHiredToDocumentDTO(Guid documentKey, Guid signerKey)
        {
            DocumentKey = documentKey;
            SignerKey = signerKey;
        }
        [JsonProperty(PropertyName = "document_key")]
        public Guid DocumentKey { get; set; }
        [JsonProperty(PropertyName = "signer_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "sign_as")]
        public string SignAs { get; set; } = "contractee";
    }

    public class AddSignerCoResponsibleToDocumentDTO
    {
        public AddSignerCoResponsibleToDocumentDTO(Guid documentKey, Guid signerKey)
        {
            DocumentKey = documentKey;
            SignerKey = signerKey;
        }
        [JsonProperty(PropertyName = "document_key")]
        public Guid DocumentKey { get; set; }
        [JsonProperty(PropertyName = "signer_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "sign_as")]
        public string SignAs { get; set; } = "co_responsible";
    }

    public class AddSignerWitnessToDocumentDTO
    {
        public AddSignerWitnessToDocumentDTO(Guid documentKey, Guid signerKey)
        {
            DocumentKey = documentKey;
            SignerKey = signerKey;
        }
        [JsonProperty(PropertyName = "document_key")]
        public Guid DocumentKey { get; set; }
        [JsonProperty(PropertyName = "signer_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "sign_as")]
        public string SignAs { get; set; } = "witness";
    }

    public class AddSignerSellerToDocumentDTO
    {
        public AddSignerSellerToDocumentDTO(Guid documentKey, Guid signerKey)
        {
            DocumentKey = documentKey;
            SignerKey = signerKey;
        }
        [JsonProperty(PropertyName = "document_key")]
        public Guid DocumentKey { get; set; }
        [JsonProperty(PropertyName = "signer_key")]
        public Guid SignerKey { get; set; }
        [JsonProperty(PropertyName = "sign_as")]
        public string SignAs { get; set; } = "seller";
    }
}

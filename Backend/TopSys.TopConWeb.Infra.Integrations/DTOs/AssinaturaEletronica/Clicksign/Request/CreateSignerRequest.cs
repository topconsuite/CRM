using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request
{
    public class CreateSignerRequest
    {
        [JsonProperty(PropertyName = "signer")]
        public SignerDTO Signer { get; set; }
    }

    public class SignerDTO
    {
        public SignerDTO(ClicksignSigner clicksignSigner)
        {
            Email = clicksignSigner.Email;
            Auths = new List<string> { clicksignSigner.MetodoAutenticacaoString };
            Name = clicksignSigner.Nome;
            Phone = clicksignSigner.Telefone;
            Document = clicksignSigner.Cpf;
            Birthday = clicksignSigner.DataNascimento;
            OfficialDocumentEnabled = clicksignSigner.ObrigaDocumentoOficial;
            SelfieEnabled = clicksignSigner.ObrigaSelfie;
            HandwrittenEnabled = clicksignSigner.ObrigaAssinaturaManuscrita;
            LivenessEnabled = clicksignSigner.ObrigaReconhecimentoFacial;
            DeliveryWhenDone = clicksignSigner.MetodoNotificaClienteNaConfirmacaoDeAssinatura;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "auths")]
        public IEnumerable<string> Auths { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "phone_number")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "documentation")]
        public string Document { get; set; }
        [JsonProperty(PropertyName = "birthday")]
        public DateTime Birthday { get; set; }
        [JsonProperty(PropertyName = "official_document_enabled")]
        public bool OfficialDocumentEnabled { get; set; }
        [JsonProperty(PropertyName = "selfie_enabled")]
        public bool SelfieEnabled { get; set; }
        [JsonProperty(PropertyName = "handwritten_enabled")]
        public bool HandwrittenEnabled { get; set; }
        [JsonProperty(PropertyName = "liveness_enabled")]
        public bool LivenessEnabled { get; set; }
        [JsonProperty(PropertyName = "delivery")]
        public string DeliveryWhenDone { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request
{
    public class CreateSignerHiredRequest
    {
        [JsonProperty(PropertyName = "signer")]
        public SignerHiredDTO Signer { get; set; }
    }

    public class SignerHiredDTO
    {
        public SignerHiredDTO(ClicksignSigner clicksignSigner)
        {
            Email = clicksignSigner.Email;
            Auths = new List<string> { clicksignSigner.MetodoAutenticacaoString };
            Phone = clicksignSigner.Telefone;
            DeliveryWhenDone = clicksignSigner.MetodoNotificaClienteNaConfirmacaoDeAssinatura;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "auths")]
        public IEnumerable<string> Auths { get; set; }
        [JsonProperty(PropertyName = "phone_number")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "delivery")]
        public string DeliveryWhenDone { get; set; }
    }
}

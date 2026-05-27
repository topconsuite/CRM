using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class ClicksignSigner
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacao { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinatura { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public bool ObrigaDocumentoOficial { get; set; }
        public bool ObrigaSelfie { get; set; }
        public bool ObrigaAssinaturaManuscrita { get; set; }
        public bool ObrigaReconhecimentoFacial { get; set; }
        public bool NotificaClienteNaConfirmacaoDeAssinatura { get; set; }
        public Guid RequestSignatureKey { get; set; }
        public string MessageEmail { get; set; }

        public string MetodoAutenticacaoString
        {
            get
            {
                switch (MetodoAutenticacao)
                {
                    case EMetodoAutenticacaoAssinaturaEletronicaClicksign.Email:
                        return "email";
                    case EMetodoAutenticacaoAssinaturaEletronicaClicksign.Whatsapp:
                        return "whatsapp";
                    case EMetodoAutenticacaoAssinaturaEletronicaClicksign.Sms:
                        return "sms";
                    default:
                        return "";
                }
            }
        }

        public string MetodoNotificaClienteNaConfirmacaoDeAssinatura
        {
            get
            {
                return NotificaClienteNaConfirmacaoDeAssinatura ? "email" : "none";
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class ClicksignParametros
    {
        public Guid Id { get; set; }
        public string CorpoEmail { get; set; }
        public bool ObrigaDocumentoOficial { get; set; }
        public bool ObrigaSelfie { get; set; }
        public bool ObrigaAssinaturaManuscrita { get; set; }
        public bool ObrigaReconhecimentoFacial { get; set; }
        public bool NotificaClienteNaConfirmacaoDeAssinatura { get; set; }
        public double PrazoLimiteAssinatura { get; set; }
        public bool EnviaAssinaturaContratada { get; set; }
        public bool EnviaAssinaturaResponsavelSolidario { get; set; }
        public string EmailContratada { get; set; }
        public int DDDContratada { get; set; }
        public int TelefoneContratada { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaContratada { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoContratada { get; set; }
        public bool PermiteAssinaturaWhatsApp { get; set; }
        public EOpcoesTestemunhasAssinaturaEletronicaClicksign PrimeiraTestemunha { get; set; }
        public EOpcoesTestemunhasAssinaturaEletronicaClicksign SegundaTestemunha { get; set; }

    }
}

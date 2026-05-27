using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class SolicitacaoAssinaturaEletronicaClicksign
    {
        public int ContratoUsina { get; set; }
        public int ContratoNumero { get; set; }
        public int ContratoAno { get; set; }
        public Stream ContratoPdf { get; set; }
        public List<DadosPessoaisAssinatura> DadosPessoaisAssinatura { get; set; } = new List<DadosPessoaisAssinatura>();
        public string EmailResponsavelSolidario { get; set; }
        public string NomeCompletoResponsavelSolidario { get; set; }
        public string CpfResponsavelSolidario { get; set; }
        public string TelefoneResponsavelSolidario { get; set; }
        public DateTime DataNascimentoResponsavelSolidario { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacao { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoResponsavelSolidario { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinatura { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaResponsavelSolidario { get; set; }
        public string EmailPrimeiraTestemunha { get; set; }
        public string NomeCompletoPrimeiraTestemunha { get; set; }
        public string CpfPrimeiraTestemunha { get; set; }
        public string TelefonePrimeiraTestemunha { get; set; }
        public DateTime DataNascimentoPrimeiraTestemunha { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoPrimeiraTestemunha { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaPrimeiraTestemunha { get; set; }
        public string EmailSegundaTestemunha { get; set; }
        public string NomeCompletoSegundaTestemunha { get; set; }
        public string CpfSegundaTestemunha { get; set; }
        public string TelefoneSegundaTestemunha { get; set; }
        public DateTime DataNascimentoSegundaTestemunha { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoSegundaTestemunha { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaSegundaTestemunha { get; set; }
    }
}
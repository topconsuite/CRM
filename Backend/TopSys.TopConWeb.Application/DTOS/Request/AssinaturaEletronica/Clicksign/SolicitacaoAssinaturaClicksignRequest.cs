using System;
using System.Collections.Generic;
using System.IO;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica.Clicksign
{
    public class SolicitacaoAssinaturaClicksignRequest
    {
        public int ContratoUsina { get; set; }
        public int ContratoNumero { get; set; }
        public int ContratoAno { get; set; }
        public List<DadosPessoaisAssinaturaRequest> DadosPessoaisAssinatura { get; set; } = new List<DadosPessoaisAssinaturaRequest>();
        public string EmailResponsavelSolidario { get; set; }
        public string NomeCompletoResponsavelSolidario { get; set; }
        public string CpfResponsavelSolidario { get; set; }
        public string TelefoneDddResponsavelSolidario { get; set; }
        public string TelefoneNumeroResponsavelSolidario { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataNascimentoResponsavelSolidario { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacao { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoResponsavelSolidario { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinatura { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaResponsavelSolidario { get; set; }
        public string EmailPrimeiraTestemunha { get; set; }
        public string NomeCompletoPrimeiraTestemunha { get; set; }
        public string CpfPrimeiraTestemunha { get; set; }
        public string TelefoneDddPrimeiraTestemunha { get; set; }
        public string TelefoneNumeroPrimeiraTestemunha { get; set; }
        public DateTime DataNascimentoPrimeiraTestemunha { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoPrimeiraTestemunha { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaPrimeiraTestemunha { get; set; }
        public string EmailSegundaTestemunha { get; set; }
        public string NomeCompletoSegundaTestemunha { get; set; }
        public string CpfSegundaTestemunha { get; set; }
        public string TelefoneDddSegundaTestemunha { get; set; }
        public string TelefoneNumeroSegundaTestemunha { get; set; }
        public DateTime DataNascimentoSegundaTestemunha { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacaoSegundaTestemunha { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinaturaSegundaTestemunha { get; set; }
    }
}

using System;
using System.IO;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica.Clicksign
{
    public class DadosPessoaisAssinaturaRequest
    {
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string TelefoneDdd { get; set; }
        public string TelefoneNumero { get; set; }
        public DateTime DataNascimento { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacao { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinatura { get; set; }
    }
}

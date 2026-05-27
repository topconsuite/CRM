using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class DadosPessoaisAssinatura
    {
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public EMetodoAutenticacaoAssinaturaEletronicaClicksign MetodoAutenticacao { get; set; }
        public EMetodoEnvioAssinaturaEletronicaClicksign MetodoEnvioAssinatura { get; set; }
    }
}
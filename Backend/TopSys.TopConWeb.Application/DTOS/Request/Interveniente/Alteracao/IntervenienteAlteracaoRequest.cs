using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Alteracao
{
    public class IntervenienteAlteracaoRequest
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }

        public string CpfCnpj { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;

        public string Profissao { get; set; }
        public string EmpresaTrabalho { get; set; }
        public int TelefoneComercialDdd { get; set; }
        public int TelefoneComercialNumero { get; set; }
        public string NomeMae { get; set; }
        public string NomeConjuge { get; set; }
        public string Contato { get; set; }
        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }
        public int Ramal { get; set; }
        public int CelularDdd { get; set; }
        public int CelularNumero { get; set; }
        public string Email { get; set; }
        public string EmailCobranca { get; set; }

        public string Observacao { get; set; }

    }
}

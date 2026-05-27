using System;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;

namespace TopSys.TopConWeb.Application.DTOS.Response.Opportunity
{
    public class IntervenienteDTO : IHasEnderecoDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }

        public string IntervenienteTipo { get; set; }
        public string CpfCnpj { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }

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

        public float LimiteValor { get; set; }

        public DateTime? LimiteData { get; set; }

        public string Observacao { get; set; }

        public virtual CadastroGeralDTO BloqueioMotivo { get; set; }

        public virtual EnderecoDTO Endereco { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class PropostaResponsavelSolidarioBase<TProposta>
    {
        public int UsinaCodigo { get; set; }
        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }
        public TProposta Proposta { get; set; }

        public string IntervenienteTipo { get; set; }
        public string CpfCnpj { get; set; }
        public string Razao { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }
        public string Email { get; set; }
        public int? TelefoneDdd { get; set; }
        public int? TelefoneNumero { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public Municipio EnderecoMunicipio { get; set; }
    }
}

using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class IntervenienteLocal : IHasEndereco
    {
        public int IntervenienteCodigo { get; set; }
        public int Sequencia { get; set; }

        public string CpfCnpj { get; set; }
        public string Razao { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public Municipio EnderecoMunicipio { get; set; }

        public string Email { get; set; }

        public string LocalCobrancaSimNao { get; set; } = "S";
        public string LocalEntregaSimNao { get; set; } = "S";
        public string LocalFaturamentoSimNao { get; set; } = "S";

        public string IdCadastro { get; set; }
        public string IdAtualizacao { get; set; }
    }
}

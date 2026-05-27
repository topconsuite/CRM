using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;

namespace TopSys.TopConWeb.Application.DTOS.Generic
{
    public class PropostaCobrancaDTO : IHasEnderecoDTO
    {
        public int UsinaCodigo { get; set; }
        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public string IntervenienteTipo { get; set; }
        public string CpfCnpj { get; set; }
        public string Razao { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }
        public string Email { get; set; }

        public EnderecoDTO Endereco { get; set; }
    }
}

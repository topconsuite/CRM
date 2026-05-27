using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato
{
    public class ContratoDadosFaturamentoDTO
    {
        public ContratoDadosFaturamentoDTO(string intervenienteTipo, string cpfCnpj, string inscricaoEstadual, string inscricaoMunicipal, string rg, string razao, string nome)
        {
            IntervenienteTipo = intervenienteTipo;
            CpfCnpj = cpfCnpj;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            Rg = rg;
            Razao = razao;
            Nome = nome;
        }

        public ContratoDadosFaturamentoDTO() { }

        [JsonProperty(PropertyName = "client_type")]
        public string IntervenienteTipo { get; set; } = "";

        [JsonProperty(PropertyName = "document")]
        public string CpfCnpj { get; set; } = "";

        [JsonProperty(PropertyName = "state_registration")]
        public string InscricaoEstadual { get; set; } = "";

        [JsonProperty(PropertyName = "municipal_registration")]
        public string InscricaoMunicipal { get; set; } = "";

        [JsonProperty(PropertyName = "rg")]
        public string Rg { get; set; } = "";

        [JsonProperty(PropertyName = "legal_name")]
        public string Razao { get; set; } = "";

        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; } = "";
    }
}

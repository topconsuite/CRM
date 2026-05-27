using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato
{
    public class PortadorDTO
    {
        public PortadorDTO(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }
    }
}

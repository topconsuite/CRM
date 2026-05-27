using Newtonsoft.Json;
namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber

{
    public class SegmentacaoDTO
    {
        
        [JsonProperty(PropertyName = "id")] 
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "name")] 
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "short_name")]
        public string NomeAbreviado { get; set; }
    }

}
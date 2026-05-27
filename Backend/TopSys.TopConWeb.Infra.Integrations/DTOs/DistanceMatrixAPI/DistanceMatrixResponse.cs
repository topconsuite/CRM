using RestSharp.Deserializers;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Infra.Integrations.DTOs.DistanceMatrixAPI
{
    public class DistanceMatrixResponse
    {
        public string Status { get; set; }

        [DeserializeAs(Name = "rows")]
        public IEnumerable<Resultado> Resultados { get; set; }

    }
    public class Resultado
    {
        [DeserializeAs(Name = "elements")]
        public IEnumerable<Rota> Rotas { get; set; }
    }
    public class Rota
    {
        [DeserializeAs(Name = "distance")]
        public Distancia Distancia { get; set; }

        [DeserializeAs(Name = "status")]
        public string StatusElemento { get; set; }
    }
    
    public class Distancia
    {
        [DeserializeAs(Name = "text")]
        public string ResultadoEmKm { get; set; }

        [DeserializeAs(Name = "value")]
        public float ResultadoEmMetros { get; set; }
    }
}

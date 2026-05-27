using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ConsularObrasRequest
{
    public class ConsultarObrasRequest : PagedRequest
    {
        public string Ordenacao { get; set; }
    }
}

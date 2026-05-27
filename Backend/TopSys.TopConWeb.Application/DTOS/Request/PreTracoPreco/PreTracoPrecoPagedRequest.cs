using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco
{
    public class PreTracoPrecoPagedRequest : PagedRequest
    {
        public int Segmentacao { get; set; }
    }
}

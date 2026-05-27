using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.GrupoProduto
{
    public class GrupoProdutoPagedRequest : PagedRequest
    {
        public int CodigoInterveniente { get; set; }
    }
}

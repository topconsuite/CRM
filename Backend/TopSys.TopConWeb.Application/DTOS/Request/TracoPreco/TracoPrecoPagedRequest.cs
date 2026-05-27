using System;
using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.TracoPreco
{
    public class TracoPrecoPagedRequest : PagedRequest
    {
        public DateTime Data { get; set; }

        public int Usina { get; set; }

        public int Segmentacao { get; set; }
    }
}

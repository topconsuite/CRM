using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Paged
{
    public abstract class PagedRequest
    {
        public int Pagina { get; set; }

        public int PorPagina { get; set; }

        public string Filter { get; set; }
    }
}

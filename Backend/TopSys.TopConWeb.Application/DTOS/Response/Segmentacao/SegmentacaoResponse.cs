using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Segmentacao
{
    public class SegmentacaoResponse
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeAbreviado { get; set; }

        public string ExternalId { get; set; }
    }
}
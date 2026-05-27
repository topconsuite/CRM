using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraBomba
{
    public class CalcularEbitdaObraBomba : ObraBombaBase<Domain.Entities.ObraBomba>
    {
        public int UsinaEntregaCodigo { get; set; }
        public int EnderecoMunicipioCodigo { get; set; }
        public ICollection<Domain.Entities.ObraTraco> ObraTracos { get; set; }
    }
}

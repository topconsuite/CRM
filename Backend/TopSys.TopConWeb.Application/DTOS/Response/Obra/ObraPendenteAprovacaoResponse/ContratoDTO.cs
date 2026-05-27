using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse
{
    public class ContratoDTO
    {
        public ICollection<ContratoTracoReajusteDTO> ContratoTracoReajustes { get; set; }
    }
}

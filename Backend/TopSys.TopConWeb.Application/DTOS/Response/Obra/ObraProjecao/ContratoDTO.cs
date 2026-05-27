using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecaoDTO;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecao
{
    public class ContratoDTO
    {


        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public int? IntervenienteCodigo { get; set; } = 0;

        public virtual IntervenienteDTO Interveniente { get; set; }

        public int? VendedorCodigo { get; set; } = 0;

        public virtual VendedorDTO Vendedor { get; set; }

        public ICollection<ObraProjecaoDTO> ObraProjecao { get; set; }

    }
}

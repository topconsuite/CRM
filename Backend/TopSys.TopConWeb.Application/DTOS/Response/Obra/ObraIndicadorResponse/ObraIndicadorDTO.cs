using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraIndicadorResponse
{
    public class ObraIndicadorDTO
    {

        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int IntervenienteCodigo { get; set; }
        public virtual IntervenienteDTO Interveniente { get; set; }

        public int VendedorCodigo { get; set; }
        public virtual VendedorDTO Vendedor { get; set; }

    }
}

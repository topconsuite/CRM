using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse
{
    public class ObraDTO
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public string Nome { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public int? AnoContrato { get; set; } = 0;
        public int? NumContrato { get; set; } = 0;

        public virtual EStatusProjecao StatusProjecao { get; set; }

        public TipoCobrancaDTO TipoCobranca { get; set; }
        public ContratoDTO Contrato { get; set; }
    }
}

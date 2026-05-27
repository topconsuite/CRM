using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse
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

        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public TipoCobrancaDTO TipoCobranca { get; set; }
        public ContratoDTO Contrato { get; set; }

        public ICollection<ObraBombaDTO> ObraBombas { get; set; }
    }
}

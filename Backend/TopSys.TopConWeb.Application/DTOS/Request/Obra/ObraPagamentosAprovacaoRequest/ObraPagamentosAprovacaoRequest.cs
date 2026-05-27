using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest
{
    public class ObraPagamentosAprovacaoRequest
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public int? AnoContrato { get; set; } = 0;
        public int? NumContrato { get; set; } = 0;

        public EObraStatusFinanceiro StatusFinanceiro { get; set; }

        public ICollection<ObraPagamentoDTO> ObraPagamentos { get; set; }

        public ICollection<MovimentoBancoDTO> MovimentosBancoAVincular { get; set; }
    }
}

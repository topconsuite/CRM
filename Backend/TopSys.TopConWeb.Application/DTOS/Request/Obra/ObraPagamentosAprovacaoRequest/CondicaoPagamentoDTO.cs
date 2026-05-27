namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest
{
    public class CondicaoPagamentoDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public float MediaDias { get; set; }
    }
}

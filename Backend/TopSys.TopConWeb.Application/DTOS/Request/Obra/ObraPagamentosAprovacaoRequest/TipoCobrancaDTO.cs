namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest
{
    public class TipoCobrancaDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Forma { get; set; }
        
        public PortadorDTO Portador { get; set; }

        public string Fixo { get; set; }

        public string Aprovacao { get; set; }
    }
}

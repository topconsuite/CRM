namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest
{
    public class CartaoBandeiraDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public virtual IntervenienteDTO Interveniente { get; set; }
        
        public virtual PortadorDTO Portador { get; set; }
    }
}

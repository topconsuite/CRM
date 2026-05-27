namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class CartaoBandeiraDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public virtual IntervenienteDTO Interveniente { get; set; }
        
        public virtual PortadorDTO Portador { get; set; }
    }
}

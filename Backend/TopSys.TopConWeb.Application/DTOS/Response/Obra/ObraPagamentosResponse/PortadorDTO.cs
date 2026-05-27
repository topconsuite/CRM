namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class PortadorDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }
        public virtual ContaDTO Conta { get; set; }
    }
}

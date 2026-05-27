
namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoPagamentoVersao : ContratoPagamentoBase<ContratoPagamentoDetalheVersao>
    {
        public ContratoPagamentoVersao() : base() { }
        public int NumeroVersao { get; set; }
    }
}

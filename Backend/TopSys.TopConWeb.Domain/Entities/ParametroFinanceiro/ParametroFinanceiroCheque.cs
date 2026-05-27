namespace TopSys.TopConWeb.Domain.Entities
{
    public class ParametroFinanceiroCheque : ParametroFinanceiro
    {
        public int OperacaoPadraoInclusao { get; set; }
        public int PortadorPadrao { get; set; }
        public int SituacaoPadrao { get; set; }
    }
}

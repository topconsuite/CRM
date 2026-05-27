namespace TopSys.TopConWeb.Domain.Entities
{
    public class ParametroTaxaExtra : Parametro
    {
        public ParametroTaxaExtra() : base() { }

        public string MensagemAlteracaoPedra { get; set; }

        public string MensagemAlteracaoSlump { get; set; }
    }
}

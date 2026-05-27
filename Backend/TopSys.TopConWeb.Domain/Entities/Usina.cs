namespace TopSys.TopConWeb.Domain.Entities
{
    public class Usina
    {
        public int Codigo { get; set; }

        public int? ClicksignConfigId { get; set; }

        public virtual ClicksignConfiguracao ClicksignConfiguracao { get; set; }

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public int? FilialCodigo { get; set; } = 0;

        public string GeraCreditoClientePagamentoAntecipadoSimNao { get; set; }
        public virtual bool GeraCreditoClientePagamentoAntecipado
        {
            get { return (GeraCreditoClientePagamentoAntecipadoSimNao ?? "").Equals("S"); }
            set { GeraCreditoClientePagamentoAntecipadoSimNao = value ? "S" : "N"; }
        }

        public int PrazoPesagem { get; set; }
        public float PorcentagemRetornoObra { get; set; }

        public int TempoBtNaObra { get; set; }

        public int TempoBtAteAObra { get; set; }

        public string MoldagemRemota { get; set; }

        // Parametros Programação
        public int IntervaloEmMinutosEntreCargas { get; set; }

        public string ExternalId { get; set; }

        public int EmpresaCodigo {
            get
            {
                return (FilialCodigo.GetValueOrDefault()) / 1000;
            }
        }

    }
}

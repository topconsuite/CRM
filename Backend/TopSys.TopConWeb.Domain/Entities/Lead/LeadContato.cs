namespace TopSys.TopConWeb.Domain.Entities.Lead
{
    public class LeadContato
    {
        public int Usina { get; set; }
        public int NumeroLead { get; set; }
        public int AnoLead { get; set; }
        public int Sequencia { get; set; }
        public string Nome { get; set; }
        public virtual CadastroGeral Funcao { get; set; }
        public int? FuncaoCodigo { get; set; } = 0;
        public int Ddd { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
    }
}

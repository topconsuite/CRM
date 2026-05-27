namespace TopSys.TopConWeb.Domain.Entities.Oportunidades
{
    public class OportunidadeContato
    {
        public int Usina { get; set; }
        public int NumeroOportunidade { get; set; }
        public int AnoOportunidade { get; set; }
        public int Sequencia { get; set; }
        public virtual CadastroGeral Funcao { get; set; }
        public int? FuncaoCodigo { get; set; } = 0;
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
    }
}

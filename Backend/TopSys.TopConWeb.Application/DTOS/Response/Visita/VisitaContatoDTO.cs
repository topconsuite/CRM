namespace TopSys.TopConWeb.Application.DTOS.Response.Visita
{
    public class VisitaContatoDTO
    {

        public int Usina { get; set; }
        public int NumeroVisita { get; set; }
        public int AnoVisita { get; set; }
        public int Sequencia { get; set; }
        public string Nome { get; set; }
        public CadastroGeralDTO Funcao { get; set; }
        public int FuncaoCodigo { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }

    }
}

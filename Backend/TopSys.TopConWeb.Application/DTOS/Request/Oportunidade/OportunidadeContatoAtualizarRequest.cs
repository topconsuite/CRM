namespace TopSys.TopConWeb.Application.DTOS.Request.Oportunidade
{
    public class OportunidadeContatoAtualizarRequest
    {

        public int Usina { get; set; }
        public int NumeroOportunidade { get; set; }
        public int AnoOportunidade { get; set; }
        public int Sequencia { get; set; }
        public string Nome { get; set; }
        public int FuncaoCodigo { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }

    }
}

namespace TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral
{
    public class CadastroGeralResponse
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public CadastroGeralViaCaptacaoResponse ViaCaptacao { get; set; }
    }
}

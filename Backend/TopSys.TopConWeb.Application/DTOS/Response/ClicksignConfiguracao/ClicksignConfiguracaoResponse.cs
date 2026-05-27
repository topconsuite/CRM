namespace TopSys.TopConWeb.Application.DTOS.Response.ClicksignConfiguracao
{
    public class ClicksignConfiguracaoResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; }

        public string BaseUrl { get; set; }

        public string Sha256Secret { get; set; }

        public bool Ativo { get; set; }
    }
}

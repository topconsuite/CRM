namespace TopSys.TopConWeb.Application.DTOS.Request.ClicksignConfiguracao
{
    public class ClicksignConfiguracaoRequest
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string BaseUrl { get; set; }

        public string Alias { get; set; }

        public string Sha256Secret { get; set; }

        public bool Ativo { get; set; }
    }
}

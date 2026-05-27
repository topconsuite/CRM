using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ParametrosSSO
    {
        public Guid Id { get; set; }
        public bool Habilitado { get; set; }
        public string Dominio { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string UrlRedirecionamento { get; set; }
        public ETipoProvedor TipoProvedor { get; set; }

        public enum ETipoProvedor
        {
            Microsoft = 0,
            B2C = 1
        }
    }
}

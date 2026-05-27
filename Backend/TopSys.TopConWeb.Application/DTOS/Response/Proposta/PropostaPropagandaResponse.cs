using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta
{
    public class PropostaPropagandaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ativa { get; set; }

    }
}

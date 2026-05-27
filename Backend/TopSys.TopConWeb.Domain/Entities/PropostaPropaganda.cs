using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaPropaganda
    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ativa { get; set; }

    }
}

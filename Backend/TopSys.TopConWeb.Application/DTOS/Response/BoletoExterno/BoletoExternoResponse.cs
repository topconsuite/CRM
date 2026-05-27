using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.BoletoExterno
{
    public class BoletoExternoResponse
    {
        public Guid Id { get; set; }
        public string Chave { get; set; }
        public int Sequencia { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataHora { get; set; }
    }
}

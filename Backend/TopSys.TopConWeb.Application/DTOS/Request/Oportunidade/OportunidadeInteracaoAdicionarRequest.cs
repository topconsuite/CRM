using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Oportunidade
{
    public class OportunidadeInteracaoAdicionarRequest
    {
        public int Usina { get; set; }
        public int NumeroOportunidade { get; set; }
        public int AnoOportunidade { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public DateTime DataRetorno { get; set; }
        public DateTime HoraRetorno { get; set; }
    }
}

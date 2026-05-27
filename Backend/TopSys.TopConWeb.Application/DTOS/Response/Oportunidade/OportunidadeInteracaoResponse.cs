using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Oportunidade
{
    public class OportunidadeInteracaoResponse
    {
        public Guid Id { get; set; }
        public int Usina { get; set; }
        public int NumeroOportunidade { get; set; }
        public int AnoOportunidade { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public DateTime? DataRetorno { get; set; }
        public TimeSpan? HoraRetorno { get; set; }
        public string IdCadastro { get; set; }
    }
}

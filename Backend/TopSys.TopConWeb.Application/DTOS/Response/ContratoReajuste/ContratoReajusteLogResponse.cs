using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste
{
    public class ContratoReajusteLogResponse
    {
        public int Usina { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public DateTime DataVigencia { get; set; }
        public int Sequencia { get; set; }
        public string Tipo { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public string Usuario { get; set; }
        public string Evento { get; set; }
        public string Complemento { get; set; }
    }
}

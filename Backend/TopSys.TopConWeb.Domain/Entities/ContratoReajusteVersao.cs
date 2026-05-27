using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoReajusteVersao
    {
        public int NumeroVersao { get; set; }
        public int Usina { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public DateTime DataVigencia { get; set; }
        public string Tipo { get; set; }
    }
}

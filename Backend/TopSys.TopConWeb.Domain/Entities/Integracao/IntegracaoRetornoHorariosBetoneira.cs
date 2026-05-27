using System;

namespace TopSys.TopConWeb.Domain.Entities.Integracao
{
    public class IntegracaoRetornoHorariosBetoneira
    {

        public IntegracaoRetornoHorariosBetoneira() { }

        public string Filial { get; set; }
        public string Interveniente { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroNota { get; set; }
        public string Serie { get; set; }
        public string Sequencia { get; set; }

        public string HoraSaidaUsina { get; set; }
        public string HoraChegadaObra { get; set; }
        public string HoraInicioDescarga { get; set; }
        public string HoraFimDescarga { get; set; }
        public string HoraSaidaObra { get; set; }
        public string HoraChegadaUsina { get; set; }

        public int VelocimetroInicial { get; set; }
        public int VelocimetroFinal { get; set; }
        public double AdicaoAgua { get; set; }
        public DateTime DataRecebimento { get; set; } 

        public string AssinaturaAutorizante { get; set; }

    }
}

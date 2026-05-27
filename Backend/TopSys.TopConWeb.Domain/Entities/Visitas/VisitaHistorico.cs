using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.Visitas
{
    public class VisitaHistorico
    {

        public Guid Id { get; set; }

        public int Usina { get; set; }
        public int NumeroVisita { get; set; }
        public int AnoVisita { get; set; }

        public string Tipo { get; set; } = "ATIVIDADE";
        public string Descricao { get; set; }

        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }

        public DateTime? DataRetorno { get; set; }
        public TimeSpan? HoraRetorno { get; set; }

        public string IdCadastro { get; set; }

        public EVisitaHistoricoTipo TipoHistorico
        {
            get
            {
                switch (this.Tipo)
                {
                    case "ATIVIDADE":
                        return EVisitaHistoricoTipo.AtividadeRealizada;
                    case "INTERACAO":
                        return EVisitaHistoricoTipo.InteracaoCliente;
                    case "CHAMADA":
                        return EVisitaHistoricoTipo.Chamada;
                    default:
                        return EVisitaHistoricoTipo.AtividadeRealizada;
                }
            }

            set
            {
    
                if (value == EVisitaHistoricoTipo.AtividadeRealizada)
                    this.Tipo = "ATIVIDADE";

                if (value == EVisitaHistoricoTipo.InteracaoCliente)
                    this.Tipo = "INTERACAO";

                if (value == EVisitaHistoricoTipo.Chamada)
                    this.Tipo = "CHAMADA";

            }

        }

    }
}

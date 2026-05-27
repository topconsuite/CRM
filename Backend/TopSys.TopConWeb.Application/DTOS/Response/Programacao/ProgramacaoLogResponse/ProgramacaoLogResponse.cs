using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoLogResponse
{
    public class ProgramacaoLogResponse
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int ProgramacaoSequencia { get; set; }

        public int? PropostaAno { get; set; } = 0;

        public int? PropostaNumero { get; set; } = 0;

        public int? ContratoAno { get; set; } = 0;

        public int? ContratoNumero { get; set; } = 0;

        public DateTime DataHora { get; set; }

        public string Horario { get; set; }

        public string Usuario { get; set; }

        public string Evento { get; set; }

        public int Sequencia { get; set; }

        public string Complemento { get; set; }

        public string Descricao { get; set; }
    }
}

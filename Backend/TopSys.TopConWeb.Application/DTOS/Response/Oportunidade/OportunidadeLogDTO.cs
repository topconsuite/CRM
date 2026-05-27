using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Oportunidade
{
    public class OportunidadeLogDTO
    {

        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public int Sequencia { get; set; }

        public string Tipo { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public string Usuario { get; set; }

        public string Evento { get; set; }

        public string Complemento { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.IntervenienteHistorico.Inclusao
{
    public class IntervenienteHistoricoRequest
    {
        public int CodigoInterveniente { get; set; }
        public DateTime Data { get; set; }
        //public string Hora { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataPrevistaDeRetorno { get; set; }
        public string HoraPrevistaDeRetornoString { get; set; }
        public string Vinculo { get; set; } = "";
        public int EmpresaCodigo { get; set; }
        public int DocumentoTipo { get; set; }
        public string DocumentoSerie { get; set; }
        public int DocumentoNumero { get; set; }
        public string DocumentoSequencia { get; set; }
        public string IdAtual { get; set; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.IntervenienteHistorico
{
    public class IntervenienteHistoricoResponse
    {
        public int CodigoInterveniente { get; set; }
        public int SequenciaHistorico { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan? Hora { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataPrevistaDeRetorno { get; set; }
        public TimeSpan? HoraPrevistaDeRetorno { get; set; }
        public DateTime? DataDeRetorno { get; set; }
        public TimeSpan? HoraDeRetorno { get; set; }
        public string Vinculo { get; set; }
        public int EmpresaCodigo { get; set; }
        public int DocumentoTipo { get; set; }
        public string DocumentoSerie { get; set; }
        public int DocumentoNumero { get; set; }
        public string DocumentoSequencia { get; set; }
        public string IdCadastro { get; set; }
        public string IdAtual { get; set; }
    }
}

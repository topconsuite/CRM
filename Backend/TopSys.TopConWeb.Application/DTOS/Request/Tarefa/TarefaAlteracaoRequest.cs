using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Tarefa
{
    public class TarefaAlteracaoRequest
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool DiaInteiro { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan Horario { get; set; }
        public string Observacao { get; set; }
        public string Contato { get; set; }
        public int DddTelefone { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
        public bool Finalizado { get; set; }
        public string Providencia { get; set; }
        public string Conclusao { get; set; }
        public int UsinaCodigo { get; set; }
        public int AnoVisita { get; set; }
        public int NumeroVisita { get; set; }
        public int AnoLead { get; set; }
        public int NumeroLead { get; set; }
        public int AnoOportunidade { get; set; }
        public int NumeroOportunidade { get; set; }

    }
}

using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaItemLog
    {
        public PropostaItemLog(string user, string source, string script, string payload = "")
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            Tabela = "con_proposta_item";
            User = user;
            Source = source;
            Script = script;
            Payload = payload;
        }

        public PropostaItemLog(int usina, int obraNumero, int propostaNumero, int propostaAno, int sequencia, int numeroVersao, string user, string source, string script, string payload = "")
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            Usina = usina;
            ObraNumero = obraNumero;
            PropostaNumero = propostaNumero;
            PropostaAno = propostaAno;
            Sequencia = sequencia;
            NumeroVersao = numeroVersao;
            Tabela = numeroVersao > 0 ? "con_proposta_item_versao" : "con_proposta_item";
            User = user;
            Source = source;
            Script = script;
            Payload = payload;
        }

        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public int Usina { get; set; }
        public int ObraNumero { get; set; }
        public int PropostaNumero { get; set; }
        public int PropostaAno { get; set; }
        public int Sequencia { get; set; }
        public int NumeroVersao { get; set; }
        public string Tabela { get; set; }
        public string User { get; set; }
        public string Source { get; set; }
        public string Script { get; set; }
        public string Payload { get; set; }
    }
}

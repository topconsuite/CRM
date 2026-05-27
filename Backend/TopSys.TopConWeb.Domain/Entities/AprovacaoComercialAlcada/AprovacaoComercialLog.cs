using System;

namespace TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada
{
    public class AprovacaoComercialLog
    {
        public AprovacaoComercialLog(string tabela, string source, string script, string payload = "")
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            Tabela = tabela;
            Source = source;
            Script = script;
            Payload = payload;
        }

        public AprovacaoComercialLog(int obraUsina, int obraNumero, int obraVersao, string tabela, string source, string script, string payload = "")
        {
            Id = Guid.NewGuid();
            Data = DateTime.Now;
            ObraUsina = obraUsina;
            ObraNumero = obraNumero;
            ObraVersao = obraVersao;
            Tabela = tabela;
            Source = source;
            Script = script;
            Payload = payload;
        }

        public Guid Id { get; set; }
        public DateTime Data { get; set; }

        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }
        public int ObraVersao { get; set; }
        public string Tabela { get; set; }
        
        public string Source { get; set; }
        public string Script { get; set; }
        public string Payload { get; set; }

    }
}

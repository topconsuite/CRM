using System;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso
{
    public class GrupoAcesso
    {
        public int Codigo { get; set; }

        public int Usina { get; set; }

        public string Descricao { get; set; }

        public DateTime? CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }

        public virtual ICollection<LiberacaoAcesso> LiberacoesAcessos { get; set; }
    }
}

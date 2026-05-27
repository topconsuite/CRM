using System;
using System.Collections.Generic;
namespace TopSys.TopConWeb.Application.DTOS.Response.LiberacaoAcesso
{
    public class GrupoAcessoResponse
    {
        public int Codigo { get; set; }

        public int Usina { get; set; }

        public string Descricao { get; set; }

        public DateTime? CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }

        public virtual ICollection<LiberacaoAcessoResponse> LiberacoesAcessos { get; set; }
    }
}

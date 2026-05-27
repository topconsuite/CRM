using System;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso
{
    public class GrupoAcessoAlteracaoRequest
    {
        public int Codigo { get; set; }

        public int Usina { get; set; }

        public string Descricao { get; set; }

        public DateTime? CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }

        public ICollection<LiberacaoAcessoDTO> LiberacoesAcessos { get; set; }
    }
}

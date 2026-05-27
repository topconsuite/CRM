using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso
{
    public class GrupoAcessoInclusaoRequest
    {
        public int Usina { get; set; }

        public string Descricao { get; set; }

        public ICollection<LiberacaoAcessoDTO> LiberacoesAcessos { get; set; }
    }
}

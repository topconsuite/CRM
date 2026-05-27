using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;

namespace TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Alteracao
{
    public class GrupoEconomicoAlteracaoRequest
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public float LimiteValor { get; set; }
        public DateTime? LimiteData { get; set; }
        public int? BloqueioMotivoCodigo { get; set; } = 0;
        public string BloqueioObservacao { get; set; } = "";
        public virtual CadastroGeralDTO BloqueioMotivo { get; set; }
        public IEnumerable<ClienteDTO> Clientes { get; set; }
    }
}

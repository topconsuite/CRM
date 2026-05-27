using System;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Interveniente
{
    public class GrupoEconomicoDTO
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public float LimiteValor { get; set; }
        public DateTime? LimiteData { get; set; }
        public int? BloqueioMotivoCodigo { get; set; } = 0;
        public string BloqueioObservacao { get; set; } = "";
        public virtual CadastroGeralDTO BloqueioMotivo { get; set; }
    }
}

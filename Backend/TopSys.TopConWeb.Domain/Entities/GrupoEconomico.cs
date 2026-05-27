using System;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class GrupoEconomico
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public float LimiteValor { get; set; }

        public DateTime? LimiteData { get; set; }

        public int? BloqueioMotivoCodigo { get; set; } = 0;

        public string BloqueioObservacao { get; set; } = "";

        public virtual CadastroGeral BloqueioMotivo { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public virtual ICollection<Interveniente> Clientes { get; set; }
    }
}

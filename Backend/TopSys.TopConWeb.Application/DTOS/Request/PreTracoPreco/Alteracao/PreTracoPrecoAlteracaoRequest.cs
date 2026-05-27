using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;

namespace TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco.Alteracao
{
    public class PreTracoPrecoAlteracaoRequest
    {

        public string Id { get; set; }

        public int UsinaCodigo { get; set; }

        public int UsoCodigo { get; set; }

        public int ResistenciaTipoCodigo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public int PedraCodigo { get; set; }

        public int SlumpCodigo { get; set; }

        public float CustoMaterial { get; set; }
        public float ValorServico { get; set; }
        public float Markup { get; set; }
        public float M3Preco { get; set; }

        public string IDCiencia { get; set; }
        public DateTime? DataCiencia { get; set; }

        public string TracoEspecificacao { get; set; }
        public string ExternalID { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TracoPrecoResponse TracoPrecoVigente { get; set; }

    }
}

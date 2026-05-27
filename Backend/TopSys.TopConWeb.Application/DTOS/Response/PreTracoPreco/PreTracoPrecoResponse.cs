using System;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Response.PreTracoPreco
{
    public class PreTracoPrecoResponse
    {

        public Guid Id { get; set; }

        public int UsinaCodigo { get; set; }
        public virtual UsinaDTO Usina { get; set; }

        public int UsoCodigo { get; set; }
        public virtual UsoDTO Uso { get; set; }

        public int ResistenciaTipoCodigo { get; set; }
        public ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public int PedraCodigo { get; set; }
        public virtual PedraDTO Pedra { get; set; }

        public int SlumpCodigo { get; set; }
        public virtual SlumpDTO Slump { get; set; }

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

        public int? NumeracaoProduto { get; set; }

        public virtual TracoPrecoResponse TracoPrecoVigente { get; set; }

    }
}

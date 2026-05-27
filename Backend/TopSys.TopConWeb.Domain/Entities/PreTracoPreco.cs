using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PreTracoPreco
    {

        public Guid Id { get; set; }

        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }

        public int UsoCodigo { get; set; }
        public virtual Uso Uso { get; set; }

        public int ResistenciaTipoCodigo { get; set; }
        public ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public int PedraCodigo { get; set; }
        public virtual Pedra Pedra { get; set; }

        public int SlumpCodigo { get; set; }
        public virtual SlumpReal Slump { get; set; }

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

        public void AjustarValorServicoPorPorcentagem(float valor, float valorServico)
        {
            valorServico += (valorServico * (valor / 100));
            var custoServico = CustoMaterial + valorServico;
            M3Preco = (custoServico / (100 - Markup)) * 100;
        }

        public void AjustarValorServicoPorReais(float valor, float valorServico)
        {
            valorServico = (valorServico + valor) < 0 ? 0 : (valorServico + valor);
            var custoServico = CustoMaterial + valorServico;
            M3Preco = (custoServico / (100 - Markup)) * 100;
        }

        public void AjustarValorServicoPorValorFixo(float valor)
        {
            var custoServico = CustoMaterial + valor;
            M3Preco = (custoServico / (100 - Markup)) * 100;
        }

        public void AjustarMantendoPrecoVenda(float m3PrecoAnterior)
        {
            M3Preco = m3PrecoAnterior;
        }

        public void AjustarValorMarkupPorPorcentagem(float valor, float valorServico)
        {
            Markup += valor;
            var custoServico = CustoMaterial + valorServico;
            M3Preco = (custoServico / (100 - (Markup < 0 ? 0 : Markup))) * 100;
        }

        public void AjustarValorMarkupPorPorcentagemFixa(float valor, float valorServico)
        {
            if (Markup == valor)
                return;

            var custoServico = CustoMaterial + valorServico;
            M3Preco = (custoServico / (100 - valor)) * 100;
            Markup = valor;
        }

    }
    
}

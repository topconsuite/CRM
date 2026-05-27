using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TracoPreco
    {

        public int NumeroTabela { get; set; }

        public int UsinaBaseCodigo { get; set; }
        public virtual Usina UsinaBase { get; set; }

        public int VendedorRepresentanteCodigo { get; set; }
        public virtual Vendedor VendedorRepresentante { get; set; }

        public int UsoCodigo { get; set; }
        public virtual Uso Uso { get; set; }

        public int ResistenciaTipoCodigo { get; set; }
        public virtual ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }
        
        public int PedraCodigo { get; set; }
        public virtual Pedra Pedra { get; set; }

        public int SlumpCodigo { get; set; }
        public virtual SlumpReal Slump { get; set; }

        public float M3Preco { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public DateTime? DataFinalVigencia { get; set; }

        public float M3PrecoRecalculo { get; set; }

        public float PercentualVariacao { get; set; }

        public DateTime? DataRecalculo { get; set; }

        public float CustoMaterial { get; set; }

        public float Markup { get; set; }

        public string TracoEspecificacao { get; set; }

        public float ComissaoPercentualServico { get; set; }

        public float ComissaoPercentualSobrePreco { get; set; }

        public string ComissaoServicoSobrePreco { get; set; }   // VALIDAR NOME DO CAMPO

        public string ComissaoSobreMaior { get; set; }          // VALIDAR NOME DO CAMPO

        public int? UsinaReferenciaCodigo { get; set; } = 0;
        public virtual Usina UsinaReferencia { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

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

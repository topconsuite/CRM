using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Mercadoria
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string ProdutoServico { get; set; }

        public int NumeracaoProduto { get; set; }
        
        public string IdExterno { get; set; }

        public static string GerarCodigoMercadoriaTraco(int idUso, int idPedra, int idSlump, ResistenciaTipo resistencia, float mpa, int consumo)
        {
            var codProduto = "";
            if (resistencia != null)
            {
                switch (resistencia.Vinculo)
                {
                    case Enums.EResistenciaVinculoTipo.Inexistente:
                        codProduto = "0";
                        break;
                    case Enums.EResistenciaVinculoTipo.Mpa:
                        codProduto = mpa.ToString("0.0");
                        break;
                    case Enums.EResistenciaVinculoTipo.Consumo:
                        codProduto = consumo.ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (mpa != 0)
                    codProduto = mpa.ToString("0.0");
                else
                    codProduto = consumo.ToString();
            }

            codProduto = $"{codProduto}/{idPedra}/{idSlump}/{idUso}";

            if (resistencia != null)
                codProduto = codProduto + $"/{resistencia.Codigo}";

            return codProduto;
        }
    }
}

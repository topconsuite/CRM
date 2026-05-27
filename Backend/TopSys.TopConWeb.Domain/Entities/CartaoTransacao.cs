using System;
using System.Collections.Generic;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CartaoTransacao
    {
        public int Id { get; set; }
        public string Origem { get; set; }
        public Guid TransacaoId { get; set; }
        public Guid PedidoId { get; set; }
        public string Status { get; set; }
        public string EstabelecimentoCod { get; set; }
        public Guid MerchantId { get; set; }
        public string TerminalNumero { get; set; }
        public string AutorizacaoNumero { get; set; }
        public string CartaoNumero { get; set; }

        public int CartaoNumeroAsInteger
        {
            get
            {
                int.TryParse(CartaoNumero, out int value);
                return value;
             }
        }

        public DateTime TransacaoDataHora { get; set; }
        public int StatusProcesso { get; set; }

        public float Valor { get; set; }

        public string TransacaoTipo { get; set; }
        public string ProdutoNome { get; set; }
        public string SubProdutoNome { get; set; }
        public string BandeiraNome { get; set; }

        public int QuantidadeParcelas { get; set; }

        public float ValorPorParcela
        {
            get { return (float)Math.Round(Valor / QuantidadeParcelas, 2); }
        }

        public IList<float> ValoresParcelas
        {
            get
            {
                List<float> valoresParcelas = new List<float>();
                var valorParcelaAcumaldo = 0.0;
                var valorPorParcela = ValorPorParcela;
                for (int i = 1; i <= QuantidadeParcelas; i++)
                {
                    if (i == QuantidadeParcelas)
                        valoresParcelas.Add((float)Math.Round(Valor - valorParcelaAcumaldo, 2));
                    else
                        valoresParcelas.Add(valorPorParcela);

                    valorParcelaAcumaldo += valorPorParcela;
                }
                return valoresParcelas;
            }
        }

        public bool OrigemCieLio()
        {
            return Origem == "cielo_lio";
        }

        public bool JaVinculado()
        {
            return StatusProcesso != 0; 
        }

        public bool FormaPagamentoCredito()
        {
            return ProdutoNome.ToLower() == "credito";
        }

    }
}

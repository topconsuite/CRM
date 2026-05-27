using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TituloContasAReceber : IQueryResult
    {
        public int EmpresaCodigo { get; set; }
        public int DocumentoTipoCodigo { get; set; }
        public string DocumentoSerie { get; set; }
        public long DocumentoNumero { get; set; }
        public string DocumentoSequencia { get; set; }
        public int BancoCodigoOficial { get; set; }
        public int BancoNumeroAgencia { get; set; }
        public long BancoNumeroConta { get; set; }

        // TEVE QUE SER TROCADO PARA BYTE, PORQUE O ENTITY E O MYSQL TEM UM PROBLEMA COM TINY SIGNED
        public byte BancoDvConta { get; set; }
        public int Desdobramento { get; set; }
        public int IntervenienteCodigo { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataOperacao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataVencimentoOriginal { get; set; }
        public float Valor { get; set; }
        public float SomaRecebimentos { get; set; }
        public float Saldo { get; set; }
        public int OperacaoFinanceiraCodigo { get; set; }
        public int CentroCustoCodigo { get; set; }
        public string NossoNumero { get; set; }
        public int ContratoUsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoVersao { get; set; }
        public int ContratoNumero { get; set; }
        public string CartaoNumero { get; set; }
        public string CartaoAutorizacao { get; set; }
        public string CartaoConciliado { get; set; } = "";
        public int Alocado { get; set; }
        public long IdMovimentoBanco { get; set; }

        //Para integracao Publica

        public int Situacao { get; set; } = 0;
        public DateTime? DataSituacao { get; set; }
        public string Observacao { get; set; } = "";
        public int BancoPortador { get; set; } = 0;
        public DateTime? DataLiquidacao { get; set; }
        public int OperacaoLiquidacao { get; set; } = 0;
        public float LiquidacaoJuros { get; set; } = 0;
        public float LiquidacaoDesconto { get; set; } = 0;
        public float LiquidacaoDespesas { get; set; } = 0;
        public float LiquidacaoValorRecebido { get; set; } = 0;
        public int BancoLiquidacao { get; set; } = 0;
        public DateTime? DataAtualizacao { get; set; }
        public float ValorBruto { get; set; }
        public float ValorRetencoes { get; set; }
        public long LiquidacaoLoteBaixa { get; set; } = 0;
        public DateTime? DataLiquidacaoCliente { get; set; }
        public float ValorMoraNaoLiquidado { get; set; } = 0;
        public float MultaMoraCalculado { get; set; } = 0;
        public float DescontoMora { get; set; } = 0;
        public long DocumentoLiquidacao { get; set; } = 0;
        public int AtualizaBanco { get; set; } = 0;
        public float JurosMoraCalculado { get; set; } = 0;
        public string IdLiquidacao { get; set; } = "";
        public int MeioPagamento { get; set; } = 0;
        public int CondicaoPagamento { get; set; } = 0;
        public string LiquidadoEmCheque { get; set; }
        public string Devolucao { get; set; }
        public long LoteOrigem { get; set; }
        public DateTime? DataUtilVencimento { get; set; }
        public string LinhaDigitavelBoleto { get; set; }
        
        public float ValorCbs { get; set; }

        public float ValorIbs { get; set; }

        public float ValorIs { get; set; }

        public string TotalizaCbsIbsIs { get; set; }
        
        public string CodigoBarrasBoleto { get; set; }
        
        public string NossoNumeroIntermediarioBoleto { get; set; }

        // Referente a Fatura, somente dapper
        public ulong NumeroNFSE { get; set; }

        // Dados formatados, somente dapper
        public double? ValorToDouble { get; set; }
        public double? SomaRecebimentosToDouble { get; set; }
        public double? SaldoToDouble { get; set; }
        
        //Titulos que compoem junção, somente dapper
        public IEnumerable<ChaveTituloContasAReceber> ChaveTitulosDaJuncao { get; set; }

        // Validacoes
        public bool EstaLiquidado { get => DataLiquidacao != null && LiquidacaoLoteBaixa != 0; }
        public int BancoBoleto
        {
            get
            {
                if (LinhaDigitavelBoleto.Length < 3)
                    return 0;

                int.TryParse(LinhaDigitavelBoleto.Substring(0, 3), out var codigoBanco);

                return codigoBanco;
            }
        }
    }
}

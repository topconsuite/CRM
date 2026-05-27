using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TituloContasAReceberMap : EntityTypeConfiguration<TituloContasAReceber>
    {
        public TituloContasAReceberMap()
        {
            ToTable("topsys.fin_car");

            HasKey(t => new {
                t.EmpresaCodigo,
                t.DocumentoTipoCodigo,
                t.DocumentoSerie,
                t.DocumentoNumero,
                t.DocumentoSequencia,
                t.BancoCodigoOficial,
                t.BancoNumeroAgencia,
                t.BancoNumeroConta,
                t.BancoDvConta,
                t.Desdobramento
            });

            var i = 0;

            Property(t => t.EmpresaCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("emp");

            Property(t => t.DocumentoTipoCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("tp_doc");

            Property(t => t.DocumentoSerie)
                .HasColumnOrder(i++)
                .HasColumnName("ser_doc");

            Property(t => t.DocumentoNumero)
                .HasColumnOrder(i++)
                .HasColumnName("num_doc");

            Property(t => t.DocumentoSequencia)
                .HasColumnOrder(i++)
                .HasColumnName("seq");

            Property(t => t.BancoCodigoOficial)
                .HasColumnOrder(i++)
                .HasColumnName("cod_banco_band");

            Property(t => t.BancoNumeroAgencia)
                .HasColumnOrder(i++)
                .HasColumnName("num_agencia");

            Property(t => t.BancoNumeroConta)
                .HasColumnOrder(i++)
                .HasColumnName("num_conta");

            Property(t => t.BancoDvConta)
                .HasColumnOrder(i++)
                .HasColumnName("dv_conta");

            Property(t => t.Desdobramento)
                .HasColumnOrder(i++)
                .HasColumnName("desdo");

            Property(t => t.IntervenienteCodigo)
                .HasColumnName("cli");

            Property(t => t.DataEmissao)
                .HasColumnName("dt_emi");

            Property(t => t.DataOperacao)
                .HasColumnName("dt_oper");

            Property(t => t.DataVencimento)
                .HasColumnName("dt_vcto");

            Property(t => t.DataVencimentoOriginal)
                .HasColumnName("dt_vencto_orig");

            Property(t => t.Valor)
                .HasColumnName("vl");

            Property(t => t.SomaRecebimentos)
                .HasColumnName("soma_recbtos");

            Property(t => t.Saldo)
                .HasColumnName("sal");

            Property(t => t.OperacaoFinanceiraCodigo)
                .HasColumnName("oper");

            Property(t => t.CentroCustoCodigo)
                .HasColumnName("cc");

            Property(t => t.NossoNumero)
                .HasColumnName("nosso_num");

            Property(t => t.ContratoUsinaCodigo)
                .HasColumnName("usina_contrato");

            Property(t => t.ContratoAno)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoVersao)
                .HasColumnName("versao_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnName("num_contrato");

            Property(t => t.CartaoNumero)
                .HasColumnName("num_cartao");

            Property(t => t.CartaoAutorizacao)
                .HasColumnName("num_autorizacao");

            Property(t => t.CartaoConciliado)
                .HasColumnName("conc_cartao");

            Property(t => t.Alocado)
                .HasColumnName("alocado");

            Property(t => t.IdMovimentoBanco)
                .HasColumnName("id_mov_bco");

            //Para integracao publica

            Property(t => t.Situacao)
                .HasColumnName("sit");

            Property(t => t.DataSituacao)
                .HasColumnName("dt_sit");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.BancoPortador)
                .HasColumnName("bco_port");

            Property(t => t.DataLiquidacao)
                .HasColumnName("liq_dt");

            Property(t => t.OperacaoLiquidacao)
                .HasColumnName("liq_oper");

            Property(t => t.LiquidacaoJuros)
                .HasColumnName("liq_juros");

            Property(t => t.LiquidacaoDesconto)
                .HasColumnName("liq_desc");

            Property(t => t.LiquidacaoDespesas)
                .HasColumnName("liq_desp");

            Property(t => t.LiquidacaoValorRecebido)
                .HasColumnName("liq_vl_rec");
            
            Property(t => t.ValorBruto)
                .HasColumnName("vl_bruto");
            
            Property(t => t.ValorRetencoes)
                .HasColumnName("vl_retencoes");
            
            Property(t => t.BancoLiquidacao)
                .HasColumnName("liq_bco");

            Property(t => t.LiquidacaoLoteBaixa)
                .HasColumnName("liq_lot_bx");

            Property(t => t.DataLiquidacaoCliente)
                .HasColumnName("liq_dt_cli");

            Property(t => t.ValorMoraNaoLiquidado)
                .HasColumnName("vlr_mora_n_liq");

            Property(t => t.MultaMoraCalculado)
                .HasColumnName("multa_mora_calc");

            Property(t => t.DescontoMora)
                .HasColumnName("mora_des_acr_ef");

            Property(t => t.DocumentoLiquidacao)
                .HasColumnName("liq_doc");

            Property(t => t.AtualizaBanco)
                .HasColumnName("at_bco");

            Property(t => t.JurosMoraCalculado)
                .HasColumnName("juros_mora_calc");

            Property(t => t.IdLiquidacao)
                .HasColumnName("id_liq");

            Property(t => t.DataAtualizacao)
                .HasColumnName("atualizado_em")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            Property(t => t.MeioPagamento)
                .HasColumnName("meio_pagamento");

            Property(t => t.LiquidadoEmCheque)
                .HasColumnName("liq_cheque");

            Property(t => t.Devolucao)
                .HasColumnName("devolucao");

            Property(t => t.LoteOrigem)
                .HasColumnName("lote_origem");

            Property(t => t.DataUtilVencimento)
                .HasColumnName("dt_util_vencto");

            Property(t => t.LinhaDigitavelBoleto)
                .HasColumnName("linha_dig");
            
            Property(t => t.ValorCbs)
                .HasColumnName("vl_cbs");
            
            Property(t => t.ValorIbs)
                .HasColumnName("vl_ibs");
            
            Property(t => t.ValorIs)
                .HasColumnName("vl_is");
            
            Property(t => t.TotalizaCbsIbsIs)
                .HasColumnName("totaliza_fin_reform_trib");
            
            Property(t => t.CodigoBarrasBoleto)
                .HasColumnName("barra");
            
            Property(t => t.NossoNumeroIntermediarioBoleto)
                .HasColumnName("noss_num_interm");

            Ignore(t => t.ValorToDouble);

            Ignore(t => t.SomaRecebimentosToDouble);

            Ignore(t => t.SaldoToDouble);
            
            Ignore(t => t.ChaveTitulosDaJuncao);

            Ignore(t => t.CondicaoPagamento);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraBombaVersaoMap : EntityTypeConfiguration<ObraBombaVersao>
    {
        public ObraBombaVersaoMap()
        {
            ToTable("topsys.con_prop_bomba_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo, t.Sequencia });

            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);
            Ignore(t => t.BombaPropria);
            Ignore(t => t.FaturamentoDireto);
            Ignore(t => t.AlugadaPeloCliente);
            Ignore(t => t.Inativo);

            HasOptional(t => t.BombaTipo)
                .WithMany()
                .HasForeignKey(t => t.BombaTipoCodigo);

            HasOptional(t => t.Terceiro)
                .WithMany()
                .HasForeignKey(t => t.TerceiroCodigo);

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(2)
                .HasColumnName("no_obra");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq");

            Property(t => t.BombaTipoCodigo)
                .HasColumnName("tipo_bomba");

            Property(t => t.BombaPropriaSimNao)
                .HasColumnName("bomba_propria");

            Property(t => t.TerceiroCodigo)
                .HasColumnName("terceiro");

            Property(t => t.FaturamentoDiretoSimNao)
                .HasColumnName("fat_direto");

            Property(t => t.AlugadaPeloClienteSimNao)
                .HasColumnName("alugadapcliente");

            Property(t => t.M3TabelaAte)
                .HasColumnName("m3_pr_tab");

            Property(t => t.TaxaMinimaPrecoTabela)
                .HasColumnName("taxa_minima_tab");

            Property(t => t.M3PrecoTabela)
                .HasColumnName("pr_m3_tab");

            Property(t => t.M3PropostoAte)
                .HasColumnName("m3_pr_prop");

            Property(t => t.TaxaMinimaPrecoProposto)
                .HasColumnName("txa_min_pr_prop");

            Property(t => t.M3PrecoProposto)
                .HasColumnName("pr_m3_bomb_pr_p");

            Property(t => t.TaxaMinimaReajustadaAnterior)
                .HasColumnName("taxa_reajust_ant");

            Property(t => t.M3ReajustadoAteAnterior)
                .HasColumnName("m3_pr_reajust_ant");

            Property(t => t.M3PrecoReajustadoAnterior)
                .HasColumnName("pr_m3_reajust_ant");

            Property(t => t.TaxaMinimaReajustadaAtual)
                .HasColumnName("taxa_reajustada");

            Property(t => t.M3ReajustadoAteAtual)
                .HasColumnName("m3_pr_reajustada");

            Property(t => t.M3PrecoReajustadoAtual)
                .HasColumnName("pr_m3_reajustada");

            Property(t => t.DataUltimoReajuste)
                .HasColumnName("dt_ult_reajuste");

            Property(t => t.DescontoPercentual)
                .HasColumnName("pct_descto");

            Property(t => t.DescontoSolicitante)
                .HasColumnName("id_aprovacao");

            Property(t => t.AprovacaoVerbal)
                .HasColumnName("aprov_verbal");

            Property(t => t.AprovacaoObservacao)
                .HasColumnName("obs_aprov");

            Property(t => t.AprovacaoOperacao)
                .HasColumnName("status_aprov");

            Property(t => t.AprovacaoCiente)
                .HasColumnName("ciente_aprov");

            Property(t => t.Justificativa)
                .HasColumnName("just_aprov");

            Property(t => t.DistanciaTubulacao)
                .HasColumnName("dist_tub_bomba");

            Property(t => t.ValorAdicionalTubulacao)
                .HasColumnName("vlr_adic_tub");

            Property(t => t.PropostaAno)
                .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
                .HasColumnName("no_chamada");

            Property(t => t.TipoCalculo)
                .HasColumnName("tipo_calc");

            Property(t => t.HoraTipoCalculo)
                .HasColumnName("tipo_calc_hora");

            Property(t => t.HoraTaxaMinimaPrecoTabela)
                .HasColumnName("tx_min_hora_tab");

            Property(t => t.HoraTabelaAte)
                .HasColumnName("tempo_min_h_tab");

            Property(t => t.HoraPrecoTabela)
                .HasColumnName("vlr_hora_exc_tab");

            Property(t => t.HoraTaxaMinimaPrecoProposto)
                .HasColumnName("tx_min_hora_prop");

            Property(t => t.HoraPropostoAte)
                .HasColumnName("tempo_min_h_prop");

            Property(t => t.HoraPrecoProposto)
                .HasColumnName("vlr_hora_exc_prop");

            Property(t => t.HoraDescontoPercentual)
                .HasColumnName("pct_descto_hora");

            Property(t => t.ImpostoAplicadoEstadual)
                .HasColumnName("imposto_aplicado_estadual");

            Property(t => t.ImpostoAplicadoFederal)
                .HasColumnName("imposto_aplicado_federal");

            Property(t => t.IssDedutivel)
                .HasColumnName("iss_aplicado");

            Property(t => t.Ebitda)
                .HasColumnName("ebitda");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

        }
    }
}

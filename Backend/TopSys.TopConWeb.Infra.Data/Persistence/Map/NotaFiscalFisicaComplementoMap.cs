using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalFisicaComplementoMap : EntityTypeConfiguration<NotaFiscalFisicaComplemento>
    {
        public NotaFiscalFisicaComplementoMap()
        {
            ToTable("topsys.con_nf_complemento");

            HasKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia });

            Property(t => t.FilialCodigo)
                .HasColumnOrder(0)
                .HasColumnName("filial");

            Property(t => t.IntervenienteCodigo)
                .HasColumnOrder(1)
                .HasColumnName("interv");

            Property(t => t.TipoDocumentoCodigo)
                .HasColumnOrder(2)
                .HasColumnName("tp_doc");

            Property(t => t.Numero)
                .HasColumnOrder(3)
                .HasColumnName("num_nf");

            Property(t => t.Serie)
                .HasColumnOrder(4)
                .HasColumnName("serie");

            Property(t => t.Sequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq_nf");

            Property(t => t.TaxaPermanenciaValor)
                .HasColumnName("tx_permanencia");

            Property(t => t.ValorAdicionais)
                .HasColumnName("vlr_adicionais");

            Property(t => t.AdicionalKmValorTotal)
                .HasColumnName("vlr_tot_adic_km");

            Property(t => t.AdicionalRetornoConcretoTotal)
                .HasColumnName("vlr_tot_ret_con");

            Property(t => t.ValorDemaisServicos)
                .HasColumnName("vlr_demais_servicos");

            Property(t => t.ValorTotalCobranca)
                .HasColumnName("vlr_total_cobranca");

            Property(t => t.ValorUnitarioAdicionalFeriado)
                .HasColumnName("adic_feriado_vlr_unit");

            Property(t => t.ValorUnitarioAdicionalHoraExtra)
                .HasColumnName("adic_he_vlr_unit");

            Property(t => t.ValorAdicionalZmrc)
                .HasColumnName("adic_zmrc");

            Property(t => t.AdicionalTubulacaoExtra)
                .HasColumnName("vlr_total_tub_extra");

            Property(t => t.ConfirmaMoldagemRemota)
                .HasColumnName("confirm_mold_remota");

            Property(t => t.EquipamentoTransporteMateriaPrimaCodigo)
                .HasColumnName("equip_transp_mp");

            Property(t => t.HoraBombeamentoFim)
                .HasColumnName("hora_bomb_fim");

            Property(t => t.HoraBombeamentoInicio)
                .HasColumnName("hora_bomb_inicio");

            Property(t => t.HoraBombaPronta)
                .HasColumnName("hora_bomba_pronta");

            Property(t => t.HoraTrabalhadaEfetivamente)
                .HasColumnName("hora_trab_efetiva");

            Property(t => t.HoraTrabalhada)
                .HasColumnName("hora_trabalhada");

            Property(t => t.IdUsuarioMoldagemRemota)
                .HasColumnName("id_usuario_mold_remota");

            Property(t => t.JustificativaOrdemBt)
                .HasColumnName("just_ordem_bt");

            Property(t => t.LoteEmissao)
                .HasColumnName("lote_emissao");

            Property(t => t.MotivoMudancaTaxaPermanencia)
                .HasColumnName("mot_mud_tx_perm");

            Property(t => t.ObservacaoMoldagemRemota)
                .HasColumnName("obs_mold_remota");

            Property(t => t.OrdemBt)
                .HasColumnName("ordem_bt");

            Property(t => t.PercentualAdicionalZmrc)
                .HasColumnName("pct_adic_zmrc");

            Property(t => t.QuantidadeAdicionalKmRodado)
                .HasColumnName("qt_adic_km");

            Property(t => t.QuantidadeAdicionalRetornoConcreto)
                .HasColumnName("qt_adic_ret_con");

            Property(t => t.QuantidadeTaxaPermanencia)
                .HasColumnName("qt_tx_perm");

            Property(t => t.QuantidadeAdicionalFeriado)
                .HasColumnName("qtd_adic_feriado");

            Property(t => t.QuantidadeAdicionalHoraExtra)
                .HasColumnName("qtd_adic_he");

            Property(t => t.ReaproveitamentoProgramacao)
                .HasColumnName("reaproveitamento_prog");

            Property(t => t.QuantidadeTaxaPermanenciaBomba)
                .HasColumnName("temp_cob_ociosid_bomb");

            Property(t => t.ValorUnitarioTaxaPermanenciaBomba)
                .HasColumnName("tx_perm_bomb_vlr_unit");

            Property(t => t.TaxaPermanenciaBombaValor)
                .HasColumnName("tx_permanencia_bomba");

            Property(t => t.VersaoContrato)
                .HasColumnName("versao_contrato");

            Property(t => t.ValorUnitarioAdicionalKmRodado)
                .HasColumnName("vl_unit_adic_km");

            Property(t => t.ValorUnitarioAdicionalRetornoConcreto)
                .HasColumnName("vl_unit_ret_con");

            Property(t => t.ValorHoraTrabalhada)
                .HasColumnName("vlr_hora_trabalhada");

            Property(t => t.ValorVendaHoraBomba)
                .HasColumnName("vlr_venda_h");

            Property(t => t.ValorTotalHoraBomba)
                .HasColumnName("vlr_venda_total_h");

            Property(t => t.NumeracaoProduto)
                .HasColumnName("numeracao_produto");
        }
    }
}

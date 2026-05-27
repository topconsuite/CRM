using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class NotaFiscalFisicaRepository : RepositoryBase<NotaFiscalFisica>, INotaFiscalFisicaRepository
    {
        public NotaFiscalFisicaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public bool Emitida(Programacao programacao)
        {
            var notas = _context.NotasFiscaisFisicas
                .Where(t => t.ContratoUsinaCodigo == programacao.UsinaCodigo
                    && t.ContratoAno == programacao.ContratoAno
                    && t.ContratoNumero == programacao.ContratoNumero
                    && t.ProgramacaoSequencia == programacao.Sequencia)
                .ToList();

            return (notas?.Count ?? 0) > 0;
        }

        public PagedList<NotaFiscalFisica> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nf.filial {nameof(NotaFiscalFisica.FilialCodigo)}");
            sqlComando.Append($", nf.interv {nameof(NotaFiscalFisica.IntervenienteCodigo)}");
            sqlComando.Append($", nf.tp_doc {nameof(NotaFiscalFisica.TipoDocumentoCodigo)}");
            sqlComando.Append($", nf.num_nf {nameof(NotaFiscalFisica.Numero)}");
            sqlComando.Append($", nf.serie {nameof(NotaFiscalFisica.Serie)}");
            sqlComando.Append($", nf.seq_nf {nameof(NotaFiscalFisica.Sequencia)}");
            sqlComando.Append($", nf.usina_contrato {nameof(NotaFiscalFisica.ContratoUsinaCodigo)}");
            sqlComando.Append($", nf.num_contrato {nameof(NotaFiscalFisica.ContratoNumero)}");
            sqlComando.Append($", nf.ano_contrato {nameof(NotaFiscalFisica.ContratoAno)}");
            sqlComando.Append($", nf.seq_prog {nameof(NotaFiscalFisica.ProgramacaoSequencia)}");
            sqlComando.Append($", nf.motivo_cancel {nameof(NotaFiscalFisica.MotivoCancelamentoCodigo)}");
            sqlComando.Append($", nf.qtde_m3_bt {nameof(NotaFiscalFisica.Volume)}");
            sqlComando.Append($", nf.hr_saida_usina {nameof(NotaFiscalFisica.HoraSaidaUsina)}");
            sqlComando.Append($", nf.no_betoneira {nameof(NotaFiscalFisica.BetoneiraCodigo)}");
            sqlComando.Append($", nf.vlr_venda_m3 {nameof(NotaFiscalFisica.TracoValorUnitario)}");
            sqlComando.Append($", nf.vlr_venda_total {nameof(NotaFiscalFisica.TracoValorTotal)}");
            sqlComando.Append($", nf.vlr_bomba_total {nameof(NotaFiscalFisica.BombaValorTotal)}");
            sqlComando.Append($", nf.vlr_m3_fal {nameof(NotaFiscalFisica.M3FaltanteValor)}");
            sqlComando.Append($", nf.vibr_vlr_total {nameof(NotaFiscalFisica.VibradorValorTotal)}");
            sqlComando.Append($", nf.adic_he {nameof(NotaFiscalFisica.AdicionalHoraExtraValorTotal)}");
            sqlComando.Append($", nf.adic_feriado {nameof(NotaFiscalFisica.AdicionalFeriadoValorTotal)}");
            sqlComando.Append($", nf.adicao_agua {nameof(NotaFiscalFisica.AdicaoAgua)}");
            sqlComando.Append($", nf.ag_colocada_usi {nameof(NotaFiscalFisica.AguaColocadaNaUsina)}");
            sqlComando.Append($", nf.ag_colocar_obr {nameof(NotaFiscalFisica.AguaColocarNaObra)}");
            sqlComando.Append($", nf.atraso_entrega {nameof(NotaFiscalFisica.AtrasoEntrega)}");
            sqlComando.Append($", nf.aux_bombista {nameof(NotaFiscalFisica.AuxiliarBombista)}");
            sqlComando.Append($", nf.aux_bombista2 {nameof(NotaFiscalFisica.AuxiliarBombista2)}");
            sqlComando.Append($", nf.aux_bombista3 {nameof(NotaFiscalFisica.AuxiliarBombista3)}");
            sqlComando.Append($", nf.balanca {nameof(NotaFiscalFisica.Balanca)}");
            sqlComando.Append($", nf.bomba {nameof(NotaFiscalFisica.Bomba)}");
            sqlComando.Append($", nf.bombista {nameof(NotaFiscalFisica.Bombista)}");
            sqlComando.Append($", nf.chave_familia {nameof(NotaFiscalFisica.ChaveFamilia)}");
            sqlComando.Append($", nf.cimento {nameof(NotaFiscalFisica.Cimento)}");
            sqlComando.Append($", nf.cod_inconsist {nameof(NotaFiscalFisica.CodigoInconsistencia)}");
            sqlComando.Append($", nf.cod_integracao {nameof(NotaFiscalFisica.CodigoIntegracao)}");
            sqlComando.Append($", nf.cod_transp_mp {nameof(NotaFiscalFisica.CodigoTransportadorMateriaPrima)}");
            sqlComando.Append($", nf.codigos_cps {nameof(NotaFiscalFisica.CodigosCorpoProvas)}");
            sqlComando.Append($", nf.com_repres_dif {nameof(NotaFiscalFisica.ComissaoRepresentanteDiferenca)}");
            sqlComando.Append($", nf.com_repres_serv {nameof(NotaFiscalFisica.ComissaoRepresentanteServico)}");
            sqlComando.Append($", nf.comis_ger {nameof(NotaFiscalFisica.ComissaoGerada)}");
            sqlComando.Append($", nf.comis_aj_bomba {nameof(NotaFiscalFisica.ComissaoAjudanteBomba)}");
            sqlComando.Append($", nf.comis_bombista {nameof(NotaFiscalFisica.ComissaoBombista)}");
            sqlComando.Append($", nf.comis_motorista {nameof(NotaFiscalFisica.ComissaoMotorista)}");
            sqlComando.Append($", nf.com_repre_bomb {nameof(NotaFiscalFisica.ComissaoRepresentanteBomba)}");
            sqlComando.Append($", nf.com_repr_concr {nameof(NotaFiscalFisica.ComissaoRepresentanteConcreto)}");
            sqlComando.Append($", nf.comis_represent {nameof(NotaFiscalFisica.ComissaoRepresentante)}");
            sqlComando.Append($", nf.com_transp_mp {nameof(NotaFiscalFisica.ComissaoTransportadorMateriaPrima)}");
            sqlComando.Append($", nf.com_vend_dif {nameof(NotaFiscalFisica.ComissaoVendedorDiferenca)}");
            sqlComando.Append($", nf.com_vend_bomb {nameof(NotaFiscalFisica.ComissaoVendaBombista)}");
            sqlComando.Append($", nf.com_vend_padr {nameof(NotaFiscalFisica.ComissaoVendaPadrao)}");
            sqlComando.Append($", nf.com_vend_tx_extra {nameof(NotaFiscalFisica.ComissaoVendaTaxaExtra)}");
            sqlComando.Append($", nf.com_vend_vibr {nameof(NotaFiscalFisica.ComissaoVendaVibrador)}");
            sqlComando.Append($", nf.comis_vendedor {nameof(NotaFiscalFisica.ComissaoVendedor)}");
            sqlComando.Append($", nf.com_vend_concr {nameof(NotaFiscalFisica.ComissaoVendedorConcreto)}");
            sqlComando.Append($", nf.com_vend_serv {nameof(NotaFiscalFisica.ComissaoVendedorServico)}");
            sqlComando.Append($", nf.cp {nameof(NotaFiscalFisica.CorpoProva)}");
            sqlComando.Append($", nf.corte_agua_pm3 {nameof(NotaFiscalFisica.CorteAguaPorM3)}");
            sqlComando.Append($", nf.custo_conc_pes {nameof(NotaFiscalFisica.CustoConcretoPesado)}");
            sqlComando.Append($", nf.dt_base_comis {nameof(NotaFiscalFisica.DataBaseComissao)}");
            sqlComando.Append($", nf.dt_coleta_cp {nameof(NotaFiscalFisica.DataColetaCorpoProva)}");
            sqlComando.Append($", nf.dt_fatura {nameof(NotaFiscalFisica.DataFatura)}");
            sqlComando.Append($", nf.dt_prog {nameof(NotaFiscalFisica.DataProgramacao)}");
            sqlComando.Append($", nf.dt_prorrog_pend {nameof(NotaFiscalFisica.DataProrrogacaoPendencia)}");
            sqlComando.Append($", nf.data_remessa {nameof(NotaFiscalFisica.DataRemessa)}");
            sqlComando.Append($", nf.dt_vcto_1_parc {nameof(NotaFiscalFisica.DataVencimentoPrimeiraParcela)}");
            sqlComando.Append($", nf.descartado_cp {nameof(NotaFiscalFisica.DescartadoCorpoProva)}");
            sqlComando.Append($", nf.desvio {nameof(NotaFiscalFisica.Desvio)}");
            sqlComando.Append($", nf.especificacao_traco {nameof(NotaFiscalFisica.EspecificacaoTraco)}");
            sqlComando.Append($", nf.espera_inicio {nameof(NotaFiscalFisica.EsperaInicio)}");
            sqlComando.Append($", nf.espera_saida_ob {nameof(NotaFiscalFisica.EsperaSaidaObra)}");
            sqlComando.Append($", nf.filial_estoque {nameof(NotaFiscalFisica.FilialEstoque)}");
            sqlComando.Append($", nf.filial_fat {nameof(NotaFiscalFisica.FilialFaturamento)}");
            sqlComando.Append($", nf.hr_cheg_usina {nameof(NotaFiscalFisica.HoraChegadaUsina)}");
            sqlComando.Append($", nf.hora_fim_carga {nameof(NotaFiscalFisica.HoraFimCarga)}");
            sqlComando.Append($", nf.hora_ini_carga {nameof(NotaFiscalFisica.HoraInicioCarga)}");
            sqlComando.Append($", nf.hr_prevista {nameof(NotaFiscalFisica.HoraPrevista)}");
            sqlComando.Append($", nf.hr_recomend_utl {nameof(NotaFiscalFisica.HoraRecomendadaUtilizacao)}");
            sqlComando.Append($", nf.hr_saida_func {nameof(NotaFiscalFisica.HoraSaidaFuncionario)}");
            sqlComando.Append($", nf.hr_saida_obra {nameof(NotaFiscalFisica.HoraSaidaObra)}");
            sqlComando.Append($", nf.hr_saida_usina_efet {nameof(NotaFiscalFisica.HoraSaidaUsinaEfetiva)}");
            sqlComando.Append($", nf.horimetro_final {nameof(NotaFiscalFisica.HorimetroFinal)}");
            sqlComando.Append($", nf.horimetro_inicial {nameof(NotaFiscalFisica.HorimetroInicial)}");
            sqlComando.Append($", nf.horimetro_rodado {nameof(NotaFiscalFisica.HorimetroRodado)}");
            sqlComando.Append($", nf.hr_cheg_obra {nameof(NotaFiscalFisica.HoraChegadaObra)}");
            sqlComando.Append($", nf.hr_desc_final {nameof(NotaFiscalFisica.HoraDescargaFinal)}");
            sqlComando.Append($", nf.hr_desc_inic {nameof(NotaFiscalFisica.HoraDescargaInicial)}");
            sqlComando.Append($", nf.hr_solicitada {nameof(NotaFiscalFisica.HoraSolicitada)}");
            sqlComando.Append($", nf.id_aprov_dir {nameof(NotaFiscalFisica.IdAprovacaoDiretoria)}");
            sqlComando.Append($", nf.id_aprov_lab {nameof(NotaFiscalFisica.IdAprovacaoLaboratorio)}");
            sqlComando.Append($", nf.id_atual {nameof(NotaFiscalFisica.IdAtual)}");
            sqlComando.Append($", nf.id_cadast {nameof(NotaFiscalFisica.IdCadastro)}");
            sqlComando.Append($", nf.id_coleta_cp {nameof(NotaFiscalFisica.IdColetaCorpoProva)}");
            sqlComando.Append($", nf.import_gps {nameof(NotaFiscalFisica.ImportaGps)}");
            sqlComando.Append($", nf.import_rfid {nameof(NotaFiscalFisica.ImportaRfid)}");
            sqlComando.Append($", nf.importou_da_nf {nameof(NotaFiscalFisica.ImportouDaNotaFiscalRemessa)}");
            sqlComando.Append($", nf.inconsistencias {nameof(NotaFiscalFisica.Inconsistencias)}");
            sqlComando.Append($", nf.km_rodado {nameof(NotaFiscalFisica.KmRodado)}");
            sqlComando.Append($", nf.m3_bombeado {nameof(NotaFiscalFisica.M3Bombeado)}");
            sqlComando.Append($", nf.m3_bomb_cob {nameof(NotaFiscalFisica.M3BombeadoCobrado)}");
            sqlComando.Append($", nf.m3_faltantes {nameof(NotaFiscalFisica.M3Faltantes)}");
            sqlComando.Append($", nf.mao_obra_prop {nameof(NotaFiscalFisica.MaoDeObraPropria)}");
            sqlComando.Append($", nf.material_m3 {nameof(NotaFiscalFisica.MaterialM3)}");
            sqlComando.Append($", nf.material_total {nameof(NotaFiscalFisica.MaterialTotal)}");
            sqlComando.Append($", nf.minutos_desc {nameof(NotaFiscalFisica.MinutosDescarga)}");
            sqlComando.Append($", nf.motivo_atraso {nameof(NotaFiscalFisica.MotivoAtraso)}");
            sqlComando.Append($", nf.motivo_atraso_concr {nameof(NotaFiscalFisica.MotivoAtrasoConcretagem)}");
            sqlComando.Append($", nf.motivo_incons {nameof(NotaFiscalFisica.MotivoInconsistencia)}");
            sqlComando.Append($", nf.motorista {nameof(NotaFiscalFisica.Motorista)}");
            sqlComando.Append($", nf.num_fatura {nameof(NotaFiscalFisica.NumeroFatura)}");
            sqlComando.Append($", nf.no_faturamento {nameof(NotaFiscalFisica.NumeroFaturamento)}");
            sqlComando.Append($", nf.num_lacre {nameof(NotaFiscalFisica.NumeroLacre)}");
            sqlComando.Append($", nf.obs_entrega {nameof(NotaFiscalFisica.ObservacaoEntrega)}");
            sqlComando.Append($", nf.obs_aprov_km {nameof(NotaFiscalFisica.ObservacaoAprovacaoKm)}");
            sqlComando.Append($", nf.obs_cancel {nameof(NotaFiscalFisica.ObservacaoCancelamento)}");
            sqlComando.Append($", nf.obs_pesagem {nameof(NotaFiscalFisica.ObservacaoPesagem)}");
            sqlComando.Append($", nf.obs_traco {nameof(NotaFiscalFisica.ObservacaoTraco)}");
            sqlComando.Append($", nf.obs {nameof(NotaFiscalFisica.Observacao)}");
            sqlComando.Append($", nf.pendente {nameof(NotaFiscalFisica.Pendente)}");
            sqlComando.Append($", nf.pct_adic_feriad {nameof(NotaFiscalFisica.PercentualAdicionalFeriado)}");
            sqlComando.Append($", nf.pct_adic_he {nameof(NotaFiscalFisica.PercentualAdicionalHoraExtra)}");
            sqlComando.Append($", nf.pct_desvio_pesagem {nameof(NotaFiscalFisica.PercentualDesvioPesagem)}");
            sqlComando.Append($", nf.peso_rodoviario {nameof(NotaFiscalFisica.PesoRodoviario)}");
            sqlComando.Append($", nf.pos_venda {nameof(NotaFiscalFisica.PosVenda)}");
            sqlComando.Append($", nf.qtde_cp {nameof(NotaFiscalFisica.QuantidadeCorpoProva)}");
            sqlComando.Append($", nf.qtde_manual_pes {nameof(NotaFiscalFisica.QuantidadeManualPesagem)}");
            sqlComando.Append($", nf.qtde_pausa_pes {nameof(NotaFiscalFisica.QuantidadePausaPesagem)}");
            sqlComando.Append($", nf.represent {nameof(NotaFiscalFisica.Represente)}");
            sqlComando.Append($", nf.respon_cliente {nameof(NotaFiscalFisica.ResponsavelCliente)}");
            sqlComando.Append($", nf.slump {nameof(NotaFiscalFisica.Slump)}");
            sqlComando.Append($", nf.slump_real {nameof(NotaFiscalFisica.SlumpReal)}");
            sqlComando.Append($", nf.status_km_limit {nameof(NotaFiscalFisica.StatusKmLimite)}");
            sqlComando.Append($", nf.tempo_entre_via {nameof(NotaFiscalFisica.TempoEntreVia)}");
            sqlComando.Append($", nf.tempo_ida {nameof(NotaFiscalFisica.TempoIda)}");
            sqlComando.Append($", nf.tempo_na_obra {nameof(NotaFiscalFisica.TempoNaObra)}");
            sqlComando.Append($", nf.tempo_total {nameof(NotaFiscalFisica.TempoTotal)}");
            sqlComando.Append($", nf.tempo_vg_saida {nameof(NotaFiscalFisica.TempoVgSaida)}");
            sqlComando.Append($", nf.tempo_volta {nameof(NotaFiscalFisica.TempoVolta)}");
            sqlComando.Append($", nf.terc_bomba {nameof(NotaFiscalFisica.TerceiroBomba)}");
            sqlComando.Append($", nf.tp_cobranca {nameof(NotaFiscalFisica.TipoCobranca)}");
            sqlComando.Append($", nf.traco_concreto {nameof(NotaFiscalFisica.TracoConcreto)}");
            sqlComando.Append($", nf.traco_concreto_pesado {nameof(NotaFiscalFisica.TracoConcretoPesado)}");
            sqlComando.Append($", nf.usado_na_nf {nameof(NotaFiscalFisica.UsadoNaNotaFiscalRemessaNumero)}");
            sqlComando.Append($", nf.usina_fat {nameof(NotaFiscalFisica.UsinaFaturamento)}");
            sqlComando.Append($", nf.usina {nameof(NotaFiscalFisica.UsinaPesagem)}");
            sqlComando.Append($", nf.vl_bomba_Calc {nameof(NotaFiscalFisica.ValorBombaCalculo)}");
            sqlComando.Append($", nf.vlr_bomba_unit {nameof(NotaFiscalFisica.ValorBombaUnitario)}");
            sqlComando.Append($", nf.vlr_com_aux1 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar1)}");
            sqlComando.Append($", nf.vlr_com_aux2 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar2)}");
            sqlComando.Append($", nf.vlr_com_aux3 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar3)}");
            sqlComando.Append($", nf.vlr_com_bombist {nameof(NotaFiscalFisica.ValorComissaoBombista)}");
            sqlComando.Append($", nf.vlr_com_motoris {nameof(NotaFiscalFisica.ValorComissaoMotorista)}");
            sqlComando.Append($", nf.vl_desco {nameof(NotaFiscalFisica.ValorDesconto)}");
            sqlComando.Append($", nf.valor_tx_comis {nameof(NotaFiscalFisica.ValorTaxaComissao)}");
            sqlComando.Append($", nf.vlr_vend_tb_tot {nameof(NotaFiscalFisica.ValorVendaTabelaTotal)}");
            sqlComando.Append($", nf.velocimento {nameof(NotaFiscalFisica.Velocimento)}");
            sqlComando.Append($", nf.velocimento_final {nameof(NotaFiscalFisica.VelocimentoFinal)}");
            sqlComando.Append($", nf.vendedor {nameof(NotaFiscalFisica.Vendedor)}");
            sqlComando.Append($", nf.vibr_qtde {nameof(NotaFiscalFisica.VibradorQuantidade)}");
            sqlComando.Append($", nf.vibr_vendedor {nameof(NotaFiscalFisica.VibradorVendedor)}");
            sqlComando.Append($", nf.vibr_vlr_unit {nameof(NotaFiscalFisica.VibradorValorUnitario)}");
            sqlComando.Append($", nf.vol_entreg_bomb {nameof(NotaFiscalFisica.VolumeEntregaBombeado)}");
            sqlComando.Append($", nf.espera_usina {nameof(NotaFiscalFisica.EsperaNaUsina)}");
            sqlComando.Append($", nf.atualizado_em {nameof(NotaFiscalFisica.DataAtualizacao)}");
            sqlComando.Append($" FROM con_nf nf");
            sqlComando.Append($" WHERE atualizado_em>='{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");
            sqlComando.Append($" AND usina_contrato<>0");
            sqlComando.Append($" AND qtde_m3_bt<>0");

            if (dataFim != null)
                sqlComando.Append($" AND atualizado_em<='{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" ORDER BY atualizado_em");

            var notasFiscaisFisicas = _context.Connection.QueryPagedList<NotaFiscalFisica>(sqlComando.ToString(), page, limit);

            var notasFiscaisFisicasLista = new List<NotaFiscalFisica>();

            var notasFiscaisFisicasResultPagedList = new PagedList<NotaFiscalFisica>
            {
                CurrentPage = notasFiscaisFisicas.CurrentPage,
                PageCount = notasFiscaisFisicas.PageCount,
                PageSize = notasFiscaisFisicas.PageSize,
                RecordCount = notasFiscaisFisicas.RecordCount
            };

            foreach (var record in notasFiscaisFisicas.Records)
            {
                var nota = (NotaFiscalFisica)record;

                sqlComando.Clear();
                sqlComando.Append($"SELECT cfop {nameof(NotaFiscalFisicaItem.Cfop)}");
                sqlComando.Append($", custo_tot_item {nameof(NotaFiscalFisicaItem.CustoTotal)}");
                sqlComando.Append($", dt_hr_ult_atual {nameof(NotaFiscalFisicaItem.DataHoraUltimaAtualizacao)}");
                sqlComando.Append($", dt_op {nameof(NotaFiscalFisicaItem.DataOperacao)}");
                sqlComando.Append($", filial {nameof(NotaFiscalFisicaItem.FilialCodigo)}");
                sqlComando.Append($", id_atual {nameof(NotaFiscalFisicaItem.IdAtual)}");
                sqlComando.Append($", id_cadast {nameof(NotaFiscalFisicaItem.IdCadastro)}");
                sqlComando.Append($", interv_estq {nameof(NotaFiscalFisicaItem.IntervenienteEstoque)}");
                sqlComando.Append($", interv {nameof(NotaFiscalFisicaItem.IntervenienteCodigo)}");
                sqlComando.Append($", local_estoque {nameof(NotaFiscalFisicaItem.LocalEstoque)}");
                sqlComando.Append($", local_insumo {nameof(NotaFiscalFisicaItem.LocalInsumo)}");
                sqlComando.Append($", merc {nameof(NotaFiscalFisicaItem.MercadoriaCodigo)}");
                sqlComando.Append($", num_nf {nameof(NotaFiscalFisicaItem.Numero)}");
                sqlComando.Append($", num_seq_item_nf {nameof(NotaFiscalFisicaItem.SequenciaItem)}");
                sqlComando.Append($", percentual_ajuste {nameof(NotaFiscalFisicaItem.PercentualAjuste)}");
                sqlComando.Append($", peso {nameof(NotaFiscalFisicaItem.Peso)}");
                sqlComando.Append($", preco_un {nameof(NotaFiscalFisicaItem.PrecoUnitario)}");
                sqlComando.Append($", qt {nameof(NotaFiscalFisicaItem.Quantidade)}");
                sqlComando.Append($", qtde_comis {nameof(NotaFiscalFisicaItem.QuantidadeComissao)}");
                sqlComando.Append($", qtd_estoque {nameof(NotaFiscalFisicaItem.QuantidadeEstoque)}");
                sqlComando.Append($", qt_teorica {nameof(NotaFiscalFisicaItem.QuantidadeTeorica)}");
                sqlComando.Append($", seq_cfop {nameof(NotaFiscalFisicaItem.SequenciaCfop)}");
                sqlComando.Append($", seq_nf {nameof(NotaFiscalFisicaItem.Sequencia)}");
                sqlComando.Append($", ser {nameof(NotaFiscalFisicaItem.Serie)}");
                sqlComando.Append($", tp_doc {nameof(NotaFiscalFisicaItem.TipoDocumentoCodigo)}");
                sqlComando.Append($", tp_estq {nameof(NotaFiscalFisicaItem.TipoEstoque)}");
                sqlComando.Append($", traco_concreto {nameof(NotaFiscalFisicaItem.TracoConcreto)}");
                sqlComando.Append($", trans {nameof(NotaFiscalFisicaItem.Transacao)}");
                sqlComando.Append($", umidade {nameof(NotaFiscalFisicaItem.Umidade)}");
                sqlComando.Append($", vl_desc {nameof(NotaFiscalFisicaItem.ValorDesconto)}");
                sqlComando.Append($", vl_frete {nameof(NotaFiscalFisicaItem.ValorFrete)}");
                sqlComando.Append($", vl_o_desp {nameof(NotaFiscalFisicaItem.ValorOutrasDespesas)}");
                sqlComando.Append($", vl_seg {nameof(NotaFiscalFisicaItem.ValorSeguro)}");
                sqlComando.Append($", vl_tot {nameof(NotaFiscalFisicaItem.ValorTotal)}");
                sqlComando.Append($", volume {nameof(NotaFiscalFisicaItem.Volume)}");
                sqlComando.Append($" FROM con_item_nf");
                sqlComando.Append($" WHERE filial=@filial");
                sqlComando.Append($" AND interv=@interveniente");
                sqlComando.Append($" AND tp_doc=@tipoDocumento");
                sqlComando.Append($" AND ser=@serie");
                sqlComando.Append($" AND num_nf=@numeroNf");
                sqlComando.Append($" AND seq_nf=@sequenciaNf");

                nota.Itens = _context.Database.Connection.Query<NotaFiscalFisicaItem>(sqlComando.ToString(), new
                {
                    filial = nota.FilialCodigo,
                    interveniente = nota.IntervenienteCodigo,
                    tipoDocumento = nota.TipoDocumentoCodigo,
                    serie = nota.Serie,
                    numeroNf = nota.Numero,
                    sequenciaNf = nota.Sequencia
                }).ToList();
                
                foreach (var notaItem in nota.Itens)
                {
                    notaItem.Mercadoria = ObterMercadoria(notaItem.MercadoriaCodigo);
                }

                sqlComando.Clear();
                sqlComando.Append($"SELECT filial {nameof(NotaFiscalFisicaDemaisServicos.FilialCodigo)}");
                sqlComando.Append($", interv {nameof(NotaFiscalFisicaDemaisServicos.IntervenienteCodigo)}");
                sqlComando.Append($", tp_doc {nameof(NotaFiscalFisicaDemaisServicos.TipoDocumentoCodigo)}");
                sqlComando.Append($", num_nf {nameof(NotaFiscalFisicaDemaisServicos.Numero)}");
                sqlComando.Append($", serie {nameof(NotaFiscalFisicaDemaisServicos.Serie)}");
                sqlComando.Append($", seq_nf {nameof(NotaFiscalFisicaDemaisServicos.Sequencia)}");
                sqlComando.Append($", seq_serv_obra {nameof(NotaFiscalFisicaDemaisServicos.SequenciaServico)}"); 
                sqlComando.Append($", merc {nameof(NotaFiscalFisicaDemaisServicos.MercadoriaCodigo)}");
                sqlComando.Append($", quantidade {nameof(NotaFiscalFisicaDemaisServicos.Quantidade)}");
                sqlComando.Append($", valor_unitario {nameof(NotaFiscalFisicaDemaisServicos.ValorUnitario)}");
                sqlComando.Append($", valor_total {nameof(NotaFiscalFisicaDemaisServicos.ValorTotal)}");
                sqlComando.Append($", valor_cobrado {nameof(NotaFiscalFisicaDemaisServicos.ValorCobrado)}");
                sqlComando.Append($" FROM con_nf_servicos");
                sqlComando.Append($" WHERE filial=@filial");
                sqlComando.Append($" AND interv=@interveniente");
                sqlComando.Append($" AND tp_doc=@tipoDocumento");
                sqlComando.Append($" AND serie=@serie");
                sqlComando.Append($" AND num_nf=@numeroNf");
                sqlComando.Append($" AND seq_nf=@sequenciaNf");

                nota.DemaisServicos = _context.Database.Connection.Query<NotaFiscalFisicaDemaisServicos>(sqlComando.ToString(), new
                {
                    filial = nota.FilialCodigo,
                    interveniente = nota.IntervenienteCodigo,
                    tipoDocumento = nota.TipoDocumentoCodigo,
                    serie = nota.Serie,
                    numeroNf = nota.Numero,
                    sequenciaNf = nota.Sequencia
                }).ToList();

                foreach (var notaServico in nota.DemaisServicos)
                {
                    notaServico.Mercadoria = ObterMercadoria(notaServico.MercadoriaCodigo);
                }

                nota.Complemento = _context.NotasFiscaisFisicasComplemento
                    .Where(t => t.FilialCodigo == nota.FilialCodigo
                        && t.IntervenienteCodigo == nota.IntervenienteCodigo
                        && t.TipoDocumentoCodigo == nota.TipoDocumentoCodigo
                        && t.Serie == nota.Serie
                        && t.Numero == nota.Numero
                        && t.Sequencia == nota.Sequencia).FirstOrDefault();

                nota.Reaproveitamentos = _context.Reaproveitamentos
                    .Where(t => t.FilialNotaDestino == nota.FilialCodigo
                        && t.IntervenienteNotaDestino == nota.IntervenienteCodigo
                        && t.TipoDocumentoNotaDestino == nota.TipoDocumentoCodigo
                        && t.SerieNotaDestino == nota.Serie
                        && t.NumeroNotaDestino == nota.Numero
                        && t.SequenciaNotaDestino == nota.Sequencia).ToList();

                notasFiscaisFisicasLista.Add(nota);
            }

            notasFiscaisFisicasResultPagedList.Records = notasFiscaisFisicasLista;

            return notasFiscaisFisicasResultPagedList;
        }

        public NotaFiscalFisica ObterPorChave(int? filial, int interveniente, int? tipoDocumento, string serie, long? numero, int? sequencia)
        {
            return _context.NotasFiscaisFisicas
                    .Where(t => t.FilialCodigo == filial
                        && t.IntervenienteCodigo == interveniente
                        && t.TipoDocumentoCodigo == tipoDocumento
                        && t.Serie == serie
                        && t.Numero == numero
                        && t.Sequencia == sequencia).FirstOrDefault();
        }

        public NotaFiscalFisicaComplemento ObterComplemento(int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia)
        {
            return _context.NotasFiscaisFisicasComplemento
                    .Where(t => t.FilialCodigo == filial
                        && t.IntervenienteCodigo == interveniente
                        && t.TipoDocumentoCodigo == tipoDocumento
                        && t.Serie == serie
                        && t.Numero == numero
                        && t.Sequencia == sequencia).FirstOrDefault();
        }

        public Mercadoria ObterMercadoria(string mercadoriaCodigo)
        {
            return _context.Mercadoria.FirstOrDefault(t => t.Codigo == mercadoriaCodigo);
        }

        public PagedList<NotaFiscalFisica> ObterPorDataRetornoAutomacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT nf.filial {nameof(NotaFiscalFisica.FilialCodigo)}");
            sqlComando.Append($", nf.interv {nameof(NotaFiscalFisica.IntervenienteCodigo)}");
            sqlComando.Append($", nf.tp_doc {nameof(NotaFiscalFisica.TipoDocumentoCodigo)}");
            sqlComando.Append($", nf.num_nf {nameof(NotaFiscalFisica.Numero)}");
            sqlComando.Append($", nf.serie {nameof(NotaFiscalFisica.Serie)}");
            sqlComando.Append($", nf.seq_nf {nameof(NotaFiscalFisica.Sequencia)}");
            sqlComando.Append($", nf.usina_contrato {nameof(NotaFiscalFisica.ContratoUsinaCodigo)}");
            sqlComando.Append($", nf.num_contrato {nameof(NotaFiscalFisica.ContratoNumero)}");
            sqlComando.Append($", nf.ano_contrato {nameof(NotaFiscalFisica.ContratoAno)}");
            sqlComando.Append($", nf.seq_prog {nameof(NotaFiscalFisica.ProgramacaoSequencia)}");
            sqlComando.Append($", nf.motivo_cancel {nameof(NotaFiscalFisica.MotivoCancelamentoCodigo)}");
            sqlComando.Append($", nf.qtde_m3_bt {nameof(NotaFiscalFisica.Volume)}");
            sqlComando.Append($", nf.hr_saida_usina {nameof(NotaFiscalFisica.HoraSaidaUsina)}");
            sqlComando.Append($", nf.no_betoneira {nameof(NotaFiscalFisica.BetoneiraCodigo)}");
            sqlComando.Append($", nf.vlr_venda_m3 {nameof(NotaFiscalFisica.TracoValorUnitario)}");
            sqlComando.Append($", nf.vlr_venda_total {nameof(NotaFiscalFisica.TracoValorTotal)}");
            sqlComando.Append($", nf.vlr_bomba_total {nameof(NotaFiscalFisica.BombaValorTotal)}");
            sqlComando.Append($", nf.vlr_m3_fal {nameof(NotaFiscalFisica.M3FaltanteValor)}");
            sqlComando.Append($", nf.vibr_vlr_total {nameof(NotaFiscalFisica.VibradorValorTotal)}");
            sqlComando.Append($", nf.adic_he {nameof(NotaFiscalFisica.AdicionalHoraExtraValorTotal)}");
            sqlComando.Append($", nf.adic_feriado {nameof(NotaFiscalFisica.AdicionalFeriadoValorTotal)}");
            sqlComando.Append($", nf.adicao_agua {nameof(NotaFiscalFisica.AdicaoAgua)}");
            sqlComando.Append($", nf.ag_colocada_usi {nameof(NotaFiscalFisica.AguaColocadaNaUsina)}");
            sqlComando.Append($", nf.ag_colocar_obr {nameof(NotaFiscalFisica.AguaColocarNaObra)}");
            sqlComando.Append($", nf.atraso_entrega {nameof(NotaFiscalFisica.AtrasoEntrega)}");
            sqlComando.Append($", nf.aux_bombista {nameof(NotaFiscalFisica.AuxiliarBombista)}");
            sqlComando.Append($", nf.aux_bombista2 {nameof(NotaFiscalFisica.AuxiliarBombista2)}");
            sqlComando.Append($", nf.aux_bombista3 {nameof(NotaFiscalFisica.AuxiliarBombista3)}");
            sqlComando.Append($", nf.balanca {nameof(NotaFiscalFisica.Balanca)}");
            sqlComando.Append($", nf.bomba {nameof(NotaFiscalFisica.Bomba)}");
            sqlComando.Append($", nf.bombista {nameof(NotaFiscalFisica.Bombista)}");
            sqlComando.Append($", nf.chave_familia {nameof(NotaFiscalFisica.ChaveFamilia)}");
            sqlComando.Append($", nf.cimento {nameof(NotaFiscalFisica.Cimento)}");
            sqlComando.Append($", nf.cod_inconsist {nameof(NotaFiscalFisica.CodigoInconsistencia)}");
            sqlComando.Append($", nf.cod_integracao {nameof(NotaFiscalFisica.CodigoIntegracao)}");
            sqlComando.Append($", nf.cod_transp_mp {nameof(NotaFiscalFisica.CodigoTransportadorMateriaPrima)}");
            sqlComando.Append($", nf.codigos_cps {nameof(NotaFiscalFisica.CodigosCorpoProvas)}");
            sqlComando.Append($", nf.com_repres_dif {nameof(NotaFiscalFisica.ComissaoRepresentanteDiferenca)}");
            sqlComando.Append($", nf.com_repres_serv {nameof(NotaFiscalFisica.ComissaoRepresentanteServico)}");
            sqlComando.Append($", nf.comis_ger {nameof(NotaFiscalFisica.ComissaoGerada)}");
            sqlComando.Append($", nf.comis_aj_bomba {nameof(NotaFiscalFisica.ComissaoAjudanteBomba)}");
            sqlComando.Append($", nf.comis_bombista {nameof(NotaFiscalFisica.ComissaoBombista)}");
            sqlComando.Append($", nf.comis_motorista {nameof(NotaFiscalFisica.ComissaoMotorista)}");
            sqlComando.Append($", nf.com_repre_bomb {nameof(NotaFiscalFisica.ComissaoRepresentanteBomba)}");
            sqlComando.Append($", nf.com_repr_concr {nameof(NotaFiscalFisica.ComissaoRepresentanteConcreto)}");
            sqlComando.Append($", nf.comis_represent {nameof(NotaFiscalFisica.ComissaoRepresentante)}");
            sqlComando.Append($", nf.com_transp_mp {nameof(NotaFiscalFisica.ComissaoTransportadorMateriaPrima)}");
            sqlComando.Append($", nf.com_vend_dif {nameof(NotaFiscalFisica.ComissaoVendedorDiferenca)}");
            sqlComando.Append($", nf.com_vend_bomb {nameof(NotaFiscalFisica.ComissaoVendaBombista)}");
            sqlComando.Append($", nf.com_vend_padr {nameof(NotaFiscalFisica.ComissaoVendaPadrao)}");
            sqlComando.Append($", nf.com_vend_tx_extra {nameof(NotaFiscalFisica.ComissaoVendaTaxaExtra)}");
            sqlComando.Append($", nf.com_vend_vibr {nameof(NotaFiscalFisica.ComissaoVendaVibrador)}");
            sqlComando.Append($", nf.comis_vendedor {nameof(NotaFiscalFisica.ComissaoVendedor)}");
            sqlComando.Append($", nf.com_vend_concr {nameof(NotaFiscalFisica.ComissaoVendedorConcreto)}");
            sqlComando.Append($", nf.com_vend_serv {nameof(NotaFiscalFisica.ComissaoVendedorServico)}");
            sqlComando.Append($", nf.cp {nameof(NotaFiscalFisica.CorpoProva)}");
            sqlComando.Append($", nf.corte_agua_pm3 {nameof(NotaFiscalFisica.CorteAguaPorM3)}");
            sqlComando.Append($", nf.custo_conc_pes {nameof(NotaFiscalFisica.CustoConcretoPesado)}");
            sqlComando.Append($", nf.dt_base_comis {nameof(NotaFiscalFisica.DataBaseComissao)}");
            sqlComando.Append($", nf.dt_coleta_cp {nameof(NotaFiscalFisica.DataColetaCorpoProva)}");
            sqlComando.Append($", nf.dt_fatura {nameof(NotaFiscalFisica.DataFatura)}");
            sqlComando.Append($", nf.dt_prog {nameof(NotaFiscalFisica.DataProgramacao)}");
            sqlComando.Append($", nf.dt_prorrog_pend {nameof(NotaFiscalFisica.DataProrrogacaoPendencia)}");
            sqlComando.Append($", nf.data_remessa {nameof(NotaFiscalFisica.DataRemessa)}");
            sqlComando.Append($", nf.dt_vcto_1_parc {nameof(NotaFiscalFisica.DataVencimentoPrimeiraParcela)}");
            sqlComando.Append($", nf.descartado_cp {nameof(NotaFiscalFisica.DescartadoCorpoProva)}");
            sqlComando.Append($", nf.desvio {nameof(NotaFiscalFisica.Desvio)}");
            sqlComando.Append($", nf.especificacao_traco {nameof(NotaFiscalFisica.EspecificacaoTraco)}");
            sqlComando.Append($", nf.espera_inicio {nameof(NotaFiscalFisica.EsperaInicio)}");
            sqlComando.Append($", nf.espera_saida_ob {nameof(NotaFiscalFisica.EsperaSaidaObra)}");
            sqlComando.Append($", nf.filial_estoque {nameof(NotaFiscalFisica.FilialEstoque)}");
            sqlComando.Append($", nf.filial_fat {nameof(NotaFiscalFisica.FilialFaturamento)}");
            sqlComando.Append($", nf.hr_cheg_usina {nameof(NotaFiscalFisica.HoraChegadaUsina)}");
            sqlComando.Append($", nf.hora_fim_carga {nameof(NotaFiscalFisica.HoraFimCarga)}");
            sqlComando.Append($", nf.hora_ini_carga {nameof(NotaFiscalFisica.HoraInicioCarga)}");
            sqlComando.Append($", nf.hr_prevista {nameof(NotaFiscalFisica.HoraPrevista)}");
            sqlComando.Append($", nf.hr_recomend_utl {nameof(NotaFiscalFisica.HoraRecomendadaUtilizacao)}");
            sqlComando.Append($", nf.hr_saida_func {nameof(NotaFiscalFisica.HoraSaidaFuncionario)}");
            sqlComando.Append($", nf.hr_saida_obra {nameof(NotaFiscalFisica.HoraSaidaObra)}");
            sqlComando.Append($", nf.hr_saida_usina_efet {nameof(NotaFiscalFisica.HoraSaidaUsinaEfetiva)}");
            sqlComando.Append($", nf.horimetro_final {nameof(NotaFiscalFisica.HorimetroFinal)}");
            sqlComando.Append($", nf.horimetro_inicial {nameof(NotaFiscalFisica.HorimetroInicial)}");
            sqlComando.Append($", nf.horimetro_rodado {nameof(NotaFiscalFisica.HorimetroRodado)}");
            sqlComando.Append($", nf.hr_cheg_obra {nameof(NotaFiscalFisica.HoraChegadaObra)}");
            sqlComando.Append($", nf.hr_desc_final {nameof(NotaFiscalFisica.HoraDescargaFinal)}");
            sqlComando.Append($", nf.hr_desc_inic {nameof(NotaFiscalFisica.HoraDescargaInicial)}");
            sqlComando.Append($", nf.hr_solicitada {nameof(NotaFiscalFisica.HoraSolicitada)}");
            sqlComando.Append($", nf.id_aprov_dir {nameof(NotaFiscalFisica.IdAprovacaoDiretoria)}");
            sqlComando.Append($", nf.id_aprov_lab {nameof(NotaFiscalFisica.IdAprovacaoLaboratorio)}");
            sqlComando.Append($", nf.id_atual {nameof(NotaFiscalFisica.IdAtual)}");
            sqlComando.Append($", nf.id_cadast {nameof(NotaFiscalFisica.IdCadastro)}");
            sqlComando.Append($", nf.id_coleta_cp {nameof(NotaFiscalFisica.IdColetaCorpoProva)}");
            sqlComando.Append($", nf.import_gps {nameof(NotaFiscalFisica.ImportaGps)}");
            sqlComando.Append($", nf.import_rfid {nameof(NotaFiscalFisica.ImportaRfid)}");
            sqlComando.Append($", nf.importou_da_nf {nameof(NotaFiscalFisica.ImportouDaNotaFiscalRemessa)}");
            sqlComando.Append($", nf.inconsistencias {nameof(NotaFiscalFisica.Inconsistencias)}");
            sqlComando.Append($", nf.km_rodado {nameof(NotaFiscalFisica.KmRodado)}");
            sqlComando.Append($", nf.m3_bombeado {nameof(NotaFiscalFisica.M3Bombeado)}");
            sqlComando.Append($", nf.m3_bomb_cob {nameof(NotaFiscalFisica.M3BombeadoCobrado)}");
            sqlComando.Append($", nf.m3_faltantes {nameof(NotaFiscalFisica.M3Faltantes)}");
            sqlComando.Append($", nf.mao_obra_prop {nameof(NotaFiscalFisica.MaoDeObraPropria)}");
            sqlComando.Append($", nf.material_m3 {nameof(NotaFiscalFisica.MaterialM3)}");
            sqlComando.Append($", nf.material_total {nameof(NotaFiscalFisica.MaterialTotal)}");
            sqlComando.Append($", nf.minutos_desc {nameof(NotaFiscalFisica.MinutosDescarga)}");
            sqlComando.Append($", nf.motivo_atraso {nameof(NotaFiscalFisica.MotivoAtraso)}");
            sqlComando.Append($", nf.motivo_atraso_concr {nameof(NotaFiscalFisica.MotivoAtrasoConcretagem)}");
            sqlComando.Append($", nf.motivo_incons {nameof(NotaFiscalFisica.MotivoInconsistencia)}");
            sqlComando.Append($", nf.motorista {nameof(NotaFiscalFisica.Motorista)}");
            sqlComando.Append($", nf.num_fatura {nameof(NotaFiscalFisica.NumeroFatura)}");
            sqlComando.Append($", nf.no_faturamento {nameof(NotaFiscalFisica.NumeroFaturamento)}");
            sqlComando.Append($", nf.num_lacre {nameof(NotaFiscalFisica.NumeroLacre)}");
            sqlComando.Append($", nf.obs_entrega {nameof(NotaFiscalFisica.ObservacaoEntrega)}");
            sqlComando.Append($", nf.obs_aprov_km {nameof(NotaFiscalFisica.ObservacaoAprovacaoKm)}");
            sqlComando.Append($", nf.obs_cancel {nameof(NotaFiscalFisica.ObservacaoCancelamento)}");
            sqlComando.Append($", nf.obs_pesagem {nameof(NotaFiscalFisica.ObservacaoPesagem)}");
            sqlComando.Append($", nf.obs_traco {nameof(NotaFiscalFisica.ObservacaoTraco)}");
            sqlComando.Append($", nf.obs {nameof(NotaFiscalFisica.Observacao)}");
            sqlComando.Append($", nf.pendente {nameof(NotaFiscalFisica.Pendente)}");
            sqlComando.Append($", nf.pct_adic_feriad {nameof(NotaFiscalFisica.PercentualAdicionalFeriado)}");
            sqlComando.Append($", nf.pct_adic_he {nameof(NotaFiscalFisica.PercentualAdicionalHoraExtra)}");
            sqlComando.Append($", nf.pct_desvio_pesagem {nameof(NotaFiscalFisica.PercentualDesvioPesagem)}");
            sqlComando.Append($", nf.peso_rodoviario {nameof(NotaFiscalFisica.PesoRodoviario)}");
            sqlComando.Append($", nf.pos_venda {nameof(NotaFiscalFisica.PosVenda)}");
            sqlComando.Append($", nf.qtde_cp {nameof(NotaFiscalFisica.QuantidadeCorpoProva)}");
            sqlComando.Append($", nf.qtde_manual_pes {nameof(NotaFiscalFisica.QuantidadeManualPesagem)}");
            sqlComando.Append($", nf.qtde_pausa_pes {nameof(NotaFiscalFisica.QuantidadePausaPesagem)}");
            sqlComando.Append($", nf.represent {nameof(NotaFiscalFisica.Represente)}");
            sqlComando.Append($", nf.respon_cliente {nameof(NotaFiscalFisica.ResponsavelCliente)}");
            sqlComando.Append($", nf.slump {nameof(NotaFiscalFisica.Slump)}");
            sqlComando.Append($", nf.slump_real {nameof(NotaFiscalFisica.SlumpReal)}");
            sqlComando.Append($", nf.status_km_limit {nameof(NotaFiscalFisica.StatusKmLimite)}");
            sqlComando.Append($", nf.tempo_entre_via {nameof(NotaFiscalFisica.TempoEntreVia)}");
            sqlComando.Append($", nf.tempo_ida {nameof(NotaFiscalFisica.TempoIda)}");
            sqlComando.Append($", nf.tempo_na_obra {nameof(NotaFiscalFisica.TempoNaObra)}");
            sqlComando.Append($", nf.tempo_total {nameof(NotaFiscalFisica.TempoTotal)}");
            sqlComando.Append($", nf.tempo_vg_saida {nameof(NotaFiscalFisica.TempoVgSaida)}");
            sqlComando.Append($", nf.tempo_volta {nameof(NotaFiscalFisica.TempoVolta)}");
            sqlComando.Append($", nf.terc_bomba {nameof(NotaFiscalFisica.TerceiroBomba)}");
            sqlComando.Append($", nf.tp_cobranca {nameof(NotaFiscalFisica.TipoCobranca)}");
            sqlComando.Append($", nf.traco_concreto {nameof(NotaFiscalFisica.TracoConcreto)}");
            sqlComando.Append($", nf.traco_concreto_pesado {nameof(NotaFiscalFisica.TracoConcretoPesado)}");
            sqlComando.Append($", nf.usado_na_nf {nameof(NotaFiscalFisica.UsadoNaNotaFiscalRemessaNumero)}");
            sqlComando.Append($", nf.usina_fat {nameof(NotaFiscalFisica.UsinaFaturamento)}");
            sqlComando.Append($", nf.usina {nameof(NotaFiscalFisica.UsinaPesagem)}");
            sqlComando.Append($", nf.vl_bomba_Calc {nameof(NotaFiscalFisica.ValorBombaCalculo)}");
            sqlComando.Append($", nf.vlr_bomba_unit {nameof(NotaFiscalFisica.ValorBombaUnitario)}");
            sqlComando.Append($", nf.vlr_com_aux1 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar1)}");
            sqlComando.Append($", nf.vlr_com_aux2 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar2)}");
            sqlComando.Append($", nf.vlr_com_aux3 {nameof(NotaFiscalFisica.ValorComissaoAuxiliar3)}");
            sqlComando.Append($", nf.vlr_com_bombist {nameof(NotaFiscalFisica.ValorComissaoBombista)}");
            sqlComando.Append($", nf.vlr_com_motoris {nameof(NotaFiscalFisica.ValorComissaoMotorista)}");
            sqlComando.Append($", nf.vl_desco {nameof(NotaFiscalFisica.ValorDesconto)}");
            sqlComando.Append($", nf.valor_tx_comis {nameof(NotaFiscalFisica.ValorTaxaComissao)}");
            sqlComando.Append($", nf.vlr_vend_tb_tot {nameof(NotaFiscalFisica.ValorVendaTabelaTotal)}");
            sqlComando.Append($", nf.velocimento {nameof(NotaFiscalFisica.Velocimento)}");
            sqlComando.Append($", nf.velocimento_final {nameof(NotaFiscalFisica.VelocimentoFinal)}");
            sqlComando.Append($", nf.vendedor {nameof(NotaFiscalFisica.Vendedor)}");
            sqlComando.Append($", nf.vibr_qtde {nameof(NotaFiscalFisica.VibradorQuantidade)}");
            sqlComando.Append($", nf.vibr_vendedor {nameof(NotaFiscalFisica.VibradorVendedor)}");
            sqlComando.Append($", nf.vibr_vlr_unit {nameof(NotaFiscalFisica.VibradorValorUnitario)}");
            sqlComando.Append($", nf.vol_entreg_bomb {nameof(NotaFiscalFisica.VolumeEntregaBombeado)}");
            sqlComando.Append($", nf.espera_usina {nameof(NotaFiscalFisica.EsperaNaUsina)}");
            sqlComando.Append($", nf.atualizado_em {nameof(NotaFiscalFisica.DataAtualizacao)}");
            sqlComando.Append($" FROM con_nf nf");
            sqlComando.Append($" INNER JOIN con_pesagem pes");
            sqlComando.Append($" ON pes.cod_integracao=nf.cod_integracao");
            sqlComando.Append($" WHERE pes.hora_fim>='{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");
            sqlComando.Append($" AND usina_contrato<>0");

            if (dataFim != null)
                sqlComando.Append($" AND pes.hora_fim<='{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" AND nf.cod_integracao<>''");

            sqlComando.Append($" ORDER BY pes.hora_fim");

            var notasFiscaisFisicas = _context.Connection.QueryPagedList<NotaFiscalFisica>(sqlComando.ToString(), page, limit);

            var notasFiscaisFisicasLista = new List<NotaFiscalFisica>();

            var notasFiscaisFisicasResultPagedList = new PagedList<NotaFiscalFisica>
            {
                CurrentPage = notasFiscaisFisicas.CurrentPage,
                PageCount = notasFiscaisFisicas.PageCount,
                PageSize = notasFiscaisFisicas.PageSize,
                RecordCount = notasFiscaisFisicas.RecordCount
            };

            foreach (var record in notasFiscaisFisicas.Records)
            {
                var nota = (NotaFiscalFisica)record;

                sqlComando.Clear();
                sqlComando.Append($"SELECT cfop {nameof(NotaFiscalFisicaItem.Cfop)}");
                sqlComando.Append($", custo_tot_item {nameof(NotaFiscalFisicaItem.CustoTotal)}");
                sqlComando.Append($", dt_hr_ult_atual {nameof(NotaFiscalFisicaItem.DataHoraUltimaAtualizacao)}");
                sqlComando.Append($", dt_op {nameof(NotaFiscalFisicaItem.DataOperacao)}");
                sqlComando.Append($", filial {nameof(NotaFiscalFisicaItem.FilialCodigo)}");
                sqlComando.Append($", id_atual {nameof(NotaFiscalFisicaItem.IdAtual)}");
                sqlComando.Append($", id_cadast {nameof(NotaFiscalFisicaItem.IdCadastro)}");
                sqlComando.Append($", interv_estq {nameof(NotaFiscalFisicaItem.IntervenienteEstoque)}");
                sqlComando.Append($", interv {nameof(NotaFiscalFisicaItem.IntervenienteCodigo)}");
                sqlComando.Append($", local_estoque {nameof(NotaFiscalFisicaItem.LocalEstoque)}");
                sqlComando.Append($", local_insumo {nameof(NotaFiscalFisicaItem.LocalInsumo)}");
                sqlComando.Append($", merc {nameof(NotaFiscalFisicaItem.MercadoriaCodigo)}");
                sqlComando.Append($", num_nf {nameof(NotaFiscalFisicaItem.Numero)}");
                sqlComando.Append($", num_seq_item_nf {nameof(NotaFiscalFisicaItem.SequenciaItem)}");
                sqlComando.Append($", percentual_ajuste {nameof(NotaFiscalFisicaItem.PercentualAjuste)}");
                sqlComando.Append($", peso {nameof(NotaFiscalFisicaItem.Peso)}");
                sqlComando.Append($", preco_un {nameof(NotaFiscalFisicaItem.PrecoUnitario)}");
                sqlComando.Append($", qt {nameof(NotaFiscalFisicaItem.Quantidade)}");
                sqlComando.Append($", qtde_comis {nameof(NotaFiscalFisicaItem.QuantidadeComissao)}");
                sqlComando.Append($", qtd_estoque {nameof(NotaFiscalFisicaItem.QuantidadeEstoque)}");
                sqlComando.Append($", qt_teorica {nameof(NotaFiscalFisicaItem.QuantidadeTeorica)}");
                sqlComando.Append($", seq_cfop {nameof(NotaFiscalFisicaItem.SequenciaCfop)}");
                sqlComando.Append($", seq_nf {nameof(NotaFiscalFisicaItem.Sequencia)}");
                sqlComando.Append($", ser {nameof(NotaFiscalFisicaItem.Serie)}");
                sqlComando.Append($", tp_doc {nameof(NotaFiscalFisicaItem.TipoDocumentoCodigo)}");
                sqlComando.Append($", tp_estq {nameof(NotaFiscalFisicaItem.TipoEstoque)}");
                sqlComando.Append($", traco_concreto {nameof(NotaFiscalFisicaItem.TracoConcreto)}");
                sqlComando.Append($", trans {nameof(NotaFiscalFisicaItem.Transacao)}");
                sqlComando.Append($", umidade {nameof(NotaFiscalFisicaItem.Umidade)}");
                sqlComando.Append($", vl_desc {nameof(NotaFiscalFisicaItem.ValorDesconto)}");
                sqlComando.Append($", vl_frete {nameof(NotaFiscalFisicaItem.ValorFrete)}");
                sqlComando.Append($", vl_o_desp {nameof(NotaFiscalFisicaItem.ValorOutrasDespesas)}");
                sqlComando.Append($", vl_seg {nameof(NotaFiscalFisicaItem.ValorSeguro)}");
                sqlComando.Append($", vl_tot {nameof(NotaFiscalFisicaItem.ValorTotal)}");
                sqlComando.Append($", volume {nameof(NotaFiscalFisicaItem.Volume)}");
                sqlComando.Append($" FROM con_item_nf");
                sqlComando.Append($" WHERE filial=@filial");
                sqlComando.Append($" AND interv=@interveniente");
                sqlComando.Append($" AND tp_doc=@tipoDocumento");
                sqlComando.Append($" AND ser=@serie");
                sqlComando.Append($" AND num_nf=@numeroNf");
                sqlComando.Append($" AND seq_nf=@sequenciaNf");

                nota.Itens = _context.Database.Connection.Query<NotaFiscalFisicaItem>(sqlComando.ToString(), new
                {
                    filial = nota.FilialCodigo,
                    interveniente = nota.IntervenienteCodigo,
                    tipoDocumento = nota.TipoDocumentoCodigo,
                    serie = nota.Serie,
                    numeroNf = nota.Numero,
                    sequenciaNf = nota.Sequencia
                }).ToList();
                
                foreach (var notaItem in nota.Itens)
                {
                    notaItem.Mercadoria = ObterMercadoria(notaItem.MercadoriaCodigo);
                }

                sqlComando.Clear();
                sqlComando.Append($"SELECT filial {nameof(NotaFiscalFisicaDemaisServicos.FilialCodigo)}");
                sqlComando.Append($", interv {nameof(NotaFiscalFisicaDemaisServicos.IntervenienteCodigo)}");
                sqlComando.Append($", tp_doc {nameof(NotaFiscalFisicaDemaisServicos.TipoDocumentoCodigo)}");
                sqlComando.Append($", num_nf {nameof(NotaFiscalFisicaDemaisServicos.Numero)}");
                sqlComando.Append($", serie {nameof(NotaFiscalFisicaDemaisServicos.Serie)}");
                sqlComando.Append($", seq_nf {nameof(NotaFiscalFisicaDemaisServicos.Sequencia)}");
                sqlComando.Append($", seq_serv_obra {nameof(NotaFiscalFisicaDemaisServicos.SequenciaServico)}");
                sqlComando.Append($", merc {nameof(NotaFiscalFisicaDemaisServicos.MercadoriaCodigo)}");
                sqlComando.Append($", quantidade {nameof(NotaFiscalFisicaDemaisServicos.Quantidade)}");
                sqlComando.Append($", valor_unitario {nameof(NotaFiscalFisicaDemaisServicos.ValorUnitario)}");
                sqlComando.Append($", valor_total {nameof(NotaFiscalFisicaDemaisServicos.ValorTotal)}");
                sqlComando.Append($", valor_cobrado {nameof(NotaFiscalFisicaDemaisServicos.ValorCobrado)}");
                sqlComando.Append($" FROM con_nf_servicos");
                sqlComando.Append($" WHERE filial=@filial");
                sqlComando.Append($" AND interv=@interveniente");
                sqlComando.Append($" AND tp_doc=@tipoDocumento");
                sqlComando.Append($" AND serie=@serie");
                sqlComando.Append($" AND num_nf=@numeroNf");
                sqlComando.Append($" AND seq_nf=@sequenciaNf");

                nota.DemaisServicos = _context.Database.Connection.Query<NotaFiscalFisicaDemaisServicos>(sqlComando.ToString(), new
                {
                    filial = nota.FilialCodigo,
                    interveniente = nota.IntervenienteCodigo,
                    tipoDocumento = nota.TipoDocumentoCodigo,
                    serie = nota.Serie,
                    numeroNf = nota.Numero,
                    sequenciaNf = nota.Sequencia
                }).ToList();

                foreach (var notaServico in nota.DemaisServicos)
                {
                    notaServico.Mercadoria = ObterMercadoria(notaServico.MercadoriaCodigo);
                }

                nota.Complemento = _context.NotasFiscaisFisicasComplemento
                    .Where(t => t.FilialCodigo == nota.FilialCodigo
                        && t.IntervenienteCodigo == nota.IntervenienteCodigo
                        && t.TipoDocumentoCodigo == nota.TipoDocumentoCodigo
                        && t.Serie == nota.Serie
                        && t.Numero == nota.Numero
                        && t.Sequencia == nota.Sequencia).FirstOrDefault();

                nota.Reaproveitamentos = _context.Reaproveitamentos
                    .Where(t => t.FilialNotaDestino == nota.FilialCodigo
                        && t.IntervenienteNotaDestino == nota.IntervenienteCodigo
                        && t.TipoDocumentoNotaDestino == nota.TipoDocumentoCodigo
                        && t.SerieNotaDestino == nota.Serie
                        && t.NumeroNotaDestino == nota.Numero
                        && t.SequenciaNotaDestino == nota.Sequencia).ToList();

                notasFiscaisFisicasLista.Add(nota);
            }

            notasFiscaisFisicasResultPagedList.Records = notasFiscaisFisicasLista;

            return notasFiscaisFisicasResultPagedList;
        }

        public PagedList<NotaFiscalFisicaIndicadorPontos> ObterIndicadorPontos(DateTime? dataInicio, DateTime? dataFim, int vendedor, string indicadorNome, int page, int limit)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine($"SELECT");
            sqlComando.AppendLine($"    i.CNPJ_CPF {nameof(NotaFiscalFisicaIndicadorPontos.IndicadorCpfCnpj)},");
            sqlComando.AppendLine($"    i.Razao {nameof(NotaFiscalFisicaIndicadorPontos.IndicadorNome)},");
            sqlComando.AppendLine($"    IF(i.tp_cliente = 'F', 1, 2) {nameof(NotaFiscalFisicaIndicadorPontos.IndicadorTipo)},");
            sqlComando.AppendLine($"    nf.qtde_m3_bt {nameof(NotaFiscalFisicaIndicadorPontos.IndicadorPontos)},");
            sqlComando.AppendLine($"    nf.serie {nameof(NotaFiscalFisicaIndicadorPontos.Serie)},");
            sqlComando.AppendLine($"    nf.num_nf {nameof(NotaFiscalFisicaIndicadorPontos.Numero)},");
            sqlComando.AppendLine($"    nf.usina {nameof(NotaFiscalFisicaIndicadorPontos.UsinaPesagem)},");
            sqlComando.AppendLine($"    nf.data_remessa {nameof(NotaFiscalFisicaIndicadorPontos.DataRemessa)},");
            sqlComando.AppendLine($"    nfi.CNPJ_CPF {nameof(NotaFiscalFisicaIndicadorPontos.IntervenienteCpfCnpj)},");
            sqlComando.AppendLine($"    nfi.Nome {nameof(NotaFiscalFisicaIndicadorPontos.IntervenienteNome)},");
            sqlComando.AppendLine($"    nf.vlr_venda_total {nameof(NotaFiscalFisicaIndicadorPontos.TracoValorTotal)}");
            sqlComando.AppendLine($"FROM con_obras_indicador oi");
            sqlComando.AppendLine($"INNER JOIN con_obras o ON");
            sqlComando.AppendLine($"    o.numero = oi.obra_numero");
            sqlComando.AppendLine($"    AND o.usina = oi.obra_usina");
            sqlComando.AppendLine($"INNER JOIN con_nf nf ON");
            sqlComando.AppendLine($"    nf.usina_contrato = o.usina");
            sqlComando.AppendLine($"    AND nf.num_contrato = o.no_contrato");
            sqlComando.AppendLine($"    AND nf.ano_contrato = o.ano_contrato");
            sqlComando.AppendLine($"INNER JOIN ger_interv nfi ON");
            sqlComando.AppendLine($"    nfi.Cod = nf.interv");
            sqlComando.AppendLine($"LEFT JOIN con_vendedor v ON");
            sqlComando.AppendLine($"    v.cod = oi.vendedor");
            sqlComando.AppendLine($"LEFT JOIN ger_interv i ON");
            sqlComando.AppendLine($"    i.Cod = IF(oi.interveniente = 0, v.interv, oi.interveniente)");
            sqlComando.AppendLine($"WHERE");
            sqlComando.AppendLine($"    (oi.interveniente > 0 OR oi.vendedor > 0)");

            if (dataInicio != null)
                sqlComando.AppendLine($"    AND nf.data_remessa >= @DataInicio");

            if (dataFim != null)
                sqlComando.AppendLine($"    AND nf.data_remessa <= @DataFim");

            if (vendedor > 0)
                sqlComando.AppendLine($"    AND nf.vendedor = @Vendedor");

            if (!string.IsNullOrEmpty(indicadorNome))
                sqlComando.AppendLine($"    AND i.razao LIKE @IndicadorNome");

            var parametros = new
            {
                DataInicio = dataInicio,
                DataFim = dataFim,
                Vendedor = vendedor,
                IndicadorNome = indicadorNome
            };

            var notas = _context.Connection.QueryPagedList<NotaFiscalFisicaIndicadorPontos>(sqlComando.ToString(), page, limit, parametros);

            var notaFiscalFisicaIndicadorPontosResultPagedList = new PagedList<NotaFiscalFisicaIndicadorPontos>
            {
                CurrentPage = notas.CurrentPage,
                PageCount = notas.PageCount,
                PageSize = notas.PageSize,
                RecordCount = notas.RecordCount,
                Records = (IEnumerable<NotaFiscalFisicaIndicadorPontos>) notas.Records
            };

            return notaFiscalFisicaIndicadorPontosResultPagedList;

        }

    }
}

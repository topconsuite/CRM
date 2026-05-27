using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalFisicaMap : EntityTypeConfiguration<NotaFiscalFisica>
    {
        public NotaFiscalFisicaMap()
        {
            ToTable("topsys.con_nf");

            HasKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia });

            Ignore(t => t.Complemento);

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

            Property(t => t.ContratoUsinaCodigo)
                .HasColumnName("usina_contrato");

            Property(t => t.ContratoAno)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnName("num_contrato");

            Property(t => t.ProgramacaoSequencia)
                .HasColumnName("seq_prog");

            Property(t => t.MotivoCancelamentoCodigo)
                .HasColumnName("motivo_cancel");

            Property(t => t.Volume)
                .HasColumnName("qtde_m3_bt");

            Property(t => t.HoraSaidaUsina)
                .HasColumnName("hr_saida_usina");

            Property(t => t.BetoneiraCodigo)
                .HasColumnName("no_betoneira");

            Property(t => t.TracoValorUnitario)
                .HasColumnName("vlr_venda_m3");

            Property(t => t.TracoValorTotal)
                .HasColumnName("vlr_venda_total");

            Property(t => t.BombaValorTotal)
                .HasColumnName("vlr_bomba_total");

            Property(t => t.M3FaltanteValor)
                .HasColumnName("vlr_m3_fal");

            Property(t => t.VibradorValorTotal)
                .HasColumnName("vibr_vlr_total");

            Property(t => t.AdicionalHoraExtraValorTotal)
                .HasColumnName("adic_he");

            Property(t => t.AdicionalFeriadoValorTotal)
                .HasColumnName("adic_feriado");

           Property(t => t.AdicaoAgua)
                .HasColumnName("adicao_agua");

           Property(t => t.AguaColocadaNaUsina)
                .HasColumnName("ag_colocada_usi");

           Property(t => t.AguaColocarNaObra)
                .HasColumnName("ag_colocar_obr");

           Property(t => t.AtrasoEntrega)
                .HasColumnName("atraso_entrega");

           Property(t => t.AuxiliarBombista)
                .HasColumnName("aux_bombista");

           Property(t => t.AuxiliarBombista2)
                .HasColumnName("aux_bombista2");

           Property(t => t.AuxiliarBombista3)
                .HasColumnName("aux_bombista3");

           Property(t => t.Balanca)
                .HasColumnName("balanca");

           Property(t => t.Bomba)
                .HasColumnName("bomba");

           Property(t => t.Bombista)
                .HasColumnName("bombista");

           Property(t => t.ChaveFamilia)
                .HasColumnName("chave_familia");

           Property(t => t.Cimento)
                .HasColumnName("cimento");

           Property(t => t.CodigoInconsistencia)
                .HasColumnName("cod_inconsist");

           Property(t => t.CodigoIntegracao)
                .HasColumnName("cod_integracao");

           Property(t => t.CodigoTransportadorMateriaPrima)
                .HasColumnName("cod_transp_mp");

           Property(t => t.CodigosCorpoProvas)
                .HasColumnName("codigos_cps");

           Property(t => t.ComissaoRepresentanteDiferenca)
                .HasColumnName("com_repres_dif");

           Property(t => t.ComissaoRepresentanteServico)
                .HasColumnName("com_repres_serv");

           Property(t => t.ComissaoGerada)
                .HasColumnName("comis_ger");

           Property(t => t.ComissaoAjudanteBomba)
                .HasColumnName("comis_aj_bomba");

           Property(t => t.ComissaoBombista)
                .HasColumnName("comis_bombista");

           Property(t => t.ComissaoMotorista)
                .HasColumnName("comis_motorista");

           Property(t => t.ComissaoRepresentanteBomba)
                .HasColumnName("com_repre_bomb");

           Property(t => t.ComissaoRepresentanteConcreto)
                .HasColumnName("com_repr_concr");

           Property(t => t.ComissaoRepresentante)
                .HasColumnName("comis_represent");

           Property(t => t.ComissaoTransportadorMateriaPrima)
                .HasColumnName("com_transp_mp");

           Property(t => t.ComissaoVendedorDiferenca)
                .HasColumnName("com_vend_dif");

           Property(t => t.ComissaoVendaBombista)
                .HasColumnName("com_vend_bomb");

           Property(t => t.ComissaoVendaPadrao)
                .HasColumnName("com_vend_padr");

           Property(t => t.ComissaoVendaTaxaExtra)
                .HasColumnName("com_vend_tx_extra");

           Property(t => t.ComissaoVendaVibrador)
                .HasColumnName("com_vend_vibr");

           Property(t => t.ComissaoVendedor)
                .HasColumnName("comis_vendedor");

           Property(t => t.ComissaoVendedorConcreto)
                .HasColumnName("com_vend_concr");

           Property(t => t.ComissaoVendedorServico)
                .HasColumnName("com_vend_serv");

           Property(t => t.CorpoProva)
                .HasColumnName("cp");

           Property(t => t.CorteAguaPorM3)
                .HasColumnName("corte_agua_pm3");

           Property(t => t.CustoConcretoPesado)
                .HasColumnName("custo_conc_pes");

           Property(t => t.DataBaseComissao)
                .HasColumnName("dt_base_comis");

           Property(t => t.DataColetaCorpoProva)
                .HasColumnName("dt_coleta_cp");

           Property(t => t.DataFatura)
                .HasColumnName("dt_fatura");

           Property(t => t.DataProgramacao)
                .HasColumnName("dt_prog");

           Property(t => t.DataProrrogacaoPendencia)
                .HasColumnName("dt_prorrog_pend");

           Property(t => t.DataRemessa)
                .HasColumnName("data_remessa");

           Property(t => t.DataVencimentoPrimeiraParcela)
                .HasColumnName("dt_vcto_1_parc");

           Property(t => t.DescartadoCorpoProva)
                .HasColumnName("descartado_cp");

            Property(t => t.Desvio)
                .HasColumnName("desvio");

            Property(t => t.EspecificacaoTraco)
                .HasColumnName("especificacao_traco");

            Property(t => t.EsperaInicio)
                .HasColumnName("espera_inicio");

            Property(t => t.EsperaSaidaObra)
                .HasColumnName("espera_saida_ob");

            Property(t => t.FilialEstoque)
                .HasColumnName("filial_estoque");

            Property(t => t.FilialFaturamento)
                .HasColumnName("filial_fat");

            Property(t => t.HoraChegadaUsina)
                .HasColumnName("hr_cheg_usina");

            Property(t => t.HoraFimCarga)
                .HasColumnName("hora_fim_carga");

            Property(t => t.HoraInicioCarga)
                .HasColumnName("hora_ini_carga");

            Property(t => t.HoraPrevista)
                .HasColumnName("hr_prevista");

            Property(t => t.HoraRecomendadaUtilizacao)
                .HasColumnName("hr_recomend_utl");

            Property(t => t.HoraSaidaFuncionario)
                .HasColumnName("hr_saida_func");

            Property(t => t.HoraSaidaObra)
                .HasColumnName("hr_saida_obra");

            Property(t => t.HoraSaidaUsinaEfetiva)
                .HasColumnName("hr_saida_usina_efet");

            Property(t => t.HorimetroFinal)
                .HasColumnName("horimetro_final");

            Property(t => t.HorimetroInicial)
                .HasColumnName("horimetro_inicial");

            Property(t => t.HorimetroRodado)
                .HasColumnName("horimetro_rodado");

            Property(t => t.HoraChegadaObra)
                .HasColumnName("hr_cheg_obra");

            Property(t => t.HoraDescargaFinal)
                .HasColumnName("hr_desc_final");

            Property(t => t.HoraDescargaInicial)
                .HasColumnName("hr_desc_inic");

            Property(t => t.HoraSaidaUsina)
                .HasColumnName("hr_saida_usina");

            Property(t => t.HoraSolicitada)
                .HasColumnName("hr_solicitada");

            Property(t => t.IdAprovacaoDiretoria)
                .HasColumnName("id_aprov_dir");

            Property(t => t.IdAprovacaoLaboratorio)
                .HasColumnName("id_aprov_lab");

            Property(t => t.IdAtual)
                .HasColumnName("id_atual");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdColetaCorpoProva)
                .HasColumnName("id_coleta_cp");

            Property(t => t.ImportaGps)
                .HasColumnName("import_gps");

            Property(t => t.ImportaRfid)
                .HasColumnName("import_rfid");

            Property(t => t.ImportouDaNotaFiscalRemessa)
                .HasColumnName("importou_da_nf");

            Property(t => t.Inconsistencias)
                .HasColumnName("inconsistencias");

            Property(t => t.KmRodado)
                .HasColumnName("km_rodado");

            Property(t => t.M3Bombeado)
                .HasColumnName("m3_bombeado");

            Property(t => t.M3BombeadoCobrado)
                .HasColumnName("m3_bomb_cob");

            Property(t => t.M3Faltantes)
                .HasColumnName("m3_faltantes");

            Property(t => t.MaoDeObraPropria)
                .HasColumnName("mao_obra_prop");

            Property(t => t.MaterialM3)
                .HasColumnName("material_m3");

            Property(t => t.MaterialTotal)
                .HasColumnName("material_total");

            Property(t => t.MinutosDescarga)
                .HasColumnName("minutos_desc");

            Property(t => t.MotivoAtraso)
                .HasColumnName("motivo_atraso");

            Property(t => t.MotivoAtrasoConcretagem)
                .HasColumnName("motivo_atraso_concr");

            Property(t => t.MotivoInconsistencia)
                .HasColumnName("motivo_incons");

            Property(t => t.Motorista)
                .HasColumnName("motorista");

            Property(t => t.NumeroFatura)
                .HasColumnName("num_fatura");

            Property(t => t.NumeroFaturamento)
                .HasColumnName("no_faturamento");

            Property(t => t.NumeroLacre)
                .HasColumnName("num_lacre");

            Property(t => t.ObservacaoEntrega)
                .HasColumnName("obs_entrega");

            Property(t => t.ObservacaoAprovacaoKm)
                .HasColumnName("obs_aprov_km");

            Property(t => t.ObservacaoCancelamento)
                .HasColumnName("obs_cancel");

            Property(t => t.ObservacaoPesagem)
                .HasColumnName("obs_pesagem");

            Property(t => t.ObservacaoTraco)
                .HasColumnName("obs_traco");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.Pendente)
                .HasColumnName("pendente");

            Property(t => t.PercentualAdicionalFeriado)
                .HasColumnName("pct_adic_feriad");

            Property(t => t.PercentualAdicionalHoraExtra)
                .HasColumnName("pct_adic_he");

            Property(t => t.PercentualDesvioPesagem)
                .HasColumnName("pct_desvio_pesagem");

            Property(t => t.PesoRodoviario)
                .HasColumnName("peso_rodoviario");

            Property(t => t.PosVenda)
                .HasColumnName("pos_venda");

            Property(t => t.QuantidadeCorpoProva)
                .HasColumnName("qtde_cp");

            Property(t => t.QuantidadeManualPesagem)
                .HasColumnName("qtde_manual_pes");

            Property(t => t.QuantidadePausaPesagem)
                .HasColumnName("qtde_pausa_pes");

            Property(t => t.Represente)
                .HasColumnName("represent");

            Property(t => t.ResponsavelCliente)
                .HasColumnName("respon_cliente");

            Property(t => t.Slump)
                .HasColumnName("slump");

            Property(t => t.SlumpReal)
                .HasColumnName("slump_real");

            Property(t => t.StatusKmLimite)
                .HasColumnName("status_km_limit");

            Property(t => t.TempoEntreVia)
                .HasColumnName("tempo_entre_via");

            Property(t => t.TempoIda)
                .HasColumnName("tempo_ida");

            Property(t => t.TempoNaObra)
                .HasColumnName("tempo_na_obra");

            Property(t => t.TempoTotal)
                .HasColumnName("tempo_total");

            Property(t => t.TempoVgSaida)
                .HasColumnName("tempo_vg_saida");

            Property(t => t.TempoVolta)
                .HasColumnName("tempo_volta");

            Property(t => t.TerceiroBomba)
                .HasColumnName("terc_bomba");

            Property(t => t.TipoCobranca)
                .HasColumnName("tp_cobranca");

            Property(t => t.TracoConcreto)
                .HasColumnName("traco_concreto");

            Property(t => t.TracoConcretoPesado)
                .HasColumnName("traco_concreto_pesado");

            Property(t => t.UsadoNaNotaFiscalRemessaNumero)
                .HasColumnName("usado_na_nf");

            Property(t => t.UsinaFaturamento)
                .HasColumnName("usina_fat");

            Property(t => t.UsinaPesagem)
                .HasColumnName("usina");

            Property(t => t.ValorBombaCalculo)
                .HasColumnName("vl_bomba_Calc");

            Property(t => t.ValorBombaUnitario)
                .HasColumnName("vlr_bomba_unit");

            Property(t => t.ValorComissaoAuxiliar1)
                .HasColumnName("vlr_com_aux1");

            Property(t => t.ValorComissaoAuxiliar2)
                .HasColumnName("vlr_com_aux2");

            Property(t => t.ValorComissaoAuxiliar3)
                .HasColumnName("vlr_com_aux3");

            Property(t => t.ValorComissaoBombista)
                .HasColumnName("vlr_com_bombist");

            Property(t => t.ValorComissaoMotorista)
                .HasColumnName("vlr_com_motoris");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desco");

            Property(t => t.ValorTaxaComissao)
                .HasColumnName("valor_tx_comis");

            Property(t => t.ValorVendaTabelaTotal)
                .HasColumnName("vlr_vend_tb_tot");

            Property(t => t.Velocimento)
                .HasColumnName("velocimento");

            Property(t => t.VelocimentoFinal)
                .HasColumnName("velocimento_final");

            Property(t => t.Vendedor)
                .HasColumnName("vendedor");

            Property(t => t.VibradorQuantidade)
                .HasColumnName("vibr_qtde");

            Property(t => t.VibradorVendedor)
                .HasColumnName("vibr_vendedor");

            Property(t => t.VibradorValorUnitario)
                .HasColumnName("vibr_vlr_unit");

            Property(t => t.VolumeEntregaBombeado)
                .HasColumnName("vol_entreg_bomb");

            Property(t => t.EsperaNaUsina)
                .HasColumnName("espera_usina");

            Property(t => t.DataAtualizacao)
                .HasColumnName("atualizado_em")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            HasMany(t => t.Itens)
                .WithOptional()
                .HasForeignKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia })
                .WillCascadeOnDelete(false);

            HasMany(t => t.DemaisServicos)
                .WithOptional()
                .HasForeignKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia })
                .WillCascadeOnDelete(false);
        }
    }
}

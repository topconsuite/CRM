using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.Remessa
{
    public class RemessaResponse
    {
        [JsonProperty(PropertyName = "branch")]
        public int FilialCodigo { get; set; }

        [JsonProperty(PropertyName = "intervener")]
        public int IntervenienteCodigo { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumentoCodigo { get; set; }

        [JsonProperty(PropertyName = "invoice_number_packing_list")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "packing_list_invoice_sequence")]
        public int Sequencia { get; set; }

        [JsonProperty(PropertyName = "contract_concrete_batching_plant")]
        public int ContratoUsinaCodigo { get; set; }

        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [JsonProperty(PropertyName = "purpose")]
        public int ContratoFinalidade { get; set; }

        [JsonProperty(PropertyName = "programming_sequence")]
        public int ProgramacaoSequencia { get; set; }

        [JsonProperty(PropertyName = "cancellation_reason")]
        public int MotivoCancelamentoCodigo { get; set; }

        [JsonProperty(PropertyName = "quantity_m3_concrete_mixer")]
        public float Volume { get; set; }

        [JsonProperty(PropertyName = "departure_time_of_the_concrete_batching_plant")]
        public string HoraSaidaUsina { get; set; }

        [JsonProperty(PropertyName = "concrete_mixer_number")]
        public string BetoneiraCodigo { get; set; }

        [JsonProperty(PropertyName = "sales_value_m3")]
        public float? TracoValorUnitario { get; set; }

        [JsonProperty(PropertyName = "total_sale_value")]
        public float TracoValorTotal { get; set; }

        [JsonProperty(PropertyName = "total_value_of_the_pump")]
        public float BombaValorTotal { get; set; }

        [JsonProperty(PropertyName = "missing_m3_value")]
        public float M3FaltanteValor { get; set; }

        [JsonProperty(PropertyName = "vibrator_total_value")]
        public float? VibradorValorTotal { get; set; }

        [JsonProperty(PropertyName = "additional_overtime")]
        public float AdicionalHoraExtraValorTotal { get; set; }

        [JsonProperty(PropertyName = "additional_holiday")]
        public float AdicionalFeriadoValorTotal { get; set; }

        [JsonProperty(PropertyName = "water_addition")]
        public int AdicaoAgua { get; set; }

        [JsonProperty(PropertyName = "water_placed_in_the_concrete_batching_plant")]
        public int AguaColocadaNaUsina { get; set; }

        [JsonProperty(PropertyName = "water_placed_on_the_construction_site")]
        public int AguaColocarNaObra { get; set; }

        [JsonProperty(PropertyName = "delay_delivery")]
        public int AtrasoEntrega { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant")]
        public int AuxiliarBombista { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_secundary")]
        public int AuxiliarBombista2 { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_tertiary")]
        public int AuxiliarBombista3 { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public int Balanca { get; set; }

        [JsonProperty(PropertyName = "pump")]
        public int Bomba { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator")]
        public int Bombista { get; set; }

        [JsonProperty(PropertyName = "family_key")]
        public string ChaveFamilia { get; set; }

        [JsonProperty(PropertyName = "cement")]
        public string Cimento { get; set; }

        [JsonProperty(PropertyName = "inconsistency_code")]
        public int CodigoInconsistencia { get; set; }

        [JsonProperty(PropertyName = "integration_code")]
        public string CodigoIntegracao { get; set; }

        [JsonProperty(PropertyName = "raw_material_transporter_code")]
        public int CodigoTransportadorMateriaPrima { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_code")]
        public string CodigosCorpoProvas { get; set; }

        [JsonProperty(PropertyName = "difference_representation_commission")]
        public double ComissaoRepresentanteDiferenca { get; set; }

        [JsonProperty(PropertyName = "service_representation_commission")]
        public double ComissaoRepresentanteServico { get; set; }

        [JsonProperty(PropertyName = "commission_generated")]
        public string ComissaoGerada { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_commission")]
        public bool ComissaoAjudanteBomba { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_commission")]
        public bool ComissaoBombista { get; set; }

        [JsonProperty(PropertyName = "driver_commission")]
        public bool ComissaoMotorista { get; set; }

        [JsonProperty(PropertyName = "pump_representative_commission")]
        public double ComissaoRepresentanteBomba { get; set; }

        [JsonProperty(PropertyName = "concrete_representative_commission")]
        public double ComissaoRepresentanteConcreto { get; set; }

        [JsonProperty(PropertyName = "representative_commission")]
        public bool ComissaoRepresentante { get; set; }

        [JsonProperty(PropertyName = "raw_material_transporter_commision")]
        public double ComissaoTransportadorMateriaPrima { get; set; }

        [JsonProperty(PropertyName = "sellers_commission_on_the_difference")]
        public double ComissaoVendedorDiferenca { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_sales_commission")]
        public double ComissaoVendaBombista { get; set; }

        [JsonProperty(PropertyName = "default_sales_commission")]
        public double ComissaoVendaPadrao { get; set; }

        [JsonProperty(PropertyName = "extra_fees_sales_commission")]
        public double ComissaoVendaTaxaExtra { get; set; }

        [JsonProperty(PropertyName = "vibrator_sales_commission")]
        public double ComissaoVendaVibrador { get; set; }

        [JsonProperty(PropertyName = "seller_commission")]
        public bool ComissaoVendedor { get; set; }

        [JsonProperty(PropertyName = "concrete_seller_commission")]
        public double ComissaoVendedorConcreto { get; set; }

        [JsonProperty(PropertyName = "service_seller_commission")]
        public double ComissaoVendedorServico { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen")]
        public string CorpoProva { get; set; }

        [JsonProperty(PropertyName = "water_cut_per_m3")]
        public float CorteAguaPorM3 { get; set; }

        [JsonProperty(PropertyName = "concrete_weighed")]
        public double CustoConcretoPesado { get; set; }

        [JsonProperty(PropertyName = "commission_base_date")]
        public DateTime? DataBaseComissao { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_collection_date")]
        public DateTime? DataColetaCorpoProva { get; set; }

        [JsonProperty(PropertyName = "invoice_date")]
        public DateTime? DataFatura { get; set; }

        [JsonProperty(PropertyName = "schedule_date")]
        public DateTime? DataProgramacao { get; set; }

        [JsonProperty(PropertyName = "extension_date_pending")]
        public DateTime? DataProrrogacaoPendencia { get; set; }

        [JsonProperty(PropertyName = "date_packing_list")]
        public DateTime? DataRemessa { get; set; }

        [JsonProperty(PropertyName = "due_date_of_first_installment")]
        public DateTime? DataVencimentoPrimeiraParcela { get; set; }

        [JsonProperty(PropertyName = "discarded_concrete_test_specimen")]
        public string DescartadoCorpoProva { get; set; }

        [JsonProperty(PropertyName = "detour")]
        public bool Desvio { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe_specification")]
        public string EspecificacaoTraco { get; set; }

        [JsonProperty(PropertyName = "wait_to_start")]
        public int EsperaInicio { get; set; }

        [JsonProperty(PropertyName = "wait_for_exit_construction_site")]
        public int EsperaSaidaObra { get; set; }

        [JsonProperty(PropertyName = "stock_branch")]
        public int FilialEstoque { get; set; }

        [JsonProperty(PropertyName = "billing_branch")]
        public int FilialFaturamento { get; set; }

        [JsonProperty(PropertyName = "arrival_time_at_the_concrete_batching_plant")]
        public string HoraChegadaUsina { get; set; }

        [JsonProperty(PropertyName = "end_of_charge_time")]
        public string HoraFimCarga { get; set; }

        [JsonProperty(PropertyName = "charging_start_time")]
        public string HoraInicioCarga { get; set; }

        [JsonProperty(PropertyName = "estimated_time")]
        public string HoraPrevista { get; set; }

        [JsonProperty(PropertyName = "recommended_time_of_use")]
        public string HoraRecomendadaUtilizacao { get; set; }

        [JsonProperty(PropertyName = "employee_departure_time")]
        public string HoraSaidaFuncionario { get; set; }

        [JsonProperty(PropertyName = "construction_site_departure_time")]
        public string HoraSaidaObra { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant_departure_effective_time")]
        public string HoraSaidaUsinaEfetiva { get; set; }

        [JsonProperty(PropertyName = "final_hour_meter")]
        public int HorimetroFinal { get; set; }

        [JsonProperty(PropertyName = "initial_hour_meter")]
        public int HorimetroInicial { get; set; }

        [JsonProperty(PropertyName = "rotated_hour_meter")]
        public int HorimetroRodado { get; set; }

        [JsonProperty(PropertyName = "arrival_time_at_the_construction_site")]
        public string HoraChegadaObra { get; set; }

        [JsonProperty(PropertyName = "unloading_end_time")]
        public string HoraDescargaFinal { get; set; }

        [JsonProperty(PropertyName = "unloading_start_time")]
        public string HoraDescargaInicial { get; set; }

        [JsonProperty(PropertyName = "requested_time")]
        public string HoraSolicitada { get; set; }

        [JsonProperty(PropertyName = "board_approval_id")]
        public string IdAprovacaoDiretoria { get; set; }

        [JsonProperty(PropertyName = "lab_approval_id")]
        public string IdAprovacaoLaboratorio { get; set; }

        [JsonProperty(PropertyName = "update_id")]
        public string IdAtual { get; set; }

        [JsonProperty(PropertyName = "registration_id")]
        public string IdCadastro { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_collection_id")]
        public string IdColetaCorpoProva { get; set; }

        [JsonProperty(PropertyName = "gps_import")]
        public int ImportaGps { get; set; }

        [JsonProperty(PropertyName = "rfid_import")]
        public int ImportaRfid { get; set; }

        [JsonProperty(PropertyName = "imported_from_the_shipping_invoice")]
        public int ImportouDaNotaFiscalRemessa { get; set; }

        [JsonProperty(PropertyName = "inconsistencies")]
        public string Inconsistencias { get; set; }

        [JsonProperty(PropertyName = "km_driven")]
        public int KmRodado { get; set; }

        [JsonProperty(PropertyName = "m3_pumped")]
        public float M3Bombeado { get; set; }

        [JsonProperty(PropertyName = "m3_pumped_charged")]
        public float M3BombeadoCobrado { get; set; }

        [JsonProperty(PropertyName = "m3_missing")]
        public float M3Faltantes { get; set; }

        [JsonProperty(PropertyName = "own_labor")]
        public bool MaoDeObraPropria { get; set; }

        [JsonProperty(PropertyName = "m3_material")]
        public float? MaterialM3 { get; set; }

        [JsonProperty(PropertyName = "total_material")]
        public float? MaterialTotal { get; set; }

        [JsonProperty(PropertyName = "minutes_unloading")]
        public int MinutosDescarga { get; set; }

        [JsonProperty(PropertyName = "reason_for_delay")]
        public string MotivoAtraso { get; set; }

        [JsonProperty(PropertyName = "reason_for_concreting_delay")]
        public string MotivoAtrasoConcretagem { get; set; }

        [JsonProperty(PropertyName = "inconsistency_reason")]
        public string MotivoInconsistencia { get; set; }

        [JsonProperty(PropertyName = "driver")]
        public int Motorista { get; set; }

        [JsonProperty(PropertyName = "invoice_number")]
        public int NumeroFatura { get; set; }

        [JsonProperty(PropertyName = "billing_number")]
        public int NumeroFaturamento { get; set; }

        [JsonProperty(PropertyName = "seal_number")]
        public long NumeroLacre { get; set; }

        [JsonProperty(PropertyName = "delivery_observation")]
        public string ObservacaoEntrega { get; set; }

        [JsonProperty(PropertyName = "observation_km_approval")]
        public string ObservacaoAprovacaoKm { get; set; }

        [JsonProperty(PropertyName = "observation_cancellation")]
        public string ObservacaoCancelamento { get; set; }

        [JsonProperty(PropertyName = "weighing_observation")]
        public string ObservacaoPesagem { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe_observation")]
        public string ObservacaoTraco { get; set; }

        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }

        [JsonProperty(PropertyName = "pending")]
        public int Pendente { get; set; }

        [JsonProperty(PropertyName = "additional_percentage_holiday")]
        public float PercentualAdicionalFeriado { get; set; }

        [JsonProperty(PropertyName = "additional_percentage_overtime")]
        public float PercentualAdicionalHoraExtra { get; set; }

        [JsonProperty(PropertyName = "percentage_deviation_from_weighing")]
        public float PercentualDesvioPesagem { get; set; }

        [JsonProperty(PropertyName = "road_weight")]
        public int PesoRodoviario { get; set; }

        [JsonProperty(PropertyName = "after_sales")]
        public string PosVenda { get; set; }

        [JsonProperty(PropertyName = "concrete_test_specimen_quantity")]
        public int QuantidadeCorpoProva { get; set; }

        [JsonProperty(PropertyName = "manual_quantity_weighing")]
        public int QuantidadeManualPesagem { get; set; }

        [JsonProperty(PropertyName = "quantity_pause_weighing")]
        public int QuantidadePausaPesagem { get; set; }

        [JsonProperty(PropertyName = "representative")]
        public int Represente { get; set; }

        [JsonProperty(PropertyName = "responsible_customer")]
        public bool ResponsavelCliente { get; set; }

        [JsonProperty(PropertyName = "slump")]
        public int Slump { get; set; }

        [JsonProperty(PropertyName = "real_slump")]
        public int SlumpReal { get; set; }

        [JsonProperty(PropertyName = "km_limit_status")]
        public string StatusKmLimite { get; set; }

        [JsonProperty(PropertyName = "time_between_route")]
        public int TempoEntreVia { get; set; }

        [JsonProperty(PropertyName = "departure_time")]
        public int TempoIda { get; set; }

        [JsonProperty(PropertyName = "time_on_work")]
        public int TempoNaObra { get; set; }

        [JsonProperty(PropertyName = "total_time")]
        public int TempoTotal { get; set; }

        [JsonProperty(PropertyName = "time_vg_departure")]
        public int TempoVgSaida { get; set; }

        [JsonProperty(PropertyName = "time_goes_back")]
        public int TempoVolta { get; set; }

        [JsonProperty(PropertyName = "third_party_pump")]
        public int TerceiroBomba { get; set; }

        [JsonProperty(PropertyName = "type_of_charge")]
        public int TipoCobranca { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe")]
        public string TracoConcreto { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe_weighed")]
        public string TracoConcretoPesado { get; set; }

        [JsonProperty(PropertyName = "used_in_the_packing_list_invoice_number")]
        public int UsadoNaNotaFiscalRemessaNumero { get; set; }

        [JsonProperty(PropertyName = "billing_concrete_batching_plantconcrete_batching_plant")]
        public int UsinaFaturamento { get; set; }

        [JsonProperty(PropertyName = "weighing_concrete_batching_plant")]
        public int UsinaPesagem { get; set; }

        [JsonProperty(PropertyName = "pump_calculation_value")]
        public double ValorBombaCalculo { get; set; }

        [JsonProperty(PropertyName = "pump_unit_value")]
        public float ValorBombaUnitario { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_commission_value_first")]
        public double ValorComissaoAuxiliar1 { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_commission_value_secundary")]
        public double ValorComissaoAuxiliar2 { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_assistant_commission_value_tertiary")]
        public double ValorComissaoAuxiliar3 { get; set; }

        [JsonProperty(PropertyName = "concrete_pump_operator_commission_value")]
        public double ValorComissaoBombista { get; set; }

        [JsonProperty(PropertyName = "driver_commission_value")]
        public double ValorComissaoMotorista { get; set; }

        [JsonProperty(PropertyName = "discount_value")]
        public double ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "commission_rate_value")]
        public double ValorTaxaComissao { get; set; }

        [JsonProperty(PropertyName = "total_table_sales_value")]
        public double ValorVendaTabelaTotal { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public int Velocimento { get; set; }

        [JsonProperty(PropertyName = "final_speed")]
        public int VelocimentoFinal { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public int Vendedor { get; set; }

        [JsonProperty(PropertyName = "vibrator_quantity")]
        public int VibradorQuantidade { get; set; }

        [JsonProperty(PropertyName = "vibrator_seller")]
        public int VibradorVendedor { get; set; }

        [JsonProperty(PropertyName = "vibrator_unit_value")]
        public float? VibradorValorUnitario { get; set; }

        [JsonProperty(PropertyName = "pumped_delivery_volume")]
        public float VolumeEntregaBombeado { get; set; }

        [JsonProperty(PropertyName = "wait_for_exit_concrete_batching_plant")]
        public int EsperaNaUsina { get; set; }

        [JsonProperty(PropertyName = "additional_holiday_unit_value")]
        public double ValorUnitarioAdicionalFeriado { get; set; }

        [JsonProperty(PropertyName = "additional_overtime_unit_value")]
        public double ValorUnitarioAdicionalHoraExtra { get; set; }

        [JsonProperty(PropertyName = "additional_maximum_movement_restriction_zone")]
        public double ValorAdicionalZmrc { get; set; }

        [JsonProperty(PropertyName = "additional_extra_piping")]
        public double AdicionalTubulacaoExtra { get; set; }

        [JsonProperty(PropertyName = "confirms_remote_molding")]
        public string ConfirmaMoldagemRemota { get; set; }

        [JsonProperty(PropertyName = "raw_material_equipment_code")]
        public string EquipamentoTransporteMateriaPrimaCodigo { get; set; }

        [JsonProperty(PropertyName = "pumping_end_time")]
        public string HoraBombeamentoFim { get; set; }

        [JsonProperty(PropertyName = "pumping_start_time")]
        public string HoraBombeamentoInicio { get; set; }

        [JsonProperty(PropertyName = "pump_ready_time")]
        public string HoraBombaPronta { get; set; }

        [JsonProperty(PropertyName = "effective_hours_worked")]
        public double HoraTrabalhadaEfetivamente { get; set; }

        [JsonProperty(PropertyName = "pump_hours_worked")]
        public double HoraTrabalhada { get; set; }

        [JsonProperty(PropertyName = "user_remote_molding")]
        public string IdUsuarioMoldagemRemota { get; set; }

        [JsonProperty(PropertyName = "concrete_mixer_order_justification")]
        public string JustificativaOrdemBt { get; set; }

        [JsonProperty(PropertyName = "issue_lot")]
        public int LoteEmissao { get; set; }

        [JsonProperty(PropertyName = "reason_for_change_stay_fee")]
        public string MotivoMudancaTaxaPermanencia { get; set; }

        [JsonProperty(PropertyName = "observation_remote_molding")]
        public string ObservacaoMoldagemRemota { get; set; }

        [JsonProperty(PropertyName = "concrete_mixer_order")]
        public int OrdemBt { get; set; }

        [JsonProperty(PropertyName = "additional_percentage_maximum_movement_restriction_zone")]
        public double PercentualAdicionalZmrc { get; set; }

        [JsonProperty(PropertyName = "additional_km_driven_quantity")]
        public int QuantidadeAdicionalKmRodado { get; set; }

        [JsonProperty(PropertyName = "additional_concrete_return_quantity")]
        public double QuantidadeAdicionalRetornoConcreto { get; set; }

        [JsonProperty(PropertyName = "stay_fee_quantity")]
        public int QuantidadeTaxaPermanencia { get; set; }

        [JsonProperty(PropertyName = "additional_holiday_quantity")]
        public double QuantidadeAdicionalFeriado { get; set; }

        [JsonProperty(PropertyName = "additional_overtime_quantity")]
        public double QuantidadeAdicionalHoraExtra { get; set; }

        [JsonProperty(PropertyName = "reuse_programming")]
        public double ReaproveitamentoProgramacao { get; set; }

        [JsonProperty(PropertyName = "pump_stay_fee_quantity")]
        public double QuantidadeTaxaPermanenciaBomba { get; set; }

        [JsonProperty(PropertyName = "pump_stay_fee_unit_value")]
        public double ValorUnitarioTaxaPermanenciaBomba { get; set; }

        [JsonProperty(PropertyName = "stay_fee_value")]
        public double ValorTaxaPermanencia { get; set; }

        [JsonProperty(PropertyName = "pump_stay_fee_value")]
        public double ValorTaxaPermanenciaBomba { get; set; }

        [JsonProperty(PropertyName = "contract_version")]
        public int VersaoContrato { get; set; }

        [JsonProperty(PropertyName = "additional_km_driven_unit_value")]
        public double ValorUnitarioAdicionalKmRodado { get; set; }

        [JsonProperty(PropertyName = "additional_concrete_return_unit_value")]
        public double ValorUnitarioAdicionalRetornoConcreto { get; set; }

        [JsonProperty(PropertyName = "additional_value")]
        public double ValorAdicionais { get; set; }

        [JsonProperty(PropertyName = "other_services_value")]
        public double ValorDemaisServicos { get; set; }

        [JsonProperty(PropertyName = "hours_worked_value")]
        public double ValorHoraTrabalhada { get; set; }

        [JsonProperty(PropertyName = "additional_km_driven_value")]
        public double ValorAdicionalKmRodado { get; set; }

        [JsonProperty(PropertyName = "additional_concrete_return_value")]
        public double ValorAdicionalRetornoConcreto { get; set; }

        [JsonProperty(PropertyName = "total_charge_amount")]
        public double ValorTotalCobranca { get; set; }

        [JsonProperty(PropertyName = "pump_hours_worked_unit_value")]
        public double ValorVendaHoraBomba { get; set; }

        [JsonProperty(PropertyName = "pump_hours_worked_value")]
        public double ValorTotalHoraBomba { get; set; }

        [JsonProperty(PropertyName = "product_numbering")]
        public long NumeracaoProduto { get; set; }

        [JsonProperty(PropertyName = "cei_cno")]
        public string Cei { get; set; } = "";

        [JsonProperty(PropertyName = "contract_observation")]
        public string ObservacaoContrato { get; set; } = "";

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "update_date")]
        public DateTime? DataAtualizacao { get; set; }

        [JsonProperty(PropertyName = "items")]
        public RemessaItemDto[] Itens { get; set; }

        [JsonProperty(PropertyName = "other_services")]
        public RemessaDemaisServicosDto[] DemaisServicos { get; set; }

        [JsonProperty(PropertyName = "reuses")]
        public RemessaReaproveitamentoDto[] Reaproveitamentos { get; set; }
    }
}

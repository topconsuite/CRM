using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalFisica : IQueryResult
    {
        //filial
        public int FilialCodigo { get; set; }

        //interv
        public int IntervenienteCodigo { get; set; }

        //tp_doc
        public int TipoDocumentoCodigo { get; set; }

        //num_nf
        public long Numero { get; set; }

        //serie
        public string Serie { get; set; }

        //seq_nf
        public int Sequencia { get; set; }

        //usina_contrato
        public int ContratoUsinaCodigo { get; set; }

        //ano_contrato
        public int ContratoAno { get; set; }

        //num_contrato
        public int ContratoNumero { get; set; }

        //seq_prog
        public int ProgramacaoSequencia { get; set; }

        //motivo_cancel
        public int MotivoCancelamentoCodigo { get; set; }

        //------------------
        //qtde_m3_bt
        public float Volume { get; set; }

        //hr_saida_usina
        public string HoraSaidaUsina { get; set; }

        //no_betoneira
        public string BetoneiraCodigo { get; set; }

        //vlr_venda_m3
        public float? TracoValorUnitario { get; set; }

        //vlr_venda_total
        public float TracoValorTotal { get; set; }

        //vlr_bomba_total
        public float BombaValorTotal { get; set; }

        //vlr_m3_fal
        public float M3FaltanteValor { get; set; }

        //vibr_vlr_total
        public float? VibradorValorTotal { get; set; }

        //adic_he
        public float AdicionalHoraExtraValorTotal { get; set; }

        //adic_feriado
        public float AdicionalFeriadoValorTotal { get; set; }

        public int AdicaoAgua { get; set; }

        public int AguaColocadaNaUsina { get; set; }

        public int AguaColocarNaObra { get; set; }

        public int AtrasoEntrega { get; set; }

        public int AuxiliarBombista { get; set; }

        public int AuxiliarBombista2 { get; set; }

        public int AuxiliarBombista3 { get; set; }

        public int Balanca { get; set; }

        public int Bomba { get; set; }

        public int Bombista { get; set; }

        public string ChaveFamilia { get; set; }

        public string Cimento { get; set; }

        public int CodigoInconsistencia { get; set; }

        public string CodigoIntegracao { get; set; }

        public int CodigoTransportadorMateriaPrima { get; set; }

        public string CodigosCorpoProvas { get; set; }

        public double ComissaoRepresentanteDiferenca { get; set; }

        public double ComissaoRepresentanteServico { get; set; }

        public string ComissaoGerada { get; set; }

        public string ComissaoAjudanteBomba { get; set; }

        public string ComissaoBombista { get; set; }

        public string ComissaoMotorista { get; set; }

        public double ComissaoRepresentanteBomba { get; set; }

        public double ComissaoRepresentanteConcreto { get; set; }

        public string ComissaoRepresentante { get; set; }

        public double ComissaoTransportadorMateriaPrima { get; set; }

        public double ComissaoVendedorDiferenca { get; set; }

        public double ComissaoVendaBombista { get; set; }

        public double ComissaoVendaPadrao { get; set; }

        public double ComissaoVendaTaxaExtra { get; set; }

        public double ComissaoVendaVibrador { get; set; }

        public string ComissaoVendedor { get; set; }

        public double ComissaoVendedorConcreto { get; set; }

        public double ComissaoVendedorServico { get; set; }

        public string CorpoProva { get; set; }

        public float CorteAguaPorM3 { get; set; }

        public double CustoConcretoPesado { get; set; }

        public DateTime? DataBaseComissao { get; set; }

        public DateTime? DataColetaCorpoProva { get; set; }

        public DateTime? DataFatura { get; set; }

        public DateTime? DataProgramacao { get; set; }

        public DateTime? DataProrrogacaoPendencia { get; set; }

        public DateTime? DataRemessa { get; set; }

        public DateTime? DataVencimentoPrimeiraParcela { get; set; }

        public string DescartadoCorpoProva { get; set; }

        public string Desvio { get; set; }

        public string EspecificacaoTraco { get; set; }

        public int EsperaInicio { get; set; }

        public int EsperaSaidaObra { get; set; }

        public int FilialEstoque { get; set; }

        public int FilialFaturamento { get; set; }

        public string HoraChegadaUsina { get; set; }

        public string HoraFimCarga { get; set; }

        public string HoraInicioCarga { get; set; }

        public string HoraPrevista { get; set; }

        public string HoraRecomendadaUtilizacao { get; set; }

        public string HoraSaidaFuncionario { get; set; }

        public string HoraSaidaObra { get; set; }

        public string HoraSaidaUsinaEfetiva { get; set; }

        public int HorimetroFinal { get; set; }

        public int HorimetroInicial { get; set; }

        public int HorimetroRodado { get; set; }

        public string HoraChegadaObra { get; set; }

        public string HoraDescargaFinal { get; set; }

        public string HoraDescargaInicial { get; set; }

        public string HoraSolicitada { get; set; }

        public string IdAprovacaoDiretoria { get; set; }

        public string IdAprovacaoLaboratorio { get; set; }

        public string IdAtual { get; set; }

        public string IdCadastro { get; set; }

        public string IdColetaCorpoProva { get; set; }

        public int ImportaGps { get; set; }

        public int ImportaRfid { get; set; }

        public int ImportouDaNotaFiscalRemessa { get; set; }

        public string Inconsistencias { get; set; }

        public int KmRodado { get; set; }

        public float M3Bombeado { get; set; }

        public float M3BombeadoCobrado { get; set; }

        public float M3Faltantes { get; set; }

        public string MaoDeObraPropria { get; set; }

        public float? MaterialM3 { get; set; }

        public float? MaterialTotal { get; set; }

        public int MinutosDescarga { get; set; }

        public string MotivoAtraso { get; set; }

        public string MotivoAtrasoConcretagem { get; set; }

        public string MotivoInconsistencia { get; set; }

        public int Motorista { get; set; }

        public int NumeroFatura { get; set; }

        public int NumeroFaturamento { get; set; }

        public long NumeroLacre { get; set; }

        public string ObservacaoEntrega { get; set; }

        public string ObservacaoAprovacaoKm { get; set; }

        public string ObservacaoCancelamento { get; set; }

        public string ObservacaoPesagem { get; set; }

        public string ObservacaoTraco { get; set; }

        public string Observacao { get; set; }

        public int Pendente { get; set; }

        public float PercentualAdicionalFeriado { get; set; }

        public float PercentualAdicionalHoraExtra { get; set; }

        public float PercentualDesvioPesagem { get; set; }

        public int PesoRodoviario { get; set; }

        public string PosVenda { get; set; }

        public int QuantidadeCorpoProva { get; set; }

        public int QuantidadeManualPesagem { get; set; }

        public int QuantidadePausaPesagem { get; set; }

        public int Represente { get; set; }

        public string ResponsavelCliente { get; set; }

        public int Slump { get; set; }

        public int SlumpReal { get; set; }

        public string StatusKmLimite { get; set; }

        public int TempoEntreVia { get; set; }

        public int TempoIda { get; set; }

        public int TempoNaObra { get; set; }

        public int TempoTotal { get; set; }

        public int TempoVgSaida { get; set; }

        public int TempoVolta { get; set; }

        public int TerceiroBomba { get; set; }

        public int TipoCobranca { get; set; }

        public string TracoConcreto { get; set; }

        public string TracoConcretoPesado { get; set; }

        public int UsadoNaNotaFiscalRemessaNumero { get; set; }

        public int UsinaFaturamento { get; set; }

        public int UsinaPesagem { get; set; }

        public double ValorBombaCalculo { get; set; }

        public float ValorBombaUnitario { get; set; }

        public double ValorComissaoAuxiliar1 { get; set; }

        public double ValorComissaoAuxiliar2 { get; set; }

        public double ValorComissaoAuxiliar3 { get; set; }

        public double ValorComissaoBombista { get; set; }

        public double ValorComissaoMotorista { get; set; }

        public double ValorDesconto { get; set; }

        public double ValorTaxaComissao { get; set; }

        public double ValorVendaTabelaTotal { get; set; }

        public int Velocimento { get; set; }

        public int VelocimentoFinal { get; set; }

        public int Vendedor { get; set; }

        public int VibradorQuantidade { get; set; }

        public int VibradorVendedor { get; set; }

        public float? VibradorValorUnitario { get; set; }

        public float VolumeEntregaBombeado { get; set; }

        public int EsperaNaUsina { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<NotaFiscalFisicaItem> Itens { get; set; }

        public virtual NotaFiscalFisicaComplemento Complemento { get; set; }

        public ICollection<Reaproveitamento> Reaproveitamentos { get; set; }

        public ICollection<NotaFiscalFisicaDemaisServicos> DemaisServicos { get; set; }
    }
}

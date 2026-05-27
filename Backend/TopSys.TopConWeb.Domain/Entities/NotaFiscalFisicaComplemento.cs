using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalFisicaComplemento
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

        //tx_permanencia
        public float TaxaPermanenciaValor { get; set; }

        //vlr_adicionais
        public float ValorAdicionais { get; set; }

        //vlr_tot_adic_km
        public float AdicionalKmValorTotal { get; set; }

        //vlr_tot_ret_con
        public float AdicionalRetornoConcretoTotal { get; set; }

        //vlr_demais_servicos
        public float ValorDemaisServicos { get; set; }

        //vlr_total_cobranca
        public float ValorTotalCobranca { get; set; }

        public double ValorUnitarioAdicionalFeriado { get; set; }

        public double ValorUnitarioAdicionalHoraExtra { get; set; }

        public double ValorAdicionalZmrc { get; set; }

        public double AdicionalTubulacaoExtra { get; set; }

        public string ConfirmaMoldagemRemota { get; set; }

        public string EquipamentoTransporteMateriaPrimaCodigo { get; set; }

        public string HoraBombeamentoFim { get; set; }

        public string HoraBombeamentoInicio { get; set; }

        public string HoraBombaPronta { get; set; }

        public double HoraTrabalhadaEfetivamente { get; set; }

        public double HoraTrabalhada { get; set; }

        public string IdUsuarioMoldagemRemota { get; set; }

        public string JustificativaOrdemBt { get; set; }

        public int LoteEmissao { get; set; }

        public string MotivoMudancaTaxaPermanencia { get; set; }

        public string ObservacaoMoldagemRemota { get; set; }

        public int OrdemBt { get; set; }

        public double PercentualAdicionalZmrc { get; set; }

        public int QuantidadeAdicionalKmRodado { get; set; }

        public double QuantidadeAdicionalRetornoConcreto { get; set; }

        public int QuantidadeTaxaPermanencia { get; set; }

        public double QuantidadeAdicionalFeriado { get; set; }

        public double QuantidadeAdicionalHoraExtra { get; set; }

        public double ReaproveitamentoProgramacao { get; set; }

        public double QuantidadeTaxaPermanenciaBomba { get; set; }

        public double ValorUnitarioTaxaPermanenciaBomba { get; set; }

        public double TaxaPermanenciaBombaValor { get; set; }

        public int VersaoContrato { get; set; }

        public double ValorUnitarioAdicionalKmRodado { get; set; }

        public double ValorUnitarioAdicionalRetornoConcreto { get; set; }

        public double ValorHoraTrabalhada { get; set; }

        public double ValorVendaHoraBomba { get; set; }

        public double ValorTotalHoraBomba { get; set; }

        public long NumeracaoProduto { get; set; }
    }
}

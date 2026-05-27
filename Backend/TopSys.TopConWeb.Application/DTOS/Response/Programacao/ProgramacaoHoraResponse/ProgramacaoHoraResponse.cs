namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoHoraResponse
{
    public class ProgramacaoHoraResponse
    {
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public int Sequencia { get; set; }

        public string Horario { get; set; }

        public float VolumeProgramado { get; set; }
        public float VolumeEntregue { get; set; }

        public int CorpoDeProvaQuantidade { get; set; }

        public string Status { get; set; }

        public int? NfFilialCodigo { get; set; } = 0;
        public int? NfIntervenienteCodigo { get; set; } = 0;
        public int? NfTipoDocumentoCodigo { get; set; } = 0;
        public long? NfNumero { get; set; } = 0;
        public string NfSerie { get; set; } = "";
        public int? NfSequencia { get; set; } = 0;

        public int NfMotivoCancelamentoCodigo { get; set; }

        public float NfVolume { get; set; }

        public string NfHoraSaidaUsina { get; set; }

        public string NfBetoneiraCodigo { get; set; }

        public float NfTracoValorUnitario { get; set; }

        public float NfTracoValorTotal { get; set; }

        public float NfBombaValorTotal { get; set; }

        public float NfM3FaltanteValor { get; set; }

        public float NfVibradorValorTotal { get; set; }

        public float NfAdicionalHoraExtraValorTotal { get; set; }

        public float NfAdicionalFeriadoValorTotal { get; set; }

        public float NfComplementoTaxaPermanenciaValor { get; set; }

        public float NfComplementoValorAdicionais { get; set; }

        public float NfComplementoAdicionalKmValorTotal { get; set; }

        public float NfComplementoAdicionalRetornoConcretoTotal { get; set; }

        public float NfComplementoValorDemaisServicos { get; set; }

        public float NfComplementoValorTotalCobranca { get; set; }
    }
}

using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse
{
    public class ContratoDTO
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string DescricaoCoincidencia { get; set; }
        public string AguardandoAprovacao { get; set; }
        public EContratoStatus Status { get; set; }
        public IntervenienteDTO Interveniente { get; set; }
        public FuncionarioDTO Analista { get; set; }
        public string CodigoObraPrefeitura { get; set; }
        public bool FaturamentoPendente { get; set; }
        public int ModeloDocumentoRemessaConcreto { get; set; }
        public int ModeloDocumentoRemessaBomba { get; set; }
        public int ModeloItensDanfeERomaneio { get; set; }
        public float PercentualRetencaoContratual { get; set; }
        public string MaoObraPropria { get; set; }
        public float PercentualLocacao { get; set; }
    }
}

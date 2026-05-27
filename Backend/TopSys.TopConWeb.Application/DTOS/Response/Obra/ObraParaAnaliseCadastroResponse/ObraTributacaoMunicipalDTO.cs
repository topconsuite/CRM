namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse
{
    public class ObraTributacaoMunicipalDTO
    {
        public int ObraUsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int? ContratoAno { get; set; } = 0;
        public int? ContratoNumero { get; set; } = 0;

        public int UsinaEntregaCodigo { get; set; }

        public string CodigoObraPrefeitura { get; set; }

        public string ObraCCM { get; set; }

        public int TributacaoISS { get; set; }

        public string RetencaoISS { get; set; }
    }
}

using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest
{
    public class ObraAlterarStatusCadastroEAnalistaRequest
    {
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public EObraStatusCadastro StatusCadastro { get; set; }
        public ContratoDTO Contrato { get; set; }
        public string ObservacaoAcompanhamento { get; set; } = "";
    }
}

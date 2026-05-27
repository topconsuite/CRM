using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;

namespace TopSys.TopConWeb.Application.DTOS.Response.Lead
{
    public class LeadContatoDTO
    {
        public int Usina { get; set; }
        public int NumeroLead { get; set; }
        public int AnoLead { get; set; }
        public int Sequencia { get; set; }
        public string Nome { get; set; }
        public CadastroGeralDTO Funcao { get; set; }
        public int Ddd { get; set; }
        public int Telefone { get; set; }
        public int DddCelular { get; set; }
        public int Celular { get; set; }
        public string Email { get; set; }
    }
}

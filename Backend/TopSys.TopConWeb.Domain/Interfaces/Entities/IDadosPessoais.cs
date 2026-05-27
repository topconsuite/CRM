namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IDadosPessoais : IHasEndereco
    {
        string IntervenienteTipo { get; set; }
        string CpfCnpj { get; set; }
        string Razao { get; set; }
        string Nome { get; set; }
        string InscricaoEstadual { get; set; }
        string InscricaoMunicipal { get; set; }
        string Rg { get; set; }
        string OrgaoExpedidor { get; set; }
        string Email { get; set; }
    }
}

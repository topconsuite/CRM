using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ILeadContatoRepository : IRepositoryBase<LeadContato>
    {
        new void Adicionar(LeadContato contato);
    }
}

using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IMercadoriaRepository : IRepositoryBase<Mercadoria>
    {
        bool ObtemTracoCriadoPeloTech(string codMercadoria);
    }
}

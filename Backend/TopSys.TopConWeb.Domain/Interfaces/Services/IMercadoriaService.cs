using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IMercadoriaService: IServiceBase<Mercadoria>
    {
        Mercadoria ObterTracoMercadoria(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
        bool TracoCriadoPeloTech(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
    }
}

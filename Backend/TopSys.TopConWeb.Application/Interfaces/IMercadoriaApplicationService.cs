using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IMercadoriaApplicationService: IApplicationServiceBase<Mercadoria>
    {
        MercadoriaResponse ObterTracoMercadoriaComDescricaoPersonalizada(int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo);
    }
}

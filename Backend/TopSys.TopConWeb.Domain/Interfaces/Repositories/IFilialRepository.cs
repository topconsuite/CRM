using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IFilialRepository : IRepositoryBase<Filial>
    {
        Filial ObterPorId(int idFilial);
        ICollection<Filial> Listar();
        Filial ObterPorCentroCusto(int centroCusto);
    }
}

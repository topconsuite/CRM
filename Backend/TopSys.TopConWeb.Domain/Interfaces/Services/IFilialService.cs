using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IFilialService : IServiceBase<Filial>
    {
        Filial ObterPorId(int idFilial);
        ICollection<Filial> Listar();
        Filial ObterPorCentroCusto(int centroCusto);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IPortadorRepository : IRepositoryBase<Portador>
    {
        IEnumerable<Portador> ListarVinculadosComContas();
        ICollection<Portador> ListarPortador();
        Portador ObterPorIdPortador(int id, bool tracking = false);
        bool EstaEmUsoPortador(int id);
    }
}

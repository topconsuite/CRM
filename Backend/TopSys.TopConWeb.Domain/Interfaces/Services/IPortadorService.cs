using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IPortadorService : IServiceBase<Portador>
    {
        IEnumerable<Portador> ListarVinculadosComContas();
        ICollection<Portador> Listar();
        Portador ObterPorId(int id, bool tracking = false);
        bool EstaEmUsoPortador(int id);
    }
}

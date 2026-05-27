using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IFuncionarioRepository: IRepositoryBase<Funcionario>
    {
        IEnumerable<Funcionario> ListarAnalistas();
        bool ReEmUso(int re);
        int ObterProximoCodigo();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IFuncionarioService: IServiceBase<Funcionario>
    {
        IEnumerable<Funcionario> ListarAnalistas();
        bool ReEmUso(int re);
        int ObterProximoCodigo();
    }
}

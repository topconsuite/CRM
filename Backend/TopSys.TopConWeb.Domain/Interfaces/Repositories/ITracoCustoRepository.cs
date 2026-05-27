using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITracoCustoRepository : IRepositoryBase<TracoCusto>
    {
        TracoCusto ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase);

        TracoCusto ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase
            (int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase);
    }
}

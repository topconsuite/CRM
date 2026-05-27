using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TracoCustoService : ServiceBase<TracoCusto>, ITracoCustoService
    {
        private ITracoCustoRepository _tracoCustoRepository;

        public TracoCustoService(ITracoCustoRepository tracoCustoRepository) : base(tracoCustoRepository)
        {
            _tracoCustoRepository = tracoCustoRepository;
        }

        public TracoCusto ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase)
        {
            return _tracoCustoRepository.ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, dataBase);
        }

        public TracoCusto ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase)
        {
            return _tracoCustoRepository.ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, dataBase);
        }
    }
}

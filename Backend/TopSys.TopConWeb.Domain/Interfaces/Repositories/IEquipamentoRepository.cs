using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IEquipamentoRepository : IRepositoryBase<Equipamento>
    {

        Equipamento ObterPorId(string codigo, bool tracking = false);

        Equipamento ObterPorPlaca(string placa, bool tracking = false);

        Equipamento ObterPorExternalId(string externalId, bool tracking = false);

        bool JaFoiUtilizado(string codEquipamento);

    }
}

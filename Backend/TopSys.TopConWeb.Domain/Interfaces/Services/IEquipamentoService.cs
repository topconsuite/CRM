using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IEquipamentoService : IServiceBase<Equipamento>
    {

        Equipamento ObterPorId(string codigo, bool tracking = false);
        Equipamento ObterPorPlaca(string placa, bool tracking = false);
        bool JaFoiUtilizado(string codEquipamento);
        Equipamento ObterPorExternalId(string externalId, bool tracking = false);

    }
}

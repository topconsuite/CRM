using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class EquipamentoService : ServiceBase<Equipamento>, IEquipamentoService
    {

        private readonly IEquipamentoRepository _equipamentoRepository;

        public EquipamentoService(IEquipamentoRepository equipamentoRepository) : base(equipamentoRepository)
        {
            _equipamentoRepository = equipamentoRepository;
        }

        public Equipamento ObterPorId(string codigo, bool tracking = false)
        {
            return _equipamentoRepository.ObterPorId(codigo, tracking);
        }

        public Equipamento ObterPorPlaca(string placa, bool tracking = false)
        {
            return _equipamentoRepository.ObterPorPlaca(placa, tracking);
        }

        public Equipamento ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _equipamentoRepository.ObterPorExternalId(externalId, tracking);
        }

        public bool JaFoiUtilizado(string codEquipamento)
        {
            return _equipamentoRepository.JaFoiUtilizado(codEquipamento);
        }
    }
}

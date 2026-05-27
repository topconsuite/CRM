using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class FuncionarioComplementoService : ServiceBase<FuncionarioComplemento>, IFuncionarioComplementoService
    {
        private readonly IFuncionarioComplementoRepository _funcionarioComplementoRepository;

        public FuncionarioComplementoService(IFuncionarioComplementoRepository funcionarioComplementoRepository) : base(funcionarioComplementoRepository)
        {
            _funcionarioComplementoRepository = funcionarioComplementoRepository;
        }
    }
}

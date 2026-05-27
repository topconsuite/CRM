using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class FuncionarioComplementoApplicationService : ApplicationServiceBase<FuncionarioComplemento>, IFuncionarioComplementoApplicationService
    {
        private readonly IFuncionarioComplementoService _funcionarioComplementoService;

        public FuncionarioComplementoApplicationService(IFuncionarioComplementoService funcionarioComplementoService, IUnitOfWork unityOfWork) : base(funcionarioComplementoService, unityOfWork)
        {
            _funcionarioComplementoService = funcionarioComplementoService;
        }
    }
}

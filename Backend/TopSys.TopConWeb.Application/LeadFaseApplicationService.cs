using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class LeadFaseApplicationService : ApplicationServiceBase<LeadFase>, ILeadFaseApplicationService
    {
        private readonly ILeadFaseService _faseLeadService;

        public LeadFaseApplicationService(ILeadFaseService faseLeadService, IUnitOfWork unityOfWork) : base(faseLeadService, unityOfWork)
        {
            _faseLeadService = faseLeadService;
        }
    }
}

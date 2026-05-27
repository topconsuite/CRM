using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class SegmentacaoApplicationService : ApplicationServiceBase<Segmentacao>, ISegmentacaoApplicationService
    {

        private readonly ISegmentacaoService _segmentacaoService;

        public SegmentacaoApplicationService(ISegmentacaoService segmentacaoService, IUnitOfWork unityOfWork) : base(segmentacaoService, unityOfWork)
        {
            _segmentacaoService = segmentacaoService;
        }

    }
}

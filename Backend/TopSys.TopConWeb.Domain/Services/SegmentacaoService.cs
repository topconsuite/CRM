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
    public class SegmentacaoService : ServiceBase<Segmentacao>, ISegmentacaoService
    {

        private readonly ISegmentacaoRepository _segmentacaoRepository;

        public SegmentacaoService(ISegmentacaoRepository segmentacaoRepository) : base(segmentacaoRepository)
        {
            _segmentacaoRepository = segmentacaoRepository;
        }

    }
}

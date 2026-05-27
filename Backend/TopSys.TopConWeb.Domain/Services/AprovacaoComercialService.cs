using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Services
{
    public class AprovacaoComercialService : ServiceBase<AprovacaoComercialUsina>, IAprovacaoComercialService
    {

        private readonly IAprovacaoComercialUsinaRepository _aprovacaoComercialUsinaRepository;
        private readonly IAprovacaoComercialHierarquiaRepository _aprovacaoComercialHierarquiaRepository;

        public AprovacaoComercialService(IAprovacaoComercialUsinaRepository aprovacaoComercialUsinaRepository, IAprovacaoComercialHierarquiaRepository aprovacaoComercialHierarquiaRepository): base(aprovacaoComercialUsinaRepository)
        {
            _aprovacaoComercialHierarquiaRepository = aprovacaoComercialHierarquiaRepository;
            _aprovacaoComercialUsinaRepository = aprovacaoComercialUsinaRepository;
        }

        public AprovacaoComercialUsina ObterPorId(Guid id, bool tracking = false)
        {
            return _aprovacaoComercialUsinaRepository.ObterPorId(id, tracking);
        }
        public AprovacaoComercialUsina ObterPorUsina(int usinaId, bool tracking = false)
        {
            return _aprovacaoComercialUsinaRepository.ObterPorUsina(usinaId, tracking);
        }

        public PagedList<AprovacaoComercialUsina> ListarAprovacaoComercialUsina(int pagina, int porPagina, Expression<Func<AprovacaoComercialUsina, bool>> filter)
        {

            var result = _aprovacaoComercialUsinaRepository.Listar(pagina, porPagina, filter);

            return result;
        }

        public bool UtilizaAprovacaoComercialPorAlcada(int usinaId)
        {
            return _aprovacaoComercialUsinaRepository.UtilizaAprovacaoComercialPorAlcada(usinaId);
        }

        public void AdicionarLog(AprovacaoComercialLog log)
        {
            _aprovacaoComercialUsinaRepository.AdicionarLog(log);
        }

    }
}

using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TributacaoReformaService : ServiceBase<TributacaoReforma>, ITributacaoReformaService
    {
        private readonly ITributacaoReformaRepository _tributacaoReformaRepository;

        public TributacaoReformaService(ITributacaoReformaRepository ITributacaoReformaRepository) : base(ITributacaoReformaRepository)
        {
            _tributacaoReformaRepository = ITributacaoReformaRepository;
        }

        public IEnumerable<TributacaoReforma> ListarTodosProducao(string imposto)
        {
            return _tributacaoReformaRepository.ListarTodosProducao(imposto);
        }
    }
}

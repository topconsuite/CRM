using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TipoDeCobrancaService : ServiceBase<TipoDeCobranca>, ITipoDeCobrancaService
    {
        private readonly ITipoDeCobrancaRepository _tipoDeCobrancaRepository;
        
        public TipoDeCobrancaService(ITipoDeCobrancaRepository tipoDeCobrancaRepository) : base(tipoDeCobrancaRepository)
        {
            _tipoDeCobrancaRepository = tipoDeCobrancaRepository;
        }

        public ICollection<TipoDeCobranca> Listar()
        {
            return _tipoDeCobrancaRepository.ListarTipoDeCobranca();
        }

        public TipoDeCobranca ObterPorId(int id, bool tracking = false)
        {
            return _tipoDeCobrancaRepository.ObterPorIdTipoDeCobranca(id, tracking);
        }
    }
}
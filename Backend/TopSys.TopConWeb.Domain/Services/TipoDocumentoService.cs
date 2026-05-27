using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TipoDocumentoService : ServiceBase<TipoDocumento>, ITipoDocumentoService{
        
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public TipoDocumentoService(ITipoDocumentoRepository tipoDocumentoRepository) : base(tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }

        public ICollection<TipoDocumento> Listar()
        {
            return _tipoDocumentoRepository.ListarTipoDocumento();
        }

        public TipoDocumento ObterPorId(int id, bool tracking = false)
        {
            return _tipoDocumentoRepository.ObterPorIdTipoDocumento(id, tracking);
        }
    }
}
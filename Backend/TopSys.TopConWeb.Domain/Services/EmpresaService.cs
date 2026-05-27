using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class EmpresaService : ServiceBase<Empresa>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaService(IEmpresaRepository empresaRepository) : base(empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }
        public ICollection<Empresa> Listar()
        {
            return _empresaRepository.ListarEmpresa();
        }

        public Empresa ObterPorId(int id, bool tracking = false)
        {
            return _empresaRepository.ObterPorIdEmpresa(id, tracking);
        }
    }
}

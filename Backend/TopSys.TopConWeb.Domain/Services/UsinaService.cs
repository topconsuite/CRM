using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;

namespace TopSys.TopConWeb.Domain.Services
{
    public class UsinaService : ServiceBase<Usina>, IUsinaService
    {

        private readonly IUsinaRepository _usinaRepository;

        public UsinaService(IUsinaRepository usinaRepository) : base(usinaRepository)
        {
            _usinaRepository = usinaRepository;
        }

        public IEnumerable<Usina> ListarPorEmpresa(int empresa)
        {
            return _usinaRepository.ListarPorEmpresa(empresa);
        }

        public float? ObterValorAdicionalM3PorUsinaKm(int idUsina, int km)
        {
            var valor = _usinaRepository.ObterValorAdicionalM3PorUsinaKm(idUsina, km);

            ObraScopes.KmAtendidoScopeIsValid(valor);

            return valor;
        }

        public bool UsinaAtendeKm(int idUsina, int km)
        {
            return _usinaRepository.UsinaAtendeKm(idUsina, km);
        }

        public IEnumerable<Usina> ListarUsinasPermitidasUsuario(string idUsuario)
        {
            var usinas = _usinaRepository.ListarUsinasPermitidasUsuario(idUsuario);

            if (usinas.Count() == 0)
                usinas = _usinaRepository.ListarTodos();

            return usinas;
        }

        public IEnumerable<Usina> ListarUsinasAtivas()
        {
            return _usinaRepository.ListarUsinasAtivas();
        }
    }
}

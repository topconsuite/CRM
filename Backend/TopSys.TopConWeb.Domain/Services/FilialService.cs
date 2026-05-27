using System.Collections;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class FilialService : ServiceBase<Filial>, IFilialService
    {

        private readonly IFilialRepository _filialRepository;

        public FilialService(IFilialRepository filialRepository) : base(filialRepository)
        {
            _filialRepository = filialRepository;
        }

        public Filial ObterPorId(int idFilial)
        {
            var filial = _filialRepository.ObterPorId(idFilial);

            return filial;
        }

        public ICollection<Filial> Listar()
        {

            var filiais = _filialRepository.Listar();

            return filiais;

        }

        public Filial ObterPorCentroCusto(int centroCusto)
        {
            return _filialRepository.ObterPorCentroCusto(centroCusto);
        }
    }
}

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
    public class BombaPrecoService : IBombaPrecoService
    {
        private readonly IBombaPrecoRepository _bombaPrecoRepository;

        public BombaPrecoService(IBombaPrecoRepository bombaPrecoRepository)
        {
            _bombaPrecoRepository = bombaPrecoRepository;
        }

        public IEnumerable<CadastroGeral> ListarBombaTiposPorUsina(int idUsina)
        {
            return _bombaPrecoRepository.ListarBombaTiposPorUsina(idUsina);
        }

        public IEnumerable<Interveniente> ListarTerceirosPorBombaTipo(int idBombaTipo)
        {
            return _bombaPrecoRepository.ListarTerceirosPorBombaTipo(idBombaTipo);
        }

        public BombaPrecoTerceiro ObterPorBombistaBombaTipoData(int idBombista, int idBombaTipo, DateTime dataBase)
        {
            return _bombaPrecoRepository.ObterPorBombistaBombaTipoData(idBombista, idBombaTipo, dataBase);
        }

        public BombaPreco ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime dataBase)
        {
            return _bombaPrecoRepository.ObterPorUsinaBombaTipoData(idUsina, idBombaTipo, dataBase);
        }

        public float ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao)
        {
            return _bombaPrecoRepository.ObterValorAdicional(idUsina, idBombaTipo, distanciaTubulacao);
        }
    }
}

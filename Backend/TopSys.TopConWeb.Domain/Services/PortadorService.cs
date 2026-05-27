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
    public class PortadorService : ServiceBase<Portador>, IPortadorService
    {
        private readonly IPortadorRepository _portadorRepository;

        public PortadorService(IPortadorRepository portadorRepository) : base(portadorRepository)
        {
            _portadorRepository = portadorRepository;
        }

        public IEnumerable<Portador> ListarVinculadosComContas()
        {
            return _portadorRepository.ListarVinculadosComContas();
        }
        
        public ICollection<Portador> Listar()
        {
            return _portadorRepository.ListarPortador();
        }

        public Portador ObterPorId(int id, bool tracking = false)
        {
            return _portadorRepository.ObterPorIdPortador(id, tracking);
        }

        public bool EstaEmUsoPortador(int id)
        {
            return _portadorRepository.EstaEmUsoPortador(id);
        }
    }
}

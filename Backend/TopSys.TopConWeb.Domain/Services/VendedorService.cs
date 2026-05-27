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
    public class VendedorService : ServiceBase<Vendedor>, IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository) : base(vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public ICollection<Vendedor> Listar()
        {
            return _vendedorRepository.Listar();
        }

        public Vendedor ObterPorId(int id, bool tracking = false)
        {
            return _vendedorRepository.ObterPorId(id, tracking);
        }

        public Vendedor ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _vendedorRepository.ObterPorExternalIdVendedor(externalId, tracking);
        }

        public bool EstaEmUsoVendedor(int id)
        {
            return _vendedorRepository.EstaEmUsoVendedor(id);
        }

        public int ObterProximoCodigo()
        {
            return _vendedorRepository.ObterProximoCodigo();
        }

        public bool ReEmUsoVendedor(int re, int id = 0)
        {
            return _vendedorRepository.ReEmUsoVendedor(re, id);
        }

        public void InativarVendedor(int id)
        {
            _vendedorRepository.InativarVendedor(id);
        }
    }
}

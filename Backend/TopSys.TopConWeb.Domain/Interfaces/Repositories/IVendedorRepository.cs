using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IVendedorRepository : IRepositoryBase<Vendedor>
    {
        ICollection<Vendedor> Listar();
        Vendedor ObterPorId(int id, bool tracking = false);
        Vendedor ObterPorExternalIdVendedor(string externalId, bool tracking = false);
        bool EstaEmUsoVendedor(int id);
        bool ReEmUsoVendedor(int re, int id = 0);
        int ObterProximoCodigo();
        void InativarVendedor(int id);
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CartaoBandeiraRepository : RepositoryBase<CartaoBandeira>, ICartaoBandeiraRepository
    {
        public CartaoBandeiraRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<CartaoBandeira> ListarTodosComAgregados()
        {
            return _context.CartaoBandeiras
                .Include(t => t.Interveniente)
                .Include(t => t.Portador)
                .Include(t => t.Portador.Conta)
                .Where(t => t.Ativo == "S")
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<CartaoBandeira> ListarTodosComIntegracao()
        {
            return _context.CartaoBandeiras
                .Include(t => t.Interveniente)
                .Include(t => t.Portador)
                .Include(t => t.Portador.Conta)
                .Where(t => t.TipoIntegracao != "")
                .AsNoTracking()
                .ToList();
        }
    }
}

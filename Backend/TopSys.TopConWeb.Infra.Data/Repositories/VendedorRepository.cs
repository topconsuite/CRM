using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class VendedorRepository : RepositoryBase<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Vendedor> Listar()
        {
            return _context
                .Vendedores
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public Vendedor ObterPorId(int id, bool tracking = false)
        {
            return _context
                .Vendedores
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Vendedor ObterPorExternalIdVendedor(string externalId, bool tracking = false)
        {
            return _context
                .Vendedores
                .Where(t => t.ExternalId == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoVendedor(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;

            sqlComando.Append($"SELECT COUNT(vendedor) FROM con_chtel WHERE vendedor={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(vendedor) FROM con_contrato WHERE vendedor={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(vendedor) FROM con_nf WHERE vendedor={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        public int ObterProximoCodigo()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT MAX(cod) + 1 FROM con_vendedor WHERE cod<>999");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }

        public bool ReEmUsoVendedor(int re, int id = 0)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(re) FROM con_vendedor WHERE re={re}");

            if (id != 0)
                sqlComando.Append($" AND cod<>{id}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        public void InativarVendedor(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_vendedor SET ativo='N' WHERE cod={id}");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }
    }
}

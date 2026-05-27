using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Dapper;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class PortadorRepository : RepositoryBase<Portador>, IPortadorRepository
    {
        public PortadorRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Portador> ListarVinculadosComContas()
        {
            return _context.Portadores
                .Include(t => t.Conta)
                .Where(t => t.ContaEmpresaCodigo != 0
                            && t.ContaCodigo != 0)
                .AsNoTracking()
                .ToList();
        }

        public ICollection<Portador> ListarPortador()
        {
            return _context
                .Portadores
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public Portador ObterPorIdPortador(int id, bool tracking = false)
        {
            return _context
                .Portadores
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoPortador(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;

            sqlComando.Append($"SELECT COUNT(bco_port) FROM fin_car WHERE bco_port ={id} or bco_port_orig ={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(Port) FROM fin_cap WHERE Port={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            
            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(portador) FROM con_bandeira WHERE portador={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            
            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(port_cobranca) FROM con_usina WHERE port_cobranca={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(port_cobranca) FROM ger_interv WHERE port_cobranca={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());


            return (result > 0);
        }
    }
}

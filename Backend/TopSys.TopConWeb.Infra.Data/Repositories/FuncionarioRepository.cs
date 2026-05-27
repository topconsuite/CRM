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
    public class FuncionarioRepository : RepositoryBase<Funcionario>,IFuncionarioRepository
    {
        public FuncionarioRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Funcionario> ListarAnalistas()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT func.cod {nameof(Funcionario.Codigo)}");
            sqlComando.Append($", func.nome  {nameof(Funcionario.Nome)}");
            sqlComando.Append(" FROM con_funcionario func");
            sqlComando.Append(" INNER JOIN view_dir_usuario vdirusuario");
            sqlComando.Append(" ON vdirusuario.usuario = func.usuario_sist");
            sqlComando.Append(" WHERE func.ativo = 'S'");
            sqlComando.Append(" AND vdirusuario.num_prog = 6118");
            sqlComando.Append(" AND vdirusuario.acesso = 'S'");
            sqlComando.Append(" AND vdirusuario.aplicativo = 'CON';");

            return _context.Database.Connection.Query<Funcionario>(sqlComando.ToString());

        }

        public int ObterProximoCodigo()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT MAX(cod) + 1 FROM con_funcionario");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }

        public bool ReEmUso(int re)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT COUNT(re) FROM con_funcionario WHERE re={re}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }
    }
}

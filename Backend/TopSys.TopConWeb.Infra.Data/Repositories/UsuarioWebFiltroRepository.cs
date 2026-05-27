using Dapper;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class UsuarioWebFiltroRepository : IUsuarioWebFiltroRepository
    {

        private readonly AppDataContext _context;

        public UsuarioWebFiltroRepository(AppDataContext context)
        {
            _context = context;
        }

        public UsuarioWebFiltro ObterPorId(string usuario, string aplicativo)
        {
            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"   `usuario` {nameof(UsuarioWebFiltro.Usuario)},");
            sql.AppendLine($"   `aplicativo` {nameof(UsuarioWebFiltro.Aplicativo)},");
            sql.AppendLine($"   `json` {nameof(UsuarioWebFiltro.Json)},");
            sql.AppendLine($"   `filter_string` {nameof(UsuarioWebFiltro.FilterString)}");
            sql.AppendLine($"FROM `topsys`.`usr_web_filtros`");
            sql.AppendLine($"WHERE");
            sql.AppendLine($"   `usuario` = @{nameof(usuario)}");
            sql.AppendLine($"   AND `aplicativo` = @{nameof(aplicativo)}");

            var result = _context.Database.Connection.QueryFirstOrDefault<UsuarioWebFiltro>(sql.ToString(), new { usuario, aplicativo });

            return result;


        }

        public void Salvar(UsuarioWebFiltro filtro) => Salvar(filtro.Usuario, filtro.Aplicativo, filtro.Json, filtro.FilterString);

        public void Salvar(string usuario, string aplicativo, string json, string filterString)
        {
            var sql = new StringBuilder();

            sql.AppendLine($"REPLACE INTO `topsys`.`usr_web_filtros` (`usuario`, `aplicativo`, `json`, `filter_string`) ");
            sql.AppendLine($"VALUES (");
            sql.AppendLine($"   @{nameof(usuario)}");
            sql.AppendLine($",   @{nameof(aplicativo)}");
            sql.AppendLine($",   @{nameof(json)}");
            sql.AppendLine($",   @{nameof(filterString)}");
            sql.AppendLine($")");

            _context.Database.Connection.Execute(sql.ToString(), new { usuario, aplicativo, json, filterString });

        }

    }
}

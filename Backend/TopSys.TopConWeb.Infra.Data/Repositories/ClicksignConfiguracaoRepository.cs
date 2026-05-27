using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ClicksignConfiguracaoRepository : RepositoryBase<ClicksignConfiguracao>, IClicksignConfiguracaoRepository
    {
        public ClicksignConfiguracaoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ClicksignConfiguracao ObterConfiguracaoPorUsina(int usinaId)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append($" cs.id {nameof(ClicksignConfiguracao.Id)},");
            sql.Append($" cs.email {nameof(ClicksignConfiguracao.Email)},");
            sql.Append($" cs.token {nameof(ClicksignConfiguracao.Token)},");
            sql.Append($" cs.base_url {nameof(ClicksignConfiguracao.BaseUrl)},");
            sql.Append($" cs.alias {nameof(ClicksignConfiguracao.Alias)},");
            sql.Append($" cs.sha256_secret {nameof(ClicksignConfiguracao.Sha256Secret)},");
            sql.Append($" cs.active {nameof(ClicksignConfiguracao.Ativo)}");
            sql.Append(" FROM topsys.configuracoes_clicksign cs");
            sql.Append(" INNER JOIN topsys.con_usina u ON u.clicksign_config_id = cs.id");
            sql.Append(" WHERE u.cod = @USINA_ID");
            sql.Append(" AND cs.active = 1");
            sql.Append(" LIMIT 1");

            return _context.Database.Connection
                .Query<ClicksignConfiguracao>(sql.ToString(), new { USINA_ID = usinaId })
                .FirstOrDefault();
        }

        public IEnumerable<Usina> ListarUsinasPorConfiguracao(int configuracaoId)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append($" u.cod {nameof(Usina.Codigo)},");
            sql.Append($" u.nome {nameof(Usina.Nome)},");
            sql.Append($" u.sigla {nameof(Usina.Sigla)}");
            sql.Append(" FROM topsys.con_usina u");
            sql.Append(" WHERE u.clicksign_config_id = @CONFIG_ID");
            sql.Append(" ORDER BY u.nome");

            return _context.Database.Connection
                .Query<Usina>(sql.ToString(), new { CONFIG_ID = configuracaoId });
        }

        public void VincularUsina(int configuracaoId, int usinaId)
        {
            var sql = "UPDATE topsys.con_usina SET clicksign_config_id = @CONFIG_ID WHERE cod = @USINA_ID";
            _context.Database.Connection.Execute(sql, new { CONFIG_ID = configuracaoId, USINA_ID = usinaId });
        }

        public void DesvincularUsina(int usinaId)
        {
            var sql = "UPDATE topsys.con_usina SET clicksign_config_id = NULL WHERE cod = @USINA_ID";
            _context.Database.Connection.Execute(sql, new { USINA_ID = usinaId });
        }
    }
}

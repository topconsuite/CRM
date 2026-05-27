using Dapper;
using System.Text;
using TopSys.TopConWeb.Domain.Entities.CartaoPagamentoIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.CartaoPagamentoIntegracao;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories.CartaoPagamentoIntegracao
{
    public class CieloLioDadosRepository : ICieloLioDadosRepository
    {
        private readonly AppDataContext _context;

        public CieloLioDadosRepository(AppDataContext context)
        {
            _context = context;
        }

        public CieloLioDados ObterPorCartaoBandeiraCodigo(int cartaoBandeiraCodigo)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT b.cod_bandeira CartaoBandeiraCodigo");
            sql.Append(", i.parametro_valor CieloAccessToken");
            sql.Append(", i2.parametro_valor CieloClientId");
            sql.Append(", i3.parametro_valor CieloMerchantId");
            sql.Append(" FROM topsys.con_bandeira b");
            sql.Append(" LEFT JOIN topsys.con_bandeira_integracoes i");
            sql.Append(" ON b.cod_bandeira=i.cod_bandeira AND i.parametro_nome='cielo_access_token'");
            sql.Append(" LEFT JOIN topsys.con_bandeira_integracoes i2");
            sql.Append(" ON b.cod_bandeira=i2.cod_bandeira AND i2.parametro_nome='cielo_client_id'");
            sql.Append(" LEFT JOIN topsys.con_bandeira_integracoes i3");
            sql.Append(" ON b.cod_bandeira=i3.cod_bandeira AND i3.parametro_nome='cielo_merchant_id'");
            sql.Append(" WHERE b.tipo_integracao='cielo_lio'");
            sql.Append($" AND b.cod_bandeira={cartaoBandeiraCodigo}");

            return _context.Database.Connection.QueryFirstOrDefault<CieloLioDados>(sql.ToString());
        }
    }
}

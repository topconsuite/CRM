using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TipoDeCobrancaRepository : RepositoryBase<TipoDeCobranca>, ITipoDeCobrancaRepository
    {
        public TipoDeCobrancaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<TipoDeCobranca> ListarTipoDeCobranca()
        {
            return _context
                .TipoDeCobranca
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public TipoDeCobranca ObterPorIdTipoDeCobranca(int id, bool tracking = false)
        {
            return _context
                .TipoDeCobranca
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public TipoDeCobranca ObterTipoDeCobrancaComDescricao(int codigoTipoCobranca)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT ctc.tipo_cobranca Codigo");
            sqlComando.Append(", ctc.forma Forma");
            sqlComando.Append(", ctc.portador Portador");
            sqlComando.Append(", ctc.situacao Situacao");
            sqlComando.Append(", gg.descr Descricao");
            sqlComando.Append(" FROM con_tipo_cobranca ctc");
            sqlComando.Append(" LEFT JOIN ger_geral gg");
            sqlComando.Append(" ON ctc.tipo_cobranca = gg.cod");
            sqlComando.Append($" WHERE ctc.tipo_cobranca = {codigoTipoCobranca}");
            sqlComando.Append(" LIMIT 1");

            return _context.Database.Connection.Query<TipoDeCobranca>(sqlComando.ToString()).Single();
        }
    }
}
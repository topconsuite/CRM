using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class AprovacaoComercialUsinaRepository : RepositoryBase<AprovacaoComercialUsina>, IAprovacaoComercialUsinaRepository
    {

        public AprovacaoComercialUsinaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public AprovacaoComercialUsina ObterPorId(Guid id, bool tracking = false)
        {

            var result = _context.AprovacaoComercialUsinas
                .Include(x => x.Usina)
                .Include(x => x.Hierarquias)
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;

        }

        public AprovacaoComercialUsina ObterPorUsina(int usinaId, bool tracking = false)
        {

            var result = _context.AprovacaoComercialUsinas
                .Include(x => x.Usina)
                .Include(x => x.Hierarquias)
                .Where(x => x.UsinaId == usinaId)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;

        }

        public PagedList<AprovacaoComercialUsina> Listar(int pagina, int porPagina, Expression<Func<AprovacaoComercialUsina, bool>> filter)
        {

            var pagedList = _context.AprovacaoComercialUsinas
                .Include(x => x.Usina)
                .Include(x => x.Hierarquias)
                .AsNoTracking()
                .OrderBy(x => x.UsinaId)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public bool UtilizaAprovacaoComercialPorAlcada(int usinaId)
        {

            var result = _context.AprovacaoComercialUsinas.Where(x => x.UsinaId == usinaId).FirstOrDefault();

            if (result == null)
                return false;

            return result.Ativo;

        }

        public void AdicionarLog(AprovacaoComercialLog log)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT valor FROM ger_parametro");
            sql.AppendLine($"WHERE grupo = 'FeatureFlags'");
            sql.AppendLine($"  AND chave = 'HabilitaLogAprovacaoComercial';");

            var habilitadoLog = _context.Database.Connection.QueryFirstOrDefault<string>(sql.ToString());

            if (string.IsNullOrEmpty(habilitadoLog) || !habilitadoLog.Equals("1"))
                return;

            sql.AppendLine("INSERT INTO con_aprovacao_comercial_log SET");
            sql.AppendLine($"    id = @{nameof(AprovacaoComercialLog.Id)},");
            sql.AppendLine($"    obra_usina = @{nameof(AprovacaoComercialLog.ObraUsina)},");
            sql.AppendLine($"    obra_numero = @{nameof(AprovacaoComercialLog.ObraNumero)},");
            sql.AppendLine($"    tabela = @{nameof(AprovacaoComercialLog.Tabela)},");
            sql.AppendLine($"    data = @{nameof(AprovacaoComercialLog.Data)},");
            sql.AppendLine($"    source = @{nameof(AprovacaoComercialLog.Source)},");
            sql.AppendLine($"    script = @{nameof(AprovacaoComercialLog.Script)},");
            sql.AppendLine($"    payload = @{nameof(AprovacaoComercialLog.Payload)};");

            _context.Database.Connection.Execute(sql.ToString(), log);


        }
        
    }
}

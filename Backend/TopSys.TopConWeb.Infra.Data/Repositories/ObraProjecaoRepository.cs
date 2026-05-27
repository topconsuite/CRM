using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ObraProjecaoRepository : RepositoryBase<ObraProjecao>, IObraProjecaoRepository
    {
        private IDatabaseRepository _databaseRepository;
        private readonly IdentityHelperService _identityHelperService;

        public ObraProjecaoRepository(AppDataContext context, IDatabaseRepository databaseRepository, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
        }

        public IEnumerable<ObraProjecao> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false)
        {
            return _context
                .ObraProjecao
                .Where(x => x.Usina == obraUsina && x.NoObra == obraNumero)
                .Tracking(tracking)
                .OrderBy(x => x.Periodo);

        }

        public ObraProjecao ObterPorObraVolumePeriodo(int obraUsina, int obraNumero, int anoChamada, int noChamada, float volumeAnterior, DateTime periodoAnterior, bool tracking = false)
        {
            return _context
                .ObraProjecao
                .Where(x => x.Usina == obraUsina && x.NoObra == obraNumero && x.NoChamada == noChamada 
                && x.AnoChamada == anoChamada && x.Volume == volumeAnterior )
                .Tracking(tracking)
                .FirstOrDefault();
        }
        public void AtualizarProjecao(ObraProjecao obraProjecao, float volumeAnterior, DateTime periodoAnterior)
        {
            obraProjecao.Periodo = new DateTime(obraProjecao.Periodo.Year, obraProjecao.Periodo.Month, 1);

            var saldoAnterior = GetSaldoProjecaoAnterior(obraProjecao.Usina, obraProjecao.NoObra, obraProjecao.AnoChamada, obraProjecao.NoChamada, obraProjecao.Periodo);
            obraProjecao.Saldo = saldoAnterior - obraProjecao.Volume;

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_obras_projecao ");
            sqlComando.Append($"SET periodo='{obraProjecao.Periodo.ToString("yyyy-MM-dd")}', ");
            sqlComando.Append($"volume_m3=@{nameof(obraProjecao.Volume)} ");
            sqlComando.Append($",saldo_m3=@{nameof(obraProjecao.Saldo)} ");
            sqlComando.Append($" WHERE usina={obraProjecao.Usina} ");
            sqlComando.Append($"AND no_obra={obraProjecao.NoObra} ");
            sqlComando.Append($"AND ano_chamada={obraProjecao.AnoChamada} ");
            sqlComando.Append($"AND no_chamada={obraProjecao.NoChamada} ");
            sqlComando.Append($"AND periodo='{periodoAnterior.ToString("yyyy-MM-dd")}' ");
            sqlComando.Append($"AND volume_m3=@{nameof(volumeAnterior)} ");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
               obraProjecao.Volume,
                obraProjecao.Saldo,
                volumeAnterior
            });
        }

        public float? ObterSaldoProjecaoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT sum(volume_m3) from con_obras_projecao");
            sqlComando.AppendLine(" WHERE");
            sqlComando.AppendLine($" usina = @{nameof(usina)}");
            sqlComando.AppendLine($" AND no_obra = @{nameof(noObra)}");
            sqlComando.AppendLine($" AND ano_chamada = @{nameof(anoChamada)}");
            sqlComando.AppendLine($" AND no_chamada = @{nameof(noChamada)}");

            var saldo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usina, noObra, anoChamada, noChamada });
            sqlComando.Clear();

            return saldo;

        }

        public float? ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada)
        {
            DateTime dataAtual = DateTime.Now;


            DateTime periodo = new DateTime(dataAtual.Year, dataAtual.Month, 1);

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT sum(volume_m3) from con_obras_projecao");
            sqlComando.AppendLine(" WHERE");
            sqlComando.AppendLine($" usina = @{nameof(usina)}");
            sqlComando.AppendLine($" AND no_obra = @{nameof(noObra)}");
            sqlComando.AppendLine($" AND ano_chamada = @{nameof(anoChamada)}");
            sqlComando.AppendLine($" AND no_chamada = @{nameof(noChamada)}");
            sqlComando.AppendLine($" AND periodo >= @{nameof(periodo)}");

            var saldo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usina, noObra, anoChamada, noChamada, periodo });
            sqlComando.Clear();

            return saldo;

        }

        public DateTime? GetProximoPeriodoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT coalesce(DATE_ADD(MAX(periodo), INTERVAL 1 MONTH),DATE_FORMAT( DATE_ADD(LAST_DAY(CURDATE()), INTERVAL 1 DAY),'%Y-%m-%d')) from con_obras_projecao");
            sqlComando.AppendLine(" WHERE");
            sqlComando.AppendLine($" usina = @{nameof(usina)}");
            sqlComando.AppendLine($" AND no_obra = @{nameof(noObra)}");
            sqlComando.AppendLine($" AND ano_chamada = @{nameof(anoChamada)}");
            sqlComando.AppendLine($" AND no_chamada = @{nameof(noChamada)}");

            var proximoPeriodo = _context.Database.Connection.QueryFirstOrDefault<DateTime?>(sqlComando.ToString(), new { usina, noObra, anoChamada, noChamada });
            sqlComando.Clear();

            return proximoPeriodo;

        }

        public float GetSaldoProjecaoAnterior(int usina, int noObra, int? anoChamada, int? noChamada, DateTime periodo)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT saldo_m3 from con_obras_projecao");
            sqlComando.AppendLine(" WHERE");
            sqlComando.AppendLine($" usina = @{nameof(usina)}");
            sqlComando.AppendLine($" AND no_obra = @{nameof(noObra)}");
            sqlComando.AppendLine($" AND ano_chamada = @{nameof(anoChamada)}");
            sqlComando.AppendLine($" AND no_chamada = @{nameof(noChamada)}");
            sqlComando.AppendLine($" AND periodo < @{nameof(periodo)}");
            sqlComando.AppendLine(" ORDER BY periodo DESC LIMIT 1");

            var saldo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usina, noObra, anoChamada, noChamada, periodo }).GetValueOrDefault(0f);
            sqlComando.Clear();

            return saldo;

        }
    }
}

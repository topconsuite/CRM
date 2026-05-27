using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class UsinaRepository : RepositoryBase<Usina>, IUsinaRepository
    {
        public UsinaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Usina> ListarPorEmpresa(int empresa)
        {
            StringBuilder sqlComando = new StringBuilder();
            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append($" cod {nameof(Usina.Codigo)},");
            sqlComando.Append($" nome {nameof(Usina.Nome)},");
            sqlComando.Append($" sigla {nameof(Usina.Sigla)},");
            sqlComando.Append($" emp_filial {nameof(Usina.FilialCodigo)},");
            sqlComando.Append($" intervalo_carga {nameof(Usina.IntervaloEmMinutosEntreCargas)},");
            sqlComando.Append($" gera_cred_cli {nameof(Usina.GeraCreditoClientePagamentoAntecipadoSimNao)},");
            sqlComando.Append($" min_pesagem {nameof(Usina.PrazoPesagem)},");
            sqlComando.Append($" pct_retorn_obra {nameof(Usina.PorcentagemRetornoObra)},");
            sqlComando.Append($" temp_bt_na_obra {nameof(Usina.TempoBtNaObra)},");
            sqlComando.Append($" temp_ate_a_obra {nameof(Usina.TempoBtAteAObra)},");
            sqlComando.Append($" external_id {nameof(Usina.ExternalId)}");
            sqlComando.Append(" FROM topsys.con_usina");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" (emp_filial DIV 1000)=@EMPRESA");
            sqlComando.Append(" and ativo = 'S'");

            var parametros = new { EMPRESA = empresa };

            var result = _context.Database.Connection.Query<Usina>(sqlComando.ToString(), parametros);

            return result;
        }

        public float? ObterValorAdicionalM3PorUsinaKm(int idUsina, int km)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" atende, valor");
            sqlComando.Append(" FROM topsys.con_tab_venda_km");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina_entrega=@ID_USINA");
            sqlComando.Append(" AND dist_km_inicial<=@KM");
            sqlComando.Append(" AND dist_km_final>=@KM");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USINA = idUsina, KM = km })
                .Select(row => new { row.atende, row.valor })
                .FirstOrDefault();

            if (result == null) return 0.0f;

            if (result.atende == "N") return null;

            return result.valor;
        }

        public bool UsinaAtendeKm(int idUsina, int km)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" atende");
            sqlComando.Append(" FROM topsys.con_tab_venda_km");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina_entrega=@ID_USINA");
            sqlComando.Append(" AND dist_km_inicial<=@KM");
            sqlComando.Append(" AND dist_km_final>=@KM");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USINA = idUsina, KM = km })
                .Select(row => new { row.atende })
                .FirstOrDefault();

            if (result == null) return true;

            return result.atende == "S";
        }

        public IEnumerable<Usina> ListarUsinasPermitidasUsuario(string idUsuario)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append($" usr.usina {nameof(Usina.Codigo)},");
            sqlComando.Append($" u.Nome {nameof(Usina.Nome)},");
            sqlComando.Append($" u.Sigla {nameof(Usina.Sigla)},");
            sqlComando.Append($" u.emp_filial {nameof(Usina.FilialCodigo)}");
            sqlComando.Append(" FROM topsys.con_usr_usina usr");
            sqlComando.Append(" INNER JOIN con_usina u");
            sqlComando.Append(" ON usr.usina = u.cod");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usr.usuario=@ID_USUARIO");
            sqlComando.Append(" and u.ativo = 'S'");

            var result = _context.Database.Connection.Query<Usina>(sqlComando.ToString(), new { ID_USUARIO = idUsuario });

            return result;
        }

        public IEnumerable<Usina> ListarUsinasAtivas()
        {
            StringBuilder sqlComando = new StringBuilder();
            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append($" cod {nameof(Usina.Codigo)},");
            sqlComando.Append($" nome {nameof(Usina.Nome)},");
            sqlComando.Append($" sigla {nameof(Usina.Sigla)},");
            sqlComando.Append($" emp_filial {nameof(Usina.FilialCodigo)},");
            sqlComando.Append($" intervalo_carga {nameof(Usina.IntervaloEmMinutosEntreCargas)},");
            sqlComando.Append($" gera_cred_cli {nameof(Usina.GeraCreditoClientePagamentoAntecipadoSimNao)},");
            sqlComando.Append($" min_pesagem {nameof(Usina.PrazoPesagem)},");
            sqlComando.Append($" pct_retorn_obra {nameof(Usina.PorcentagemRetornoObra)},");
            sqlComando.Append($" temp_bt_na_obra {nameof(Usina.TempoBtNaObra)},");
            sqlComando.Append($" temp_ate_a_obra {nameof(Usina.TempoBtAteAObra)},");
            sqlComando.Append($" external_id {nameof(Usina.ExternalId)}");
            sqlComando.Append(" FROM topsys.con_usina");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" ativo = 'S'");

            return _context.Database.Connection.Query<Usina>(sqlComando.ToString());
        }
    }
}

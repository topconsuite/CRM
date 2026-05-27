using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TracoCustoRepository : RepositoryBase<TracoCusto>, ITracoCustoRepository
    {
        public TracoCustoRepository(AppDataContext context) : base(context)
        {
        }

        public TracoCusto ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT custo.usina {nameof(TracoCusto.UsinaCodigo)}, custo.sigla_compos {nameof(TracoCusto.TracoEspecificacao)}");
            sql.Append($", custo.dt_inic_valid {nameof(TracoCusto.DataInicioVigencia)}, custo.uso {nameof(TracoCusto.UsoCodigo)}");
            sql.Append($", custo.tp_resist {nameof(TracoCusto.ResistenciaTipoCodigo)}, custo.fck {nameof(TracoCusto.Mpa)}");
            sql.Append($", custo.consumo {nameof(TracoCusto.Consumo)}, custo.pedra {nameof(TracoCusto.PedraCodigo)}");
            sql.Append($", custo.slump {nameof(TracoCusto.SlumpCodigo)}, custo.custo_p {nameof(TracoCusto.CustoPuro)}");
            sql.Append($", custo.custo_a {nameof(TracoCusto.CustoAjustado)}, custo.dt_calc_custo {nameof(TracoCusto.DataCalculoCusto)}");
            sql.Append($", custo.custo_recal {nameof(TracoCusto.CustoRecalculado)}, custo.dt_recal {nameof(TracoCusto.DataRecalculoCusto)}");
            sql.Append($", custo.pct_variacao {nameof(TracoCusto.PercentalVariacao)}, custo.ativo {nameof(TracoCusto.Ativo)}");
            sql.Append($", custo.id_cadast {nameof(TracoCusto.IdCadastro)}, custo.id_atual {nameof(TracoCusto.IdAtualizacao)}");
            sql.Append($", custo.vlr_servico {nameof(TracoCusto.ValorServico)}, custo.vlr_custo_outro {nameof(TracoCusto.ValorOutrosCustos)}");
            sql.Append($", custo.markup {nameof(TracoCusto.ValorMarkup)}");
            sql.Append($" FROM con_custo_concreto custo");
            sql.Append($" LEFT JOIN con_def_familiar def");
            sql.Append($" ON def.especificacao=custo.sigla_compos");
            sql.Append($" AND def.usina = custo.usina");
            sql.Append($" AND def.uso = custo.uso");
            sql.Append($" AND def.pedra = custo.pedra");
            sql.Append($" AND def.slump = custo.slump");
            sql.Append($" WHERE custo.usina=@idUsina");
            sql.Append($" AND custo.dt_inic_valid<=@dataBase");
            sql.Append($" AND custo.uso=@idUso");
            sql.Append($" AND custo.tp_resist=@idResistenciaTipo");
            sql.Append($" AND custo.fck=@mpa");
            sql.Append($" AND custo.consumo=@consumo");
            sql.Append($" AND custo.pedra=@idPedra");
            sql.Append($" AND custo.slump=@idSlump");
            sql.Append($" AND custo.sigla_compos <> ''");
            sql.Append($" AND def.status=7101");
            sql.Append($" ORDER BY custo.dt_inic_valid DESC");    

            return _context.Database.Connection.QueryFirstOrDefault<TracoCusto>(sql.ToString(), new { idUsina, dataBase, idUso, idResistenciaTipo, mpa, consumo, idPedra, idSlump });
        }

        public TracoCusto ObterUltimoTracoPrecoPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, DateTime dataBase)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT custo.usina {nameof(TracoCusto.UsinaCodigo)}, custo.sigla_compos {nameof(TracoCusto.TracoEspecificacao)}");
            sql.Append($", custo.dt_inic_valid {nameof(TracoCusto.DataInicioVigencia)}, custo.uso {nameof(TracoCusto.UsoCodigo)}");
            sql.Append($", custo.tp_resist {nameof(TracoCusto.ResistenciaTipoCodigo)}, custo.fck {nameof(TracoCusto.Mpa)}");
            sql.Append($", custo.consumo {nameof(TracoCusto.Consumo)}, custo.pedra {nameof(TracoCusto.PedraCodigo)}");
            sql.Append($", custo.slump {nameof(TracoCusto.SlumpCodigo)}, custo.custo_p {nameof(TracoCusto.CustoPuro)}");
            sql.Append($", custo.custo_a {nameof(TracoCusto.CustoAjustado)}, custo.dt_calc_custo {nameof(TracoCusto.DataCalculoCusto)}");
            sql.Append($", custo.custo_recal {nameof(TracoCusto.CustoRecalculado)}, custo.dt_recal {nameof(TracoCusto.DataRecalculoCusto)}");
            sql.Append($", custo.pct_variacao {nameof(TracoCusto.PercentalVariacao)}, custo.ativo {nameof(TracoCusto.Ativo)}");
            sql.Append($", custo.id_cadast {nameof(TracoCusto.IdCadastro)}, custo.id_atual {nameof(TracoCusto.IdAtualizacao)}");
            sql.Append($", custo.vlr_servico {nameof(TracoCusto.ValorServico)}, custo.vlr_custo_outro {nameof(TracoCusto.ValorOutrosCustos)}");
            sql.Append($", custo.markup {nameof(TracoCusto.ValorMarkup)}");
            sql.Append($" FROM con_custo_concreto custo");
            sql.Append($" LEFT JOIN con_def_familiar def");
            sql.Append($" ON def.especificacao=custo.sigla_compos");
            sql.Append($" AND def.usina = custo.usina");
            sql.Append($" AND def.uso = custo.uso");
            sql.Append($" AND def.pedra = custo.pedra");
            sql.Append($" AND def.slump = custo.slump");
            sql.Append($" INNER JOIN con_tab_preco tp");
            sql.Append($" ON tp.usina_base = custo.usina AND tp.uso = custo.uso");
            sql.Append($" AND tp.slump = custo.slump AND tp.fck = custo.fck");
            sql.Append($" AND tp.pedra = custo.pedra AND tp.consumo = custo.consumo");
            sql.Append($" AND tp.espec_familia=custo.sigla_compos");
            sql.Append($" WHERE custo.usina=@idUsina");
            sql.Append($" AND custo.dt_inic_valid<=@dataBase");
            sql.Append($" AND custo.uso=@idUso");
            sql.Append($" AND custo.tp_resist=@idResistenciaTipo");
            sql.Append($" AND custo.fck=@mpa");
            sql.Append($" AND custo.consumo=@consumo");
            sql.Append($" AND custo.pedra=@idPedra");
            sql.Append($" AND custo.slump=@idSlump");
            sql.Append($" AND custo.sigla_compos <> ''");
            sql.Append($" ORDER BY custo.dt_inic_valid DESC");

            return _context.Database.Connection.QueryFirstOrDefault<TracoCusto>(sql.ToString(), new { idUsina, dataBase, idUso, idResistenciaTipo, mpa, consumo, idPedra, idSlump });
        }
    }
}

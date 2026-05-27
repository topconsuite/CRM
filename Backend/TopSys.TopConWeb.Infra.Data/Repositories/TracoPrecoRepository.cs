using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Infra.Data.Helpers;
using LinqKit;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TracoPrecoRepository : RepositoryBase<TracoPreco>, ITracoPrecoRepository
    {
        private readonly int VENDEDOR_PADRAO = 999;
        private readonly string[] STATUS_TRACO_ATIVO =
            {
                "7101", // Homologado em Uso
                "7103", // Homologado Futuro
                "7105"  // Custo Virtual
            };

        public TracoPrecoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public DateTime ObterDataVigenciaPorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            var tracoPreco = _context.TracoPrecos
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.DataInicioVigencia <= dataBase
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.DataInicioVigencia)
                .FirstOrDefault();

            if (tracoPreco == null)
                return new DateTime(2018, 1, 1);
            
            return tracoPreco.DataInicioVigencia;
        }

        public DateTime ObterDataVigenciaPorDataBase(DateTime dataBase)
        {
            return _context.TracoPrecos
                .Where(tp => tp.DataInicioVigencia <= dataBase
                             && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.DataInicioVigencia)
                .FirstOrDefault()
                .DataInicioVigencia;
        }

        public int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            var result = _context.TracoPrecos
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.DataInicioVigencia <= dataBase
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.DataInicioVigencia)
                .FirstOrDefault();

            return result == null ? 1 : result.NumeroTabela;

        }

        public int ObterNumeroTabelaVigentePorDataBase(DateTime dataBase)
        {
            var result = _context.TracoPrecos
                .Where(tp => tp.DataInicioVigencia <= dataBase
                             && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.DataInicioVigencia)
                .FirstOrDefault();

            return result == null ? 1 : result.NumeroTabela;
        }

        public IEnumerable<TracoPreco> ListarPorDataUsina(DateTime data, int idUsina)
        {
            var tracoPrecos = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO
                            && tp.TracoEspecificacao != ""
                            && tp.DataInicioVigencia == data)
                .AsNoTracking()
                .ToList();

            return tracoPrecos;
        }

        public PagedList<TracoPreco> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, IEnumerable<string> tracosAtivos, Expression<Func<TracoPreco, bool>> filter)
        {
            var pagedList = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                             && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO 
                             && tp.DataInicioVigencia == data
                             && tp.TracoEspecificacao != ""
                             && tracosAtivos.Contains(tp.TracoEspecificacao))
                .Where(filter)
                .Where(tp => segmentacao == -1 || tp.Uso.Segmentacao == segmentacao)
                .OrderByDescending(tp => tp.NumeroTabela)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public PagedList<TracoPreco> ListarPorDataPagina(DateTime data, int pagina, int porPagina, Expression<Func<TracoPreco, bool>> filter)
        {
            StringBuilder sqlComando = new StringBuilder();
            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append($" cod {nameof(Usina.Codigo)}");
            sqlComando.Append(" FROM topsys.con_usina");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" ativo = 'N'");

            var usinasDesativadas = _context.Database.Connection.Query<Usina>(sqlComando.ToString());

            var usinasDesativadasCod = new List<int>();

            foreach (var usina in usinasDesativadas)
            {
                usinasDesativadasCod.Add(usina.Codigo);
            }

            var pagedList = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO
                             && tp.DataInicioVigencia == data
                             && tp.TracoEspecificacao != "")
                .Where(filter)
                .Where(tp => !usinasDesativadasCod.Contains(tp.UsinaBaseCodigo))
                .OrderByDescending(tp => tp.NumeroTabela)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public PagedList<TracoPreco> ListarTodosPorPagina(int pagina, int porPagina, int? segmentacao, IEnumerable<string> tracosAtivos, Expression<Func<TracoPreco, bool>> filter)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Clear();
            sqlComando.Append($"SELECT tab.usina_base {nameof(TracoPreco.UsinaBaseCodigo)}");
            sqlComando.Append($", MAX(tab.num_tab) {nameof(TracoPreco.NumeroTabela)} FROM con_tab_preco tab");
            sqlComando.Append($" INNER JOIN con_usina usina");
            sqlComando.Append($" ON usina.cod=tab.usina_base");
            sqlComando.Append($" WHERE usina.ativo<>'N'");
            sqlComando.Append($" GROUP BY usina_base");

            var resultado = _context.Database.Connection.Query<TracoPreco>(sqlComando.ToString());

            Expression<Func<TracoPreco, bool>> filterVigencias = null;
            var controle = 0;
            foreach (var item in resultado)
            {
                controle += 1;
                if (controle == 1)
                {
                    filterVigencias = ((t => t.UsinaBaseCodigo == item.UsinaBaseCodigo && t.NumeroTabela == item.NumeroTabela));
                }
                else
                {
                    filterVigencias = filterVigencias.Or((t => t.UsinaBaseCodigo == item.UsinaBaseCodigo && t.NumeroTabela == item.NumeroTabela));
                }
            }

            var pagedList = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO
                             && tp.TracoEspecificacao != ""
                             && tracosAtivos.Contains(tp.TracoEspecificacao))
                .Where(filter.And(filterVigencias))
                .Where(tp => segmentacao == -1 || tp.Uso.Segmentacao == segmentacao) 
                .OrderByDescending(tp => tp.NumeroTabela)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public int ObterNumeroTabelaVigentePorUsina(int idUsina)
        {
            var result = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.NumeroTabela)
                .FirstOrDefault();

            return result == null ? 1 : result.NumeroTabela;
        }

        public DateTime ObterDataTabelaVigentePorUsina(int idUsina)
        {
            return _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO)
                .AsNoTracking()
                .OrderByDescending(tp => tp.NumeroTabela)
                .FirstOrDefault()
                .DataInicioVigencia;
        }

        public TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, bool tracking = false)
        {
            var tracoPrecoResult = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO
                            && tp.NumeroTabela == numeroTabela
                            && tp.UsoCodigo == idUso
                            && tp.PedraCodigo == idPedra
                            && tp.SlumpCodigo == idSlump
                            && tp.ResistenciaTipoCodigo == idResistenciaTipo
                            && tp.Mpa == mpa
                            && tp.Consumo == consumo)
                .OrderByDescending(tp => tp.DataInicioVigencia)
                .Tracking(tracking)
                .FirstOrDefault();

            return tracoPrecoResult;
        }

        public IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProdutoPorNumeroTabelaUsina(int numeroTabela, int idUsina, int idSegmentacao)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT t.numeracao_produto {nameof(TracoPrecoNumeracaoProduto.Numeracao)}, u.descr {nameof(TracoPrecoNumeracaoProduto.UsoDescricao)}");
            sql.Append($", f.status {nameof(TracoPrecoNumeracaoProduto.Status)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN con_uso u");
            sql.Append(" ON t.uso=u.cod AND u.generico='S' AND u.ativo = 'S'");
            sql.Append(" INNER JOIN fis_mercadoria m ON t.numeracao_produto=m.numeracao_produto");
            sql.Append(" WHERE t.usina_base=@idUsina AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" AND m.inativo='N'");
            sql.Append(" AND NOT ISNULL(t.numeracao_produto)");

            sql.Append(" AND u.id_segmentacao IN (" + (idSegmentacao == (int)ESegmentacaoCodigo.Concreto || idSegmentacao == (int)ESegmentacaoCodigo.ConcretoExpress ? "1,2" : "@idSegmentacao") + ")");

            sql.Append(" GROUP BY t.numeracao_produto;");

            var result = _context.Database.Connection.Query<TracoPrecoNumeracaoProduto>(sql.ToString(), new { idUsina, VENDEDOR_PADRAO, numeroTabela, idSegmentacao });

            return result;
        }

        public IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProduto()
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT t.numeracao_produto {nameof(TracoPrecoNumeracaoProduto.Numeracao)}, u.descr {nameof(TracoPrecoNumeracaoProduto.UsoDescricao)}");
            sql.Append($", t.usina_base {nameof(TracoPrecoNumeracaoProduto.UsinaBase)}, u.id_segmentacao {nameof(TracoPrecoNumeracaoProduto.IdSegmentacao)}");
            sql.Append($", f.status {nameof(TracoPrecoNumeracaoProduto.Status)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN con_uso u");
            sql.Append(" ON t.uso=u.cod AND u.generico='S' AND u.ativo = 'S'");
            sql.Append(" WHERE t.vend_repres=@VENDEDOR_PADRAO");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" AND NOT ISNULL(t.numeracao_produto)");
            sql.Append(" GROUP BY t.numeracao_produto, t.usina_base;");

            var result = _context.Database.Connection.Query<TracoPrecoNumeracaoProduto>(sql.ToString(), new { VENDEDOR_PADRAO });

            return result;
        }

        public int ObterStatusPorTracoPreco(TracoPreco tracoPreco)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT f.status StatusTraco FROM con_def_familiar f WHERE");
            sql.AppendLine($"    f.usina = @{nameof(tracoPreco.UsinaBaseCodigo)}");
            sql.AppendLine($"AND f.uso = @{nameof(tracoPreco.UsoCodigo)}");
            sql.AppendLine($"AND f.pedra = @{nameof(tracoPreco.PedraCodigo)}");
            sql.AppendLine($"AND f.slump = @{nameof(tracoPreco.SlumpCodigo)}");
            sql.AppendLine($"AND f.especificacao = @{nameof(tracoPreco.TracoEspecificacao)}");
            sql.AppendLine($"AND f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)})");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sql.ToString(), tracoPreco);

            return result;

        }

        public TracoPreco ObterPorNumeracaoProduto(int numeroTabela, int idUsina, int numeracaoProduto)
        {
            var tracoPrecoResult = _context.TracoPrecos
                .Include(tp => tp.UsinaBase)
                .Include(tp => tp.VendedorRepresentante)
                .Include(tp => tp.ResistenciaTipo)
                .Include(tp => tp.Uso)
                .Include(tp => tp.Pedra)
                .Include(tp => tp.Slump)
                .Include(tp => tp.UsinaReferencia)
                .Where(tp => tp.UsinaBaseCodigo == idUsina
                            && tp.VendedorRepresentanteCodigo == VENDEDOR_PADRAO
                            && tp.NumeroTabela == numeroTabela
                            && tp.NumeracaoProduto == numeracaoProduto)
                .FirstOrDefault();

            return tracoPrecoResult;
        }

        public IEnumerable<Uso> ListarUsosPorNumeroTabelaUsina(int numeroTabela, int idUsina, int idSegmentacao)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT u.cod {nameof(Uso.Codigo)}, u.descr {nameof(Uso.Descricao)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN con_uso u ON t.uso=u.cod AND u.generico='S' AND u.ativo='S'");
            sql.Append(" WHERE t.usina_base=@idUsina AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");

            sql.Append(" AND u.id_segmentacao IN (" + (idSegmentacao == (int)ESegmentacaoCodigo.Concreto || idSegmentacao == (int)ESegmentacaoCodigo.ConcretoExpress ? "1,2" : "@idSegmentacao") + ")");

            sql.Append(" GROUP BY u.cod;");

            var result = _context.Database.Connection.Query<Uso>(sql.ToString(), new { idUsina, VENDEDOR_PADRAO, numeroTabela, idSegmentacao });

            return result;
        }

        public IEnumerable<Pedra> ListarPedrasPorNumeroTabelaUsinaUso(int numeroTabela, int idUsina, int idUso)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT p.cod {nameof(Pedra.Codigo)}, p.descr {nameof(Pedra.Descricao)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN con_pedra p ON t.pedra=p.cod");
            sql.Append(" WHERE t.usina_base=@idUsina");
            sql.Append(" AND t.uso=@idUso");
            sql.Append(" AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" GROUP BY p.cod;");

            var result = _context.Database.Connection.Query<Pedra>(sql.ToString(), new { idUsina, idUso, VENDEDOR_PADRAO, numeroTabela });

            return result;
        }

        public IEnumerable<SlumpReal> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int numeroTabela, int idUsina, int idUso, int idPedra)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT s.cod {nameof(SlumpReal.Codigo)}, s.amplitude_de {nameof(SlumpReal.AmplitudeDe)}, s.variavao {nameof(SlumpReal.Variacao)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN view_slump_real s ON t.slump=s.cod");
            sql.Append(" WHERE t.usina_base=@idUsina");
            sql.Append(" AND t.uso=@idUso");
            sql.Append(" AND t.pedra=@idPedra");
            sql.Append(" AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" GROUP BY s.cod;");

            var result = _context.Database.Connection.Query<SlumpReal>(sql.ToString(), new { idUsina, idUso, idPedra, VENDEDOR_PADRAO, numeroTabela });

            return result;
        }

        public IEnumerable<Slump> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int numeroTabela, int idUsina, int idUso, int idPedra)
        {
            var slumpsReais = ListarSlumpsPorNumeroTabelaUsinaUsoPedra(numeroTabela, idUsina, idUso, idPedra).Select(s => s?.Codigo).ToArray();

            return _context.Slumps
                .Where(t => slumpsReais.Contains(t.AmplitudeDe))
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT tr.cod {nameof(ResistenciaTipo.Codigo)}, tr.descr {nameof(ResistenciaTipo.Descricao)}");
            sql.Append($", tr.abrev {nameof(ResistenciaTipo.Abreviatura)}, tr.mpa_cons {nameof(ResistenciaTipo.Vinculo)}");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN con_tipo_resistencia tr ON t.tp_resist=tr.cod");
            sql.Append(" WHERE t.usina_base=@idUsina");
            sql.Append(" AND t.uso=@idUso");
            sql.Append(" AND t.pedra=@idPedra");
            sql.Append(" AND t.slump=@idSlump");
            sql.Append(" AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" GROUP BY tr.cod;");

            var result = _context.Database.Connection.Query<ResistenciaTipo>(sql.ToString(), new { idUsina, idUso, idPedra, idSlump, VENDEDOR_PADRAO, numeroTabela });

            return result;
        }

        public IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT t.fck");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN fis_mercadoria m ON t.numeracao_produto=m.numeracao_produto");
            sql.Append(" WHERE t.usina_base=@idUsina");
            sql.Append(" AND t.uso=@idUso");
            sql.Append(" AND t.pedra=@idPedra");
            sql.Append(" AND t.slump=@idSlump");
            sql.Append(" AND t.tp_resist=@idResistenciaTipo");
            sql.Append(" AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" AND m.inativo='N'");
            sql.Append(" GROUP BY t.fck;");

            var result = _context.Database.Connection.Query<float>(sql.ToString(), new { idUsina, idUso, idPedra, idSlump, idResistenciaTipo, VENDEDOR_PADRAO, numeroTabela });

            return result;
        }

        public IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT t.consumo");
            sql.Append(" FROM con_tab_preco t");
            sql.Append(" INNER JOIN con_def_familia_resist r");
            sql.Append(" ON t.usina_base=r.usina AND t.uso=r.uso AND t.pedra=r.pedra AND t.slump=r.slump");
            sql.Append(" AND t.tp_resist=r.tp_resist AND t.fck=r.mpa AND t.consumo=r.consumo");
            sql.Append(" INNER JOIN con_def_familiar f");
            sql.Append(" ON r.usina=f.usina AND r.uso=f.uso AND r.pedra=f.pedra AND r.slump=f.slump AND r.versao=f.versao AND t.espec_familia=f.especificacao");
            sql.Append(" INNER JOIN fis_mercadoria m ON t.numeracao_produto=m.numeracao_produto");
            sql.Append(" WHERE t.usina_base=@idUsina");
            sql.Append(" AND t.uso=@idUso");
            sql.Append(" AND t.pedra=@idPedra");
            sql.Append(" AND t.slump=@idSlump");
            sql.Append(" AND t.tp_resist=@idResistenciaTipo");
            sql.Append(" AND t.vend_repres=@VENDEDOR_PADRAO AND num_tab=@numeroTabela");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append(" AND m.inativo='N'");
            sql.Append(" GROUP BY t.consumo;");

            var result = _context.Database.Connection.Query<int>(sql.ToString(), new { idUsina, idUso, idPedra, idSlump, idResistenciaTipo, VENDEDOR_PADRAO, numeroTabela });

            return result;
        }

        public float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" valor, tipo");
            sqlComando.Append(" FROM topsys.con_tab_vlr_m3_adic");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina_entrega=@ID_USINA");
            sqlComando.Append(" AND qt_m3_inicial<=@VOLUME");
            sqlComando.Append(" AND qt_m3_final>=@VOLUME");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USINA = idUsina, VOLUME = volume })
                .Select(row => new { row.valor, row.tipo })
                .FirstOrDefault();

            if (result == null) return 0.0f;

            switch ((string)result.tipo)
            {
                case "$":
                    return result.valor;
                case "%":
                    return precoUnitarioTabela * (result.valor / 100.0f);
                default:
                    return 0.0f;
            }
        }

        public TracoParticularidades ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT f.slump_inicial {nameof(TracoParticularidades.SlumpInicial )}, dte.especificacao {nameof(TracoParticularidades.TracoEspecificacao )},");
            sql.Append($" f.uso {nameof(TracoParticularidades.UsoCodigo )}, f.usina {nameof(TracoParticularidades.UsinaCodigo)}, f.pedra {nameof(TracoParticularidades.PedraCodigo )},");
            sql.Append($" dt.tp_resist {nameof(TracoParticularidades.ResistenciaTipoCodigo )}, f.slump {nameof(TracoParticularidades.SlumpCodigo)},");
            sql.Append($" dt.Fck {nameof(TracoParticularidades.Mpa )}, dt.consumo {nameof(TracoParticularidades.Consumo )}, dt.especificacao {nameof(TracoParticularidades.Especificacao)}");
            sql.Append($" FROM con_def_familiar f");
            sql.Append($" LEFT JOIN con_formula_dt dt ");
            sql.Append($" ON dt.usina = f.usina AND dt.uso = f.uso AND dt.pedra = f.pedra AND dt.slump = f.slump");
            sql.Append($" AND dt.especificacao =f.especificacao");
            sql.Append($" LEFT JOIN con_formula_dt_especificacao dte ");
            sql.Append($" ON dte.usina = f.usina AND dte.uso = f.uso AND dte.pedra = f.pedra AND dte.slump = f.slump");
            sql.Append($" AND dte.versao = f.versao AND dte.fck = dt.fck AND dte.consumo = dt.consumo");
            sql.Append($" WHERE f.usina = @idUsina ");
            sql.Append($" AND f.uso = @idUso");
            sql.Append($" AND f.pedra = @idPedra");
            sql.Append($" AND f.slump = @idSlump");
            sql.Append($" AND dt.tp_resist = @idResistenciaTipo");
            sql.Append($" AND dt.fck = @mpa");
            sql.Append($" AND dt.consumo = @consumo");
            sql.Append($" AND (f.status IN ({string.Join(",", STATUS_TRACO_ATIVO)}))");
            sql.Append("  order by f.status limit 1");

            var result = _context.Database.Connection.QueryFirstOrDefault<TracoParticularidades>(sql.ToString(), new { idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo });

            if (result == null)
                return result;

            var Especificacao = result.Especificacao;

            sql = new StringBuilder();

            sql.Append($"SELECT dti.valor ECI FROM con_formula_dt dti ");
            sql.Append($" WHERE dti.especificacao= @Especificacao");
            sql.Append($" AND dti.tp_resist = @idResistenciaTipo");
            sql.Append($" AND dti.fck = @mpa");
            sql.Append($" AND dti.consumo = @consumo");
            sql.Append($" AND dti.insumo_form = 'ECI'");

            result.Eci = _context.Database.Connection.QueryFirstOrDefault<float>(sql.ToString(), new { Especificacao, idResistenciaTipo, mpa, consumo });

            sql = new StringBuilder();

            sql.Append($"SELECT dti.valor ECI FROM con_formula_dt dti ");
            sql.Append($" WHERE dti.especificacao= @Especificacao");
            sql.Append($" AND dti.tp_resist = @idResistenciaTipo");
            sql.Append($" AND dti.fck = @mpa");
            sql.Append($" AND dti.consumo = @consumo");
            sql.Append($" AND dti.insumo_form = 'ECS'");

            result.Ecs = _context.Database.Connection.QueryFirstOrDefault<float>(sql.ToString(), new { Especificacao, idResistenciaTipo, mpa, consumo });


            return result;
        }

        public void SalvarLogUpdate(TracoPreco tracoPreco, string usuario)
        {
            var sqlComando = tracoPreco.MontarSqlUpdate(_context);

            _context.Database.Connection.GravarLogGeral(usuario, "con_tab_preco", sqlComando.ToString());
        }

        public IEnumerable<string> ListarTracosAtivos(int idUsina = 0)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT especificacao FROM con_def_familiar WHERE status IN (7101, 7105)");

            if (idUsina != 0)
                sqlComando.Append($" AND usina={idUsina}");

            return _context.Database.Connection.Query<string>(sqlComando.ToString());
        }

        public IEnumerable<Uso> ListarUsosPorSegmentacao(int idSegmentacao)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT u.cod {nameof(Uso.Codigo)}, u.descr {nameof(Uso.Descricao)}");
            sql.Append(" FROM con_uso u");
            sql.Append(" WHERE u.id_segmentacao=@idSegmentacao");
            sql.Append(" GROUP BY u.cod;");

            var result = _context.Database.Connection.Query<Uso>(sql.ToString(), new { idSegmentacao });

            return result;
        }

        public int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT def.numeracao_familia FROM con_tab_preco tab");

            sql.Append($" INNER JOIN con_def_familia_resist res");
            sql.Append($" ON res.usina=tab.usina_base");
            sql.Append($" AND res.uso=tab.uso");
            sql.Append($" AND res.pedra=tab.pedra");
            sql.Append($" AND res.slump=tab.slump");
            sql.Append($" AND res.tp_resist=tab.tp_resist");
            sql.Append($" AND res.mpa=tab.fck");
            sql.Append($" AND res.consumo=tab.consumo");

            sql.Append($" INNER JOIN con_def_familiar def");
            sql.Append($" ON def.usina=res.usina");
            sql.Append($" AND def.uso=res.uso");
            sql.Append($" AND def.pedra=res.pedra");
            sql.Append($" AND def.slump=res.slump");
            sql.Append($" AND def.versao=res.versao");
            sql.Append($" AND def.especificacao=tab.espec_familia");

            sql.Append($" INNER JOIN con_uso uso");
            sql.Append($" ON tab.uso=uso.cod AND uso.generico='S' AND uso.ativo='S'");

            sql.Append($" WHERE res.usina=@idUsina");
            sql.Append($" AND res.uso=@idUso");
            sql.Append($" AND res.pedra=@idPedra");
            sql.Append($" AND res.slump=@idSlump");
            sql.Append($" AND res.tp_resist=@idResistenciaTipo");
            sql.Append($" AND res.mpa=@mpa");
            sql.Append($" AND res.consumo=@consumo");
            sql.Append($" AND def.status=7101");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sql.ToString(), new { idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo });
        }
    }
}

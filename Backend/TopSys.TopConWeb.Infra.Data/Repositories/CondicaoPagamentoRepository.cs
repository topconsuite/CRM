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
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CondicaoPagamentoRepository : RepositoryBase<CondicaoPagamento>, ICondicaoPagamentoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public CondicaoPagamentoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public bool CondicaoPagamentoPadraoUsinaTipoPessoa(int condicaoPagamentoCodigo, int idUsina, DateTime data, string intervenienteTipo)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append(" dt_inicio");
            sqlComando.Append(" FROM topsys.con_cond_pagto");
            sqlComando.Append(" WHERE usina=@ID_USINA");
            sqlComando.Append(" AND dt_inicio<=@DATA");
            sqlComando.Append(" ORDER BY dt_inicio DESC");
            sqlComando.Append(" LIMIT 1");

            DateTime? dataVigencia = _context.Database.Connection
                .Query<DateTime?>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = data })
                .FirstOrDefault();

            if (dataVigencia != null && !intervenienteTipo.Trim().ToUpper().Equals(""))
            {
                sqlComando.Clear();
                sqlComando.Append("SELECT");
                sqlComando.Append(" gcp.cod");
                sqlComando.Append(" FROM topsys.con_cond_pagto ccp");
                sqlComando.Append(" CROSS JOIN topsys.ger_cond_pag gcp");
                if (intervenienteTipo.Trim().ToUpper().Equals("F"))
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagf) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                else
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagj) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                sqlComando.Append(" WHERE ccp.usina=@ID_USINA");
                sqlComando.Append(" AND ccp.dt_inicio=@DATA");

                var result = _context.Database.Connection
                    .Query<int>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = dataVigencia })
                    .Distinct()
                    .ToList();

                return result.Contains(condicaoPagamentoCodigo);
            }

            return false;

        }

        public IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append(" dt_inicio");
            sqlComando.Append(" FROM topsys.con_cond_pagto");
            sqlComando.Append(" WHERE usina=@ID_USINA");
            sqlComando.Append(" AND dt_inicio<=@DATA");
            sqlComando.Append(" ORDER BY dt_inicio DESC");
            sqlComando.Append(" LIMIT 1");

            DateTime? dataVigencia = _context.Database.Connection
                .Query<DateTime?>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = data })
                .FirstOrDefault();

            if (dataVigencia != null && !intervenienteTipo.Trim().ToUpper().Equals(""))
            {
                sqlComando.Clear();
                sqlComando.Append("SELECT");
                sqlComando.Append(" gcp.cod");
                sqlComando.Append(" FROM topsys.con_cond_pagto ccp");
                sqlComando.Append(" CROSS JOIN topsys.ger_cond_pag gcp");
                if (intervenienteTipo.Trim().ToUpper().Equals("F"))
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagf) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                else
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagj) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                sqlComando.Append(" WHERE ccp.usina=@ID_USINA");
                sqlComando.Append(" AND ccp.dt_inicio=@DATA");

                var result = _context.Database.Connection
                    .Query<int>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = dataVigencia })
                    .Distinct()
                    .ToList();

                var condicoesPagamento = _context.CondicoesPagamento
                    .Where(t => result.Contains(t.Codigo))
                    .AsNoTracking()
                    .ToList();

                var condicoesPagamentosForaPadrao = _context.CondicoesPagamento
                    .Where(t => !result.Contains(t.Codigo))
                    .AsNoTracking()
                    .ToList();

                condicoesPagamento.AddRange(condicoesPagamentosForaPadrao);
                condicoesPagamento = condicoesPagamento.Where(x => x.Ativo == "S").ToList();

                return condicoesPagamento;
            }

            return _context.CondicoesPagamento.AsNoTracking().ToList();
        }

        public IEnumerable<CondicaoPagamento> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine($"SELECT show_demais_cond_pagamento ParametroCondicoesPagamento");
            sqlComando.AppendLine($"FROM con_usina");
            sqlComando.AppendLine($"WHERE cod = @ID_USINA");

            var parametroShowCondicoesPagamento = _context.Database.Connection
                .QueryFirstOrDefault<string>(sqlComando.ToString(), new { ID_USINA = idUsina });

            var showCondicoesPagamento = (string.IsNullOrEmpty(parametroShowCondicoesPagamento) ? false : parametroShowCondicoesPagamento.Equals("S"));

            sqlComando.Clear();
            sqlComando.Append("SELECT");
            sqlComando.Append(" dt_inicio");
            sqlComando.Append(" FROM topsys.con_cond_pagto");
            sqlComando.Append(" WHERE usina=@ID_USINA");
            sqlComando.Append(" AND dt_inicio<=@DATA");
            sqlComando.Append(" AND id_segmentacao=@SEGMENTACAO");
            sqlComando.Append(" ORDER BY dt_inicio DESC");
            sqlComando.Append(" LIMIT 1");

            DateTime? dataVigencia = _context.Database.Connection
                .Query<DateTime?>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = data, SEGMENTACAO = segmentacao })
                .FirstOrDefault();

            if (dataVigencia != null && !intervenienteTipo.Trim().ToUpper().Equals(""))
            {
                sqlComando.Clear();
                sqlComando.Append("SELECT");
                sqlComando.Append(" gcp.cod");
                sqlComando.Append(" FROM topsys.con_cond_pagto ccp");
                sqlComando.Append(" CROSS JOIN topsys.ger_cond_pag gcp");
                if (intervenienteTipo.Trim().ToUpper().Equals("F"))
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagf) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                else
                {
                    sqlComando.Append(" ON CONCAT(',',ccp.cond_pagj) LIKE CONCAT('%,',LPAD(gcp.cod,3,'0'),',%')");
                }
                sqlComando.Append(" WHERE ccp.usina=@ID_USINA");
                sqlComando.Append(" AND ccp.dt_inicio=@DATA");
                sqlComando.Append(" AND id_segmentacao=@SEGMENTACAO");

                var result = _context.Database.Connection
                    .Query<int>(sqlComando.ToString(), new { ID_USINA = idUsina, DATA = dataVigencia, SEGMENTACAO = segmentacao })
                    .Distinct()
                    .ToList();

                var condicoesPagamento = _context.CondicoesPagamento
                    .Where(t => result.Contains(t.Codigo))
                    .AsNoTracking()
                    .ToList();

                if(showCondicoesPagamento)
                {
                    
                    var condicoesPagamentosForaPadrao = _context.CondicoesPagamento
                    .Where(t => !result.Contains(t.Codigo))
                    .AsNoTracking()
                    .ToList();

                    condicoesPagamento.AddRange(condicoesPagamentosForaPadrao);

                }

                return condicoesPagamento;
            }

            return _context.CondicoesPagamento.AsNoTracking().ToList();

        }

        public IEnumerable<CondicaoPagamento> ListarComParcelasPorCodigos(params int[] codigos)
        {
            var condicoes = _context.CondicoesPagamento
                .Where(t => codigos.Contains(t.Codigo))
                .Include(t => t.Parcelas)
                .AsNoTracking()
                .ToList();

            return condicoes;
        }

        public float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" valor, tipo");
            sqlComando.Append(" FROM topsys.con_cond_pagto_tx");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" (usina=@ID_USINA OR usina=0)");
            sqlComando.Append(" AND cond_pagto=@ID_CONDICAO");
            sqlComando.Append(" ORDER BY usina DESC");
            sqlComando.Append(" LIMIT 1");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_CONDICAO = idCondicaoPagamento, ID_USINA = idUsina })
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

        public PagedList<CondicaoPagamento> ListaEmOrdemCrescente(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter)
        {
            var pagedList = _context.CondicoesPagamento
                .OrderBy(t => new { t.Codigo })
                .Include(t => t.Parcelas)
                .Where(filter)
                .PagedList(pagina, porPagina, filter);

            foreach (var item in pagedList.Records)
            {
                item.CondicaoDaCobranca = _context.CadastrosDiversos.Where(t => t.Aplicativo == ""
                && t.ProgramaNumero == 12 && t.ProgramaCampo == "base_nf"
                && t.Codigo == item.CondicaoDaCobrancaCod).FirstOrDefault();

                item.Parcelas = item.Parcelas.OrderBy(t => t.Dias).ToList();
            }

            return pagedList;
        }

        public bool PossuiObrasUtilizando(int idCondicaoPagamento)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" count(*)");
            sqlComando.Append(" FROM topsys.con_obras");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" cond_pgto=@ID_CONDICAO");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString(), new { ID_CONDICAO = idCondicaoPagamento });

            return result > 0;
    
        }
        new public void  Adicionar(CondicaoPagamento condicaoPagamento)
        {
            var sqlComando = condicaoPagamento.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_cond_pag", sqlComando.ToString());
        }

        public ICollection<CondicaoPagamento> ListarCondicaoPagamento()
        {
            return _context
                .CondicoesPagamento
                .Include(t => t.Parcelas)
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public CondicaoPagamento ObterPorIdCondicaoPagamento(int id, bool tracking = false)
        {
            return _context
                .CondicoesPagamento
                .Include(t => t.Parcelas)
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public CondicaoPagamento ObterPorExternalIdCondicaoPagamento(string externalId, bool tracking = false)
        {
            return _context
                .CondicoesPagamento
                .Include(t => t.Parcelas)
                .Where(t => t.IdExterno == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public int ObterProximoCodigo()
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("SELECT IFNULL(MAX(cod) + 1, 1) FROM ger_cond_pag");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }
    }
}

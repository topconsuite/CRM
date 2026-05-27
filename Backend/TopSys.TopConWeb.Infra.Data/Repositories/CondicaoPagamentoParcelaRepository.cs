using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CondicaoPagamentoParcelaRepository: RepositoryBase<CondicaoPagamentoParcela>, ICondicaoPagamentoParcelaRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public CondicaoPagamentoParcelaRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(CondicaoPagamentoParcela condicaoPagamentoParcela)
        {
            var sqlComando = condicaoPagamentoParcela.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_cond_pag_parc", sqlComando.ToString());
        }



		public void Remover(int conditionPayment)
		{
			var sqlCommand = new StringBuilder();
			sqlCommand.AppendLine("DELETE");
			sqlCommand.AppendLine(" FROM");
			sqlCommand.AppendLine(" ger_cond_pag_parc");
			sqlCommand.AppendLine(" WHERE");
			sqlCommand.Append($" cond_pag = '{conditionPayment}'");

			_context.Connection.Execute(sqlCommand.ToString(), new { ConditionPayment = conditionPayment });
			_context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_cond_pag_parc", sqlCommand.ToString());
		}
	}
}

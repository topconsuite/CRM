using Common.Logging;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System;
using System.Linq;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Context
{
    public class CustomEFInterceptor : IDbCommandInterceptor

    {
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            WriteLog(command, interceptionContext, "NonQueryExecuted");
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //WriteLog(command, interceptionContext, "NonQueryExecuting");
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            //WriteLog(command, interceptionContext, "ReaderExecuted");
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            //WriteLog(command, interceptionContext, "ReaderExecuting");
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //WriteLog(command, interceptionContext, "ScalarExecuted");
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //WriteLog(command, interceptionContext, "ScalarExecuting");
        }

        private void WriteLog<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext, string source)
        {
            //if (interceptionContext.Exception != null) return;
            
            var sql = command.CommandText;

            foreach (DbParameter param in command.Parameters)
            {
                string value = "";
                switch (param.Value.GetType().Name)
                {
                    case "String":
                        value = $"'{param.Value}'";
                        break;
                    case "DateTime":
                        var dateValue = (DateTime?)param.Value;
                        value = $"'{dateValue?.Year}-{dateValue?.Month}-{dateValue?.Day}'";
                        break;
                    default:
                        value = $"{param.Value.ToString().Replace(',', '.')}";
                        break;
                }
                if (param.Value == null) value = "NULL";

                var paramName = param.ParameterName;
                if (!paramName.Contains("@")) paramName = $"@{param.ParameterName}";

                sql = sql.Replace(paramName, value);
            }

            var context = interceptionContext.DbContexts.Where(t => t.Database.Connection == command.Connection).FirstOrDefault();
            var _logGeralRepository = new LogGeralRepository((AppDataContext)context);

            var log = new LogGeral();
            log.Data = DateTime.Now;
            log.Hora = DateTime.Now;
            log.Tabela = "?";
            log.Script = sql;
            log.Usuario = "?";

            _logGeralRepository.Adicionar(log);
        }
    }
}

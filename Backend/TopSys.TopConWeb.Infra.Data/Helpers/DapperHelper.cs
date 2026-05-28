using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace TopSys.TopConWeb.Infra.Data.Helpers
{
    public static class DapperHelper
    {
        public static int GravarLogGeral(this IDbConnection cnn, string userName, string tableName, string sql, object param = null, IDbTransaction transaction = null)
        {
            string sqlFinal = SubstituirParametros(sql, param);

            var sqlLog = new StringBuilder();

            sqlLog.Append($"INSERT INTO ger_log");
            sqlLog.Append($" SET usuario=@{nameof(userName)}");
            sqlLog.Append($", tabela=@{nameof(tableName)}");
            sqlLog.Append($", script=@{nameof(sqlFinal)}");
            sqlLog.Append($", data=curdate()");
            sqlLog.Append($", hora=curtime()");
            sqlLog.Append($", local=999");
            sqlLog.Append($", at=0");

            return cnn.Execute(sqlLog.ToString(), new { userName, tableName, sqlFinal }, transaction);
        }

        public static string SubstituirParametros(string sql, object param)
        {
            string sqlFinal = sql;

            if (param != null)
            {
                var properties = param.GetType().GetProperties().OrderByDescending(t => t.Name).ToArray();

                foreach (var property in properties)
                {
                    var value = property.GetValue(param);

                    if (value == null)
                    {
                        sqlFinal = sqlFinal.Replace($"@{property.Name}", "null");
                        continue;
                    }

                    if (value.GetType().BaseType.Name == "Enum")
                    {
                        sqlFinal = sqlFinal.Replace($"@{property.Name}", $"{(int)value}");
                        continue;
                    }

                    switch (value.GetType().Name)
                    {
                        case "String":
                            sqlFinal = sqlFinal.Replace($"@{property.Name}", $"'{value.ToString().Replace("'", "''")}'");
                            break;
                        case "DateTime":
                            var d = (DateTime)value;
                            sqlFinal = sqlFinal.Replace($"@{property.Name}", $"'{d.Year}-{d.Month}-{d.Day} {d.Hour}:{d.Minute}:{d.Second}'");
                            break;
                        default:
                            sqlFinal = sqlFinal.Replace($"@{property.Name}", $"{value.ToString().Replace(',', '.')}");
                            break;
                    }
                }
            }

            return sqlFinal;
        }
    }
}

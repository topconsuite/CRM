using DbUp;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.Infra.Data.Migrations
{
    public static class DbUpMigration
    {
        public static void UpgradeDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AppCnnStr"].ConnectionString;

            if(!connectionString.ToLower().Contains("charset"))
                connectionString = connectionString + (connectionString.EndsWith(";") ? " " : "; ") + "Charset=latin1";

            var upgrader = DeployChanges.To
                .MySqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), System.Text.Encoding.Default)
                .LogToConsole()
                .WithExecutionTimeout(TimeSpan.FromHours(24))
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception(result.Error?.Message, result.Error);
            }
        }

        public static void VerifyColumnsVersionTables(IDatabaseRepository databaseRepository)
        {
            var tables = new string[] {
                "con_aprov",
                "con_chtel_cobranca",
                "con_chtel_faturamento",
                "con_chtel_pag",
                "con_chtel_resp_solid",
                "con_chtel",
                "con_contrato_boleto",
                "con_contrato_ccredit",
                "con_contrato_cheque",
                "con_contrato_dep",
                "con_contrato_dinheir",
                "con_contrato_pag",
                "con_contrato",
                "con_obras_dem_serv",
                "con_obras_log",
                "con_obras_mp",
                "con_obras_reajuste",
                "con_obras_trib_mun",
                "con_obras_tx",
                "con_obras",
                "con_programacao_dem_serv",
                "con_prop_bomba",
                "con_proposta_item",
                "con_reaj_bomba",
                "con_reajuste_item",
                "con_taxa_extra"
             };
            foreach (var tableName in tables) {
                var table = databaseRepository.ObterColunasTabela(tableName);
                var tableVersion = databaseRepository.ObterColunasTabela($"{tableName}_versao");

                var differenceColumns = table.Except(tableVersion).Concat(tableVersion.Except(table));

                if (differenceColumns.Count() > 0)
                {
                    throw new Exception($"Há diferença entre as tabelas {tableName} e {tableName}_versao, colunas:\n\n{string.Join("\n", differenceColumns)}");
                }
            }
        }
    }
}

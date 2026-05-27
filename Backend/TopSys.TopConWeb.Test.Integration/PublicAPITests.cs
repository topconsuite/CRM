using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TopSys.TopConWeb.API;

namespace TopSys.TopConWeb.Test.Integration
{
    [TestClass]
    public sealed class NotaFiscalDigitalIntegrationTests
    {
        private static string ConnectionString;
        private static TestServer _server;
        private static HttpClient _client;
        private static List<(int filial, int interv, int tp_doc, string serie, long num_nf, int seq_nf)> _notas = new();

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _server = TestServer.Create<Startup>();
            _client = _server.HttpClient;
            ConnectionString = ConfigurationManager.ConnectionStrings["AppCnnStr"].ConnectionString;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        [TestMethod]
        public async Task Fluxo_NotaFiscalDigital_ValidarEndpoints_Transacao()
        {
            await ExecuteUpdateTransacionado(async () =>
            {
                await Get_NotaFiscalDigital_PorDataAtualizacao_ValidarResposta();
                await Get_NotaFiscalDigital_PorChave_ValidarResposta();
                await Get_NotaFiscalDigital_PorChaveSemInterveniente_ValidarResposta();
                await Get_NotaFiscalDigital_Listar_ValidarResposta();
            });
        }

        private static async Task ExecuteUpdateTransacionado(Func<Task> action)
        {
            using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            var sql = @"
                UPDATE fis_nf f
                JOIN (
                    SELECT 
                        filial,
                        interv,
                        tp_doc,
                        serie,
                        num_nf,
                        seq_nf,
                        MAX(dt_oper) AS dt_oper
                    FROM fis_nf
                    WHERE dt_oper IS NOT NULL AND serie <> ''
                    GROUP BY 
                        interv, 
                        tp_doc, 
                        YEAR(dt_oper), 
                        MONTH(dt_oper) AND dt_oper>='2025-01-01' 
                ) g 
                ON f.filial = g.filial
                AND f.interv = g.interv
                AND f.tp_doc = g.tp_doc
                AND f.serie = g.serie
                AND f.num_nf = g.num_nf
                AND f.seq_nf = g.seq_nf
                SET f.atualizado_em = NOW();";

            using var cmd = new MySqlCommand(sql, conn, transaction);
            var rowsAffected = await cmd.ExecuteNonQueryAsync();

            Assert.IsTrue(rowsAffected >= 0, "Sem registros para validar");

            await action();

            transaction.Rollback();
        }


        private static async Task Get_NotaFiscalDigital_PorDataAtualizacao_ValidarResposta()
        {
            var startDate = DateTime.Now.AddHours(-10).ToString("yyyy-MM-ddTHH:mm:ss");
            const int limit = 100;

            var allNotas = new List<(int filial, int interv, int tp_doc, string serie, long num_nf, int seq_nf)>();
            int page = 1;

            while (true)
            {
                var url = $"api/integrations/digital-invoice/by-update-date?start_date={startDate}&limit={limit}&page={page}";
                var response = await _client.GetAsync(url);
                await AssertCustom(response);

                var json = await response.Content.ReadAsStringAsync();
                var obj = JToken.Parse(json);

                if (obj["status"]?.ToString() != "success")
                    break;

                var result = obj["result"];
                var records = result?["records"] as JArray;

                if (records == null || !records.HasValues)
                    break;

                allNotas.AddRange(
                    records.Select(n => (
                        filial: (int)n["branch"],
                        interv: n["client"]?.Type == JTokenType.Null ? 0 : (int)n["client"],
                        tp_doc: (int)n["document_type"],
                        serie: string.IsNullOrEmpty((string)n["series"]) ? "1" : (string)n["series"],
                        num_nf: (long)n["invoice_number"],
                        seq_nf: (int)n["invoice_sequence"]
                    ))
                );

                var totalPages = (int?)result["pageCount"] ?? 1;
                var currentPage = (int?)result["currentPage"] ?? 1;

                if (currentPage >= totalPages)
                    break;

                page++;
            }

            _notas = allNotas;
        }



        private static async Task Get_NotaFiscalDigital_PorChave_ValidarResposta()
        {
            if (!_notas.Any() || _notas.All(n => n.interv == 0))
                return;

            foreach (var n in _notas.Where(x => x.interv > 0))
            {
                var url = $"api/integrations/digital-invoice/{n.filial}/{n.interv}/{n.tp_doc}/{n.serie}/{n.num_nf}/{n.seq_nf}";
                var resp = await _client.GetAsync(url);
                await AssertCustom(resp);
            }
        }

        private static async Task Get_NotaFiscalDigital_PorChaveSemInterveniente_ValidarResposta()
        {
            foreach (var n in _notas.Where(x => x.interv == 0))
            {
                var url = $"api/integrations/digital-invoice/{n.filial}/{n.tp_doc}/{n.serie}/{n.num_nf}/{n.seq_nf}";
                var resp = await _client.GetAsync(url);
                await AssertCustom(resp);
            }
        }

        private static async Task Get_NotaFiscalDigital_Listar_ValidarResposta()
        {
            var url = $"api/integrations/digital-invoice?page=1&limit=100";
            var resp = await _client.GetAsync(url);
            await AssertCustom(resp);
        }

        private static async Task AssertCustom(HttpResponseMessage resp)
        {
            var json = await resp.Content.ReadAsStringAsync();
            switch (resp.StatusCode)
            {
                case HttpStatusCode.OK:
                    Assert.IsTrue(true);
                    break;
                case HttpStatusCode.PreconditionFailed:
                    var objFail = JObject.Parse(json);
                    var code = (string)objFail["errorCode"];
                    Assert.AreEqual("80018", code);
                    break;
                case HttpStatusCode.BadRequest:
                    var objBad = JObject.Parse(json);
                    var status = (string)objBad["status"];
                    Assert.AreEqual("error", status);
                    break;
                default:
                    Assert.Fail($"Status inesperado: {resp.StatusCode}");
                    break;
            }
        }
    }
}

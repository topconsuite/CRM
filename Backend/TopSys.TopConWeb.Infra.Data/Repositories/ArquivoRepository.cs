using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using System.Text;
using TopSys.TopConWeb.SharedKernel.Helpers;
using Dapper;
using System.Collections;
using TopSys.TopConWeb.Domain.Entities;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ArquivoRepository : IArquivoRepository
    {
        private readonly AppDataContext _context;
        public ArquivoRepository(AppDataContext context) 
        {
            _context = context;
        }

        public IEnumerable<ArquivoBanco> ListarArquivosPorChave(int numProg, string chave)
        {

            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine($"    c.id {nameof(ArquivoBanco.Id)}");
            sql.AppendLine($"    ,c.aplic {nameof(ArquivoBanco.Aplicacao)}");
            sql.AppendLine($"    ,c.num_prog {nameof(ArquivoBanco.Programa)}");
            sql.AppendLine($"    ,c.chave {nameof(ArquivoBanco.Chave)}");
            sql.AppendLine($"    ,c.seq {nameof(ArquivoBanco.Sequencia)}");
            sql.AppendLine($"    ,c.id_cadast {nameof(ArquivoBanco.IdCadastro)}");
            sql.AppendLine($"    ,c.id_atual {nameof(ArquivoBanco.IdAtualizacao)}");
            sql.AppendLine("FROM topsysarquivos.arquivos c");
            sql.AppendLine("WHERE aplic='CRM'");
            sql.AppendLine("    AND c.num_prog = @NumProg");
            sql.AppendLine("    AND c.chave = @Chave");

            var parametros = new { NumProg = numProg, Chave = chave };
            var result = _context.Database.Connection.Query<ArquivoBanco>(sql.ToString(), parametros);

            return result;

        }

        public void SalvarArquivo(int numProg, string chave, byte[] reportPDF, int sequencia, string idCadastro)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT IGNORE INTO topsysarquivos.arquivos SET ");
            sql.Append($"id = uuid(), ");
            sql.Append($"aplic='CRM', ");
            sql.Append($"num_prog=@{nameof(numProg)}, ");
            sql.Append($"chave=@{nameof(chave)}, ");
            sql.Append($"arquivo=@{nameof(reportPDF)}, ");
            sql.Append($"seq=@{nameof(sequencia)},");
            sql.Append($"id_cadast='{StringHelper.GetIDD(idCadastro)}'");

            _context.Database.Connection.Execute(sql.ToString(), new { numProg, chave, reportPDF, sequencia });
        }

        public byte[] ObterArquivo(int numProg, string chave, int sequencia)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT arquivo FROM topsysarquivos.arquivos ");
            sql.Append($"WHERE aplic='CRM' ");
            sql.Append($"AND num_prog=@{nameof(numProg)} ");
            sql.Append($"AND chave=@{nameof(chave)} ");
            sql.Append($"AND seq=@{nameof(sequencia)}");

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { numProg, chave, sequencia });
        }
    }
}

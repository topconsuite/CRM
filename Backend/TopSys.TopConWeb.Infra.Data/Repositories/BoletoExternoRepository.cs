using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class BoletoExternoRepository : RepositoryBase<BoletoExterno>, IBoletoExternoRepository
    {
        public BoletoExternoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<BoletoExterno> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT arq.id {nameof(BoletoExterno.Id)}");
            sqlComando.Append($", arq.chave {nameof(BoletoExterno.Chave)}");
            sqlComando.Append($", arq.seq {nameof(BoletoExterno.Sequencia)}");
            sqlComando.Append($", arq.nome_arquivo {nameof(BoletoExterno.NomeArquivo)}");
            sqlComando.Append($", hist.dt_hr {nameof(BoletoExterno.DataHora)}");
            sqlComando.Append($" FROM topsysarquivos.arquivos arq");
            sqlComando.Append($" INNER JOIN topsys.con_fat_email_hist hist");
            sqlComando.Append($" ON arq.id=hist.id_arquivo");
            sqlComando.Append($" WHERE arq.aplic='API'");
            sqlComando.Append($" AND arq.num_prog=6325");
            sqlComando.Append($" AND arq.chave='{codUsina}-{numeroContrato}-{anoContrato}'");
            sqlComando.Append($" ORDER BY hist.dt_hr, arq.seq");

            return _context.Database.Connection.Query<BoletoExterno>(sqlComando.ToString()).ToList();
        }

        public byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT arquivo");
            sql.Append($" FROM topsysarquivos.arquivos");
            sql.Append($" WHERE aplic='API'");
            sql.Append($" AND num_prog=6325");
            sql.Append($" AND id=@{nameof(idArquivo)}");
            sql.Append($" AND chave=@{nameof(chave)}");
            sql.Append($" AND seq=@{nameof(sequencia)}");

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { idArquivo, chave, sequencia });
        }

        public BoletoExterno ObterPorChaveNomeArquivo(string chave, string nomeArquivo)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT arq.id {nameof(BoletoExterno.Id)}");
            sqlComando.Append($", arq.chave {nameof(BoletoExterno.Chave)}");
            sqlComando.Append($", arq.seq {nameof(BoletoExterno.Sequencia)}");
            sqlComando.Append($", arq.nome_arquivo {nameof(BoletoExterno.NomeArquivo)}");
            sqlComando.Append($" FROM topsysarquivos.arquivos arq");
            sqlComando.Append($" WHERE arq.aplic='API'");
            sqlComando.Append($" AND arq.num_prog=6325");
            sqlComando.Append($" AND arq.chave='{chave}'");
            sqlComando.Append($" AND arq.nome_arquivo='{nomeArquivo}'");

            return _context.Database.Connection.Query<BoletoExterno>(sqlComando.ToString()).FirstOrDefault();
        }

        public void AdicionarBoletoExterno(BoletoExterno externalBankSlip, Guid idFile, Guid idHistory)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT IGNORE INTO  topsysarquivos.arquivos SET ");
            sql.Append($"id=@idArquivo, ");
            sql.Append($"aplic='API', ");
            sql.Append($"num_prog='6325', ");
            sql.Append($"chave=@chave, ");
            sql.Append($"seq=@sequencia, ");
            sql.Append($"caminho_arquivo='', ");
            sql.Append($"nome_arquivo=@nomeArquivo, ");
            sql.Append($"arquivo=@arquivo, ");
            sql.Append($"id_cadast='API {DateTime.Now.ToString("dd/MM/yy")}' ");

            _context.Database.Connection.Execute(sql.ToString(), new
            {
                idArquivo = idFile,
                chave = externalBankSlip.Chave,
                arquivo = externalBankSlip.Arquivo,
                sequencia = externalBankSlip.Sequencia,
                nomeArquivo = externalBankSlip.NomeArquivo
            });

            sql.Clear();
            sql.Append("INSERT IGNORE INTO topsys.con_fat_email_hist SET ");
            sql.Append($"id=@idHistorico, ");
            sql.Append($"tipo_registro='BOLETO', ");
            sql.Append($"chave=@chave, ");
            sql.Append($"dt_hr=@dataHora, ");
            sql.Append($"usuario='API {DateTime.Now.ToString("dd/MM/yy")}', ");
            sql.Append($"email_destino='', ");
            sql.Append($"id_arquivo=@idArquivo ");

            _context.Database.Connection.Execute(sql.ToString(), new
            {
                idHistorico = idHistory,
                chave = externalBankSlip.Chave,
                dataHora = DateTime.Now,
                idArquivo = idFile
            });
        }
    }
}

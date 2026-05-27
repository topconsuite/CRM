using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class MovimentoBancoRepository : IMovimentoBancoRepository
    {
        protected AppDataContext _context;
        private readonly IdentityHelperService _identityHelperService;

        public MovimentoBancoRepository(AppDataContext context, IdentityHelperService identityHelperService)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public void Adicionar(MovimentoBanco movimentoBanco)
        {
            var sqlCommand = new StringBuilder();

            _context.Database.Connection.Execute("INSERT INTO topsys.fin_mov_banco_aut_inc SET id=NULL");
            movimentoBanco.Id = _context.Database.Connection.QueryFirstOrDefault<long>("SELECT LAST_INSERT_ID()");

            sqlCommand.Clear();
            sqlCommand.Append($"INSERT INTO topsys.fin_mov_banco");
            sqlCommand.Append($" SET");
            sqlCommand.Append($" controle=@{nameof(movimentoBanco.Id)},");
            sqlCommand.Append($" emp=@{nameof(movimentoBanco.EmpresaCodigo)},");
            sqlCommand.Append($" bco=@{nameof(movimentoBanco.ContaCodigo)},");
            sqlCommand.Append($" dt_op=@{nameof(movimentoBanco.DataOperacao)},");
            sqlCommand.Append($" tp_doc=@{nameof(movimentoBanco.DocumentoTipo)},");
            sqlCommand.Append($" num_doc=@{nameof(movimentoBanco.DocumentoNumero)},");
            sqlCommand.Append($" es=@{nameof(movimentoBanco.EntradaSaida)},");
            sqlCommand.Append($" op=@{nameof(movimentoBanco.OperacaoCodigo)},");
            sqlCommand.Append($" vl=@{nameof(movimentoBanco.Valor)},");
            sqlCommand.Append($" orig=@{nameof(movimentoBanco.Origem)},");
            sqlCommand.Append($" cc=@{nameof(movimentoBanco.CentroCustoCodigo)},");
            sqlCommand.Append($" id_cadast=@{nameof(movimentoBanco.IdCadastro)},");
            sqlCommand.Append($" obs=@{nameof(movimentoBanco.Observacao)},");
            sqlCommand.Append($" natureza_finan=@{nameof(movimentoBanco.NaturezaFinanceira)}");

            _context.Database.Connection.Execute(sqlCommand.ToString(), movimentoBanco);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_mov_banco", sqlCommand.ToString(), movimentoBanco);
        }

        public void AtualizarNaoConciliado(MovimentoBanco movimentoBanco)
        {
            var sqlCommand = new StringBuilder();
           
            sqlCommand.Clear();
            sqlCommand.Append($"UPDATE topsys.fin_mov_banco");
            sqlCommand.Append($" SET");
            sqlCommand.Append($" emp=@{nameof(movimentoBanco.EmpresaCodigo)},");
            sqlCommand.Append($" bco=@{nameof(movimentoBanco.ContaCodigo)},");
            sqlCommand.Append($" dt_op=@{nameof(movimentoBanco.DataOperacao)},");
            sqlCommand.Append($" tp_doc=@{nameof(movimentoBanco.DocumentoTipo)},");
            sqlCommand.Append($" num_doc=@{nameof(movimentoBanco.DocumentoNumero)},");
            sqlCommand.Append($" es=@{nameof(movimentoBanco.EntradaSaida)},");
            sqlCommand.Append($" op=@{nameof(movimentoBanco.OperacaoCodigo)},");
            sqlCommand.Append($" vl=@{nameof(movimentoBanco.Valor)},");
            sqlCommand.Append($" orig=@{nameof(movimentoBanco.Origem)},");
            sqlCommand.Append($" cc=@{nameof(movimentoBanco.CentroCustoCodigo)},");
            sqlCommand.Append($" id_cadast=@{nameof(movimentoBanco.IdCadastro)},");
            sqlCommand.Append($" obs=@{nameof(movimentoBanco.Observacao)}");
            sqlCommand.Append($" WHERE controle=@{nameof(movimentoBanco.Id)}");
            sqlCommand.Append($" AND conc<>'R'");

            _context.Database.Connection.Execute(sqlCommand.ToString(), movimentoBanco);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_mov_banco", sqlCommand.ToString(), movimentoBanco);
        }

        public IEnumerable<MovimentoBanco> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao)
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.Append($"SELECT");
            sqlCommand.Append($" m.controle {nameof(MovimentoBanco.Id)},");
            sqlCommand.Append($" m.emp {nameof(MovimentoBanco.EmpresaCodigo)},");
            sqlCommand.Append($" m.bco {nameof(MovimentoBanco.ContaCodigo)},");
            sqlCommand.Append($" m.dt_op {nameof(MovimentoBanco.DataOperacao)},");
            sqlCommand.Append($" m.tp_doc {nameof(MovimentoBanco.DocumentoTipo)},");
            sqlCommand.Append($" m.num_doc {nameof(MovimentoBanco.DocumentoNumero)},");
            sqlCommand.Append($" m.es {nameof(MovimentoBanco.EntradaSaida)},");
            sqlCommand.Append($" m.op {nameof(MovimentoBanco.OperacaoCodigo)},");
            sqlCommand.Append($" o.descr {nameof(MovimentoBanco.OperacaoDescricao)},");
            sqlCommand.Append($" ABS(m.vl) {nameof(MovimentoBanco.Valor)},");
            sqlCommand.Append($" (ABS(m.vl)-IFNULL(b.vlr_vinculado,0)) {nameof(MovimentoBanco.Saldo)},");
            sqlCommand.Append($" m.orig {nameof(MovimentoBanco.Origem)},");
            sqlCommand.Append($" m.cc {nameof(MovimentoBanco.CentroCustoCodigo)},");
            sqlCommand.Append($" m.id_cadast {nameof(MovimentoBanco.IdCadastro)},");
            sqlCommand.Append($" m.obs {nameof(MovimentoBanco.Observacao)}");
            sqlCommand.Append($" FROM topsys.fin_mov_banco m");
            sqlCommand.Append($" INNER JOIN fin_operacao o ON m.op=o.cod");
            sqlCommand.Append($" LEFT JOIN (");
            sqlCommand.Append($" SELECT id_mov_bco,SUM(ABS(valor)) vlr_vinculado");
            sqlCommand.Append($" FROM fin_vinc_car_mov_bco GROUP BY id_mov_bco");
            sqlCommand.Append($" ) b ON m.controle=b.id_mov_bco");
            sqlCommand.Append($" WHERE o.vinc_lancamento IN('O','S')");
            sqlCommand.Append($" AND IFNULL(b.vlr_vinculado,0)<ABS(m.vl)");
            sqlCommand.Append($" AND m.es=1");
            sqlCommand.Append($" AND m.emp=@{nameof(empresaCodigo)}");
            sqlCommand.Append($" AND m.bco=@{nameof(contaCodigo)}");

            if (dataOperacao != null)
            {
                sqlCommand.Append($" AND dt_op=@{nameof(dataOperacao)}");
            }

            return _context.Database.Connection
                .Query<MovimentoBanco>(sqlCommand.ToString(), new { empresaCodigo, contaCodigo, dataOperacao });
        }

        public MovimentoBanco ObterPorControle(long controle)
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.Append($"SELECT");
            sqlCommand.Append($" controle {nameof(MovimentoBanco.Id)},");
            sqlCommand.Append($" emp {nameof(MovimentoBanco.EmpresaCodigo)},");
            sqlCommand.Append($" bco {nameof(MovimentoBanco.ContaCodigo)},");
            sqlCommand.Append($" dt_op {nameof(MovimentoBanco.DataOperacao)},");
            sqlCommand.Append($" tp_doc {nameof(MovimentoBanco.DocumentoTipo)},");
            sqlCommand.Append($" num_doc {nameof(MovimentoBanco.DocumentoNumero)},");
            sqlCommand.Append($" es {nameof(MovimentoBanco.EntradaSaida)},");
            sqlCommand.Append($" op {nameof(MovimentoBanco.OperacaoCodigo)},");
            sqlCommand.Append($" vl {nameof(MovimentoBanco.Valor)},");
            sqlCommand.Append($" orig {nameof(MovimentoBanco.Origem)},");
            sqlCommand.Append($" cc {nameof(MovimentoBanco.CentroCustoCodigo)},");
            sqlCommand.Append($" id_cadast {nameof(MovimentoBanco.IdCadastro)},");
            sqlCommand.Append($" obs {nameof(MovimentoBanco.Observacao)}");
            sqlCommand.Append($" FROM topsys.fin_mov_banco");
            sqlCommand.Append($" WHERE controle = @{nameof(controle)}");

            return _context.Database.Connection.QueryFirstOrDefault<MovimentoBanco>(sqlCommand.ToString(), new { controle } );

        }
 
        public void RemoverNaoConciliadoPorControle(long controle, string userName = "")
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.Append($" DELETE FROM fin_mov_banco");
            sqlCommand.Append($" where controle=@{nameof(controle)}");
            sqlCommand.Append($" AND conc<>'R'");

            _context.Database.Connection.Execute(sqlCommand.ToString(), new { controle });

            _context.Database.Connection.GravarLogGeral(userName == "" ? _identityHelperService.GetUserName() : userName, "fin_mov_banco", sqlCommand.ToString(), new { controle });
        }

        //Para integração publica

        public long AdicionarERetornaId(MovimentoBanco movimentoBanco)
        {
            var sqlCommand = new StringBuilder();

            _context.Database.Connection.Execute("INSERT INTO topsys.fin_mov_banco_aut_inc SET id=NULL");
            movimentoBanco.Id = _context.Database.Connection.QueryFirstOrDefault<long>("SELECT LAST_INSERT_ID()");

            sqlCommand.Clear();
            sqlCommand.Append($"INSERT INTO topsys.fin_mov_banco");
            sqlCommand.Append($" SET");
            sqlCommand.Append($" controle=@{nameof(movimentoBanco.Id)},");
            sqlCommand.Append($" emp=@{nameof(movimentoBanco.EmpresaCodigo)},");
            sqlCommand.Append($" bco=@{nameof(movimentoBanco.ContaCodigo)},");
            sqlCommand.Append($" dt_op=@{nameof(movimentoBanco.DataOperacao)},");
            sqlCommand.Append($" tp_doc=@{nameof(movimentoBanco.DocumentoTipo)},");
            sqlCommand.Append($" num_doc=@{nameof(movimentoBanco.DocumentoNumero)},");
            sqlCommand.Append($" es=@{nameof(movimentoBanco.EntradaSaida)},");
            sqlCommand.Append($" op=@{nameof(movimentoBanco.OperacaoCodigo)},");
            sqlCommand.Append($" vl=@{nameof(movimentoBanco.Valor)},");
            sqlCommand.Append($" orig=@{nameof(movimentoBanco.Origem)},");
            sqlCommand.Append($" cc=@{nameof(movimentoBanco.CentroCustoCodigo)},");
            sqlCommand.Append($" id_cadast=@{nameof(movimentoBanco.IdCadastro)},");
            sqlCommand.Append($" obs=@{nameof(movimentoBanco.Observacao)},");
            sqlCommand.Append($" natureza_finan=@{nameof(movimentoBanco.NaturezaFinanceira)}");

            _context.Database.Connection.Execute(sqlCommand.ToString(), movimentoBanco);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_mov_banco", sqlCommand.ToString(), movimentoBanco);

            return movimentoBanco.Id;
        }

        public void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco, bool naoRemoverMovimentoConciliado = true)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_mov_banco ");
            sqlComando.Append($" WHERE controle = {idMovimentoBanco}");

            if (naoRemoverMovimentoConciliado) sqlComando.Append($" AND dt_conc IS NULL");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_mov_banco", comandoSql);
        }

        public double? ValorMovimentoDeBanco(long idMovimentoBanco)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("SELECT vl FROM fin_mov_banco ");
            sqlComando.Append($" WHERE controle = {idMovimentoBanco}");

            comandoSql = sqlComando.ToString();

            return _context.Database.Connection.QueryFirstOrDefault<double?>(comandoSql);

        }

        public void DescontaValorMovimentosDeBancoNaoConciliado(float? valor, long idMovimentoBanco)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("UPDATE fin_mov_banco ");
            sqlComando.Append($" SET vl=vl-'{valor}'");
            sqlComando.Append($" WHERE controle={idMovimentoBanco}");
            sqlComando.Append($" AND conc<>'R'");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_mov_banco", comandoSql);
        }
    }
}

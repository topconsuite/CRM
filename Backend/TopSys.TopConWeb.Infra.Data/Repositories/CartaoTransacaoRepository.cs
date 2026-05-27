using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CartaoTransacaoRepository: ICartaoTransacaoRepository
    {
        protected AppDataContext _context;
        private readonly IdentityHelperService _identityHelperService;

        public CartaoTransacaoRepository(AppDataContext context, IdentityHelperService identityHelperService)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public CartaoTransacao ObterCartaoTransacaoPeloTransacaoId(string transacaoId)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT  ID, ");
            sql.Append($"id_transacao {nameof(CartaoTransacao.TransacaoId)}, ");
            sql.Append($"cod_estabelecimento {nameof(CartaoTransacao.EstabelecimentoCod)}, ");
            sql.Append($"num_autorizacao {nameof(CartaoTransacao.AutorizacaoNumero)}, ");
            sql.Append($"right(num_cartao,4) {nameof(CartaoTransacao.CartaoNumero)}, ");
            sql.Append($"data_hora_transacao {nameof(CartaoTransacao.TransacaoDataHora)}, ");
            sql.Append($"status_processo {nameof(CartaoTransacao.StatusProcesso)}, ");
            sql.Append($"valor_transacao {nameof(CartaoTransacao.Valor)}, ");
            sql.Append($"qtde_parcelas {nameof(CartaoTransacao.QuantidadeParcelas)}, ");
            sql.Append($"nome_produto {nameof(CartaoTransacao.ProdutoNome)} ");
            sql.Append("FROM  topsys.con_cartao_transacao ");
            sql.Append("WHERE ");
            sql.Append("id_transacao = @TRANSACAOID ");

            return _context.Database.Connection.QueryFirstOrDefault<CartaoTransacao>(sql.ToString(), new { TRANSACAOID = transacaoId });

        }

        public void AtualizaErroNaGeracaoContasAReceber(string mensagem, string transacaoId)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT id INTO @id FROM topsys.con_cartao_transacao WHERE id_transacao = @transacao; ");
            sql.Append("UPDATE topsys.con_cartao_transacao ");
            sql.Append($"SET status_processo = 2, ");
            sql.Append($"motivo_erro = @MOTIVOERRO ");
            sql.Append("WHERE ");
            sql.Append("id = @id ");

            var filtro = new
            {
                MOTIVOERRO = mensagem,
                transacao = transacaoId
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_cartao_transacao", sql.ToString(), filtro);
        }

        public void AtualizaSucessoNaGeracaoContasAReceber(string transacaoId)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT id INTO @id FROM topsys.con_cartao_transacao WHERE id_transacao = @transacao; ");
            sql.Append("UPDATE topsys.con_cartao_transacao ");
            sql.Append($"SET status_processo = 1, ");
            sql.Append($"motivo_erro = '' ");
            sql.Append("WHERE ");
            sql.Append("id = @id ");

            _context.Database.Connection.Execute(sql.ToString(), new
            {
                transacao = transacaoId
            });

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_cartao_transacao", sql.ToString(), new
            {
                transacao = transacaoId
            });
        }

        public CartaoTransacao ObterPorDataNumeroCartaoAutorizacao(DateTime dataTransacao, int numeroCartao, string autorizacao)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT Id, ");
            sql.Append($"id_transacao {nameof(CartaoTransacao.TransacaoId)}, ");
            sql.Append($"cod_estabelecimento {nameof(CartaoTransacao.EstabelecimentoCod)}, ");
            sql.Append($"num_autorizacao {nameof(CartaoTransacao.AutorizacaoNumero)}, ");
            sql.Append($"right(num_cartao,4) {nameof(CartaoTransacao.CartaoNumero)}, ");
            sql.Append($"data_hora_transacao {nameof(CartaoTransacao.TransacaoDataHora)}, ");
            sql.Append($"status_processo {nameof(CartaoTransacao.StatusProcesso)}, ");
            sql.Append($"valor_transacao {nameof(CartaoTransacao.Valor)}, ");
            sql.Append($"qtde_parcelas {nameof(CartaoTransacao.QuantidadeParcelas)}, ");
            sql.Append($"origem {nameof(CartaoTransacao.Origem)} ");
            sql.Append("FROM  topsys.con_cartao_transacao ");
            sql.Append("WHERE ");
            sql.Append($"LPAD(LEFT(SUBSTRING_INDEX(num_cartao, '*', -1), 4),4,'0')='{numeroCartao.ToString().PadLeft(4,'0')}' ");
            sql.Append($"AND num_autorizacao='{autorizacao}' ");
            sql.Append($"AND YEAR(data_hora_transacao)={dataTransacao.Year} ");

            return _context.Database.Connection.QueryFirstOrDefault<CartaoTransacao>(sql.ToString());

        }

        public void Adicionar(CartaoTransacao cartaoTransacao)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT INTO con_cartao_transacao");
            sql.Append($" SET id_transacao=@{nameof(CartaoTransacao.TransacaoId)}");
            sql.Append($", origem=@{nameof(CartaoTransacao.Origem)}");
            sql.Append($", id_pedido=@{nameof(CartaoTransacao.PedidoId)}");
            sql.Append($", status=@{nameof(CartaoTransacao.Status)}");
            sql.Append($", cod_estabelecimento=@{nameof(CartaoTransacao.EstabelecimentoCod)}");
            sql.Append($", num_autorizacao=@{nameof(CartaoTransacao.AutorizacaoNumero)}");
            sql.Append($", data_hora_transacao=@{nameof(CartaoTransacao.TransacaoDataHora)}");
            sql.Append($", valor_transacao=@{nameof(CartaoTransacao.Valor)}");
            sql.Append($", tipo_transacao=@{nameof(CartaoTransacao.TransacaoTipo)}");
            sql.Append($", nome_produto=@{nameof(CartaoTransacao.ProdutoNome)}");
            sql.Append($", nome_sub_produto=@{nameof(CartaoTransacao.SubProdutoNome)}");
            sql.Append($", qtde_parcelas=@{nameof(CartaoTransacao.QuantidadeParcelas)}");
            sql.Append($", num_cartao=@{nameof(CartaoTransacao.CartaoNumero)}");
            sql.Append($", status_processo=@{nameof(CartaoTransacao.StatusProcesso)}");

            _context.Database.Connection.Execute(sql.ToString(), cartaoTransacao);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_cartao_transacao", sql.ToString(), cartaoTransacao);
        }

        public void RemoverPorId(int cartaoTransacaoId, string userName = "")
        {
            var sql = new StringBuilder();

            sql.Append($"DELETE FROM con_cartao_transacao WHERE id={cartaoTransacaoId}");
            _context.Database.Connection.Execute(sql.ToString());

            _context.Database.Connection.GravarLogGeral(userName == "" ? _identityHelperService.GetUserName() : userName, "con_cartao_transacao", sql.ToString());
        }

        public CartaoTransacao ObterPorDataNumeroCartaoAutorizacaoValorQuantidadeParcelas(DateTime dataTransacao, int numeroCartao, string autorizacao, float valor, int quantidadeParcelas)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT Id, ");
            sql.Append($"id_transacao {nameof(CartaoTransacao.TransacaoId)}, ");
            sql.Append($"cod_estabelecimento {nameof(CartaoTransacao.EstabelecimentoCod)}, ");
            sql.Append($"num_autorizacao {nameof(CartaoTransacao.AutorizacaoNumero)}, ");
            sql.Append($"right(num_cartao,4) {nameof(CartaoTransacao.CartaoNumero)}, ");
            sql.Append($"data_hora_transacao {nameof(CartaoTransacao.TransacaoDataHora)}, ");
            sql.Append($"status_processo {nameof(CartaoTransacao.StatusProcesso)}, ");
            sql.Append($"valor_transacao {nameof(CartaoTransacao.Valor)}, ");
            sql.Append($"qtde_parcelas {nameof(CartaoTransacao.QuantidadeParcelas)}, ");
            sql.Append($"origem {nameof(CartaoTransacao.Origem)} ");
            sql.Append("FROM  topsys.con_cartao_transacao ");
            sql.Append("WHERE ");
            sql.Append($"LPAD(LEFT(SUBSTRING_INDEX(num_cartao, '*', -1), 4),4,'0')='{numeroCartao.ToString().PadLeft(4, '0')}' ");
            sql.Append($"AND num_autorizacao=@AUTORIZACAO ");
            sql.Append($"AND DATE(data_hora_transacao)=@DATATRANSACAO ");
            sql.Append($"AND valor_transacao=@VALOR ");
            sql.Append($"AND qtde_parcelas=@QUANTIDADEPARCELAS ");

            return _context.Database.Connection.QueryFirstOrDefault<CartaoTransacao>(sql.ToString(),
                new
                {
                    AUTORIZACAO = autorizacao,
                    DATATRANSACAO = dataTransacao.Date,
                    VALOR = valor,
                    QUANTIDADEPARCELAS = quantidadeParcelas

                });
        }

        public List<CartaoTransacao> ObterPorNumeroCartaoENumeroAutorizacaoEDataHoraTransacao(string numeroCartao, string numeroAutorizacao, DateTime dataHoraTransacao)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("SELECT Id, ");
            sqlComando.Append($"id_transacao {nameof(CartaoTransacao.TransacaoId)}, ");
            sqlComando.Append($"origem {nameof(CartaoTransacao.Origem)} ");
            sqlComando.Append("FROM  topsys.con_cartao_transacao ");
            sqlComando.Append($" WHERE num_cartao LIKE '%{numeroCartao}%'");
            sqlComando.Append($" AND num_autorizacao='{numeroAutorizacao}'");
            sqlComando.Append($" AND YEAR(data_hora_transacao)={dataHoraTransacao.ToString("yyyy")}");

            return _context.Database.Connection.Query<CartaoTransacao>(sqlComando.ToString()).ToList();
        }
    }
}

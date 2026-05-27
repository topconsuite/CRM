using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICartaoTransacaoRepository
    {
        CartaoTransacao ObterCartaoTransacaoPeloTransacaoId(string transacaoId);
        void AtualizaErroNaGeracaoContasAReceber(string mensagem, string transacaoId);
        void AtualizaSucessoNaGeracaoContasAReceber(string transacaoId);
        CartaoTransacao ObterPorDataNumeroCartaoAutorizacao(DateTime dataTransacao, int numeroCartao, string autorizacao);
        void Adicionar(CartaoTransacao cartaoTransacao);
        void RemoverPorId(int cartaoTransacaoId, string userName = "");
        CartaoTransacao ObterPorDataNumeroCartaoAutorizacaoValorQuantidadeParcelas(DateTime dataTransacao, int numeroCartao, string autorizacao, float valor, int quantidadeParcelas);
        List<CartaoTransacao> ObterPorNumeroCartaoENumeroAutorizacaoEDataHoraTransacao(string numeroCartao, string numeroAutorizacao, DateTime dataHoraTransacao);
    }
}

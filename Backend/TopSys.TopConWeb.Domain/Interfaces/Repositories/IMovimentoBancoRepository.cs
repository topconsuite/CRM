using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IMovimentoBancoRepository
    {
        void Adicionar(MovimentoBanco movimentoBanco);
        MovimentoBanco ObterPorControle(long controle);
        void RemoverNaoConciliadoPorControle(long controle, string userName = "");
        void AtualizarNaoConciliado(MovimentoBanco movimentoBanco);
        IEnumerable<MovimentoBanco> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, DateTime? dataOperacao);

        long AdicionarERetornaId(MovimentoBanco movimentoBanco);
        void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco, bool naoRemoverMovimentoConciliado = true);
        double? ValorMovimentoDeBanco(long idMovimentoBanco);
        void DescontaValorMovimentosDeBancoNaoConciliado(float? valor, long idMovimentoBanco);
    }
}

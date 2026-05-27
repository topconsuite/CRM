using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IContratoPagamentoRepository: IRepositoryBase<ContratoPagamento>
    {
        void AtualizarIdAprovacao(ContratoPagamento contratoPagamento);
        void AtualizarIdAprovacao(ContratoPagamentoVersao contratoPagamento);
        ContratoPagamento ObterContratoPagamentoDetalhado(int contratoUsina, int contratoAno, 
            int contratoNumero, int pagamentoSequencia, bool tracking = false);
        ContratoPagamentoVersao ObterContratoPagamentoDetalhado(int numeroVersao, int contratoUsina, int contratoAno,
            int contratoNumero, int pagamentoSequencia, bool tracking = false);
        IEnumerable<ContratoPagamento> ListarContratoPagamentosDetalhados(int contratoUsina, int contratoAno,
            int contratoNumero, bool tracking = false);
        IEnumerable<ContratoPagamentoVersao> ListarContratoPagamentosDetalhados(int numVersao, int contratoUsina, 
            int contratoAno, int contratoNumero, bool tracking = false);
        void CarregarDetalhes(ContratoPagamento contratoPagamento, bool tracking = false);
        void CarregarDetalhes(ContratoPagamentoVersao contratoPagamento, bool tracking = false);
        ContratoPagamentoDetalheCheque ObterDetalheCheque(ContratoPagamento contratoPagamento, int detalheSequencia, bool tracking = false);
        ContratoPagamentoDetalheChequeVersao ObterDetalheCheque(ContratoPagamentoVersao contratoPagamento, int detalheSequencia, bool tracking = false);
        TDetalhe BuscarDetalhes<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;
        TDetalhe BuscarDetalhesVersao<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, int numVersao = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;
        IEnumerable<TDetalhe> ListarDetalhes<TDetalhe>(string forma, Expression<Func<TDetalhe, bool>> filter, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;
        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao);
        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato);
    }
}

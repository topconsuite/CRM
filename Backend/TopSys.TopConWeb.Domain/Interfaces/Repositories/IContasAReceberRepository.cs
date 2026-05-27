using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IContasAReceberRepository
    {
        void InsereContasAReceber(ContasAReceber contasAReceber);
        void AtualizarAlocadoContasAReceber(ContasAReceber contasAReceber);
        void AtualizarIdMovimentoBanco(ContasAReceber contasAReceber);
        ContratoPagamentoDetalheCartao ObterContratoPagamentoDetalheCartao(CartaoTransacao cartaoTransacao);
        IEnumerable<ContasAReceber> ListarContasAReceberPeloNumeroCartaoAutorizacaoEDataTransacao(string cartaoNumero, string autorizacaoNumero, DateTime dataTransacao);
        IEnumerable<ContasAReceber> ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(string cartaoNumero, string autorizacaoNumero, int anoTransacao);
        void VincularContasAReceberComMovimentoBanco(ContasAReceber contasAReceber, MovimentoBanco movimentoBanco, float valorVinculo);
        IEnumerable<ContasAReceber> ListarContasAReceberDeCartaoVinculado(ContratoPagamentoDetalheCartao contratoPagamentoDetalheCartao, int empresaCodigo);
        IEnumerable<TituloContasAReceber> ListarContasAReceberCartaoASeremDesaprovados(int empresaCod, int usina, string sequencia, long numeroDocumento);
        void DeletarContasAReceberDoCliente(int empresa, int usina, long documentoNumero, string sequencia);
        void DeletarContasAReceberDeCartaoDoCliente(int empresa, int usina, long documentoNumero, string sequencia);
        void DeletarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(string numeroCartao, string numeroAutorizacao, int anoTransacao, EDocumentoTipo documentoTipo);
        void DeletarContasAReceberTipoCheque(ContratoPagamentoDetalheCheque detalheCheque);
        void DeletarContasAReceberTipoCheque(ContratoPagamentoDetalheChequeVersao detalheCheque);
        void AtualizarAlocadoContasAReceberPorCartaoEAutorizacao(string numCartao, string autorizacao, int anoDataEmissao, EContasAReceberStatusAlocado eContasAReceberStatusAlocado);
    }
}

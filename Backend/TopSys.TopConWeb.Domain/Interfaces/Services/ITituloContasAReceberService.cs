using System;
using System.Collections.Generic;
using System.ComponentModel;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITituloContasAReceberService : IServiceBase<TituloContasAReceber>
    {
        IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao);
        IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao);


        // FOR PUBLIC INTEGRATIONS
        List<string[]> ValidaCamposRequestAdicionarTituloContasAReceber(int? empresa, int? documentoTipo, int? cliente, int? operacaoCodigo, int? centroCusto, int? situacao, int? BancoPortador, int? operacaoLiquidacao, int? BancoLiquidacao, DateTime? dataLiquidacao);
        List<string[]> ValidaCamposRequestAtualizarTituloContasAReceber(int? empresa, int? documentoTipo, int? cliente, int? operacaoCodigo, int? centroCusto, int? situacao, int? BancoPortador, int? operacaoLiquidacao, int? BancoLiquidacao, DateTime? dataLiquidacao);
        List<string[]> ValidaSaldoAdicionarRequest(float? valor, float? recebimentos);
        List<string[]> ValidaSaldoAtualizarRequest(float? valor, float? recebimentos, float? valorOriginal, float? recebimentosOriginal);
        TituloContasAReceber ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        ICollection<TituloContasAReceber> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        TituloContasAReceber ObterPorOriginalIdReguaDeCobranca(string id);
        ICollection<TituloContasAReceber> Listar(DateTime? dataEmissao, DateTime? dataOperacao, int tipoDocumento, int? centroCusto, string serieDocumento, long? numeroDocumento, int cliente, int pagina = 0, int limite = 0);
        PagedList<TituloContasAReceber> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);

        Tuple<long, long> RetornaParametrosMovimento(int? operacaoMovimentoBancario);
        int? RetornaBancoDeLiquidacao(int? operacaoLiquidacao);
        int? RetornaDesdobramentoMaximo(int? empresaCodigo, int? documentoTipoCodigo, string documentoSerie, long? documentoNumero, string documentoSequencia);
        int? RetornaOperacaoMovimentoBancario(int? operacaoBaixa);
        int? RetornaOperacaoBaixa(int? operacaoLiquidacao);
        List<string[]> ValidaBancoLiquidacao(int? empresa, int? operacaoLiquidacao, int? bancoLiquidacao);
        string ObterLinkSegundaViaBoleto(int codigoBanco);
        List<string[]> ValidaValoresLiquidacao(float? valor, float? liquidacaoValorRecebido, float? liquidacaoJuros, float? liquidacaoDesconto, float? liquidacaoDespesas);
        Tuple<int, int> RetornaTipoMovimentoEBaixa(int empresa);
        string DefineTipoLiquidacao(float? valor, float? valorRecebidoLiquidacao, float? somaRecebimentos);

        void CriaEAtualizaVinculoMovimentoTitulo(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int? documentoSequencia, int desdobramento, long documentoNumero, float valor, long interveniente, long idMovimentoBancario);
        float CalculoRateio(float liquidacaoDespesas, float liquidacaoJuros, float liquidacaoDesconto, int empresaCodigo, int documentoTipoCodigo, string documentoSerie, long documentoNumero, int documentoSequencia, int codBancoBand, int numAgencia, long numConta, byte numContaDv, float valor, DateTime? dataVencimento);
        void GeraMoraNaoLiquidada(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int numeroDocumento, int? documentoSequencia, int desdobramento, float valorMoraNaoLiquidada, DateTime dataEmissao, long interveniente);
        long AdicionarMovimento(MovimentoBanco movimentoBanco);
        List<string[]> ValidaValoresRecebimento(float? somaRecebimentos, float? valor, float? liquidacaoTotalRecebido);
        TipoDeCobranca RetornaTipoDeCobrancaComDescricao(int codTipoCobranca);
        bool DentroDoMesFechamento(int empresa, DateTime? dataLiquidacao);
        bool CancelamentoParcialDeRecebimentoEmCartao(long lote);
        bool CancelamentoParcialDeRecebimentoEmCheque(long lote);
        bool ExisteTituloDevolucaoEmCheque(int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        bool ExisteLiquidacaoGeradoNoContasAPagar(long lote, int empresa, int clienteDocumento, string serieDocumento, int numeroDocumento, double liquidacaoValorRecebidoDocumento);
        bool ExisteMovimentoDeBancoConciliado(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long IdMovimentoBanco);
        bool ExisteBaixasDoContasAPagar(long lote);
        bool ExisteChequeLiquidadoRelacionadoABaixaDoLote(long lote);
        bool ExisteCreditoCompensadoNaGeracaoDaLiquidacao(long lote);

        void AjustaDescontoDeMora(long loteBaixa, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento);
        void RemovePendenciaCobranca(long loteBaixa);
        void RemoveDesdobramentos(long loteBaixa, int sequencia, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento);
        void RemoveMovimentoDeBancoNaoConciliadoDeTituloDeCredito(int empresa, long loteBaixa, long clienteDocumento);
        void AjustaMovimentosComCartaoDeCredito(long lote);
        void RemoveTitulo(long lote);
        void RemoveTituloDeMora(int empresa, int tipoDocumento, string serieDocumento, int cliente, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void RemoveTituloDeCredito(long loteBaixa, int empresa, long clienteDocumento);
        void RemoveTituloDeCreditoContasAPagar(long loteBaixa, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, double valorRecebido);
        void RemoveMovimentoDeBancoNaoConciliado(int idMovimentoDeBanco, int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        void AjustaSaldoDoTituloPrincipal(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void AjustaDescontoDeMoraDoTituloPrincipal(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        List<string[]> ValidaCancelamentoDeTitulo(TituloContasAReceber tituloContasAReceber);
        void CancelarRecebimentoDeTitulo(TituloContasAReceber tituloContasAReceber);
        
        Segmentacao RetornaSegmentacao(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, long numeroDocumento);

    }
}

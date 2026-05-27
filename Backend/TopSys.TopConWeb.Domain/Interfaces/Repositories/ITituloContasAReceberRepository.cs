using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITituloContasAReceberRepository : IRepositoryBase<TituloContasAReceber>
    {
        IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao);
        IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao);

        bool VerificaSeExisteEmTabelasRelacionadas(string fieldValue, string fieldName, string tableName);
        TituloContasAReceber ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, bool dapper = false);
        ICollection<TituloContasAReceber> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        ICollection<TituloContasAReceber> ListarComPaginacao(DateTime? dataEmissao, DateTime? dataOperacao, int? tipoDocumento,int? centroCusto,string serieDocumento, long? numeroDocumento, int? cliente, int pagina, int limite);
        Tuple<long, long> RetornaParametrosDeMovimentoBancario(int? operacaoMovimentoBancario);
        int? RetornaBancoDeLiquidacao(int? operacaoLiquidacao);
        int? RetornaDesdobramentoMaximo(int? empresaCodigo, int? documentoTipoCodigo, string documentoSerie, long? documentoNumero, string documentoSequencia);
        int? ValidaOperacaoMovimentoBancario(int? operacaoLiquidacao);
        int? ValidaOperacaoBaixa(int? operacaoBaixa);
        bool? ValidaBancoLiquidacao(int? empresa, int? operacaoLiquidacao, int? bancoLiquidacao);
        string ObterLinkSegundaViaBoleto(int codigoBanco);
        Tuple<int, int> RetornaTipoMovimentoEBaixa(int empresa);
        void CriaEAtualizaVinculoMovimentoTitulo(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int? documentoSequencia, int desdobramento, long documentoNumero, float valor, long interveniente, long idMovimentoBancario);
        float CalculoRateio(float liquidacaoDespesas, float liquidacaoJuros, float liquidacaoDesconto, int empresaCodigo, int documentoTipoCodigo, string documentoSerie, long documentoNumero, int documentoSequencia, int codBancoBand, int numAgencia, long numConta, byte numContaDv, float valor, DateTime? dataVencimento);
        void GeraMoraNaoLiquidada(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int numeroDocumento, int? documentoSequencia, int desdobramento, float valorMoraNaoLiquidada, DateTime dataEmissao, long interveniente);
        PagedList<TituloContasAReceber> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        bool ExisteTituloRecebimentoEmCartao(long loteBaixa);
        bool ExisteTituloRecebimentoEmCheque(long loteBaixa);
        int QuantidadeTitulosDeCredito(long loteBaixa);
        bool ExisteTituloDevolucaoEmCheque(int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        bool ExisteMovimentoDeBancoConciliado(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long idMovimentoBanco);
        bool ExisteChequeLiquidadoRelacionadoABaixaDoLote(long loteOrigem, int desdobramento);
        bool ExisteCreditoCompensadoNaGeracaoDaLiquidacao(long loteOrigem);
        void AjustaDescontoDeMora(long loteBaixa, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento);
        void RemovePendenciaDeCobranca(long loteBaixa);
        void RemoveDesdobramentos(long loteBaixa, int sequencia, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento);
        List<TituloContasAReceber> ObterTitulosDeCredito(int empresa, long loteBaixa, long clienteDocumento);
        (long idMovimento, float valor) ObterVinculosMovimentosDeBanco(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long idMovimentoBanco = 0);
        void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco);
        List<TituloContasAReceber> ObterTituloRecebimentoEmCartao(long loteBaixa);
        void DesvinculaTituloCartaoDeCredito(string numeroCartao, string numeroAutorizacao, DateTime dataEmissao);
        void RemoveTitulo(long loteOrigem);
        void RemoveTituloDeMora(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void RemoveTituloDeCredito(long loteOrigem, int empresa, long clienteDocumento);
        DateTime? ObterDataMaximaDesdobramentos(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        float? ObterSomatorioRecebimentos(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void AtualizarRecebimentosTituloPrincipal(float? valorRecebimentos, DateTime? dataMora, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        float ObterValorOcorrenciaDeDescontoMora(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void AtualizarDescontoMora(float valor, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco, int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        void RemoveVinculoIdMovimentosDeBanco(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
    }
}

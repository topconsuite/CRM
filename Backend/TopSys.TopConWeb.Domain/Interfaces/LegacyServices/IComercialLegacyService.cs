using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.LegacyServices
{
    public interface IComercialLegacyService
    {
        bool VerificarFraude(int intervenienteCodigo, string enderecoLogradouro, int enderecoNumero,
            string enderecoCobrancaLogradouro, int enderecoCobrancaNumero, string enderecoFaturamentoLogradouro,
            int enderecoFaturamentoNumero, string enderecoObraLogradouro, int enderecoObraNumero, int usina,
            int obraNumero, out string aguardandoAprovacao, out string mensagemRetorno);

        void FinalizarAprovacoesComerciais(string usuario, string chaveObra, List<ObraLogDado> logs);
        void FinalizarAprovacoesComerciaisVersao(string usuario, string chaveObra, List<ObraLogDado> logs);
        string FinalizarRevalidacaoCadastro(string usuario, Contrato contrato, string observacaoLog);
        bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out Contrato contrato, out string mensagem);
        PagedList<IQueryResult> ConsultarObras(IFilter filtro, int pagina, int porPagina, string usuario, string ordenacao = "");
        PagedList<IQueryResult> ConsultarObrasVersao(IFilter filtro, int pagina, int porPagina, string Usuario);
        bool ValidarContrato(Contrato contrato, string usuario, out string mensagensRetorno, bool aprovarAutomaticamente = false);
        bool ValidarContrato(ContratoVersao contrato, string usuario, out string mensagensRetorno, bool aprovarAutomaticamente = false);
        bool ValidarAprovacaoCadastro(Contrato contrato, out string mensagens);
        bool ValidarAprovacaoCadastro(ContratoVersao contrato, out string mensagens);
        string ValidarPagamentosContrato(Contrato contrato, bool verificaAprovacaoFinanceira, out string mensagens);
        string ValidarPagamentosContrato(ContratoVersao contrato, bool verificaAprovacaoFinanceira, out string mensagens);
        bool ValidarAprovacaoDistanciaUsinaEntregaCep(int usinaEntrega, string cep);
        bool ValidarCodigoObraPrefeitura(int usina, string codigoObraPrefeitura, int contratoNumero = 0, int contratoAno = 0);

        void VerificaRegrasAlteracaoTraco(int propostaUsina, int numeroObra, int sequenciaProposta,
            string idAlteracaoTracoProposta, int codigoInterveniente, int uso, int pedra, int slump,
            int tipoResistencia, float fck, int consumo, float quantidadeM3,
             int pUso = 0, int pPedra = 0, int pSlump = 0, int pTipoResistencia = 0, int pConsumo = 0, float pFck = 0 );
        void VerificaRegrasAlteracaoTraco(int numVersao, int propostaUsina, int numeroObra, int sequenciaProposta,
            string idAlteracaoTracoProposta, int codigoInterveniente, int uso, int pedra, int slump,
            int tipoResistencia, float fck, int consumo, float quantidadeM3,
             int pUso = 0, int pPedra = 0, int pSlump = 0, int pTipoResistencia = 0, int pConsumo = 0, float pFck = 0);

        bool VerificaRegrasAprovacaoEngenharia(ObraTraco traco, int codigoInterveniente, string tipoCliente = "");

        bool VerificaRegrasAprovacaoEngenharia(ObraTracoVersao traco, int codigoInterveniente, string tipoCliente = "");

        void DesaprovarCondicaoPagamento(int contratoUsina, int contratoAno, int contratoNumero, 
            int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = false);
        void DesaprovarCondicaoPagamento(int numeroVersao, int contratoUsina, int contratoAno, int contratoNumero,
            int pagamentoSequencia, string usuario, bool verificaMovimentoDeBancoConciliado = false);

        void TotalizarValoresProgramacao(Programacao programacao);
        bool EhDomingoOuFeriado(DateTime data, int municipioCodigo);
        bool ExcluirFinCar(ContratoPagamento contratoPagamento, bool verificaMovimentoDeBancoConciliado = true);
        bool ExcluirFinCar(ContratoPagamentoVersao contratoPagamento, bool verificaMovimentoDeBancoConciliado = true);
        void ExcluirMovimentoDeBancoVinculadoContrato(int contratoUsinaCodigo, long contratoNumero, int contratoAno, string sequencia, bool verificaMovimentoDeBancoConciliado);
        bool VerificaMovimentoBancoConciliado(TituloContasAReceber contasAReceber);

        bool RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, string observacao, string usuario);

        bool GeraProgramacao(int idUsina, int obraNumero, int sequencia, bool atualizaComplexidadeBombeado, bool gravaContinuidadeProgramacao, string usuario);

        bool TemComplexidadeBombeado(Programacao programacao);

        string VerificaContinuidade(Programacao programacao);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IObraService : IServiceBase<Obra>
    {

        PagedList<Obra> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter);
        void Adicionar(Obra obra, float valorExtras);

        void Adicionar(ObraVersao obra, float valorExtras);

        void Adicionar(DemaisAprovacao demaisAprovacao);

        void Adicionar(AprovacaoScript aprovacaoScript);

        float ObterValoresAdicionaisObraTraco(ObraTraco obraTraco, int usinaEntregaCodigo, DateTime dataBase,
            string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, out TracoPreco tracoPreco, Obra obra);

        void ValidarObraTraco(string usuario, ObraTraco obraTraco, int usinaEntregaCodigo, DateTime dataBase,
            string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, float percentualDescontoLimite,
            Obra obra, ref bool aprovacaoComercialPendente, ref bool hasNotifications);

        void ValidarObraTraco(string usuario, ObraTracoVersao obraTraco, int usinaEntregaCodigo, DateTime dataBase,
            string obraCep, int distanciaUsina, int condicaoPagamentoCodigo, float percentualDescontoLimite,
            ObraVersao obra, ref bool aprovacaoComercialPendente, ref bool hasNotifications);

        void ValidarObraBomba(string usuario, ObraBomba obraBomba, int usinaEntregaCodigo, DateTime dataBase,
            ref bool aprovacaoComercialPendente, ref bool hasNotifications);

        void ValidarObraBomba(string usuario, ObraBombaVersao obraBomba, int usinaEntregaCodigo, DateTime dataBase,
           ref bool aprovacaoComercialPendente, ref bool hasNotifications);

        void ValidarObraTaxa(string usuario, ObraTaxa obraTaxa, ref bool aprovacaoComercialPendente);

        void ValidarObraTaxa(string usuario, ObraTaxaVersao obraTaxa, ref bool aprovacaoComercialPendente);

        IEnumerable<Obra> ListaPendentesDeAprovacao(string usuario);

        bool TemAprovacaoPendente(int usina, int numero, int anoChamada, int noChamada);

        //TODO: Renomear método para o infinitivo Obter
        Obra ObtemPendentePorId(int usina, int numero, int anoChamada, int noChamada, string usuario);

        ObraVersao ObtemPendentePorId(int numVersao, int usina, int numero, int anoChamada, int noChamada, string usuario);

        void AprovarObraPendente(string usuario, Obra obra);

        void AprovarObraPendente(string usuario, ObraVersao obra);

        void FinalizarAprovacaoObraPendente(string usuario, Obra obra, bool logVolume = false, bool logDemaisCondicao = false);

        void FinalizarAprovacaoObraPendente(string usuario, ObraVersao obra, bool logVolume = false, bool logDemaisCondicao = false);

        IEnumerable<ObraLog> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada);

        IEnumerable<ObraLogVersao> ListarObraLogsPorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada);

        int ObterTempoDescarga(int idUsina);

        void AtualizarEnderecoProgramacoesFuturas(int usina, int obraNumero);

        void AtualizarValoresProgramacoesFuturas(int usina, int obraNumero);

        void AtualizarStatusComercial(int usina, int obraNumero);

        void AtualizarStatusComercial(int usina, int obraNumero, int numVersao);

        void AtualizarStatusComercial(Obra obra, bool utilizaAprovacaoPorAlcada = false);

        void AtualizarStatusComercial(ObraVersao obra, bool utilizaAprovacaoPorAlcada = false);

        void AtualizarStatusEngenharia(Obra obra);

        void AtualizarStatusEngenharia(ObraVersao obra);

        IEnumerable<Obra> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao);

        Obra ListarObraPagamentos(int idUsina, int obraNumero);

        ObraVersao ListarObraPagamentos(int numVersao, int idUsina, int obraNumero);

        void AprovarObraPagamentos(string usuario, Obra obra, IEnumerable<MovimentoBanco> movimentosBancoAVincular);

        void AprovarObraPagamentos(string usuario, ObraVersao obra, IEnumerable<MovimentoBanco> movimentosBancoAVincular);

        void AtualizarStatusFinanceiro(Obra obra, string usuario);

        void AtualizarStatusFinanceiro(ObraVersao obra, string usuario);

        Obra ListarObraTracos(int idUsina, int obraNumero, bool tracking = false);

        ObraVersao ListarObraTracos(int numVersao, int idUsina, int obraNumero);

        Obra ListarObraBombas(int idUsina, int obraNumero, bool tracking = false);

        ObraVersao ListarObraBombas(int numVersao, int idUsina, int obraNumero);

        void AprovarEngenharia(string usuario, Obra obra);

        void AprovarEngenharia(string usuario, ObraVersao obra);

        Obra ObterObraParaAnaliseCadastro(int idUsina, int obraNumero);

        ObraVersao ObterObraParaAnaliseCadastro(int numVersao, int idUsina, int obraNumero);

        void AprovarDistanciaUsinaCep(string usuario, Obra obra);

        void AlterarStatusCadastroEAnalista(Obra obra, string observacao, string usuario);

        void AlterarStatusCadastroEAnalista(ObraVersao obra, string observacao, string usuario);

        void TentarAprovarCadastroEContrato(Contrato contrato, string usuario, bool adicionarNotificacaoCadastro, bool adicionarNotificacaoContrato = false);

        void TentarAprovarCadastroEContrato(ContratoVersao contrato, string usuario, bool adicionarNotificacaoCadastro, bool adicionarNotificacaoContrato = false);

        void AprovarAutomaticamentePagamentosDaCieloLio(Obra obra);

        void AprovarAutomaticamenteContratoPagamentosDaCieloLio(ContratoPagamento contratoPagamento, Obra obra);

        TDetalhe BuscarDetalhes<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;

        TDetalhe BuscarDetalhesVersao<TDetalhe>(string forma, int contratoUsina, int contratoAno, int contratoNumero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0, int numVersao = 0, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;

        IEnumerable<TDetalhe> ListarDetalhes<TDetalhe>(string forma, Expression<Func<TDetalhe, bool>> filter, bool tracking = false) where TDetalhe : ObraPagamentoDetalhe;

        float CalcularImpostosAplicadoEstadual(ObraTraco obraTraco, CustoServico custoServico, Obra obra);
        float CalcularCustoValorBombaPorM3(ObraTraco obraTraco, Obra obra);
        void CalcularEbitdaObraTraco(ObraTraco obraTraco, Obra obra);
        void CalcularEbitdaObraTraco(ObraTracoVersao obraTraco, ObraVersao obra);
        void CalcularEbitdaObraBomba(ObraBomba obraBomba, Obra obra);
        void CalcularEbitdaObraBomba(ObraBombaVersao obraBomba, ObraVersao obra);
        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);
        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);        
        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);
        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato, int usinaProposta, int anoProposta, int numeroProposta, int numObra);
        float? ObterConsumoPorTraco(int numeroContrato, int anoContrato, string traco, int interveniente);
        float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato);
        void AlterarMensagemObraReajuste(int codUsina, int codObra, string mensagem);
        void AlterarMensagemObraReajusteVersao(int numVersao, int codUsina, int codObra, string mensagem);
        int ObterUltimaVersaoObra(int obraUsina, int obraNumero);
        Obra ObterPorIdAprovacaoComercial(int usina, int numero);
        ObraVersao ObterPorIdAprovacaoComercial(int usina, int numero, int versao);
        Obra ObterObraPorContrato(int codUsina, int anoContrato, int numeroContrato, bool tracking = false);

        bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero);
        bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero, int obraVersao);

        void AtualizaObraTracoReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, float m3PrecoProposto, float valorServico, float descontoPercentual, ContratoTracoReajuste tracoReajuste);
        void AtualizaObraBombaReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, int m3PropostoAte, float taxaMinimaPrecoProposto, float m3PrecoProposto, float descontoPercentual, ContratoBombaReajuste bombaReajuste);

        IEnumerable<ObraProjecao> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada);

        float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);

        float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada);
        float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);
        float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato);
        void AtualizarDadosReajuste(ObraTraco obraTraco);
        void AtualizarDadosReajuste(ObraBomba obraBomba);

        void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, float valorReajustado, float valorServico, float descontoPercentual);
        void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, int m3ReajustadoAteAtual, float taxaMinimaReajustadaAtual, float m3PrecoReajustadoAtual, float descontoPercentual);
    }
}

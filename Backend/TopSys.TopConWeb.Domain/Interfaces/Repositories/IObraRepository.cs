using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IObraRepository : IRepositoryBase<Obra>
    {

        void DetectEntry(Obra obra);

        PagedList<Obra> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter);
        void Adicionar(Obra obra, float valorExtras);

        void Adicionar(ObraVersao obra, float valorExtras);

        void Adicionar(DemaisAprovacao demaisAprovacao);

        void Adicionar(AprovacaoScript aprovacaoScript);

        IEnumerable<Obra> ListarComTracoPendenteDeAprovacao(string usuario);

        IEnumerable<ObraVersao> ListarComTracoPendenteDeAprovacao(int numVersao, string usuario);

        IEnumerable<Obra> ListarComBombaPendenteDeAprovacao(string usuario);

        IEnumerable<Obra> ListarComTaxaExtraPendenteDeAprovacao();

        IEnumerable<ObraVersao> ListarComTaxaExtraPendenteDeAprovacao(int numVersao);

        IEnumerable<Obra> ListarComDemaisAprovacoesPendentes(string usuario);

        IEnumerable<Obra> ListarPorCliente(int codCliente);

        IEnumerable<ObraVersao> ListarComDemaisAprovacoesPendentes(int numVersao, string usuario);

        int ObterTempoDescarga(int idUsina);

        bool TemAprovacaoTracoPendente(int usina, int numero, int anoChamada, int noChamada);

        bool TemAprovacaoTracoPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada);

        bool TemAprovacaoBombaPendente(int usina, int numero, int anoChamada, int noChamada);

        bool TemAprovacaoBombaPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada);

        bool TemAprovacaoTaxaExtraPendente(int usina, int numero, int anoChamada, int noChamada);

        bool TemAprovacaoTaxaExtraPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada);

        bool TemDemaisAprovacoesPendentes(int usina, int numero, int anoChamada, int noChamada);

        bool TemDemaisAprovacoesPendentes(int numVersao, int usina, int numero, int anoChamada, int noChamada);

        Obra ObterPendentePorId(int usina, int numero, int? anoChamada, int? noChamada);

        ObraVersao ObterPendentePorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada);

        void AtualizarObraPendente(Obra obra);

        void AtualizarObraPendente(ObraVersao obra);

        IEnumerable<ObraLog> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada);

        IEnumerable<ObraLogVersao> ListarObraLogsPorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada);

        void AtualizarEnderecoProgramacoesFuturas(int usina, int obraNumero);

        bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero);

        bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero, int obraVersao);

        void AtualizarStatusComercial(int usina, int obraNumero);

        void AtualizarStatusComercial(int usina, int obraNumero, int numVersao);

        void AtualizarStatusComercial(Obra obra, EObraStatusComercial statusComercial);

        void AtualizarStatusComercial(ObraVersao obra, EObraStatusComercial statusComercial);

        IEnumerable<Obra> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao);

        Obra ListarObraPagamentos(int idUsina, int obraNumero);

        ObraVersao ListarObraPagamentos(int idUsina, int obraNumero, int numVersao);

        Obra ListarObraTracos(int idUsina, int obraNumero, bool tracking = false);

        Obra ListarObraBombas(int idUsina, int obraNumero, bool tracking = false);

        ObraVersao ListarObraBombas(int numVersao, int idUsina, int obraNumero);

        ObraVersao ListarObraTracos(int numVersao, int idUsina, int obraNumero);

        Obra ObterObraParaAnaliseCadastro(int idUsina, int obraNumero);

        ObraVersao ObterObraParaAnaliseCadastro(int numVersao, int idUsina, int obraNumero);

        void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);

        void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);

        void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra);

        void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato, int usinaProposta, int anoProposta, int numeroProposta, int numObra);
        float? ObterConsumoPorTraco(int numeroContrato, int anoContrato, string traco, int interveniente);
        float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato);
        void AlterarMensagemObraReajuste(int codUsina, int codObra, string mensagem);
        void AlterarMensagemObraReajusteVersao(int numVersao, int codUsina, int codObra, string mensagem);

        int ObterUltimaVersaoObra(int obraUsina, int obraNumero);
        Obra ObterPorIdAprovacaoComercial(int usina, int numero, bool tracking = true);
        ObraVersao ObterPorIdAprovacaoComercial(int usina, int numero, int versao, bool tracking = true);
        int ObterSegmentacaoPropostaPorObra(int usinaEntregaCodigo, int obraNumero);

        Obra ObterObraPorContrato(int codUsina, int anoContrato, int numeroContrato, bool tracking = false);

        void AtualizaObraTracoReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, float m3PrecoProposto, float valorServico, float descontoPercentual, ContratoTracoReajuste tracoReajuste);
        void AtualizaObraBombaReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, int m3PropostoAte, float taxaMinimaPrecoProposto, float m3PrecoProposto, float descontoPercentual, ContratoBombaReajuste bombaReajuste);

        IEnumerable<ObraProjecao> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada);
        float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);

        float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada);

        float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato);
        float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato);
        void AtualizarDadosReajuste(ObraTraco obraTraco);
        void AtualizarDadosReajuste(ObraBomba obraBomba);

        int ObterCondicaoPagamentoPorContrato(int usina, int anoContrato, int numeroContrato, int versao);

        void ObterStatusObra(int obraUsina, int obraNumero, int versao, out int obraStatusCadastro, out int obraStatusComercial, out int statusContrato);
        void alterarStatusContratoPelaObra(int obraUsina, int obraNumero, int obraVersao, int novoStatus);

        void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, float valorReajustado, float valorServico, float descontoPercentual);
        void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, int m3ReajustadoAteAtual, float taxaMinimaReajustadaAtual, float m3PrecoReajustadoAtual, float descontoPercentual);
        void AtualizarTracoAtivoPropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, string ativo);
    }
}

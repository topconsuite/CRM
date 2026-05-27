using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IAprovacaoComercialPendenteService : IServiceBase<AprovacaoComercialPendente>
    {
        void ForcarAprovacaoRegistrosAlcada(int obraUsina, int obraNumero, int obraVersao);
        void RemoverVestigiosAprovacoesAnteriores(int obraUsina, int obraNumero, int obraVersao, int excluirAcimaNivelAutoridade = 0);
        IEnumerable<AprovacaoComercialPendente> ListarTodasAprovacoesPorObraVersao(int obraUsina, int obraNumero, int obraVersao);
        IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentePorObraVersao(int obraUsina, int obraNumero, int obraVersao);

        void ValidarComercialObra(int obraUsina, int obraNumero, string usuario);

        EStatusAprovacao AtualizarAprovacaoAlcadaTraco(Proposta proposta, Obra obra, ObraTraco newTraco);
        EStatusAprovacao AtualizarAprovacaoAlcadaBomba(Proposta proposta, Obra obra, ObraBomba newBomba);
        EStatusAprovacao AtualizarAprovacaoAlcadaVolume(Proposta proposta, Obra obra);
        EStatusAprovacao AtualizarAprovacaoAlcadaCondicaoPagamento(Proposta proposta, Obra obra);

        void RemoverAprovacaoAlcadaTraco(Proposta proposta, ObraTraco oldTraco);
        void RemoverAprovacaoAlcadaBomba(Proposta proposta, ObraBomba oldBomba);
        void RemoverAprovacaoAlcadaVolume(Proposta proposta);
        void RemoverAprovacaoAlcadaCondicaoPagamento(Proposta proposta);

        void ValidaAprovacoesObra(string usuario, Obra obra);

        void RevisarAprovacaoComercialPendente(Obra obra, List<AprovacaoComercialPendente> pendentes, Proposta proposta = null);
        void RevisarAprovacaoComercialPendenteVersao(ObraVersao obra, List<AprovacaoComercialPendente> pendentes, PropostaVersao proposta = null);

        AprovacaoComercialPendente ObterAprovacaoPendentePorObraVersaoNivelHierarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia);
        AprovacaoComercialPendente ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia);

        bool UsuarioJaRealizouAprovacaoPendenteTracoDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaTraco, string usuarioId);
        bool UsuarioJaRealizouAprovacaoPendenteBombaDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaBomba, string usuarioId);

        void ValidarComercialObraVersao(int obraUsina, int obraNumero, int obraVersao, string usuario);

        EStatusAprovacao AtualizarAprovacaoAlcadaTracoVersao(PropostaVersao proposta, ObraVersao obra, ObraTracoVersao newTraco);
        EStatusAprovacao AtualizarAprovacaoAlcadaBombaVersao(PropostaVersao proposta, ObraVersao obra, ObraBombaVersao newBomba);
        EStatusAprovacao AtualizarAprovacaoAlcadaVolumeVersao(PropostaVersao proposta, ObraVersao obra);
        EStatusAprovacao AtualizarAprovacaoAlcadaCondicaoPagamentoVersao(PropostaVersao proposta, ObraVersao obra);

        void RemoverAprovacaoAlcadaTracoVersao(PropostaVersao proposta, ObraTracoVersao oldTraco);
        void RemoverAprovacaoAlcadaBombaVersao(PropostaVersao proposta, ObraBombaVersao oldBomba);
        void RemoverAprovacaoAlcadaVolumeVersao(PropostaVersao proposta);
        void RemoverAprovacaoAlcadaCondicaoPagamentoVersao(PropostaVersao proposta);

        void ValidaAprovacoesObraVersao(string usuario, ObraVersao obra);

        List<AprovacaoComercialPendente> FluxoAprovacaoWorkFlowSetarStatusAguardandoNoMenorNivel(List<AprovacaoComercialPendente> pendentes);

        void AdicionarAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente);
        void AdicionarAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente);
        void AdicionarAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente);
        void AdicionarAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente);


    }
}

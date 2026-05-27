using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial
{
    public interface IAprovacaoComercialPendenteRepository : IRepositoryBase<AprovacaoComercialPendente>
    {

        void ForcarAprovacaoRegistrosAlcada(int obraUsina, int obraNumero, int obraVersao);
        void RemoverVestigiosAprovacoesAnteriores(int obraUsina, int obraNumero, int obraVersao, int excluirAcimaNivelAutoridade = 0);
        IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentePorObraVersao(int obraUsina, int obraNumero, int obraVersao);
        AprovacaoComercialPendente ObterAprovacaoPendentePorObraVersaoNivelHierarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia);
        AprovacaoComercialPendente ObterAprovacaoReprovadoPorObraVersaoNivelHerarquia(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia);
        IEnumerable<AprovacaoComercialPendente> ListarTodasAprovacoesPorObraVersao(int obraUsina, int obraNumero, int obraVersao);
        bool UsuarioJaRealizouAprovacaoPendenteTracoDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaTraco, string usuarioId);
        bool UsuarioJaRealizouAprovacaoPendenteBombaDeObraVersao(int obraUsina, int obraNumero, int obraVersao, int nivelHierarquia, int sequenciaBomba, string usuarioId);

        void AdicionarAprovacoesPendentes(List<AprovacaoComercialPendente> pendentes);

        void RemoverAprovacaoPendente(AprovacaoComercialPendente pendente);
        void RemoverAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente);
        void RemoverAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente);
        void RemoverAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente);
        void RemoverAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente);

        void AdicionarAprovacaoPendenteTraco(AprovacaoComercialPendenteTraco pendente);
        void AdicionarAprovacaoPendenteBomba(AprovacaoComercialPendenteBomba pendente);
        void AdicionarAprovacaoPendenteVolume(AprovacaoComercialPendenteVolume pendente);
        void AdicionarAprovacaoPendenteCondicaoPagamento(AprovacaoComercialPendenteCondicaoPagamento pendente);

    }
}

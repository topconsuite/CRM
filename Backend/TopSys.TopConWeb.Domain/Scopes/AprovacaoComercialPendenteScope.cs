using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class AprovacaoComercialPendenteScope
    {

        public static bool TracoNecessitaAprovacaoComercialScope(this List<AprovacaoComercialPendente> pendentes, int tracoSequencia) => pendentes.Where(x => x.TracoNecessitaAprovacaoComercialScope(tracoSequencia)).Count() > 0;

        public static bool TracoNecessitaAprovacaoComercialScope(this AprovacaoComercialPendente pendente, int tracoSequencia)
        {

            if (pendente == null)
                return false;

            var tracoPendente = pendente
                .Tracos
                .Where(y => y.ObraSeq == tracoSequencia && y.PendenteAprovacaoComercial())
                .Count() > 0;

            return tracoPendente;
        }

        public static bool BombaNecessitaAprovacaoComercialScope(this AprovacaoComercialPendente pendente, int bombaSequencia)
        {

            if (pendente == null)
                return false;

            var bombaPendente = pendente
                .Bombas
                .Where(y => y.ObraSeq == bombaSequencia && y.PendenteAprovacaoComercial())
                .Count() > 0;

            return bombaPendente;
        }

        public static bool BombaNecessitaAprovacaoComercialScope(this List<AprovacaoComercialPendente> pendentes, int bombaSequencia) => pendentes.Where(x => x.BombaNecessitaAprovacaoComercialScope(bombaSequencia)).Count() > 0;
    

        public static bool VolumeNecessitaAprovacaoComercialScope(this AprovacaoComercialPendente pendente)
        {

            if (pendente == null)
                return false;

            var volumePendente = pendente
                .Volumes
                .Where(y => y.PendenteAprovacaoComercial())
                .Count() > 0;

            return volumePendente;

        }

        public static bool VolumeNecessitaAprovacaoComercialScope(this List<AprovacaoComercialPendente> pendentes) => pendentes.Where(x => x.VolumeNecessitaAprovacaoComercialScope()).Count() > 0;

        public static bool CondicaoPagamentoNecessitaAprovacaoComercialScope(this AprovacaoComercialPendente pendente)
        {

            if (pendente == null)
                return false;

            var CondicaoPagamentoPendente = pendente
                .CondicoesPagamento
                .Where(y => y.PendenteAprovacaoComercial())
                .Count() > 0;

            return CondicaoPagamentoPendente;

        }

        public static bool CondicaoPagamentoNecessitaAprovacaoComercialScope(this List<AprovacaoComercialPendente> pendentes) => pendentes.Where(x => x.CondicaoPagamentoNecessitaAprovacaoComercialScope()).Count() > 0;

    }
}

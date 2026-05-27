using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IObraProjecaoRepository : IRepositoryBase<ObraProjecao>
    {

        IEnumerable<ObraProjecao> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false);

        ObraProjecao ObterPorObraVolumePeriodo(int obraUsina, int obraNumero, int anoChamada, int noChamada, float volumeAnterior, DateTime periodoAnterior, bool tracking = false);
        void AtualizarProjecao(ObraProjecao obraProjecao, float volumeAnterior, DateTime periodoAnterior);

        float? ObterSaldoProjecaoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada);

        float? ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada);
        DateTime? GetProximoPeriodoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada);
        float GetSaldoProjecaoAnterior(int usina, int noObra, int? anoChamada, int? noChamada, DateTime periodo);

    }
}
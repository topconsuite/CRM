using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ObraProjecaoService : ServiceBase<ObraProjecao>, IObraProjecaoService
    {

        private readonly IObraProjecaoRepository _obraProjecaoRepository;

        public ObraProjecaoService(IObraProjecaoRepository ObraProjecaoRepository) : base(ObraProjecaoRepository)
        {
            _obraProjecaoRepository = ObraProjecaoRepository;
        }


        public IEnumerable<ObraProjecao> ListarPorObra(int obraUsina, int obraNumero, bool tracking = false)
        {
            return _obraProjecaoRepository.ListarPorObra(obraUsina, obraNumero, tracking);
        }

        public ObraProjecao ObterPorObraVolumePeriodo(int obraUsina, int obraNumero, int anoChamada, int noChamada, float volumeAnterior, DateTime periodoAnterior, bool tracking = false)
        {
            return _obraProjecaoRepository.ObterPorObraVolumePeriodo(obraUsina, obraNumero, anoChamada, noChamada, volumeAnterior, periodoAnterior, tracking);
        }

        public void AtualizarProjecao(ObraProjecao obraProjecao, float volumeAnterior, DateTime periodoAnterior)
        {
            _obraProjecaoRepository.AtualizarProjecao(obraProjecao, volumeAnterior, periodoAnterior);
        }

        public float? ObterSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            return _obraProjecaoRepository.ObterSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);
        }

        public float? ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            return _obraProjecaoRepository.ObterPrevisaoSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);
        }

        public DateTime? GetProximoPeriodoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada)
        {
            return _obraProjecaoRepository.GetProximoPeriodoPorContrato(usina, noObra, anoChamada, noChamada);
        }

        public float GetSaldoProjecaoAnterior(int usina, int noObra, int? anoChamada, int? noChamada, DateTime periodo)
        {
            return _obraProjecaoRepository.GetSaldoProjecaoAnterior(usina, noObra, anoChamada, noChamada, periodo);
        }
    }
}

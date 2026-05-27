using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ContratoBombaReajusteService : ServiceBase<ContratoBombaReajuste>, IContratoBombaReajusteService
    {
        private readonly IContratoBombaReajusteRepository _contratoBombaReajusteRepository;

        public ContratoBombaReajusteService(IContratoBombaReajusteRepository contratoBombaReajusteRepository) : base(contratoBombaReajusteRepository)
        {
            _contratoBombaReajusteRepository = contratoBombaReajusteRepository;
        }

        public IEnumerable<ContratoBombaReajuste> ListarContratoReajusteBombaPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia)
        {
            return _contratoBombaReajusteRepository.ListarContratoReajusteBombaPorContrato(usina, anoContrato, numContrato, dataVigencia);
        }

        public PagedList<ContratoBombaReajuste> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter)
        {
            var pagedList = _contratoBombaReajusteRepository.ListarContratoReajusteBombaPorPagina(pagina, porPagina, filter);

            return pagedList;
        }

        public IEnumerable<DateTime> ObterVigencias()
        {
            return _contratoBombaReajusteRepository.ObterVigencias();
        }

        public void AtualizarObraBombaReajustes(Obra obra, int sequencia, int numVersao, bool atualizaTaxa = false)
        {
            var obraBombaVersao = _contratoBombaReajusteRepository.ListarFiltradosTracking<ObraBombaVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero && t.NumeroVersao == numVersao && t.Sequencia == sequencia).FirstOrDefault();

            if (obraBombaVersao != null)
            {
                if (atualizaTaxa && obraBombaVersao.TaxaMinimaReajustadaAtual > 0)
                {
                    obraBombaVersao.TaxaMinimaPrecoProposto = obraBombaVersao.TaxaMinimaReajustadaAtual;
                    obraBombaVersao.M3PropostoAte = obraBombaVersao.M3ReajustadoAteAtual;
                    obraBombaVersao.M3PrecoProposto = obraBombaVersao.M3PrecoReajustadoAtual;
                }

                obraBombaVersao.TaxaMinimaReajustadaAnterior = 0;
                obraBombaVersao.M3ReajustadoAteAnterior = 0;
                obraBombaVersao.M3PrecoReajustadoAnterior = 0;
                obraBombaVersao.TaxaMinimaReajustadaAtual = 0;
                obraBombaVersao.M3ReajustadoAteAtual = 0;
                obraBombaVersao.M3PrecoReajustadoAtual = 0;
                obraBombaVersao.DataUltimoReajuste = null;

                _contratoBombaReajusteRepository.SaveChanges();
            }
        }
    }
}

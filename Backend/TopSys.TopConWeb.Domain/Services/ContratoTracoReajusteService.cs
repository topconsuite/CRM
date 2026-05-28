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
    public class ContratoTracoReajusteService : ServiceBase<ContratoTracoReajuste>, IContratoTracoReajusteService
    {
        private readonly IContratoTracoReajusteRepository _contratoTracoReajusteRepository;
        private readonly IObraRepository _obraRepository;

        public ContratoTracoReajusteService(IContratoTracoReajusteRepository contratoTracoReajusteRepository, IObraRepository obraRepository) : base(contratoTracoReajusteRepository)
        {
            _contratoTracoReajusteRepository = contratoTracoReajusteRepository;
            _obraRepository = obraRepository;
        }

        public IEnumerable<ContratoTracoReajuste> ListarContratoReajusteTracoPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia)
        {
            return _contratoTracoReajusteRepository.ListarContratoReajusteTracoPorContrato(usina, anoContrato, numContrato, dataVigencia);
        }

        public PagedList<ContratoTracoReajuste> ListarContratoReajusteTracoPorPagina(int pagina, int porPagina, string filter)
        {
            var pagedList = _contratoTracoReajusteRepository.ListarContratoReajusteTracoPorPagina(pagina, porPagina, filter);

            return pagedList;
        }

        public IEnumerable<DateTime> ObterVigencias()
        {
            return _contratoTracoReajusteRepository.ObterVigencias();
        }

        public void AtualizarObraTracoReajustes(Obra obra, int sequencia, int numVersao, bool atualizaPrecoProposto = false)
        {
            var obraTracoVersao = _contratoTracoReajusteRepository.ListarFiltradosTracking<ObraTracoVersao>(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero && t.NumeroVersao == numVersao && t.Sequencia == sequencia).FirstOrDefault();

            if (obraTracoVersao != null)
            {
                if (atualizaPrecoProposto && obraTracoVersao.PrecoReajustadoAtual > 0)
                    obraTracoVersao.M3PrecoProposto = obraTracoVersao.PrecoReajustadoAtual;

                obraTracoVersao.CustoServicoAnterior = 0;
                obraTracoVersao.CustoServicoReajustado = 0;
                obraTracoVersao.PrecoReajustadoAnterior = 0;
                obraTracoVersao.PrecoReajustadoAtual = 0;
                obraTracoVersao.DataUltimoReajuste = null;

                _obraRepository.AdicionarLogPropostaItem(obraTracoVersao, "ContratoTracoReajusteService.AtualizarObraTracoReajustes");
                _contratoTracoReajusteRepository.SaveChanges();
            }
                
        }
    }
}

using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class DemaisServicosService: ServiceBase<DemaisServicos>, IDemaisServicosService
    {
        private IDemaisServicosRepository _demaisServicosRepository;

        public DemaisServicosService(IDemaisServicosRepository demaisServicosRepositoy) : base(demaisServicosRepositoy)
        {
            _demaisServicosRepository = demaisServicosRepositoy;
        }

        public PagedList<DemaisServicos> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<DemaisServicos, bool>> filter)
        {
           var pagedList = _demaisServicosRepository.ListaEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }

        public void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            _demaisServicosRepository.AdicionarVersaoContrato(codUsina, numVersao, numObra);
        }

        public void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            _demaisServicosRepository.ExcluirVersaoContrato(codUsina, numVersao, numObra);
        }

        public void AdicionarContrato(int codUsina, int numVersao, int numObra)
        {
            _demaisServicosRepository.AdicionarContrato(codUsina, numVersao, numObra);
        }

        public void ExcluirContrato(int codUsina, int numObra)
        {
            _demaisServicosRepository.ExcluirContrato(codUsina, numObra);
        }
    }
}

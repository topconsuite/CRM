using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class OportunidadeTipoService : ServiceBase<OportunidadeTipo>, IOportunidadeTipoService
    {
        private readonly IOportunidadeTipoRepository _oportunidadeTipoRepository;

        public OportunidadeTipoService(IOportunidadeTipoRepository oportunidadeTipoRepository) : base(oportunidadeTipoRepository)
        {
            _oportunidadeTipoRepository = oportunidadeTipoRepository;
        }

        public PagedList<OportunidadeTipo> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<OportunidadeTipo, bool>> filter)
        {
            var pagedList = _oportunidadeTipoRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraFrenteResponse;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class ObraFrenteApplicationService : ApplicationServiceBase<ObraFrente>, IObraFrenteApplicationService
    {

        private readonly IObraFrenteService _obraFrenteService;

        public ObraFrenteApplicationService(
            IObraFrenteService obraFrenteService, 
            IUnitOfWork unityOfWork) : base(obraFrenteService, unityOfWork)
        {
            _obraFrenteService = obraFrenteService;
        }

        public IEnumerable<ObraFrenteResponse> ListarPorObra(int obraUsina, int obraNumero)
        {
            return AutoMapper.Mapper.Map(_obraFrenteService.ListarPorObra(obraUsina, obraNumero), new List<ObraFrenteResponse>());
        }

        public bool VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia)
        {
            return _obraFrenteService.VerificarEnderecoPossuiProgramacao(obraUsina, obraNumero, obraSequencia);
        }
    }
}

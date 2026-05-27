using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Distancia;
using TopSys.TopConWeb.Application.DTOS.Response.Endereco;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class EnderecoApplicationService : ApplicationServiceBase<Endereco>, IEnderecoApplicationService
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IUsinaDistanciaCepService _usinaDistanciaCepService;

        public EnderecoApplicationService(IEnderecoService enderecoService, 
            IUsinaDistanciaCepService usinaDistanciaCepService, 
            IUnitOfWork unityOfWork)
            : base(enderecoService, unityOfWork)
        {
            _enderecoService = enderecoService;
            _usinaDistanciaCepService = usinaDistanciaCepService;
        }

        public EnderecoResponse ObterPorCep(string cep)
        {
            return AutoMapper.Mapper.Map(_enderecoService.ObterPorCep(cep), new EnderecoResponse());
        }

        public IEnumerable<MunicipioDTO> ListarMunicipios()
        {
            return AutoMapper.Mapper.Map(_enderecoService.ListarMunicipios(), new List<MunicipioDTO>());
        }

        public bool UsinaAtendeCep(int idUsina, string cep)
        {
            return _enderecoService.UsinaAtendeCep(idUsina, cep);
        }

        public float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep)
        {
            return _enderecoService.ObterValorAdicionalM3PorUsinaCep(idUsina, cep);
        }

        public UsinaDistanciaCep ObterDistanciaKmPorUsinaCep(int idUsina, string cep)
        {
            return _usinaDistanciaCepService.ObterPorId(idUsina, cep);
        }

        public EstimativaKmEnderecoResponse ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(int idUsina, string enderecoObra)
        {
            var distanciaEntreUsinaeObra =  _enderecoService.ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(idUsina, enderecoObra, out var possuiGoogleApi);

            return new EstimativaKmEnderecoResponse { DistanciaEmKm = distanciaEntreUsinaeObra, UtilizaGoogleApi = possuiGoogleApi };
        }
    }
}

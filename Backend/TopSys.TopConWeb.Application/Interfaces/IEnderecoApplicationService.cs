using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Distancia;
using TopSys.TopConWeb.Application.DTOS.Response.Endereco;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IEnderecoApplicationService : IApplicationServiceBase<Endereco>
    {
        EnderecoResponse ObterPorCep(string cep);
        IEnumerable<MunicipioDTO> ListarMunicipios();
        bool UsinaAtendeCep(int idUsina, string cep);
        float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep);
        UsinaDistanciaCep ObterDistanciaKmPorUsinaCep(int idUsina, string cep);
        EstimativaKmEnderecoResponse ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(int idUsina, string enderecoObra);
    }
}

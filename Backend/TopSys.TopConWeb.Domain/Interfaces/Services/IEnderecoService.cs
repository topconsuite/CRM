using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Services;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IEnderecoService : IServiceBase<Endereco>
    {
        Endereco ObterPorCep(string cep);
        IEnumerable<Municipio> ListarMunicipios();
        void Salvar(string cep, int municipioCodigo, string campo);
        bool UsinaAtendeCep(int idUsina, string cep);
        float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep);
        int? ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(int idUsina, string enderecoObra, out bool possuiGoogleApi);
        Municipio SalvarMunicipio(Municipio municipio);
    }
}

using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IEnderecoRepository : IRepositoryBase<Endereco>
    {
        Endereco ObterPorCep(string cep);
        IEnumerable<Municipio> ListarMunicipios();
        Municipio ObterMunicipioPorNomeUf(string municipioNome, string uf);
        Municipio ObterMunicipioPorNomeUf(string municipioNome, string uf, params char[] escaparCaracteres);
        Municipio ObterMunicipioPorIbgeCodigo(int ibgeCodigo);
        bool UsinaAtendeCep(int idUsina, string cep);
        float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep);
        void Salvar(Endereco endereco);
        Endereco ObterEnderecoUsina(int codUsina);
        void SalvarMunicipio(Municipio municipio);
    }
}

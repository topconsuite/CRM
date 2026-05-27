using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Services
{
    public class EnderecoService : ServiceBase<Endereco>, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly IViaCepService _viaCepService;
        private readonly IGoogleServices _googleServices;

        public EnderecoService(IEnderecoRepository enderecoRepository, IViaCepService viaCepService,
            IParametroRepository parametroRepository, IGoogleServices googleServices)
            : base(enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
            _viaCepService = viaCepService;
            _parametroRepository = parametroRepository;
            _googleServices = googleServices;
        }

        public Endereco ObterPorCep(string cep)
        {
            var endereco = _enderecoRepository.ObterPorCep(cep);

            if (endereco == null || !endereco.IsConfiavel)
            {
                endereco = _viaCepService.ObterEnderecoPorCep(cep) ?? endereco;

                if (endereco != null && endereco.MunicipioCodigo == 0)
                {
                    var municipioPorNome = _enderecoRepository.ObterMunicipioPorNomeUf(endereco.Municipio.Nome, endereco.Municipio.Uf);

                    if (municipioPorNome == null)
                        municipioPorNome = _enderecoRepository.ObterMunicipioPorNomeUf(endereco.Municipio.Nome, endereco.Municipio.Uf, '-', ')', '(', '\'');

                    if (municipioPorNome != null)
                    {
                        endereco.MunicipioCodigo = municipioPorNome.Codigo;
                        endereco.Municipio.Codigo = municipioPorNome.Codigo;
                    }
                    else
                    {
                        var municipioPorIbgeCodigo = _enderecoRepository.ObterMunicipioPorIbgeCodigo(endereco.Municipio.IbgeCodigo);

                        if (municipioPorIbgeCodigo != null)
                        {
                            endereco.MunicipioCodigo = municipioPorIbgeCodigo.Codigo;
                            endereco.Municipio.Codigo = municipioPorIbgeCodigo.Codigo;
                        }
                        else
                        {
                            var municipioNovo = new Municipio();
                            
                            municipioNovo.Nome = endereco.Municipio.Nome;
                            municipioNovo.IbgeCodigo = endereco.Municipio.IbgeCodigo;
                            municipioNovo.Uf = endereco.Municipio.Uf;

                            municipioNovo.Pais = 1058;

                            endereco.MunicipioCodigo = municipioNovo.Codigo;
                            endereco.Municipio = municipioNovo;
                        }
                    }
                }

                if (endereco != null)
                    endereco.Complemento = "";
            }

            if (endereco != null && endereco.Logradouro != "" && !endereco.IsConfiavel)
                endereco.Logradouro = EnderecoHelper.RetornarComTipoLogradouroAbreviado(endereco.Logradouro);

            if (endereco != null)
            {
                endereco.Bairro = endereco.Bairro.Replace("'", " ");
                endereco.Cep = endereco.Cep.Replace("'", " ");
                endereco.Complemento = endereco.Complemento.Replace("'", " ");
                endereco.Logradouro = endereco.Logradouro.Replace("'", " ");
            }

            return endereco;
        }

        public IEnumerable<Municipio> ListarMunicipios()
        {
            return _enderecoRepository.ListarMunicipios();
        }

        public bool UsinaAtendeCep(int idUsina, string cep)
        {
            return _enderecoRepository.UsinaAtendeCep(idUsina, cep);
        }

        public float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep)
        {
            var valor = _enderecoRepository.ObterValorAdicionalM3PorUsinaCep(idUsina, cep);

            ObraScopes.CepAtendidoScopeIsValid(valor);
            
            return valor;
        }

        public void Salvar(string cep, int municipioCodigo, string campo)
        {
            var endereco = ObterPorCep(cep);

            if (endereco.EnderecoScopeIsValid(campo) && !endereco.IsConfiavel)
            {
                endereco.Municipio = _enderecoRepository.ObterPorId<Municipio>(municipioCodigo);

                _enderecoRepository.Salvar(endereco);
            }
            
        }

        public int? ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(int idUsina, string enderecoObra, out bool possuiGoogleKey)
        {
            var googleApiKey = _parametroRepository.ObterParametroN("web", "DistanceMatrixAPIKey");

            possuiGoogleKey = !googleApiKey.Equals("") ? true : false;
             
            var enderecoUsina = _enderecoRepository.ObterEnderecoUsina(idUsina);
            if (enderecoUsina == null)
                return null;

            return _googleServices.ObterDistanciaKmViaDistanceMatrixApi(enderecoUsina, enderecoObra, googleApiKey);
            
        }

        public Municipio SalvarMunicipio(Municipio municipio)
        {
            var municipios = _enderecoRepository.ListarMunicipios();
            var codigoMunicipio = municipios.Max(t => t.Codigo) + 1;

            var municipioExistente = municipios.Where(t => t.IbgeCodigo == municipio.IbgeCodigo && IguaisSemAcento(t.Nome, municipio.Nome)).FirstOrDefault();

            if (municipioExistente != null)
                return municipioExistente;

            var municipioNovo = new Municipio();

            municipioNovo.Codigo = codigoMunicipio;
            municipioNovo.Nome = municipio.Nome;
            municipioNovo.IbgeCodigo = municipio.IbgeCodigo;
            municipioNovo.Uf = municipio.Uf;
            municipioNovo.Pais = municipio.Pais;

            _enderecoRepository.SalvarMunicipio(municipioNovo);
            
            return municipioNovo;
        }

        static bool IguaisSemAcento(string texto1, string texto2)
        {
            string normalizado1 = StringHelper.RemoverAcentos(texto1).ToUpperInvariant().Trim();
            string normalizado2 = StringHelper.RemoverAcentos(texto2).ToUpperInvariant().Trim();

            return string.Equals(normalizado1, normalizado2, StringComparison.Ordinal);
        }
    }
}

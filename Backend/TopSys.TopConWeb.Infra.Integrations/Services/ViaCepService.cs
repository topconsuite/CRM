using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Infra.Integrations.DTOs;

namespace TopSys.TopConWeb.Infra.Integrations.Services
{
    public class ViaCepService : IViaCepService
    {
        public Endereco ObterEnderecoPorCep(string cep)
        {
            const int TIMEOUT = 2000;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new RestClient($"https://viacep.com.br/ws/{cep}/json/");
            client.Timeout = TIMEOUT;

            var req = new RestRequest();
            req.Timeout = TIMEOUT;

            client.UserAgent = "";

            var response = client.Execute<ViaCepEndereco>(req);

            var endereco = response.Data;

            if (endereco == null) return null;

            if (endereco.Erro)
            {
                // Cep não encontrado
                endereco = null;
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                // Serviço indisponível ou falha interna do servidor
                endereco = null;
            }
            else
            {
                // Necessário pois o viaCep retorna formatado
                endereco.Cep = cep;
            }

            return this.Parse(endereco);
        }

        private Endereco Parse(ViaCepEndereco viaCepEndereco)
        {
            if (viaCepEndereco == null) return null;

            return new Endereco()
            {
                Cep = viaCepEndereco.Cep,
                Logradouro = viaCepEndereco.Logradouro.ToUpper(),
                Complemento = viaCepEndereco.Complemento.ToUpper(),
                Bairro = viaCepEndereco.Bairro.ToUpper(),
                Municipio = new Municipio()
                {
                    Codigo = 0,
                    Nome = viaCepEndereco.MunicipioNome.ToUpper(),
                    Uf = viaCepEndereco.MunicipioUf.ToUpper(),
                    IbgeCodigo = viaCepEndereco.MunicipioIbgeCodigo
                },
                Numero = 0,
                IsConfiavel = false
            };
        }
    }
}

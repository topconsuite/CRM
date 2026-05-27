using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Infra.Integrations.DTOs.DistanceMatrixAPI;

namespace TopSys.TopConWeb.Infra.Integrations.Services
{
    public class GoogleServices : IGoogleServices
    {
        private const string googleEndPoint= "https://maps.googleapis.com/maps/api/";
        private const string distanceMatrixApiParametro = "distancematrix";

        public int? ObterDistanciaKmViaDistanceMatrixApi(Endereco enderecoUsina, string enderecoObra, string apiKey)
        {

            var enderecoUsinaFormatado = FormatarEnderecoGoogleApi(enderecoUsina);
            var enderecoObraFormatado = enderecoObra.Replace(" ","+");

            var client = new RestClient($"{googleEndPoint}{distanceMatrixApiParametro}/json?units=metric&origins={enderecoUsinaFormatado}&destinations={enderecoObraFormatado}&key={apiKey}");

            var request = new RestRequest();

            var response = client.Execute<DistanceMatrixResponse>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var resposta = response.Data;

            if (resposta.Status != "OK" || resposta?.Resultados?.FirstOrDefault()?.Rotas?.FirstOrDefault()?.StatusElemento != "OK")
                return null;

            var resultadoEmKm = ((resposta?.Resultados?.FirstOrDefault()?.Rotas?.FirstOrDefault()?.Distancia?.ResultadoEmMetros) / 1000);
            return (int)Math.Ceiling(System.Convert.ToDouble(resultadoEmKm));
        }

        private string FormatarEnderecoGoogleApi(Endereco endereco)
        {
            return $"{endereco.Logradouro.Replace(" ","+")}+{endereco.Numero}+{endereco.Cep.Replace(" ", "+").Replace("-", "")}";
        }
    }
}

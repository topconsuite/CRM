using RestSharp;
using System;
using System.Net;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.CartaoPagamentoIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Infra.Integrations.DTOs.CartaoPagamentoIntegracao;

namespace TopSys.TopConWeb.Infra.Integrations.Services
{
    public class CieloLioIntegrationService : ICieloLioIntegrationService
    {
        public bool EnviarSolicitacaoPagamento(CieloLioDados cieloLioDados, SolicitacaoPagamento solicitacaoPagamento)
        {
            const int TIMEOUT = 10000;

            var client = new RestClient($"https://api.cielo.com.br/order-management/v1/orders");
            client.Timeout = TIMEOUT;

            var request = new RestRequest(Method.POST);
            request.Timeout = TIMEOUT;
            request.AddHeader("client-id", cieloLioDados.CieloClientId);
            request.AddHeader("access-token", cieloLioDados.CieloAccessToken);
            request.AddHeader("merchant-id", cieloLioDados.CieloMerchantId);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");

            var s = solicitacaoPagamento;

            var contrato = (s.ObraNumContrato > 0 ? $" CONTRATO: {s.ObraUsinaCodigo}-{s.ObraNumContrato}/{s.ObraAnoContrato}" : "");

            var endereco = (s.ObraEnderecoLogradouro != ""
                ? $"{s.ObraEnderecoLogradouro},{(s.ObraEnderecoNumero > 0 ? $" {s.ObraEnderecoNumero}" : "")} {s.ObraEnderecoComplemento} - {s.ObraEnderecoBairro}"
                    + (s.ObraEnderecoMunicipio != null ? $" - {s.ObraEnderecoMunicipio.Nome}/{s.ObraEnderecoMunicipio.Uf}" : "")
                : "");

            var formaPagamento = (s.TipoCobranca.Equals("CC", StringComparison.InvariantCultureIgnoreCase) ? "CREDITO" : (s.TipoCobranca.Equals("CC", StringComparison.InvariantCultureIgnoreCase) ? "DEBITO" : s.TipoCobranca));
            
            var body = new CieloLioOrderRequest
            {
                reference = $"{s.IntervenienteRazao} {s.CpfCnpj}{contrato} {endereco}",
                status = "ENTERED",
                items = new CieloLioOrderItemDTO[] {
                    new CieloLioOrderItemDTO {
                        name = $"{s.IntervenienteRazao}",
                        unit_price = Convert.ToInt32(s.ValorTotal * 100),
                        quantity = 1
                    },
                    new CieloLioOrderItemDTO {
                        name = $"CPF: {s.CpfCnpj}{contrato}"
                    },
                    new CieloLioOrderItemDTO {
                        name = $"{endereco}"
                    },
                    new CieloLioOrderItemDTO {
                        name = $"Forma pagamento: {formaPagamento} - Parcelas: {s.QuantidadeParcelas}"
                    },
                    new CieloLioOrderItemDTO {
                        name = $"Obs.: {s.Observacao}"
                    }
                },
                notes = "",
                price = Convert.ToInt32(s.ValorTotal * 100)
            };

            request.AddJsonBody(body);

            var response = client.Execute<CieloLioCreateOrderResponse>(request);

            var success = (response.StatusCode == HttpStatusCode.Created);

            if (success)
                solicitacaoPagamento.IntegracaoId = response.Data.Id;

            return success;
        }
    }

    public class CieloLioCreateOrderResponse
    {
        public string Id { get; set; }
    }
}

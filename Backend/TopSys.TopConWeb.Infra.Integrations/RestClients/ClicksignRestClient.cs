using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;

namespace TopSys.TopConWeb.Infra.Integrations.RestClients
{
    public class ClicksignRestClient : ThrottledRestClientDynamic
    {
        private readonly IParametroRepository _parametroRepository;
        private readonly IClicksignConfiguracaoRepository _clicksignConfiguracaoRepository;

        private const string DefaultBaseUrl = "https://app.clicksign.com";
        private const string DefaultSandboxUrl = "https://sandbox.clicksign.com";

        private int? _usinaIdContexto;

        public ClicksignRestClient(
            IParametroRepository parametrosRepository,
            IClicksignConfiguracaoRepository clicksignConfiguracaoRepository)
        {
            _parametroRepository = parametrosRepository;
            _clicksignConfiguracaoRepository = clicksignConfiguracaoRepository;

            var baseUrl = _parametroRepository.ObterParametroIntegracoes("clicksign", "BaseApiUrl");

#if DEBUG
            baseUrl = DefaultSandboxUrl;
#endif

            if (baseUrl == "")
                baseUrl = DefaultBaseUrl;

            this.BaseUrl = new Uri(baseUrl);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Define o contexto da Usina para resolução das credenciais ClickSign.
        /// Quando configurado, o sistema buscará as credenciais vinculadas à Usina informada.
        /// Caso a Usina não possua configuração própria, as credenciais globais serão utilizadas (fallback).
        /// </summary>
        public void SetUsinaContext(int? usinaId)
        {
            _usinaIdContexto = usinaId;
        }

        /// <summary>
        /// Resolve as credenciais ClickSign aplicando a hierarquia de fallback:
        /// Usina -> ClicksignConfiguracao (se existir e ativo) -> credenciais globais (legado).
        /// </summary>
        private (string token, string baseUrl) ResolverCredenciais()
        {
            if (_usinaIdContexto.HasValue)
            {
                var config = _clicksignConfiguracaoRepository.ObterConfiguracaoPorUsina(_usinaIdContexto.Value);

                if (config != null && config.Ativo)
                {
                    var baseUrl = config.BaseUrl;

#if DEBUG
                    baseUrl = DefaultSandboxUrl;
#endif
                    return (config.Token, baseUrl);
                }
            }

            // Fallback para configuração global (legado)
            return (
                _parametroRepository.ObterParametroIntegracoes("clicksign", "access_token"),
                _parametroRepository.ObterParametroIntegracoes("clicksign", "BaseApiUrl")
            );
        }

        public new IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var (acessToken, baseUrl) = ResolverCredenciais();

            if (!string.IsNullOrEmpty(baseUrl))
                this.BaseUrl = new Uri(baseUrl);

            request.Resource = $"{request.Resource}?access_token={acessToken}";
            return base.Execute<T>(request);
        }

        public new IRestResponse Execute(IRestRequest request)
        {
            var (acessToken, baseUrl) = ResolverCredenciais();

            if (!string.IsNullOrEmpty(baseUrl))
                this.BaseUrl = new Uri(baseUrl);

            request.Resource = $"{request.Resource}?access_token={acessToken}";
            return base.Execute(request);
        }

    }
}

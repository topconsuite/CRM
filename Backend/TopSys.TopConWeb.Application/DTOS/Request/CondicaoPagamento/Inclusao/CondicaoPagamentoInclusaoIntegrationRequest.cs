using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Domain.Constants.CondicaoPagamento;


namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao
{
	public class CondicaoPagamentoInclusaoIntegrationRequest
	{
		[Required]
        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.CondicoesPagamentos)]
        public IEnumerable<CondicaoPagamentoIntegrationRequest> CondicoesPagamentos { get; set; }
	}
}

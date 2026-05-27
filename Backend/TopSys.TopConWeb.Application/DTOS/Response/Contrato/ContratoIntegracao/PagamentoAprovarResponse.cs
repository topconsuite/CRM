using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao
{
    public class PagamentoAprovarResponse
    {
		[Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::sequence")]
		[JsonProperty(PropertyName = "sequence")]
		public int Sequencia { get; set; }

		[Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::approved")]
		[JsonProperty(PropertyName = "approved")]
		public bool Aprovado { get; set; } = false;

		[Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::disapproved")]
		[JsonProperty(PropertyName = "disapproved")]
		public bool Reprovado { get; set; } = false;
	}
}

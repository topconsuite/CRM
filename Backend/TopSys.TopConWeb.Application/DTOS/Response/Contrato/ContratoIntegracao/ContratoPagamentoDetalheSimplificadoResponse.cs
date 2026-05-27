using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao
{
    public class ContratoPagamentoDetalheSimplificadoResponse
    {

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::payment::detail::sequence")]
        [JsonProperty(PropertyName = "sequence")]
        public int DetalheSequencia { get; set; }

        [JsonProperty(PropertyName = "value")]
        public float Valor { get; set; }

    }
}

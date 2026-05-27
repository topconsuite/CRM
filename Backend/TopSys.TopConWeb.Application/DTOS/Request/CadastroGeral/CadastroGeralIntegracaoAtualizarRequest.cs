using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral
{
    public class CadastroGeralIntegracaoAtualizarRequest
    {
        [Range(1, 9999, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_RANGE) + "::code::1::9999")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }

        [StringLength(30, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::description::30")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [StringLength(17, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::short_description::17")]
        [JsonProperty(PropertyName = "short_description")]
        public string DescricaoReduzida { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceber
{
    public class TituloContasAReceberRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::accounts_receivable")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                                           "::accounts_receivable" + "::100")]
        [JsonProperty(PropertyName = "accounts_receivable")]
        public TituloContasAReceberAdicionarRequest[] TitulosContasAReceber { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;

namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class PublicoTituloContasAReceberResponseList
    {
        [Required(ErrorMessage = "The acounts receivable is required.")]
        [UniqueCombinations(nameof(PublicoTituloContasAReceberResponse.EmpresaCodigo), ErrorMessage = "Duplicate code in the list.")]
        [MaxListLength(100, ErrorMessage = "The list cannot have more than 100 elements.")]
        [JsonProperty(PropertyName = "accounts_receivable")]
        public List<PublicoTituloContasAReceberResponse> ContasAReceberTitulos { get; set; }
    }
}

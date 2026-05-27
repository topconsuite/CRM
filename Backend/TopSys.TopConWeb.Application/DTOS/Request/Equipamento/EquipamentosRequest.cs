using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Equipamento
{
    public class EquipamentosRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::equipments")]
        [UniqueCombinations(nameof(EquipamentoAdicionarRequest.Codigo), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_COMBINATIONS) + "::code")]
        [UniqueFields(nameof(EquipamentoAdicionarRequest.Placa), nameof(EquipamentoAdicionarRequest.ExternalId), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS) + "::(license_plate, external_id)")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MAX_LIST_LENGTH) + "::100")]
        [JsonProperty(PropertyName = "equipments")]
        public EquipamentoAdicionarRequest[] Equipamentos { get; set; }
    }

}

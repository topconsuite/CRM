using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato
{
    public class ObraTributacaoMunicipalDTO
    {

        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int UsinaEntregaCodigo { get; set; }

        [JsonProperty(PropertyName = "construction_permit_number")]
        public string CodigoObraPrefeitura { get; set; }
        
        [JsonProperty(PropertyName = "construction_ccm")]
        public string ObraCCM { get; set; }

        [JsonProperty(PropertyName = "taxation_iss")]
        public string TributacaoISS { get; set; }

        [JsonProperty(PropertyName = "withholding_iss")]
        public string RetencaoISS { get; set; }
    }
}

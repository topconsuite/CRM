using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato
{
    public class VendedorDTO
    {
        public VendedorDTO(int codigo, string cpfCnpj, string externalId)
        {
            Codigo = codigo;
            CpfCnpj = cpfCnpj;
            ExternalId = externalId;
        }

        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }
        [JsonProperty(PropertyName = "cnpj_cpf")]
        public string CpfCnpj { get; set; }
        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }
    }
}

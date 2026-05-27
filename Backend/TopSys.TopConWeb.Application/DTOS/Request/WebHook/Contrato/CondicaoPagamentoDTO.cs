using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{
    public class CondicaoPagamentoDTO
    {
        public CondicaoPagamentoDTO(int codigo, string descricao, string IdCadastro)
        {
            Codigo = codigo;
            Descricao = descricao;

            
            Data = DateHelper.ExtractFromIDD(IdCadastro) ?? DateTime.MinValue;
        }

        [JsonProperty("code")]
        public int Codigo { get; set; }

        [JsonProperty("description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Data { get; set; }
    }

}

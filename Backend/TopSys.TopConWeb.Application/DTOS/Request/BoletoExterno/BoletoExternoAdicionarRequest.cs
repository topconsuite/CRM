using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.BoletoExterno
{
    public class BoletoExternoAdicionarRequest
    {
        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::concrete_batching_plant_contract" + "::3")]
        [JsonProperty(PropertyName = "concrete_batching_plant_contract")]
        public int UsinaCodigo { get; set; }

        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::contract_number" + "::6")]
        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::contract_year" + "::2")]
        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [Range(0, 99999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::branch" + "::5")]
        [JsonProperty(PropertyName = "branch")]
        public int Filial { get; set; }

        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::client" + "::6")]
        [JsonProperty(PropertyName = "client")]
        public int Cliente { get; set; }

        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::document_type" + "::2")]
        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumento { get; set; }

        [Range(0, 99999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::invoice_number" + "::11")]
        [JsonProperty(PropertyName = "invoice_number")]
        public long FaturaNumero { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::invoice_series" + "::3")]
        [JsonProperty(PropertyName = "invoice_series")]
        public string FaturaSerie { get; set; } = "";

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::invoice_sub_series" + "::3")]
        [JsonProperty(PropertyName = "invoice_sub_series")]
        public int FaturaSubSerie { get; set; }

        [JsonProperty(PropertyName = "file")]
        public byte[] Arquivo { get; set; }

        public int? Sequencia { get; set; }

        public string NomeArquivo { get; set; } = "";

        public bool PossuiChaveContrato
        {
            get
            {
                return UsinaCodigo != 0 && ContratoNumero != 0 && ContratoAno != 0;
            }
        }

        public bool PossuiChaveFatura
        {
            get
            {
                return Filial != 0 && Cliente != 0 && TipoDocumento != 0 && FaturaNumero != 0 && !string.IsNullOrWhiteSpace(FaturaSerie) && FaturaSubSerie != 0;
            }
        }

        public string Chave 
        {
            get
            {
                var chave = "";
                if (PossuiChaveFatura)
                {
                    chave = $"{Filial}-{Cliente}-{TipoDocumento}-{FaturaNumero}-{FaturaSerie}-{FaturaSubSerie}";
                }
                else if (PossuiChaveContrato)
                {
                    chave = $"{UsinaCodigo}-{ContratoNumero}-{ContratoAno}";
                }
                return chave;
            }
        }
    }
}

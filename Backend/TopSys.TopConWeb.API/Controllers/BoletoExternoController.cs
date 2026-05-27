using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.BoletoExterno;
using TopSys.TopConWeb.API.Security;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class BoletoExternoController : BaseController
    {
        private readonly IBoletoExternoApplicationService _boletoExternoApplicationService;

        public BoletoExternoController(IBoletoExternoApplicationService boletoExternoApplicationService)
        {
            _boletoExternoApplicationService = boletoExternoApplicationService;
        }

        [HttpGet]
        [Route("v1/boletosExternos/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarBoletosExternos(int idUsina, int ano, int numero)
        {
            var listarBoletosExternos = _boletoExternoApplicationService.ListarBoletosExternos(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, listarBoletosExternos);
        }

        [HttpGet]
        [Route("v1/boletosExternos/arquivo/id/{idArquivo}/chave/{chave}/sequencia/{sequencia}")]
        [Authorize]
        public string ObterArquivo(Guid idArquivo, string chave, int sequencia)
        {
            var arquivo = _boletoExternoApplicationService.ObterArquivo(idArquivo, chave, sequencia);

            return _boletoExternoApplicationService.ObterArquivoConvertidoBase64(arquivo);
        }

        [HttpPost]
        [Route("integrations/external-bank-slip")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> AdicionarBoletoExterno()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
               return CreateResponse(HttpStatusCode.UnsupportedMediaType, "O tipo de conteúdo da requisição deve ser multipart/form-data.");
            }

            var provider = new MultipartMemoryStreamProvider();
            Request.Content.ReadAsMultipartAsync(provider).GetAwaiter().GetResult();

            var jsonContent = provider.Contents.FirstOrDefault(c => c.Headers.ContentDisposition.Name.Trim('\"').Equals("externalBankSlip"));

            if (jsonContent == null)
                return CreateResponse(HttpStatusCode.BadRequest, "A parte 'externalBankSlip' com os dados JSON não foi encontrada.");

            var jsonString = jsonContent.ReadAsStringAsync().GetAwaiter().GetResult();
            var bankSlipRequest = JsonConvert.DeserializeObject<BoletoExternoAdicionarRequest>(jsonString);

            if (bankSlipRequest == null)
                return CreateResponse(HttpStatusCode.BadRequest, "JSON inválido na parte 'externalBankSlip'.");

            if (!bankSlipRequest.PossuiChaveContrato && !bankSlipRequest.PossuiChaveFatura)
                return CreateResponse(HttpStatusCode.BadRequest, "É necessário informar os campos da chave de contrato ou da chave de fatura.");

            var fileContents = provider.Contents.Where(c => c.Headers.ContentDisposition.Name.Trim('\"').Equals("file")).ToList();

            if (!fileContents.Any())
                return CreateResponse(HttpStatusCode.BadRequest, "Nenhum arquivo foi enviado.");

            var requests = new List<BoletoExternoAdicionarRequest>();
            int seq = 1;
            foreach (var fileContent in fileContents)
            {
                var filename = fileContent.Headers.ContentDisposition?.FileName?.Trim('"') ?? $"arquivo_{seq}";

                // Validação de content-type
                var mediaType = fileContent.Headers.ContentType?.MediaType;
                var fileBytes = fileContent.ReadAsByteArrayAsync().GetAwaiter().GetResult();

                // Validação: tipo e assinatura PDF ("%PDF")
                var isPdfType = string.Equals(mediaType, "application/pdf", StringComparison.OrdinalIgnoreCase);
                var isPdfSignature = fileBytes != null && fileBytes.Length >= 4 &&
                                     fileBytes[0] == (byte)'%' &&
                                     fileBytes[1] == (byte)'P' &&
                                     fileBytes[2] == (byte)'D' &&
                                     fileBytes[3] == (byte)'F';

                if (!isPdfType || !isPdfSignature)
                    return CreateResponse(HttpStatusCode.BadRequest, $"O arquivo '{filename}' não é um PDF válido.");

                var single = JsonConvert.DeserializeObject<BoletoExternoAdicionarRequest>(jsonString);
                single.Arquivo = fileBytes;
                single.Sequencia = seq++;
                single.NomeArquivo = filename;

                requests.Add(single);
            }

            var result = _boletoExternoApplicationService.AdicionarBoletoExterno(requests.ToArray());

            return CreateResponse(result);
        }
    }
}
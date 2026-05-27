using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.BoletoExterno;
using TopSys.TopConWeb.Application.DTOS.Response.BoletoExterno;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class BoletoExternoApplicationService : ApplicationServiceBase<BoletoExterno>, IBoletoExternoApplicationService
    {
        private readonly IBoletoExternoService _boletoExternoService;
        private readonly IHeaderProvider _headerProvider;

        public BoletoExternoApplicationService(IBoletoExternoService boletoExternoService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(boletoExternoService, unityOfWork)
        {
            _boletoExternoService = boletoExternoService;
            _headerProvider = headerProvider;
        }
        public ICollection<BoletoExternoResponse> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato)
        {
            return AutoMapper.Mapper.Map(_boletoExternoService.ListarBoletosExternos(codUsina, anoContrato, numeroContrato), new List<BoletoExternoResponse>());
        }

        public byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia)
        {
            return _boletoExternoService.ObterArquivo(idArquivo, chave, sequencia);
        }

        public string ObterArquivoConvertidoBase64(byte[] arquivo)
        {
            var base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,2}$", RegexOptions.None);

            var arquivoString = new ByteArrayContent(arquivo).ReadAsStringAsync().Result;

            if (!arquivoString.StartsWith("data:") && !arquivoString.Contains(";base64,"))
            {
                if (!base64Regex.IsMatch(arquivoString))
                    return $"data:application/pdf;base64,{Convert.ToBase64String(arquivo)}";
            }

            return arquivoString;
        }

        public ResultDTO<BoletoExternoAdicionarResponse> AdicionarBoletoExterno(BoletoExternoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_boletoExternoService.ObterPorChaveNomeArquivo(request[i].Chave, request[i].NomeArquivo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "File"),
                        i));
            }

            if (errors.Count > 0)
                return new ResultDTO<BoletoExternoAdicionarResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);

            // Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var register in request)
                    {
                        var newExternalBankSlip = new BoletoExterno();

                        newExternalBankSlip = AutoMapper.Mapper.Map(register, newExternalBankSlip);

                        _boletoExternoService.AdicionarBoletoExterno(newExternalBankSlip);
                    }

                    Commit();
                    scope.Complete();

                    var result = new BoletoExternoAdicionarResponse(request.Length);

                    return new ResultDTO<BoletoExternoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<BoletoExternoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }
    }
}

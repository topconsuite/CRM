using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.AlterarLimiteCreditoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteAnexo;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteHistorico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.CentroCusto;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Response.IntervenienteAnexo;
using TopSys.TopConWeb.Application.DTOS.Response.IntervenienteHistorico;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class IntervenienteApplicationService : ApplicationServiceBase<Interveniente>, IIntervenienteApplicationService
    {

        private readonly IIntervenienteService _intervenienteService;
        private readonly IHeaderProvider _headerProvider;

        public IntervenienteApplicationService(IIntervenienteService intervenienteService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(intervenienteService, unityOfWork)
        {
            _intervenienteService = intervenienteService;
            _headerProvider = headerProvider;
        }

        public void AlterarLimiteDeCredito(AlterarLimiteCreditoRequest alterarLimiteCreditoRequest)
        {
            var interv = AutoMapper.Mapper.Map(alterarLimiteCreditoRequest, new Interveniente());
            _intervenienteService.AtualizarLimite(interv, alterarLimiteCreditoRequest.LimiteData, alterarLimiteCreditoRequest.LimiteValor);
            _intervenienteService.AtualizaInformacoesBloqueio(interv, alterarLimiteCreditoRequest.BloqueioMotivo?.Codigo ?? 0, alterarLimiteCreditoRequest.BloqueioObservacao);
            Commit();
        }

        public void AprovarIss(string usuario, int codInterveniente)
        {
            var interveniente = _intervenienteService.ObterPorId(codInterveniente);
            interveniente.AprovaIss(usuario);
            Commit();
        }

        public bool InscricaoEstadualEhValida(string inscricaoEstadual, string uf = "")
        {
            return _intervenienteService.InscricaoEstadualEhValida(inscricaoEstadual, uf);
        }
        public IntervenienteResponse ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual)
        {
            return AutoMapper.Mapper.Map(_intervenienteService.ObterPorCpfCnpj(cpfCnpj, inscricaoEstadual), new IntervenienteResponse());
        }

        public PagedList<IntervenienteHistoricoResponse> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter)
        {
            var programacoes = _intervenienteService.ListarHistoricoEmOrdemDescrescente(pagina, porPagina, filter);

            var historicosDTO = AutoMapper.Mapper.Map(programacoes, new PagedList<IntervenienteHistoricoResponse>());

            return historicosDTO;
        }

        public IntervenienteResponse ObterPorCodigo(int intervenienteCodigo)
        {
            return AutoMapper.Mapper.Map(_intervenienteService.ObterPorId(intervenienteCodigo), new IntervenienteResponse());
        }

        public void AdicionarHistorico(string usuario, IntervenienteHistoricoRequest historico)
        {
            var newHistorico = AutoMapper.Mapper.Map(historico, new IntervenienteHistorico());

            _intervenienteService.AdicionarHistorico(newHistorico, usuario);
            Commit();
        }

        public IntervenienteResponse Adicionar(IntervenienteInclusaoRequest interveniente, string usuario)
        {

            var newInterveniente = AutoMapper.Mapper.Map(interveniente, new Interveniente());

            newInterveniente.IdAtualizacao = StringHelper.GetIDD(usuario);

            _intervenienteService.Adicionar(newInterveniente);

            Commit();

            return ObterPorCpfCnpj(newInterveniente.CpfCnpj, newInterveniente.InscricaoEstadual);


        }

        public void Atualizar(IntervenienteAlteracaoRequest interveniente, string usuario)
        {

            var oldInterveniente = _intervenienteService.ObterPorId(interveniente.Codigo);

            if (oldInterveniente == null)
                throw new Exception($"Cliente código '{interveniente.Codigo}' não encontrado/cadastrado.");

            oldInterveniente.IdAtualizacao = StringHelper.GetIDD(usuario);

            var newInterveniente = AutoMapper.Mapper.Map(interveniente, oldInterveniente);

            _intervenienteService.Atualizar(newInterveniente);

            Commit();

        }

        public void AdicionarAnexo(string usuario, IntervenienteAnexoAdicionarRequest anexo, out string mensagem)
        {
            mensagem = "";
            
            if (anexo == null)
            {
                mensagem = $"Arquivos maiores que 10 MB não são suportados. Por favor, envie um arquivo com tamanho inferior.";
                return;
            }

            var newAnexo = AutoMapper.Mapper.Map(anexo, new IntervenienteAnexo());

            if(anexo.NumeroOportunidade > 0)
            {
                var oportunidadeAnexo = new OportunidadeAnexo()
                {
                    Usina = 999,
                    AnoOportunidade = anexo.AnoOportunidade,
                    NumeroOportunidade = anexo.NumeroOportunidade,

                    Nome = anexo.Nome,
                    Interveniente = anexo.IntervenienteCodigo
                };

                newAnexo.OportunidadeAnexo = oportunidadeAnexo;
            }

            _intervenienteService.AdicionarAnexo(usuario, newAnexo);
        }

        public ICollection<IntervenienteAnexoResponse> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada)
        {
            return AutoMapper.Mapper.Map(_intervenienteService.ListarAnexos(intervenienteCodigo, anoChamada, numeroChamada), new List<IntervenienteAnexoResponse>());
        }

        public ICollection<IntervenienteAnexoResponse> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade)
        {
            return AutoMapper.Mapper.Map(_intervenienteService.ListarAnexosPorOportunidade(intervenienteCodigo, usina, anoOportunidade, numeroOportunidade), new List<IntervenienteAnexoResponse>());
        }

        public byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            return _intervenienteService.ObterAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);
        }

        public string ObterAnexoConvertidoBase64(byte[] anexo, string nome)
        {
            var base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,2}$", RegexOptions.None);

            var anexoString = new ByteArrayContent(anexo).ReadAsStringAsync().Result;

            if (!anexoString.StartsWith("data:") && !anexoString.Contains(";base64,"))
            {
                if (!base64Regex.IsMatch(anexoString))
                {
                    string extension = Path.GetExtension(nome).ToLowerInvariant();
                    string mime;
                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            mime = "image/jpeg";
                            break;
                        case ".png":
                            mime = "image/png";
                            break;
                        case ".gif":
                            mime = "image/gif";
                            break;
                        case ".bmp":
                            mime = "image/bmp";
                            break;
                        case ".pdf":
                            mime = "application/pdf";
                            break;
                        case ".zip":
                            mime = "application/zip";
                            break;
                        case ".rar":
                            mime = "application/vnd.rar";
                            break;
                        case ".doc":
                        case ".docx":
                            mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case ".xls":
                        case ".xlsx":
                            mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case ".ppt":
                        case ".pptx":
                            mime = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                            break;
                        case ".txt":
                            mime = "text/plain";
                            break;
                        case ".html":
                        case ".htm":
                            mime = "text/html";
                            break;
                        case ".csv":
                            mime = "text/csv";
                            break;
                        case ".xml":
                            mime = "application/xml";
                            break;
                        case ".mp3":
                            mime = "audio/mpeg";
                            break;
                        case ".wav":
                            mime = "audio/wav";
                            break;
                        case ".mp4":
                            mime = "video/mp4";
                            break;
                        case ".avi":
                            mime = "video/x-msvideo";
                            break;
                        case ".mov":
                            mime = "video/quicktime";
                            break;
                        case ".json":
                            mime = "application/json";
                            break;
                        default:
                            mime = "application/octet-stream"; // Tipo MIME padrão para arquivos desconhecidos
                            break;
                    }

                    return $"data:{mime};base64,{Convert.ToBase64String(anexo)}";
                }
            }

            return anexoString;
        }

        public void AtualizarDescricaoAnexo(IntervenienteAnexoAtualizarRequest anexo)
        {
            _intervenienteService.AtualizarDescricaoAnexo(AutoMapper.Mapper.Map(anexo, new IntervenienteAnexo()));
        }

        public void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            _intervenienteService.RemoverAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);
        }

        //Public Integration
        public ResultDTO<IntervenienteAdicionarResponse> IntervenienteAdicionar(IntervenienteAdicionarRequest[] requests)
        {
            var errorsInRequest = new List<string[]>();
            var errorsReturn = new List<Error>();

            foreach (IntervenienteAdicionarRequest request in requests)
            {
                errorsInRequest = _intervenienteService.ValidaCamposRequestAdicionarInterveniente(request.CpfCnpj, request.IdExterno, request.EnderecoMunicipioCodigo, request.PortadorCobranca, request.VendedorCodigo, request.EnderecoNumero, request.EnderecoComplemento);

                if (errorsInRequest.Count > 0)
                {
                    foreach (string[] erros in errorsInRequest)
                    {
                        errorsReturn.Add(new Error(erros[0], erros[1], errorsInRequest.IndexOf(erros)));
                    }
                }
            }
            
            if (errorsReturn.Count > 0) return new ResultDTO<IntervenienteAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                            errorsReturn);

            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (IntervenienteAdicionarRequest request in requests)
                    {
                        var newInterveniente = new Interveniente();

                        newInterveniente = AutoMapper.Mapper.Map(request, newInterveniente);
                        newInterveniente.IdAtualizacao = StringHelper.GetIDD("API");
                        _intervenienteService.Adicionar(newInterveniente);
                    }
                 
                    Commit();
                    scope.Complete();

                    var result = new IntervenienteAdicionarResponse(requests.Length);
                    return new ResultDTO<IntervenienteAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<IntervenienteAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<PublicoIntervenienteResponse> AtualizarPorId(int id, IntervenienteAtualizarRequest request)
        {
            Interveniente interveniente = _intervenienteService.ObterPorId(id);

            if (interveniente == null)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }

            var fieldsValidationResult = _intervenienteService.ValidaCamposRequestAtualizarInterveniente(request.CpfCnpj, request.IdExterno, request.EnderecoMunicipioCodigo, request.PortadorCobranca, request.VendedorCodigo, interveniente, request.EnderecoNumero, request.EnderecoComplemento);

            if (fieldsValidationResult.Count > 0)
            {
                var errorsReturn = new List<Error>();

                foreach (string[] errors in fieldsValidationResult)
                {
                    errorsReturn.Add(new Error(errors[0], errors[1], fieldsValidationResult.IndexOf(errors)));
                }

                return new ResultDTO<PublicoIntervenienteResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                            errorsReturn);
            }

            return AtualizarInterveniente(interveniente, request);
        }

        public ResultDTO<PublicoIntervenienteResponse> AtualizarPorCnpjCpf(string cnpjCpf, IntervenienteAtualizarRequest request)
        {
            Interveniente interveniente = _intervenienteService.ObterPorCnpjCpf(cnpjCpf.ToString());

            if (interveniente == null)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }

            List<string[]> fieldsValidationResult = _intervenienteService.ValidaCamposRequestAtualizarInterveniente(request.CpfCnpj, request.IdExterno, request.EnderecoMunicipioCodigo,request.PortadorCobranca,request.VendedorCodigo, interveniente, request.EnderecoNumero, request.EnderecoComplemento);

            if (fieldsValidationResult.Count > 0)
            {
                var errorsReturn = new List<Error>();

                foreach (string[] errors in fieldsValidationResult)
                {
                    errorsReturn.Add(new Error(errors[0], errors[1], fieldsValidationResult.IndexOf(errors)));
                }

                return new ResultDTO<PublicoIntervenienteResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                            errorsReturn);
            }

            return AtualizarInterveniente(interveniente, request);
        }

        public ResultDTO<PublicoIntervenienteResponse> AtualizarPorExternalId(string externalId, IntervenienteAtualizarRequest request)
        {
            Interveniente interveniente = _intervenienteService.ObterPorIdExterno(externalId);

            if (interveniente == null)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "external_id");
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }

            var fieldsValidationResult = _intervenienteService.ValidaCamposRequestAtualizarInterveniente(request.CpfCnpj, request.IdExterno, request.EnderecoMunicipioCodigo, request.PortadorCobranca, request.VendedorCodigo, interveniente, request.EnderecoNumero, request.EnderecoComplemento);

            if (fieldsValidationResult.Count > 0)
            {
                var errorsReturn = new List<Error>();

                foreach (string[] errors in fieldsValidationResult)
                {
                    errorsReturn.Add(new Error(errors[0], errors[1], fieldsValidationResult.IndexOf(errors)));
                }

                return new ResultDTO<PublicoIntervenienteResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                            errorsReturn);
            }

            return AtualizarInterveniente(interveniente, request);
        }

        public ResultDTO<PublicoIntervenienteResponse> AtualizarInterveniente(Interveniente interveniente, IntervenienteAtualizarRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var newInterveniente = AutoMapper.Mapper.Map(request, interveniente);
                    newInterveniente.IdAtualizacao = StringHelper.GetIDD("API");

                    _intervenienteService.Atualizar(newInterveniente);
                    Commit();
                    scope.Complete();

                    return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(interveniente, new PublicoIntervenienteResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }  
        }

        public ResultDTO<PagedList<PublicoIntervenienteResponse>> Listar(int page = 0, int limit = 0)
        {
            try
            {
                if (page == 0) page = 1;
                if (limit == 0 || limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_intervenienteService.Listar(page, limit), new PagedList<PublicoIntervenienteResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            } 
        }

        public ResultDTO<PublicoIntervenienteResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_intervenienteService.ObterPorId(id), new PublicoIntervenienteResponse());

                if (result == null)
                {
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                    return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }

                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PublicoIntervenienteResponse> ObterPorExternalId(string externalId)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_intervenienteService.ObterPorIdExterno(externalId), new PublicoIntervenienteResponse());

                if (result == null)
                {
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                    return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }

                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PublicoIntervenienteResponse> ObterPorCnpjCpf(string CnpjCpf)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_intervenienteService.ObterPorCnpjCpf(CnpjCpf), new PublicoIntervenienteResponse());

                if (result == null)
                {
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode();
                    return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }

                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PublicoIntervenienteResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<PublicoIntervenienteResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_intervenienteService.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit), new PagedList<PublicoIntervenienteResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<PublicoIntervenienteResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}

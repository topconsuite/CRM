using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Response.Portador;
using TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.PortadorCobranca;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;

namespace TopSys.TopConWeb.Application
{
    public class PortadorApplicationService : ApplicationServiceBase<Portador>, IPortadorApplicationService
    {
        private readonly IPortadorService _portadorService;
        private readonly IEmpresaService _empresaService;
        private readonly IBancoService _bancoService;
        private readonly ISituacaoFinanceiraService _situacaoFinanceiraService;
        private readonly IHeaderProvider _headerProvider;

        public PortadorApplicationService(IPortadorService portadorService, IEmpresaService empresaService,
            IBancoService bancoService, ISituacaoFinanceiraService situacaoFinanceiraService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(portadorService, unityOfWork)
        {
            _portadorService = portadorService;
            _empresaService = empresaService;
            _bancoService = bancoService;
            _situacaoFinanceiraService = situacaoFinanceiraService;
            _headerProvider = headerProvider;
        }


        public IEnumerable<PortadorResponse> ListarVinculadosComContas()
        {
            return AutoMapper.Mapper.Map(_portadorService.ListarVinculadosComContas(), new List<PortadorResponse>());
        }
        
        public ResultDTO<PortadorCobrancaAdicionarResponse> Adicionar(PortadorCobrancaAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verifica Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_portadorService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                     errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "code"),
                        i));

                if (_empresaService.ObterPorId(request[i].ContaEmpresaCodigo)==null)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "company"),
                        i));

                if (_bancoService.ObterPorId(request[i].ContaCodigo, request[i].ContaEmpresaCodigo) == null)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bank_code"),
                        i));

                if (_situacaoFinanceiraService.ObterPorId(request[i].Situacao) == null)
                    errors.Add(new Error(
                       EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                       EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "situation"),
                       i));

            }
            
            if (errors.Count > 0)
                return new ResultDTO<PortadorCobrancaAdicionarResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);


            //Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var cadastro in request)
                    {
                        var newPortador = new Portador();

                        newPortador = AutoMapper.Mapper.Map(cadastro, newPortador);
                
                        _portadorService.Adicionar(newPortador);
                    }

                    Commit();
                    scope.Complete();

                    var result = new PortadorCobrancaAdicionarResponse(request.Length);
                    return new ResultDTO<PortadorCobrancaAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch 
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PortadorCobrancaAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<PortadorCobrancaResponse> AtualizarPorId(int id, PortadorCobrancaAtualizarRequest request)
        {
            var portador = _portadorService.ObterPorId(id, true);
            if (portador == null)
                return new ResultDTO<PortadorCobrancaResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            
            return Atualizar(portador, request);
        }

        public ResultDTO<PortadorCobrancaResponse> Atualizar(Portador portador,
            PortadorCobrancaAtualizarRequest request)
        {
            // Verifica Erros
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (_empresaService.ObterPorId(request.ContaEmpresaCodigo ?? 0)==null && request.ContaEmpresaCodigo != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "Company")));

            if (request.ContaCodigo == null && request.ContaEmpresaCodigo != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "bank_code")));

            if (request.ContaEmpresaCodigo == null && request.ContaCodigo != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "company")));

            if (_bancoService.ObterPorId(request.ContaCodigo ?? 0, request.ContaEmpresaCodigo ?? 0) == null && request.ContaCodigo != null && request.ContaEmpresaCodigo != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "Bank_code")));

            if (_situacaoFinanceiraService.ObterPorId(request.Situacao ?? 0) == null && request.Situacao != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "Situation")));

            if (errors.Count > 0)
                return new ResultDTO<PortadorCobrancaResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);

            using (var scope = new TransactionScope())
            {
                try
                {
                    portador = AutoMapper.Mapper.Map(request, portador);

                    Commit();   
                    scope.Complete();

                    return new ResultDTO<PortadorCobrancaResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(portador, new PortadorCobrancaResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PortadorCobrancaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
            
        }
        
        public ResultDTO<ICollection<PortadorCobrancaResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_portadorService.Listar(), new List<PortadorCobrancaResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<PortadorCobrancaResponse>>(EResultDTOStatus.Error, EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                    _headerProvider.GetAcceptLanguage()), EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<PortadorCobrancaResponse>>(EResultDTOStatus.Success, "", result);
        }
        
        public ResultDTO<PortadorCobrancaResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_portadorService.ObterPorId(id), new PortadorCobrancaResponse());

                if (result == null)
                    return new ResultDTO<PortadorCobrancaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PortadorCobrancaResponse>(EResultDTOStatus.Success, "", result);  
            }catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PortadorCobrancaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
            
        }
        
        public ResultDTO<int> DeletarPorId(int id)
        {
            var portador = _portadorService.ObterPorId(id, true);
            if (portador == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Deletar(portador);
        }

        public ResultDTO<int> Deletar(Portador portador)
        {
            try
            {
                var estaEmUso = _portadorService.EstaEmUsoPortador(portador.Codigo);
                if (estaEmUso)
                    return new ResultDTO<int>(
                        EResultDTOStatus.Alert,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Bill collector"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetMessageCode());

                _portadorService.Remover(portador);

                Commit();

                return new ResultDTO<int>(EResultDTOStatus.Success, "Successfully deleted");
            }
            catch 
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<int>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
            
        }
    }
}

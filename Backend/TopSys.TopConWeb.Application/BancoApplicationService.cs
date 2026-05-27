using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Banco;
using TopSys.TopConWeb.Application.DTOS.Response.Banco;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class BancoApplicationService : ApplicationServiceBase<Conta>, IBancoApplicationService
    {
        private readonly IBancoService _bancoService;
        private readonly IEmpresaService _empresaService;
        private readonly IHeaderProvider _headerProvider;

        public BancoApplicationService(IBancoService bancoService, IEmpresaService empresaService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(bancoService, unityOfWork)
        {
            _bancoService = bancoService;
            _empresaService = empresaService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<BancoAdicionarResponse> Adicionar(BancoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_bancoService.ObterPorId(request[i].Codigo, request[i].EmpresaCodigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Key (code + company)"),
                            i));

                if (_empresaService.ObterPorId(request[i].EmpresaCodigo) == null)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "company"),
                        i));

                if (request[i].EmpresaProprietaria != 0)
                {
                    if (_empresaService.ObterPorId(request[i].EmpresaProprietaria) == null)
                        errors.Add(new Error(
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "owner_company"),
                            i));
                }
            }

            if (errors.Count > 0)
                return new ResultDTO<BancoAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors); ;

            // Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var cadastro in request)
                    {
                        var newBanco = new Conta();

                        newBanco = AutoMapper.Mapper.Map(cadastro, newBanco);

                        _bancoService.Adicionar(newBanco);
                    }

                    Commit();
                    scope.Complete();

                    var result = new BancoAdicionarResponse(request.Length);

                    return new ResultDTO<BancoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<BancoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<BancoResponse> AtualizarPorId(int cod, int emp, BancoAtualizarRequest request)
        {
            var banco = _bancoService.ObterPorId(cod, emp, true);
            if (banco == null)
                return new ResultDTO<BancoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Atualizar(banco, request);
        }

        public ResultDTO<BancoResponse> Atualizar(Conta banco, BancoAtualizarRequest request)
        {
            if (request.EmpresaProprietaria != 0 && request.EmpresaProprietaria != null)
            {
                if (_empresaService.ObterPorId(request?.EmpresaProprietaria ?? 0) == null)
                    return new ResultDTO<BancoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    banco = AutoMapper.Mapper.Map(request, banco);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<BancoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(banco, new BancoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<BancoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<BancoResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_bancoService.Listar(), new List<BancoResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<BancoResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<BancoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<BancoResponse> ObterPorId(int cod, int emp)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_bancoService.ObterPorId(cod, emp), new BancoResponse());

                if (result == null)
                    return new ResultDTO<BancoResponse>(
                     EResultDTOStatus.Error,
                     EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                     EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<BancoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<BancoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<int> DeletarPorId(int cod, int emp)
        {
            var banco = _bancoService.ObterPorId(cod, emp, true);
            if (banco == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Deletar(banco);
        }

        public ResultDTO<int> Deletar(Conta banco)
        {
            try
            {
                var estaEmUso = _bancoService.EstaEmUsoBanco(banco.Codigo, banco.EmpresaCodigo);
                if (estaEmUso)
                    return new ResultDTO<int>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "bank"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetMessageCode());

                _bancoService.Remover(banco);

                Commit();

                return new ResultDTO<int>(EResultDTOStatus.Success, "Successfully deleted");
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<int>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}

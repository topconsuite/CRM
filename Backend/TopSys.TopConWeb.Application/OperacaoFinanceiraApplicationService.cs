using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.OperacaoFinanceira;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;


namespace TopSys.TopConWeb.Application
{
    public class OperacaoFinanceiraApplicationService : ApplicationServiceBase<OperacaoFinanceira>,
        IOperacaoFinanceiraApplicationService
    {
        private readonly IOperacaoFinanceiraService _operacaoFinanceiraService;
        private readonly ICentroCustoService _centroCustoService;
        private readonly IHeaderProvider _headerProvider;

        public OperacaoFinanceiraApplicationService(IOperacaoFinanceiraService operacaoFinanceiraService,
            ICentroCustoService centroCustoService,
            IHeaderProvider headerProvider, IUnitOfWork unityOfWork) : base(operacaoFinanceiraService, unityOfWork)
        {
            _operacaoFinanceiraService = operacaoFinanceiraService;
            _centroCustoService = centroCustoService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<OperacaoFinanceiraAdicionarResponse> Adicionar(OperacaoFinanceiraAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verifica Erros
            for (int i = 0; i < request.Length; i++)
            {
                var opBaixa = _operacaoFinanceiraService.ObterPorId(request[i].OperacaoBaixa ?? 0);
                var opMovBco = _operacaoFinanceiraService.ObterPorId(request[i].OperacaoMovBco ?? 0);

                var codeExists = (_operacaoFinanceiraService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(
                            idioma, "code"),
                        i));

                if (request[i].SubSistema != "CR" && request[i].SubSistema != "CP" && request[i].SubSistema != "BC")
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetResourceMessage(idioma, "subsystem"), i));

                if (request[i].InclusaoOuBaixa != "I" && request[i].InclusaoOuBaixa != "B")
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetResourceMessage(idioma, "inclusion_discharge"), i));

                if (request[i].AtualizaBanco != 1 && request[i].AtualizaBanco != 2 && request[i].AtualizaBanco != 9)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetResourceMessage(idioma, "update_bank"), i));

                if (request[i].AtualizaBanco == 9 && (request[i].OperacaoBaixa ?? 0) != 0)
                    errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetMessageCode(),
                EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetResourceMessage(idioma, "sell_operation"), i));

                if (opBaixa?.InclusaoOuBaixa != "B" && (request[i].OperacaoBaixa ?? 0) != 0)
                    errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f32.GetMessageCode(),
                        EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f32.GetResourceMessage(idioma),
                        i));

                if (opBaixa == null && (request[i].OperacaoBaixa ?? 0) != 0)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "sell_operation"), 0));

                if (request[i].AtualizaBanco == 1 && (opMovBco?.SubSistema != "CR" || opMovBco?.InclusaoOuBaixa != "B") && (request[i].OperacaoMovBco ?? 0) != 0)
                    errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetMessageCode(),
                        EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetResourceMessage(idioma, "CR"),
                        i));

                if (request[i].AtualizaBanco == 2 && (opMovBco?.SubSistema != "CP" || opMovBco?.InclusaoOuBaixa != "B") && (request[i].OperacaoMovBco ?? 0) != 0)
                    errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetMessageCode(),
                        EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetResourceMessage(idioma, "CP"),
                        i));

                if (request[i].AtualizaBanco == 9 && (request[i].OperacaoMovBco ?? 0) != 0)
                    errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetMessageCode(),
                        EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetResourceMessage(idioma, "bank_movement_operation"),
                        i));

                if (opMovBco == null && (request[i].OperacaoMovBco ?? 0) != 0)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "bank_movement_operation"), 0));


                if (request[i].ReceitaDespesa != 1 && request[i].ReceitaDespesa != 2 && request[i].ReceitaDespesa != 9)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetResourceMessage(idioma, "revenue_expense"), i));

                errors.AddRange(from centroCusto in request[i].CentrosDeCusto
                    where _centroCustoService.ObterPorId(centroCusto) == null
                    select new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "cost_center_that_use"), i));

                if (request[i].CentrosDeCusto.GroupBy(x => x).Any(g => g.Count() > 1))
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetResourceMessage(idioma, "CostCenter", "cost_center_that_use"), i));
            }

            if (errors.Count > 0)
                return new ResultDTO<OperacaoFinanceiraAdicionarResponse>(EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma), errors);


            // Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var cadastro in request)
                    {
                        var newOperacaoFinanceira = new OperacaoFinanceira();

                        newOperacaoFinanceira = AutoMapper.Mapper.Map(cadastro, newOperacaoFinanceira);

                        _centroCustoService.Adicionar(newOperacaoFinanceira);
                    }

                    Commit();
                    scope.Complete();

                    var result = new OperacaoFinanceiraAdicionarResponse(request.Length);

                    return new ResultDTO<OperacaoFinanceiraAdicionarResponse>(EResultDTOStatus.Success,
                        "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<OperacaoFinanceiraAdicionarResponse>(EResultDTOStatus.Error, errorMessage,
                        errorCode);
                }
            }
        }

        public ResultDTO<OperacaoFinanceiraResponse> AtualizarPorId(int id, OperacaoFinanceiraAtualizarRequest request)
        {
            var operacaoFinanceira = _operacaoFinanceiraService.ObterPorId(id, true);
            if (operacaoFinanceira == null)
                return new ResultDTO<OperacaoFinanceiraResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Atualizar(operacaoFinanceira, request);
        }

        public ResultDTO<OperacaoFinanceiraResponse> Atualizar(OperacaoFinanceira operacaoFinanceira,
            OperacaoFinanceiraAtualizarRequest request)
        {
            // Verifica Erros
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();
            var opBaixa = _operacaoFinanceiraService.ObterPorId(request.OperacaoBaixa ?? operacaoFinanceira.OperacaoBaixa);
            var opMovBco = _operacaoFinanceiraService.ObterPorId(request.OperacaoMovBco ?? operacaoFinanceira.OperacaoMovBco);

            if (request.SubSistema != "CR" && request.SubSistema != "CP" && request.SubSistema != "BC" &&
                request.SubSistema != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetResourceMessage(idioma, "subsystem"), 0));

            if (request.InclusaoOuBaixa != "I" && request.InclusaoOuBaixa != "B" && request.InclusaoOuBaixa != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetResourceMessage(idioma, "inclusion_discharge"), 0));

            if (request.AtualizaBanco != 1 && request.AtualizaBanco != 2 && request.AtualizaBanco != 9 &&
                request.AtualizaBanco != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetResourceMessage(idioma, "update_bank"), 0));

            if ((request.AtualizaBanco ?? operacaoFinanceira.AtualizaBanco) == 9  && (request.OperacaoBaixa ?? operacaoFinanceira.OperacaoBaixa) != 0)
                errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetMessageCode(),
                    EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetResourceMessage(idioma, "sell_operation"), 0));

            if (opBaixa?.InclusaoOuBaixa != "B" && (request.OperacaoBaixa ?? operacaoFinanceira.OperacaoBaixa) != 0)
                errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f32.GetMessageCode(),
                    EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f32.GetResourceMessage(idioma),
                    0));

            if (opBaixa == null && (request.OperacaoBaixa ?? operacaoFinanceira.OperacaoBaixa) != 0)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                        idioma, "sell_operation"), 0));

            if ((request.AtualizaBanco ?? operacaoFinanceira.AtualizaBanco) == 1 && (opMovBco?.SubSistema != "CR" || opMovBco?.InclusaoOuBaixa != "B") && (request.OperacaoMovBco ?? operacaoFinanceira.OperacaoMovBco) != 0)
                errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetMessageCode(),
                    EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetResourceMessage(idioma, "CR"),
                    0));

            if ((request.AtualizaBanco ?? operacaoFinanceira.AtualizaBanco) == 2 && (opMovBco?.SubSistema != "CP" || opMovBco?.InclusaoOuBaixa != "B") && (request.OperacaoMovBco ?? operacaoFinanceira.OperacaoMovBco) != 0)
                errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetMessageCode(),
                    EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f33.GetResourceMessage(idioma, "CP"),
                    0));

            if ((request.AtualizaBanco ?? operacaoFinanceira.AtualizaBanco) == 9 && (request.OperacaoMovBco ?? operacaoFinanceira.OperacaoMovBco) != 0)
                errors.Add(new Error(EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetMessageCode(),
                    EResourcesOperacaoFinanceira.OPERACAO_FINANCEIRA_ERROR_TCON375f325f31.GetResourceMessage(idioma, "bank_movement_operation"),
                    0));

            if (opMovBco == null && (request.OperacaoMovBco ?? operacaoFinanceira.OperacaoMovBco) != 0)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                        idioma, "bank_movement_operation"), 0));


            if (request.ReceitaDespesa != 1 && request.ReceitaDespesa != 2 && request.ReceitaDespesa != 9 &&
                request.ReceitaDespesa != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                        .GetResourceMessage(idioma, "revenue_expense"), 0));


            if (request.CentrosDeCusto != null)
            {
                errors.AddRange(from centroCusto in request.CentrosDeCusto
                    where _centroCustoService.ObterPorId(centroCusto) == null
                    select new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "cost_center_that_use"), 0));

                if (request.CentrosDeCusto.GroupBy(x => x).Any(g => g.Count() > 1))
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetResourceMessage(idioma, "CostCenter", "cost_center_that_use", 0)));
            }

            if (errors.Count > 0)
                return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Error, EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);

            //Atualiza
            using (var scope = new TransactionScope())
            {
                try
                {
                    operacaoFinanceira = AutoMapper.Mapper.Map(request, operacaoFinanceira);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Success, "Successfully updated",
                        AutoMapper.Mapper.Map(operacaoFinanceira, new OperacaoFinanceiraResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<OperacaoFinanceiraResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_operacaoFinanceiraService.Listar(),
                new List<OperacaoFinanceiraResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<OperacaoFinanceiraResponse>>(EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<OperacaoFinanceiraResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<OperacaoFinanceiraResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_operacaoFinanceiraService.ObterPorId(id),
                    new OperacaoFinanceiraResponse());

                if (result == null)
                    return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage =
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<OperacaoFinanceiraResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<int> DeletarPorId(int id)
        {
            var operacaoFinanceira = _operacaoFinanceiraService.ObterPorId(id, true);
            if (operacaoFinanceira == null)
                return new ResultDTO<int>(EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Deletar(operacaoFinanceira);
        }

        public ResultDTO<int> Deletar(OperacaoFinanceira operacaoFinanceira)
        {
            try
            {
                var estaEmUso = _operacaoFinanceiraService.EstaEmUsoOperacaoFinanceira(operacaoFinanceira.Codigo);
                if (estaEmUso)
                    return new ResultDTO<int>(EResultDTOStatus.Alert,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage(), "Finance Operation"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetMessageCode());

                _operacaoFinanceiraService.Remover(operacaoFinanceira);

                Commit();

                return new ResultDTO<int>(EResultDTOStatus.Success, "Successfully deleted");
            }
            catch
            {
                var errorMessage =
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<int>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
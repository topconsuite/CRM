using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Equipamento;
using TopSys.TopConWeb.Application.DTOS.Response.Equipamento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class EquipamentoApplicationService : ApplicationServiceBase<Equipamento>, IEquipamentoApplicationService
    {

        private readonly IEquipamentoService _equipamentoService;
        private readonly IParametroService _parametroService;
        private readonly ICadastroGeralService _cadastroGeralService;
        private readonly IUsinaService _usinaService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly IHeaderProvider _headerProvider;

        public EquipamentoApplicationService(
            IEquipamentoService equipamentoService,
            IParametroService parametroService,
            ICadastroGeralService cadastroGeralService,
            IUsinaService usinaService,
            IIntervenienteService intervenienteService,
            IUnitOfWork unityOfWork,
            IHeaderProvider headerProvider) : base(equipamentoService, unityOfWork)
        {
            _equipamentoService = equipamentoService;
            _parametroService = parametroService;
            _cadastroGeralService = cadastroGeralService;
            _usinaService = usinaService;
            _intervenienteService = intervenienteService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<EquipamentoAdicionarResponse> Adicionar(EquipamentoAdicionarRequest[] request)
        {
            using (var scope = new TransactionScope())
            {
                int i = 0;
                try
                {
                    // == Validar Request =====================================================================

                    var prefixoBetoneira = _parametroService.ObterParametroN("TopCon", "BetoneiraPrefixo");
                    var equipamentosAdicionar = new List<EquipamentoAdicionarRequest>();
                    var listaTipoEquipamento = _cadastroGeralService.Listar(ECadastroGeralTipo.EquipamentoTipo).ToList();

                    var errors = new List<Error>();
                    var idioma = _headerProvider.GetAcceptLanguage();

                    for (i = 0; i < request.Length; i++)
                    {
                        var equipamento = request[i];
                        var tipoEquipamento = listaTipoEquipamento.Where(x => x.Codigo == equipamento.Tipo).FirstOrDefault();
                        var codeAlreadyExist = _equipamentoService.ObterPorId(equipamento.Codigo) != null;
                        var externalIdAlreadyExist = _equipamentoService.ObterPorExternalId(equipamento.ExternalId) != null;
                        var licensePlateAlreadyExist = _equipamentoService.ObterPorPlaca(equipamento.Placa) != null;
                        var concreteBatchingPlantIsValid = equipamento.UsinaAlocada == 0 ? true : (_usinaService.ObterPorId(equipamento.UsinaAlocada) != null);

                        if (codeAlreadyExist)
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393239.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393239.GetResourceMessage(idioma),
                                i));

                        if (externalIdAlreadyExist)
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323137.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323137.GetResourceMessage(idioma),
                                i));

                        if (licensePlateAlreadyExist)
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323130.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323130.GetResourceMessage(idioma),
                                i));

                        if (!concreteBatchingPlantIsValid)
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323135.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323135.GetResourceMessage(idioma),
                                i));

                        if (tipoEquipamento is null)
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393231.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393231.GetResourceMessage(idioma),
                                i));
                        else if ((tipoEquipamento.Codigo == 6101 || tipoEquipamento.Descricao.Equals("BETONEIRA")) && (prefixoBetoneira != "XX" && !equipamento.Codigo.StartsWith(prefixoBetoneira)))
                            errors.Add(new Error(
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393236.GetMessageCode(),
                                EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393236.GetResourceMessage(idioma, prefixoBetoneira),
                                i));
                    }

                    if (errors.Count > 0)
                        return new ResultDTO<EquipamentoAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

                    // Inserção =================================================================================

                    for (i = 0; i < request.Length; i++)
                    {
                        var equipamento = AutoMapper.Mapper.Map(request[i], new Equipamento());

                        equipamento.FinalPlaca = equipamento.Placa[equipamento.Placa.Length - 1].ToString();
                        equipamento.ControlaKm = false;
                        equipamento.Betoneira = equipamento.Tipo == 6101 ? "S" : "N";
                        equipamento.IdCadastro = StringHelper.GetIDD("APICRM");
                        equipamento.IdAtual = "";
                        equipamento.Ativo = "S";
                        equipamento.FuncionarioAlocado = "";
                        equipamento.Gps = "";
                        equipamento.PossuiGps = "";

                        if (equipamento.CapacidadeM3 == 0)
                            equipamento.CapacidadeM3 = 8;

                        this.Adicionar(equipamento);
                    }

                    Commit();
                    scope.Complete();

                    var result = new EquipamentoAdicionarResponse(request.Length);

                    return new ResultDTO<EquipamentoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting equipaments.", result, "");
                }
                catch(Exception e)
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetResourceMessage(_headerProvider.GetAcceptLanguage(), i-1);
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetMessageCode();
                    return new ResultDTO<EquipamentoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<EquipamentoResponse> AtualizarPorExternalID(string externalID, EquipamentoAtualizarRequest request)
        {

            var old = _equipamentoService.ObterPorExternalId(externalID, true);

            if(old == null)
                return new ResultDTO<EquipamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

            return Atualizar(old, request);

        }

        public ResultDTO<EquipamentoResponse> AtualizarPorID(string codigo, EquipamentoAtualizarRequest request)
        {
            var old = _equipamentoService.ObterPorId(codigo, true);

            if (old == null)
                return new ResultDTO<EquipamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<EquipamentoResponse> Atualizar(Equipamento equipamento, EquipamentoAtualizarRequest request)
        {
            try
            {
                var errors = new List<Error>();
                var idioma = _headerProvider.GetAcceptLanguage();

                var prefixoBetoneira = _parametroService.ObterParametroN("TopCon", "BetoneiraPrefixo");

                if (request.Tipo != null)
                {
                    var tipoEquipamento = _cadastroGeralService.ObterPorId((int)request.Tipo, ECadastroGeralTipo.EquipamentoTipo);
                    if (tipoEquipamento is null)
                        errors.Add(new Error(
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393231.GetMessageCode(),
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393231.GetResourceMessage(idioma)));
                    else if ((tipoEquipamento.Codigo == 6101 || tipoEquipamento.Descricao.Equals("BETONEIRA")) && (prefixoBetoneira != "XX" && equipamento.Codigo.StartsWith(prefixoBetoneira)))
                        errors.Add(new Error(
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393236.GetMessageCode(),
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393236.GetResourceMessage(idioma, prefixoBetoneira)));
                }

                if (request.Placa != null)
                {
                    var licensePlateAlreadyExist = request.Placa != equipamento.Placa ? (_equipamentoService.ObterPorPlaca(request.Placa) != null) : false;
                    if (licensePlateAlreadyExist)
                        errors.Add(new Error(
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323130.GetMessageCode(),
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323130.GetResourceMessage(idioma, prefixoBetoneira)));
                }

                if (request.UsinaAlocada != null)
                {
                    var concreteBatchingPlantIsValid = request.UsinaAlocada == 0 ? true : (_usinaService.ObterPorId((int)request.UsinaAlocada) != null);
                    if (!concreteBatchingPlantIsValid)
                        errors.Add(new Error(
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323135.GetMessageCode(),
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323135.GetResourceMessage(idioma, prefixoBetoneira)));
                }

                if (request.FuncionarioAlocado != null)
                {
                    var allocatedEmployeeIsValid = (request.FuncionarioAlocado == "" || request.FuncionarioAlocado == "0") ? true : (_intervenienteService.ObterPorId(int.Parse(request.FuncionarioAlocado)) != null);
                    if (!allocatedEmployeeIsValid)
                        errors.Add(new Error(
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323136.GetMessageCode(),
                            EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323136.GetResourceMessage(idioma, prefixoBetoneira)));
                }

                if (errors.Count > 0)
                    return new ResultDTO<EquipamentoResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

                equipamento = AutoMapper.Mapper.Map(request, equipamento);

                equipamento.FinalPlaca = equipamento.Placa[equipamento.Placa.Length - 1].ToString();
                equipamento.IdAtual = StringHelper.GetIDD("APICRM");

                Commit();

                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(equipamento, new EquipamentoResponse()));
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<object> DeletarPorExternalID(string externalId)
        {
            var old = _equipamentoService.ObterPorExternalId(externalId, true);

            if (old == null)
                return new ResultDTO<object>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

            return Deletar(old);
        }

        public ResultDTO<object> DeletarPorID(string codigo)
        {
            var old = _equipamentoService.ObterPorId(codigo, true);

            if (old == null)
                return new ResultDTO<object>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

            return Deletar(old);
        }

        public ResultDTO<object> Deletar(Equipamento equipamento)
        {
            try
            {

                if(_equipamentoService.JaFoiUtilizado(equipamento.Codigo))
                    return new ResultDTO<object>(
                        EResultDTOStatus.Error,
                        EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323134.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323134.GetMessageCode());

                _equipamentoService.Remover(equipamento);

                Commit();

                return new ResultDTO<object>(EResultDTOStatus.Success, "Successfully deleted");
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<object>(EResultDTOStatus.Error, errorMessage, errorCode);
            }

        }

        public ResultDTO<List<EquipamentoResponse>> Listar()
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_equipamentoService.ListarTodos(), new List<EquipamentoResponse>());

                return new ResultDTO<List<EquipamentoResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<EquipamentoResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
            
        }

        public ResultDTO<EquipamentoResponse> ObterPorExternalID(string codigo)
        {
            try
            {
                var equipamento = AutoMapper.Mapper.Map(_equipamentoService.ObterPorExternalId(codigo), new EquipamentoResponse());

                if (equipamento == null)
                    return new ResultDTO<EquipamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Success, "", equipamento);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
           
        }

        public ResultDTO<EquipamentoResponse> ObterPorID(string codigo)
        {
            try
            {
                var equipamento = AutoMapper.Mapper.Map(_equipamentoService.ObterPorId(codigo), new EquipamentoResponse());

                if (equipamento == null)
                    return new ResultDTO<EquipamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232.GetMessageCode());

                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Success, "", equipamento);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<EquipamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}

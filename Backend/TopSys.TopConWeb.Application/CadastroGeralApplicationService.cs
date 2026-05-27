using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CadastroGeralApplicationService : ApplicationServiceBase<CadastroGeral>, ICadastroGeralApplicationService
    {
        private readonly ICadastroGeralService _cadastroGeralService;
        private readonly IHeaderProvider _headerProvider;

        public CadastroGeralApplicationService(ICadastroGeralService cadastroGeralService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(cadastroGeralService, unityOfWork)
        {
            _cadastroGeralService = cadastroGeralService;
            _headerProvider = headerProvider;
        }

        public ICollection<CadastroGeralResponse> ListarFuncoes()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarFuncoes(), new List<CadastroGeralResponse>());
        }

        public ICollection<CadastroGeralResponse> ListarMotivosBloqueioInterveniente()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarMotivosBloqueioInterveniente(), new List<CadastroGeralResponse>());
             
        }

        public ICollection<CadastroGeralResponse> ListarViasCaptacao()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarViasCaptacao(), new List<CadastroGeralResponse>());
        }

        public ICollection<CadastroGeralResponse> ListarTipoObra()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarTipoObra(), new List<CadastroGeralResponse>());
        }

        public ICollection<CadastroGeralResponse> ListarPorteObra()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarPorteObra(), new List<CadastroGeralResponse>());
        }

        public ICollection<CadastroGeralResponse> ListarTemposAprovacaoMedicao()
        {
            return AutoMapper.Mapper.Map(_cadastroGeralService.ListarTemposAprovacaoMedicao(), new List<CadastroGeralResponse>());
        }

        public ResultDTO<ICollection<CadastroGeralIntegracaoResponse>> Listar(ECadastroGeralTipo type)
        {
            var result = AutoMapper.Mapper.Map(_cadastroGeralService.Listar(type), new List<CadastroGeralIntegracaoResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<CadastroGeralIntegracaoResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetMessageCode());

            return new ResultDTO<ICollection<CadastroGeralIntegracaoResponse>>(EResultDTOStatus.Success, "", result);

        }

        public ResultDTO<CadastroGeralIntegracaoResponse> ObterPorId(int id, ECadastroGeralTipo type)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_cadastroGeralService.ObterPorId(id, type), new CadastroGeralIntegracaoResponse());

                if (result == null)
                    return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Error,
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetMessageCode());

                return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<CadastroGeralIntegracaoResponse> ObterPorExternalId(string externalId, ECadastroGeralTipo type)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_cadastroGeralService.ObterPorExternalId(externalId, type), new CadastroGeralIntegracaoResponse());

                if (result == null)
                    return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Error,
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393131.GetMessageCode());

                return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Success, "", result);
 }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<CadastroGeralIntegracaoAdicionarResponses> Adicionar(CadastroGeralIntegracaoAdicionarRequest[] request, ECadastroGeralTipo type)
        {
            using (var scope = new TransactionScope())
            {
                int i = 0;
                try
                {
                    // == Validar Request =====================================================================
                    var errors = new List<Error>();
                    var idioma = _headerProvider.GetAcceptLanguage();

                    for (i = 0; i < request.Length; i++)
                    {
                        if (!VerificarSeIDValido(_cadastroGeralService.ObterProximoCodigo(type), type))
                        {
                            errors.Add(new Error(
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393133.GetMessageCode(),
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393133.GetResourceMessage(idioma),
                                i));
                        } 
                        else
                        {
                            var externalIdExists = (_cadastroGeralService.ObterPorExternalId(request[i].ExternalId, type) != null);

                            if (externalIdExists)
                                errors.Add(new Error(
                                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393132.GetResourceMessage(idioma),
                                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393132.GetMessageCode(),
                                    i));
                        }
                    }

                    if (errors.Count > 0)
                    {
                        return new ResultDTO<CadastroGeralIntegracaoAdicionarResponses>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);
                    }

                    // Inserção =================================================================================
            
                    for (i = 0; i < request.Length; i++)
                    {
                        var newCadastroGeral = new CadastroGeral()
                        {
                            Codigo = _cadastroGeralService.ObterProximoCodigo(type),
                            IdAtualizacao = "",
                            IdCadastro = ""
                        };

                        newCadastroGeral = AutoMapper.Mapper.Map(request[i], newCadastroGeral);

                        _cadastroGeralService.Adicionar(newCadastroGeral);
                        Commit();
                    }

                    Commit();
                    scope.Complete();

                    var result = new CadastroGeralIntegracaoAdicionarResponses(request.Length);

                    return new ResultDTO<CadastroGeralIntegracaoAdicionarResponses>(EResultDTOStatus.Success, "Success when inserting records.", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetResourceMessage(_headerProvider.GetAcceptLanguage(), i-1);
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetMessageCode();
                    return new ResultDTO<CadastroGeralIntegracaoAdicionarResponses>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<CadastroGeralIntegracaoResponse> AtualizarId(int id, CadastroGeralIntegracaoAtualizarRequest request, ECadastroGeralTipo type)
        {

            var old = _cadastroGeralService.ObterPorId(id, type, true);

            if(old == null)
                return new ResultDTO<CadastroGeralIntegracaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393137.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393137.GetMessageCode());

            return Atualizar(old, request, type);

        }

        public ResultDTO<CadastroGeralIntegracaoResponse> AtualizarExternalId(string externalId, CadastroGeralIntegracaoAtualizarRequest request, ECadastroGeralTipo type)
        {

            var old = _cadastroGeralService.ObterPorExternalId(externalId, type, true);

            if (old == null)
                return new ResultDTO<CadastroGeralIntegracaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393138.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393138.GetMessageCode());

            return Atualizar(old, request, type);

        }

        public ResultDTO<CadastroGeralIntegracaoResponse> Atualizar(CadastroGeral old, CadastroGeralIntegracaoAtualizarRequest request, ECadastroGeralTipo type)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var idioma = _headerProvider.GetAcceptLanguage();
                    var atualizarCodigo = !(request.Codigo is null) && old.Codigo != request.Codigo;

                    if (atualizarCodigo)
                    {
                        if (!VerificarSeIDValido((int)request.Codigo, type))
                            return new ResultDTO<CadastroGeralIntegracaoResponse>(
                                EResultDTOStatus.Error,
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393133.GetResourceMessage(idioma),
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393133.GetMessageCode());

                        var codeExists = (_cadastroGeralService.ObterPorId((int)request.Codigo, type) != null);

                        if (codeExists)
                            return new ResultDTO<CadastroGeralIntegracaoResponse>(
                                EResultDTOStatus.Error,
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393134.GetResourceMessage(idioma),
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393134.GetMessageCode());

                        if (_cadastroGeralService.EstaEmUsoCadastroGeral(old.Codigo, type))
                            return new ResultDTO<CadastroGeralIntegracaoResponse>(
                                EResultDTOStatus.Error,
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON39313130.GetResourceMessage(idioma),
                                EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON39313130.GetMessageCode());
                    }

            
                    old = AutoMapper.Mapper.Map(request, old);

                    Commit();

                    if (atualizarCodigo)
                    {
                        _cadastroGeralService.AtualizarId(old.Codigo, (int)request.Codigo);
                        old.Codigo = (int)request.Codigo;
                    }

                    scope.Complete();

                    return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(old, new CadastroGeralIntegracaoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<CadastroGeralIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<int> DeletarPorId(int id, ECadastroGeralTipo type)
        {
            var old = _cadastroGeralService.ObterPorId(id, type, true);

            if (old == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393137.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393137.GetMessageCode());

            return Deletar(old, type);
           
        }

        public ResultDTO<int> DeletarPorExternalId(string externalId, ECadastroGeralTipo type)
        {
            var old = _cadastroGeralService.ObterPorExternalId(externalId, type, true);

            if (old == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393138.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393138.GetMessageCode());

            return Deletar(old, type);
        }

        public ResultDTO<int> Deletar(CadastroGeral cadastroGeral, ECadastroGeralTipo type)
        {
            try
            {
                var estaEmUso = _cadastroGeralService.EstaEmUsoCadastroGeral(cadastroGeral.Codigo, type);

                if (estaEmUso)
                    return new ResultDTO<int>(
                        EResultDTOStatus.Error,
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393139.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesCadastroGeral.CADASTRO_GERAL_ERROR_TCON393139.GetMessageCode());
                _cadastroGeralService.Remover(cadastroGeral);

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

        public Boolean VerificarSeIDValido(int id, ECadastroGeralTipo type)
        {
            switch(type)
            {
                case ECadastroGeralTipo.FuncionarioFuncao:
                    return (id >= 6900) && (id <= 7099);
                case ECadastroGeralTipo.FuncionarioDepartamento:
                    return ((id >= 8000) && (id <= 8099)) || ((id >= 9200) && (id <= 9299));
                case ECadastroGeralTipo.EquipamentoTipo:
                    return (id >= 6100) && (id <= 6199);
                case ECadastroGeralTipo.FuncionarioStatus:
                    return (id >= 7300) && (id <= 7399);
                default:
                    return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Vendedor;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class VendedorApplicationService : ApplicationServiceBase<Vendedor>, IVendedorApplicationService
    {
        private readonly IVendedorService _vendedorService;
        private readonly IUsuarioService _usuarioService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly IFuncionarioService _funcionarioService;
        private readonly IEnderecoService _enderecoService;
        private readonly IMunicipioService _municipioService;
        private readonly IHeaderProvider _headerProvider;

        public VendedorApplicationService(IVendedorService vendedorService, IUsuarioService usuarioService
            , IIntervenienteService intervenienteService, ICondicaoPagamentoService condicaoPagamentoService
            , IFuncionarioService funcionarioService, IEnderecoService enderecoService
            , IMunicipioService municipioService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider) : base(vendedorService, unityOfWork)
        {
            _vendedorService = vendedorService;
            _usuarioService = usuarioService;
            _intervenienteService = intervenienteService;
            _condicaoPagamentoService = condicaoPagamentoService;
            _funcionarioService = funcionarioService;
            _enderecoService = enderecoService;
            _municipioService = municipioService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<List<VendedorIntegracaoResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_vendedorService.Listar(), new List<VendedorIntegracaoResponse>());
            return new ResultDTO<List<VendedorIntegracaoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<VendedorIntegracaoResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_vendedorService.ObterPorId(id), new VendedorIntegracaoResponse());

                if (result == null)
                    return new ResultDTO<VendedorIntegracaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393431.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393431.GetMessageCode());

                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<VendedorIntegracaoResponse> ObterPorExternalId(string externalId)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_vendedorService.ObterPorExternalId(externalId), new VendedorIntegracaoResponse());

                if (result == null)
                    return new ResultDTO<VendedorIntegracaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393431.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393431.GetMessageCode());

                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<VendedorIntegracaoAdicionarResponse> Adicionar(VendedorIntegracaoAdicionarRequest[] request)
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
                        var vendedor = request[i];
                
                        vendedor.Codigo = _vendedorService.ObterProximoCodigo();

                        if (!string.IsNullOrEmpty(vendedor.Celular))
                        {
                            vendedor.Celular = Regex.Replace(vendedor.Celular, @"\D", "");
                        }

                        var externalIdAlreadyExist = _vendedorService.ObterPorExternalId(vendedor.ExternalId);
                        if (externalIdAlreadyExist != null)
                            errors.Add(new Error(
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343139.GetMessageCode(),
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343139.GetResourceMessage(idioma),
                                i));

                        if (vendedor.Funcao.Equals("V"))
                        {
                            if (vendedor.Usuario.Length == 0)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON393438.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON393438.GetResourceMessage(idioma),
                                    i));
                        }
                        else if (vendedor.Funcao.Equals("R"))
                        {
                            if (vendedor.Interveniente == 0)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343130.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343130.GetResourceMessage(idioma),
                                    i));

                            if (vendedor.CondicaoPagamento == 0)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343131.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343131.GetResourceMessage(idioma),
                                    i));
                        }

                        if (vendedor.Usuario.Length != 0)
                        {
                            var userExists = (_usuarioService.ObterPorId(vendedor.Usuario) != null);
                            if (!userExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON393439.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON393439.GetResourceMessage(idioma),
                                    i));
                        }

                        if (vendedor.Interveniente != 0)
                        {
                            var intervenerExists = (_intervenienteService.ObterPorId(vendedor.Interveniente) != null);
                            if (!intervenerExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343132.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343132.GetResourceMessage(idioma),
                                    i));
                        }

                        if (vendedor.CondicaoPagamento != 0)
                        {
                            var paymentTermsExists = (_condicaoPagamentoService.ObterPorId(vendedor.CondicaoPagamento) != null);
                            if (!paymentTermsExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343133.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343133.GetResourceMessage(idioma),
                                    i));
                        }
                   
                        if (vendedor.Re != 0)
                        {
                            var reExists = _funcionarioService.ReEmUso(vendedor.Re);
                            if (!reExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343134.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343134.GetResourceMessage(idioma),
                                    i));

                            var reAlreadyUsed = _vendedorService.ReEmUsoVendedor(vendedor.Re);
                            if (reAlreadyUsed)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343137.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343137.GetResourceMessage(idioma),
                                    i));
                        }

                        if (vendedor.VendedorPadrinho != 0)
                        {
                            var godfatherSeller = _vendedorService.ObterPorId(vendedor.VendedorPadrinho);

                            var godfatherSellerExists = (godfatherSeller != null);
                            if (!godfatherSellerExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetResourceMessage(idioma),
                                    i));
                            else
                            {
                                if (!godfatherSeller.Funcao.Equals("V"))
                                    errors.Add(new Error(
                                        EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetMessageCode(),
                                        EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetResourceMessage(idioma),
                                        i));
                            }
                        }

                        if (vendedor.Cep.Length != 0)
                        {
                            var endereco = _enderecoService.ObterPorCep(vendedor.Cep);

                            if (endereco != null)
                            {
                                if (vendedor.EnderecoLogradouro.Length == 0)
                                    vendedor.EnderecoLogradouro = endereco.Logradouro;

                                if (endereco.MunicipioCodigo != 0)
                                    vendedor.Municipio = endereco.MunicipioCodigo;
                            }
                        }

                        if (vendedor.Municipio != 0)
                        {
                            var countyExists = (_municipioService.ObterPorId(vendedor.Municipio) != null);
                            if (!countyExists)
                                errors.Add(new Error(
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343138.GetMessageCode(),
                                    EResourcesVendedor.VENDEDOR_ERROR_TCON39343138.GetResourceMessage(idioma),
                                    i));
                        }
                    }

                    if (errors.Count > 0)
                    {
                        return new ResultDTO<VendedorIntegracaoAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);
                    }

                    // Inserção =================================================================================

                    for (i = 0; i < request.Length; i++)
                    {
                        var newVendedor = new Vendedor()
                        {
                            IdCadastro = "",
                            IdAtualizacao = ""
                        };

                        newVendedor = AutoMapper.Mapper.Map(request[i], newVendedor);

                        _vendedorService.Adicionar(newVendedor);
                    }

                    Commit();
                    scope.Complete();

                    var result = new VendedorIntegracaoAdicionarResponse(request.Length);

                    return new ResultDTO<VendedorIntegracaoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting sellers.", result, "");

                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetResourceMessage(_headerProvider.GetAcceptLanguage(), i-1);
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetMessageCode();
                    return new ResultDTO<VendedorIntegracaoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<VendedorIntegracaoResponse> Atualizar(Vendedor old, VendedorIntegracaoAtualizarRequest request)
        {
            try
            {
                var errors = new List<Error>();
                var idioma = _headerProvider.GetAcceptLanguage();
                if (!string.IsNullOrEmpty(request.Celular))
                {
                    request.Celular = Regex.Replace(request.Celular, @"\D", "");
                }

                if (request.Funcao != null)
                {
                    if (request.Funcao.Equals("V"))
                    {
                        if (request.Usuario != null && request.Usuario.Length == 0)
                            errors.Add(new Error(
                                EResourcesVendedor.VENDEDOR_ERROR_TCON393438.GetMessageCode(),
                                EResourcesVendedor.VENDEDOR_ERROR_TCON393438.GetResourceMessage(idioma)));
                    }

                    if (request.Funcao.Equals("R"))
                    {
                        if (request.Interveniente == 0)
                            errors.Add(new Error(
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343130.GetMessageCode(),
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343130.GetResourceMessage(idioma)));

                        if (request.CondicaoPagamento == 0)
                            errors.Add(new Error(
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343131.GetMessageCode(),
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343131.GetResourceMessage(idioma)));
                    }
                }

                if (request.Usuario != null && request.Usuario.Length != 0)
                {
                    var userExists = (_usuarioService.ObterPorId(request.Usuario) != null);
                    if (!userExists)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON393439.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON393439.GetResourceMessage(idioma)));
                }

                if (request.Interveniente != null && request.Interveniente != 0)
                {
                    var intervenerExists = (_intervenienteService.ObterPorId((int)request.Interveniente) != null);
                    if (!intervenerExists)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343132.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343132.GetResourceMessage(idioma)));
                }

                if (request.CondicaoPagamento != null && request.CondicaoPagamento != 0)
                {
                    var paymentTermsExists = (_condicaoPagamentoService.ObterPorId((int)request.CondicaoPagamento) != null);
                    if (!paymentTermsExists)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343133.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343133.GetResourceMessage(idioma)));
                }

                if (request.Re != null && request.Re != 0)
                {
                    var reAlreadyUsed = _vendedorService.ReEmUsoVendedor((int)request.Re, (int)request.Codigo);
                    if (reAlreadyUsed)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343137.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343137.GetResourceMessage(idioma)));
                }

                if (request.VendedorPadrinho != null && request.VendedorPadrinho != 0)
                {
                    var godfatherSeller = _vendedorService.ObterPorId((int)request.VendedorPadrinho);

                    var godfatherSellerExists = (godfatherSeller != null);
                    if (!godfatherSellerExists)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetResourceMessage(idioma)));
                    else
                    {
                        if (!godfatherSeller.Funcao.Equals("V"))
                            errors.Add(new Error(
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetMessageCode(),
                                EResourcesVendedor.VENDEDOR_ERROR_TCON39343135.GetResourceMessage(idioma)));
                    }
                }

                if (request.Cep != null && request.Cep.Length != 0)
                {
                    var endereco = _enderecoService.ObterPorCep(request.Cep);

                    if (endereco != null)
                    {
                        if (request.EnderecoLogradouro.Length == 0)
                            request.EnderecoLogradouro = endereco.Logradouro;
                    
                        if (endereco.MunicipioCodigo != 0)
                            request.Municipio = endereco.MunicipioCodigo;
                    }
                }

                if (request.Municipio != null && request.Municipio != 0)
                {
                    var countyExists = (_municipioService.ObterPorId((int)request.Municipio) != null);
                    if (!countyExists)
                        errors.Add(new Error(
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343138.GetMessageCode(),
                            EResourcesVendedor.VENDEDOR_ERROR_TCON39343138.GetResourceMessage(idioma)));
                }

                if (errors.Count > 0)
                {
                    return new ResultDTO<VendedorIntegracaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                        errors);
                }

                var celular = old.Celular;

                old = AutoMapper.Mapper.Map(request, old);

                if (old.Celular == 0 && celular != 0)
                {
                    old.Celular = celular;
                }

                _vendedorService.Atualizar(old);

                Commit();

                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(old, new VendedorIntegracaoResponse()));

            }
            catch
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<VendedorIntegracaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<VendedorIntegracaoResponse> AtualizarId(int id, VendedorIntegracaoAtualizarRequest request)
        {
            var old = _vendedorService.ObterPorId(id);

            if (old == null)
                return new ResultDTO<VendedorIntegracaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesVendedor.VENDEDOR_ERROR_TCON393434.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesVendedor.VENDEDOR_ERROR_TCON393434.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<VendedorIntegracaoResponse> AtualizarExternalId(string externalId, VendedorIntegracaoAtualizarRequest request)
        {
            var old = _vendedorService.ObterPorExternalId(externalId);

            if (old == null)
                return new ResultDTO<VendedorIntegracaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesVendedor.VENDEDOR_ERROR_TCON393435.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesVendedor.VENDEDOR_ERROR_TCON393435.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<int> Deletar(Vendedor vendedor)
        {
            try
            {
                var estaEmUso = _vendedorService.EstaEmUsoVendedor(vendedor.Codigo);

                if (estaEmUso)
                {
                    _vendedorService.InativarVendedor(vendedor.Codigo);

                    return new ResultDTO<int>(
                        EResultDTOStatus.Alert,
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393436.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesVendedor.VENDEDOR_ERROR_TCON393436.GetMessageCode());
                }

                _vendedorService.Remover(vendedor);

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

        public ResultDTO<int> DeletarPorId(int id)
        {
            var old = _vendedorService.ObterPorId(id, true);

            if (old == null)
                return new ResultDTO<int>(EResultDTOStatus.Error, "Id not founded", "TCON393434");

            return Deletar(old);
        }

        public ResultDTO<int> DeletarPorExternalId(string externalId)
        {
            var old = _vendedorService.ObterPorExternalId(externalId, true);

            if (old == null)
                return new ResultDTO<int>(EResultDTOStatus.Error, "External_id not founded.", "TCON393435");

            return Deletar(old);
        }
    }
}

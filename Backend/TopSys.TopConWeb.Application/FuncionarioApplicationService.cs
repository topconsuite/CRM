using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Response.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application
{
    public class FuncionarioApplicationService : ApplicationServiceBase<Funcionario>, IFuncionarioApplicationService
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICadastroGeralService _cadastroGeralService;
        private readonly IUsinaService _usinaService;
        private readonly IHeaderProvider _headerProvider;
        private readonly IFuncionarioComplementoService _funcionarioComplementoService;

        public FuncionarioApplicationService(IFuncionarioService funcionarioService, IUnitOfWork unityOfWork, IIntervenienteService intervenienteService, IUsuarioService usuarioService, ICadastroGeralService cadastroGeralService, IUsinaService usinaService, IHeaderProvider headerProvider, IFuncionarioComplementoService funcionarioComplementoService) : base(funcionarioService, unityOfWork)
        {
            _funcionarioService = funcionarioService;
            _intervenienteService = intervenienteService;
            _usuarioService = usuarioService;
            _cadastroGeralService = cadastroGeralService;
            _usinaService = usinaService;
            _headerProvider = headerProvider;
            _funcionarioComplementoService = funcionarioComplementoService;
        }

        public ResultDTO<FuncionarioAdicionarResponse> Adicionar(FuncionarioAdicionarRequest[] request)
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
                        var funcionario = request[i];

                        var cpf = ObterApenasNumeros(request[i].CPF);
                        var interveniente = _intervenienteService.ObterPorCnpjCpf(cpf);
                        var cpfJaExiste = interveniente is null ? false : (_funcionarioService.ListarFiltrados(f => f.CodigoInterveniente == interveniente.Codigo).FirstOrDefault() != null);
                        var externalIdJaExiste = _funcionarioService.ListarFiltrados(f => f.ExternalId == funcionario.ExternalId).FirstOrDefault() != null;
                        var userIsValid = _usuarioService.ObterPorId(funcionario.UsuarioSistema) != null;
                        var departmentIsValid = _cadastroGeralService.ObterPorId(funcionario.Departamento, ECadastroGeralTipo.FuncionarioDepartamento) != null;
                        var statusIsValid = _cadastroGeralService.ObterPorId(funcionario.Status, ECadastroGeralTipo.FuncionarioStatus) != null;
                        var rolesIsValid = _cadastroGeralService.ObterPorId(funcionario.Funcao, ECadastroGeralTipo.FuncionarioFuncao) != null;
                        var concreteBatchingPlantIsValid = _usinaService.ObterPorId(funcionario.Usina) != null;
                        var reJaExiste = _funcionarioService.ListarFiltrados(t => t.RE == funcionario.RE).ToList().Count >= 1; 

                        if (cpfJaExiste)
                            errors.Add(new Error(
                                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393532.GetMessageCode(),
                                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393532.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                    i));

                        if (reJaExiste)
                            errors.Add(new Error(
                                     EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393540.GetMessageCode(),
                                     EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393540.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                     i));

                        if (externalIdJaExiste)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393533.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393533.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));

                        if (!userIsValid)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393534.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393534.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));

                        if (!departmentIsValid)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393536.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393536.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));

                        if (!statusIsValid)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393537.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393537.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));

                        if (!rolesIsValid)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393538.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393538.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));

                        if (!concreteBatchingPlantIsValid)
                            errors.Add(new Error(
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393539.GetMessageCode(),
                                EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393539.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                                i));
                    }

                    if (errors.Count > 0)
                        return new ResultDTO<FuncionarioAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

                    // Inserção =================================================================================

                    for (i = 0; i < request.Length; i++)
                    {
                        var funcionario = new Funcionario()
                        {
                            Codigo = _funcionarioService.ObterProximoCodigo(),
                            IdCadastro = StringHelper.GetIDD("APICRM"),
                            IdAtual = ""
                        };

                        AutoMapper.Mapper.Map(request[i], funcionario);

                        _funcionarioService.Adicionar(funcionario);

                        Commit();

                        var cpf = ObterApenasNumeros(request[i].CPF);
                        var interveniente = _intervenienteService.ObterPorCnpjCpf(cpf);

                        var funcionarioComplemento = new FuncionarioComplemento()
                        {
                            Codigo = funcionario.Codigo,
                            CPF = cpf
                        };

                        _funcionarioComplementoService.Adicionar(funcionarioComplemento);

                        Commit();

                        if (interveniente is null)
                        {
                            interveniente = new Interveniente()
                            {
                                Nome = funcionario.Nome,
                                Razao = funcionario.Nome,
                                Funcionario = "S",
                                CpfCnpj = cpf,
                                Inativo = funcionario.Ativo == "S" ? "N" : "S",
                                Rg = "",
                                OrgaoExpedidor = "",
                                InscricaoEstadual = "",
                                InscricaoMunicipal = "",
                                EnderecoCep = "",
                                EnderecoLogradouro = "",
                                EnderecoComplemento = "",
                                EnderecoBairro = "",
                                Profissao = "",
                                EmpresaTrabalho = "",
                                NomeMae = "",
                                NomeConjuge = "",
                                Contato = "",
                                Email = "",
                                EmailCobranca = "",
                                Observacao = "",
                                Cliente = "N",
                                Fornecedor = "N",
                                Transportador = "N",
                                PrestadorServico = "N",
                                OrgaoPublico = "N",
                                Outros = "N",
                                Cei = "",
                                IdAtualizacao = "",
                                IdExterno = ""
                            };

                            _intervenienteService.Adicionar(interveniente);
                            Commit();
                            interveniente = _intervenienteService.ObterPorCnpjCpf(cpf);
                        }

                        funcionario.CodigoInterveniente = interveniente.Codigo;
                    }

                    Commit();
                    scope.Complete();

                    var result = new FuncionarioAdicionarResponse(request.Length);

                    return new ResultDTO<FuncionarioAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting employees.", result, "");
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetResourceMessage(_headerProvider.GetAcceptLanguage(), i - 1);
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR_IN_LIST.GetMessageCode();
                    return new ResultDTO<FuncionarioAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<FuncionarioResponse> AtualizarPorExternalId(string externalId, FuncionarioAtualizarRequest request)
        {
            var old = _funcionarioService.ListarFiltradosTracking(f => f.ExternalId == externalId).FirstOrDefault();

            if (old == null)
                return new ResultDTO<FuncionarioResponse>(
                    EResultDTOStatus.Error,
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<FuncionarioResponse> AtualizarPorId(int codigo, FuncionarioAtualizarRequest request)
        {
            var old = _funcionarioService.ObterPorId(codigo);

            if (old == null)
                return new ResultDTO<FuncionarioResponse>(
                    EResultDTOStatus.Error,
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetMessageCode());

            return Atualizar(old, request);
        }

        public ResultDTO<FuncionarioResponse> Atualizar(Funcionario funcionario, FuncionarioAtualizarRequest request)
        {
            try
            {
                var errors = new List<Error>();
                var idioma = _headerProvider.GetAcceptLanguage();

                var interveniente = request.CPF is null ? null : (_intervenienteService.ObterPorCnpjCpf(ObterApenasNumeros(request.CPF)));
                var funcionarioCpf = interveniente is null ? null : _funcionarioService.ListarFiltrados(f => f.CodigoInterveniente == interveniente.Codigo).FirstOrDefault();
                var cpfJaExiste = funcionarioCpf is null ? false : funcionarioCpf.Codigo != funcionario.Codigo;

                var userIsValid = (request.UsuarioSistema is null || request.UsuarioSistema == funcionario.UsuarioSistema)
                    ? true : (_usuarioService.ObterPorId(request.UsuarioSistema) != null);

                var departmentIsValid = (request.Departamento is null || request.Departamento == funcionario.Departamento)
                    ? true : _cadastroGeralService.ObterPorId(request.Departamento, ECadastroGeralTipo.FuncionarioDepartamento) != null;

                var statusIsValid = (request.Status is null || request.Status == funcionario.Status)
                    ? true : _cadastroGeralService.ObterPorId(request.Status, ECadastroGeralTipo.FuncionarioStatus) != null;

                var rolesIsValid = (request.Funcao is null || request.Funcao == funcionario.Funcao)
                    ? true : _cadastroGeralService.ObterPorId(request.Funcao, ECadastroGeralTipo.FuncionarioFuncao) != null;

                var concreteBatchingPlantIsValid = (request.Usina is null || request.Usina == funcionario.Usina)
                    ? true : (_usinaService.ObterPorId(request.Usina) != null);

                if (cpfJaExiste)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393532.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393532.GetResourceMessage(idioma)));

                if (!userIsValid)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393534.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393534.GetResourceMessage(idioma)));

                if (!departmentIsValid)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393536.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393536.GetResourceMessage(idioma)));

                if (!statusIsValid)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393537.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393537.GetResourceMessage(idioma)));

                if (!rolesIsValid)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393538.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393538.GetResourceMessage(idioma)));

                if (!concreteBatchingPlantIsValid)
                    errors.Add(new Error(
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393539.GetMessageCode(),
                        EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393539.GetResourceMessage(idioma)));

                if (errors.Count > 0)
                    return new ResultDTO<FuncionarioResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                        errors);

                funcionario = AutoMapper.Mapper.Map(request, funcionario);

                funcionario.IdAtual = StringHelper.GetIDD("APICRM");

                Commit();

                funcionario.Interveniente = _intervenienteService.ObterPorId(funcionario.CodigoInterveniente);

                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(funcionario, new FuncionarioResponse()));
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<List<FuncionarioResponse>> Listar()
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_funcionarioService.ListarFiltrados(f => f.Codigo != -1, f => f.Interveniente), new List<FuncionarioResponse>());

                foreach (var funcionario in result)
                {
                    var interveniente = _funcionarioService.ListarFiltrados<Interveniente>(t => t.Codigo == funcionario.IntervenienteCodigo).FirstOrDefault();
                    if (interveniente != null)
                    {
                        funcionario.Email = interveniente.Email;
                        funcionario.CPF = interveniente.CpfCnpj;
                    }
                }
                
                return new ResultDTO<List<FuncionarioResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<FuncionarioResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public IEnumerable<FuncionarioAnalistaResponse> ListarAnalistas()
        {
            return AutoMapper.Mapper.Map(_funcionarioService.ListarAnalistas(), new List<FuncionarioAnalistaResponse>());
        }

        public ResultDTO<FuncionarioResponse> ObterPorExternalId(string externalId)
        {
            try
            {
                var funcionario = AutoMapper.Mapper.Map(_funcionarioService.ListarFiltrados(f => f.ExternalId == externalId, f => f.Interveniente).FirstOrDefault(), new FuncionarioResponse());

                if (funcionario == null)
                    return new ResultDTO<FuncionarioResponse>(
                    EResultDTOStatus.Error,
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetMessageCode());

                var interveniente = _funcionarioService.ListarFiltrados<Interveniente>(t => t.Codigo == funcionario.IntervenienteCodigo).FirstOrDefault();
                if (interveniente != null)
                {
                    funcionario.Email = interveniente.Email;
                    funcionario.CPF = interveniente.CpfCnpj;
                }

                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Success, "", funcionario);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<FuncionarioResponse> ObterPorId(int codigo)
        {
            try
            {
                var funcionario = AutoMapper.Mapper.Map(_funcionarioService.ListarFiltrados(f => f.Codigo == codigo, f => f.Interveniente).FirstOrDefault(), new FuncionarioResponse());

                if (funcionario == null)
                    return new ResultDTO<FuncionarioResponse>(
                    EResultDTOStatus.Error,
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesFuncionario.FUNCIONARIO_ERROR_TCON393531.GetMessageCode());

                var interveniente = _funcionarioService.ListarFiltrados<Interveniente>(t => t.Codigo == funcionario.IntervenienteCodigo).FirstOrDefault();
                if (interveniente != null)
                {
                    funcionario.Email = interveniente.Email;
                    funcionario.CPF = interveniente.CpfCnpj;
                }

                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Success, "", funcionario);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FuncionarioResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        private static string ObterApenasNumeros(string input)
        {
            Regex regex = new Regex(@"\D");
            return regex.Replace(input, "");
        }
    }
}

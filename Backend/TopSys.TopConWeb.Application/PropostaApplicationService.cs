using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraFrenteDTO;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaImportacaoSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaInseridaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaVersaoResponse;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Application
{
    public class PropostaApplicationService : ApplicationServiceBase<Proposta>, IPropostaApplicationService
    {
        public readonly IPropostaService _propostaService;
        public readonly IObraService _obraService;
        public readonly IObraTaxaService _obraTaxaService;
        public readonly IIntervenienteService _intervenienteService;
        public readonly IContratoService _contratoService;
        public readonly IContratoVersaoService _contratoVersaoService;
        public readonly IProgramacaoService _programacaoService;
        public readonly IParametroService _parametroService;
        public readonly ICadastroDiversoService _cadastroDiversoService;
        public readonly ICadastroGeralService _cadastroGeralService;
        public readonly ITracoCustoService _tracoCustoService;
        public readonly IDemaisServicosService _demaisServicosService;
        public readonly IContratoApplicationService _contratoApplicationService;
        public readonly IAprovacaoComercialPendenteService _aprovacaoComercialPendenteService;
        public readonly IAprovacaoComercialHierarquiaService _aprovacaoComercialHierarquiaService;
        public readonly IAprovacaoComercialService _aprovacaoComercialService;
        public readonly IObraFrenteService _obraFrenteService; 
        public readonly IObraApplicationService _obraApplicationService;
        public readonly IVendedorService _vendedorService;
        public readonly IObraTaxaRepository _obraTaxaRepository;
        public readonly IArquivoService _arquivoService;
        public readonly IOportunidadeService _oportunidadeService;
        public readonly ITracoPrecoService _tracoPrecoService;
        public readonly IMercadoriaService _mercadoriaService;
        public readonly IWebHookApplicationService _webHookApplicationService;
        public readonly IContratoPagamentoRepository _contratoPagamentoRepository;
        private readonly ReportService _reportService;

        private const int NUMERO_APLICACAO_PROPOSTA_LISTA = 6101;

        public PropostaApplicationService(
            IPropostaService propostaService, 
            IObraService obraService,
            IObraTaxaService obraTaxaService,
            IDemaisServicosService demaisServicosService,
            IIntervenienteService intervenienteService,
            IContratoService contratoService,
            IContratoVersaoService contratoVersaoService,
            IProgramacaoService programacaoService,
            IParametroService parametroService,
            ICadastroDiversoService cadastroDiversoService,
            ICadastroGeralService cadastroGeralService,
            IUnitOfWork unityOfWork,
            ITracoCustoService tracoCustoService,
            IAprovacaoComercialPendenteService aprovacaoComercialPendenteService,
            IAprovacaoComercialHierarquiaService aprovacaoComercialHierarquiaService,
            IAprovacaoComercialService aprovacaoComercialService,
            IObraFrenteService obraFrenteService,
            IContratoApplicationService contratoApplicationService,
            IObraApplicationService obraApplicationService,
            IVendedorService vendedorService,
            IObraTaxaRepository taxaExtraRepository,
            IArquivoService arquivoService,
			IOportunidadeService oportunidadeService,
            ITracoPrecoService tracoPrecoService,
            IContratoPagamentoRepository contratoPagamentoRepository,
            IMercadoriaService mercadoriaService,
            IWebHookApplicationService webHookApplicationService,
            ReportService reportService)
            : base(propostaService, unityOfWork)
        {
            _propostaService = propostaService;
            _obraService = obraService;
            _obraTaxaService = obraTaxaService;
            _intervenienteService = intervenienteService;
            _contratoService = contratoService;
            _contratoVersaoService = contratoVersaoService;
            _programacaoService = programacaoService;
            _parametroService = parametroService;
            _cadastroDiversoService = cadastroDiversoService;
            _cadastroGeralService = cadastroGeralService;
            _tracoCustoService = tracoCustoService;
            _demaisServicosService = demaisServicosService;
            _contratoApplicationService = contratoApplicationService;
            _aprovacaoComercialPendenteService = aprovacaoComercialPendenteService;
            _aprovacaoComercialHierarquiaService = aprovacaoComercialHierarquiaService;
            _obraFrenteService = obraFrenteService;
            _aprovacaoComercialService = aprovacaoComercialService;
            _obraApplicationService = obraApplicationService;
            _vendedorService = vendedorService;
            _obraTaxaRepository = taxaExtraRepository;
            _reportService = reportService;
            _arquivoService = arquivoService;
			_oportunidadeService = oportunidadeService;
            _tracoPrecoService = tracoPrecoService;
            _mercadoriaService = mercadoriaService;
            _webHookApplicationService = webHookApplicationService;
            _contratoPagamentoRepository = contratoPagamentoRepository;
        }

        public ICollection<PropostaReportPDFResponse> ListarPropostaReportPDFSequencias(int codUsina, int anoChamada, int numeroChamada)
        {

            var chave = $"{codUsina};{numeroChamada};{anoChamada}";
            var arquivos = _arquivoService.ListarArquivosPorChave(NUMERO_APLICACAO_PROPOSTA_LISTA, chave).ToList();

            var result = new List<PropostaReportPDFResponse>();

            arquivos.ForEach(arquivo =>
            {
                var usuarioData = arquivo.IdCadastro.Split(' ');

                result.Add(new PropostaReportPDFResponse()
                {
                    Sequencia = arquivo.Sequencia,
                    Usuario = usuarioData[0],
                    Data = usuarioData.Length >= 1 ? usuarioData[1] : ""
                });
            });

            result = result.OrderByDescending(x => x.Sequencia).ToList();

            return result;

        }

        public int CriarNovaPropostaReportPDF(int codUsina, int anoChamada, int numeroChamada, string usuario, Guid? propagandaId)
        {

            var chave = $"{codUsina};{numeroChamada};{anoChamada}";
            var arquivos = _arquivoService.ListarArquivosPorChave(NUMERO_APLICACAO_PROPOSTA_LISTA, chave).ToList();

            var proximaSequencia = arquivos.Count == 0 ? 1 : arquivos.Max(x => x.Sequencia) + 1;

            var report = _reportService.GetPropostaReport(codUsina, anoChamada, numeroChamada, proximaSequencia, propagandaId);

            _arquivoService.SalvarArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave, report, proximaSequencia, usuario);

            AtualizarDataUltimaVersaoPDF(DateTime.Now, codUsina, anoChamada, numeroChamada);

            return proximaSequencia;

        }

        public void AtualizarDataUltimaVersaoPDF(DateTime data, int codUsina, int anoChamada, int numeroChamada)
        {

            var ultimaVersao = _propostaService.GetUltimaVersaoProposta(codUsina, anoChamada, numeroChamada);

            var proposta = _propostaService.ObterPorId(codUsina, anoChamada, numeroChamada);
            proposta.DataUltimaVersaoGerada = data;

            if(ultimaVersao > 0)
            {
                var propostaVersao = _programacaoService.ObterPorId<PropostaVersao>(ultimaVersao, codUsina, anoChamada, numeroChamada);
                propostaVersao.DataUltimaVersaoGerada = data;
            }

            Commit();

        }

        public Stream ObterPropostaReportPDF(int codUsina, int anoChamada, int numeroChamada, int sequenciaVersao)
        {
            var chave = $"{codUsina};{numeroChamada};{anoChamada}";
            return _arquivoService.ObterArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave, sequenciaVersao);
        }

        public PropostaInseridaResponse Adicionar(string usuario, PropostaInclusaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            var proposta = AutoMapper.Mapper.Map(propostaRequest, new Proposta());

            var parametroDesativarObrigatorioedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            if (parametroDesativarObrigatorioedadeAprovacaoCadastro)
                proposta.Obra.StatusCadastro = EObraStatusCadastro.Aprovado;

            proposta.Cobranca = AutoMapper.Mapper.Map(propostaRequest.Cobranca, new PropostaCobranca());
            proposta.Faturamento = AutoMapper.Mapper.Map(propostaRequest.Faturamento, new PropostaFaturamento());
            proposta.ResponsavelSolidario = AutoMapper.Mapper.Map(propostaRequest.ResponsavelSolidario, new PropostaResponsavelSolidario());

            if (propostaRequest.UtilizaResponsavelSolidario && !proposta.PropostaResponsavelSolidarioIsValid(proposta.ResponsavelSolidario))
                return null;

            if (!proposta.PropostaCobrancaIsValid(proposta.Cobranca, propostaRequest.UtilizaDadosCobranca, propostaRequest.UtilizaEnderecoCobranca))
                return null;

            if (!proposta.PropostaFaturamentoIsValid(proposta.Faturamento, propostaRequest.UtilizaDadosFaturamento, propostaRequest.UtilizaEnderecoFaturamento))
                return null;

            if (proposta.UtilizaDadosFaturamento && !proposta.UtilizaEnderecoFaturamento)
            {
                proposta.Faturamento.EnderecoBairro = proposta.EnderecoBairro;
                proposta.Faturamento.EnderecoCep = proposta.EnderecoCep;
                proposta.Faturamento.EnderecoComplemento = proposta.EnderecoComplemento;
                proposta.Faturamento.EnderecoLogradouro = proposta.EnderecoLogradouro;
                proposta.Faturamento.EnderecoMunicipioCodigo = proposta.EnderecoMunicipioCodigo;
                //proposta.Faturamento.EnderecoMunicipio = proposta.EnderecoMunicipio;
                proposta.Faturamento.EnderecoNumero = proposta.EnderecoNumero;
            }
            else if (!proposta.UtilizaDadosFaturamento && proposta.UtilizaEnderecoFaturamento)
            {
                proposta.Faturamento.CpfCnpj = proposta.CpfCnpj;
                proposta.Faturamento.InscricaoEstadual = proposta.InscricaoEstadual;
                proposta.Faturamento.InscricaoMunicipal = proposta.InscricaoMunicipal;
                proposta.Faturamento.Nome = proposta.IntervenienteNome;
                proposta.Faturamento.OrgaoExpedidor = proposta.OrgaoExpedidor;
                proposta.Faturamento.Razao = proposta.IntervenienteRazao;
                proposta.Faturamento.Rg = proposta.Rg;
            }

            if (proposta.UtilizaDadosCobranca && !proposta.UtilizaEnderecoCobranca)
            {
                proposta.Cobranca.EnderecoBairro = proposta.EnderecoBairro;
                proposta.Cobranca.EnderecoCep = proposta.EnderecoCep;
                proposta.Cobranca.EnderecoComplemento = proposta.EnderecoComplemento;
                proposta.Cobranca.EnderecoLogradouro = proposta.EnderecoLogradouro;
                proposta.Cobranca.EnderecoMunicipioCodigo = proposta.EnderecoMunicipioCodigo;
                //proposta.Cobranca.EnderecoMunicipio = proposta.EnderecoMunicipio;
                proposta.Cobranca.EnderecoNumero = proposta.EnderecoNumero;
            }
            else if (!proposta.UtilizaDadosCobranca && proposta.UtilizaEnderecoCobranca)
            {
                proposta.Cobranca.CpfCnpj = proposta.CpfCnpj;
                proposta.Cobranca.InscricaoEstadual = proposta.InscricaoEstadual;
                proposta.Cobranca.InscricaoMunicipal = proposta.InscricaoMunicipal;
                proposta.Cobranca.Nome = proposta.IntervenienteNome;
                proposta.Cobranca.OrgaoExpedidor = proposta.OrgaoExpedidor;
                proposta.Cobranca.Razao = proposta.IntervenienteRazao;
                proposta.Cobranca.Rg = proposta.Rg;
            }

            using (var scope = new TransactionScope())
            {
                var statusAnterior = proposta.Status;

                if ((proposta.IntervenienteCodigo ?? 0) == 0)
                {
                    var interveniente = _intervenienteService.ObterPorCpfCnpj(proposta.CpfCnpj, proposta.InscricaoEstadual);

                    if (interveniente != null)
                        proposta.IntervenienteCodigo = interveniente.Codigo;
                }

                _propostaService.Adicionar(usuario, proposta, filtroVendedores);

                // atualizando os dados do interveniente
                if (propostaRequest.IntervenienteCodigo != 0)
                {
                    var intervenienteOld = _intervenienteService.ObterPorId(propostaRequest.IntervenienteCodigo);
                    var cpfCnpj = intervenienteOld.CpfCnpj;


                    if (propostaRequest.Email.Equals(""))
                    {
                        if (!propostaRequest.Interveniente.Email.Equals(""))
                        {
                            var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Now);

                            if (propostaRequest.Interveniente.IntervenienteTipo != "F" || parametroProposta.ObrigaEmailPessoaFisica)
                                propostaRequest.Email = propostaRequest.Interveniente.Email;
                        }
                    }

                    var houveAlteracaoInterv = HouveAlteracaoInterveniente(intervenienteOld, propostaRequest);

                    AutoMapper.Mapper.Map(propostaRequest, intervenienteOld);
                    if (propostaRequest.CpfCnpj == "")
                    {
                        intervenienteOld.CpfCnpj = cpfCnpj;
                    }
                    intervenienteOld.IdAtualizacao = StringHelper.GetIDD(usuario);

                    if(houveAlteracaoInterv)
                        _webHookApplicationService.AdicionarWebHookInterveniente(intervenienteOld, EWebHookTipoEvento.Update);

                    Commit();
                }

                if (!_notifications.HasNotifications())
                {
                    int i = 1;

                    _obraService.Adicionar(new ObraLog {
                        UsinaCodigo = proposta.UsinaCodigo,
                        ObraCodigo = proposta.Obra.Numero,
                        AnoChamada = proposta.Ano,
                        NumChamada = proposta.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "INSERÇÃO PROPOSTA",
                        Complemento = $"Status: {(int)statusAnterior} - {statusAnterior}",
                        Observacao = $"{proposta.UsinaCodigo}/{proposta.Numero.ToString().PadLeft(5, '0')}-{proposta.Ano}",
                        Sequencia = i++
                    });

                    if(proposta.ModeloDocumentoRemessaConcreto > 0)
                    {
                        var modelo = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto().Where(x => x.Codigo.Equals(proposta.ModeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();

                        var descricao = $"Modelo de documento de remessa: {proposta.ModeloDocumentoRemessaConcreto}";

                        if(modelo != null)
                            descricao = $"Modelo de documento de remessa: {modelo.Descricao.ToUpper()}";

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "MODELO DE DOCUMENTO CONCRETO",
                            Complemento = descricao,
                            Observacao = "Proposta inserida com modelo de documento de concreto diferente do padrão.",
                            Sequencia = i++
                        });

                    }

                    if (proposta.ModeloDocumentoRemessaBomba > 0)
                    {
                        var modelo = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto().Where(x => x.Codigo.Equals(proposta.ModeloDocumentoRemessaBomba.ToString())).FirstOrDefault();

                        var descricao = $"Modelo de documento de remessa: {proposta.ModeloDocumentoRemessaBomba}";

                        if (modelo != null)
                            descricao = $"Modelo de documento de remessa: {modelo.Descricao.ToUpper()}";

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "MODELO DE DOCUMENTO BOMBA",
                            Complemento = descricao,
                            Observacao = "Proposta inserida com modelo de documento de bomba diferente do padrão.",
                            Sequencia = i++
                        });

                    }

                    if (proposta.Obra.CondicaoPagamentoCodigo > 0)
                    {
                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "INSERÇÃO COND.PAGTO.PRINC.",
                            Complemento = $"Cond. Pagto.: {proposta.Obra.CondicaoPagamentoCodigo} - {_obraService.ObterPorId<CondicaoPagamento>(proposta.Obra.CondicaoPagamentoCodigo)?.Descricao}",
                            Observacao = "",
                            Sequencia = i++
                        });
                    }

                    if (proposta.Obra.TipoCobrancaCodigo > 0)
                    {
                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "INSERÇÃO TIPO COB.PRINC.",
                            Complemento = $"Tipo Cob.: {proposta.Obra.TipoCobrancaCodigo} - {_obraService.ObterPorId<TipoCobranca>(proposta.Obra.TipoCobrancaCodigo)?.Descricao}",
                            Observacao = "",
                            Sequencia = i++
                        });
                    }

                    if (statusAnterior != proposta.Status)
                    {
                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO STATUS",
                            Complemento = $"De: {(int)statusAnterior} - {statusAnterior} Para: {(int)proposta.Status} - {proposta.Status}",
                            Observacao = "",
                            Sequencia = i++
                        });
                    }

                    if (proposta.ValorTotalContrato > 0)
                    {
                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = proposta.UsinaCodigo,
                            ObraCodigo = proposta.Obra.Numero,
                            AnoChamada = proposta.Ano,
                            NumChamada = proposta.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO TOTAL CONTRATO",
                            Complemento = $"Inserção Valor: {proposta.ValorTotalContrato}",
                            Observacao = "",
                            Sequencia = i++
                        });
                    }

                    if(proposta.NumeroOportunidade > 0)
                    {

                        var oportunidade = _oportunidadeService.ObterPorId(proposta.UsinaCodigo, proposta.AnoOportunidade, proposta.NumeroOportunidade, true);

                        if(oportunidade != null)
                        {
                            var log = new OportunidadeLog()
                            {
                                Usina = oportunidade.UsinaCodigo,
                                Ano = oportunidade.Ano,
                                Numero = oportunidade.Numero,
                                Usuario = usuario,
                                DataHoraEvento = DateTime.Now,
                                Tipo = "ALTERACAO",
                                Evento = "PROPOSTA GERADA",
                                Complemento = $"Proposta {proposta.Numero.ToString().PadLeft(6, '0')}-{proposta.Ano} gerada em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}."
                            };

                            oportunidade.Logs.Add(log);

                        }

                    }

                    var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
                    if (parametroDesativarObrigatoriedadeAprovacaoCadastro)
                    {
                        proposta.Obra.StatusCadastro = EObraStatusCadastro.Aprovado;
                    }

                    Commit();

                    scope.Complete();

                }
            }

            return AutoMapper.Mapper.Map(proposta, new PropostaInseridaResponse());

        }

        public void Atualizar(string usuario, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            using (var scope = new TransactionScope())
            {
                if (propostaRequest.Interveniente != null)
                {
                    propostaRequest.InscricaoEstadual = propostaRequest.IntervenienteTipo == "F" ? propostaRequest.Interveniente.InscricaoEstadual : propostaRequest.InscricaoEstadual;
                    propostaRequest.InscricaoMunicipal = propostaRequest.IntervenienteTipo == "F" ? propostaRequest.Interveniente.InscricaoMunicipal : propostaRequest.InscricaoMunicipal;
                }

                var versionamentoTraco = _parametroService.ObterParametroN("web", "VersionamentoTraco").Contains("true");
                var versionamentoBomba = _parametroService.ObterParametroN("web", "VersionamentoBomba").Contains("true");
                var versionamentoTaxaExtra = _parametroService.ObterParametroN("web", "VersionamentoTaxaExtra").Contains("true");
                var versionamentoCondicaoPagamento = _parametroService.ObterParametroN("web", "VersionamentoCondicaoPagamento").Contains("true");
                var versionamentoEnderecoObra = _parametroService.ObterParametroN("web", "VersionamentoEnderecoObra").Contains("true");
                var versionamentoDemaisServicos = _parametroService.ObterParametroN("web", "VersionamentoDemaisServicos").Contains("true");
                var versionamentoReajusteContrato = _parametroService.ObterParametroN("web", "VersionamentoReajusteContrato").Contains("true");

                var numProximaVersao = _contratoService.GetUltimaVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value) + 1;

                

                if (versionamentoTraco || versionamentoBomba
                    || versionamentoTaxaExtra || versionamentoCondicaoPagamento
                    || versionamentoEnderecoObra || versionamentoDemaisServicos)
                    //|| versionamentoReajusteContrato)
                {

                    var statusContratoReal = propostaRequest.StatusContrato;
                    var numVersao = _contratoService.GetUltimaVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);

                    if(numVersao > 0)
                    {
                        var contratoVersao = _contratoService.ContratoVersaoObterPorId(numVersao, propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                        if (contratoVersao != null)
                            statusContratoReal = contratoVersao.Status;
                    } else
                    {
                        var contrato = _contratoService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                        if(contrato != null)
                            statusContratoReal = contrato.Status;
                    }

                    if (statusContratoReal == EContratoStatus.Aprovado || statusContratoReal == EContratoStatus.Reprovado)
                    {   
                        if ((versionamentoTraco && HouveAlteracaoTraco(propostaRequest))
                            || (versionamentoBomba && HouveAlteracaoBomba(propostaRequest))
                            || (versionamentoTaxaExtra && HouveAlteracaoTaxaExtra(propostaRequest))
                            || (versionamentoCondicaoPagamento && HouveAlteracaoCondicaoPagamento(propostaRequest))
                            || (versionamentoEnderecoObra && HouveAlteracaoEnderecoObra(propostaRequest))
                            || (versionamentoDemaisServicos && HouveAlteracaoDemaisServicos(propostaRequest))
                            // || (versionamentoReajusteContrato )
                            )
                        {
                            _propostaService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, numProximaVersao);
                            _contratoService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numProximaVersao);
                            _obraService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numProximaVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero);
                            _obraTaxaService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaEntrega.Codigo, numProximaVersao, propostaRequest.Obra.Numero);
                            _demaisServicosService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, numProximaVersao, propostaRequest.Obra.Numero);

                            if (numProximaVersao == 1)
                            {
                                _contratoApplicationService.SalvarPDFContratoVersao(1, propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);

                                numProximaVersao = numProximaVersao + 1;
                                _propostaService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, numProximaVersao);
                                _contratoService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numProximaVersao);
                                _obraService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numProximaVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero);
                                _obraTaxaService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaEntrega.Codigo, numProximaVersao, propostaRequest.Obra.Numero);
                                _demaisServicosService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, numProximaVersao, propostaRequest.Obra.Numero);
                            } else
                            {
                                _contratoApplicationService.SalvarPDFContratoVersao(numProximaVersao-1, propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                            }

                            AtualizarContratoVersao(usuario, numProximaVersao, propostaRequest, filtroVendedores);
                        }
                        else
                        {
                            AtualizarContrato(usuario, propostaRequest, filtroVendedores);
                            var numUltimaVersao = _contratoService.GetUltimaVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);

                            if (numUltimaVersao > 1)
                            {
                                _propostaService.ExcluirVersaoContrato(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, numUltimaVersao);
                                _contratoService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numUltimaVersao);
                                _obraService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numUltimaVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero);
                                _obraTaxaService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaEntrega.Codigo, numUltimaVersao, propostaRequest.Obra.Numero);
                                _demaisServicosService.ExcluirVersaoContrato(propostaRequest.Usina.Codigo, numUltimaVersao, propostaRequest.Obra.Numero);

                                _propostaService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, numUltimaVersao);
                                _contratoService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numUltimaVersao);
                                _obraService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numUltimaVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero);
                                _obraTaxaService.AdicionarVersaoContrato(propostaRequest.Obra.UsinaEntrega.Codigo, numUltimaVersao, propostaRequest.Obra.Numero);
                                _demaisServicosService.AdicionarVersaoContrato(propostaRequest.Usina.Codigo, numUltimaVersao, propostaRequest.Obra.Numero);
                            }
                        }                        
                    }
                    else
                    {
                        var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                        if (versaoAtual == 0) AtualizarContrato(usuario, propostaRequest, filtroVendedores);
                        else AtualizarContratoVersao(usuario, versaoAtual, propostaRequest, filtroVendedores);
                    }

                    numVersao = _contratoService.GetUltimaVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                    if (numVersao != 0)
                        _contratoService.AtualizarDataEncerramentoEStatusContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numVersao);
                }
                else
                { 
                    AtualizarContrato(usuario, propostaRequest, filtroVendedores);
                }
                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }

        public void WebHookContratoPagamentoAtualizar(int contratoUsina, int contratoAno, int contratoNumero)
        {

            var contrato = _contratoService.ObterPorId(contratoUsina, contratoAno, contratoNumero);
            var pagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contratoUsina, contratoAno, contratoNumero).ToList();

            _webHookApplicationService.AdicionarWebHookContratoPagamento(contrato, pagamentos, EWebHookTipoEvento.Update);

        }

        public void AtualizarContratoVersao(string usuario, int numVersao, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            var propostaOld = _propostaService.ObterPorId<PropostaVersao>(numVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero);
            var obraOld = _obraService.ObterPorId<ObraVersao>(numVersao, propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            var obraIndicador = _obraService.ObterPorId<ObraIndicadorVersao>(numVersao, propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            var utilizaAprovacaoComercicalPorAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(obraOld.UsinaEntregaCodigo);
            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var parametroValidaAprovacaoComparandoComPrecoTabela = _parametroService.ObterParametroN("web", "ValidaAprovacaoComparandoComPrecoTabela") == "1";

            int sequenciaObraLog = 1;
            int sequenciaProgramacaoLog = 1;

            var cpfCnpjAnterior = propostaOld.CpfCnpj;
            var propostaStatusAnterior = propostaOld.Status;
            var condicaoPagamentoAnterior = propostaOld.Obra.CondicaoPagamentoCodigo ?? 0;
            var condicaoPagamentoAnteriorDescricao = _obraService.ObterPorId<CondicaoPagamento>(condicaoPagamentoAnterior)?.Descricao ?? "";
            var tipoCobrancaAnterior = propostaOld.Obra.TipoCobrancaCodigo ?? 0;
            var tipoCobrancaAnteriorDescricao = _obraService.ObterPorId<TipoCobranca>(tipoCobrancaAnterior)?.Descricao ?? "";
            var valorTotalAnterior = propostaOld.ValorTotalContrato;
            var modeloDocumentoRemessaConcreto = propostaOld.ModeloDocumentoRemessaConcreto;
            var modeloDocumentoRemessaBomba = propostaOld.ModeloDocumentoRemessaBomba;
            var segmentacaoProposta = propostaOld.Segmentacao;
            var finalidadeProposta = propostaOld.ContratoFinalidade;
            var ceiObraAnterior = obraOld.Cei;
            var tributacaoPisCofinsAnterior = obraOld.TributacaoPisCofinsCodigo;

            var tributacaoISAnterior = obraOld.TributacaoISCodigo;
            var tributacaoIBSAnterior = obraOld.TributacaoIBSCodigo;
            var tributacaoCBSAnterior = obraOld.TributacaoCBSCodigo;

            var codigoCibAnterior = obraOld.CodigoCib;
            var construcaoCivilTipoAlvaraAnterior = obraOld.ConstrucaoCivilTipoAlvara;

            var logradouroObraAnterior = obraOld.EnderecoLogradouro;
            var numeroObraAnterior = obraOld.EnderecoNumero;
            var cepObraAnterior = obraOld.EnderecoCep;
            var complementoObraAnterior = obraOld.EnderecoComplemento;
            var bairroObraAnterior = obraOld.EnderecoBairro;
            var municipioObraAnterior = obraOld.EnderecoMunicipioCodigo;
            var usaAdicionalZMRCAnterior = obraOld.UsaAdicionalZMRC;
            var viaCaptacaoAnterior = obraOld.ViaCaptacaoCodigo;

            var aprovacaoMedicaoAnterior = propostaOld.AprovacaoMedicao;
            var tempoAprovacaoMedicaoAnterior = propostaOld.TempoAprovacaoMedicao;

            var volumeTotalObraOld = _propostaService.ListarFiltrados<ObraTraco>(x => x.UsinaCodigo == obraOld.UsinaCodigo && x.ObraCodigo == obraOld.Numero).Sum(x => x.M3Quantidade);
            var volumeTotalObraNew = propostaRequest.Obra.ObraTracos.Sum(x => x.M3Quantidade);

            var vendedorOld = _vendedorService.ObterPorId<Vendedor>(propostaOld.VendedorCodigo);

            propostaOld = AutoMapper.Mapper.Map(propostaRequest, propostaOld);

            propostaOld.Vendedor = _vendedorService.ObterPorId<Vendedor>(propostaOld.VendedorCodigo);

            var houveMudancaSegmentacaoProposta = segmentacaoProposta != propostaRequest.Segmentacao;
            var houveMudancaFinalidadeProposta = finalidadeProposta != propostaRequest.ContratoFinalidade;

            propostaOld.ModeloItensDanfeERomaneio = propostaRequest.ModeloItensDanfeERomaneio;

            var cobrancaOld = _propostaService.ObterPorId<PropostaCobrancaVersao>(propostaOld.NumeroVersao, propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.Cobranca = AutoMapper.Mapper.Map(propostaRequest.Cobranca, cobrancaOld ?? new PropostaCobrancaVersao());

            var faturamentoOld = _propostaService.ObterPorId<PropostaFaturamentoVersao>(propostaOld.NumeroVersao, propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.Faturamento = AutoMapper.Mapper.Map(propostaRequest.Faturamento, faturamentoOld ?? new PropostaFaturamentoVersao());

            var responsavelSolidarioOld = _propostaService.ObterPorId<PropostaResponsavelSolidarioVersao>(propostaOld.NumeroVersao, propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.ResponsavelSolidario = AutoMapper.Mapper.Map(propostaRequest.ResponsavelSolidario, responsavelSolidarioOld ?? new PropostaResponsavelSolidarioVersao());

            if (propostaRequest.UtilizaResponsavelSolidario && !propostaOld.PropostaResponsavelSolidarioIsValid(propostaOld.ResponsavelSolidario))
                return;

            if (!propostaOld.PropostaCobrancaIsValid(propostaOld.Cobranca, propostaRequest.UtilizaDadosCobranca, propostaRequest.UtilizaEnderecoCobranca))
                return;

            if (!propostaOld.PropostaFaturamentoIsValid(propostaOld.Faturamento, propostaRequest.UtilizaDadosFaturamento, propostaRequest.UtilizaEnderecoFaturamento))
                return;

            var pagamentos = AutoMapper.Mapper.Map(propostaRequest.Obra.ObraPagamentos, new List<PropostaPagamentoVersao>());

            if (propostaRequest.StatusProposta == EPropostaStatusCliente.Aprovado)
            {
                if (!pagamentos.PagamentosAtivosIsValid())
                    return;
            }

            if (propostaOld.UtilizaDadosFaturamento && !propostaOld.UtilizaEnderecoFaturamento)
            {
                propostaOld.Faturamento.EnderecoBairro = propostaOld.EnderecoBairro;
                propostaOld.Faturamento.EnderecoCep = propostaOld.EnderecoCep;
                propostaOld.Faturamento.EnderecoComplemento = propostaOld.EnderecoComplemento;
                propostaOld.Faturamento.EnderecoLogradouro = propostaOld.EnderecoLogradouro;
                propostaOld.Faturamento.EnderecoMunicipioCodigo = propostaOld.EnderecoMunicipioCodigo;
                //propostaOld.Faturamento.EnderecoMunicipio = propostaOld.EnderecoMunicipio;
                propostaOld.Faturamento.EnderecoNumero = propostaOld.EnderecoNumero;
                Commit();
            }
            else if (!propostaOld.UtilizaDadosFaturamento && propostaOld.UtilizaEnderecoFaturamento)
            {
                propostaOld.Faturamento.CpfCnpj = propostaOld.CpfCnpj;
                propostaOld.Faturamento.InscricaoEstadual = propostaOld.InscricaoEstadual;
                propostaOld.Faturamento.InscricaoMunicipal = propostaOld.InscricaoMunicipal;
                propostaOld.Faturamento.Nome = propostaOld.IntervenienteNome;
                propostaOld.Faturamento.OrgaoExpedidor = propostaOld.OrgaoExpedidor;
                propostaOld.Faturamento.Razao = propostaOld.IntervenienteRazao;
                propostaOld.Faturamento.Rg = propostaOld.Rg;
                Commit();
            }

            if (propostaOld.UtilizaDadosCobranca && !propostaOld.UtilizaEnderecoCobranca)
            {
                propostaOld.Cobranca.EnderecoBairro = propostaOld.EnderecoBairro;
                propostaOld.Cobranca.EnderecoCep = propostaOld.EnderecoCep;
                propostaOld.Cobranca.EnderecoComplemento = propostaOld.EnderecoComplemento;
                propostaOld.Cobranca.EnderecoLogradouro = propostaOld.EnderecoLogradouro;
                propostaOld.Cobranca.EnderecoMunicipioCodigo = propostaOld.EnderecoMunicipioCodigo;
                //propostaOld.Cobranca.EnderecoMunicipio = propostaOld.EnderecoMunicipio;
                propostaOld.Cobranca.EnderecoNumero = propostaOld.EnderecoNumero;
                Commit();
            }
            else if (!propostaOld.UtilizaDadosCobranca && propostaOld.UtilizaEnderecoCobranca)
            {
                propostaOld.Cobranca.CpfCnpj = propostaOld.CpfCnpj;
                propostaOld.Cobranca.InscricaoEstadual = propostaOld.InscricaoEstadual;
                propostaOld.Cobranca.InscricaoMunicipal = propostaOld.InscricaoMunicipal;
                propostaOld.Cobranca.Nome = propostaOld.IntervenienteNome;
                propostaOld.Cobranca.OrgaoExpedidor = propostaOld.OrgaoExpedidor;
                propostaOld.Cobranca.Razao = propostaOld.IntervenienteRazao;
                propostaOld.Cobranca.Rg = propostaOld.Rg;
                Commit();
            }

            if(viaCaptacaoAnterior != propostaRequest.Obra.ViaCaptacao.Codigo)
            {

                var viaCaptacaoOld = _obraService.ObterPorId<CadastroGeral>(viaCaptacaoAnterior);

                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO VIA DE CAPTAÇÃO",
                    Complemento = "",
                    Observacao = $"Alteração de via de captação de {(viaCaptacaoOld == null ? viaCaptacaoAnterior.ToString() : viaCaptacaoOld.Descricao)} para {propostaRequest.Obra.ViaCaptacao.Descricao}",
                    Sequencia = sequenciaObraLog++
                });

            }

            if(propostaRequest.Obra.Indicador != null)
            {

                var newIndicador = propostaRequest.Obra.Indicador;
                var mensagem = "";
                var indicadorNewDescricao = "";

                if (newIndicador.IntervenienteCodigo != 0)
                    indicadorNewDescricao = $"Cliente {newIndicador.IntervenienteCodigo} - {_intervenienteService.ObterPorId(newIndicador.IntervenienteCodigo)?.Nome ?? ""}";

                if(newIndicador.VendedorCodigo != 0)
                    indicadorNewDescricao = $"Vendedor {newIndicador.VendedorCodigo} - {_vendedorService.ObterPorId(newIndicador.VendedorCodigo)?.Nome ?? ""}";

                if (obraIndicador != null)
                {
                    var vendedorIgual = newIndicador.VendedorCodigo == obraIndicador.VendedorCodigo;
                    var intervenienteIgual = newIndicador.IntervenienteCodigo == obraIndicador.IntervenienteCodigo;

                    if (!vendedorIgual || !intervenienteIgual)
                    {

                        var indicadorOldDescricao = "";

                        if (obraIndicador.IntervenienteCodigo != 0)
                            indicadorOldDescricao = $"Cliente {obraIndicador.IntervenienteCodigo} - {_intervenienteService.ObterPorId(obraIndicador.IntervenienteCodigo)?.Nome ?? ""}";

                        if (obraIndicador.VendedorCodigo != 0)
                            indicadorOldDescricao = $"Vendedor {obraIndicador.VendedorCodigo} - {_vendedorService.ObterPorId(obraIndicador.VendedorCodigo)?.Nome ?? ""}";

                        mensagem = $"Indicador alterado de {indicadorOldDescricao} para {indicadorNewDescricao}";

                    }

                }

                if(!string.IsNullOrEmpty(mensagem))
                {
                    _obraService.Adicionar(new ObraLogVersao
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = propostaOld.UsinaCodigo,
                        ObraCodigo = propostaOld.Obra.Numero,
                        AnoChamada = propostaOld.Ano,
                        NumChamada = propostaOld.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO INDICADOR CAPTAÇÃO",
                        Complemento = "",
                        Observacao = mensagem,
                        Sequencia = sequenciaObraLog++
                    });
                }

            }

            if (modeloDocumentoRemessaConcreto != propostaRequest.ModeloDocumentoRemessaConcreto)
            {
                propostaOld.ModeloDocumentoRemessaConcreto = propostaRequest.ModeloDocumentoRemessaConcreto;

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var descricaoAlteracaoModelo = "";

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(propostaRequest.ModeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();

                if (modeloOld != null && modeloNew != null)
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloOld.Descricao.ToUpper()} Para: {modeloNew.Descricao.ToUpper()}";
                }
                else
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloDocumentoRemessaConcreto} Para: {propostaRequest.ModeloDocumentoRemessaConcreto}";
                }

                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricaoAlteracaoModelo,
                    Observacao = "Alteração de modelo de documento para remessa de concreto",
                    Sequencia = sequenciaObraLog++
                }); ;

                Commit();
            }

            if (modeloDocumentoRemessaBomba != propostaRequest.ModeloDocumentoRemessaBomba)
            {
                propostaOld.ModeloDocumentoRemessaBomba = propostaRequest.ModeloDocumentoRemessaBomba;

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var descricaoAlteracaoModelo = "";

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaBomba.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(propostaRequest.ModeloDocumentoRemessaBomba.ToString())).FirstOrDefault();

                if (modeloOld != null && modeloNew != null)
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloOld.Descricao.ToUpper()} Para: {modeloNew.Descricao.ToUpper()}";
                }
                else
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloDocumentoRemessaBomba} Para: {propostaRequest.ModeloDocumentoRemessaBomba}";
                }

                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricaoAlteracaoModelo,
                    Observacao = "Alteração de modelo de documento para remessa de bomba",
                    Sequencia = sequenciaObraLog++
                });

                Commit();
            }

            // log para alteração de CEI/CNO da Obra
            if (propostaRequest.Obra.Cei != ceiObraAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO CEI/CNO DA OBRA",
                    Complemento = $"Valor Alterado De: {ceiObraAnterior} Para: {propostaRequest.Obra.Cei}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }
            
            // log para alteração de  tributação pis/cofins na Obra
            if ((propostaRequest.Obra.TributacaoPisCofins?.Codigo ?? "") != tributacaoPisCofinsAnterior)
                {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIBUTAÇÃO PIS/COFINS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoPisCofinsAnterior} Para: {propostaRequest.Obra.TributacaoPisCofins?.Codigo ?? ""}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoIS?.Id ?? 0) != tributacaoISAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. IS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoISAnterior} Para: {propostaRequest.Obra.TributacaoIS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoIBS?.Id ?? 0) != tributacaoIBSAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. IBS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoIBSAnterior} Para: {propostaRequest.Obra.TributacaoIBS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoCBS?.Id ?? 0) != tributacaoCBSAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. CBS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoCBSAnterior} Para: {propostaRequest.Obra.TributacaoCBS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // log para alteração do código CIB da Obra
            if (propostaRequest.Obra.CodigoCib != codigoCibAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO CÓDIGO CIB DA OBRA",
                    Complemento = $"Valor Alterado De: {codigoCibAnterior} Para: {propostaRequest.Obra.CodigoCib}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // log para alteração do campo Construção Civil Tipo Alvará da Obra
            if (propostaRequest.Obra.ConstrucaoCivilTipoAlvara != construcaoCivilTipoAlvaraAnterior)
            {
                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO C. CIVIL TIPO ALVARÁ",
                    Complemento = $"Valor Alterado De: {construcaoCivilTipoAlvaraAnterior} Para: {propostaRequest.Obra.ConstrucaoCivilTipoAlvara}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // correção para acesso concorrente onde o cliente era cadastrado no
            // desktop enquanto a proposta estava aberta no web
            if (cpfCnpjAnterior != propostaOld.CpfCnpj || (propostaOld.IntervenienteCodigo == 0 && propostaRequest.IntervenienteCodigo != 0))
            {
                propostaOld.IntervenienteCodigo = propostaRequest.IntervenienteCodigo;
                Commit();
            }

            // Correção proposta em negociação sem CPF_CNPJ
            if (propostaRequest.StatusProposta == EPropostaStatusCliente.EmNegociacao && propostaOld.CpfCnpj.Length < 11)
            {
                propostaRequest.IntervenienteCodigo = 0;
                propostaOld.CpfCnpj = "";
                propostaOld.IntervenienteCodigo = 0;
                Commit();
            }

            // atualizando os dados do interveniente
            if (propostaOld.IntervenienteCodigo != 0 && propostaRequest.IntervenienteCodigo != 0)
            {
                var intervenienteOld = _intervenienteService.ObterPorId(propostaOld.IntervenienteCodigo);

                if (propostaRequest.Email.Equals(""))
                {
                    if (!intervenienteOld.Email.Equals(""))
                    {
                        var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Now);

                        if (propostaRequest.Interveniente.IntervenienteTipo != "F" || parametroProposta.ObrigaEmailPessoaFisica)
                            propostaRequest.Email = intervenienteOld.Email;
                    }
                }

                var houveAlteracaoInterv = HouveAlteracaoInterveniente(intervenienteOld, propostaRequest);

                AutoMapper.Mapper.Map(propostaRequest, intervenienteOld);
                intervenienteOld.IdAtualizacao = StringHelper.GetIDD(usuario);

                if (houveAlteracaoInterv)
                    _webHookApplicationService.AdicionarWebHookInterveniente(intervenienteOld, EWebHookTipoEvento.Update);

                Commit();
            }

            // Tratamento para quando o interveniente tem vendedor exclusivo diferente do da proposta:
            // o cod do cliente vem zerado do frontend quando o vendedor não tem direito no grupo do vendedor exclusivo
            if ((propostaOld.IntervenienteCodigo ?? 0) == 0)
            {
                var interveniente = _intervenienteService.ObterPorCpfCnpj(propostaOld.CpfCnpj, propostaOld.InscricaoEstadual);

                if (interveniente != null)
                {
                    propostaOld.IntervenienteCodigo = interveniente.Codigo;
                    Commit();
                }
            }

            propostaOld.IdAtualizacao = StringHelper.GetIDD(usuario);
            Commit();

            var novoObraIndicador = propostaRequest.Obra.Indicador != null && obraOld.Indicador == null;
            obraOld = AutoMapper.Mapper.Map(propostaRequest.Obra, obraOld);
            obraOld.IdAtualizacao = StringHelper.GetIDD(usuario);

            obraOld.TributacaoCBS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoCBS, obraOld.TributacaoCBS);
            obraOld.TributacaoCBSCodigo = obraOld.TributacaoCBS != null ? obraOld.TributacaoCBS.Id : 0;

            obraOld.TributacaoIBS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoIBS, obraOld.TributacaoIBS);
            obraOld.TributacaoIBSCodigo = obraOld.TributacaoIBS != null ? obraOld.TributacaoIBS.Id : 0;

            obraOld.TributacaoIS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoIS, obraOld.TributacaoIS);
            obraOld.TributacaoISCodigo = obraOld.TributacaoIS != null ? obraOld.TributacaoIS.Id : 0;

            if (obraOld.Indicador != null)
            {
                obraOld.Indicador.ObraUsina = obraOld.UsinaCodigo;
                obraOld.Indicador.ObraNumero = obraOld.Numero;
                obraOld.Indicador.ObraVersao = obraOld.NumeroVersao;
            }

            if(novoObraIndicador)
                _obraService.Adicionar(obraOld.Indicador);

            Commit();

            _obraService.AlterarMensagemObraReajusteVersao(numVersao, propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero, propostaRequest.Obra.ObraReajuste.MensagemReajuste);

            // ObraFrentes
            List<int> sequenciasFrentes = new List<int>();
            foreach (var fDto in propostaRequest.Obra.ObraFrentes)
            {
                var frenteOld = _obraService.ListarFiltradosTracking<ObraFrente>(t => t.UsinaCodigo == fDto.UsinaCodigo
                                                                                      && t.ObraCodigo == fDto.ObraCodigo
                                                                                     && t.ObraSequencia == fDto.ObraSequencia).FirstOrDefault();
                var novoRegistro = (frenteOld == null);

                if (novoRegistro)
                {
                    var frenteNew = AutoMapper.Mapper.Map(fDto, new ObraFrente());
                    frenteNew.ID = Guid.NewGuid();
                    frenteNew.UsinaCodigo = obraOld.UsinaCodigo;
                    frenteNew.ObraCodigo = obraOld.Numero;

                    if (frenteNew.ObraSequencia == 0)
                        frenteNew.ObraSequencia = _obraFrenteService.ProximaSequenciaNaObra(frenteNew.UsinaCodigo, frenteNew.ObraCodigo);

                    sequenciasFrentes.Add(frenteNew.ObraSequencia);

                    _obraService.Adicionar(frenteNew);

                    Commit();

                }
                else
                {

                    sequenciasFrentes.Add(fDto.ObraSequencia);

                    frenteOld = AutoMapper.Mapper.Map(fDto, frenteOld);

                    // Buscando programações
                    var programacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraFrenteSequencia == frenteOld.ObraSequencia);

                    foreach (var programacao in programacoes)
                    {
                        programacao.EnderecoBairro = frenteOld.EnderecoBairro;
                        programacao.EnderecoCep = frenteOld.EnderecoCep;
                        programacao.EnderecoComplemento = frenteOld.EnderecoComplemento;
                        programacao.EnderecoLogradouro = frenteOld.EnderecoLogradouro;
                        programacao.EnderecoNumero = frenteOld.EnderecoNumero;
                    }

                    Commit();

                }

                

            }

            var seqsObraFrente = sequenciasFrentes.ToArray();
            var frentesExcluidas = _obraService.ListarFiltradosTracking<ObraFrente>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqsObraFrente.Contains(t.ObraSequencia));

            foreach (var f in frentesExcluidas)
            {

                var temProgramacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraFrenteSequencia == f.ObraSequencia).Count() > 0;

                if (temProgramacoes)
                {
                    AssertionConcern.Notify("obraFrente", $"Não é possível excluir a Frente de obra de sequência ({f.ObraSequencia}).");
                    continue;
                }

                _obraService.Remover(f);

                Commit();
            }

            // ObraTracos
            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraTracos)
            {
                sequencias.Add(tDto.Sequencia);
                var tracoOld = _obraService.ListarFiltradosTracking<ObraTracoVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == tDto.Usina.Codigo && t.ObraCodigo == tDto.ObraCodigo && t.Sequencia == tDto.Sequencia,
                    t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                    .FirstOrDefault();

                if (tracoOld != null)
                {

                    var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObraVersao(AutoMapper.Mapper.Map(tDto, new ObraTracoVersao()), obraOld) == 7105;
                    var mesmoTraco = true;

                    mesmoTraco = mesmoTraco && (tDto.Uso.Codigo == tracoOld.UsoCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Pedra.Codigo == tracoOld.PedraCodigo);
                    mesmoTraco = mesmoTraco && (tDto.SlumpNominal.Codigo == tracoOld.SlumpNominalCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Slump.Codigo == tracoOld.SlumpCodigo);
                    mesmoTraco = mesmoTraco && (tDto.ResistenciaTipo.Codigo == tracoOld.ResistenciaTipoCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Mpa == tracoOld.Fck);
                    mesmoTraco = mesmoTraco && (tDto.Consumo == tracoOld.Consumo);

                    var mercadoria = _mercadoriaService.ObterTracoMercadoria(tDto.Uso.Codigo, tDto.Pedra.Codigo, tDto.Slump.Codigo, tDto.ResistenciaTipo.Codigo, tDto.Mpa, tDto.Consumo);
                    var descricaoTracoNovo = mercadoria is null ? "" : mercadoria.Descricao;

                    if (tDto.M3Quantidade != tracoOld.M3Quantidade && mesmoTraco)
                    {

                        _obraService.Adicionar(new ObraLogVersao()
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO VOLUME DO TRAÇO",
                            Complemento = $"Alteração do volume do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De {tracoOld.Descricao} {tracoOld.M3Quantidade.ToString("F1")} M3" +
                            $" Para: {tracoOld.Descricao} {tDto.M3Quantidade.ToString("F1")} M3",
                            Sequencia = sequenciaObraLog++
                        });

                    }

                    if (tDto.Ativo != tracoOld.Ativo && mesmoTraco)
                    {
                        _obraService.Adicionar(new ObraLogVersao()
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = tDto.Ativo == "S" ? "ALTERAÇÃO TRAÇO PARA ATIVO" : "ALTERAÇÃO TRAÇO PARA INATIVO",
                            Complemento = tDto.Ativo == "S" ? $"Traço alterado de inativo para ativo. (Sequência {tDto.Sequencia})" : $"Traço alterado de ativo para inativo. (Sequência {tDto.Sequencia})",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if ((tDto.PrecoReajustadoAtual != tracoOld.M3PrecoProposto) && (tDto.DataUltimoReajuste != null))
                    {
                        var ultimoReajusteTraco = _contratoService.ListarFiltrados<ContratoTracoReajusteVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Obra.UsinaCodigo
                            && t.ContratoAno == propostaRequest.Obra.AnoContrato && t.ContratoNumero == propostaRequest.Obra.NumContrato && t.ObraTracoSequencia == tDto.Sequencia)
                            .OrderByDescending(t => t.DataVigencia).FirstOrDefault();
                        
                        if(ultimoReajusteTraco != null)
                        {
                            if (ultimoReajusteTraco.PrecoRecalculado != tDto.PrecoReajustadoAtual)
                            {
                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                                    ObraCodigo = propostaRequest.Obra.Numero,
                                    AnoChamada = propostaRequest.Ano,
                                    NumChamada = propostaRequest.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO REAJUSTE TRAÇO",
                                    Complemento = $"Alteração do preço reajustado do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                                    Observacao = $"De: {tracoOld.M3PrecoProposto} Para: {tDto.PrecoReajustadoAtual} / Preço Ultimo Reajuste: {ultimoReajusteTraco.PrecoRecalculado}",
                                    Sequencia = sequenciaObraLog++
                                });
                            }
                        }

                        var tracoCusto = _tracoCustoService
                            .ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(propostaRequest.Obra.UsinaEntrega.Codigo, tracoOld.UsoCodigo,
                            tracoOld.PedraCodigo, tracoOld.SlumpCodigo, tracoOld.ResistenciaTipoCodigo, tracoOld.Fck, tracoOld.Consumo, propostaRequest.Data);

                        if (tracoCusto == null && propostaRequest.Data < DateTime.Today)
                        {
                            tracoCusto = _tracoCustoService
                                .ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(propostaRequest.Obra.UsinaEntrega.Codigo, tracoOld.UsoCodigo,
                                tracoOld.PedraCodigo, tracoOld.SlumpCodigo, tracoOld.ResistenciaTipoCodigo, tracoOld.Fck, tracoOld.Consumo, DateTime.Today);
                        }

                        tracoOld.CustoServicoReajustado = tDto.PrecoReajustadoAtual - tracoCusto.CustoAjustado;
                    }

                    // Buscando programações
                    var programacoes = _programacaoService
                    .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                        && t.PropostaAno == propostaOld.Ano
                        && t.PropostaNumero == propostaOld.Numero
                        && t.ObraNumero == obraOld.Numero
                        && t.ObraTracoSequencia == tracoOld.Sequencia
                        && t.Status != EProgramacaoStatus.Cancelada,
                        t => t.ResistenciaTipo, t => t.Pedra, t => t.Slump, t => t.Uso);

                    // programações
                    foreach (var prog in programacoes)
                    {
                        if (AutoMapper.Mapper.Map(tDto, new ObraTracoVersao()).TracoProgramacaoScopeIsValid(prog))
                        {
                            if (tracoOld.M3PrecoProposto != tDto.M3PrecoProposto)
                            {
                                _programacaoService.Adicionar(new ProgramacaoLog()
                                {
                                    UsinaCodigo = prog.UsinaCodigo,
                                    ObraCodigo = prog.ObraNumero,
                                    ProgramacaoSequencia = prog.Sequencia,
                                    PropostaAno = prog.PropostaAno,
                                    PropostaNumero = prog.PropostaNumero,
                                    ContratoAno = prog.ContratoAno,
                                    ContratoNumero = prog.ContratoNumero,
                                    DataHora = DateTime.Now,
                                    Horario = "",
                                    Usuario = usuario,
                                    Evento = "Alteração",
                                    Complemento = "Traço",
                                    Descricao = $"Alteração no Valor Unitário do Traço {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao} \n" +
                                    $"De: {tracoOld.Descricao} R$ {tracoOld.M3PrecoProposto.ToString("F2")} Para: {descricaoTracoNovo} R$ {tDto.M3PrecoProposto.ToString("F2")}",
                                    Sequencia = sequenciaProgramacaoLog++
                                });

                                Commit();
                            }
                        }
                    }

                    tracoOld.AtualizaStatusAprovacao(usuario);


                    if (!mesmoTraco)
                    {

                        var tracoAntigoIsCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObraVersao(tracoOld, obraOld) == 7105;

                        _obraService.Adicionar(new ObraLogVersao()
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO DE TRAÇO",
                            Complemento = $"Alteração de traço sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De:{(tracoAntigoIsCustoVirtual ? " [CUSTO VIRTUAL]" : "")} {tracoOld.Descricao} - {tracoOld.M3Quantidade.ToString("F1")} M3 - R$ {tracoOld.M3PrecoProposto.ToString("F2")}" +
                            $" Para:{(isTracoCustoVirtual ? " [CUSTO VIRTUAL]" : "")} {descricaoTracoNovo} - {tDto.M3Quantidade.ToString("F1")} M3 - R$ {tDto.M3PrecoProposto.ToString("F2")}",
                            Sequencia = sequenciaObraLog++
                        });

                    }

                    var tracoParaAprovacao = AutoMapper.Mapper.Map<ObraTracoVersao>(tDto);
                    _obraService.CalcularEbitdaObraTraco(tracoParaAprovacao, obraOld);

                    if (parametroValidaAprovacaoComparandoComPrecoTabela && ((tDto.PrecoReajustadoAtual != 0 ? tDto.PrecoReajustadoAtual : tDto.M3PrecoProposto) != tracoOld.M3PrecoTabela)
                        || (!mesmoTraco || (tDto.PrecoReajustadoAtual != 0 ? tDto.PrecoReajustadoAtual : tDto.M3PrecoProposto) < tracoOld.M3PrecoProposto))
                    {
                        tDto.AprovacaoObservacao = "";

                        if (utilizaAprovacaoComercicalPorAlcada)
                        {
                            if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTracoVersao(propostaOld, obraOld, tracoParaAprovacao) == EStatusAprovacao.Pendente)
                            {
                                tDto.AprovacaoVerbal = "N";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "G";
                            }
                            else
                            {
                                tDto.AprovacaoVerbal = "";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "";
                            }
                        }

                        _obraService.AtualizarStatusEngenharia(obraOld);
                    } else if(utilizaAprovacaoComercicalPorAlcada 
                                && tDto.M3PrecoProposto != tracoOld.M3PrecoProposto 
                                && (tracoOld.StatusAprovacao == EStatusAprovacao.Pendente || tracoOld.TracoReprovado()))
                    {

                        tDto.AprovacaoObservacao = "";

                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTracoVersao(propostaOld, obraOld, tracoParaAprovacao) == EStatusAprovacao.Pendente)
                        {
                            tDto.AprovacaoVerbal = "N";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "G";
                        }
                        else
                        {
                            tDto.AprovacaoVerbal = "";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "";
                        }
                    }

                    tracoOld = AutoMapper.Mapper.Map(tDto, tracoOld);

                    if (tDto.DataUltimoReajuste != null)
                    {
                        if (numVersao > 1)
                            tracoOld.M3PrecoProposto = tracoOld.PrecoReajustadoAtual;

                        tracoOld.CustoServicoAnterior = 0;
                        tracoOld.CustoServicoReajustado = 0;
                        tracoOld.PrecoReajustadoAnterior = 0;
                        tracoOld.PrecoReajustadoAtual = 0;
                        tracoOld.DataUltimoReajuste = null;

                        _obraService.Atualizar(tracoOld);
                    }

                    _obraService.CalcularEbitdaObraTraco(tracoOld, obraOld);
                }
                else
                {
                    tracoOld = AutoMapper.Mapper.Map<ObraTracoVersao>(tDto);
                    tracoOld.NumeroVersao = numVersao;

                    var mercadoria = _mercadoriaService.ObterTracoMercadoria(tDto.Uso.Codigo, tDto.Pedra.Codigo, tDto.Slump.Codigo, tDto.ResistenciaTipo.Codigo, tDto.Mpa, tDto.Consumo);
                    var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObraVersao(AutoMapper.Mapper.Map(tDto, new ObraTracoVersao()), obraOld) == 7105;
                    var descricaoTraco = mercadoria is null ? "" : mercadoria.Descricao;

                    _obraService.Adicionar(new ObraLogVersao()
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                        ObraCodigo = propostaRequest.Obra.Numero,
                        AnoChamada = propostaRequest.Ano,
                        NumChamada = propostaRequest.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "INSERÇÃO DE TRAÇO",
                        Complemento = $"Inserção do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {descricaoTraco}",
                        Observacao = $"{descricaoTraco} - {tracoOld.M3Quantidade.ToString("F1")} M3 - R$ {tracoOld.M3PrecoProposto.ToString("F2")}",
                        Sequencia = sequenciaObraLog++
                    });

                    _obraService.CalcularEbitdaObraTraco(tracoOld, obraOld);

                    if (utilizaAprovacaoComercicalPorAlcada)
                    {
                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTracoVersao(propostaOld, obraOld, tracoOld) == EStatusAprovacao.Pendente)
                        {
                            tracoOld.AprovacaoVerbal = "N";
                            tracoOld.AprovacaoObservacao = "";
                            tracoOld.AprovacaoOperacao = "G";
                        }
                    }

                    _propostaService.VerificaTracoJaInclusoVersao(tracoOld, numVersao);

                    tracoOld.AtualizaStatusAprovacao(usuario);

                    _obraService.Adicionar(tracoOld);
                    _obraService.AtualizarStatusEngenharia(obraOld);
                }

                _propostaService.ValidarNumeracaoProdutoCorretaObraTracoVersao(tracoOld, obraOld.UsinaEntregaCodigo);

                _obraService.AdicionarLogPropostaItem(tracoOld, "PropostaApplicationService.AtualizarContratoVersao");
                Commit();
            }

            var seqs = sequencias.ToArray();
            var tracosExcluidos = _obraService.ListarFiltradosTracking<ObraTracoVersao>
                (t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in tracosExcluidos)
            {
                var temProgramacao = _programacaoService
                    .ListarFiltradosTracking(p => p.UsinaCodigo == obraOld.UsinaCodigo
                        && p.PropostaAno == propostaOld.Ano
                        && p.PropostaNumero == propostaOld.Numero
                        && p.ObraTracoSequencia == t.Sequencia).Count() > 0;

                if (temProgramacao)
                {
                    AssertionConcern.Notify("ObraTracos", $"Não é possível excluir o traço de sequência {t.Sequencia} pois existe programação vinculada.");
                    continue;
                }

                var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObraVersao(t, obraOld) == 7105;

                _obraService.Adicionar(new ObraLogVersao()
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "REMOÇÃO DE TRAÇO",
                    Complemento = $"Remoção do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {t.Sequencia}, Descrição: {t.Descricao}",
                    Observacao = $"{t.Descricao} - {t.M3Quantidade.ToString("F1")} M3 - R$ {t.M3PrecoProposto.ToString("F2")}",
                    Sequencia = sequenciaObraLog++
                });

                if (utilizaAprovacaoComercicalPorAlcada)
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaTracoVersao(propostaOld, t);

                _obraService.Remover(t);
                _obraService.AdicionarLogPropostaItem(t, "PropostaApplicationService.AtualizarContratoVersao(Exclusão)");
                 Commit();
            }

            // ObraBombas
            sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraBombas)
            {
                sequencias.Add(tDto.Sequencia);
                var bombaOld = _obraService.ObterPorId<ObraBombaVersao>
                    (numVersao, tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                if (bombaOld != null)
                {


                    var mesmaBomba = (bombaOld.BombaTipoCodigo ?? 0) == (tDto.BombaTipo is null ? 0 : tDto.BombaTipo.Codigo);

                    if (mesmaBomba)
                    {

                        var oldBombaTipo = _cadastroGeralService.ObterPorId(bombaOld.BombaTipoCodigo ?? 0, ECadastroGeralTipo.EquipamentoTipo);

                        if (bombaOld.DistanciaTubulacao != tDto.DistanciaTubulacao)
                        {
                            _obraService.Adicionar(new ObraLogVersao
                            {
                                NumeroVersao = numVersao,
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO DISTÂNCIA TUBULAÇÃO",
                                Complemento = $"Alteração de distância da tubulação na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: {bombaOld.DistanciaTubulacao} Para: {tDto.DistanciaTubulacao}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }

                        if (tDto.DataUltimoReajuste != null)
                        {
                            var ultimoReajusteBomba = _contratoService.ListarFiltrados<ContratoBombaReajusteVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Obra.UsinaCodigo
                                    && t.ContratoAno == propostaRequest.Obra.AnoContrato && t.ContratoNumero == propostaRequest.Obra.NumContrato && t.ObraBombaReajusteSequencia == tDto.Sequencia)
                                    .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                            if (ultimoReajusteBomba != null)
                            {
                                if (tDto.TaxaMinimaReajustadaAtual != bombaOld.TaxaMinimaPrecoProposto && tDto.TaxaMinimaReajustadaAtual != ultimoReajusteBomba.ValorReajustado)
                                {
                                    _obraService.Adicionar(new ObraLogVersao
                                    {
                                        NumeroVersao = numVersao,
                                        UsinaCodigo = propostaOld.Obra.UsinaCodigo,
                                        ObraCodigo = propostaOld.Obra.Numero,
                                        AnoChamada = propostaOld.Ano,
                                        NumChamada = propostaOld.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "ALTERAÇÃO REAJUSTE TAXA MÍNIMA",
                                        Complemento = $"Alteração de taxa mínima reajustada na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                        Observacao = $"De: R$ {bombaOld.TaxaMinimaPrecoProposto.ToString("F2")} \nPara: R$ {tDto.TaxaMinimaReajustadaAtual.ToString("F2")} / Valor Último Reajuste: {ultimoReajusteBomba.ValorReajustado}",
                                        Sequencia = sequenciaObraLog++
                                    });

                                    Commit();
                                }

                                if (tDto.M3ReajustadoAteAtual != bombaOld.M3PropostoAte && tDto.M3ReajustadoAteAtual != ultimoReajusteBomba.ReajustadoAteM3)
                                {
                                    _obraService.Adicionar(new ObraLogVersao
                                    {
                                        NumeroVersao = numVersao,
                                        UsinaCodigo = propostaOld.UsinaCodigo,
                                        ObraCodigo = propostaOld.Obra.Numero,
                                        AnoChamada = propostaOld.Ano,
                                        NumChamada = propostaOld.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "ALTERAÇÃO REAJUSTE M3 ATÉ",
                                        Complemento = $"Alteração de M3 Até reajustado na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                        Observacao = $"De: {bombaOld.M3PropostoAte} M3 \nPara: {tDto.M3ReajustadoAteAtual} M3 / Valor Último Reajuste: {ultimoReajusteBomba.ReajustadoAteM3}",
                                        Sequencia = sequenciaObraLog++
                                    });

                                    Commit();
                                }

                                if (tDto.M3PrecoReajustadoAtual != bombaOld.M3PrecoProposto && tDto.M3PrecoReajustadoAtual != ultimoReajusteBomba.M3ExcedenteReajustado)
                                {
                                    _obraService.Adicionar(new ObraLogVersao
                                    {
                                        NumeroVersao = numVersao,
                                        UsinaCodigo = propostaOld.Obra.UsinaCodigo,
                                        ObraCodigo = propostaOld.Obra.Numero,
                                        AnoChamada = propostaOld.Ano,
                                        NumChamada = propostaOld.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "ALTERAÇÃO REAJUSTE PREÇO M3",
                                        Complemento = $"Alteração de preço M3 reajustado na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                        Observacao = $"De: R$ {bombaOld.M3PrecoProposto.ToString("F2")} \nPara: R$ {tDto.M3PrecoReajustadoAtual.ToString("F2")} / Valor Último Reajuste: {ultimoReajusteBomba.M3ExcedenteReajustado}",
                                        Sequencia = sequenciaObraLog++
                                    });

                                    Commit();
                                }
                            }
                        }
                        else
                        {
                            if (bombaOld.TaxaMinimaPrecoProposto != tDto.TaxaMinimaPrecoProposto)
                            {
                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO TAXA MÍNIMA BOMBA",
                                    Complemento = $"Alteração de taxa mínima na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                    Observacao = $"De: R$ {bombaOld.TaxaMinimaPrecoProposto.ToString("F2")} Para: R$ {tDto.TaxaMinimaPrecoProposto.ToString("F2")}",
                                    Sequencia = sequenciaObraLog++
                                });

                                Commit();
                            }

                            if (bombaOld.M3PropostoAte != tDto.M3PropostoAte)
                            {

                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO BOMBA M3 ATÉ",
                                    Complemento = $"Alteração de M3 Até na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                    Observacao = $"De: {bombaOld.M3PropostoAte} M3 Para: {tDto.M3PropostoAte} M3",
                                    Sequencia = sequenciaObraLog++
                                });

                                Commit();

                            }

                            if (bombaOld.M3PrecoProposto != tDto.M3PrecoProposto)
                            {
                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO PREÇO M3 BOMBA",
                                    Complemento = $"Alteração de preço M3 na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                    Observacao = $"De: R$ {bombaOld.M3PrecoProposto.ToString("F2")} Para: R$ {tDto.M3PrecoProposto.ToString("F2")}",
                                    Sequencia = sequenciaObraLog++
                                });

                                Commit();
                            }
                        }
                    }

                    if (!mesmaBomba)
                    {

                        var oldBombaTipo = _cadastroGeralService.ObterPorId(bombaOld.BombaTipoCodigo ?? 0, ECadastroGeralTipo.EquipamentoTipo);

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO BOMBA",
                            Complemento = $"Alteração na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                            Observacao = $"De: {(oldBombaTipo is null ? "TIPO NÃO INFORMADO" : oldBombaTipo.Descricao)} \nPara: {(tDto.BombaTipo is null ? "TIPO NÃO INFORMADO" : tDto.BombaTipo.Descricao)}",
                            Sequencia = sequenciaObraLog++
                        });

                        Commit();
                    }


                    // Buscando programações
                    var programacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraBombaSequencia == bombaOld.Sequencia);

                    // programações
                    foreach (var prog in programacoes)
                    {
                        if (AutoMapper.Mapper.Map(tDto, new ObraBombaVersao()).BombaProgramacaoScopeIsValid(bombaOld.BombaTipoCodigo))
                        {
                            var descricaoAlteracao = "";

                            if (prog.DistanciaTubulacao != tDto.DistanciaTubulacao)
                            {
                                descricaoAlteracao += $"Distância tubulação: {prog.DistanciaTubulacao} --> {tDto.DistanciaTubulacao} ";
                                prog.DistanciaTubulacao = tDto.DistanciaTubulacao;
                            }

                            if (bombaOld.TaxaMinimaPrecoProposto != tDto.TaxaMinimaPrecoProposto)
                            {
                                descricaoAlteracao += $"Taxa mínima: {bombaOld.TaxaMinimaPrecoProposto} --> {tDto.TaxaMinimaPrecoProposto} ";
                            }

                            if (bombaOld.M3PropostoAte != tDto.M3PropostoAte)
                            {
                                descricaoAlteracao += $"M3 até: {bombaOld.M3PropostoAte} --> {tDto.M3PropostoAte} ";
                            }

                            if (bombaOld.M3PrecoProposto != tDto.M3PrecoProposto)
                            {
                                descricaoAlteracao += $"Preço M3: {bombaOld.M3PrecoProposto} --> {tDto.M3PrecoProposto} ";
                            }

                            if (descricaoAlteracao != "")
                            {
                                Commit();

                                _programacaoService.Adicionar(new ProgramacaoLog()
                                {
                                    UsinaCodigo = prog.UsinaCodigo,
                                    ObraCodigo = prog.ObraNumero,
                                    ProgramacaoSequencia = prog.Sequencia,
                                    PropostaAno = prog.PropostaAno,
                                    PropostaNumero = prog.PropostaNumero,
                                    ContratoAno = prog.ContratoAno,
                                    ContratoNumero = prog.ContratoNumero,
                                    DataHora = DateTime.Now,
                                    Horario = "",
                                    Usuario = usuario,
                                    Evento = "Alteração",
                                    Complemento = "Bomba",
                                    Descricao = descricaoAlteracao,
                                    Sequencia = sequenciaProgramacaoLog++
                                });

                                Commit();

                            }
                        }
                    }

                    bombaOld.AtualizaStatusAprovacao(usuario);

                    if (tDto.M3PrecoProposto < bombaOld.M3PrecoProposto || tDto.M3PropostoAte < bombaOld.M3PropostoAte
                        || tDto.TaxaMinimaPrecoProposto < bombaOld.TaxaMinimaPrecoProposto
                        || tDto.HoraPrecoProposto < bombaOld.HoraPrecoProposto || tDto.HoraPropostoAte < bombaOld.HoraPropostoAte
                        || tDto.HoraTaxaMinimaPrecoProposto < bombaOld.HoraTaxaMinimaPrecoProposto)
                    {
                        tDto.AprovacaoObservacao = "";
                        tDto.AprovacaoVerbal = "";

                        if (utilizaAprovacaoComercicalPorAlcada)
                        {
                            if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBombaVersao(propostaOld, obraOld, AutoMapper.Mapper.Map(tDto, new ObraBombaVersao())) == EStatusAprovacao.Pendente)
                            {
                                tDto.AprovacaoVerbal = "S";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "G";
                            }
                        }

                    }
                    else if (utilizaAprovacaoComercicalPorAlcada
                            && (tDto.M3PrecoProposto != bombaOld.M3PrecoProposto || tDto.TaxaMinimaPrecoProposto != bombaOld.TaxaMinimaPrecoProposto)
                            && (bombaOld.StatusAprovacao == EStatusAprovacao.Pendente || bombaOld.StatusAprovacao == EStatusAprovacao.Reprovado))
                    {

                        tDto.AprovacaoObservacao = "";
                        tDto.AprovacaoVerbal = "";

                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBombaVersao(propostaOld, obraOld, AutoMapper.Mapper.Map(tDto, new ObraBombaVersao())) == EStatusAprovacao.Pendente)
                        {
                            tDto.AprovacaoVerbal = "S";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "G";
                        } 
                    }

                    if (tDto.AprovacaoObservacao == "" && tDto.M3PrecoProposto == bombaOld.M3PrecoProposto && tDto.M3PropostoAte == bombaOld.M3PropostoAte
                        && tDto.TaxaMinimaPrecoProposto == bombaOld.TaxaMinimaPrecoProposto
                        && tDto.HoraPrecoProposto == bombaOld.HoraPrecoProposto && tDto.HoraPropostoAte == bombaOld.HoraPropostoAte
                        && tDto.HoraTaxaMinimaPrecoProposto == bombaOld.HoraTaxaMinimaPrecoProposto && !utilizaAprovacaoComercicalPorAlcada) tDto.AprovacaoObservacao = "sem_alteracao";

                    bombaOld = AutoMapper.Mapper.Map(tDto, bombaOld);

                    if (tDto.DataUltimoReajuste != null)
                    {
                        if (numVersao > 1)
                        {
                            bombaOld.M3PropostoAte = bombaOld.M3ReajustadoAteAtual;
                            bombaOld.TaxaMinimaPrecoProposto = bombaOld.TaxaMinimaReajustadaAtual;
                            bombaOld.M3PrecoProposto = bombaOld.M3PrecoReajustadoAtual;
                        }

                        bombaOld.M3ReajustadoAteAnterior = 0;
                        bombaOld.TaxaMinimaReajustadaAnterior = 0;
                        bombaOld.M3PrecoReajustadoAnterior = 0;
                        bombaOld.M3ReajustadoAteAtual = 0;
                        bombaOld.TaxaMinimaReajustadaAtual = 0;
                        bombaOld.M3PrecoReajustadoAtual = 0;
                        bombaOld.DataUltimoReajuste = null;

                        _obraService.Atualizar(bombaOld);

                        _obraService.CalcularEbitdaObraBomba(bombaOld, obraOld);
                    }

                    if (!tDto.AprovacaoObservacao.Equals("sem_alteracao"))
                        bombaOld.AtualizaStatusAprovacao(usuario);
                }
                else
                {
                    bombaOld = AutoMapper.Mapper.Map(tDto, new ObraBombaVersao());
                    bombaOld.NumeroVersao = numVersao;

                    _obraService.Adicionar(new ObraLogVersao
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = propostaOld.UsinaCodigo,
                        ObraCodigo = propostaOld.Obra.Numero,
                        AnoChamada = propostaOld.Ano,
                        NumChamada = propostaOld.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "INSERÇÃO BOMBA",
                        Complemento = $"Inserção de bomba sequência {bombaOld.Sequencia}",
                        Observacao = $"{(tDto.BombaTipo is null ? "TIPO NÃO INFORMADO" : tDto.BombaTipo.Descricao)}",
                        Sequencia = sequenciaObraLog++
                    });

                    Commit();

                    if (utilizaAprovacaoComercicalPorAlcada)
                    {
                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBombaVersao(propostaOld, obraOld, bombaOld) == EStatusAprovacao.Pendente)
                        {

                            bombaOld.AprovacaoVerbal = "S";
                            bombaOld.AprovacaoObservacao = "";
                            bombaOld.AprovacaoOperacao = "G";

                        }
                    }

                    bombaOld.AtualizaStatusAprovacao(usuario);

                    _obraService.Adicionar(bombaOld);
                }
                Commit();
            }

            seqs = sequencias.ToArray();
            var bombasExcluidas = _obraService.ListarFiltradosTracking<ObraBombaVersao>
                (t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in bombasExcluidas)
            {
                var descricaoBombaTipo = _cadastroGeralService.ObterDescricaoEquipamentoBombaPorObraBomba(t.UsinaCodigo, t.ObraCodigo, t.Sequencia, numVersao);

                _obraService.Adicionar(new ObraLogVersao
                {
                    NumeroVersao = numVersao,
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "REMOÇÃO DA BOMBA",
                    Complemento = $"Remoção da bomba sequência {t.Sequencia}",
                    Observacao = $"{descricaoBombaTipo}",
                    Sequencia = sequenciaObraLog++
                });

                if (utilizaAprovacaoComercicalPorAlcada)
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaBombaVersao(propostaOld, t);

                _obraService.Remover(t);
                Commit();
            }

            // Aprovação por Alçada Volume
            if (utilizaAprovacaoComercicalPorAlcada && volumeTotalObraOld != volumeTotalObraNew)
            {

                var statusAprovacao = _aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaVolumeVersao(propostaOld, obraOld);

                if (statusAprovacao == EStatusAprovacao.Pendente)
                    obraOld.VolumeStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;

                if (statusAprovacao == EStatusAprovacao.NaoNecessita)
                {
                    obraOld.VolumeStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaVolumeVersao(propostaOld);
                }

                Commit();

            }

            // Aprovação por Alçada CondicaoPagamento
            if (utilizaAprovacaoComercicalPorAlcada && propostaOld.Obra.CondicaoPagamentoCodigo != condicaoPagamentoAnterior)
            {

                var statusAprovacao = _aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaCondicaoPagamentoVersao(propostaOld, obraOld);

                if (statusAprovacao == EStatusAprovacao.Pendente)
                    obraOld.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;

                if (statusAprovacao == EStatusAprovacao.NaoNecessita)
                {
                    obraOld.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaCondicaoPagamentoVersao(propostaOld);
                }

                Commit();

            }

            if (utilizaAprovacaoComercicalPorAlcada)
            {
                _aprovacaoComercialPendenteService.RevisarAprovacaoComercialPendenteVersao(obraOld, null, propostaOld);
            }

            var taxasOldComparacao = _obraTaxaService.ListarByIdObra(propostaRequest.Obra.UsinaEntrega.Codigo, propostaRequest.Obra.Numero);

            // ObraTaxas
            foreach (var tDto in propostaRequest.Obra.ObraTaxas)
            {
                if (tDto.Nova && tDto.Selecionada == "N") continue;

                tDto.UsinaCodigo = propostaRequest.Obra.UsinaEntrega.Codigo;

                var taxaOld = _obraService.ObterPorId<ObraTaxaVersao>
                    (numVersao, tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                var taxaOldComparacao = taxasOldComparacao.FirstOrDefault(x => x.Sequencia == tDto.Sequencia);

                if (taxaOld != null)
                {

                    var descricaoAlteracao = new Dictionary<string, string>();

                    if (taxaOldComparacao != null)
                    {

                        if (!tDto.CobrarVolume.Equals(taxaOldComparacao.CobrarVolume, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("COBRADO POR", $"De: {taxaOldComparacao.Descricao} Para: {tDto.Descricao}");

                        if (!tDto.Volume.Equals(taxaOldComparacao.Volume, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("VOLUME", $"De: {taxaOldComparacao.Volume} M3 Para: {tDto.Volume} M3");

                        if (!tDto.ValorTipo.Equals(taxaOldComparacao.ValorTipo, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("TIPO DE VALOR", $"De: {taxaOldComparacao.ValorTipo} Para: {tDto.ValorTipo}");

                        if (tDto.Valor != taxaOldComparacao.Valor)
                            descricaoAlteracao.Add("VALOR", $"De: {taxaOldComparacao.ValorTipo} {taxaOldComparacao.Valor.ToString("F2")} por {taxaOldComparacao.ValorPor}  Para: {tDto.ValorTipo} {tDto.Valor.ToString("F2")} por {tDto.ValorPor}");

                        if (!tDto.ValorPor.Equals(taxaOldComparacao.ValorPor, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("VALOR POR", $"De: {taxaOldComparacao.ValorPor} Para: {tDto.ValorPor}");

                        if (!tDto.HorarioAntesDas.Equals(taxaOldComparacao.HorarioAntesDas, StringComparison.OrdinalIgnoreCase)
                            || !tDto.HorarioAntesDas.Equals(taxaOldComparacao.HorarioAntesDas, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("HORÁRIO", $"De: Antes das {taxaOldComparacao.HorarioAntesDas} e após as {taxaOldComparacao.HorarioAposAs} Para: Antes das {tDto.HorarioAntesDas} e após as {tDto.HorarioAposAs}");

                        if (!tDto.Selecionada.Equals(taxaOldComparacao.Selecionada, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("SELECIONADA", $"De: {(taxaOldComparacao.Selecionada.Equals("S") ? "Selecionada" : "Não Selecionada")} Para: {(tDto.Selecionada.Equals("S") ? "Selecionada" : "Não Selecionada")}");

                    }

                    if (descricaoAlteracao.Count > 0)
                    {

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = $"TAXA EXTRA ALTERADA SEQUÊNCIA {tDto.Sequencia}, {string.Join("/", descricaoAlteracao.Keys.ToArray())}",
                            Complemento = $"{tDto.Descricao}\n{string.Join(", \n", descricaoAlteracao.Values.ToArray())}",
                            Observacao = $"",
                            Sequencia = sequenciaObraLog++
                        });

                    }

                    taxaOld = AutoMapper.Mapper.Map(tDto, taxaOld);
                    taxaOld.IdAtualizacao = obraOld.IdAtualizacao;
                }
                else
                {
                    taxaOld = AutoMapper.Mapper.Map<ObraTaxaVersao>(tDto);
                    taxaOld.ObraCodigo = propostaRequest.Obra.Numero;
                    taxaOld.NumeroVersao = numVersao;
                    taxaOld.IdCadastro = StringHelper.GetIDD(usuario);
                    taxaOld.AprovacaoCiente = "";
                    taxaOld.AprovacaoUsuario = "";
                    taxaOld.LogObservacao = "";
                    taxaOld.NumeroVersao = numVersao;
                    _obraService.Adicionar(taxaOld);
                }
                Commit();

                if (taxaOld.IsPersonalizada && taxaOld.Selecionada == "S")
                {
                    _obraTaxaService.SalvarPersonalizada(taxaOld);
                }
                else
                {
                    _obraTaxaService.DeletarPersonalizada(taxaOld);
                }
                Commit();
            }

            // ObraTributacoesMunicipais
            var usinasTributacaoMunicipal = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraTributacoesMunicipais)
            {
                usinasTributacaoMunicipal.Add(tDto.UsinaEntregaCodigo);
                tDto.ObraUsinaCodigo = propostaRequest.Obra.UsinaCodigo;
                tDto.ObraNumero = propostaRequest.Obra.Numero;
                tDto.ContratoAno = obraOld.AnoContrato;
                tDto.ContratoNumero = obraOld.NumContrato;

                var tribMunOld = _obraService.ObterPorId<ObraTributacaoMunicipalVersao>
                    (numVersao, tDto.ObraUsinaCodigo, tDto.ObraNumero, tDto.UsinaEntregaCodigo);

                if (tribMunOld != null)
                {
                    tribMunOld = AutoMapper.Mapper.Map(tDto, tribMunOld);
                }
                else
                {
                    tribMunOld = AutoMapper.Mapper.Map<ObraTributacaoMunicipalVersao>(tDto);
                    tribMunOld.NumeroVersao = numVersao;
                    _obraService.Adicionar(tribMunOld);
                }
                Commit();
            }

            var usinasTribMun = usinasTributacaoMunicipal.ToArray();
            var usinasTribMunExcluidas = _obraService.ListarFiltradosTracking<ObraTributacaoMunicipalVersao>
                (t => t.NumeroVersao == numVersao && t.ObraUsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraNumero == propostaRequest.Obra.Numero
                    && !usinasTribMun.Contains(t.UsinaEntregaCodigo));

            foreach (var t in usinasTribMunExcluidas)
            {
                _obraService.Remover(t);
                Commit();
            }

            // ObraDemaisServicos
            sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraDemaisServicos)
            {
                sequencias.Add(tDto.Sequencia);
                tDto.UsinaCodigo = propostaRequest.Obra.UsinaCodigo;
                tDto.ObraNumero = propostaRequest.Obra.Numero;

                var demaisServicosOld = _obraService.ObterPorId<ObraDemaisServicosVersao>
                    (numVersao, tDto.UsinaCodigo, tDto.ObraNumero, tDto.Sequencia);

                if (demaisServicosOld != null)
                {
                    demaisServicosOld = AutoMapper.Mapper.Map(tDto, demaisServicosOld);
                }
                else
                {
                    demaisServicosOld = AutoMapper.Mapper.Map<ObraDemaisServicosVersao>(tDto);
                    demaisServicosOld.NumeroVersao = numVersao; 
                    _obraService.Adicionar(demaisServicosOld);
                }
                Commit();
            }

            seqs = sequencias.ToArray();
            var demaisServicosExcluidos = _obraService.ListarFiltradosTracking<ObraDemaisServicosVersao>
                (t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraNumero == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in demaisServicosExcluidos)
            {
                _obraService.Remover(t);
                Commit();
            }

            var taxas = _obraTaxaService.ListarByIdObra(obraOld.UsinaEntregaCodigo, obraOld.Numero, obraOld.NumeroVersao);
            var valorConcreto = obraOld.CalcularValorConcreto();
            var naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
            var valorBomba = obraOld.CalcularValorBomba(naoConsideraTodasBombas);

            var valorDemaisServicos = obraOld.CalcularValorDemaisServicos();
            obraOld.ValorDemaisServicos = valorDemaisServicos;
            Commit();

            var valorM3Faltante = _obraTaxaService.ObterValorM3Faltante((propostaRequest.Obra.ObraBombas?.Count() ?? 0) > 0,
                propostaRequest.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, propostaRequest.Obra.VolumePorCarga, obraOld.ObraTaxas ?? taxas);
            var valorAdicionalPorKmRodado = _obraTaxaService.ObterValorAdicionalPorKmRodado(propostaRequest.Obra.DistanciaUsina,
                propostaRequest.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, propostaRequest.Obra.VolumePorCarga, obraOld.ObraTaxas ?? taxas, propostaRequest.Obra.ObraBombas.Count > 0);
            //var valorExtras = valorM3Faltante + (propostaRequest.Obra.VibradorValorUnitario * propostaRequest.Obra.VibradorQuantidade) + valorAdicionalPorKmRodado;
            var valorExtras = valorM3Faltante + valorAdicionalPorKmRodado;
            var valorTotalDecimal = valorConcreto + valorBomba + (decimal)valorExtras + (decimal)valorDemaisServicos;
            var valorTotal = float.Parse(valorTotalDecimal.ToString());

            bool incluiuPagamentoDetalhe = false;
            bool alterouPagamentoDetalhe = false;

            // ObraPagamentos
            sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraPagamentos)
            {
                // PropostaPagamentos
                sequencias.Add(tDto.Sequencia);
                var pagamentoOld = _obraService.ObterPorId<PropostaPagamentoVersao>
                    (numVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero, tDto.Sequencia);

                var formaPagamentoOld = pagamentoOld?.Forma ?? "";

                if (pagamentoOld != null)
                {
                    pagamentoOld = AutoMapper.Mapper.Map(tDto, pagamentoOld);
                    pagamentoOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                }
                else
                {
                    pagamentoOld = AutoMapper.Mapper.Map<PropostaPagamentoVersao>(tDto);
                    if (!pagamentoOld.PagamentoSequenciaIsValid()) return;
                    pagamentoOld.IdCadastro = StringHelper.GetIDD(usuario);
                    pagamentoOld.IdAtualizacao = "";
                    pagamentoOld.IdAprovacao = "";
                    pagamentoOld.NumeroVersao = numVersao;
                    _obraService.Adicionar(pagamentoOld);
                }

                pagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                pagamentoOld.PropostaAno = propostaRequest.Ano;
                pagamentoOld.PropostaNumero = propostaRequest.Numero;
                pagamentoOld.ObraCodigo = propostaRequest.Obra.Numero;
                pagamentoOld.Forma = tDto.TipoCobranca.Forma;
                pagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                pagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                pagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao == "" ? "S" : pagamentoOld.AtivoSimNao;
                pagamentoOld.Percentual = pagamentoOld.Valor / valorTotal * 100f;
                Commit();

                // Caso tenha Contrato
                if (propostaRequest.Obra.NumContrato > 0)
                {
                    // ContratoPagamentos
                    var contratoPagamentoOld = _contratoService.ObterPorId<ContratoPagamentoForSavingVersao>
                        (numVersao, propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato, tDto.Sequencia);

                    if (contratoPagamentoOld != null)
                    {

                        contratoPagamentoOld = AutoMapper.Mapper.Map(tDto, contratoPagamentoOld);
                        contratoPagamentoOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                        contratoPagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                        contratoPagamentoOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                        contratoPagamentoOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                        contratoPagamentoOld.Forma = tDto.TipoCobranca.Forma;
                        contratoPagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                        contratoPagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                        contratoPagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao == "" ? "S" : pagamentoOld.AtivoSimNao;
                        contratoPagamentoOld.Percentual = contratoPagamentoOld.Valor / valorTotal * 100f;
                    }
                    else
                    {

                        contratoPagamentoOld = AutoMapper.Mapper.Map<ContratoPagamentoForSavingVersao>(tDto);
                        contratoPagamentoOld.IdCadastro = StringHelper.GetIDD(usuario);
                        contratoPagamentoOld.IdAtualizacao = "";
                        contratoPagamentoOld.IdAprovacao = "";
                        contratoPagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                        contratoPagamentoOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                        contratoPagamentoOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                        contratoPagamentoOld.Forma = tDto.TipoCobranca.Forma;
                        contratoPagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                        contratoPagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                        contratoPagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao == "" ? "S" : pagamentoOld.AtivoSimNao;
                        contratoPagamentoOld.Percentual = contratoPagamentoOld.Valor / valorTotal * 100f;
                        contratoPagamentoOld.NumeroVersao = numVersao;
                        _obraService.Adicionar(contratoPagamentoOld);
                    }
                    Commit();
                }

                // PropostaPagamentoDetalhes *** verificar versao ***
                if ((new[] { "DP", "CC", "CD", "CH", "CP", "BE", "DN" }).Contains(tDto.TipoCobranca.Forma))
                {
                    var detalheSequencias = new List<int>();
                    foreach (var detalheDto in tDto.Detalhes)
                    {
                        // local void function
                        void atualizarPagamentoDetalhe<TDetalhe>(int usina, int? ano, int? numero, int pagamentoSequencia, int detalheSequencia, int obraNumero = 0) where TDetalhe : ObraPagamentoDetalhe
                        {
                            detalheSequencias.Add(detalheDto.DetalheSequencia);

                            var detalheOld = _obraService.BuscarDetalhesVersao<TDetalhe>(tDto.TipoCobranca.Forma, usina, ano ?? 0, numero ?? 0, pagamentoSequencia, detalheSequencia, obraNumero, numVersao, true);
                            //var detalheOld = _obraService.ObterPorId<TDetalhe>(chavePagamentoDetalhe);

                            if (detalheOld != null && formaPagamentoOld == tDto.TipoCobranca.Forma)
                            {
                                alterouPagamentoDetalhe = alterouPagamentoDetalhe || HouveAlteracao(formaPagamentoOld, detalheOld, detalheDto);

                                detalheOld = AutoMapper.Mapper.Map(detalheDto, detalheOld);

                                // TODO: verificar o caso do boleto express pois na tabela não existe
                                // o campo id_atual, apenas na view mapeada, o que torna o campo não alterável.
                                // Obs.: atualmente não há problema pois o detalhamento de boleto não é alterado pelo web
                                if (alterouPagamentoDetalhe)
                                    detalheOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                            }
                            else
                            {
                                if (detalheOld != null && formaPagamentoOld != tDto.TipoCobranca.Forma)
                                {
                                    _obraService.Remover(detalheOld);
                                }

                                if (formaPagamentoOld != tDto.TipoCobranca.Forma)
                                {
                                    if(detalheOld is ContratoPagamentoDetalheVersao)
                                    {
                                        var detalhesExcluidosOld = _obraService.ListarDetalhes<ContratoPagamentoDetalheVersao>
                                            (formaPagamentoOld, t => t.NumeroVersao == numVersao
                                                && t.UsinaCodigo == propostaRequest.Usina.Codigo
                                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                                && t.PagamentoSequencia == tDto.Sequencia
                                                && detalheSequencias.Contains(t.DetalheSequencia), true);

                                        foreach (var t in detalhesExcluidosOld)
                                        {
                                            _obraService.Remover(t);
                                            Commit();
                                        }
                                    } else
                                    {
                                        var detalhesExcluidosOld = _obraService.ListarDetalhes<PropostaPagamentoDetalheVersao>
                                            (formaPagamentoOld, t => t.NumeroVersao == numVersao
                                                && t.UsinaCodigo == propostaRequest.Usina.Codigo
                                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                                && t.PagamentoSequencia == tDto.Sequencia
                                                && detalheSequencias.Contains(t.DetalheSequencia), true);

                                        foreach (var t in detalhesExcluidosOld)
                                        {
                                            _obraService.Remover(t);
                                            Commit();
                                        }
                                    }
                                }

                                detalheOld = AutoMapper.Mapper.Map<TDetalhe>(detalheDto);

                                if (detalheOld is ContratoPagamentoDetalheVersao contratoDetalhe)
                                {
                                    contratoDetalhe.NumeroVersao = numVersao;
                                }
                                else if (detalheOld is PropostaPagamentoDetalheVersao propostaDetalhe)
                                {
                                    propostaDetalhe.NumeroVersao = numVersao;
                                }

                                if (!detalheOld.PagamentoDetalheSequenciaIsValid()) return;
                                detalheOld.IdCadastro = StringHelper.GetIDD(usuario);
                                detalheOld.IdAtualizacao = "";
                                _obraService.Adicionar(detalheOld);

                               incluiuPagamentoDetalhe = true;
                            }

                            detalheOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                            detalheOld.PropostaAno = propostaRequest.Obra.AnoChamada ?? 0;
                            detalheOld.PropostaNumero = propostaRequest.Obra.NumChamada ?? 0;
                            detalheOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                            detalheOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                            detalheOld.ObraCodigo = propostaRequest.Obra.Numero;
                            detalheOld.PagamentoSequencia = tDto.Sequencia;

                            if (tDto.TipoCobranca.Forma == "DP")
                            {
                                var newDetalhe = (IObraPagamentoDetalheDeposito)detalheOld;
                                newDetalhe.TomadorBanco = detalheDto.Portador.Conta.BancoCodigoOficial;
                                newDetalhe.TomadorAgencia = detalheDto.Portador.Conta.NumeroAgencia.ToString();
                                newDetalhe.TomadorNumeroConta = $"{detalheDto.Portador.Conta.NumeroConta}-{detalheDto.Portador.Conta.DvConta}";
                            }

                            Commit();
                        };

                        if (propostaRequest.Obra.NumContrato > 0)
                            atualizarPagamentoDetalhe<ContratoPagamentoDetalheVersao>(propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato, tDto.Sequencia, detalheDto.DetalheSequencia);
                        else
                            atualizarPagamentoDetalhe<PropostaPagamentoDetalheVersao>(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, tDto.Sequencia, detalheDto.DetalheSequencia, propostaRequest.Obra.Numero);
                    }

                    var detalheSeqs = detalheSequencias.ToArray();

                    if (propostaRequest.Obra.NumContrato > 0)
                    {
                        var detalhesExcluidos = _obraService.ListarDetalhes<ContratoPagamentoDetalheVersao>
                            (tDto.TipoCobranca.Forma, t => t.NumeroVersao == numVersao
                                && t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                && t.PagamentoSequencia == tDto.Sequencia
                                && !detalheSeqs.Contains(t.DetalheSequencia), true);

                        //var detalhesExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalhe>
                        //    (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                        //        && t.ContratoAno == propostaRequest.Obra.AnoContrato
                        //        && t.ContratoNumero == propostaRequest.Obra.NumContrato
                        //        && t.PagamentoSequencia == tDto.Sequencia
                        //        && !detalheSeqs.Contains(t.DetalheSequencia));

                        foreach (var t in detalhesExcluidos)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                    else
                    {
                        var detalhesExcluidos = _obraService.ListarDetalhes<PropostaPagamentoDetalheVersao>
                            (tDto.TipoCobranca.Forma, t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.PropostaAno == propostaRequest.Ano
                                && t.PropostaNumero == propostaRequest.Numero
                                && t.ObraCodigo == propostaRequest.Obra.Numero
                                && t.PagamentoSequencia == tDto.Sequencia
                                && !detalheSeqs.Contains(t.DetalheSequencia), true);

                        foreach (var t in detalhesExcluidos)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                }
                else
                {
                    var detalhesExcluidos = _obraService.ListarDetalhes<PropostaPagamentoDetalheVersao>
                        (tDto.TipoCobranca.Forma, t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                            && t.PropostaAno == propostaRequest.Ano
                            && t.PropostaNumero == propostaRequest.Numero
                            && t.ObraCodigo == propostaRequest.Obra.Numero
                            && t.PagamentoSequencia == tDto.Sequencia, true);

                    foreach (var t in detalhesExcluidos)
                    {
                        _obraService.Remover(t);
                        Commit();
                    }

                    if (formaPagamentoOld != tDto.TipoCobranca.Forma && propostaRequest.Obra.NumContrato > 0)
                    {
                        var detalhesExcluidosOld = _obraService.ListarDetalhes<ContratoPagamentoDetalheVersao>
                            (formaPagamentoOld, t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                && t.PagamentoSequencia == tDto.Sequencia, true);

                        foreach (var t in detalhesExcluidosOld)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                    else if (formaPagamentoOld != tDto.TipoCobranca.Forma)
                    {
                        var detalhesExcluidosOld = _obraService.ListarDetalhes<PropostaPagamentoDetalheVersao>
                        (formaPagamentoOld, t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaRequest.Usina.Codigo
                            && t.PropostaAno == propostaRequest.Ano
                            && t.PropostaNumero == propostaRequest.Numero
                            && t.ObraCodigo == propostaRequest.Obra.Numero
                            && t.PagamentoSequencia == tDto.Sequencia, true);

                        foreach (var t in detalhesExcluidosOld)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                }
            }

            if (incluiuPagamentoDetalhe && (propostaRequest.Obra.NumContrato ?? 0) > 0)
            {
                var contrato = _contratoService.ContratoVersaoObterPorId(obraOld.NumeroVersao, obraOld.UsinaCodigo, obraOld.AnoContrato.Value, obraOld.NumContrato.Value);
                var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(obraOld.NumeroVersao, contrato.Usina, contrato.Ano, contrato.Numero).ToList();
                _webHookApplicationService.AdicionarWebHookContratoPagamentoVersao(contrato, contratoPagamentos, EWebHookTipoEvento.Insert);
            }

            seqs = sequencias.ToArray();
            var pagamentosExcluidos = _obraService.ListarFiltradosTracking<PropostaPagamentoVersao>
                (t => t.NumeroVersao == propostaOld.NumeroVersao
                    && t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.PropostaAno == propostaRequest.Ano
                    && t.PropostaNumero == propostaRequest.Numero
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in pagamentosExcluidos)
            {
                var detalhes = _obraService.ListarDetalhes<PropostaPagamentoDetalheVersao>
                    (t.Forma, d => d.NumeroVersao == numVersao
                        && d.UsinaCodigo == propostaRequest.Usina.Codigo
                        && d.PropostaAno == propostaRequest.Ano
                        && d.PropostaNumero == propostaRequest.Numero
                        && d.ObraCodigo == propostaRequest.Obra.Numero
                        && d.PagamentoSequencia == t.Sequencia, true);

                foreach (var d in detalhes)
                {
                    _obraService.Remover(d);
                    Commit();
                }

                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentosExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoForSavingVersao>
                (t => t.NumeroVersao == numVersao
                    && t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ContratoAno == (propostaRequest.Obra.AnoContrato ?? 0)
                    && t.ContratoNumero == (propostaRequest.Obra.NumContrato ?? 0)
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in contratoPagamentosExcluidos)
            {
                _obraService.Remover(t);
                Commit();
            }

            //var propostaNew = _propostaService.ObterPorUsinaAnoNumero(propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero, true);
            if (_propostaService.ValidarProposta(usuario, propostaOld, filtroVendedores, cpfCnpjAnterior, propostaStatusAnterior))
            {
                if (!_notifications.HasNotifications())
                {
                    bool houveAlteracaoDadosProposta = false;

                    if (propostaOld.Obra.CondicaoPagamentoCodigo != condicaoPagamentoAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                         _obraService.Adicionar(new ObraLogVersao
                         {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = condicaoPagamentoAnterior > 0 ? "ALTERAÇÃO COND.PAGTO.PRINC." : "INSERÇÃO COND.PAGTO.PRINC.",
                            Complemento = (condicaoPagamentoAnterior > 0 ? $"De: {condicaoPagamentoAnterior} - {condicaoPagamentoAnteriorDescricao} Para: " : "Cond. Pagto.: ") +
                                $"{propostaOld.Obra.CondicaoPagamentoCodigo} - {_obraService.ObterPorId<CondicaoPagamento>(propostaOld.Obra.CondicaoPagamentoCodigo)?.Descricao ?? ""}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                         });
                    }

                    if (propostaOld.Obra.TipoCobrancaCodigo != tipoCobrancaAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = tipoCobrancaAnterior > 0 ? "ALTERAÇÃO TIPO COB.PRINC." : "INSERÇÃO TIPO COB.PRINC.",
                            Complemento = (tipoCobrancaAnterior > 0 ? $"De: {tipoCobrancaAnterior} - {tipoCobrancaAnteriorDescricao} Para: " : "Tipo Cob.: ") +
                                $"{propostaOld.Obra.TipoCobrancaCodigo} - {_obraService.ObterPorId<TipoCobranca>(propostaOld.Obra.TipoCobrancaCodigo)?.Descricao ?? ""}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.Vendedor.Nome != vendedorOld.Nome)
                    {
                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO DE VENDEDOR",
                            Complemento = $"O VENDEDOR {vendedorOld.Nome} FOI ALTERADO PARA O VENDEDOR {propostaOld.Vendedor.Nome}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaStatusAnterior != propostaOld.Status)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO STATUS",
                            Complemento = $"De: {(int)propostaStatusAnterior} - {propostaStatusAnterior} Para: {(int)propostaOld.Status} - {propostaOld.Status}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.ValorTotalContrato != valorTotalAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO TOTAL CONTRATO",
                            Complemento = $"Alteração no Valor De: {valorTotalAnterior} Para: {propostaOld.ValorTotalContrato}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.TempoAprovacaoMedicao != tempoAprovacaoMedicaoAnterior || propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        var aprovacaoMedicaoAnteriorString = aprovacaoMedicaoAnterior == "S" ? "SIM" : "NÃO";
                        var aprovacaoMedicaoString = propostaOld.AprovacaoMedicao == "S" ? "SIM" : "NÃO";

                        var complemento = "";
                        if (propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                            complemento += $"Medição Contrato De: {aprovacaoMedicaoAnteriorString} Para: {aprovacaoMedicaoString}";

                        if (tempoAprovacaoMedicaoAnterior != propostaOld.TempoAprovacaoMedicao)
                        {
                            if (propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                                complemento += " - ";

                            complemento += $"Tempo para Aprovação De: {tempoAprovacaoMedicaoAnterior}H Para: {propostaOld.TempoAprovacaoMedicao}H";
                        }

                        _obraService.Adicionar(new ObraLogVersao
                        {
                            NumeroVersao = numVersao,
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO MEDIÇÃO CONTRATO",
                            Complemento = complemento,
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    Commit();

                    if (obraOld.NumContrato > 0)
                    {
                        var contrato = _contratoService.ContratoVersaoObterPorId(obraOld.NumeroVersao, obraOld.UsinaCodigo, obraOld.AnoContrato.Value, obraOld.NumContrato.Value);
                        if (contrato != null)
                        {
                            contrato.NumeroContratoAnterior = propostaRequest.Obra.Contrato.NumeroContratoAnterior;
                            Commit();

                            //Adicional ZMRC
                            if (usaAdicionalZMRCAnterior == "S" && propostaRequest.Obra.UsaAdicionalZMRC == "N")
                            {
                                obraOld.NecessitaAprovacaoZMRC = "S";                                
                                if(!parametroDesativarObrigatoriedadeAprovacaoCadastro)
                                    contrato.Status = EContratoStatus.RevalidacaoCadastro;                                   

                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO ZMRC",
                                    Complemento = "DE SIM PARA NÃO",
                                    Observacao = "",
                                    Sequencia = sequenciaObraLog++
                                });
                            }

                            if (propostaRequest.Obra.Contrato?.DataEncerramento != contrato.DataEncerramento)
                            {
                                if (contrato.DataEncerramento != null)
                                {
                                    if (propostaRequest.Obra.Contrato?.DataEncerramento != null)
                                    {
                                        _obraService.Adicionar(new ObraLogVersao
                                        {
                                            NumeroVersao = numVersao,
                                            UsinaCodigo = propostaRequest.Usina.Codigo,
                                            ObraCodigo = propostaRequest.Obra.Numero,
                                            AnoChamada = propostaRequest.Ano,
                                            NumChamada = propostaRequest.Numero,
                                            DataHora = DateTime.Now,
                                            Usuario = usuario,
                                            Evento = "Data de encerramento alterada",
                                            Complemento = $"Data de encerramento alterada de {contrato.DataEncerramento} para {propostaRequest.Obra.Contrato?.DataEncerramento}",
                                            Observacao = "",
                                            Sequencia = sequenciaObraLog++
                                        });
                                    }
                                    else
                                    {
                                        _obraService.Adicionar(new ObraLogVersao
                                        {
                                            NumeroVersao = numVersao,
                                            UsinaCodigo = propostaRequest.Usina.Codigo,
                                            ObraCodigo = propostaRequest.Obra.Numero,
                                            AnoChamada = propostaRequest.Ano,
                                            NumChamada = propostaRequest.Numero,
                                            DataHora = DateTime.Now,
                                            Usuario = usuario,
                                            Evento = "CONTRATO ABERTO",
                                            Complemento = "Status contrato: Aberto",
                                            Observacao = "Contrato aberto via TopconCRM",
                                            Sequencia = sequenciaObraLog++
                                        });
                                    }
                                }
                                else
                                {
                                    _obraService.Adicionar(new ObraLogVersao
                                    {
                                        NumeroVersao = numVersao,
                                        UsinaCodigo = propostaRequest.Usina.Codigo,
                                        ObraCodigo = propostaRequest.Obra.Numero,
                                        AnoChamada = propostaRequest.Ano,
                                        NumChamada = propostaRequest.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "ENCERRADO",
                                        Complemento = "Status contrato: Encerrado",
                                        Observacao = "Contrato encerrado via TopconCRM",
                                        Sequencia = sequenciaObraLog++
                                    });
                                }

                                contrato.DataEncerramento = propostaRequest.Obra.Contrato?.DataEncerramento;
                                Commit();
                            }

                            if(houveMudancaSegmentacaoProposta)
                            {
                                contrato.Segmentacao = propostaRequest.Segmentacao;
                                Commit();
                            }

                            if(houveMudancaFinalidadeProposta)
                            {
                                contrato.ContratoFinalidade = propostaRequest.ContratoFinalidade;
                                Commit();
                            }

                            if ((contrato.IntervenienteCodigo ?? 0) == 0)
                            {
                                contrato.IntervenienteCodigo = propostaOld.IntervenienteCodigo;
                                Commit();
                            }

                            //if (!propostaOld.PropostaContratoIntervenienteIsValid(contrato))return;

                            // Dados Faturamento
                            if (propostaOld.UtilizaDadosFaturamento || propostaOld.UtilizaEnderecoFaturamento)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.Faturamento, out int sequenciaLocal, usuario, t => t.LocalFaturamentoSimNao == "S");
                                if (contrato.LocalFaturamento != sequenciaLocal)
                                {
                                    contrato.LocalFaturamento = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.LocalFaturamento > 0)
                            {
                                contrato.LocalFaturamento = 0;
                                Commit();
                            }

                            // Dados Cobrança
                            if (propostaOld.UtilizaDadosCobranca || propostaOld.UtilizaEnderecoCobranca)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.Cobranca, out int sequenciaLocal, usuario, t => t.LocalCobrancaSimNao == "S");
                                if (contrato.LocalCobranca != sequenciaLocal)
                                {
                                    contrato.LocalCobranca = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.LocalCobranca > 0)
                            {
                                contrato.LocalCobranca = 0;
                                Commit();
                            }

                            // Dados Responsável Solidário
                            if (propostaOld.UtilizaResponsavelSolidario)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.ResponsavelSolidario, out int sequenciaLocal, usuario);
                                if (contrato.ResponsavelSolidario != sequenciaLocal)
                                {
                                    contrato.ResponsavelSolidario = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.ResponsavelSolidario > 0)
                            {
                                contrato.ResponsavelSolidario = 0;
                                Commit();
                            }

                            contrato.ValorConcreto = propostaOld.ValorConcreto;
                            contrato.ValorBomba = propostaOld.ValorBomba;
                            contrato.ValorExtras = propostaOld.ValorExtras;
                            contrato.ValorTotalContrato = propostaOld.ValorTotalContrato;
                            contrato.VolumeTotal = propostaOld.VolumeTotal;
                            contrato.Observacao = propostaOld.Observacao;
                            contrato.VendedorCodigo = propostaOld.VendedorCodigo;
                            contrato.CodigoObraPrefeitura = propostaOld.CodigoObraPrefeitura;
                            contrato.UsinaEntrega = propostaOld.Obra.UsinaEntregaCodigo;
                            contrato.ModeloDocumentoRemessaConcreto = propostaOld.ModeloDocumentoRemessaConcreto;
                            contrato.ModeloDocumentoRemessaBomba = propostaOld.ModeloDocumentoRemessaBomba;

                            contrato.FimVigencia = propostaRequest.Obra.Contrato.FimVigencia;

                            contrato.AprovacaoMedicao = propostaRequest.AprovacaoMedicao;
                            contrato.TempoAprovacaoMedicao = propostaRequest.TempoAprovacaoMedicao;

                            var contratoStatusAnterior = contrato.Status;
                            var observacao = "";

                            if (houveAlteracaoDadosProposta)
                            {
                                switch (contrato.Status)
                                {
                                    case EContratoStatus.Reprovado:
                                    case EContratoStatus.Pendente:
                                    case EContratoStatus.Cancelado:
                                        contrato.StatusAnterior = contrato.Status;
                                        contrato.Status = EContratoStatus.EmAnalise;
                                        observacao = "Alteração de dados na proposta";
                                        break;
                                }

                                if (obraOld.CondicaoPagamentoCodigo != condicaoPagamentoAnterior || obraOld.TipoCobrancaCodigo != tipoCobrancaAnterior)
                                {
                                    switch (contrato.Status)
                                    {
                                        case EContratoStatus.AguardandoConfirmacaoPagamento:
                                        case EContratoStatus.AguardandoDadosPagamento:
                                            contrato.Status = EContratoStatus.EmAnalise;
                                            break;
                                        case EContratoStatus.Aprovado:
                                            contrato.Status = parametroDesativarObrigatoriedadeAprovacaoCadastro ? EContratoStatus.Aprovado : EContratoStatus.RevalidacaoCadastro;
                                            break;
                                    }
                                }
                            }

                            if (contrato.Status == EContratoStatus.AguardandoDadosPagamento && (incluiuPagamentoDetalhe || alterouPagamentoDetalhe))
                            {
                                contrato.Status = EContratoStatus.AguardandoConfirmacaoPagamento;
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && propostaOld.ValorTotalContrato != valorTotalAnterior && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Valor total contrato foi alterado";
                            }

                            if (contrato.Fechado && propostaOld.Obra.Cei != ceiObraAnterior && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Valor de CEI/CNO da Obra foi alterado";
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && (propostaOld.Obra.EnderecoLogradouro != logradouroObraAnterior
                                || propostaOld.Obra.EnderecoNumero != numeroObraAnterior
                                || propostaOld.Obra.EnderecoCep != cepObraAnterior
                                || propostaOld.Obra.EnderecoComplemento != complementoObraAnterior
                                || propostaOld.Obra.EnderecoBairro != bairroObraAnterior
                                || propostaOld.Obra.EnderecoMunicipioCodigo != municipioObraAnterior)
                                && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Endereço da Obra foi alterado";
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && propostaOld.Status == EPropostaStatus.AguardandoAprovacaoComercial && propostaOld.Status != propostaStatusAnterior)
                            {
                                contrato.StatusAnterior = contrato.Status;
                                contrato.Status = EContratoStatus.AguardandoAprovacaoComercial;
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                            }

                            if (contrato.DataEncerramento != null && contrato.Status != EContratoStatus.Encerrado)
                            {
                                if(contrato.Status != EContratoStatus.Encerrado)
                                    contrato.StatusAnterior = contrato.Status;

                                contrato.Status = EContratoStatus.Encerrado;
                            }

                            if (contrato.DataEncerramento == null && contrato.Status == EContratoStatus.Encerrado)
                            {

                                var novoStatus = contrato.StatusAnterior;

                                if (novoStatus == EContratoStatus.Inexistente || novoStatus == EContratoStatus.NaoGerado)
                                    novoStatus = EContratoStatus.EmAnalise;

                                contrato.Status = novoStatus;
                                contrato.StatusAnterior = EContratoStatus.Encerrado;
                            }

                            Commit();

                            if (contrato.Status != contratoStatusAnterior)
                            {
                                _obraService.Adicionar(new ObraLogVersao
                                {
                                    NumeroVersao = numVersao,
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO STATUS CONTRATO",
                                    Complemento = $"De: {(int)contratoStatusAnterior} - {contratoStatusAnterior} Para: {(int)contrato.Status} - {contrato.Status }",
                                    Observacao = observacao,
                                    Sequencia = sequenciaObraLog++
                                });

                                Commit();
                            }
                        }
                    }

                    _propostaService.ValidarDemaisAprovacoes(usuario, propostaOld, cpfCnpjAnterior, propostaStatusAnterior);

                    _obraService.AtualizarStatusComercial(propostaOld.UsinaCodigo, propostaOld.Obra.Numero, numVersao);

                    var statusFinanceiroAnterior = obraOld.StatusFinanceiro;

                    _obraService.AtualizarStatusFinanceiro(obraOld, usuario);

                    _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceiraVersao(propostaOld.Obra.Numero, propostaOld.Obra.UsinaCodigo, numVersao, statusFinanceiroAnterior);

                    _propostaService.AtualizarStatusProposta(propostaOld, usuario);

                    _obraService.AtualizarEnderecoProgramacoesFuturas(propostaOld.UsinaCodigo, propostaOld.Obra.Numero); //*** verificar ***

                    _obraService.AtualizarValoresProgramacoesFuturas(propostaOld.UsinaCodigo, propostaOld.Obra.Numero); //*** verificar ***                      

                    if (obraOld.Contrato.Status == EContratoStatus.Aprovado)
                    {
                        _obraApplicationService.AtualizarContratoComVersao(numVersao, obraOld.UsinaCodigo, obraOld.AnoChamada ?? 0, obraOld.NumChamada ?? 0, propostaRequest.Obra.ObraTracos ?? null);

                        if (propostaRequest.Obra.ObraTracos != null)
                        {
                            var obraTracos = AutoMapper.Mapper.Map(propostaRequest.Obra.ObraTracos, new List<ObraTraco>());
                            _obraApplicationService.AtualizarValorReajustePropostaItemVersao(numVersao, obraOld.UsinaCodigo, obraOld.AnoChamada ?? 0, obraOld.NumChamada ?? 0, obraTracos);
                        }

                        if (propostaRequest.Obra.ObraBombas != null)
                        {
                            var obraBombas = AutoMapper.Mapper.Map(propostaRequest.Obra.ObraBombas, new List<ObraBomba>());
                            _obraApplicationService.AtualizarValorReajustePropostaBombaVersao(numVersao, obraOld.UsinaCodigo, obraOld.AnoChamada ?? 0, obraOld.NumChamada ?? 0, obraBombas);
                        }
                    }
                }
            }
        }


        public void AtualizarContrato(string usuario, PropostaAlteracaoRequest propostaRequest, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            var propostaOld = _propostaService.ObterPorId(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero);
            var obraOld = _obraService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            var obraIndicador = _obraService.ObterPorId<ObraIndicador>( propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            var utilizaAprovacaoComercicalPorAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(obraOld.UsinaEntregaCodigo);
            var parametroDesativarObrigatoriedadeAprovacaoCadastro = _parametroService.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro") == "1";
            var parametroValidaAprovacaoComparandoComPrecoTabela = _parametroService.ObterParametroN("web", "ValidaAprovacaoComparandoComPrecoTabela") == "1";

            int sequenciaObraLog = 1;
            int sequenciaProgramacaoLog = 1;

            var cpfCnpjAnterior = propostaOld.CpfCnpj;
            var propostaStatusAnterior = propostaOld.Status;
            var condicaoPagamentoAnterior = propostaOld.Obra.CondicaoPagamentoCodigo ?? 0;
            var condicaoPagamentoAnteriorDescricao = _obraService.ObterPorId<CondicaoPagamento>(condicaoPagamentoAnterior)?.Descricao ?? "";
            var tipoCobrancaAnterior = propostaOld.Obra.TipoCobrancaCodigo ?? 0;
            var tipoCobrancaAnteriorDescricao = _obraService.ObterPorId<TipoCobranca>(tipoCobrancaAnterior)?.Descricao ?? "";
            var valorTotalAnterior = propostaOld.ValorTotalContrato;
            var modeloDocumentoRemessaConcreto = propostaOld.ModeloDocumentoRemessaConcreto;
            var modeloDocumentoRemessaBomba = propostaOld.ModeloDocumentoRemessaBomba;
            var segmentacaoProposta = propostaOld.Segmentacao;
            var finalidadeProposta = propostaOld.ContratoFinalidade;
            var ceiObraAnterior = obraOld.Cei;
            var tributacaoPisCofinsAnterior = obraOld.TributacaoPisCofinsCodigo;

            var tributacaoISAnterior = obraOld.TributacaoISCodigo;
            var tributacaoIBSAnterior = obraOld.TributacaoIBSCodigo;
            var tributacaoCBSAnterior = obraOld.TributacaoCBSCodigo;

            var codigoCibAnterior = obraOld.CodigoCib;
            var construcaoCivilTipoAlvaraAnterior = obraOld.ConstrucaoCivilTipoAlvara;

            var logradouroObraAnterior = obraOld.EnderecoLogradouro;
            var numeroObraAnterior = obraOld.EnderecoNumero;
            var cepObraAnterior = obraOld.EnderecoCep;
            var complementoObraAnterior = obraOld.EnderecoComplemento;
            var bairroObraAnterior = obraOld.EnderecoBairro;
            var municipioObraAnterior = obraOld.EnderecoMunicipioCodigo;
            var usaAdicionalZMRCAnterior = obraOld.UsaAdicionalZMRC;
            var viaCaptacaoAnterior = obraOld.ViaCaptacaoCodigo;

            var aprovacaoMedicaoAnterior = propostaOld.AprovacaoMedicao;
            var tempoAprovacaoMedicaoAnterior = propostaOld.TempoAprovacaoMedicao;

            var volumeTotalObraOld = _propostaService.ListarFiltrados<ObraTraco>(x => x.UsinaCodigo == obraOld.UsinaCodigo && x.ObraCodigo == obraOld.Numero).Sum(x => x.M3Quantidade);
            var volumeTotalObraNew = propostaRequest.Obra.ObraTracos.Sum(x => x.M3Quantidade);

            var vendedorOld = _vendedorService.ObterPorId<Vendedor>(propostaOld.VendedorCodigo);

            foreach (var traco in propostaRequest.Obra.ObraTracos)
            {
                if (traco.PrecoReajustadoAtual == 0)
                {
                    traco.PrecoReajustadoAtual = _obraService.ObterPorId<ObraTraco>(traco.Usina.Codigo, traco.ObraCodigo, traco.Sequencia)?.PrecoReajustadoAtual ?? 0;
                }
            }

            propostaOld = AutoMapper.Mapper.Map(propostaRequest, propostaOld);

            propostaOld.Vendedor = _vendedorService.ObterPorId<Vendedor>(propostaOld.VendedorCodigo);

            var houveMudancaSegmentacaoProposta = segmentacaoProposta != propostaRequest.Segmentacao;
            var houveMudancaFinalidadeProposta = finalidadeProposta != propostaRequest.ContratoFinalidade;

            propostaOld.ModeloItensDanfeERomaneio = propostaRequest.ModeloItensDanfeERomaneio;

            var cobrancaOld = _propostaService.ObterPorId<PropostaCobranca>(propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.Cobranca = AutoMapper.Mapper.Map(propostaRequest.Cobranca, cobrancaOld ?? new PropostaCobranca());

            var faturamentoOld = _propostaService.ObterPorId<PropostaFaturamento>(propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.Faturamento = AutoMapper.Mapper.Map(propostaRequest.Faturamento, faturamentoOld ?? new PropostaFaturamento());

            var responsavelSolidarioOld = _propostaService.ObterPorId<PropostaResponsavelSolidario>(propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero);
            propostaOld.ResponsavelSolidario = AutoMapper.Mapper.Map(propostaRequest.ResponsavelSolidario, responsavelSolidarioOld ?? new PropostaResponsavelSolidario());

            if (propostaRequest.UtilizaResponsavelSolidario && !propostaOld.PropostaResponsavelSolidarioIsValid(propostaOld.ResponsavelSolidario))
                return;

            if (!propostaOld.PropostaCobrancaIsValid(propostaOld.Cobranca, propostaRequest.UtilizaDadosCobranca, propostaRequest.UtilizaEnderecoCobranca))
                return;

            if (!propostaOld.PropostaFaturamentoIsValid(propostaOld.Faturamento, propostaRequest.UtilizaDadosFaturamento, propostaRequest.UtilizaEnderecoFaturamento))
                return;

            var pagamentos = AutoMapper.Mapper.Map(propostaRequest.Obra.ObraPagamentos, new List<PropostaPagamento>());

            if (propostaRequest.StatusProposta == EPropostaStatusCliente.Aprovado)
            {
                if (!pagamentos.PagamentosAtivosIsValid())
                    return;
            }


            if (propostaOld.UtilizaDadosFaturamento && !propostaOld.UtilizaEnderecoFaturamento)
            {
                propostaOld.Faturamento.EnderecoBairro = propostaOld.EnderecoBairro;
                propostaOld.Faturamento.EnderecoCep = propostaOld.EnderecoCep;
                propostaOld.Faturamento.EnderecoComplemento = propostaOld.EnderecoComplemento;
                propostaOld.Faturamento.EnderecoLogradouro = propostaOld.EnderecoLogradouro;
                propostaOld.Faturamento.EnderecoMunicipioCodigo = propostaOld.EnderecoMunicipioCodigo;
                //propostaOld.Faturamento.EnderecoMunicipio = propostaOld.EnderecoMunicipio;
                propostaOld.Faturamento.EnderecoNumero = propostaOld.EnderecoNumero;
                Commit();
            }
            else if (!propostaOld.UtilizaDadosFaturamento && propostaOld.UtilizaEnderecoFaturamento)
            {
                propostaOld.Faturamento.CpfCnpj = propostaOld.CpfCnpj;
                propostaOld.Faturamento.InscricaoEstadual = propostaOld.InscricaoEstadual;
                propostaOld.Faturamento.InscricaoMunicipal = propostaOld.InscricaoMunicipal;
                propostaOld.Faturamento.Nome = propostaOld.IntervenienteNome;
                propostaOld.Faturamento.OrgaoExpedidor = propostaOld.OrgaoExpedidor;
                propostaOld.Faturamento.Razao = propostaOld.IntervenienteRazao;
                propostaOld.Faturamento.Rg = propostaOld.Rg;
                Commit();
            }

            if (propostaOld.UtilizaDadosCobranca && !propostaOld.UtilizaEnderecoCobranca)
            {
                propostaOld.Cobranca.EnderecoBairro = propostaOld.EnderecoBairro;
                propostaOld.Cobranca.EnderecoCep = propostaOld.EnderecoCep;
                propostaOld.Cobranca.EnderecoComplemento = propostaOld.EnderecoComplemento;
                propostaOld.Cobranca.EnderecoLogradouro = propostaOld.EnderecoLogradouro;
                propostaOld.Cobranca.EnderecoMunicipioCodigo = propostaOld.EnderecoMunicipioCodigo;
                //propostaOld.Cobranca.EnderecoMunicipio = propostaOld.EnderecoMunicipio;
                propostaOld.Cobranca.EnderecoNumero = propostaOld.EnderecoNumero;
                Commit();
            }
            else if (!propostaOld.UtilizaDadosCobranca && propostaOld.UtilizaEnderecoCobranca)
            {
                propostaOld.Cobranca.CpfCnpj = propostaOld.CpfCnpj;
                propostaOld.Cobranca.InscricaoEstadual = propostaOld.InscricaoEstadual;
                propostaOld.Cobranca.InscricaoMunicipal = propostaOld.InscricaoMunicipal;
                propostaOld.Cobranca.Nome = propostaOld.IntervenienteNome;
                propostaOld.Cobranca.OrgaoExpedidor = propostaOld.OrgaoExpedidor;
                propostaOld.Cobranca.Razao = propostaOld.IntervenienteRazao;
                propostaOld.Cobranca.Rg = propostaOld.Rg;
                Commit();
            }

            if (viaCaptacaoAnterior != propostaRequest.Obra.ViaCaptacao.Codigo)
            {

                var viaCaptacaoOld = _obraService.ObterPorId<CadastroGeral>(viaCaptacaoAnterior);

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO VIA DE CAPTAÇÃO",
                    Complemento = "",
                    Observacao = $"Alteração de via de captação de {(viaCaptacaoOld == null ? viaCaptacaoAnterior.ToString() : viaCaptacaoOld.Descricao)} para {propostaRequest.Obra.ViaCaptacao.Descricao}",
                    Sequencia = sequenciaObraLog++
                });

            }

            if (propostaRequest.Obra.Indicador != null)
            {

                var newIndicador = propostaRequest.Obra.Indicador;
                var mensagem = "";
                var indicadorNewDescricao = "";

                if (newIndicador.IntervenienteCodigo != 0)
                    indicadorNewDescricao = $"Cliente {newIndicador.IntervenienteCodigo} - {_intervenienteService.ObterPorId(newIndicador.IntervenienteCodigo)?.Nome ?? ""}";

                if (newIndicador.VendedorCodigo != 0)
                    indicadorNewDescricao = $"Vendedor {newIndicador.VendedorCodigo} - {_vendedorService.ObterPorId(newIndicador.VendedorCodigo)?.Nome ?? ""}";

                if (obraIndicador != null)
                {
                    var vendedorIgual = newIndicador.VendedorCodigo == obraIndicador.VendedorCodigo;
                    var intervenienteIgual = newIndicador.IntervenienteCodigo == obraIndicador.IntervenienteCodigo;

                    if (!vendedorIgual || !intervenienteIgual)
                    {

                        var indicadorOldDescricao = "";

                        if (obraIndicador.IntervenienteCodigo != 0)
                            indicadorOldDescricao = $"Cliente {obraIndicador.IntervenienteCodigo} - {_intervenienteService.ObterPorId(obraIndicador.IntervenienteCodigo)?.Nome ?? ""}";

                        if (obraIndicador.VendedorCodigo != 0)
                            indicadorOldDescricao = $"Vendedor {obraIndicador.VendedorCodigo} - {_vendedorService.ObterPorId(obraIndicador.VendedorCodigo)?.Nome ?? ""}";

                        mensagem = $"Indicador alterado de {indicadorOldDescricao} para {indicadorNewDescricao}";

                    }

                }

                if (!string.IsNullOrEmpty(mensagem))
                {
                    _obraService.Adicionar(new ObraLog
                    {
                        UsinaCodigo = propostaOld.UsinaCodigo,
                        ObraCodigo = propostaOld.Obra.Numero,
                        AnoChamada = propostaOld.Ano,
                        NumChamada = propostaOld.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "ALTERAÇÃO INDICADOR CAPTAÇÃO",
                        Complemento = "",
                        Observacao = mensagem,
                        Sequencia = sequenciaObraLog++
                    });
                }

            }

            if (modeloDocumentoRemessaConcreto != propostaRequest.ModeloDocumentoRemessaConcreto)
            {
                propostaOld.ModeloDocumentoRemessaConcreto = propostaRequest.ModeloDocumentoRemessaConcreto;

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var descricaoAlteracaoModelo = "";

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(propostaRequest.ModeloDocumentoRemessaConcreto.ToString())).FirstOrDefault();

                if (modeloOld != null && modeloNew != null)
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloOld.Descricao.ToUpper()} Para: {modeloNew.Descricao.ToUpper()}";
                }
                else
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloDocumentoRemessaConcreto} Para: {propostaRequest.ModeloDocumentoRemessaConcreto}";
                }

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricaoAlteracaoModelo,
                    Observacao = "Alteração de modelo de documento para remessa de concreto",
                    Sequencia = sequenciaObraLog++
                }); 

                Commit();
            }

            if(modeloDocumentoRemessaBomba != propostaRequest.ModeloDocumentoRemessaBomba)
            {
                    
                propostaOld.ModeloDocumentoRemessaBomba = propostaRequest.ModeloDocumentoRemessaBomba;

                var modelos = _cadastroDiversoService.ListarModeloDocumentoRemessaConcreto();

                var descricaoAlteracaoModelo = "";

                var modeloOld = modelos.Where(x => x.Codigo.Equals(modeloDocumentoRemessaBomba.ToString())).FirstOrDefault();
                var modeloNew = modelos.Where(x => x.Codigo.Equals(propostaRequest.ModeloDocumentoRemessaBomba.ToString())).FirstOrDefault();

                if (modeloOld != null && modeloNew != null)
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloOld.Descricao.ToUpper()} Para: {modeloNew.Descricao.ToUpper()}";
                }
                else
                {
                    descricaoAlteracaoModelo = descricaoAlteracaoModelo + $"De: {modeloDocumentoRemessaBomba} Para: {propostaRequest.ModeloDocumentoRemessaBomba}";
                }
                        
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO MODELO DE DOCUMENTO",
                    Complemento = descricaoAlteracaoModelo,
                    Observacao = "Alteração de modelo de documento para remessa de bomba",
                    Sequencia = sequenciaObraLog++
                });

                Commit();
            }

            // log para alteração de CEI/CNO da Obra
            if (propostaRequest.Obra.Cei != ceiObraAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO CEI/CNO DA OBRA",
                    Complemento = $"Valor Alterado De: {ceiObraAnterior} Para: {propostaRequest.Obra.Cei}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }
            
            // log para alteração de  tributação pis/cofins na Obra
            if ((propostaRequest.Obra.TributacaoPisCofins?.Codigo ?? "") != tributacaoPisCofinsAnterior)
                {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIBUTAÇÃO PIS/COFINS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoPisCofinsAnterior} Para: {propostaRequest.Obra.TributacaoPisCofins?.Codigo ?? ""}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoIS?.Id ?? 0) != tributacaoISAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. IS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoISAnterior} Para: {propostaRequest.Obra.TributacaoIS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoIBS?.Id ?? 0) != tributacaoIBSAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. IBS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoIBSAnterior} Para: {propostaRequest.Obra.TributacaoIBS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            if ((propostaRequest.Obra.TributacaoCBS?.Id ?? 0) != tributacaoCBSAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO TRIB. CBS DA OBRA",
                    Complemento = $"Valor Alterado De: {tributacaoCBSAnterior} Para: {propostaRequest.Obra.TributacaoCBS?.Id ?? 0}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // log para alteração do código CIB da Obra
            if (propostaRequest.Obra.CodigoCib != codigoCibAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO CÓDIGO CIB DA OBRA",
                    Complemento = $"Valor Alterado De: {codigoCibAnterior} Para: {propostaRequest.Obra.CodigoCib}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // log para alteração do campo Construção Civil Tipo Alvará da Obra
            if (propostaRequest.Obra.ConstrucaoCivilTipoAlvara != construcaoCivilTipoAlvaraAnterior)
            {
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO C. CIVIL TIPO ALVARÁ",
                    Complemento = $"Valor Alterado De: {construcaoCivilTipoAlvaraAnterior} Para: {propostaRequest.Obra.ConstrucaoCivilTipoAlvara}",
                    Observacao = "",
                    Sequencia = sequenciaObraLog++
                });
            }

            // correção para acesso concorrente onde o cliente era cadastrado no
            // desktop enquanto a proposta estava aberta no web
            if (cpfCnpjAnterior != propostaOld.CpfCnpj || (propostaOld.IntervenienteCodigo == 0 && propostaRequest.IntervenienteCodigo != 0))
            {
                propostaOld.IntervenienteCodigo = propostaRequest.IntervenienteCodigo;
                Commit();
            }

            // Correção proposta em negociação sem CPF_CNPJ
            if (propostaRequest.StatusProposta == EPropostaStatusCliente.EmNegociacao && propostaOld.CpfCnpj.Length < 11)
            {
                propostaRequest.IntervenienteCodigo = 0;
                propostaOld.CpfCnpj = "";
                propostaOld.IntervenienteCodigo = 0;
                Commit();
            }

            // atualizando os dados do interveniente
            if (propostaOld.IntervenienteCodigo != 0 && propostaRequest.IntervenienteCodigo != 0)
            {
                var intervenienteOld = _intervenienteService.ObterPorId(propostaOld.IntervenienteCodigo);

                if (propostaRequest.Email.Equals(""))
                {
                    if (!intervenienteOld.Email.Equals(""))
                    {
                        var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(DateTime.Now);

                        if (propostaRequest.Interveniente.IntervenienteTipo != "F" || parametroProposta.ObrigaEmailPessoaFisica)
                            propostaRequest.Email = intervenienteOld.Email;
                    }
                }

                var houveAlteracaoInterv = HouveAlteracaoInterveniente(intervenienteOld, propostaRequest);

                AutoMapper.Mapper.Map(propostaRequest, intervenienteOld);
                intervenienteOld.IdAtualizacao = StringHelper.GetIDD(usuario);

                if(houveAlteracaoInterv)
                    _webHookApplicationService.AdicionarWebHookInterveniente(intervenienteOld, EWebHookTipoEvento.Update);

                Commit();
            }

            // Tratamento para quando o interveniente tem vendedor exclusivo diferente do da proposta:
            // o cod do cliente vem zerado do frontend quando o vendedor não tem direito no grupo do vendedor exclusivo
            if ((propostaOld.IntervenienteCodigo ?? 0) == 0)
            {
                var interveniente = _intervenienteService.ObterPorCpfCnpj(propostaOld.CpfCnpj, propostaOld.InscricaoEstadual);

                if (interveniente != null)
                {
                    propostaOld.IntervenienteCodigo = interveniente.Codigo;
                    Commit();
                }
            }

            propostaOld.IdAtualizacao = StringHelper.GetIDD(usuario);
            Commit();

            obraOld.Indicador = _obraService.ObterPorId<ObraIndicador>(obraOld.UsinaCodigo, obraOld.Numero);
            obraOld = AutoMapper.Mapper.Map(propostaRequest.Obra, obraOld);
            obraOld.IdAtualizacao = StringHelper.GetIDD(usuario);

            obraOld.TributacaoCBS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoCBS, obraOld.TributacaoCBS);
            obraOld.TributacaoCBSCodigo = obraOld.TributacaoCBS != null ? obraOld.TributacaoCBS.Id : 0;

            obraOld.TributacaoIBS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoIBS, obraOld.TributacaoIBS);
            obraOld.TributacaoIBSCodigo = obraOld.TributacaoIBS != null ? obraOld.TributacaoIBS.Id : 0;

            obraOld.TributacaoIS = AutoMapper.Mapper.Map(propostaRequest.Obra.TributacaoIS, obraOld.TributacaoIS);
            obraOld.TributacaoISCodigo = obraOld.TributacaoIS != null ? obraOld.TributacaoIS.Id : 0;

            if (obraOld.Indicador != null)
            {
                obraOld.Indicador.ObraNumero = obraOld.Numero;
                obraOld.Indicador.ObraUsina = obraOld.UsinaCodigo;
            }

            Commit();

            _obraService.AlterarMensagemObraReajuste(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero, propostaRequest.Obra.ObraReajuste.MensagemReajuste);

            // ObraFrentes
            List<int> sequenciasFrentes = new List<int>();
            foreach (var fDto in propostaRequest.Obra.ObraFrentes)
            {
                var frenteOld = _obraService.ListarFiltradosTracking<ObraFrente>(t => t.UsinaCodigo == fDto.UsinaCodigo
                                                                                        && t.ObraCodigo == fDto.ObraCodigo
                                                                                        && t.ObraSequencia == fDto.ObraSequencia).FirstOrDefault();
                var novoRegistro = (frenteOld == null);

                if (novoRegistro)
                {
                    var frenteNew = AutoMapper.Mapper.Map(fDto, new ObraFrente());
                    frenteNew.ID = Guid.NewGuid();
                    frenteNew.UsinaCodigo = obraOld.UsinaCodigo;
                    frenteNew.ObraCodigo = obraOld.Numero;

                    if (frenteNew.ObraSequencia == 0)
                        frenteNew.ObraSequencia = _obraFrenteService.ProximaSequenciaNaObra(frenteNew.UsinaCodigo, frenteNew.ObraCodigo);

                    sequenciasFrentes.Add(frenteNew.ObraSequencia);

                    _obraService.Adicionar(frenteNew);

                    Commit();

                }
                else
                {

                    sequenciasFrentes.Add(fDto.ObraSequencia);

                    frenteOld = AutoMapper.Mapper.Map(fDto, frenteOld);

                    // Buscando programações
                    var programacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraFrenteSequencia == frenteOld.ObraSequencia);

                    foreach (var programacao in programacoes)
                    {
                        programacao.EnderecoBairro = frenteOld.EnderecoBairro;
                        programacao.EnderecoCep = frenteOld.EnderecoCep;
                        programacao.EnderecoComplemento = frenteOld.EnderecoComplemento;
                        programacao.EnderecoLogradouro = frenteOld.EnderecoLogradouro;
                        programacao.EnderecoNumero = frenteOld.EnderecoNumero;
                    }

                    Commit();

                }

            }

            var seqsObraFrente = sequenciasFrentes.ToArray();
            var frentesExcluidas = _obraService.ListarFiltradosTracking<ObraFrente>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqsObraFrente.Contains(t.ObraSequencia));

            foreach (var f in frentesExcluidas)
            {

                var temProgramacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraFrenteSequencia == f.ObraSequencia).Count() > 0;

                if(temProgramacoes)
                {
                    AssertionConcern.Notify("obraFrente", $"Não é possível excluir a Frente de obra de sequência ({f.ObraSequencia}).");
                    continue;
                }

                _obraService.Remover(f);

                Commit();
            }



            // ObraTracos
            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraTracos)
            {
                sequencias.Add(tDto.Sequencia);
                var tracoOld = _obraService.ListarFiltradosTracking<ObraTraco>(t => t.UsinaCodigo == tDto.Usina.Codigo && t.ObraCodigo == tDto.ObraCodigo && t.Sequencia == tDto.Sequencia,
                    t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                    .FirstOrDefault();

                if (tracoOld != null)
                {

                    var mesmoTraco = true;

                    mesmoTraco = mesmoTraco && (tDto.Uso.Codigo == tracoOld.UsoCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Pedra.Codigo == tracoOld.PedraCodigo);
                    mesmoTraco = mesmoTraco && (tDto.SlumpNominal.Codigo == tracoOld.SlumpNominalCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Slump.Codigo == tracoOld.SlumpCodigo);
                    mesmoTraco = mesmoTraco && (tDto.ResistenciaTipo.Codigo == tracoOld.ResistenciaTipoCodigo);
                    mesmoTraco = mesmoTraco && (tDto.Mpa == tracoOld.Fck);
                    mesmoTraco = mesmoTraco && (tDto.Consumo == tracoOld.Consumo);

                    var mercadoria = _mercadoriaService.ObterTracoMercadoria(tDto.Uso.Codigo, tDto.Pedra.Codigo, tDto.Slump.Codigo, tDto.ResistenciaTipo.Codigo, tDto.Mpa, tDto.Consumo);
                    var descricaoTracoNovo = mercadoria is null ? "" : mercadoria.Descricao;
                    var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObra(AutoMapper.Mapper.Map(tDto, new ObraTraco()), obraOld) == 7105;

                    if (tDto.M3Quantidade != tracoOld.M3Quantidade && mesmoTraco)
                    {

                        _obraService.Adicionar(new ObraLog()
                        {
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO VOLUME DO TRAÇO",
                            Complemento = $"Alteração do volume do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De {tracoOld.Descricao} {tracoOld.M3Quantidade.ToString("F1")} M3" +
                            $" \nPara: {tracoOld.Descricao} {tDto.M3Quantidade.ToString("F1")} M3",
                            Sequencia = sequenciaObraLog++
                        });

                    }

                    if (tDto.Ativo != tracoOld.Ativo && mesmoTraco)
                    {
                        _obraService.Adicionar(new ObraLog()
                        {
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = tDto.Ativo == "S" ? "ALTERAÇÃO TRAÇO PARA ATIVO" : "ALTERAÇÃO TRAÇO PARA INATIVO",
                            Complemento = tDto.Ativo == "S" ? $"Traço alterado de inativo para ativo. (Sequência {tDto.Sequencia})" : $"Traço alterado de ativo para inativo. (Sequência {tDto.Sequencia})",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (tDto.M3PrecoProposto != tracoOld.M3PrecoProposto)
                    {

                        _obraService.Adicionar(new ObraLog()
                        {
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO VALOR DO TRAÇO",
                            Complemento = $"Alteração do valor do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De {tracoOld.Descricao} R$ {tracoOld.M3PrecoProposto.ToString("F2")}" +
                            $" \nPara: {tracoOld.Descricao} R$ {tDto.M3PrecoProposto.ToString("F2")}",
                            Sequencia = sequenciaObraLog++
                        });

                    }

                    if (tDto.PrecoReajustadoAtual != tracoOld.PrecoReajustadoAtual)
                    {
                        var ultimoReajusteTraco = _contratoService.ListarFiltrados<ContratoTracoReajuste>(t => t.UsinaCodigo == propostaRequest.Obra.UsinaCodigo 
                            && t.ContratoAno == propostaRequest.Obra.AnoContrato && t.ContratoNumero == propostaRequest.Obra.NumContrato && t.ObraTracoSequencia == tDto.Sequencia)
                            .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO REAJUSTE TRAÇO",
                            Complemento = $"Alteração do preço reajustado do traço sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De: {tracoOld.PrecoReajustadoAtual} \nPara: {tDto.PrecoReajustadoAtual} / Preço Ultimo Reajuste: {ultimoReajusteTraco.PrecoRecalculado}",
                            Sequencia = sequenciaObraLog++
                        });

                        var tracoCusto = _tracoCustoService
                            .ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(propostaRequest.Obra.UsinaEntrega.Codigo, tracoOld.UsoCodigo,
                            tracoOld.PedraCodigo, tracoOld.SlumpCodigo, tracoOld.ResistenciaTipoCodigo, tracoOld.Fck, tracoOld.Consumo, propostaRequest.Data);

                        if (tracoCusto == null && propostaRequest.Data < DateTime.Today)
                        {
                            tracoCusto = _tracoCustoService
                                .ObterPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumoDataBase(propostaRequest.Obra.UsinaEntrega.Codigo, tracoOld.UsoCodigo,
                                tracoOld.PedraCodigo, tracoOld.SlumpCodigo, tracoOld.ResistenciaTipoCodigo, tracoOld.Fck, tracoOld.Consumo, DateTime.Today);
                        }

                        tracoOld.CustoServicoReajustado = tDto.PrecoReajustadoAtual - tracoCusto.CustoAjustado;

                    }

                    // Buscando programações
                    var programacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraTracoSequencia == tracoOld.Sequencia
                            && t.Status != EProgramacaoStatus.Cancelada,
                            t => t.ResistenciaTipo, t => t.Pedra, t => t.Slump, t => t.Uso);

                    // programações
                    foreach (var prog in programacoes)
                    {
                        if (AutoMapper.Mapper.Map(tDto, new ObraTraco()).TracoProgramacaoScopeIsValid(prog))
                        {
                            if (tracoOld.M3PrecoProposto != tDto.M3PrecoProposto)
                            {
                                _programacaoService.Adicionar(new ProgramacaoLog()
                                {
                                    UsinaCodigo = prog.UsinaCodigo,
                                    ObraCodigo = prog.ObraNumero,
                                    ProgramacaoSequencia = prog.Sequencia,
                                    PropostaAno = prog.PropostaAno,
                                    PropostaNumero = prog.PropostaNumero,
                                    ContratoAno = prog.ContratoAno,
                                    ContratoNumero = prog.ContratoNumero,
                                    DataHora = DateTime.Now,
                                    Horario = "",
                                    Usuario = usuario,
                                    Evento = "Alteração",
                                    Complemento = "Traço",
                                    Descricao = $"Alteração no Valor Unitário do Traço {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao} \n" +
                                    $"De: {tracoOld.Descricao} R$ {tracoOld.M3PrecoProposto.ToString("F2")} \nPara: {descricaoTracoNovo} R$ {tDto.M3PrecoProposto.ToString("F2")}",
                                    Sequencia = sequenciaProgramacaoLog++
                                });

                                Commit();
                            }
                        }
                    }
                    tracoOld.AtualizaStatusAprovacao(usuario);

                    if (!mesmoTraco)
                    {

                        var tracoAntigoIsCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObra(tracoOld, obraOld) == 7105;

                        _obraService.Adicionar(new ObraLog()
                        {
                            UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                            ObraCodigo = propostaRequest.Obra.Numero,
                            AnoChamada = propostaRequest.Ano,
                            NumChamada = propostaRequest.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO DE TRAÇO",
                            Complemento = $"Alteração de traço sequência {tracoOld.Sequencia}, Descrição: {tracoOld.Descricao}",
                            Observacao = $"De:{(tracoAntigoIsCustoVirtual ? " [CUSTO VIRTUAL]" : "")} {tracoOld.Descricao} - {tracoOld.M3Quantidade.ToString("F1")} M3 - R$ {tracoOld.M3PrecoProposto.ToString("F2")}" +
                            $" \nPara:{(isTracoCustoVirtual ? " [CUSTO VIRTUAL]" : "")} {descricaoTracoNovo} - {tDto.M3Quantidade.ToString("F1")} M3 - R$ {tDto.M3PrecoProposto.ToString("F2")}",
                            Sequencia = sequenciaObraLog++
                        });

                    }



                    var tracoParaAprovacao = AutoMapper.Mapper.Map<ObraTraco>(tDto);
                    _obraService.CalcularEbitdaObraTraco(tracoParaAprovacao, obraOld);

                    if (parametroValidaAprovacaoComparandoComPrecoTabela && ((tDto.PrecoReajustadoAtual != 0 ? tDto.PrecoReajustadoAtual : tDto.M3PrecoProposto) != tracoOld.M3PrecoTabela)
                        || (tDto.M3PrecoProposto < tracoOld.M3PrecoProposto || !mesmoTraco || tDto.PrecoReajustadoAtual < tracoOld.PrecoReajustadoAtual))
                    {
                        tDto.AprovacaoObservacao = "";
                        _obraService.AtualizarStatusEngenharia(obraOld);

                        if (utilizaAprovacaoComercicalPorAlcada)
                        {
                            if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTraco(propostaOld, obraOld, tracoParaAprovacao) == EStatusAprovacao.Pendente)
                            {
                                tDto.AprovacaoVerbal = "N";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "G";
                            }
                            else
                            {
                                tDto.AprovacaoVerbal = "";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "";
                            }
                        }
                    }
                    else if (utilizaAprovacaoComercicalPorAlcada
                            && tDto.M3PrecoProposto != tracoOld.M3PrecoProposto
                            && (tracoOld.StatusAprovacao == EStatusAprovacao.Pendente || tracoOld.TracoReprovado()))
                    {

                        tDto.AprovacaoObservacao = "";

                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTraco(propostaOld, obraOld, tracoParaAprovacao) == EStatusAprovacao.Pendente)
                        {
                                tDto.AprovacaoVerbal = "N";
                                tDto.AprovacaoObservacao = "";
                                tDto.AprovacaoOperacao = "G";
                        }
                        else
                        {
                            tDto.AprovacaoVerbal = "";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "";
                        }
                    }

                    tracoOld = AutoMapper.Mapper.Map(tDto, tracoOld);

                    _obraService.CalcularEbitdaObraTraco(tracoOld, obraOld);
                }
                else
                {

                    tracoOld = AutoMapper.Mapper.Map<ObraTraco>(tDto);

                    var mercadoria = _mercadoriaService.ObterTracoMercadoria(tDto.Uso.Codigo, tDto.Pedra.Codigo, tDto.Slump.Codigo, tDto.ResistenciaTipo.Codigo, tDto.Mpa, tDto.Consumo);
                    var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObra(AutoMapper.Mapper.Map(tDto, new ObraTraco()), obraOld) == 7105;
                    var descricaoTraco = mercadoria is null ? "" : mercadoria.Descricao;

                    _obraService.Adicionar(new ObraLog()
                    {
                        UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                        ObraCodigo = propostaRequest.Obra.Numero,
                        AnoChamada = propostaRequest.Ano,
                        NumChamada = propostaRequest.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "INSERÇÃO DE TRAÇO",
                        Complemento = $"Inserção do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {tracoOld.Sequencia}, Descrição: {descricaoTraco}",
                        Observacao = $"{descricaoTraco} - {tracoOld.M3Quantidade.ToString("F1")} M3 - R$ {tracoOld.M3PrecoProposto.ToString("F2")}",
                        Sequencia = sequenciaObraLog++
                    });

                    _obraService.CalcularEbitdaObraTraco(tracoOld, obraOld);

                    if (utilizaAprovacaoComercicalPorAlcada)
                    {
                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaTraco(propostaOld, obraOld, tracoOld) == EStatusAprovacao.Pendente)
                        {
                            tracoOld.AprovacaoVerbal = "N";
                            tracoOld.AprovacaoObservacao = "";
                            tracoOld.AprovacaoOperacao = "G";
                        }
                    }

                    _propostaService.VerificaTracoJaIncluso(tracoOld);

                    tracoOld.AtualizaStatusAprovacao(usuario);

                    _obraService.Adicionar(tracoOld);
                    _obraService.AtualizarStatusEngenharia(obraOld);
                }

                _propostaService.ValidarNumeracaoProdutoCorretaObraTraco(tracoOld, obraOld.UsinaEntregaCodigo);

                _obraService.AdicionarLogPropostaItem(tracoOld, "PropostaApplicationService.AtualizarContrato");
                Commit();
            }

            var seqs = sequencias.ToArray();
            var tracosExcluidos = _obraService.ListarFiltradosTracking<ObraTraco>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in tracosExcluidos)
            {
                var temProgramacao = _programacaoService
                    .ListarFiltradosTracking(p => p.UsinaCodigo == obraOld.UsinaCodigo
                        && p.PropostaAno == propostaOld.Ano
                        && p.PropostaNumero == propostaOld.Numero
                        && p.ObraTracoSequencia == t.Sequencia).Count() > 0;

                if (temProgramacao)
                {
                    AssertionConcern.Notify("ObraTracos", $"Não é possível excluir o traço de sequência {t.Sequencia} pois existe programação vinculada.");
                    continue;
                }

                var isTracoCustoVirtual = _tracoPrecoService.ObterStatusTracoPorObra(t, obraOld) == 7105;

                _obraService.Adicionar(new ObraLog()
                {
                    UsinaCodigo = propostaRequest.Obra.UsinaCodigo,
                    ObraCodigo = propostaRequest.Obra.Numero,
                    AnoChamada = propostaRequest.Ano,
                    NumChamada = propostaRequest.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "REMOÇÃO DE TRAÇO",
                    Complemento = $"Remoção do traço{(isTracoCustoVirtual ? " com status de CUSTO VIRTUAL" : "")} sequência {t.Sequencia}, Descrição: {t.Descricao}",
                    Observacao = $"{t.Descricao} - {t.M3Quantidade.ToString("F1")} M3 - R$ {t.M3PrecoProposto.ToString("F2")}",
                    Sequencia = sequenciaObraLog++
                });

                _obraService.Remover(t);

                if (utilizaAprovacaoComercicalPorAlcada)
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaTraco(propostaOld, t);

                _obraService.AdicionarLogPropostaItem(t, "PropostaApplicationService.AtualizarContrato(Exclusão)");
                Commit();
            }

            // ObraBombas
            sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraBombas)
            {
                sequencias.Add(tDto.Sequencia);
                var bombaOld = _obraService.ObterPorId<ObraBomba>
                    (tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                if (bombaOld != null)
                {

                    var mesmaBomba = (bombaOld.BombaTipoCodigo ?? 0) == (tDto.BombaTipo is null ? 0 : tDto.BombaTipo.Codigo);

                    if (mesmaBomba)
                    {

                        var oldBombaTipo = _cadastroGeralService.ObterPorId(bombaOld.BombaTipoCodigo ?? 0, ECadastroGeralTipo.EquipamentoTipo);

                        if (bombaOld.DistanciaTubulacao != tDto.DistanciaTubulacao)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO DISTÂNCIA TUBULAÇÃO",
                                Complemento = $"Alteração de distância da tubulação na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: {bombaOld.DistanciaTubulacao} \nPara: {tDto.DistanciaTubulacao}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }

                        if(bombaOld.TaxaMinimaPrecoProposto != tDto.TaxaMinimaPrecoProposto)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO TAXA MÍNIMA BOMBA",
                                Complemento = $"Alteração de taxa mínima na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: R$ {bombaOld.TaxaMinimaPrecoProposto.ToString("F2")} \nPara: R$ {tDto.TaxaMinimaPrecoProposto.ToString("F2")}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }

                        if(bombaOld.M3PropostoAte != tDto.M3PropostoAte)
                        {

                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO BOMBA M3 ATÉ",
                                Complemento = $"Alteração de M3 Até na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: {bombaOld.M3PropostoAte} M3 \nPara: {tDto.M3PropostoAte} M3",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();

                        }

                        if(bombaOld.M3PrecoProposto != tDto.M3PrecoProposto)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO PREÇO M3 BOMBA",
                                Complemento = $"Alteração de preço M3 na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: R$ {bombaOld.M3PrecoProposto.ToString("F2")} \nPara: R$ {tDto.M3PrecoProposto.ToString("F2")}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }

                        var ultimoReajusteBomba = _contratoService.ListarFiltrados<ContratoBombaReajuste>(t => t.UsinaCodigo == propostaRequest.Obra.UsinaCodigo
                                && t.ContratoAno == propostaRequest.Obra.AnoContrato && t.ContratoNumero == propostaRequest.Obra.NumContrato && t.ObraBombaReajusteSequencia == tDto.Sequencia)
                                .OrderByDescending(t => t.DataVigencia).FirstOrDefault();

                        if (bombaOld.TaxaMinimaReajustadaAtual != tDto.TaxaMinimaReajustadaAtual)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.Obra.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO REAJUSTE TAXA MÍNIMA",
                                Complemento = $"Alteração de taxa mínima reajustada na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: R$ {bombaOld.TaxaMinimaReajustadaAtual.ToString("F2")} \nPara: R$ {tDto.TaxaMinimaReajustadaAtual.ToString("F2")} / Valor Último Reajuste: {ultimoReajusteBomba.ValorReajustado}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }

                        if (bombaOld.M3ReajustadoAteAtual != tDto.M3ReajustadoAteAtual)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO REAJUSTE M3 ATÉ",
                                Complemento = $"Alteração de M3 Até reajustado na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: {bombaOld.M3ReajustadoAteAtual} M3 \nPara: {tDto.M3ReajustadoAteAtual} M3 / Valor Último Reajuste: {ultimoReajusteBomba.ReajustadoAteM3}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();

                        }

                        if (bombaOld.M3PrecoReajustadoAtual != tDto.M3PrecoReajustadoAtual)
                        {
                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = "ALTERAÇÃO REAJUSTE PREÇO M3",
                                Complemento = $"Alteração de preço M3 reajustado na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                                Observacao = $"De: R$ {bombaOld.M3PrecoReajustadoAtual.ToString("F2")} \nPara: R$ {tDto.M3PrecoReajustadoAtual.ToString("F2")} / Valor Último Reajuste: {ultimoReajusteBomba.M3ExcedenteReajustado}",
                                Sequencia = sequenciaObraLog++
                            });

                            Commit();
                        }
                    } 
                    
                    if(!mesmaBomba)
                    {

                        var oldBombaTipo = _cadastroGeralService.ObterPorId(bombaOld.BombaTipoCodigo ?? 0, ECadastroGeralTipo.EquipamentoTipo);

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO BOMBA",
                            Complemento = $"Alteração na bomba sequência {bombaOld.Sequencia}, {(oldBombaTipo is null ? (bombaOld.BombaTipoCodigo ?? 0).ToString() : oldBombaTipo.Descricao)}",
                            Observacao = $"De: {(oldBombaTipo is null ? "TIPO NÃO INFORMADO" : oldBombaTipo.Descricao)} \nPara: {(tDto.BombaTipo is null ? "TIPO NÃO INFORMADO" : tDto.BombaTipo.Descricao)}",
                            Sequencia = sequenciaObraLog++
                        });

                        Commit();
                    }

                    // Buscando programações
                    var programacoes = _programacaoService
                        .ListarFiltradosTracking(t => t.UsinaCodigo == obraOld.UsinaCodigo
                            && t.PropostaAno == propostaOld.Ano
                            && t.PropostaNumero == propostaOld.Numero
                            && t.ObraNumero == obraOld.Numero
                            && t.ObraBombaSequencia == bombaOld.Sequencia);
                        
                    // programações
                    foreach (var prog in programacoes)
                    {
                        if (AutoMapper.Mapper.Map(tDto, new ObraBomba()).BombaProgramacaoScopeIsValid(bombaOld.BombaTipoCodigo))
                        {
                            var descricaoAlteracao = "";

                            if (prog.DistanciaTubulacao != tDto.DistanciaTubulacao)
                            {
                                descricaoAlteracao += $"Distância tubulação: {prog.DistanciaTubulacao} --> {tDto.DistanciaTubulacao} ";
                                prog.DistanciaTubulacao = tDto.DistanciaTubulacao;
                            }

                            if (bombaOld.TaxaMinimaPrecoProposto != tDto.TaxaMinimaPrecoProposto)
                            {
                                descricaoAlteracao += $"Taxa mínima: {bombaOld.TaxaMinimaPrecoProposto} --> {tDto.TaxaMinimaPrecoProposto} ";
                            }

                            if (bombaOld.M3PropostoAte != tDto.M3PropostoAte)
                            {
                                descricaoAlteracao += $"M3 até: {bombaOld.M3PropostoAte} --> {tDto.M3PropostoAte} ";
                            }

                            if (bombaOld.M3PrecoProposto != tDto.M3PrecoProposto)
                            {
                                descricaoAlteracao += $"Preço M3: {bombaOld.M3PrecoProposto} --> {tDto.M3PrecoProposto} ";
                            }

                            if (descricaoAlteracao != "")
                            {
                                Commit();

                                _programacaoService.Adicionar(new ProgramacaoLog()
                                {
                                    UsinaCodigo = prog.UsinaCodigo,
                                    ObraCodigo = prog.ObraNumero,
                                    ProgramacaoSequencia = prog.Sequencia,
                                    PropostaAno = prog.PropostaAno,
                                    PropostaNumero = prog.PropostaNumero,
                                    ContratoAno = prog.ContratoAno,
                                    ContratoNumero = prog.ContratoNumero,
                                    DataHora = DateTime.Now,
                                    Horario = "",
                                    Usuario = usuario,
                                    Evento = "Alteração",
                                    Complemento = "Bomba",
                                    Descricao = descricaoAlteracao,
                                    Sequencia = sequenciaProgramacaoLog++
                                });

                                Commit();
        
                            }
                        }
                    }

                    bombaOld.AtualizaStatusAprovacao(usuario);

                    if (tDto.M3PrecoProposto < bombaOld.M3PrecoProposto || tDto.M3PropostoAte < bombaOld.M3PropostoAte
                        || tDto.TaxaMinimaPrecoProposto < bombaOld.TaxaMinimaPrecoProposto
                        || tDto.HoraPrecoProposto < bombaOld.HoraPrecoProposto || tDto.HoraPropostoAte < bombaOld.HoraPropostoAte
                        || tDto.HoraTaxaMinimaPrecoProposto < bombaOld.HoraTaxaMinimaPrecoProposto)
                    {
                        tDto.AprovacaoObservacao = "";
                        tDto.AprovacaoVerbal = "";

                    if (utilizaAprovacaoComercicalPorAlcada)
                    {

                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBomba(propostaOld, obraOld, AutoMapper.Mapper.Map(tDto, new ObraBomba())) == EStatusAprovacao.Pendente)
                        {
                            tDto.AprovacaoVerbal = "S";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "G";

                        }
                    }

                    }
                    else if (utilizaAprovacaoComercicalPorAlcada
                            && (tDto.M3PrecoProposto != bombaOld.M3PrecoProposto || tDto.TaxaMinimaPrecoProposto != bombaOld.TaxaMinimaPrecoProposto)
                            && (bombaOld.StatusAprovacao == EStatusAprovacao.Pendente || bombaOld.StatusAprovacao == EStatusAprovacao.Reprovado))
                    {

                        tDto.AprovacaoObservacao = "";
                        tDto.AprovacaoVerbal = "";

                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBomba(propostaOld, obraOld, AutoMapper.Mapper.Map(tDto, new ObraBomba())) == EStatusAprovacao.Pendente)
                        {
                            tDto.AprovacaoVerbal = "S";
                            tDto.AprovacaoObservacao = "";
                            tDto.AprovacaoOperacao = "G";
                        }
                    }

                if (tDto.AprovacaoObservacao == "" && tDto.M3PrecoProposto == bombaOld.M3PrecoProposto && tDto.M3PropostoAte == bombaOld.M3PropostoAte
                        && tDto.TaxaMinimaPrecoProposto == bombaOld.TaxaMinimaPrecoProposto
                        && tDto.HoraPrecoProposto == bombaOld.HoraPrecoProposto && tDto.HoraPropostoAte == bombaOld.HoraPropostoAte
                        && tDto.HoraTaxaMinimaPrecoProposto == bombaOld.HoraTaxaMinimaPrecoProposto && !utilizaAprovacaoComercicalPorAlcada) tDto.AprovacaoObservacao = "sem_alteracao";

                    bombaOld = AutoMapper.Mapper.Map(tDto, bombaOld);

                if (!tDto.AprovacaoObservacao.Equals("sem_alteracao"))
                    bombaOld.AtualizaStatusAprovacao(usuario);
            }
                else
                {
                    bombaOld = AutoMapper.Mapper.Map(tDto, new ObraBomba());

                    _obraService.Adicionar(new ObraLog
                    {
                        UsinaCodigo = propostaOld.UsinaCodigo,
                        ObraCodigo = propostaOld.Obra.Numero,
                        AnoChamada = propostaOld.Ano,
                        NumChamada = propostaOld.Numero,
                        DataHora = DateTime.Now,
                        Usuario = usuario,
                        Evento = "INSERÇÃO BOMBA",
                        Complemento = $"Inserção de bomba sequência {bombaOld.Sequencia}",
                        Observacao = $"{(tDto.BombaTipo is null ? "TIPO NÃO INFORMADO" : tDto.BombaTipo.Descricao)}",
                        Sequencia = sequenciaObraLog++
                    });

                    Commit();

                    if (utilizaAprovacaoComercicalPorAlcada)
                    {
                        if (_aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaBomba(propostaOld, obraOld, bombaOld) == EStatusAprovacao.Pendente)
                        {

                            bombaOld.AprovacaoVerbal = "S";
                            bombaOld.AprovacaoObservacao = "";
                            bombaOld.AprovacaoOperacao = "G";

                        }
                    }

                    bombaOld.AtualizaStatusAprovacao(usuario);
                    _obraService.Adicionar(bombaOld);
                }
                    
                Commit();
            }

            seqs = sequencias.ToArray();
            var bombasExcluidas = _obraService.ListarFiltradosTracking<ObraBomba>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in bombasExcluidas)
            {
                _obraService.Remover(t);
                var descricaoBombaTipo = _cadastroGeralService.ObterDescricaoEquipamentoBombaPorObraBomba(t.UsinaCodigo, t.ObraCodigo, t.Sequencia);

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = propostaOld.UsinaCodigo,
                    ObraCodigo = propostaOld.Obra.Numero,
                    AnoChamada = propostaOld.Ano,
                    NumChamada = propostaOld.Numero,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "REMOÇÃO DA BOMBA",
                    Complemento = $"Remoção da bomba sequência {t.Sequencia}",
                    Observacao = $"{descricaoBombaTipo}",
                    Sequencia = sequenciaObraLog++
                });

                if (utilizaAprovacaoComercicalPorAlcada)
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaBomba(propostaOld, t);

                Commit();
            }

            // Aprovação por Alçada Volume
            if (utilizaAprovacaoComercicalPorAlcada && volumeTotalObraOld != volumeTotalObraNew)
            {

                var statusAprovacao = _aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaVolume(propostaOld, obraOld);

                if (statusAprovacao == EStatusAprovacao.Pendente)
                    obraOld.VolumeStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;

                if (statusAprovacao == EStatusAprovacao.NaoNecessita)
                {
                    obraOld.VolumeStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaVolume(propostaOld);
                }

                Commit();

            }

            // Aprovação por Alçada CondicaoPagamento
            if (utilizaAprovacaoComercicalPorAlcada && propostaOld.Obra.CondicaoPagamentoCodigo != condicaoPagamentoAnterior)
            {

                var statusAprovacao = _aprovacaoComercialPendenteService.AtualizarAprovacaoAlcadaCondicaoPagamento(propostaOld, obraOld);

                if (statusAprovacao == EStatusAprovacao.Pendente)
                    obraOld.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.AguardandoAprovacao;

                if (statusAprovacao == EStatusAprovacao.NaoNecessita)
                {
                    obraOld.CondicaoPagamentoStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
                    _aprovacaoComercialPendenteService.RemoverAprovacaoAlcadaCondicaoPagamento(propostaOld);
                }

                Commit();

            }


            if (utilizaAprovacaoComercicalPorAlcada)
            {
                _aprovacaoComercialPendenteService.RevisarAprovacaoComercialPendente(obraOld, null, propostaOld);
            }

            var taxasOldComparacao = _obraTaxaService.ListarByIdObra(propostaRequest.Obra.UsinaEntrega.Codigo, propostaRequest.Obra.Numero);

            // ObraTaxas
            foreach (var tDto in propostaRequest.Obra.ObraTaxas)
            {
                if (tDto.Nova && tDto.Selecionada == "N") continue;

                tDto.UsinaCodigo = propostaRequest.Obra.UsinaEntrega.Codigo;

                var taxaOld = _obraService.ObterPorId<ObraTaxa>
                    (tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                var taxaOldComparacao = taxasOldComparacao.FirstOrDefault(x => x.Sequencia == tDto.Sequencia);

                if (taxaOld != null)
                {

                    var descricaoAlteracao = new Dictionary<string, string>();

                    if (taxaOldComparacao != null)
                    {

                        if (!tDto.CobrarVolume.Equals(taxaOldComparacao.CobrarVolume, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("COBRADO POR", $"De: {taxaOldComparacao.Descricao} Para: {tDto.Descricao}");

                        if (!tDto.Volume.Equals(taxaOldComparacao.Volume, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("VOLUME", $"De: {taxaOldComparacao.Volume} M3 \nPara: {tDto.Volume} M3");

                        if (!tDto.ValorTipo.Equals(taxaOldComparacao.ValorTipo, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("TIPO DE VALOR", $"De: {taxaOldComparacao.ValorTipo} \nPara: {tDto.ValorTipo}");

                        if (tDto.Valor != taxaOldComparacao.Valor)
                            descricaoAlteracao.Add("VALOR", $"De: {taxaOldComparacao.ValorTipo} {taxaOldComparacao.Valor.ToString("F2")} por {taxaOldComparacao.ValorPor}  \nPara: {tDto.ValorTipo} {tDto.Valor.ToString("F2")} por {tDto.ValorPor}");

                        if (!tDto.ValorPor.Equals(taxaOldComparacao.ValorPor, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("VALOR POR", $"De: {taxaOldComparacao.ValorPor} \nPara: {tDto.ValorPor}");

                        if (!tDto.HorarioAntesDas.Equals(taxaOldComparacao.HorarioAntesDas, StringComparison.OrdinalIgnoreCase)
                            || !tDto.HorarioAntesDas.Equals(taxaOldComparacao.HorarioAntesDas, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("HORÁRIO", $"De: Antes das {taxaOldComparacao.HorarioAntesDas} e após as {taxaOldComparacao.HorarioAposAs} \nPara: Antes das {tDto.HorarioAntesDas} e após as {tDto.HorarioAposAs}");

                        if (!tDto.Selecionada.Equals(taxaOldComparacao.Selecionada, StringComparison.OrdinalIgnoreCase))
                            descricaoAlteracao.Add("SELECIONADA", $"De: {(taxaOldComparacao.Selecionada.Equals("S") ? "Selecionada" : "Não Selecionada")} \nPara: {(tDto.Selecionada.Equals("S") ? "Selecionada" : "Não Selecionada")}");

                    }

                    if(descricaoAlteracao.Count > 0)
                    {

                        foreach(var descricao in descricaoAlteracao.Keys)
                        {

                            var textoDescritivo = descricaoAlteracao[descricao];

                            _obraService.Adicionar(new ObraLog
                            {
                                UsinaCodigo = propostaOld.UsinaCodigo,
                                ObraCodigo = propostaOld.Obra.Numero,
                                AnoChamada = propostaOld.Ano,
                                NumChamada = propostaOld.Numero,
                                DataHora = DateTime.Now,
                                Usuario = usuario,
                                Evento = $"ALTERAÇÃO {descricao} TAXA",
                                Complemento = $"Taxa extra sequência {tDto.Sequencia}, {tDto.Descricao}\n{textoDescritivo}",
                                Observacao = $"",
                                Sequencia = sequenciaObraLog++
                            });

                        }

                    }

                    taxaOld = AutoMapper.Mapper.Map(tDto, taxaOld);
                    taxaOld.IdAtualizacao = obraOld.IdAtualizacao;
                }
                else
                {
                    taxaOld = AutoMapper.Mapper.Map<ObraTaxa>(tDto);
                    taxaOld.ObraCodigo = propostaRequest.Obra.Numero;
                    taxaOld.IdCadastro = StringHelper.GetIDD(usuario);
                    taxaOld.AprovacaoCiente = "";
                    taxaOld.AprovacaoUsuario = "";
                    taxaOld.LogObservacao = "";
                    _obraService.Adicionar(taxaOld);
                }
                Commit();

                if (taxaOld.IsPersonalizada && taxaOld.Selecionada == "S")
                {
                    _obraTaxaService.SalvarPersonalizada(taxaOld);
                }
                else if(taxaOld.IsPersonalizada)
                {
                    _obraTaxaService.DeletarPersonalizada(taxaOld);
                }
                Commit();
            }

            // ObraTributacoesMunicipais
            var usinasTributacaoMunicipal = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraTributacoesMunicipais)
            {
                usinasTributacaoMunicipal.Add(tDto.UsinaEntregaCodigo);
                tDto.ObraUsinaCodigo = propostaRequest.Obra.UsinaCodigo;
                tDto.ObraNumero = propostaRequest.Obra.Numero;
                tDto.ContratoAno = obraOld.AnoContrato;
                tDto.ContratoNumero = obraOld.NumContrato;

                var tribMunOld = _obraService.ObterPorId<ObraTributacaoMunicipal>
                    (tDto.ObraUsinaCodigo, tDto.ObraNumero, tDto.UsinaEntregaCodigo);

                if (tribMunOld != null)
                {
                    tribMunOld = AutoMapper.Mapper.Map(tDto, tribMunOld);
                }
                else
                {
                    tribMunOld = AutoMapper.Mapper.Map<ObraTributacaoMunicipal>(tDto);
                    _obraService.Adicionar(tribMunOld);
                }
                Commit();
            }

            var usinasTribMun = usinasTributacaoMunicipal.ToArray();
            var usinasTribMunExcluidas = _obraService.ListarFiltradosTracking<ObraTributacaoMunicipal>
                (t => t.ObraUsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraNumero == propostaRequest.Obra.Numero
                    && !usinasTribMun.Contains(t.UsinaEntregaCodigo));

            foreach (var t in usinasTribMunExcluidas)
            {
                _obraService.Remover(t);
                Commit();
            }

            // ObraDemaisServicos
            sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraDemaisServicos)
            {
                sequencias.Add(tDto.Sequencia);
                tDto.UsinaCodigo = propostaRequest.Obra.UsinaCodigo;
                tDto.ObraNumero = propostaRequest.Obra.Numero;

                var demaisServicosOld = _obraService.ObterPorId<ObraDemaisServicos>
                    (tDto.UsinaCodigo, tDto.ObraNumero, tDto.Sequencia);

                if (demaisServicosOld != null)
                {
                    demaisServicosOld = AutoMapper.Mapper.Map(tDto, demaisServicosOld);
                }
                else
                {
                    demaisServicosOld = AutoMapper.Mapper.Map<ObraDemaisServicos>(tDto);
                    _obraService.Adicionar(demaisServicosOld);
                }
                Commit();
            }

            seqs = sequencias.ToArray();
            var demaisServicosExcluidos = _obraService.ListarFiltradosTracking<ObraDemaisServicos>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraNumero == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in demaisServicosExcluidos)
            {
                _obraService.Remover(t);
                Commit();
            }

            var taxas = _obraTaxaService.ListarByIdObra(obraOld.UsinaEntregaCodigo, obraOld.Numero);
            var valorConcreto = obraOld.CalcularValorConcreto();
            var naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
            var valorBomba = obraOld.CalcularValorBomba(naoConsideraTodasBombas);

            var valorDemaisServicos = obraOld.CalcularValorDemaisServicos();
            obraOld.ValorDemaisServicos = valorDemaisServicos;
            Commit();

            var valorM3Faltante = _obraTaxaService.ObterValorM3Faltante((propostaRequest.Obra.ObraBombas?.Count() ?? 0) > 0,
                propostaRequest.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, propostaRequest.Obra.VolumePorCarga, obraOld.ObraTaxas ?? taxas);
            var valorAdicionalPorKmRodado = _obraTaxaService.ObterValorAdicionalPorKmRodado(propostaRequest.Obra.DistanciaUsina,
                propostaRequest.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, propostaRequest.Obra.VolumePorCarga, obraOld.ObraTaxas ?? taxas, propostaRequest.Obra.ObraBombas.Count > 0);
            //var valorExtras = valorM3Faltante + (propostaRequest.Obra.VibradorValorUnitario * propostaRequest.Obra.VibradorQuantidade) + valorAdicionalPorKmRodado;
            var valorExtras = valorM3Faltante + valorAdicionalPorKmRodado;
            var valorTotalDecimal = valorConcreto + valorBomba + (decimal)valorExtras + (decimal)valorDemaisServicos;
            var valorTotal = float.Parse(valorTotalDecimal.ToString());

            bool incluiuPagamentoDetalhe = false;
            bool alterouPagamentoDetalhe = false;

            // ObraPagamentos
            sequencias = new List<int>();
            var houveAlteracaoPagamento = false;
             foreach (var tDto in propostaRequest.Obra.ObraPagamentos)
            {
                // PropostaPagamentos
                sequencias.Add(tDto.Sequencia);
                var pagamentoOld = _obraService.ObterPorId<PropostaPagamento>
                    (propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero, tDto.Sequencia);

                var formaPagamentoOld = pagamentoOld?.Forma ?? "";

                if (pagamentoOld != null)
                {
                    pagamentoOld = AutoMapper.Mapper.Map(tDto, pagamentoOld);
                    pagamentoOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                }
                else
                {
                    pagamentoOld = AutoMapper.Mapper.Map<PropostaPagamento>(tDto);
                    if (!pagamentoOld.PagamentoSequenciaIsValid()) return;
                    pagamentoOld.IdCadastro = StringHelper.GetIDD(usuario);
                    pagamentoOld.IdAtualizacao = "";
                    pagamentoOld.IdAprovacao = "";
                    _obraService.Adicionar(pagamentoOld);
                }

                pagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                pagamentoOld.PropostaAno = propostaRequest.Ano;
                pagamentoOld.PropostaNumero = propostaRequest.Numero;
                pagamentoOld.ObraCodigo = propostaRequest.Obra.Numero;
                pagamentoOld.Forma = tDto.TipoCobranca.Forma;
                pagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                pagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                pagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao=="" ? "S" : pagamentoOld.AtivoSimNao;
                pagamentoOld.Percentual = pagamentoOld.Valor / valorTotal * 100f;
                Commit();

                // Caso tenha Contrato
                if (propostaRequest.Obra.NumContrato > 0)
                {
                    // ContratoPagamentos
                    var contratoPagamentoOld = _obraService.ObterPorId<ContratoPagamentoForSaving>
                        (propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato, tDto.Sequencia);

                    if (contratoPagamentoOld != null)
                    {

                        if(!houveAlteracaoPagamento)
                        {
                            houveAlteracaoPagamento = houveAlteracaoPagamento || (contratoPagamentoOld.Forma != tDto.TipoCobranca.Forma);
                            houveAlteracaoPagamento = houveAlteracaoPagamento || (contratoPagamentoOld.CondicaoPagamentoCodigo != tDto.CondicaoPagamento.Codigo);
                            houveAlteracaoPagamento = houveAlteracaoPagamento || (contratoPagamentoOld.Valor != tDto.Valor);
                        }
                        
                        contratoPagamentoOld = AutoMapper.Mapper.Map(tDto, contratoPagamentoOld);
                        contratoPagamentoOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                        contratoPagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                        contratoPagamentoOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                        contratoPagamentoOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                        contratoPagamentoOld.Forma = tDto.TipoCobranca.Forma;
                        contratoPagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                        contratoPagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                        contratoPagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao == "" ? "S" : pagamentoOld.AtivoSimNao;
                        contratoPagamentoOld.Percentual = contratoPagamentoOld.Valor / valorTotal * 100f;
                    }
                    else
                    {
                        houveAlteracaoPagamento = true;

                        contratoPagamentoOld = AutoMapper.Mapper.Map<ContratoPagamentoForSaving>(tDto);
                        contratoPagamentoOld.IdCadastro = StringHelper.GetIDD(usuario);
                        contratoPagamentoOld.IdAtualizacao = "";
                        contratoPagamentoOld.IdAprovacao = "";
                        contratoPagamentoOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                        contratoPagamentoOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                        contratoPagamentoOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                        contratoPagamentoOld.Forma = tDto.TipoCobranca.Forma;
                        contratoPagamentoOld.ValorFixoSimNao = tDto.TipoCobranca.Fixo;
                        contratoPagamentoOld.NecessitaAprovacaoSimNao = tDto.TipoCobranca.Aprovacao;
                        contratoPagamentoOld.AtivoSimNao = pagamentoOld.AtivoSimNao == "" ? "S" : pagamentoOld.AtivoSimNao;
                        contratoPagamentoOld.Percentual = contratoPagamentoOld.Valor / valorTotal * 100f;
                        _obraService.Adicionar(contratoPagamentoOld);
                    }
                     Commit();
                }

                // PropostaPagamentoDetalhes
                if ((new[] { "DP", "CC", "CD", "CH", "CP", "BE", "DN" }).Contains(tDto.TipoCobranca.Forma))
                {
                    var detalheSequencias = new List<int>();
                    foreach (var detalheDto in tDto.Detalhes)
                    {
                        // local void function
                        void atualizarPagamentoDetalhe<TDetalhe>(int usina, int? ano, int? numero, int pagamentoSequencia, int detalheSequencia, int obraNumero=0) where TDetalhe : ObraPagamentoDetalhe
                        {
                            detalheSequencias.Add(detalheDto.DetalheSequencia);
                               
                            var detalheOld = _obraService.BuscarDetalhes<TDetalhe>(tDto.TipoCobranca.Forma, usina, ano ?? 0, numero ?? 0,pagamentoSequencia,detalheSequencia,obraNumero,true);
                            //var detalheOld = _obraService.ObterPorId<TDetalhe>(chavePagamentoDetalhe);

                            if (detalheOld != null && formaPagamentoOld == tDto.TipoCobranca.Forma)
                            {
                                alterouPagamentoDetalhe = alterouPagamentoDetalhe || HouveAlteracao(formaPagamentoOld, detalheOld, detalheDto);

                                detalheOld = AutoMapper.Mapper.Map(detalheDto, detalheOld);

                                // TODO: verificar o caso do boleto express pois na tabela não existe
                                // o campo id_atual, apenas na view mapeada, o que torna o campo não alterável.
                                // Obs.: atualmente não há problema pois o detalhamento de boleto não é alterado pelo web
                                if (alterouPagamentoDetalhe) detalheOld.IdAtualizacao = StringHelper.GetIDD(usuario);
                            }
                            else
                            {
                                if (detalheOld != null && formaPagamentoOld != tDto.TipoCobranca.Forma)
                                {
                                    _obraService.Remover(detalheOld);
                                }
                                
                                if (formaPagamentoOld != tDto.TipoCobranca.Forma)
                                {
                                    var detalhesExcluidosOld = _obraService.ListarDetalhes<TDetalhe>
                                        (formaPagamentoOld, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                                            && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                            && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                            && t.PagamentoSequencia == tDto.Sequencia
                                            && detalheSequencias.Contains(t.DetalheSequencia), true);

                                    foreach (var t in detalhesExcluidosOld)
                                    {
                                        _obraService.Remover(t);
                                        Commit();
                                    }
                                }

                                detalheOld = AutoMapper.Mapper.Map<TDetalhe>(detalheDto);
                                if (!detalheOld.PagamentoDetalheSequenciaIsValid()) return;
                                detalheOld.IdCadastro = StringHelper.GetIDD(usuario);
                                detalheOld.IdAtualizacao = "";
                                _obraService.Adicionar(detalheOld);

                                incluiuPagamentoDetalhe = true;
                            }

                            detalheOld.UsinaCodigo = propostaRequest.Usina.Codigo;
                            detalheOld.PropostaAno = propostaRequest.Obra.AnoChamada ?? 0;
                            detalheOld.PropostaNumero = propostaRequest.Obra.NumChamada ?? 0;
                            detalheOld.ContratoAno = propostaRequest.Obra.AnoContrato ?? 0;
                            detalheOld.ContratoNumero = propostaRequest.Obra.NumContrato ?? 0;
                            detalheOld.ObraCodigo = propostaRequest.Obra.Numero;
                            detalheOld.PagamentoSequencia = tDto.Sequencia;

                            if (tDto.TipoCobranca.Forma == "DP")
                            {
                                var newDetalhe = (IObraPagamentoDetalheDeposito)detalheOld;
                                newDetalhe.TomadorBanco = detalheDto.Portador.Conta.BancoCodigoOficial;
                                newDetalhe.TomadorAgencia = detalheDto.Portador.Conta.NumeroAgencia.ToString();
                                newDetalhe.TomadorNumeroConta = $"{detalheDto.Portador.Conta.NumeroConta}-{detalheDto.Portador.Conta.DvConta}";
                            }

                            Commit();
                        };

                        if (propostaRequest.Obra.NumContrato > 0)
                            atualizarPagamentoDetalhe<ContratoPagamentoDetalhe>(propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato, tDto.Sequencia, detalheDto.DetalheSequencia);
                        else
                            atualizarPagamentoDetalhe<PropostaPagamentoDetalhe>(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, tDto.Sequencia, detalheDto.DetalheSequencia, propostaRequest.Obra.Numero);
                    }

                    var detalheSeqs = detalheSequencias.ToArray();

                    if (propostaRequest.Obra.NumContrato > 0)
                    {
                        var detalhesExcluidos = _obraService.ListarDetalhes<ContratoPagamentoDetalhe>
                            (tDto.TipoCobranca.Forma, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                && t.PagamentoSequencia == tDto.Sequencia
                                && !detalheSeqs.Contains(t.DetalheSequencia), true);

                        //var detalhesExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoDetalhe>
                        //    (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                        //        && t.ContratoAno == propostaRequest.Obra.AnoContrato
                        //        && t.ContratoNumero == propostaRequest.Obra.NumContrato
                        //        && t.PagamentoSequencia == tDto.Sequencia
                        //        && !detalheSeqs.Contains(t.DetalheSequencia));

                        foreach (var t in detalhesExcluidos)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                    else
                    {

                        var detalhesExcluidos = _obraService.ListarDetalhes<PropostaPagamentoDetalhe>
                            (tDto.TipoCobranca.Forma, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.PropostaAno == propostaRequest.Ano
                                && t.PropostaNumero == propostaRequest.Numero
                                && t.ObraCodigo == propostaRequest.Obra.Numero
                                && t.PagamentoSequencia == tDto.Sequencia
                                && !detalheSeqs.Contains(t.DetalheSequencia), true);

                        foreach (var t in detalhesExcluidos)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                }
                else
                {
                    var detalhesExcluidos = _obraService.ListarDetalhes<PropostaPagamentoDetalhe>
                        (tDto.TipoCobranca.Forma, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                            && t.PropostaAno == propostaRequest.Ano
                            && t.PropostaNumero == propostaRequest.Numero
                            && t.ObraCodigo == propostaRequest.Obra.Numero
                            && t.PagamentoSequencia == tDto.Sequencia, true);

                    foreach (var t in detalhesExcluidos)
                    {
                        _obraService.Remover(t);
                        Commit();
                    }

                    if (formaPagamentoOld != tDto.TipoCobranca.Forma && propostaRequest.Obra.NumContrato > 0)
                    {
                        var detalhesExcluidosOld = _obraService.ListarDetalhes<ContratoPagamentoDetalhe>
                            (formaPagamentoOld, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                                && t.ContratoAno == propostaRequest.Obra.AnoContrato
                                && t.ContratoNumero == propostaRequest.Obra.NumContrato
                                && t.PagamentoSequencia == tDto.Sequencia, true);

                        foreach (var t in detalhesExcluidosOld)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    } else if (formaPagamentoOld != tDto.TipoCobranca.Forma)
                    {
                        var detalhesExcluidosOld = _obraService.ListarDetalhes<PropostaPagamentoDetalhe>
                        (formaPagamentoOld, t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                            && t.PropostaAno == propostaRequest.Ano
                            && t.PropostaNumero == propostaRequest.Numero
                            && t.ObraCodigo == propostaRequest.Obra.Numero
                            && t.PagamentoSequencia == tDto.Sequencia, true);

                        foreach (var t in detalhesExcluidosOld)
                        {
                            _obraService.Remover(t);
                            Commit();
                        }
                    }
                }
            }

            if(incluiuPagamentoDetalhe && (propostaRequest.Obra.NumContrato ?? 0) > 0)
            {
                var contrato = _contratoService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato);
                var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.Usina, contrato.Ano, contrato.Numero).ToList();
                _webHookApplicationService.AdicionarWebHookContratoPagamento(contrato, contratoPagamentos, EWebHookTipoEvento.Insert);
            } 

            seqs = sequencias.ToArray();
            var pagamentosExcluidos = _obraService.ListarFiltradosTracking<PropostaPagamento>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.PropostaAno == propostaRequest.Ano
                    && t.PropostaNumero == propostaRequest.Numero
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in pagamentosExcluidos)
            {
                var detalhes = _obraService.ListarDetalhes<PropostaPagamentoDetalhe>
                    (t.Forma,d => d.UsinaCodigo == propostaRequest.Usina.Codigo
                        && d.PropostaAno == propostaRequest.Ano
                        && d.PropostaNumero == propostaRequest.Numero
                        && d.ObraCodigo == propostaRequest.Obra.Numero
                        && d.PagamentoSequencia == t.Sequencia, true);

                foreach (var d in detalhes)
                {
                    _obraService.Remover(d);
                    Commit();
                }

                _obraService.Remover(t);
                Commit();
            }

            var contratoPagamentosExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoForSaving>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ContratoAno == (propostaRequest.Obra.AnoContrato ?? 0)
                    && t.ContratoNumero == (propostaRequest.Obra.NumContrato ?? 0)
                    && !seqs.Contains(t.Sequencia));

            foreach (var t in contratoPagamentosExcluidos)
            {
                _obraService.Remover(t);
                Commit();
            }

            //var propostaNew = _propostaService.ObterPorUsinaAnoNumero(propostaOld.UsinaCodigo, propostaOld.Ano, propostaOld.Numero, true);
            if (_propostaService.ValidarProposta(usuario, propostaOld, filtroVendedores, cpfCnpjAnterior, propostaStatusAnterior))
            {
                if (!_notifications.HasNotifications())
                {
                    bool houveAlteracaoDadosProposta = false;

                    if (propostaOld.Obra.CondicaoPagamentoCodigo != condicaoPagamentoAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = condicaoPagamentoAnterior > 0 ? "ALTERAÇÃO COND.PAGTO.PRINC." : "INSERÇÃO COND.PAGTO.PRINC.",
                            Complemento = (condicaoPagamentoAnterior > 0 ? $"De: {condicaoPagamentoAnterior} - {condicaoPagamentoAnteriorDescricao} Para: " : "Cond. Pagto.: ") +
                                $"{propostaOld.Obra.CondicaoPagamentoCodigo} - {_obraService.ObterPorId<CondicaoPagamento>(propostaOld.Obra.CondicaoPagamentoCodigo)?.Descricao ?? ""}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.Obra.TipoCobrancaCodigo != tipoCobrancaAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = tipoCobrancaAnterior > 0 ? "ALTERAÇÃO TIPO COB.PRINC." : "INSERÇÃO TIPO COB.PRINC.",
                            Complemento = (tipoCobrancaAnterior > 0 ? $"De: {tipoCobrancaAnterior} - {tipoCobrancaAnteriorDescricao} Para: " : "Tipo Cob.: ") +
                                $"{propostaOld.Obra.TipoCobrancaCodigo} - {_obraService.ObterPorId<TipoCobranca>(propostaOld.Obra.TipoCobrancaCodigo)?.Descricao ?? ""}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.Vendedor.Nome != vendedorOld.Nome)
                    {
                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO DE VENDEDOR",
                            Complemento = $"O VENDEDOR {vendedorOld.Nome} FOI ALTERADO PARA O VENDEDOR {propostaOld.Vendedor.Nome}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaStatusAnterior != propostaOld.Status)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO STATUS",
                            Complemento = $"De: {(int)propostaStatusAnterior} - {propostaStatusAnterior} Para: {(int)propostaOld.Status} - {propostaOld.Status}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.ValorTotalContrato != valorTotalAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO TOTAL CONTRATO",
                            Complemento = $"Alteração no Valor De: {valorTotalAnterior} Para: {propostaOld.ValorTotalContrato}",
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    if (propostaOld.TempoAprovacaoMedicao != tempoAprovacaoMedicaoAnterior || propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                    {
                        houveAlteracaoDadosProposta = true;

                        var aprovacaoMedicaoAnteriorString = aprovacaoMedicaoAnterior == "S" ? "SIM" : "NÃO";
                        var aprovacaoMedicaoString = propostaOld.AprovacaoMedicao == "S" ? "SIM" : "NÃO";

                        var complemento = "";
                        if (propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                            complemento += $"Medição Contrato De: {aprovacaoMedicaoAnteriorString} Para: {aprovacaoMedicaoString}";

                        if (tempoAprovacaoMedicaoAnterior != propostaOld.TempoAprovacaoMedicao)
                        {
                            if (propostaOld.AprovacaoMedicao != aprovacaoMedicaoAnterior)
                                complemento += " - ";

                            complemento += $"Tempo para Aprovação De: {tempoAprovacaoMedicaoAnterior}H Para: {propostaOld.TempoAprovacaoMedicao}H";
                        }

                        _obraService.Adicionar(new ObraLog
                        {
                            UsinaCodigo = propostaOld.UsinaCodigo,
                            ObraCodigo = propostaOld.Obra.Numero,
                            AnoChamada = propostaOld.Ano,
                            NumChamada = propostaOld.Numero,
                            DataHora = DateTime.Now,
                            Usuario = usuario,
                            Evento = "ALTERAÇÃO MEDIÇÃO CONTRATO",
                            Complemento = complemento,
                            Observacao = "",
                            Sequencia = sequenciaObraLog++
                        });
                    }

                    Commit();
                        
                    if (obraOld.NumContrato > 0)
                    {
                        var contrato = _contratoService.ObterPorId(obraOld.UsinaCodigo, obraOld.AnoContrato, obraOld.NumContrato);
                        if (contrato != null)
                        {
                            contrato.NumeroContratoAnterior = propostaRequest.Obra.Contrato.NumeroContratoAnterior;
                            Commit();

                            //Adicional ZMRC
                            if (usaAdicionalZMRCAnterior == "S" && propostaRequest.Obra.UsaAdicionalZMRC == "N")
                            {
                                obraOld.NecessitaAprovacaoZMRC = "S";

                                if(!parametroDesativarObrigatoriedadeAprovacaoCadastro)
                                    contrato.Status = EContratoStatus.RevalidacaoCadastro;

                                _obraService.Adicionar(new ObraLog
                                {
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO ZMRC",
                                    Complemento = "DE SIM PARA NÃO",
                                    Observacao = "",
                                    Sequencia = sequenciaObraLog++
                                });
                            }

                        if (propostaRequest.Obra.Contrato?.DataEncerramento != contrato.DataEncerramento)
                        {
                            if (contrato.DataEncerramento != null)
                            {
                                if (propostaRequest.Obra.Contrato?.DataEncerramento != null)
                                {
                                    _obraService.Adicionar(new ObraLog
                                    {
                                        UsinaCodigo = propostaRequest.Usina.Codigo,
                                        ObraCodigo = propostaRequest.Obra.Numero,
                                        AnoChamada = propostaRequest.Ano,
                                        NumChamada = propostaRequest.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "Data de encerramento alterada",
                                        Complemento = $"Data de encerramento alterada de {contrato.DataEncerramento} para {propostaRequest.Obra.Contrato?.DataEncerramento}",
                                        Observacao = "",
                                        Sequencia = sequenciaObraLog++
                                    });
                                }
                                else
                                {
                                    _obraService.Adicionar(new ObraLog
                                    {
                                        UsinaCodigo = propostaRequest.Usina.Codigo,
                                        ObraCodigo = propostaRequest.Obra.Numero,
                                        AnoChamada = propostaRequest.Ano,
                                        NumChamada = propostaRequest.Numero,
                                        DataHora = DateTime.Now,
                                        Usuario = usuario,
                                        Evento = "CONTRATO ABERTO",
                                        Complemento = "Status contrato: Aberto",
                                        Observacao = "Contrato aberto via TopconCRM",
                                        Sequencia = sequenciaObraLog++
                                    });
                                }   
                            }
                            else
                            {
                                _obraService.Adicionar(new ObraLog
                                {
                                    UsinaCodigo = propostaRequest.Usina.Codigo,
                                    ObraCodigo = propostaRequest.Obra.Numero,
                                    AnoChamada = propostaRequest.Ano,
                                    NumChamada = propostaRequest.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ENCERRADO",
                                    Complemento = "Status contrato: Encerrado",
                                    Observacao = "Contrato encerrado via TopconCRM",
                                    Sequencia = sequenciaObraLog++
                                });
                            }

                            contrato.DataEncerramento = propostaRequest.Obra.Contrato?.DataEncerramento;
                            Commit();
                        }

                        if (houveMudancaSegmentacaoProposta)
                        {
                            contrato.Segmentacao = propostaRequest.Segmentacao;
                            Commit();
                        }

                        if (houveMudancaFinalidadeProposta)
                        {
                            contrato.ContratoFinalidade = propostaRequest.ContratoFinalidade;
                            Commit();
                        }

                            if ((contrato.IntervenienteCodigo ?? 0) == 0)
                        {
                            contrato.IntervenienteCodigo = propostaOld.IntervenienteCodigo;
                            Commit();
                        }

                        if (!propostaOld.PropostaContratoIntervenienteIsValid(contrato))
                            return;

                        // Dados Faturamento
                        if (propostaOld.UtilizaDadosFaturamento || propostaOld.UtilizaEnderecoFaturamento)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.Faturamento, out int sequenciaLocal, usuario, t => t.LocalFaturamentoSimNao == "S");
                                if (contrato.LocalFaturamento != sequenciaLocal)
                                {
                                    contrato.LocalFaturamento = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.LocalFaturamento > 0)
                            {
                                contrato.LocalFaturamento = 0;
                                Commit();
                            }

                            // Dados Cobrança
                            if (propostaOld.UtilizaDadosCobranca || propostaOld.UtilizaEnderecoCobranca)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.Cobranca, out int sequenciaLocal, usuario, t => t.LocalCobrancaSimNao == "S");
                                if (contrato.LocalCobranca != sequenciaLocal)
                                {
                                    contrato.LocalCobranca = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.LocalCobranca > 0)
                            {
                                contrato.LocalCobranca = 0;
                                Commit();
                            }

                            // Dados Responsável Solidário
                            if (propostaOld.UtilizaResponsavelSolidario)
                            {
                                SalvarIntervenienteLocal(contrato.IntervenienteCodigo, propostaOld.ResponsavelSolidario, out int sequenciaLocal, usuario);
                                if (contrato.ResponsavelSolidario != sequenciaLocal)
                                {
                                    contrato.ResponsavelSolidario = sequenciaLocal;
                                    Commit();
                                }
                            }
                            else if (contrato.ResponsavelSolidario > 0)
                            {
                                contrato.ResponsavelSolidario = 0;
                                Commit();
                            }

                            contrato.ValorConcreto = propostaOld.ValorConcreto;
                            contrato.ValorBomba = propostaOld.ValorBomba;
                            contrato.ValorExtras = propostaOld.ValorExtras;
                            contrato.ValorTotalContrato = propostaOld.ValorTotalContrato;
                            contrato.VolumeTotal = propostaOld.VolumeTotal;
                            contrato.Observacao = propostaOld.Observacao;
                            contrato.VendedorCodigo = propostaOld.VendedorCodigo;
                            contrato.CodigoObraPrefeitura = propostaOld.CodigoObraPrefeitura;
                            contrato.UsinaEntrega = propostaOld.Obra.UsinaEntregaCodigo;
                            contrato.ModeloDocumentoRemessaConcreto = propostaOld.ModeloDocumentoRemessaConcreto;
                            contrato.ModeloDocumentoRemessaBomba = propostaOld.ModeloDocumentoRemessaBomba;
                            contrato.ContratoFinalidade = propostaOld.ContratoFinalidade;
                            contrato.ModeloItensDanfeERomaneio = propostaOld.ModeloItensDanfeERomaneio;

                            contrato.FimVigencia = propostaRequest.Obra.Contrato.FimVigencia;

                            contrato.AprovacaoMedicao = propostaRequest.AprovacaoMedicao;
                            contrato.TempoAprovacaoMedicao = propostaRequest.TempoAprovacaoMedicao;

                            var contratoStatusAnterior = contrato.Status;
                            var observacao = "";

                            if (houveAlteracaoDadosProposta)
                            {
                                switch (contrato.Status)
                                {
                                    case EContratoStatus.Reprovado:
                                    case EContratoStatus.Pendente:
                                    case EContratoStatus.Cancelado:
                                        contrato.StatusAnterior = contrato.Status;
                                        contrato.Status = EContratoStatus.EmAnalise;
                                        observacao = "Alteração de dados na proposta";
                                        break;
                                }

                                if (obraOld.CondicaoPagamentoCodigo != condicaoPagamentoAnterior || obraOld.TipoCobrancaCodigo != tipoCobrancaAnterior)
                                {
                                    switch (contrato.Status)
                                    {
                                        case EContratoStatus.AguardandoConfirmacaoPagamento:
                                        case EContratoStatus.AguardandoDadosPagamento:
                                            contrato.Status = EContratoStatus.EmAnalise;
                                            break;
                                        case EContratoStatus.Aprovado:
                                        contrato.Status = parametroDesativarObrigatoriedadeAprovacaoCadastro ? EContratoStatus.Aprovado : EContratoStatus.RevalidacaoCadastro;
                                            break;
                                    }
                                }
                            }

                            if (contrato.Status == EContratoStatus.AguardandoDadosPagamento && (incluiuPagamentoDetalhe || alterouPagamentoDetalhe))
                            {
                                contrato.Status = EContratoStatus.AguardandoConfirmacaoPagamento;
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && propostaOld.ValorTotalContrato != valorTotalAnterior && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Valor total contrato foi alterado";
                            }

                            if (contrato.Fechado && propostaOld.Obra.Cei != ceiObraAnterior && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Valor de CEI/CNO da Obra foi alterado";
                            }
                            
                            if (contrato.Status == EContratoStatus.Aprovado && (propostaOld.Obra.EnderecoLogradouro != logradouroObraAnterior 
                                || propostaOld.Obra.EnderecoNumero != numeroObraAnterior
                                || propostaOld.Obra.EnderecoCep != cepObraAnterior
                                || propostaOld.Obra.EnderecoComplemento != complementoObraAnterior
                                || propostaOld.Obra.EnderecoBairro != bairroObraAnterior
                                || propostaOld.Obra.EnderecoMunicipioCodigo != municipioObraAnterior)
                                && !parametroDesativarObrigatoriedadeAprovacaoCadastro)
                            {
                                contrato.Status = EContratoStatus.RevalidacaoCadastro;
                                observacao = "Endereço da Obra foi alterado";
                            }

                            if (contrato.Status == EContratoStatus.Aprovado && propostaOld.Status == EPropostaStatus.AguardandoAprovacaoComercial && propostaOld.Status != propostaStatusAnterior)
                            {
                                contrato.StatusAnterior = contrato.Status;
                                contrato.Status = EContratoStatus.AguardandoAprovacaoComercial;
                            }

                            if (contrato.DataEncerramento != null && contrato.Status != EContratoStatus.Encerrado)
                            {
                                contrato.StatusAnterior = contrato.Status;
                                contrato.Status = EContratoStatus.Encerrado;
                            }

                            if (contrato.DataEncerramento == null && contrato.Status == EContratoStatus.Encerrado)
                            {
                                contrato.Status = contrato.StatusAnterior;
                                contrato.StatusAnterior = EContratoStatus.Encerrado;
                            }

                            Commit();

                            if (contrato.Status != contratoStatusAnterior)
                            {
                                _obraService.Adicionar(new ObraLog
                                {
                                    UsinaCodigo = propostaOld.UsinaCodigo,
                                    ObraCodigo = propostaOld.Obra.Numero,
                                    AnoChamada = propostaOld.Ano,
                                    NumChamada = propostaOld.Numero,
                                    DataHora = DateTime.Now,
                                    Usuario = usuario,
                                    Evento = "ALTERAÇÃO STATUS CONTRATO",
                                    Complemento = $"De: {(int)contratoStatusAnterior} - {contratoStatusAnterior} Para: {(int)contrato.Status} - {contrato.Status }",
                                    Observacao = observacao,
                                    Sequencia = sequenciaObraLog++
                                });

                                Commit();
                            }
                        }
                    }

                    _propostaService.ValidarDemaisAprovacoes(usuario, propostaOld, cpfCnpjAnterior, propostaStatusAnterior);

                    _obraService.AtualizarStatusComercial(propostaOld.UsinaCodigo, propostaOld.Obra.Numero);

                    var statusFinanceiroAnterior = obraOld.StatusFinanceiro;

                    _obraService.AtualizarStatusFinanceiro(obraOld, usuario);

                    _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(propostaOld.Obra.Numero, propostaOld.Obra.UsinaCodigo, statusFinanceiroAnterior);

                    _propostaService.AtualizarStatusProposta(propostaOld, usuario);

                    _obraService.AtualizarEnderecoProgramacoesFuturas(propostaOld.UsinaCodigo, propostaOld.Obra.Numero);

                    _obraService.AtualizarValoresProgramacoesFuturas(propostaOld.UsinaCodigo, propostaOld.Obra.Numero);                       
                }
            }
        }

        private void SalvarIntervenienteLocal(int? intervenienteCodigo, IDadosPessoais dados, out int intervenienteLocalSequencia, string usuario, Expression<Func<IntervenienteLocal, bool>> filtroAdicional = null)
        {
            var localOld = _intervenienteService.ObterLocalPorIntervenienteEDadosPessoais(intervenienteCodigo ?? 0, dados, filtroAdicional);
            if (localOld != null)
            {
                localOld = AutoMapper.Mapper.Map(dados, localOld);
                Commit();

                intervenienteLocalSequencia = localOld.Sequencia;
            }
            else
            {
                var maxSeq = _intervenienteService.ListarFiltrados<IntervenienteLocal>(t =>
                    t.IntervenienteCodigo == intervenienteCodigo
                ).Select(t => t.Sequencia).DefaultIfEmpty().Max();

                var localNew = AutoMapper.Mapper.Map(dados, new IntervenienteLocal
                {
                    IntervenienteCodigo = intervenienteCodigo ?? 0,
                    Sequencia = maxSeq + 1,
                    IdCadastro = StringHelper.GetIDD(usuario),
                    IdAtualizacao = ""
                });

                _intervenienteService.Adicionar(localNew);
                Commit();

                intervenienteLocalSequencia = localNew.Sequencia;
            }
        }

        private bool HouveAlteracao(string forma, ObraPagamentoDetalhe pagamentoDetalhe, DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO pagamentoDetalheDto)
        {
            var temContrato = pagamentoDetalhe.ContratoNumero > 0;

            switch (forma)
            {
                case "CC":
                case "CD":
                    var cartaoOld = (IObraPagamentoDetalheCartao)pagamentoDetalhe;
                    return (cartaoOld.BandeiraCodigo != (pagamentoDetalheDto.Bandeira?.Codigo ?? 0))
                        || (cartaoOld.DataTransacao != pagamentoDetalheDto.DataTransacao)
                        || (cartaoOld.NumeroAutorizacao != pagamentoDetalheDto.NumeroAutorizacao)
                        || (cartaoOld.NumeroCartao != pagamentoDetalheDto.NumeroCartao)
                        || (cartaoOld.QuantidadeParcelas != pagamentoDetalheDto.QuantidadeParcelas)
                        || (cartaoOld.Valor != pagamentoDetalheDto.Valor);
                case "DP":
                    var depositoOld = (IObraPagamentoDetalheDeposito)pagamentoDetalhe;
                    return (depositoOld.DataDeposito != pagamentoDetalheDto.DataDeposito)
                        || (depositoOld.IdAprovacao != pagamentoDetalheDto.IdAprovacao)
                        || (depositoOld.NumeroTerminal != pagamentoDetalheDto.NumeroTerminal)
                        || (depositoOld.PortadorCodigo != (pagamentoDetalheDto.Portador?.Codigo ?? 0))
                        || (depositoOld.TomadorAgencia != (pagamentoDetalheDto.Portador?.Conta?.NumeroAgencia ?? 0).ToString())
                        || (depositoOld.TomadorBanco != (pagamentoDetalheDto.Portador?.Conta?.BancoCodigoOficial ?? 0))
                        || (depositoOld.TomadorNumeroConta != $"{(pagamentoDetalheDto.Portador?.Conta?.NumeroConta ?? 0)}-{(pagamentoDetalheDto.Portador?.Conta?.DvConta ?? "")}")
                        || (depositoOld.Valor != pagamentoDetalheDto.Valor);
                case "CH":
                case "CP":
                    var chequeOld = (IObraPagamentoDetalheCheque)pagamentoDetalhe;
                    return (chequeOld.BancoCodigoOficial != pagamentoDetalheDto.BancoCodigoOficial)
                        || (chequeOld.BancoAgencia != pagamentoDetalheDto.BancoAgencia)
                        || (chequeOld.BancoContaNumero != pagamentoDetalheDto.BancoContaNumero)
                        || (chequeOld.BancoContaDV != pagamentoDetalheDto.BancoContaDV)
                        || (chequeOld.NumeroCheque != pagamentoDetalheDto.NumeroCheque)
                        || (chequeOld.DataRecebimento != pagamentoDetalheDto.DataRecebimento)
                        || (chequeOld.DataBomPara != pagamentoDetalheDto.DataBomPara)
                        || (chequeOld.Observacao != pagamentoDetalheDto.Observacao)
                        || (chequeOld.Valor != pagamentoDetalheDto.Valor);
                case "BE":
                    var boletoOld = (IObraPagamentoDetalheBoleto)pagamentoDetalhe;
                    return (boletoOld.DataVencimento != pagamentoDetalheDto.DataVencimento)
                        || (boletoOld.DataHoraImpressao != pagamentoDetalheDto.DataHoraImpressao)
                        || (boletoOld.NossoNumero != pagamentoDetalheDto.NossoNumero)
                        || (boletoOld.LinhaDigitavel != pagamentoDetalheDto.LinhaDigitavel)
                        || (boletoOld.CodigoDeBarras != pagamentoDetalheDto.CodigoDeBarras)
                        || (boletoOld.DataRemessa != pagamentoDetalheDto.DataRemessa)
                        || (boletoOld.DataLiquidacao != pagamentoDetalheDto.DataLiquidacao)
                        || (boletoOld.ValorLiquidacao != pagamentoDetalheDto.ValorLiquidacao)
                        || (boletoOld.IdLiquidacao != pagamentoDetalheDto.IdLiquidacao)
                        || (boletoOld.Valor != pagamentoDetalheDto.Valor);
                case "DN":
                    var dinheiroOld = (IObraPagamentoDetalheDinheiro)pagamentoDetalhe;
                    return (dinheiroOld.NumeroRecibo != pagamentoDetalheDto.NumeroRecibo)
                        || (dinheiroOld.DataPagamento != pagamentoDetalheDto.DataPagamento)
                        || (dinheiroOld.Valor != pagamentoDetalheDto.Valor);
                default:
                    return false;
            }
        }

        public PagedList<PropostaSimplesResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false)
        {
            var propostas = _propostaService.ListarEmOrdemDecrescente(pagina, porPagina, filter, divergenciaCarteira, statusClicksignDocumento, propostaComContrato);
            foreach(var proposta in propostas.Records)
            {
                if (proposta.Obra?.NumContrato == null || proposta.Obra?.AnoContrato == null) continue;
                var numContrato = proposta.Obra.NumContrato ?? 0;
                var anoContrato = proposta.Obra.AnoContrato ?? 0;
                var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(proposta.Obra.UsinaCodigo, anoContrato, numContrato);
                if (versaoAtual == 0) continue;

                var propostaVersao = _propostaService.ObterVersaoPorIdForList(proposta.UsinaCodigo, proposta.Ano, proposta.Numero, versaoAtual);
                AutoMapper.Mapper.Map(propostaVersao, proposta);
            }
            return AutoMapper.Mapper.Map(propostas, new PagedList<PropostaSimplesResponse>());
        }

        public PagedList<PropostaImportacaoSimplesResponse> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina)
        {
            var propostas = _propostaService.ListarPorCpfCnpj(cpfCnpj, pagina, porPagina);
            foreach (var proposta in propostas.Records)
            {
                if (proposta.Obra?.NumContrato == null || proposta.Obra?.AnoContrato == null) continue;
                var numContrato = proposta.Obra.NumContrato ?? 0;
                var anoContrato = proposta.Obra.AnoContrato ?? 0;
                var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(proposta.Obra.UsinaCodigo, anoContrato, numContrato);
                if (versaoAtual == 0) continue;

                var propostaVersao = _propostaService.ObterVersaoPorIdForList(proposta.UsinaCodigo, proposta.Ano, proposta.Numero, versaoAtual);
                AutoMapper.Mapper.Map(propostaVersao, proposta);
            }
            return AutoMapper.Mapper.Map(propostas, new PagedList<PropostaImportacaoSimplesResponse>());
        }

        public PropostaDetalhadaResponse ObterPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var propostas = _propostaService.ObterPorUsinaAnoNumero(idUsina, ano, numero);

            if (propostas.Obra?.NumContrato != null && propostas.Obra?.AnoContrato != null)
            {
                var numContrato = propostas.Obra.NumContrato ?? 0;
                var anoContrato = propostas.Obra.AnoContrato ?? 0;
                var versaoAtual = _contratoService.GetUltimaVersaoContratoAberta(propostas.Obra.UsinaCodigo, anoContrato, numContrato);
                if (versaoAtual != 0)
                {
                    var propostaVersao = _propostaService.ObterPorUsinaAnoNumero(versaoAtual, idUsina, ano, numero);
                    AutoMapper.Mapper.Map(propostaVersao, propostas);
                    propostas = MapObraPagamentosDetalhesVersaoParaObraPagamentos(propostaVersao, propostas);
                }
                
                if(propostas.Obra.Indicador == null)
                {
                    var indicador = new ObraIndicador()
                    {
                        ObraUsina = propostas.Obra.UsinaCodigo,
                        ObraNumero = propostas.Obra.Numero
                    };
                    propostas.Obra.Indicador = new ObraIndicador();
                }

            }

            return AutoMapper.Mapper.Map(propostas, new PropostaDetalhadaResponse());
        }

        public float ObterVolumeTotalProposto(int idUsina, int ano, int numero)
        {
            return _propostaService.ObterVolumeTotalProposto(idUsina, ano, numero);
        }        

        public void ExcluirVersaoContrato(PropostaAlteracaoRequest propostaRequest, string configuracao)
        {
            if (propostaRequest.StatusContrato == EContratoStatus.Aprovado)
            {
                var numVersao = _contratoService.GetUltimaVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value);
                string[] parametro = configuracao.Split(';');

                if (parametro[0].ToString() == "true")  //Traço
                {
                    if (HouveAlteracaoTraco(propostaRequest))
                        return;
                }
                if (parametro[1].ToString() == "true")  //Bomba
                {
                    if (HouveAlteracaoBomba(propostaRequest))
                        return;
                }
                if (parametro[2].ToString() == "true")  //Taxa Extra
                {
                    if (HouveAlteracaoTaxaExtra(propostaRequest))
                        return;
                }
                if (parametro[3].ToString() == "true")  //Condição de Pagamento
                {
                    if (HouveAlteracaoCondicaoPagamento(propostaRequest))
                        return;
                }
                if (parametro[4].ToString() == "true")  //Endereço da Obra
                {
                    if (HouveAlteracaoEnderecoObra(propostaRequest))
                        return;
                }
                if (parametro[5].ToString() == "true")  //Demais Serviços
                {
                    if (HouveAlteracaoDemaisServicos(propostaRequest))
                        return;
                }
                if (parametro[6].ToString() == "true")  //Reajuste Contrato
                {
                    return;
                }

                _propostaService.ExcluirVersaoContrato(propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, numVersao);
                _contratoService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numVersao);
                _obraService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.AnoContrato.Value, propostaRequest.Obra.NumContrato.Value, numVersao, propostaRequest.Usina.Codigo, propostaRequest.Ano, propostaRequest.Numero, propostaRequest.Obra.Numero);
                _obraTaxaService.ExcluirVersaoContrato(propostaRequest.Obra.UsinaEntrega.Codigo, numVersao, propostaRequest.Obra.Numero);
                _demaisServicosService.ExcluirVersaoContrato(propostaRequest.Usina.Codigo, numVersao, propostaRequest.Obra.Numero);
            }          
        }

        public Boolean HouveAlteracaoEnderecoObra(PropostaAlteracaoRequest propostaRequest)
        {
            var obraOld = _obraService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            if (propostaRequest.Obra.Endereco.Logradouro != obraOld.EnderecoLogradouro ||
                propostaRequest.Obra.Endereco.Numero != obraOld.EnderecoNumero ||
                propostaRequest.Obra.Endereco.Cep != obraOld.EnderecoCep ||
                propostaRequest.Obra.Endereco.Complemento != obraOld.EnderecoComplemento ||
                propostaRequest.Obra.Endereco.Bairro != obraOld.EnderecoBairro ||
                propostaRequest.Obra.Endereco.Municipio.Codigo != obraOld.EnderecoMunicipioCodigo)
            {
                return true;
            } else return false;
        }

        public Boolean HouveAlteracaoObraFrente(PropostaAlteracaoRequest propostaRequest)
        {

            List<int> sequencias = new List<int>();
            foreach(var eDto in propostaRequest.Obra.ObraFrentes)
            {
                sequencias.Add(eDto.ObraSequencia);

                var frenteOld = _obraFrenteService.ObterPorObra(eDto.UsinaCodigo, eDto.ObraCodigo, eDto.ObraSequencia);

                if (HouveAlteracaoObraFrente(eDto, frenteOld))
                    return true;

            }

            var seqs = sequencias.ToArray();
            var tracosExcluidos = _obraService.ListarFiltradosTracking<ObraFrente>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.ObraSequencia));

            return (tracosExcluidos.Count() > 0);

        }

        public Boolean HouveAlteracaoObraFrente(ObraFrenteDTO eDto, ObraFrente frenteOld)
        {

            if (frenteOld == null)
                return true;

            var houveAlteracao = false;

            houveAlteracao = houveAlteracao || eDto.EnderecoBairro != frenteOld.EnderecoBairro;
            houveAlteracao = houveAlteracao || eDto.EnderecoCep != frenteOld.EnderecoCep;
            houveAlteracao = houveAlteracao || eDto.EnderecoComplemento != frenteOld.EnderecoComplemento;
            houveAlteracao = houveAlteracao || eDto.EnderecoLogradouro != frenteOld.EnderecoLogradouro;
            houveAlteracao = houveAlteracao || eDto.EnderecoNome != frenteOld.EnderecoNome;
            houveAlteracao = houveAlteracao || eDto.EnderecoNumero != frenteOld.EnderecoNumero;

            return houveAlteracao;

        }

        public Boolean HouveAlteracaoTraco(PropostaAlteracaoRequest propostaRequest)
        {
            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraTracos)
            {
                sequencias.Add(tDto.Sequencia);
                var tracoOld = _obraService.ListarFiltradosTracking<ObraTraco>(t => t.UsinaCodigo == tDto.Usina.Codigo && t.ObraCodigo == tDto.ObraCodigo && t.Sequencia == tDto.Sequencia,
                    t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                    .FirstOrDefault();

                if (tracoOld != null)
                {            
                    if (tDto.M3PrecoProposto != tracoOld.M3PrecoProposto || tDto.Uso.Codigo != tracoOld.UsoCodigo
                        || tDto.Pedra.Codigo != tracoOld.PedraCodigo || tDto.SlumpNominal.Codigo != tracoOld.SlumpNominalCodigo
                        || tDto.Slump.Codigo != tracoOld.SlumpCodigo || tDto.ResistenciaTipo.Codigo != tracoOld.ResistenciaTipoCodigo
                        || tDto.Mpa != tracoOld.Fck || tDto.Consumo != tracoOld.Consumo || tDto.M3Quantidade != tracoOld.M3Quantidade
                        || tDto.M3QuantidadeBombeada != tracoOld.M3QuantidadeBombeada || tDto.Observacao != tracoOld.Observacao
                        || tDto.PecaConcretar != tracoOld.PecaConcretar || tDto.Justificativa != tracoOld.Justificativa
                        || tDto.PrecoConcorrencia != tracoOld.PrecoConcorrencia || tDto.PrecoReajustadoAtual != tracoOld.PrecoReajustadoAtual
                        || tDto.Ativo != tracoOld.Ativo) 
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            var seqs = sequencias.ToArray();
            var tracosExcluidos = _obraService.ListarFiltradosTracking<ObraTraco>
                (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia)).FirstOrDefault();

            if (tracosExcluidos != null)
            {
                return true;
            }

            return false;
        }

        public Boolean HouveAlteracaoBomba(PropostaAlteracaoRequest propostaRequest)
        {
            List<int> sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraBombas)
            {
                sequencias.Add(tDto.Sequencia);
                var bombaOld = _obraService.ObterPorId<ObraBomba>
                        (tDto.UsinaCodigo, tDto.ObraCodigo, tDto.Sequencia);

                if (bombaOld != null)
                {
                    if (tDto.BombaPropria != bombaOld.BombaPropria || (tDto.BombaTipo?.Codigo ?? 0) != bombaOld.BombaTipoCodigo
                        || tDto.TaxaMinimaPrecoProposto != bombaOld.TaxaMinimaPrecoProposto || tDto.M3PrecoProposto != bombaOld.M3PrecoProposto
                        || tDto.M3PropostoAte != bombaOld.M3PropostoAte || tDto.Justificativa != bombaOld.Justificativa
                        || tDto.DistanciaTubulacao != bombaOld.DistanciaTubulacao || tDto.TaxaMinimaReajustadaAtual != bombaOld.TaxaMinimaReajustadaAtual
                        || tDto.M3PrecoReajustadoAtual != bombaOld.M3PrecoReajustadoAtual || tDto.M3ReajustadoAteAtual != bombaOld.M3ReajustadoAteAtual)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            var seqs = sequencias.ToArray();
            var bombasExcluidas = _obraService.ListarFiltradosTracking<ObraBomba>
                    (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                    && t.ObraCodigo == propostaRequest.Obra.Numero
                    && !seqs.Contains(t.Sequencia)).FirstOrDefault();

            if (bombasExcluidas != null)
            {
                return true;
            }

            return false;
        }

        public Boolean HouveAlteracaoTaxaExtra(PropostaAlteracaoRequest propostaRequest)
        {
            var taxasAnteriores = _obraTaxaRepository.ListarByIdObra(propostaRequest.Obra.UsinaEntrega.Codigo, propostaRequest.Obra.Numero, propostaRequest.Segmentacao);
            var obraOld = _obraService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);

            if(obraOld != null)
            {
                // Código para Mudança de Usina
                var usinaEntregaOld = obraOld.UsinaEntregaCodigo;
                var usinaEntregaRequest = propostaRequest.Obra.UsinaEntrega.Codigo;

                var usinasValidas = usinaEntregaOld > 0 && usinaEntregaRequest > 0;
                var usinasDiferentes = usinaEntregaOld != usinaEntregaRequest;

                if (usinasValidas && usinasDiferentes)
                    return true;

            }
            
            foreach (var tDto in propostaRequest.Obra.ObraTaxas)
            {
                var taxaOld = taxasAnteriores.FirstOrDefault(t => t.Sequencia == tDto.Sequencia);

                if (taxaOld != null)
                {
                    if (tDto.Selecionada != taxaOld.Selecionada)
                    {
                        return true;
                    }
                    if (tDto.IsPersonalizada && tDto.Selecionada == "S")
                    {
                        if (tDto.Descricao != taxaOld.Descricao)
                        {
                            return true;
                        }
                    }
                }
                else if (tDto.Selecionada == "S" && tDto.Nova)
                {
                    return true;
                }
            }            

            return false;
        }

        public Boolean HouveAlteracaoDemaisServicos(PropostaAlteracaoRequest propostaRequest)
        {
            List<int> sequencias = new List<int>();

            var obraOld = _obraService.ObterPorId(propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero);
            if (propostaRequest.Obra.VibradorQuantidade != obraOld.VibradorQuantidade || propostaRequest.Obra.VibradorValorUnitario != obraOld.VibradorValorUnitario)
            {
                return true;
            }

            foreach (var tDto in propostaRequest.Obra.ObraDemaisServicos)
            {
                sequencias.Add(tDto.Sequencia);
                var demaisServicosOld = _obraService.ObterPorId<ObraDemaisServicos>
                        (propostaRequest.Obra.UsinaCodigo, propostaRequest.Obra.Numero, tDto.Sequencia);

                if (demaisServicosOld != null)
                {
                    if (tDto.Mercadoria.Codigo != demaisServicosOld.MercadoriaCodigo || tDto.PrecoProposto != demaisServicosOld.PrecoProposto
                        || tDto.Quantidade != demaisServicosOld.Quantidade || tDto.AtualizaEstoque != demaisServicosOld.AtualizaEstoque)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            var seqs = sequencias.ToArray();
            var demaisServicosExcluidos = _obraService.ListarFiltradosTracking<ObraDemaisServicos>
                    (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                        && t.ObraNumero == propostaRequest.Obra.Numero
                        && !seqs.Contains(t.Sequencia)).FirstOrDefault();

            if (demaisServicosExcluidos != null)
            {
                return true;
            }

            return false;
        }

        public Boolean HouveAlteracaoCondicaoPagamento(PropostaAlteracaoRequest propostaRequest)
        {
            var sequencias = new List<int>();
            foreach (var tDto in propostaRequest.Obra.ObraPagamentos)
            {
                sequencias.Add(tDto.Sequencia);
                var contratoPagamentoOld = _obraService.ObterPorId<ContratoPagamentoForSaving>
                            (propostaRequest.Usina.Codigo, propostaRequest.Obra.AnoContrato, propostaRequest.Obra.NumContrato, tDto.Sequencia);
                              
                if (contratoPagamentoOld != null)
                {
                    if (contratoPagamentoOld.CondicaoPagamentoCodigo != tDto.CondicaoPagamento.Codigo || contratoPagamentoOld.Forma != tDto.TipoCobranca.Forma
                        || contratoPagamentoOld.Valor != tDto.Valor) { 
                        return true;
                    }
                }
                else
                {
                    return true;
                }

                
            }
            var seqs = sequencias.ToArray();
            var contratoPagamentosExcluidos = _obraService.ListarFiltradosTracking<ContratoPagamentoForSaving>
                    (t => t.UsinaCodigo == propostaRequest.Usina.Codigo
                        && t.ContratoAno == (propostaRequest.Obra.AnoContrato ?? 0)
                        && t.ContratoNumero == (propostaRequest.Obra.NumContrato ?? 0)
                        && !seqs.Contains(t.Sequencia)).FirstOrDefault();

            if (contratoPagamentosExcluidos != null) 
            {
                return true;
            }

            return false;              
        }

        public bool HouveAlteracaoInterveniente(Interveniente intervenienteOld, PropostaInclusaoRequest propostaRequest)
        {
            return propostaRequest.IntervenienteNome != intervenienteOld.Nome
                    || propostaRequest.IntervenienteRazao != intervenienteOld.Razao
                    || propostaRequest.TelefoneComercialDdd != intervenienteOld.TelefoneComercialDdd
                    || propostaRequest.TelefoneComercialNumero != intervenienteOld.TelefoneComercialNumero
                    || propostaRequest.TelefoneDdd != intervenienteOld.TelefoneDdd
                    || propostaRequest.TelefoneNumero != intervenienteOld.TelefoneNumero
                    || propostaRequest.CpfCnpj != intervenienteOld.CpfCnpj
                    || propostaRequest.Endereco.Logradouro != intervenienteOld.EnderecoLogradouro
                    || propostaRequest.Endereco.Numero != intervenienteOld.EnderecoNumero
                    || propostaRequest.Endereco.Complemento != intervenienteOld.EnderecoComplemento
                    || propostaRequest.Endereco.Bairro != intervenienteOld.EnderecoBairro
                    || propostaRequest.Endereco.Cep != intervenienteOld.EnderecoCep
                    || propostaRequest.Endereco.Municipio.Codigo != intervenienteOld.EnderecoMunicipioCodigo
                    || propostaRequest.Email != intervenienteOld.Email
                    || propostaRequest.Interveniente.IdExterno != intervenienteOld.IdExterno;
        }

        public bool HouveAlteracaoInterveniente(Interveniente intervenienteOld, PropostaAlteracaoRequest propostaRequest)
        {
            return propostaRequest.IntervenienteNome != intervenienteOld.Nome
                    || propostaRequest.IntervenienteRazao != intervenienteOld.Razao
                    || propostaRequest.TelefoneComercialDdd != intervenienteOld.TelefoneComercialDdd
                    || propostaRequest.TelefoneComercialNumero != intervenienteOld.TelefoneComercialNumero
                    || propostaRequest.TelefoneDdd != intervenienteOld.TelefoneDdd
                    || propostaRequest.TelefoneNumero != intervenienteOld.TelefoneNumero
                    || propostaRequest.CpfCnpj != intervenienteOld.CpfCnpj
                    || propostaRequest.Endereco.Logradouro != intervenienteOld.EnderecoLogradouro
                    || propostaRequest.Endereco.Numero != intervenienteOld.EnderecoNumero
                    || propostaRequest.Endereco.Complemento != intervenienteOld.EnderecoComplemento
                    || propostaRequest.Endereco.Bairro != intervenienteOld.EnderecoBairro
                    || propostaRequest.Endereco.Cep != intervenienteOld.EnderecoCep
                    || propostaRequest.Endereco.Municipio.Codigo != intervenienteOld.EnderecoMunicipioCodigo
                    || propostaRequest.Email != intervenienteOld.Email
                    || propostaRequest.Interveniente.IdExterno != intervenienteOld.IdExterno
                    || propostaRequest.EmailCobranca != intervenienteOld.EmailCobranca;
        }

        public Proposta MapObraPagamentosDetalhesVersaoParaObraPagamentos(PropostaVersao propostaVersao, Proposta proposta)
        {
            if (propostaVersao.Obra.ContratoPagamentos != null && propostaVersao.Obra.ContratoPagamentos.Any(cp => cp.Detalhes != null))
            {
                foreach (var contratoPagamentoVersao in propostaVersao.Obra.ContratoPagamentos)
                {
                    if (contratoPagamentoVersao.Detalhes != null)
                    {
                        var contratoPagamentos = proposta.Obra.ContratoPagamentos
                            .FirstOrDefault(cp => cp.UsinaCodigo == contratoPagamentoVersao.UsinaCodigo && cp.ContratoAno == contratoPagamentoVersao.ContratoAno
                                            && cp.ContratoNumero == contratoPagamentoVersao.ContratoNumero && cp.Sequencia == contratoPagamentoVersao.Sequencia);

                        if (contratoPagamentos.Detalhes == null)
                        {
                            contratoPagamentos.Detalhes = new List<ContratoPagamentoDetalhe>();
                        }

                        foreach (var detalheVersao in contratoPagamentoVersao.Detalhes)
                        {

                            switch (contratoPagamentoVersao.TipoCobranca?.Forma?.ToUpper())
                            {
                                case "CC":
                                case "CD":
                                    contratoPagamentos.Detalhes.Add(AutoMapper.Mapper.Map<ContratoPagamentoDetalheCartao>(detalheVersao));
                                    break;
                                case "CH":
                                case "CP":
                                    contratoPagamentos.Detalhes.Add(AutoMapper.Mapper.Map<ContratoPagamentoDetalheCheque>(detalheVersao));
                                    break;
                                case "DP":
                                    contratoPagamentos.Detalhes.Add(AutoMapper.Mapper.Map<ContratoPagamentoDetalheDeposito>(detalheVersao));
                                    break;
                                case "DN":
                                    contratoPagamentos.Detalhes.Add(AutoMapper.Mapper.Map<ContratoPagamentoDetalheDinheiro>(detalheVersao));
                                    break;
                                case "BE":
                                    contratoPagamentos.Detalhes.Add(AutoMapper.Mapper.Map<ContratoPagamentoDetalheBoleto>(detalheVersao));
                                    break;
                                default:
                                    break;
                            }
                        };
                    }
                }
            }
            return proposta;
        }
    }
}

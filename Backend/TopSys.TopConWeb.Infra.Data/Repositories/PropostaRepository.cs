using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class PropostaRepository : RepositoryBase<Proposta>, IPropostaRepository
    {
        private IObraTaxaRepository _obraTaxaRepository;
        private IContratoPagamentoRepository _contratoPagamentoRepository;
        private IObraFrenteRepository _obraFrenteRepository;
        private IDatabaseRepository _databaseRepository;
        private readonly IdentityHelperService _identityHelperService;
        private IClicksignRepository _clicksignRepository;
        private IObraProjecaoRepository _obraProjecaoRepository;
        private IObraRepository _obraRepository;
        private IIntervenienteRepository _intervenienteRepository;

        public PropostaRepository(AppDataContext context, IObraTaxaRepository obraTaxaRepository, IContratoPagamentoRepository contratoPagamentoRepository, IObraFrenteRepository obraFrenteRepository, IDatabaseRepository databaseRepository, IdentityHelperService identityHelperService, IClicksignRepository clicksignRepository, IObraProjecaoRepository obraProjecaoRepository, IObraRepository obraRepository, IIntervenienteRepository intervenienteRepository) : base(context)
        {
            _context = context;
            _obraTaxaRepository = obraTaxaRepository;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _identityHelperService = identityHelperService;
            _clicksignRepository = clicksignRepository;
            _obraFrenteRepository = obraFrenteRepository;
            _obraProjecaoRepository = obraProjecaoRepository;
            _obraRepository = obraRepository;
            _databaseRepository = databaseRepository;
            _intervenienteRepository = intervenienteRepository;
        }

        new public void Adicionar(Proposta proposta)
        {
            var sqlComando = proposta.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel", sqlComando.ToString());

            proposta.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_PROPOSTA_INSERIDA").FirstOrDefault();
        }

        public PagedList<Proposta> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false)
        {
            var saldoContratado = 0.0f;
            var saldoProjetado = 0.0f;
            var saldoConsumido = 0.0f; 
            var query = _context.Propostas
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.Interveniente)
                .Include(t => t.Interveniente.BloqueioMotivo)
                .Include(t => t.Interveniente.GrupoEconomico)
                .Include(t => t.Obra)
                .Include(t => t.Obra.Contrato)
                .Include(t => t.Obra.TipoCobranca)
                .Include(t => t.Obra.ObraReajuste)
                .Include(t => t.Obra.Indicador)
                .Include(t => t.ResponsavelSolidario)
                .Include(t => t.Segmento)
                .Where(filter);

            if (propostaComContrato)
                query = query.Where(t => t.Obra.Contrato != null);

            var pagedList = query
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .AsNoTracking()
                .PagedList(pagina, porPagina, t => t.Obra.Nome != null); // cláusula para forçar o inner join com Obra no Count()


            foreach (var record in pagedList.Records)
            {
                if (record?.Obra?.Contrato != null)
                    record.Obra.Contrato.ClicksignEnvio = _clicksignRepository.ListarContratoClicksignEnvios(record.Obra.UsinaCodigo, record?.Obra?.AnoContrato ?? 0, record?.Obra?.NumContrato ?? 0);

                saldoProjetado = (float)(_obraProjecaoRepository.ObterPrevisaoSaldoProjecaoPorContrato(record.Obra.UsinaCodigo, record.Obra.Numero, record.Obra.AnoChamada, record.Obra.NumChamada) ?? 0);
                saldoContratado = (float)(_obraRepository.ObterVolumePorContrato(record.Obra.UsinaCodigo, record.Obra.Numero, (int)record.Obra.AnoChamada, (int)record.Obra.NumChamada) ?? 0);
                saldoConsumido = 0;
                if (record?.Obra?.Contrato != null)
                    saldoConsumido= (float)(_obraRepository.ObterConsumoPorContrato(record.Obra.Contrato.Usina,record.Obra.Contrato.Numero,record.Obra.Contrato.Ano) ?? 0);

                saldoContratado = saldoContratado - saldoConsumido;

                switch (saldoProjetado)
                {
                    case 0:
                        record.Obra.StatusProjecao = EStatusProjecao.NaoPossui;
                        break;
                    case var _ when saldoProjetado == saldoContratado:
                        record.Obra.StatusProjecao = EStatusProjecao.Igual;
                        break;
                    default:
                        record.Obra.StatusProjecao = EStatusProjecao.Divergente;
                        break;
                }

                if (record.Vendedor != null && record.Vendedor.Interveniente != 0)
                {
                    var vendedorInterv = _intervenienteRepository.ObterPorCodigo(record.Vendedor.Interveniente);
                    if(vendedorInterv != null)
                        record.Vendedor.CpfCnpj = vendedorInterv.CpfCnpj;
                }
            }

            if (divergenciaCarteira) {
                pagedList.Records = pagedList.Records.Where(x => x.Obra.StatusProjecao == EStatusProjecao.Divergente);
            }

            if (statusClicksignDocumento != null)
                pagedList.Records = pagedList.Records.Where(x => x.Obra.Contrato.StatusClicksignDocumento == statusClicksignDocumento);

            pagedList.RecordCount = pagedList.Records.Count();
            return pagedList;
        }        

        public PagedList<Proposta> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina)
        {
            var pagedList = _context.Propostas
                .OrderByDescending(t => new { t.Ano, t.Numero })
                .Include(t => t.Usina)
                .Include(t => t.Obra)
                .Include(t => t.Obra.UsinaEntrega)
                .Include(t => t.Obra.CondicaoPagamento)
                .Include(t => t.Obra.TipoCobranca)
                .Include(t => t.Obra.UsinaEntrega)
                .Include(t => t.Obra.EnderecoMunicipio)
                .Where(t => t.CpfCnpj == cpfCnpj)
                .PagedList(pagina, porPagina, t => t.Obra.Nome != null); // cláusula para forçar o inner join com Obra no Count()

            foreach (var record in pagedList.Records)
            {
                if (record?.Obra?.Contrato != null)
                    record.Obra.Contrato.ClicksignEnvio = _clicksignRepository.ListarContratoClicksignEnvios(record.Obra.UsinaCodigo, record?.Obra?.AnoContrato ?? 0, record?.Obra?.NumContrato ?? 0);
            }

            return pagedList;
        }

        public Proposta ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false)
        {
            var propostaResult = _context.Propostas
                .Where(t => t.UsinaCodigo == idUsina && t.Ano == ano && t.Numero == numero)
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.VendedorPadrinho)
                .Include(t => t.Interveniente)
                .Include(t => t.Interveniente.GrupoEconomico)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.Faturamento.EnderecoMunicipio)
                .Include(t => t.Cobranca.EnderecoMunicipio)
                .Include(t => t.ResponsavelSolidario)
                .Include(c => c.Segmento)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.ResponsavelSolidario = _context.PropostaResponsaveisSolidarios
                .Where(t => t.UsinaCodigo == idUsina && t.PropostaAno == ano && t.PropostaNumero == numero)
                .Include(t => t.EnderecoMunicipio)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.Obra = _context.Obras
                .Where(t => t.UsinaCodigo == idUsina && t.AnoChamada == ano && t.NumChamada == numero)
                .Include(c => c.ViaCaptacao)
                .Include(c => c.ViaCaptacao.ViaCaptacao)
                .Include(c => c.TipoObra)
                .Include(c => c.PorteObra)
                .Include(c => c.Contrato)
                .Include(c => c.CondicaoPagamento)
                .Include(c => c.TipoCobranca)
                .Include(c => c.EnderecoMunicipio)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.ContatoPrincipalFuncao)
                .Include(c => c.ContatoSecundarioFuncao)
                .Include(c => c.ObraReajuste)
                .Include(c => c.Indicador)
                .Include(c => c.Indicador.Vendedor)
                .Include(c => c.Indicador.Interveniente)
                .Include(c => c.TributacaoPisCofins)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.Obra.TributacaoCBS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoCBSCodigo };
            propostaResult.Obra.TributacaoIBS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoIBSCodigo };
            propostaResult.Obra.TributacaoIS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoISCodigo };

            if (propostaResult.Obra != null)
            {
                propostaResult.Obra.ObraTracos = _context.ObraTracos
                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Pedra)
                .Include(t => t.Slump)
                .Include(t => t.SlumpNominal)
                .Include(t => t.ResistenciaTipo)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraBombas = _context.ObraBombas
                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.BombaTipo)
                .Include(t => t.Terceiro)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraTaxas = _obraTaxaRepository.ListarByIdObra(propostaResult.Obra.UsinaEntregaCodigo, propostaResult.Obra.Numero, propostaResult.Segmentacao);

                propostaResult.Obra.ObraFrentes = _context.ObraFrente
                                                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                                                .Tracking(tracking)
                                                .ToList();

                propostaResult.Obra.ObraLogs = _context.ObraLogs
                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraTributacoesMunicipais = _context.ObraTributacoesMunicipais
                .Where(t => t.ObraUsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraNumero == propostaResult.Obra.Numero)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraDemaisServicos = _context.ObraDemaisServicos
                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraNumero == propostaResult.Obra.Numero)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Mercadoria)
                .Include(t => t.Unidade)
                .Tracking(tracking)
                .ToList();
            }
            
            if (propostaResult.Obra?.NumContrato > 0)
            {
                var localFaturamento = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.LocalFaturamento ?? 0);

                if (localFaturamento != null)
                {
                    propostaResult.Faturamento = new PropostaFaturamento {
                        UsinaCodigo = idUsina,
                        PropostaAno = ano,
                        PropostaNumero = numero,
                        IntervenienteTipo = (localFaturamento.CpfCnpj.Length == 14 ? "J" : "F"),
                        CpfCnpj = localFaturamento.CpfCnpj,
                        InscricaoEstadual = localFaturamento.InscricaoEstadual,
                        InscricaoMunicipal = localFaturamento.InscricaoMunicipal,
                        Nome = localFaturamento.Nome,
                        OrgaoExpedidor = localFaturamento.OrgaoExpedidor,
                        Razao = localFaturamento.Razao,
                        Rg = localFaturamento.Rg,
                        EnderecoBairro = localFaturamento.EnderecoBairro,
                        EnderecoCep = localFaturamento.EnderecoCep,
                        EnderecoComplemento = localFaturamento.EnderecoComplemento,
                        EnderecoLogradouro = localFaturamento.EnderecoLogradouro,
                        EnderecoMunicipioCodigo = localFaturamento.EnderecoMunicipioCodigo ?? 0,
                        EnderecoNumero = localFaturamento.EnderecoNumero,
                        Email = localFaturamento.Email
                    };

                    if ((localFaturamento.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.Faturamento.EnderecoMunicipio = ObterPorId<Municipio>(localFaturamento.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.Faturamento.EnderecoMunicipio = null;
                    }
                }

                var localCobranca = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.LocalCobranca ?? 0);

                if (localCobranca != null)
                {
                    propostaResult.Cobranca = new PropostaCobranca
                    {
                        UsinaCodigo = idUsina,
                        PropostaAno = ano,
                        PropostaNumero = numero,
                        IntervenienteTipo = (localCobranca.CpfCnpj.Length == 14 ? "J" : "F"),
                        CpfCnpj = localCobranca.CpfCnpj,
                        InscricaoEstadual = localCobranca.InscricaoEstadual,
                        InscricaoMunicipal = localCobranca.InscricaoMunicipal,
                        Nome = localCobranca.Nome,
                        OrgaoExpedidor = localCobranca.OrgaoExpedidor,
                        Razao = localCobranca.Razao,
                        Rg = localCobranca.Rg,
                        EnderecoBairro = localCobranca.EnderecoBairro,
                        EnderecoCep = localCobranca.EnderecoCep,
                        EnderecoComplemento = localCobranca.EnderecoComplemento,
                        EnderecoLogradouro = localCobranca.EnderecoLogradouro,
                        EnderecoMunicipioCodigo = localCobranca.EnderecoMunicipioCodigo ?? 0,
                        EnderecoNumero = localCobranca.EnderecoNumero,
                        Email = localCobranca.Email
                    };

                    if ((localCobranca.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.Cobranca.EnderecoMunicipio = ObterPorId<Municipio>(localCobranca.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.Cobranca.EnderecoMunicipio = null;
                    }
                }

                var localResponsavelSolidario = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.ResponsavelSolidario ?? 0);

                if (localResponsavelSolidario != null)
                {
                    if ((localResponsavelSolidario.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.ResponsavelSolidario.EnderecoMunicipio = ObterPorId<Municipio>(localResponsavelSolidario.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.ResponsavelSolidario.EnderecoMunicipio = null;
                    }
                }

                propostaResult.Obra.ContratoPagamentos = _contratoPagamentoRepository
                    .ListarContratoPagamentosDetalhados(propostaResult.UsinaCodigo, propostaResult.Obra.AnoContrato ?? 0, propostaResult.Obra.NumContrato ?? 0, tracking)
                    .ToList();

                propostaResult.Obra.Contrato.ContratoTracoReajustes = _context.ContratoTracoReajustes
                .Where(t => t.UsinaCodigo == propostaResult.UsinaCodigo
                    && t.ContratoAno == propostaResult.Obra.AnoContrato
                    && t.ContratoNumero == propostaResult.Obra.NumContrato)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Slump)
                .Include(t => t.ResistenciaTipo)
                .Include(t => t.Pedra)
                .Include(t => t.UsinaEntrega)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.Contrato.ContratoBombaReajustes = _context.ContratoBombaReajustes
                    .Where(t => t.UsinaCodigo == propostaResult.UsinaCodigo
                        && t.ContratoAno == propostaResult.Obra.AnoContrato
                        && t.ContratoNumero == propostaResult.Obra.NumContrato)
                    .Include(t => t.Usina)
                    .Tracking(tracking)
                    .ToList();
            }
            else if(propostaResult.Obra != null && propostaResult.Obra.NumChamada != null && propostaResult.Obra.NumChamada > 0)
            {
                propostaResult.Obra.PropostaPagamentos = _context.PropostaPagamentos
                .Where(t => t.UsinaCodigo == propostaResult.UsinaCodigo
                    && t.PropostaAno == propostaResult.Obra.AnoChamada
                    && t.PropostaNumero == propostaResult.Obra.NumChamada
                    && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador)
                .Include(t => t.Detalhes)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.PropostaPagamentos.ToList()
                    .ForEach(p => p.Detalhes.OfType<PropostaPagamentoDetalheCartao>().ToList()
                        .ForEach(d => {
                            d.Bandeira = _context.CartaoBandeiras
                                        .Include(t => t.Portador.Conta)
                                        .Include(t => t.Interveniente)
                                        .Where(t => t.Codigo == d.BandeiraCodigo)
                                        .Tracking(tracking)
                                        .FirstOrDefault();
                        }));
                propostaResult.Obra.PropostaPagamentos.ToList()
                    .ForEach(p => p.Detalhes.OfType<PropostaPagamentoDetalheDeposito>().ToList()
                        .ForEach(d => {
                            d.Portador = _context.Portadores
                                        .Include(t => t.Conta)
                                        .Where(t => t.Codigo == d.PortadorCodigo)
                                        .Tracking(tracking)
                                        .FirstOrDefault();
                        }));
            }

            return propostaResult;
        }

        public PropostaVersao ObterPorUsinaAnoNumero(int numVersao, int idUsina, int ano, int numero, bool tracking = false)
        {
            var propostaResult = _context.PropostasVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.Ano == ano && t.Numero == numero) 
                .Include(t => t.Usina)
                .Include(t => t.Vendedor)
                .Include(t => t.VendedorPadrinho)
                .Include(t => t.Interveniente)
                .Include(t => t.Interveniente.GrupoEconomico)
                .Include(t => t.EnderecoMunicipio)
                .Include(t => t.Faturamento.EnderecoMunicipio)
                .Include(t => t.Cobranca.EnderecoMunicipio)
                .Include(c => c.Segmento)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.ResponsavelSolidario = _context.PropostaResponsaveisSolidariosVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.PropostaAno == ano && t.PropostaNumero == numero)
                .Include(t => t.EnderecoMunicipio)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.Obra = _context.ObrasVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.AnoChamada == ano && t.NumChamada == numero)
                .Include(c => c.ViaCaptacao)
                .Include(c => c.TipoObra)
                .Include(c => c.PorteObra)
                .Include(c => c.Contrato)
                .Include(c => c.CondicaoPagamento)
                .Include(c => c.TipoCobranca)
                .Include(c => c.EnderecoMunicipio)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.ContatoPrincipalFuncao)
                .Include(c => c.ContatoSecundarioFuncao)
                .Include(c => c.ObraReajuste)
                .Include(c => c.Indicador)
                .Include(c => c.Indicador.Vendedor)
                .Include(c => c.Indicador.Interveniente)
                .Include(c => c.TributacaoPisCofins)
                .Tracking(tracking)
                .FirstOrDefault();

            propostaResult.Obra.TributacaoCBS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoCBSCodigo };
            propostaResult.Obra.TributacaoIBS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoIBSCodigo };
            propostaResult.Obra.TributacaoIS = new TributacaoReforma { Id = propostaResult.Obra.TributacaoISCodigo };

            if (propostaResult.Obra != null)
            {
                propostaResult.Obra.ObraTracos = _context.ObraTracosVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Pedra)
                .Include(t => t.Slump)
                .Include(t => t.SlumpNominal)
                .Include(t => t.ResistenciaTipo)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraBombas = _context.ObraBombasVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.BombaTipo)
                .Include(t => t.Terceiro)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraTaxas = _obraTaxaRepository.ListarByIdObra(propostaResult.Obra.UsinaEntregaCodigo, propostaResult.Obra.Numero, numVersao, propostaResult.Segmentacao);

                propostaResult.Obra.ObraFrentes = _context.ObraFrente
                                .Where(t => t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                                .Tracking(tracking)
                                .ToList();

                propostaResult.Obra.ObraLogs = _context.ObraLogsVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraCodigo == propostaResult.Obra.Numero)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraTributacoesMunicipais = _context.ObraTributacoesMunicipaisVersoes
                .Where(t => t.NumeroVersao == numVersao && t.ObraUsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraNumero == propostaResult.Obra.Numero)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.ObraDemaisServicos = _context.ObraDemaisServicosVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.Obra.UsinaCodigo && t.ObraNumero == propostaResult.Obra.Numero)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Mercadoria)
                .Include(t => t.Unidade)
                .Tracking(tracking)
                .ToList();
            }

            if (propostaResult.Obra?.NumContrato > 0)
            {
                var localFaturamento = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.LocalFaturamento ?? 0);

                if (localFaturamento != null)
                {
                    propostaResult.Faturamento = new PropostaFaturamentoVersao
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = idUsina,
                        PropostaAno = ano,
                        PropostaNumero = numero,
                        IntervenienteTipo = (localFaturamento.CpfCnpj.Length == 14 ? "J" : "F"),
                        CpfCnpj = localFaturamento.CpfCnpj,
                        InscricaoEstadual = localFaturamento.InscricaoEstadual,
                        InscricaoMunicipal = localFaturamento.InscricaoMunicipal,
                        Nome = localFaturamento.Nome,
                        OrgaoExpedidor = localFaturamento.OrgaoExpedidor,
                        Razao = localFaturamento.Razao,
                        Rg = localFaturamento.Rg,
                        EnderecoBairro = localFaturamento.EnderecoBairro,
                        EnderecoCep = localFaturamento.EnderecoCep,
                        EnderecoComplemento = localFaturamento.EnderecoComplemento,
                        EnderecoLogradouro = localFaturamento.EnderecoLogradouro,
                        EnderecoMunicipioCodigo = localFaturamento.EnderecoMunicipioCodigo ?? 0,
                        EnderecoNumero = localFaturamento.EnderecoNumero,
                        Email = localFaturamento.Email
                    };

                    if ((localFaturamento.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.Faturamento.EnderecoMunicipio = ObterPorId<Municipio>(localFaturamento.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.Faturamento.EnderecoMunicipio = null;
                    }
                }

                var localCobranca = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.LocalCobranca ?? 0);

                if (localCobranca != null)
                {
                    propostaResult.Cobranca = new PropostaCobrancaVersao
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = idUsina,
                        PropostaAno = ano,
                        PropostaNumero = numero,
                        IntervenienteTipo = (localCobranca.CpfCnpj.Length == 14 ? "J" : "F"),
                        CpfCnpj = localCobranca.CpfCnpj,
                        InscricaoEstadual = localCobranca.InscricaoEstadual,
                        InscricaoMunicipal = localCobranca.InscricaoMunicipal,
                        Nome = localCobranca.Nome,
                        OrgaoExpedidor = localCobranca.OrgaoExpedidor,
                        Razao = localCobranca.Razao,
                        Rg = localCobranca.Rg,
                        EnderecoBairro = localCobranca.EnderecoBairro,
                        EnderecoCep = localCobranca.EnderecoCep,
                        EnderecoComplemento = localCobranca.EnderecoComplemento,
                        EnderecoLogradouro = localCobranca.EnderecoLogradouro,
                        EnderecoMunicipioCodigo = localCobranca.EnderecoMunicipioCodigo ?? 0,
                        EnderecoNumero = localCobranca.EnderecoNumero,
                        Email = localCobranca.Email
                    };

                    if ((localCobranca.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.Cobranca.EnderecoMunicipio = ObterPorId<Municipio>(localCobranca.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.Cobranca.EnderecoMunicipio = null;
                    }
                }

                var localResponsavelSolidario = ObterPorId<IntervenienteLocal>(propostaResult.IntervenienteCodigo ?? 0,
                    propostaResult.Obra.Contrato?.ResponsavelSolidario ?? 0);

                if (localResponsavelSolidario != null)
                {
                    propostaResult.ResponsavelSolidario = new PropostaResponsavelSolidarioVersao
                    {
                        NumeroVersao = numVersao,
                        UsinaCodigo = idUsina,
                        PropostaAno = ano,
                        PropostaNumero = numero,
                        IntervenienteTipo = (localResponsavelSolidario.CpfCnpj.Length == 14 ? "J" : "F"),
                        CpfCnpj = localResponsavelSolidario.CpfCnpj,
                        InscricaoEstadual = localResponsavelSolidario.InscricaoEstadual,
                        InscricaoMunicipal = localResponsavelSolidario.InscricaoMunicipal,
                        Nome = localResponsavelSolidario.Nome,
                        OrgaoExpedidor = localResponsavelSolidario.OrgaoExpedidor,
                        Razao = localResponsavelSolidario.Razao,
                        Rg = localResponsavelSolidario.Rg,
                        EnderecoBairro = localResponsavelSolidario.EnderecoBairro,
                        EnderecoCep = localResponsavelSolidario.EnderecoCep,
                        EnderecoComplemento = localResponsavelSolidario.EnderecoComplemento,
                        EnderecoLogradouro = localResponsavelSolidario.EnderecoLogradouro,
                        EnderecoMunicipioCodigo = localResponsavelSolidario.EnderecoMunicipioCodigo ?? 0,
                        EnderecoNumero = localResponsavelSolidario.EnderecoNumero,
                        Email = localResponsavelSolidario.Email
                    };

                    if ((localResponsavelSolidario.EnderecoMunicipioCodigo ?? 0) > 0)
                    {
                        propostaResult.ResponsavelSolidario.EnderecoMunicipio = ObterPorId<Municipio>(localResponsavelSolidario.EnderecoMunicipioCodigo ?? 0);
                    }
                    else
                    {
                        propostaResult.ResponsavelSolidario.EnderecoMunicipio = null;
                    }
                }

                propostaResult.Obra.ContratoPagamentos = _contratoPagamentoRepository
                    .ListarContratoPagamentosDetalhados(numVersao, propostaResult.UsinaCodigo, propostaResult.Obra.AnoContrato ?? 0, propostaResult.Obra.NumContrato ?? 0, tracking)
                    .ToList();

                propostaResult.Obra.Contrato.ContratoTracoReajustes = _context.ContratoTracoReajustesVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.UsinaCodigo
                    && t.ContratoAno == propostaResult.Obra.AnoContrato
                    && t.ContratoNumero == propostaResult.Obra.NumContrato)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Slump)
                .Include(t => t.ResistenciaTipo)
                .Include(t => t.Pedra)
                .Include(t => t.UsinaEntrega)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.Contrato.ContratoBombaReajustes = _context.ContratoBombaReajustesVersoes
                    .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.UsinaCodigo
                        && t.ContratoAno == propostaResult.Obra.AnoContrato
                        && t.ContratoNumero == propostaResult.Obra.NumContrato)
                    .Include(t => t.Usina)
                    .Tracking(tracking)
                    .ToList();
            }
            else if (propostaResult.Obra != null && propostaResult.Obra.NumChamada != null && propostaResult.Obra.NumChamada > 0)
            {
                propostaResult.Obra.PropostaPagamentos = _context.PropostaPagamentosVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == propostaResult.UsinaCodigo
                    && t.PropostaAno == propostaResult.Obra.AnoChamada
                    && t.PropostaNumero == propostaResult.Obra.NumChamada
                    && t.ObraCodigo == propostaResult.Obra.Numero)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador)
                .Include(t => t.Detalhes)
                .Tracking(tracking)
                .ToList();

                propostaResult.Obra.PropostaPagamentos.ToList()
                    .ForEach(p => p.Detalhes.OfType<PropostaPagamentoDetalheCartaoVersao>().ToList()
                        .ForEach(d => {
                            d.Bandeira = _context.CartaoBandeiras
                                        .Include(t => t.Portador.Conta)
                                        .Include(t => t.Interveniente)
                                        .Where(t => t.Codigo == d.BandeiraCodigo)
                                        .Tracking(tracking)
                                        .FirstOrDefault();
                        }));
                propostaResult.Obra.PropostaPagamentos.ToList()
                    .ForEach(p => p.Detalhes.OfType<PropostaPagamentoDetalheDepositoVersao>().ToList()
                        .ForEach(d => {
                            d.Portador = _context.Portadores
                                        .Include(t => t.Conta)
                                        .Where(t => t.Codigo == d.PortadorCodigo)
                                        .Tracking(tracking)
                                        .FirstOrDefault();
                        }));
            }

            return propostaResult;
        }

        public float ObterVolumeTotalProposto(int idUsina, int ano, int numero)
        {
            return _context.Propostas
                .Where(t => t.UsinaCodigo == idUsina && t.Ano == ano && t.Numero == numero)
                .Include(c => c.Obra)
                .Include(c => c.Obra.ObraTracos)
                .FirstOrDefault()?
                .Obra?
                .ObraTracos?
                .Sum(t => t.M3Quantidade) ?? 0f;
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_chtel c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_chamada={anoContrato}");
            sqlComando.Append($" and c.num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_resp_solid_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_chtel_resp_solid c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_chamada={anoContrato}");
            sqlComando.Append($" and c.num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_cobranca_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_chtel_cobranca c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_chamada={anoContrato}");
            sqlComando.Append($" and c.num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_faturamento_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_chtel_faturamento c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_chamada={anoContrato}");
            sqlComando.Append($" and c.num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }        

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_resp_solid_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_cobranca_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_faturamento_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_chtel_versao", "con_chtel");

            sqlComando.Append($"REPLACE INTO topsys.con_chtel");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_chtel_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_chamada={anoContrato}");
            sqlComando.Append($" AND num_chamada={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_chtel_resp_solid_versao", "con_chtel_resp_solid");

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_resp_solid");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_chtel_resp_solid_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_chamada={anoContrato}");
            sqlComando.Append($" AND num_chamada={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_chtel_cobranca_versao", "con_chtel_cobranca");

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_cobranca");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_chtel_cobranca_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_chamada={anoContrato}");
            sqlComando.Append($" AND num_chamada={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_chtel_faturamento_versao", "con_chtel_faturamento");

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_faturamento");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_chtel_faturamento_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_chamada={anoContrato}");
            sqlComando.Append($" AND num_chamada={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            /*sqlComando.Append($"DELETE FROM topsys.con_chtel");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();*/

            sqlComando.Append($"DELETE FROM topsys.con_chtel_resp_solid");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_cobranca");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_faturamento");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_chamada={anoContrato}");
            sqlComando.Append($" and num_chamada={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public PropostaVersao ObterPorId(int codUsina, int anoProposta, int numeroProposta, int numVersao)
        {            
            return _context.PropostasVersoes
                .Where(p => p.NumeroVersao == numVersao && p.UsinaCodigo == codUsina && p.Numero == numeroProposta && p.Ano == anoProposta)
                .FirstOrDefault();
        }

        public PropostaVersao ObterVersaoPorIdForList(int codUsina, int anoProposta, int numeroProposta, int numVersao)
        {
            return _context.PropostasVersoes
                .Where(p => p.NumeroVersao == numVersao && p.UsinaCodigo == codUsina && p.Numero == numeroProposta && p.Ano == anoProposta)
                .Include(p => p.Usina)
                .Include(p => p.Vendedor)
                .Include(p => p.Interveniente)
                .Include(p => p.Interveniente.BloqueioMotivo)
                .Include(p => p.Obra)
                .Include(p => p.Obra.Contrato)
                .Include(p => p.Obra.TipoCobranca)
                .Include(p => p.Segmento)
                .FirstOrDefault();
        }

        public ICollection<PropostaVersao> ListarPropostaVersoes(int versao, int codUsina, int anoProposta, int numeroProposta)
        {
            var propostaVersoesResult = _context
                .PropostasVersoes
                .Where(c => c.Ano == anoProposta && c.Numero == numeroProposta && c.UsinaCodigo == codUsina)
                .Include(t => t.Obra)
                .Include(t => t.Obra.UsinaEntrega)
                .Include(t => t.Obra.CondicaoPagamento)
                .Include(t => t.Obra.TipoCobranca)
                .Include(t => t.Obra.Contrato)
                .ToList();

            foreach (var record in propostaVersoesResult) {

                record.Obra.ObraTracos = _context.ObraTracosVersoes
                    .Where(t => t.NumeroVersao == record.NumeroVersao && t.UsinaCodigo == record.Obra.UsinaCodigo && t.ObraCodigo == record.Obra.Numero)
                    .Include(t => t.Usina)
                    .Include(t => t.Uso)
                    .Include(t => t.Pedra)
                    .Include(t => t.Slump)
                    .Include(t => t.SlumpNominal)
                    .Include(t => t.ResistenciaTipo)
                    .ToList();

                record.Obra.ObraBombas = _context.ObraBombasVersoes
                    .Where(t => t.NumeroVersao == record.NumeroVersao && t.UsinaCodigo == record.Obra.UsinaCodigo && t.ObraCodigo == record.Obra.Numero)
                    .Include(t => t.BombaTipo)
                    .Include(t => t.Terceiro)
                    .ToList();

                record.Obra.ObraFrentes = _context.ObraFrente
                                .Where(t => t.UsinaCodigo == record.Obra.UsinaCodigo && t.ObraCodigo == record.Obra.Numero)
                                .ToList();

                record.Obra.ObraTaxas = _obraTaxaRepository.ListarByIdObra(record.Obra.UsinaEntregaCodigo, record.Obra.Numero, record.NumeroVersao, record.Segmentacao);

                record.Obra.ObraDemaisServicos = _context.ObraDemaisServicosVersoes
                    .Where(t => t.NumeroVersao == record.NumeroVersao && t.UsinaCodigo == record.Obra.UsinaCodigo && t.ObraNumero == record.Obra.Numero)
                    .Include(t => t.UsinaEntrega)
                    .Include(t => t.Mercadoria)
                    .Include(t => t.Unidade)
                    .ToList();

                record.Obra.ContratoPagamentos = _contratoPagamentoRepository
                    .ListarContratoPagamentosDetalhados(record.NumeroVersao, record.UsinaCodigo, record.Obra.AnoContrato ?? 0, record.Obra.NumContrato ?? 0)
                    .ToList();
            }

            return propostaVersoesResult;
        }

        public int GetUltimaVersaoProposta(int codUsina, int anoProposta, int numeroProposta)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" coalesce(max(num_versao),0) as num_versao");
            sqlComando.Append(" FROM topsys.con_chtel_versao");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina=@COD_USINA");
            sqlComando.Append(" AND ano_chamada=@ANO_PROPOSTA");
            sqlComando.Append(" AND num_chamada=@NUMERO_PROPOSTA");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString(), new
            {
                COD_USINA = codUsina,
                ANO_PROPOSTA = anoProposta,
                NUMERO_PROPOSTA = numeroProposta
            });
        }

    }
}

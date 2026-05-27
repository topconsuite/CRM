using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ObraRepository : RepositoryBase<Obra>, IObraRepository
    {
        private IObraTaxaRepository _obraTaxaRepository;
        private IContratoPagamentoRepository _contratoPagamentoRepository;
        private IRepasseReajusteRepository _repasseReajusteRepository;
        private IObraFrenteRepository _obraFrenteRepository;
        private IDatabaseRepository _databaseRepository;
        private IParametroRepository _parametroRepository;
        private IObraProjecaoRepository _obraProjecaoRepository;
        private readonly IdentityHelperService _identityHelperService;

        public ObraRepository(AppDataContext context, 
            IObraTaxaRepository obraTaxaRepository, 
            IContratoPagamentoRepository contratoPagamentoRepository, 
            IRepasseReajusteRepository repasseReajusteRepository, 
            IObraFrenteRepository obraFrenteRepository, 
            IDatabaseRepository databaseRepository,
            IObraProjecaoRepository obraProjecaoRepository,
            IdentityHelperService identityHelperService, 
            IParametroRepository parametroRepository) : base(context)
        {
            _context = context;
            _obraTaxaRepository = obraTaxaRepository;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _repasseReajusteRepository = repasseReajusteRepository;
            _identityHelperService = identityHelperService;
            _obraFrenteRepository = obraFrenteRepository;
            _databaseRepository = databaseRepository;
            _parametroRepository = parametroRepository;
            _obraProjecaoRepository = obraProjecaoRepository;
        }

        public void DetectEntry(Obra obra)
        {
            _context.Entry(obra).State = EntityState.Detached;
        }

        public PagedList<Obra> ListarObraPorPaginaParaCarteira(int pagina, int porPagina, Expression<Func<Obra, bool>> filter)
        {

            var result = _context
                .Obras
                .Include(x => x.UsinaEntrega)
                .Include(x => x.Contrato)
                .Include(x => x.Contrato.Vendedor)
                .Include(x => x.Contrato.Interveniente)
                .Include(x => x.ObraProjecao)
                .Where(x => x.NumContrato > 0 && x.Contrato.Status != EContratoStatus.Encerrado)
                .Where(filter)
                .OrderByDescending(x => x.Numero)
                .PagedList(pagina, porPagina, filter);

            foreach(var record in result.Records)
            {

                var saldoProjetado = (float)(_obraProjecaoRepository.ObterPrevisaoSaldoProjecaoPorContrato(record.UsinaCodigo, record.Numero, record.AnoChamada, record.NumChamada) ?? 0);
                var saldoContratado = (float)(ObterVolumePorContrato(record.UsinaCodigo, record.Numero, (int)record.AnoChamada, (int)record.NumChamada) ?? 0);
                var saldoConsumido = 0f;
                if (record.Contrato != null)
                    saldoConsumido = (float)(ObterConsumoPorContrato(record.Contrato.Usina, record.Contrato.Numero, record.Contrato.Ano) ?? 0);

                saldoContratado = saldoContratado - saldoConsumido;

                switch (saldoProjetado)
                {
                    case 0:
                        record.StatusProjecao = EStatusProjecao.NaoPossui;
                        break;
                    case var _ when saldoProjetado == saldoContratado:
                        record.StatusProjecao = EStatusProjecao.Igual;
                        break;
                    default:
                        record.StatusProjecao = EStatusProjecao.Divergente;
                        break;
                }

            }

            return result;

        }

        public void Adicionar(Obra obra, float valorExtras)
        {
            var repasseReajusteDefault = _repasseReajusteRepository.ObterVigente(DateTime.Today);

            if (repasseReajusteDefault != null)
            {
                obra.PercentualRepasseReajusteAreia = repasseReajusteDefault.PercentualAreia;
                obra.PercentualRepasseReajusteCimento = repasseReajusteDefault.PercentualCimento;
                obra.PercentualRepasseReajusteDiesel = repasseReajusteDefault.PercentualDiesel;
                obra.PercentualRepasseReajusteMaoDeObra = repasseReajusteDefault.PercentualMaoDeObra;
                obra.PercentualRepasseReajustePedra = repasseReajusteDefault.PercentualPedra;
            }

            var sqlComando = obra.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras", sqlComando.ToString(), obra);

            obra.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_OBRA_INSERIDA").FirstOrDefault();

            if (obra.Numero > 0 && obra.NumChamada != null && obra.NumChamada > 0)
            {
                sqlComando = $"UPDATE {obra.Proposta.GetTableName(_context)} "
                    + $"SET {obra.Proposta.GetColumnName(nameof(Proposta.ObraCodigo), _context)}={obra.Numero} "
                    + $"WHERE {obra.Proposta.GetColumnName(nameof(Proposta.UsinaCodigo), _context)}={obra.UsinaCodigo} "
                    + $"AND {obra.Proposta.GetColumnName(nameof(Proposta.Ano), _context)}={obra.AnoChamada} "
                    + $"AND {obra.Proposta.GetColumnName(nameof(Proposta.Numero), _context)}={obra.NumChamada} ";
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel", sqlComando.ToString());
            }

            var sequencia = 1;
            foreach(var obraFrente in obra.ObraFrentes)
            {
                obraFrente.UsinaCodigo = obra.UsinaCodigo;
                obraFrente.ObraCodigo = obra.Numero;
                obraFrente.ObraSequencia = sequencia;
                obraFrente.ID = Guid.NewGuid();
                sequencia++;

                sqlComando = obraFrente.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_frente", sqlComando.ToString(), obraFrente);

            }

            foreach (var obraTraco in obra.ObraTracos)
            {
                obraTraco.UsinaCodigo = obra.UsinaCodigo;
                obraTraco.ObraCodigo = obra.Numero;
                obraTraco.PropostaAno = obra.AnoChamada ?? 0;
                obraTraco.PropostaNumero = obra.NumChamada ?? 0;

                sqlComando = obraTraco.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_proposta_item", sqlComando.ToString(), obraTraco);
            }

            foreach (var obraBomba in obra.ObraBombas)
            {
                obraBomba.UsinaCodigo = obra.UsinaCodigo;
                obraBomba.ObraCodigo = obra.Numero;
                obraBomba.PropostaAno = obra.AnoChamada ?? 0;
                obraBomba.PropostaNumero = obra.NumChamada ?? 0;

                sqlComando = obraBomba.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_prop_bomba", sqlComando.ToString(), obraBomba);
            }

            foreach (var obraTribMun in obra.ObraTributacoesMunicipais)
            {
                obraTribMun.ObraUsinaCodigo = obra.UsinaCodigo;
                obraTribMun.ObraNumero = obra.Numero;
                obraTribMun.ContratoAno = obra.AnoContrato ?? 0;
                obraTribMun.ContratoNumero = obra.NumContrato ?? 0;

                sqlComando = obraTribMun.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_trib_mun", sqlComando.ToString(), obraTribMun);
            }

            if(obra.Indicador != null)
            {
                obra.Indicador.ObraUsina = obra.UsinaCodigo;
                obra.Indicador.ObraNumero = obra.Numero;

                sqlComando = obra.Indicador.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_indicador", sqlComando.ToString(), obra.Indicador);
            }

            foreach (var demaisServico in obra.ObraDemaisServicos)
            {
                demaisServico.UsinaCodigo = obra.UsinaCodigo;
                demaisServico.ObraNumero = obra.Numero;

                sqlComando = demaisServico.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_dem_serv", sqlComando.ToString(), demaisServico);
            }

            var valorConcreto = obra.CalcularValorConcreto();
            var valorBomba = obra.CalcularValorBomba();
            var valorTotal = (float)valorConcreto + (float)valorBomba + valorExtras;

            if (obra.NumContrato != null && obra.NumContrato > 0)
            {
                foreach (var contratoPagamento in obra.ContratoPagamentos)
                {
                    contratoPagamento.UsinaCodigo = obra.UsinaCodigo;
                    contratoPagamento.ContratoAno = obra.AnoContrato ?? 0;
                    contratoPagamento.ContratoNumero = obra.NumContrato ?? 0;
                    contratoPagamento.Forma = contratoPagamento.TipoCobranca.Forma;
                    contratoPagamento.ValorFixoSimNao = contratoPagamento.TipoCobranca.Fixo;
                    contratoPagamento.NecessitaAprovacaoSimNao = contratoPagamento.TipoCobranca.Aprovacao;
                    contratoPagamento.Percentual = contratoPagamento.Valor / valorTotal * 100f;
                    contratoPagamento.IdCadastro = obra.IdCadastro;

                    sqlComando = contratoPagamento.MontarSqlInsert(_context);
                    _context.Database.Connection.Execute(sqlComando);

                    contratoPagamento.Detalhes.OfType<ContratoPagamentoDetalheDeposito>().ToList()
                        .ForEach(t => {
                            t.TomadorBanco = t.Portador.Conta.BancoCodigoOficial;
                            t.TomadorAgencia = t.Portador.Conta.NumeroAgencia.ToString();
                            t.TomadorAgencia = t.Portador.Conta.NumeroConta.ToString() + "-" + t.Portador.Conta.DvConta.ToString();
                        });

                    foreach (var detalhe in contratoPagamento.Detalhes)
                    {
                        detalhe.UsinaCodigo = contratoPagamento.UsinaCodigo;
                        detalhe.ContratoAno = contratoPagamento.ContratoAno;
                        detalhe.ContratoNumero = contratoPagamento.ContratoNumero;
                        detalhe.PropostaAno = obra.AnoChamada ?? 0;
                        detalhe.PropostaNumero = obra.NumChamada ?? 0;
                        detalhe.ObraCodigo = obra.Numero;
                        detalhe.PagamentoSequencia = contratoPagamento.Sequencia;

                        sqlComando = detalhe.MontarSqlInsert(_context);
                        _context.Database.Connection.Execute(sqlComando);
                    }
                }
            }
            else if (obra.NumChamada != null && obra.NumChamada > 0)
            {
                foreach (var propostaPagamento in obra.PropostaPagamentos)
                {
                    propostaPagamento.UsinaCodigo = obra.UsinaCodigo;
                    propostaPagamento.PropostaAno = obra.AnoChamada ?? 0;
                    propostaPagamento.PropostaNumero = obra.NumChamada ?? 0;
                    propostaPagamento.ObraCodigo = obra.Numero;
                    propostaPagamento.Forma = propostaPagamento.TipoCobranca.Forma;
                    propostaPagamento.ValorFixoSimNao = propostaPagamento.TipoCobranca.Fixo;
                    propostaPagamento.NecessitaAprovacaoSimNao = propostaPagamento.TipoCobranca.Aprovacao;
                    propostaPagamento.Percentual = propostaPagamento.Valor / valorTotal * 100f;
                    propostaPagamento.IdCadastro = obra.IdCadastro;

                    sqlComando = propostaPagamento.MontarSqlInsert(_context);
                    _context.Database.Connection.Execute(sqlComando);
                    _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel_pag", sqlComando.ToString(), propostaPagamento);

                    propostaPagamento.Detalhes.OfType<PropostaPagamentoDetalheDeposito>().ToList()
                        .ForEach(t => {
                            t.TomadorBanco = t.Portador.Conta.BancoCodigoOficial;
                            t.TomadorAgencia = t.Portador.Conta.NumeroAgencia.ToString();
                            t.TomadorNumeroConta = t.Portador.Conta.NumeroConta.ToString() + "-" + t.Portador.Conta.DvConta.ToString();
                        });

                    foreach (var detalhe in propostaPagamento.Detalhes)
                    {
                        detalhe.UsinaCodigo = propostaPagamento.UsinaCodigo;
                        detalhe.PropostaAno = propostaPagamento.PropostaAno;
                        detalhe.PropostaNumero = propostaPagamento.PropostaNumero;
                        detalhe.ContratoAno = obra.AnoContrato ?? 0;
                        detalhe.ContratoNumero = obra.NumContrato ?? 0;
                        detalhe.ObraCodigo = propostaPagamento.ObraCodigo;
                        detalhe.PagamentoSequencia = propostaPagamento.Sequencia;
                        detalhe.IdCadastro = obra.IdCadastro;

                        sqlComando = detalhe.MontarSqlInsert(_context);
                        _context.Database.Connection.Execute(sqlComando);
                    }
                }
            }

            foreach (var obraTaxa in obra.ObraTaxas)
            {
                obraTaxa.ObraCodigo = obra.Numero;

                sqlComando = obraTaxa.MontarSqlInsert(_context).Replace("INSERT INTO", "REPLACE INTO");
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_taxa_extra", sqlComando.ToString());

                if (obraTaxa.IsPersonalizada && obraTaxa.Selecionada == "S")
                {
                    obraTaxa.IdAtualizacao = obra.IdCadastro;
                    _obraTaxaRepository.SalvarPersonalizada(obraTaxa);
                }
            }

            obra.ObraReajuste.UsinaCodigo = obra.UsinaCodigo;
            obra.ObraReajuste.ObraCodigo = obra.Numero;
            sqlComando = obra.ObraReajuste.MontarSqlInsert(_context).Replace("INSERT INTO", "REPLACE INTO");
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_reajuste", sqlComando.ToString());

            // INSERE MENSAGENS PADRAO
            var obraMensagemTabela = EntityMapHelper.GetTableName<ObraMensagemPadrao>(_context);
            var obraMensagemUsinaCampo = EntityMapHelper.GetColumnName<ObraMensagemPadrao>(nameof(ObraMensagemPadrao.UsinaCodigo), _context);
            var obraMensagemObraCampo = EntityMapHelper.GetColumnName<ObraMensagemPadrao>(nameof(ObraMensagemPadrao.ObraNumero), _context);
            var obraMensagemCodigoCampo = EntityMapHelper.GetColumnName<ObraMensagemPadrao>(nameof(ObraMensagemPadrao.MensagemPadraoCodigo), _context);
            var obraMensagemSelecionadoCampo = EntityMapHelper.GetColumnName<ObraMensagemPadrao>(nameof(ObraMensagemPadrao.SelecionadoSimNao), _context);

            var mensagemPadraoTabela = EntityMapHelper.GetTableName<MensagemPadrao>(_context);
            var mensagemPadraoCodigoCampo = EntityMapHelper.GetColumnName<MensagemPadrao>(nameof(MensagemPadrao.Codigo), _context);

            sqlComando = $"REPLACE INTO {obraMensagemTabela} "
                + $"({obraMensagemUsinaCampo}, {obraMensagemObraCampo}, {obraMensagemCodigoCampo}, {obraMensagemSelecionadoCampo}) "
                + $"SELECT {obra.UsinaCodigo} usina, {obra.Numero} obra, {mensagemPadraoCodigoCampo}, 'S' selecionada "
                + $"FROM {mensagemPadraoTabela}";
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), obraMensagemTabela, sqlComando.ToString());
        }

        public void Adicionar(ObraVersao obra, float valorExtras)
        {
            var repasseReajusteDefault = _repasseReajusteRepository.ObterVigente(DateTime.Today);

            if (repasseReajusteDefault != null)
            {
                obra.PercentualRepasseReajusteAreia = repasseReajusteDefault.PercentualAreia;
                obra.PercentualRepasseReajusteCimento = repasseReajusteDefault.PercentualCimento;
                obra.PercentualRepasseReajusteDiesel = repasseReajusteDefault.PercentualDiesel;
                obra.PercentualRepasseReajusteMaoDeObra = repasseReajusteDefault.PercentualMaoDeObra;
                obra.PercentualRepasseReajustePedra = repasseReajusteDefault.PercentualPedra;
            }

            var sqlComando = obra.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_versao", sqlComando.ToString(), obra);

           // obra.Numero = _context.Database.Connection.Query<int>("SELECT @NUMERO_OBRA_INSERIDA").FirstOrDefault();

            if (obra.Numero > 0 && obra.NumChamada != null && obra.NumChamada > 0)
            {
                sqlComando = $"UPDATE {obra.Proposta.GetTableName(_context)} "
                    + $"SET {obra.Proposta.GetColumnName(nameof(Proposta.ObraCodigo), _context)}={obra.Numero} "
                    + $"WHERE {obra.Proposta.GetColumnName(nameof(Proposta.UsinaCodigo), _context)}={obra.UsinaCodigo} "
                    + $"AND {obra.Proposta.GetColumnName(nameof(Proposta.Ano), _context)}={obra.AnoChamada} "
                    + $"AND {obra.Proposta.GetColumnName(nameof(Proposta.Numero), _context)}={obra.NumChamada} ";
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel_versao", sqlComando.ToString());
            }

            var sequencia = 1;
            foreach (var obraFrenteVersao in obra.ObraFrentes)
            {
                obraFrenteVersao.UsinaCodigo = obra.UsinaCodigo;
                obraFrenteVersao.ObraCodigo = obra.Numero;
                obraFrenteVersao.ObraSequencia = sequencia;
                obraFrenteVersao.ID = Guid.NewGuid();
                sequencia++;

                sqlComando = obraFrenteVersao.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_frente_versao", sqlComando.ToString(), obraFrenteVersao);

            }

            foreach (var obraTracoVersao in obra.ObraTracos)
            {
                obraTracoVersao.UsinaCodigo = obra.UsinaCodigo;
                obraTracoVersao.ObraCodigo = obra.Numero;
                obraTracoVersao.PropostaAno = obra.AnoChamada ?? 0;
                obraTracoVersao.PropostaNumero = obra.NumChamada ?? 0;

                sqlComando = obraTracoVersao.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_proposta_item_versao", sqlComando.ToString(), obraTracoVersao);
            }

            foreach (var obraBombaVersao in obra.ObraBombas)
            {
                obraBombaVersao.UsinaCodigo = obra.UsinaCodigo;
                obraBombaVersao.ObraCodigo = obra.Numero;
                obraBombaVersao.PropostaAno = obra.AnoChamada ?? 0;
                obraBombaVersao.PropostaNumero = obra.NumChamada ?? 0;

                sqlComando = obraBombaVersao.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_prop_bomba_versao", sqlComando.ToString(), obraBombaVersao);
            }

            foreach (var obraTribMunVersao in obra.ObraTributacoesMunicipais)
            {
                obraTribMunVersao.ObraUsinaCodigo = obra.UsinaCodigo;
                obraTribMunVersao.ObraNumero = obra.Numero;
                obraTribMunVersao.ContratoAno = obra.AnoContrato ?? 0;
                obraTribMunVersao.ContratoNumero = obra.NumContrato ?? 0;

                sqlComando = obraTribMunVersao.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_trib_mun_versao", sqlComando.ToString(), obraTribMunVersao);
            }

            foreach (var demaisServicoVersao in obra.ObraDemaisServicos)
            {
                demaisServicoVersao.UsinaCodigo = obra.UsinaCodigo;
                demaisServicoVersao.ObraNumero = obra.Numero;

                sqlComando = demaisServicoVersao.MontarSqlInsert(_context);
                _context.Database.Connection.Execute(sqlComando);

                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_dem_serv_versao", sqlComando.ToString(), demaisServicoVersao);
            }

            var valorConcreto = obra.CalcularValorConcreto();
            var valorBomba = obra.CalcularValorBomba();
            var valorTotal = (float)valorConcreto + (float)valorBomba + valorExtras;

            if (obra.NumContrato != null && obra.NumContrato > 0)
            {
                foreach (var contratoPagamentoVersao in obra.ContratoPagamentos)
                {
                    contratoPagamentoVersao.UsinaCodigo = obra.UsinaCodigo;
                    contratoPagamentoVersao.ContratoAno = obra.AnoContrato ?? 0;
                    contratoPagamentoVersao.ContratoNumero = obra.NumContrato ?? 0;
                    contratoPagamentoVersao.Forma = contratoPagamentoVersao.TipoCobranca.Forma;
                    contratoPagamentoVersao.ValorFixoSimNao = contratoPagamentoVersao.TipoCobranca.Fixo;
                    contratoPagamentoVersao.NecessitaAprovacaoSimNao = contratoPagamentoVersao.TipoCobranca.Aprovacao;
                    contratoPagamentoVersao.Percentual = contratoPagamentoVersao.Valor / valorTotal * 100f;
                    contratoPagamentoVersao.IdCadastro = obra.IdCadastro;

                    sqlComando = contratoPagamentoVersao.MontarSqlInsert(_context);
                    _context.Database.Connection.Execute(sqlComando);

                    contratoPagamentoVersao.Detalhes.OfType<ContratoPagamentoDetalheDepositoVersao>().ToList()
                        .ForEach(t => {
                            t.TomadorBanco = t.Portador.Conta.BancoCodigoOficial;
                            t.TomadorAgencia = t.Portador.Conta.NumeroAgencia.ToString();
                            t.TomadorAgencia = t.Portador.Conta.NumeroConta.ToString() + "-" + t.Portador.Conta.DvConta.ToString();
                        });

                    foreach (var detalhe in contratoPagamentoVersao.Detalhes)
                    {
                        detalhe.UsinaCodigo = contratoPagamentoVersao.UsinaCodigo;
                        detalhe.ContratoAno = contratoPagamentoVersao.ContratoAno;
                        detalhe.ContratoNumero = contratoPagamentoVersao.ContratoNumero;
                        detalhe.PropostaAno = obra.AnoChamada ?? 0;
                        detalhe.PropostaNumero = obra.NumChamada ?? 0;
                        detalhe.ObraCodigo = obra.Numero;
                        detalhe.PagamentoSequencia = contratoPagamentoVersao.Sequencia;

                        sqlComando = detalhe.MontarSqlInsert(_context);
                        _context.Database.Connection.Execute(sqlComando);
                    }
                }
            }
            else if (obra.NumChamada != null && obra.NumChamada > 0)
            {
                foreach (var propostaPagamentoVersao in obra.PropostaPagamentos)
                {
                    propostaPagamentoVersao.UsinaCodigo = obra.UsinaCodigo;
                    propostaPagamentoVersao.PropostaAno = obra.AnoChamada ?? 0;
                    propostaPagamentoVersao.PropostaNumero = obra.NumChamada ?? 0;
                    propostaPagamentoVersao.ObraCodigo = obra.Numero;
                    propostaPagamentoVersao.Forma = propostaPagamentoVersao.TipoCobranca.Forma;
                    propostaPagamentoVersao.ValorFixoSimNao = propostaPagamentoVersao.TipoCobranca.Fixo;
                    propostaPagamentoVersao.NecessitaAprovacaoSimNao = propostaPagamentoVersao.TipoCobranca.Aprovacao;
                    propostaPagamentoVersao.Percentual = propostaPagamentoVersao.Valor / valorTotal * 100f;
                    propostaPagamentoVersao.IdCadastro = obra.IdCadastro;

                    sqlComando = propostaPagamentoVersao.MontarSqlInsert(_context);
                    _context.Database.Connection.Execute(sqlComando);
                    _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_chtel_pag_versao", sqlComando.ToString(), propostaPagamentoVersao);

                    propostaPagamentoVersao.Detalhes.OfType<PropostaPagamentoDetalheDepositoVersao>().ToList()
                        .ForEach(t => {
                            t.TomadorBanco = t.Portador.Conta.BancoCodigoOficial;
                            t.TomadorAgencia = t.Portador.Conta.NumeroAgencia.ToString();
                            t.TomadorNumeroConta = t.Portador.Conta.NumeroConta.ToString() + "-" + t.Portador.Conta.DvConta.ToString();
                        });

                    foreach (var detalheVersao in propostaPagamentoVersao.Detalhes)
                    {
                        detalheVersao.UsinaCodigo = propostaPagamentoVersao.UsinaCodigo;
                        detalheVersao.PropostaAno = propostaPagamentoVersao.PropostaAno;
                        detalheVersao.PropostaNumero = propostaPagamentoVersao.PropostaNumero;
                        detalheVersao.ContratoAno = obra.AnoContrato ?? 0;
                        detalheVersao.ContratoNumero = obra.NumContrato ?? 0;
                        detalheVersao.ObraCodigo = propostaPagamentoVersao.ObraCodigo;
                        detalheVersao.PagamentoSequencia = propostaPagamentoVersao.Sequencia;
                        detalheVersao.IdCadastro = obra.IdCadastro;

                        sqlComando = detalheVersao.MontarSqlInsert(_context);
                        _context.Database.Connection.Execute(sqlComando);
                    }
                }
            }

            foreach (var obraTaxaVersao in obra.ObraTaxas)
            {
                obraTaxaVersao.ObraCodigo = obra.Numero;

                sqlComando = obraTaxaVersao.MontarSqlInsert(_context).Replace("INSERT INTO", "REPLACE INTO");
                _context.Database.Connection.Execute(sqlComando);
                _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_taxa_extra_versao", sqlComando.ToString());

                if (obraTaxaVersao.IsPersonalizada && obraTaxaVersao.Selecionada == "S")
                {
                    obraTaxaVersao.IdAtualizacao = obra.IdCadastro;
                    _obraTaxaRepository.SalvarPersonalizada(obraTaxaVersao);
                }
            }


            // INSERE MENSAGENS PADRAO
            var obraMensagemTabela = EntityMapHelper.GetTableName<ObraMensagemPadraoVersao>(_context);
            var obraMensagemUsinaCampo = EntityMapHelper.GetColumnName<ObraMensagemPadraoVersao>(nameof(ObraMensagemPadraoVersao.UsinaCodigo), _context);
            var obraMensagemObraCampo = EntityMapHelper.GetColumnName<ObraMensagemPadraoVersao>(nameof(ObraMensagemPadraoVersao.ObraNumero), _context);
            var obraMensagemCodigoCampo = EntityMapHelper.GetColumnName<ObraMensagemPadraoVersao>(nameof(ObraMensagemPadraoVersao.MensagemPadraoCodigo), _context);
            var obraMensagemSelecionadoCampo = EntityMapHelper.GetColumnName<ObraMensagemPadraoVersao>(nameof(ObraMensagemPadraoVersao.SelecionadoSimNao), _context);

            var mensagemPadraoTabela = EntityMapHelper.GetTableName<ObraMensagemPadraoVersao>(_context);
            var mensagemPadraoCodigoCampo = EntityMapHelper.GetColumnName<ObraMensagemPadraoVersao>(nameof(MensagemPadrao.Codigo), _context);

            sqlComando = $"REPLACE INTO {obraMensagemTabela} "
                + $"({obraMensagemUsinaCampo}, {obraMensagemObraCampo}, {obraMensagemCodigoCampo}, {obraMensagemSelecionadoCampo}) "
                + $"SELECT {obra.UsinaCodigo} usina, {obra.Numero} obra, {mensagemPadraoCodigoCampo}, 'S' selecionada "
                + $"FROM {mensagemPadraoTabela}";
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), obraMensagemTabela, sqlComando.ToString());
        }

        public void Adicionar(DemaisAprovacao demaisAprovacao)
        {
            var sqlComando = demaisAprovacao.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_aprov", sqlComando.ToString(), demaisAprovacao);
        }

        public void Adicionar(AprovacaoScript aprovacaoScript)
        {
            var sqlComando = aprovacaoScript.MontarSqlInsert(_context);
            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_aprov_script", sqlComando.ToString(), aprovacaoScript);
        }

        public void AtualizarEnderecoProgramacoesFuturas(int usina, int obraNumero)
        {
            var sqlComando = $"UPDATE con_programacao p " +
                $"INNER JOIN con_obras o " +
                $"ON p.usina = o.usina " +
                $"AND p.no_obra = o.numero " +
                $"SET p.obra_cep = o.obra_cep, " +
                $"p.obra_end = o.obra_end, " +
                $"p.obra_no = o.obra_num, " +
                $"p.obra_compl = o.obra_compl, " +
                $"p.obra_bairro = o.obra_bairro, " +
                $"p.obra_mun = o.obra_mun " +
                $"WHERE p.usina = {usina} " +
                $"AND p.no_obra = {obraNumero} " +
                $"AND p.dt_concretagem >= CURRENT_DATE() ";

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_programacao", sqlComando.ToString());
        }

        public void AtualizarObraPendente(Obra obra)
        {
            _context.Obras.Attach(obra);

        }

        public void AtualizarObraPendente(ObraVersao obra)
        {
            _context.ObrasVersoes.Attach(obra);

        }

        public void AtualizarStatusComercial(int usina, int obraNumero)
        {
            var aprovado = (int)EObraStatusComercial.Aprovado;
            var naoNecessita = (int)EObraStatusComercial.NaoNecessita;
            var aguardando = (int)EObraStatusComercial.Aguardando;

            var aprovacaoAlcadaAguardando = (int)EAprovacaoComercialPendenteStatus.AguardandoAprovacao;
            var aprovacaoAlcadaAguardadoOutroNivel = (int)EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;
            var ultimaVersaoObra = ObterUltimaVersaoObra(usina, obraNumero);

            var sqlComando = $"UPDATE con_obras o " +
                $"LEFT JOIN view_obras_pendentes_aprov p " +
                $"ON o.usina=p.usina AND o.numero=p.numero " +
                $"SET o.status_comercial=" +
                $"  IF(" +
                $"      IFNULL(" +
                $"             (SELECT 'S' FROM con_aprovacao_comercial_pendente cacp" +
                $"              WHERE aprovacao_status IN({aprovacaoAlcadaAguardando}, {aprovacaoAlcadaAguardadoOutroNivel}) " +
                $"              AND obra_numero={obraNumero}" +
                $"              AND obra_usina={usina}" +
                $"              AND obra_versao={ultimaVersaoObra}" +
                $"              ORDER BY data_criacao DESC LIMIT 1)" +
                $"             , 'N') = 'S'" +
                $"  , {aguardando}" +
                $"  , IF(ISNULL(p.numero), IF(o.status_comercial={aprovado},{aprovado},{naoNecessita}), {aguardando}) " +
                $"  )" +
                $"WHERE o.usina = {usina} " +
                $"AND o.numero = {obraNumero} ";

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras", sqlComando.ToString());
        }

        public void AtualizarStatusComercial(int usina, int obraNumero, int numVersao)
        {
            var aprovado = (int)EObraStatusComercial.Aprovado;
            var naoNecessita = (int)EObraStatusComercial.NaoNecessita;
            var aguardando = (int)EObraStatusComercial.Aguardando;

            var aprovacaoAlcadaAguardando = (int)EAprovacaoComercialPendenteStatus.AguardandoAprovacao;
            var aprovacaoAlcadaAguardadoOutroNivel = (int)EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

            var sqlComando = $"UPDATE con_obras_versao o " +
                $"LEFT JOIN view_obras_pendentes_aprov_versao p " +
                $"ON o.usina=p.usina AND o.numero=p.numero and o.num_versao=p.versao " +
                $"SET o.status_comercial=" +
                $"  IF(" +
                $"      IFNULL(" +
                $"             (SELECT 'S' FROM con_aprovacao_comercial_pendente cacp" +
                $"              WHERE aprovacao_status IN({aprovacaoAlcadaAguardando}, {aprovacaoAlcadaAguardadoOutroNivel}) " +
                $"              AND obra_numero={obraNumero}" +
                $"              AND obra_usina={usina}" +
                $"              AND obra_versao={numVersao}" +
                $"              ORDER BY data_criacao DESC LIMIT 1)" +
                $"             , 'N') = 'S'" +
                $"  , {aguardando}" +
                $"  , IF(ISNULL(p.numero), IF(o.status_comercial={aprovado},{aprovado},{naoNecessita}), {aguardando}) " +
                $"  )" +
                $"WHERE o.usina = {usina} " +
                $"AND o.numero = {obraNumero} "+
                $"AND o.num_versao = {numVersao} ";

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_versao", sqlComando.ToString());
        }

        public bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero)
        {

            var sqlComando = $"SELECT o.status_comercial FROM con_obras o " +
                $"WHERE o.usina = @{nameof(obraUsina)} " +
                $"AND o.numero = @{nameof(obraNumero)} ";

            var statusObra = _context.Database.Connection.QueryFirst<int>(sqlComando, new { obraUsina, obraNumero });

            return statusObra == (int)EObraStatusComercial.Reprovado;

        }

        public bool VerificarStatusComercialEstaReprovada(int obraUsina, int obraNumero, int obraVersao)
        {

            var sqlComando = $"SELECT o.status_comercial FROM con_obras_versao o " +
                $"WHERE o.usina = @{nameof(obraUsina)} " +
                $"AND o.numero = @{nameof(obraNumero)} " +
                $"AND o.num_versao = @{nameof(obraVersao)} ";

            var statusObra = _context.Database.Connection.QueryFirst<int>(sqlComando, new { obraUsina, obraNumero, obraVersao });

            return statusObra == (int)EObraStatusComercial.Reprovado;

        }

        public void AtualizarStatusComercial(Obra obra, EObraStatusComercial statusComercial)
        {

            obra.StatusComercial = statusComercial;

            var sqlComando = $"UPDATE con_obras o " +
                $"SET o.status_comercial={(int)statusComercial} " +
                $"WHERE o.usina = {obra.UsinaCodigo} " +
                $"AND o.numero = {obra.Numero} ";

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras", sqlComando.ToString());

        }

        public void AtualizarStatusComercial(ObraVersao obra, EObraStatusComercial statusComercial)
        {
            obra.StatusComercial = statusComercial;

            var sqlComando = $"UPDATE con_obras_versao o " +
                $"SET o.status_comercial={(int)statusComercial} " +
                $"WHERE o.usina = {obra.UsinaCodigo} " +
                $"AND o.numero = {obra.Numero} " +
                $"AND o.num_versao = {obra.NumeroVersao} ";

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_obras_versao", sqlComando.ToString());
        }

        public IEnumerable<Obra> ListarComBombaPendenteDeAprovacao(string usuario)
        {

            return _context.Obras
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Vendedor)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.UsinaEntrega)
                .AsNoTracking()
                .Where(p => p.ObraBombas.Any(
                c => c.AprovacaoVerbal == "S"
                && (c.AprovacaoObservacao == "" || c.AprovacaoObservacao == usuario))
                ).ToList();
        }

        public IEnumerable<Obra> ListarComDemaisAprovacoesPendentes(string usuario)
        {
            StringBuilder sqlCommand = new StringBuilder();

            //Montando a lista contendo todos os tipos de Aprovação
            string aprovacaoTipo = string.Join(",", Enum.GetValues(typeof(EAprovacaoTipo)).Cast<int>());
            
            sqlCommand.Append("SELECT ");

            var columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Obra>(_context, "obras");
            sqlCommand.Append(columns);
            
            sqlCommand.Append(" FROM con_obras obras");
            sqlCommand.Append(" LEFT JOIN con_aprov aprovacaoComercial");

            sqlCommand.Append(" ON SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 1 )=obras.usina ");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', -1 ),'-',1)=obras.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 2 ),'/',-1)=obras.no_chamada");

            sqlCommand.Append(" WHERE");

            sqlCommand.Append(" (aprovacaoComercial.dt_hora_exec='1000-01-01 00:00:00' OR ISNULL(aprovacaoComercial.dt_hora_exec))");
            sqlCommand.Append(" AND (aprovacaoComercial.usuario_aprov='' OR aprovacaoComercial.usuario_aprov= @Usuario)");
            sqlCommand.Append(" AND aprovacaoComercial.tipo_aprov IN (@AprovacaoTipo)");

            sqlCommand.Append(" GROUP BY obras.usina, obras.ano_chamada, obras.no_chamada");
            

            IEnumerable<Obra> obras = _context.Obras.SqlQuery(sqlCommand.ToString(),
               new MySqlParameter("@AprovacaoTipo", aprovacaoTipo),
               new MySqlParameter("@Usuario", usuario)
               )
               .AsNoTracking()
               .ToList();


            ICollection<Obra> listaObras = new List<Obra>();
            
            foreach (var obra in obras)
                listaObras.Add(_context.Obras
                    .Include(c => c.Contrato.Interveniente)
                    .Include(c => c.Contrato.Vendedor)
                    .Include(c => c.Proposta.Interveniente)
                    .Include(c => c.Proposta.Vendedor)
                    .Include(c => c.UsinaEntrega)
                    .AsNoTracking()
                    .Where(t => t.UsinaCodigo == obra.UsinaCodigo 
                            && t.Numero == obra.Numero
                            && t.AnoChamada == obra.AnoChamada
                            && t.NumChamada == obra.NumChamada
                            ).SingleOrDefault());
            
            return listaObras;
            
        }

        public IEnumerable<ObraVersao> ListarComDemaisAprovacoesPendentes(int numVersao, string usuario)
        {
            StringBuilder sqlCommand = new StringBuilder();

            //Montando a lista contendo todos os tipos de Aprovação
            string aprovacaoTipo = string.Join(",", Enum.GetValues(typeof(EAprovacaoTipo)).Cast<int>());

            sqlCommand.Append("SELECT ");

            var columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<ObraVersao>(_context, "obras_versao");
            sqlCommand.Append(columns);

            sqlCommand.Append(" FROM con_obras_versao obras");
            sqlCommand.Append(" LEFT JOIN con_aprov_versao aprovacaoComercial");

            sqlCommand.Append(" ON SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 1 )=obras.usina ");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', -1 ),'-',1)=obras.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 2 ),'/',-1)=obras.no_chamada");
            sqlCommand.Append(" AND aprovacaoComercial.num_versao=obras.num_versao");

            sqlCommand.Append(" WHERE");

            sqlCommand.Append(" (aprovacaoComercial.dt_hora_exec='1000-01-01 00:00:00' OR ISNULL(aprovacaoComercial.dt_hora_exec))");
            sqlCommand.Append(" AND (aprovacaoComercial.usuario_aprov='' OR aprovacaoComercial.usuario_aprov= @Usuario)");
            sqlCommand.Append(" AND aprovacaoComercial.tipo_aprov IN (@AprovacaoTipo)");
            sqlCommand.Append(" AND obras.num_versao=@NumVersao");

            sqlCommand.Append(" GROUP BY obras.usina, obras.ano_chamada, obras.no_chamada");


            IEnumerable<ObraVersao> obras = _context.ObrasVersoes.SqlQuery(sqlCommand.ToString(),
               new MySqlParameter("@AprovacaoTipo", aprovacaoTipo),
               new MySqlParameter("@Usuario", usuario),
               new MySqlParameter("@NumVersao", numVersao)
               )
               .AsNoTracking()
               .ToList();


            ICollection<ObraVersao> listaObras = new List<ObraVersao>();

            foreach (var obra in obras)
                listaObras.Add(_context.ObrasVersoes
                    .Include(c => c.Contrato.Interveniente)
                    .Include(c => c.Contrato.Vendedor)
                    .Include(c => c.Proposta.Interveniente)
                    .Include(c => c.Proposta.Vendedor)
                    .Include(c => c.UsinaEntrega)
                    .AsNoTracking()
                    .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                            && t.Numero == obra.Numero
                            && t.AnoChamada == obra.AnoChamada
                            && t.NumChamada == obra.NumChamada
                            && t.NumeroVersao == obra.NumeroVersao
                            ).SingleOrDefault());

            return listaObras;

        }

        public IEnumerable<Obra> ListarComTaxaExtraPendenteDeAprovacao()
        {
            
            IEnumerable<Obra> obras =  (from obra in _context.Obras
                    join taxa in _context.ObraTaxas
                    on new { x1 = obra.Numero, x2 = obra.UsinaEntregaCodigo }
                    equals
                    new { x1 = taxa.ObraCodigo, x2 = taxa.UsinaCodigo }
                    where taxa.Aprovada == "N"
                    select obra).ToList().GroupBy(o => o).Select(g => g.First()).ToList();

            ICollection<Obra> listaObras = new List<Obra>();

            foreach (var obra in obras)
                listaObras.Add(_context.Obras
                    .Include(c => c.Contrato.Interveniente)
                    .Include(c => c.Contrato.Vendedor)
                    .Include(c => c.Proposta.Interveniente)
                    .Include(c => c.Proposta.Vendedor)
                    .Include(c => c.UsinaEntrega)
                    .AsNoTracking()
                    .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                            && t.Numero == obra.Numero
                            && t.AnoChamada == obra.AnoChamada
                            && t.NumChamada == obra.NumChamada
                            ).SingleOrDefault());
            
            return listaObras;
            
        }

        public IEnumerable<ObraVersao> ListarComTaxaExtraPendenteDeAprovacao(int numVersao)
        {

            IEnumerable<ObraVersao> obras = (from obra in _context.ObrasVersoes
                                       join taxa in _context.ObraTaxasVersoes
                                       on new { x1 = obra.Numero, x2 = obra.UsinaEntregaCodigo }
                                       equals
                                       new { x1 = taxa.ObraCodigo, x2 = taxa.UsinaCodigo }
                                       where taxa.Aprovada == "N" && taxa.NumeroVersao == numVersao
                                       select obra).ToList().GroupBy(o => o).Select(g => g.First()).ToList();

            ICollection<ObraVersao> listaObras = new List<ObraVersao>();

            foreach (var obra in obras)
                listaObras.Add(_context.ObrasVersoes
                    .Include(c => c.Contrato.Interveniente)
                    .Include(c => c.Contrato.Vendedor)
                    .Include(c => c.Proposta.Interveniente)
                    .Include(c => c.Proposta.Vendedor)
                    .Include(c => c.UsinaEntrega)
                    .AsNoTracking()
                    .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                            && t.Numero == obra.Numero
                            && t.AnoChamada == obra.AnoChamada
                            && t.NumChamada == obra.NumChamada
                            && t.NumeroVersao == numVersao
                            ).SingleOrDefault());

            return listaObras;

        }

        public IEnumerable<Obra> ListarComTracoPendenteDeAprovacao(string usuario)
        {
            return _context.Obras
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Vendedor)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.UsinaEntrega)
                .AsNoTracking()
                .Where(
                    p => p.ObraTracos.Any(
                    c => c.AprovacaoVerbal == "N" 
                    && (c.AprovacaoObservacao == "" || c.AprovacaoObservacao == usuario ))
                    );
        }

        public IEnumerable<ObraVersao> ListarComTracoPendenteDeAprovacao(int numVersao, string usuario)
        {
            return _context.ObrasVersoes
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Vendedor)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.UsinaEntrega)
                .AsNoTracking()
                .Where(
                    p => p.ObraTracos.Any(
                    c => c.AprovacaoVerbal == "N"
                    && (c.AprovacaoObservacao == "" || c.AprovacaoObservacao == usuario)
                    && c.NumeroVersao == numVersao)
                    );
        }

        public IEnumerable<ObraLog> ListarObraLogsPorId(int usina, int numero, int? anoChamada, int? noChamada)
        {
            var obraLogs = _context.ObraLogs
                .Where(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.AnoChamada == anoChamada
                    && t.NumChamada == noChamada)
                .AsNoTracking()
                .OrderByDescending(t => new { t.DataHora, t.Sequencia})
                .ToList();

            return obraLogs;
        }

        public IEnumerable<ObraLogVersao> ListarObraLogsPorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada)
        {
            var obraLogs = _context.ObraLogsVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.AnoChamada == anoChamada
                    && t.NumChamada == noChamada)
                .AsNoTracking()
                .OrderByDescending(t => new { t.DataHora, t.Sequencia })
                .ToList();

            return obraLogs;
        }

        public Obra ListarObraPagamentos(int idUsina, int obraNumero)
        {
            var tracking = false;

            var obra = _context.Obras
                .Where(t => t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Contrato)
                .Include(t => t.Contrato.Interveniente)
                .Include(t => t.Proposta)
                .Tracking(tracking).FirstOrDefault();

            obra.ContratoPagamentos = _context.ContratoPagamentos
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                    && t.ContratoAno == obra.AnoContrato
                    && t.ContratoNumero == obra.NumContrato)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador.Conta)
                .Tracking(tracking)
                .ToList();

            obra.PropostaPagamentos = _context.PropostaPagamentos
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                    && t.PropostaAno == obra.AnoChamada
                    && t.PropostaNumero == obra.NumChamada)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador.Conta)
                .Tracking(tracking)
                .ToList();

            obra.ContratoPagamentos.ToList()
                .ForEach(p => _contratoPagamentoRepository.CarregarDetalhes(p, tracking));

            return obra;
        }

        public ObraVersao ListarObraPagamentos(int idUsina, int obraNumero, int numVersao)
        {
            var tracking = false;

            var obra = _context.ObrasVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Contrato)
                .Include(t => t.Contrato.Interveniente)
                .Include(t => t.Proposta)
                .Tracking(tracking).FirstOrDefault();

            obra.ContratoPagamentos = _context.ContratoPagamentosVersoes
                .Where(t => t.NumeroVersao == obra.NumeroVersao
                    && t.UsinaCodigo == obra.UsinaCodigo
                    && t.ContratoAno == obra.AnoContrato
                    && t.ContratoNumero == obra.NumContrato)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador.Conta)
                .Tracking(tracking)
                .ToList();

            obra.PropostaPagamentos = _context.PropostaPagamentosVersoes
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                    && t.PropostaAno == obra.AnoChamada
                    && t.PropostaNumero == obra.NumChamada)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador.Conta)
                .Tracking(tracking)
                .ToList();

            obra.ContratoPagamentos.ToList()
                .ForEach(p => _contratoPagamentoRepository.CarregarDetalhes(p, tracking));

            return obra;
        }

        public ObraVersao ListarObraPagamentosVersao(int idUsina, int obraNumero, int numVersao)
        {
            var tracking = false;

            var obra = _context.ObrasVersoes
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Include(t => t.UsinaEntrega)
                .Include(t => t.Contrato)
                .Include(t => t.Contrato.Interveniente)
                .Tracking(tracking).FirstOrDefault();

            obra.ContratoPagamentos = _context.ContratoPagamentosVersoes
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo
                    && t.ContratoAno == obra.AnoContrato
                    && t.ContratoNumero == obra.NumContrato
                    && t.NumeroVersao == obra.NumeroVersao)
                .Include(t => t.CondicaoPagamento)
                .Include(t => t.TipoCobranca.Portador.Conta)
                .Tracking(tracking)
                .ToList();

            obra.ContratoPagamentos.ToList()
                .ForEach(p => _contratoPagamentoRepository.CarregarDetalhes(p, tracking));

            return obra;
        }

        public Obra ListarObraTracos(int idUsina, int obraNumero, bool tracking = false)
        {
            var obra = _context.Obras
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente)
                .Where(t => t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Tracking(tracking).FirstOrDefault();
            
            obra.ObraTracos = _context.ObraTracos
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Pedra)
                .Include(t => t.Slump)
                .Include(t => t.SlumpNominal)
                .Include(t => t.ResistenciaTipo)
                .Tracking(tracking)
                .ToList();

            return obra;
        }

        public ObraVersao ListarObraTracos(int numVersao, int idUsina, int obraNumero)
        {
            var tracking = false;

            var obra = _context.ObrasVersoes
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente)
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Tracking(tracking).FirstOrDefault();

            obra.ObraTracos = _context.ObraTracosVersoes
                .Where(t => t.NumeroVersao == obra.NumeroVersao && t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero)
                .Include(t => t.Usina)
                .Include(t => t.Uso)
                .Include(t => t.Pedra)
                .Include(t => t.Slump)
                .Include(t => t.SlumpNominal)
                .Include(t => t.ResistenciaTipo)
                .Tracking(tracking)
                .ToList();

            return obra;
        }

        public Obra ListarObraBombas(int idUsina, int obraNumero, bool tracking = false)
        {
            var obra = _context.Obras
                .Include(c => c.Contrato)
                .Where(t => t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Tracking(tracking).FirstOrDefault();

            obra.ObraBombas = _context.ObraBombas
                .Where(t => t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero)
                .Include(t => t.BombaTipo)
                .Tracking(tracking)
                .ToList();

            return obra;
        }

        public ObraVersao ListarObraBombas(int numVersao, int idUsina, int obraNumero)
        {
            var tracking = false;

            var obra = _context.ObrasVersoes
                .Include(c => c.Contrato)
                .Where(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina && t.Numero == obraNumero)
                .Tracking(tracking).FirstOrDefault();

            obra.ObraBombas = _context.ObraBombasVersoes
                .Where(t => t.NumeroVersao == obra.NumeroVersao && t.UsinaCodigo == obra.UsinaCodigo && t.ObraCodigo == obra.Numero)
                .Include(t => t.BombaTipo)
                .Tracking(tracking)
                .ToList();

            return obra;
        }

        public IEnumerable<Obra> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao)
        {
            var obras = (from obra in _context.Obras
                        join ccredit in _context.PropostaPagamentoDetalhesCartao
                        on new { x1 = obra.Numero, x2 = obra.UsinaCodigo }
                        equals
                        new { x1 = ccredit.ObraCodigo, x2 = ccredit.UsinaCodigo }
                        where ccredit.NumeroCartao == numeroCartao && ccredit.NumeroAutorizacao == autorizacao
                        && (obra.UsinaCodigo != idUsina || obra.Numero != obraNumero)
                        select obra)
                        .Include(c => c.Contrato.Interveniente)
                        .Include(c => c.Contrato.Vendedor)
                        .Include(c => c.Proposta.Interveniente)
                        .Include(c => c.Proposta.Vendedor)
                        .Include(c => c.UsinaEntrega)
                        .AsNoTracking().ToList()
                        .GroupBy(o => o)
                        .Select(g => g.First())
                        .ToList();

            return obras;
        }

        public IEnumerable<Obra> ListarPorCliente(int codCliente)
        {

            var obras = _context.Obras
                .Include(c => c.Contrato)
                .Where(c => c.Contrato.IntervenienteCodigo == codCliente);

            return obras;

        }

        public Obra ObterObraParaAnaliseCadastro(int idUsina, int obraNumero)
        {
            var obra = _context.Obras
                .Include(c => c.UsinaEntrega)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Analista)
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.TipoCobranca)
                .Include(c => c.EnderecoMunicipio)
                .Include(c => c.ObraTributacoesMunicipais)
                .Where(x => x.UsinaCodigo == idUsina && x.Numero == obraNumero)
                .FirstOrDefault();

            obra.ObraBombas = _context.ObraBombas
                .Include(c => c.BombaTipo)
                .Where(x => x.UsinaCodigo == idUsina && x.ObraCodigo == obraNumero).ToList();

            var analiseCadastroDesativada = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro").Equals("1");  

            if (analiseCadastroDesativada) 
                obra.PendenteAprovacaoDistanciaUsinaCEP = false;
            else
            {
                var usinaDistanciaCep = _context.UsinaDistanciasCep
                    .Where(c => c.Cep == obra.EnderecoCep && c.UsinaEntrega == obra.UsinaEntregaCodigo
                            && c.IdAprovacao == "").FirstOrDefault();

                obra.PendenteAprovacaoDistanciaUsinaCEP = usinaDistanciaCep != null;
            }
                
            if (obra.Contrato != null)
            {
                obra.Contrato.Interveniente.GrupoEconomico = _context.GruposEconomicos.Where(x => x.Codigo == obra.Contrato.Interveniente.GrupoEconomicoCodigo).FirstOrDefault();

                if (obra.Contrato.Interveniente.GrupoEconomico != null)
                    obra.Contrato.Interveniente.GrupoEconomico.BloqueioMotivo = _context.CadastrosGerais.Where(x => x.Codigo == obra.Contrato.Interveniente.GrupoEconomico.BloqueioMotivoCodigo).FirstOrDefault();
            }
                
            return obra;
        }

        public ObraVersao ObterObraParaAnaliseCadastro(int numVersao, int idUsina, int obraNumero)
        {
            var obra = _context.ObrasVersoes
                .Include(c => c.UsinaEntrega)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Analista)
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.TipoCobranca)
                .Include(c => c.EnderecoMunicipio)
                .Include(c => c.ObraTributacoesMunicipais)
                .Where(x => x.NumeroVersao == numVersao && x.UsinaCodigo == idUsina && x.Numero == obraNumero)
                .FirstOrDefault();

            obra.ObraBombas = _context.ObraBombasVersoes
                .Include(c => c.BombaTipo)
                .Where(x => x.NumeroVersao == numVersao &&  x.UsinaCodigo == idUsina && x.ObraCodigo == obraNumero).ToList();

            var analiseCadastroDesativada = _parametroRepository.ObterParametroN("web", "DesativarObrigatoriedadeAprovacaoCadastro").Equals("1");

            if (analiseCadastroDesativada)
                obra.PendenteAprovacaoDistanciaUsinaCEP = false;
            else
            {
                var usinaDistanciaCep = _context.UsinaDistanciasCep
                    .Where(c => c.Cep == obra.EnderecoCep && c.UsinaEntrega == obra.UsinaEntregaCodigo
                            && c.IdAprovacao == "").FirstOrDefault();

                obra.PendenteAprovacaoDistanciaUsinaCEP = usinaDistanciaCep != null;
            }

            obra.Contrato.Interveniente.GrupoEconomico = _context.GruposEconomicos.Where(x => x.Codigo == obra.Contrato.Interveniente.GrupoEconomicoCodigo).FirstOrDefault();

            if (obra.Contrato.Interveniente.GrupoEconomico != null)
                obra.Contrato.Interveniente.GrupoEconomico.BloqueioMotivo = _context.CadastrosGerais.Where(x => x.Codigo == obra.Contrato.Interveniente.GrupoEconomico.BloqueioMotivoCodigo).FirstOrDefault();

            return obra;
        }

        public Obra ObterPorIdAprovacaoComercial(int usina, int numero, bool tracking = true)
        {
            var obra = _context.Obras
                .Include(c => c.Proposta)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Interveniente.BloqueioMotivo)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.Contrato.ContratoTracoReajustes)
                .Include(c => c.EnderecoMunicipio)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.Numero == numero)
                .FirstOrDefault();

            obra.UsinaEntrega = _context.Usinas.Where(x => x.Codigo == obra.UsinaEntregaCodigo).FirstOrDefault();

            if (obra.Contrato != null)
            {
                obra.Contrato.Interveniente = _context.Intervenientes.Where(x => x.Codigo == obra.Contrato.IntervenienteCodigo).FirstOrDefault();
                obra.Contrato.Vendedor = _context.Vendedores.Where(x => x.Codigo == obra.Contrato.VendedorCodigo).FirstOrDefault();
            }
                
            obra.CondicaoPagamento = _context.CondicoesPagamento.Where(x => x.Codigo == obra.CondicaoPagamentoCodigo).FirstOrDefault();
            obra.TipoCobranca = _context.TiposCobranca.Where(x => x.Codigo == obra.TipoCobrancaCodigo).FirstOrDefault();

            obra.ObraTracos = _context.ObraTracos
                .Include(c => c.Uso)
                .Include(c => c.Pedra)
                .Include(c => c.Slump)
                .Include(c => c.ResistenciaTipo)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            obra.ObraBombas = _context.ObraBombas
                .Include(c => c.BombaTipo)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            obra.ObraTaxas = _context.ObraTaxas
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            obra.ObraFrentes = _context.ObraFrente
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            obra.ObraLogs = _context.ObraLogs
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            return obra;
        }

        public ObraVersao ObterPorIdAprovacaoComercial(int usina, int numero, int versao, bool tracking = true)
        {
            var obra = _context.ObrasVersoes
                .Include(c => c.Proposta)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Interveniente.BloqueioMotivo)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.Contrato.Vendedor)
                .Include(c => c.Contrato.ContratoTracoReajustes)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.EnderecoMunicipio)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.Numero == numero && x.NumeroVersao == versao)
                .FirstOrDefault();

            _context.Entry(obra).Reference(o => o.CondicaoPagamento).Load();
            _context.Entry(obra).Reference(o => o.TipoCobranca).Load();

            obra.ObraTracos = _context.ObraTracosVersoes
                .Include(c => c.Uso)
                .Include(c => c.Pedra)
                .Include(c => c.Slump)
                .Include(c => c.ResistenciaTipo)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.NumeroVersao == versao).ToList();

            obra.ObraBombas = _context.ObraBombasVersoes
                .Include(c => c.BombaTipo)
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.NumeroVersao == versao).ToList();

            obra.ObraFrentes = _context.ObraFrente
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero).ToList();

            obra.ObraTaxas = _context.ObraTaxasVersoes
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.NumeroVersao == versao).ToList();

            obra.ObraLogs = _context.ObraLogsVersoes
                .Tracking(tracking)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.NumeroVersao == versao).ToList();

            return obra;
        }


        public Obra ObterPendentePorId(int usina, int numero, int? anoChamada, int? noChamada)
        {
            var obra = _context.Obras
                .Include(c => c.Proposta)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Interveniente.BloqueioMotivo)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.Contrato.ContratoTracoReajustes)
                .Include(c => c.EnderecoMunicipio)
                .Where(x => x.UsinaCodigo == usina && x.Numero == numero && x.AnoChamada == anoChamada && x.NumChamada == noChamada)
                .FirstOrDefault();

            obra.UsinaEntrega = _context.Usinas.Where(x => x.Codigo == obra.UsinaEntregaCodigo).FirstOrDefault();

            if (obra.Contrato != null)
            {
                obra.Contrato.Interveniente = _context.Intervenientes.Where(x => x.Codigo == obra.Contrato.IntervenienteCodigo).FirstOrDefault();
                obra.Contrato.Vendedor = _context.Vendedores.Where(x => x.Codigo == obra.Contrato.VendedorCodigo).FirstOrDefault();
            }
                
            obra.CondicaoPagamento = _context.CondicoesPagamento.Where(x => x.Codigo == obra.CondicaoPagamentoCodigo).FirstOrDefault();
            obra.TipoCobranca = _context.TiposCobranca.Where(x => x.Codigo == obra.TipoCobrancaCodigo).FirstOrDefault();

            obra.ObraTracos = _context.ObraTracos
                .Include(c => c.Uso)
                .Include(c => c.Pedra)
                .Include(c => c.Slump)
                .Include(c => c.ResistenciaTipo)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.PropostaAno == anoChamada && x.PropostaNumero == noChamada).ToList();

            obra.ObraBombas = _context.ObraBombas
                .Include(c => c.BombaTipo)
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.PropostaAno == anoChamada && x.PropostaNumero == noChamada).ToList();

            obra.ObraLogs = _context.ObraLogs
                .Where(x => x.UsinaCodigo == usina && x.ObraCodigo == numero && x.AnoChamada == anoChamada && x.NumChamada == noChamada).ToList();

            /*obra.ObraTaxas = _context.ObraTaxas
                            .Where(x => x.UsinaCodigo == obra.UsinaEntregaCodigo && x.ObraCodigo == numero).ToList();*/
            
          //  if (obra.Contrato != null)
           //     obra.Contrato.ContratoTracoReajustes = _context.ContratoTracoReajustes.Where(x => x.UsinaCodigo == obra.Contrato.Usina && x.ContratoAno == obra.Contrato.Ano &&  x.ContratoNumero == obra.Contrato.Numero).ToList();

            //obra.ObraTaxas = BuscarTaxaByIdObra(obra.UsinaEntregaCodigo, obra.Numero);

            //obra.AprovacoesComerciais = BuscarAprovacaoComercialByIdObra(obra.UsinaCodigo, obra.Numero);

            return obra;
        }

        public ObraVersao ObterPendentePorId(int numVersao, int usina, int numero, int? anoChamada, int? noChamada)
        {
            var obra = _context.ObrasVersoes
                .Include(c => c.Proposta)
                .Include(c => c.Proposta.Interveniente)
                .Include(c => c.Proposta.Interveniente.BloqueioMotivo)
                .Include(c => c.Proposta.Vendedor)
                .Include(c => c.Contrato)
                .Include(c => c.Contrato.Interveniente)
                .Include(c => c.Contrato.Interveniente.BloqueioMotivo)
                .Include(c => c.Contrato.Vendedor)
                .Include(c => c.Contrato.ContratoTracoReajustes)
                .Include(c => c.UsinaEntrega)
                .Include(c => c.EnderecoMunicipio)
                .Where(x => x.NumeroVersao == numVersao && x.UsinaCodigo == usina && x.Numero == numero && x.AnoChamada == anoChamada && x.NumChamada == noChamada)
                .FirstOrDefault();

            _context.Entry(obra).Reference(o => o.CondicaoPagamento).Load();
            _context.Entry(obra).Reference(o => o.TipoCobranca).Load();
            

            obra.ObraTracos = _context.ObraTracosVersoes
                .Include(c => c.Uso)
                .Include(c => c.Pedra)
                .Include(c => c.Slump)
                .Include(c => c.ResistenciaTipo)
                .Where(x => x.NumeroVersao == numVersao && x.UsinaCodigo == usina && x.ObraCodigo == numero && x.PropostaAno == anoChamada && x.PropostaNumero == noChamada).ToList();

            obra.ObraBombas = _context.ObraBombasVersoes
                .Include(c => c.BombaTipo)
                .Where(x => x.NumeroVersao == numVersao && x.UsinaCodigo == usina && x.ObraCodigo == numero && x.PropostaAno == anoChamada && x.PropostaNumero == noChamada).ToList();

            obra.ObraLogs = _context.ObraLogsVersoes
                .Where(x => x.NumeroVersao == numVersao && x.UsinaCodigo == usina && x.ObraCodigo == numero && x.AnoChamada == anoChamada && x.NumChamada == noChamada).ToList();

            if (obra.Contrato != null)
                obra.Contrato.ContratoTracoReajustes = _context.ContratoTracoReajustesVersoes.Where(x => x.NumeroVersao == obra.NumeroVersao && x.UsinaCodigo == obra.Contrato.Usina && x.ContratoAno == obra.Contrato.Ano && x.ContratoNumero == obra.Contrato.Numero).ToList();

            //obra.ObraTaxas = BuscarTaxaByIdObra(obra.UsinaEntregaCodigo, obra.Numero);

            //obra.AprovacoesComerciais = BuscarAprovacaoComercialByIdObra(obra.UsinaCodigo, obra.Numero);

            return obra;
        }

        public bool TemAprovacaoBombaPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = _context.ObraBombas
                .AsNoTracking()
                .Where(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.PropostaAno == anoChamada
                    && t.PropostaNumero == noChamada
                    && t.AprovacaoVerbal == "S"
                ).ToList();

            return pendentes.Count > 0;
        }

        public bool TemAprovacaoBombaPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = _context.ObraBombasVersoes
                .AsNoTracking()
                .Where(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.PropostaAno == anoChamada
                    && t.PropostaNumero == noChamada
                    && t.NumeroVersao == numVersao
                    && t.AprovacaoVerbal == "S"
                ).ToList();

            return pendentes.Count > 0;
        }

        public bool TemAprovacaoTaxaExtraPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = (from obra in _context.Obras
                            join taxa in _context.ObraTaxas
                            on new { x1 = obra.Numero, x2 = obra.UsinaEntregaCodigo }
                            equals
                            new { x1 = taxa.ObraCodigo, x2 = taxa.UsinaCodigo }
                            where obra.UsinaCodigo == usina
                                && obra.Numero == numero
                                && obra.AnoChamada == anoChamada
                                && obra.NumChamada == noChamada
                                && taxa.Aprovada == "N"
                            select taxa).ToList();

            return pendentes.Count > 0;
        }

        public bool TemAprovacaoTaxaExtraPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = (from obra in _context.ObrasVersoes
                             join taxa in _context.ObraTaxasVersoes
                             on new { x1 = obra.Numero, x2 = obra.UsinaEntregaCodigo }
                             equals
                             new { x1 = taxa.ObraCodigo, x2 = taxa.UsinaCodigo }
                             where obra.UsinaCodigo == usina
                                 && obra.Numero == numero
                                 && obra.AnoChamada == anoChamada
                                 && obra.NumChamada == noChamada
                                 && taxa.Aprovada == "N"
                                 && obra.NumeroVersao == numVersao
                             select taxa).ToList();

            return pendentes.Count > 0;
        }

        public bool TemAprovacaoTracoPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = _context.ObraTracos
                .AsNoTracking()
                .Where(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.PropostaAno == anoChamada
                    && t.PropostaNumero == noChamada
                    && t.AprovacaoVerbal == "N"
                ).ToList();

            return pendentes.Count > 0;
        }

        public bool TemAprovacaoTracoPendente(int numVersao, int usina, int numero, int anoChamada, int noChamada)
        {
            var pendentes = _context.ObraTracosVersoes
                .AsNoTracking()
                .Where(t => t.UsinaCodigo == usina
                    && t.ObraCodigo == numero
                    && t.PropostaAno == anoChamada
                    && t.PropostaNumero == noChamada
                    && t.AprovacaoVerbal == "N"
                    && t.NumeroVersao == numVersao
                ).ToList();

            return pendentes.Count > 0;
        }

        public bool TemDemaisAprovacoesPendentes(int usina, int numero, int anoChamada, int noChamada)
        {
            StringBuilder sqlCommand = new StringBuilder();

            //Montando a lista contendo todos os tipos de Aprovação
            string aprovacaoTipo = string.Join(",", Enum.GetValues(typeof(EAprovacaoTipo)).Cast<int>());

            sqlCommand.Append("SELECT ");

            var columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Obra>(_context, "obras");
            sqlCommand.Append(columns);

            sqlCommand.Append(" FROM con_obras obras");
            sqlCommand.Append(" INNER JOIN con_aprov aprovacaoComercial");

            sqlCommand.Append(" ON SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 1 )=obras.usina ");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', -1 ),'-',1)=obras.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 2 ),'/',-1)=obras.no_chamada");

            sqlCommand.Append(" WHERE");

            sqlCommand.Append(" obras.usina=@USINA");
            sqlCommand.Append(" AND obras.numero=@NUMERO");
            sqlCommand.Append(" AND obras.ano_chamada=@ANO_CHAMADA");
            sqlCommand.Append(" AND obras.no_chamada=@NUM_CHAMADA");

            sqlCommand.Append(" AND (aprovacaoComercial.dt_hora_exec='1000-01-01 00:00:00' OR ISNULL(aprovacaoComercial.dt_hora_exec))");
            sqlCommand.Append(" AND aprovacaoComercial.tipo_aprov IN (@AprovacaoTipo)");

            sqlCommand.Append(" GROUP BY obras.usina, obras.ano_chamada, obras.no_chamada");


            var pendentes = _context.Obras.SqlQuery(sqlCommand.ToString(),
               new MySqlParameter("@USINA", usina),
               new MySqlParameter("@NUMERO", numero),
               new MySqlParameter("@ANO_CHAMADA", anoChamada),
               new MySqlParameter("@NUM_CHAMADA", noChamada),
               new MySqlParameter("@AprovacaoTipo", aprovacaoTipo)
               ).AsNoTracking()
               .ToList();

            return pendentes.Count > 0;
        }

        public bool TemDemaisAprovacoesPendentes(int numVersao, int usina, int numero, int anoChamada, int noChamada)
        {
            StringBuilder sqlCommand = new StringBuilder();

            //Montando a lista contendo todos os tipos de Aprovação
            string aprovacaoTipo = string.Join(",", Enum.GetValues(typeof(EAprovacaoTipo)).Cast<int>());

            sqlCommand.Append("SELECT ");

            var columns = EntityMapHelper.GetMappedColumnNamesAndPropertyNamesForSelect<Obra>(_context, "obras");
            sqlCommand.Append(columns);

            sqlCommand.Append(" FROM con_obras_versao obras");
            sqlCommand.Append(" INNER JOIN con_aprov_versao aprovacaoComercial");

            sqlCommand.Append(" ON SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 1 )=obras.usina ");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', -1 ),'-',1)=obras.ano_chamada");
            sqlCommand.Append(" AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 2 ),'/',-1)=obras.no_chamada");
            sqlCommand.Append(" AND aprovacaoComercial.num_versao=obras.num_versao");

            sqlCommand.Append(" WHERE");

            sqlCommand.Append(" obras.usina=@USINA");
            sqlCommand.Append(" AND obras.numero=@NUMERO");
            sqlCommand.Append(" AND obras.ano_chamada=@ANO_CHAMADA");
            sqlCommand.Append(" AND obras.no_chamada=@NUM_CHAMADA");
            sqlCommand.Append(" AND obras.num_versao=@NUM_VERSAO");

            sqlCommand.Append(" AND (aprovacaoComercial.dt_hora_exec='1000-01-01 00:00:00' OR ISNULL(aprovacaoComercial.dt_hora_exec))");
            sqlCommand.Append(" AND aprovacaoComercial.tipo_aprov IN (@AprovacaoTipo)");

            sqlCommand.Append(" GROUP BY obras.usina, obras.ano_chamada, obras.no_chamada");


            var pendentes = _context.Obras.SqlQuery(sqlCommand.ToString(),
               new MySqlParameter("@USINA", usina),
               new MySqlParameter("@NUMERO", numero),
               new MySqlParameter("@ANO_CHAMADA", anoChamada),
               new MySqlParameter("@NUM_CHAMADA", noChamada),
               new MySqlParameter("@NUM_VERSAO", numVersao),
               new MySqlParameter("@AprovacaoTipo", aprovacaoTipo)
               ).AsNoTracking()
               .ToList();

            return pendentes.Count > 0;
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.ano_chamada={anoProposta}");
            sqlComando.Append($" and c.no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_proposta_item_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_proposta_item c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.ano_chamada={anoProposta}");
            sqlComando.Append($" and c.no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_prop_bomba_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_prop_bomba c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.ano_chamada={anoProposta}");
            sqlComando.Append($" and c.no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_pag_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_chtel_pag c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.ano_chamada={anoProposta}");
            sqlComando.Append($" and c.num_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_log_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras_log c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_mp_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras_mp c");
            sqlComando.Append($" where c.usina={usinaProposta}");
            sqlComando.Append($" and c.obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_trib_mun_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras_trib_mun c");
            sqlComando.Append($" where c.usina_contrato={usinaProposta}");
            sqlComando.Append($" and c.no_obra={numObra}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_reajuste_versao");
            sqlComando.Append($" SELECT {numVersao}, r.* from topsys.con_obras_reajuste r");
            sqlComando.Append($" where r.usina={usinaProposta}");
            sqlComando.Append($" and r.obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            _contratoPagamentoRepository.AdicionarVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);

            if (numVersao > 1)
                _repasseReajusteRepository.AdicionarVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_obras_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_proposta_item_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_prop_bomba_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_pag_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and num_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_log_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_mp_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_trib_mun_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina_contrato={usinaProposta}");
            sqlComando.Append($" and no_obra={numObra}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_reajuste_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_indicador_versao");
            sqlComando.Append($" WHERE obra_versao={numVersao}");
            sqlComando.Append($" AND obra_usina={usinaProposta}");
            sqlComando.Append($" AND obra_numero={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            _contratoPagamentoRepository.ExcluirVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
            _repasseReajusteRepository.ExcluirVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public int ObterUltimaVersaoObra(int obraUsina, int obraNumero)
        {
            var ultimaVersaoObra = _context.ObrasVersoes.Where(x => x.UsinaCodigo == obraUsina
                                                                    && x.Numero == obraNumero)
                .OrderByDescending(x => x.NumeroVersao)
                .FirstOrDefault();

            if (ultimaVersaoObra == null)
                return 0;

            return ultimaVersaoObra.NumeroVersao;

        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();
            
            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_versao", "con_obras");

            sqlComando.Append($"REPLACE INTO topsys.con_obras");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numeroProposta}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_proposta_item_versao", "con_proposta_item");

            sqlComando.Append($"REPLACE INTO topsys.con_proposta_item");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_proposta_item_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numeroProposta}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_prop_bomba_versao", "con_prop_bomba");

            sqlComando.Append($"REPLACE INTO topsys.con_prop_bomba");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_prop_bomba_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numeroProposta}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_chtel_pag_versao", "con_chtel_pag");

            sqlComando.Append($"REPLACE INTO topsys.con_chtel_pag");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_chtel_pag_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND num_chamada={numeroProposta}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_log_versao", "con_obras_log");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_log");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_log_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_mp_versao", "con_obras_mp");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_mp");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_mp_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_trib_mun_versao", "con_obras_trib_mun");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_trib_mun");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_trib_mun_versao");
            sqlComando.Append($" WHERE usina_contrato={usinaProposta}");
            sqlComando.Append($" AND no_obra={numObra}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_reajuste_versao", "con_obras_reajuste");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_reajuste");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_reajuste_versao");
            sqlComando.Append($" WHERE usina={usinaProposta}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_indicador_versao", "con_obras_indicador");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_indicador");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_indicador_versao");
            sqlComando.Append($" WHERE obra_usina={usinaProposta}");
            sqlComando.Append($" AND obra_numero={numObra}");
            sqlComando.Append($" AND obra_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            _contratoPagamentoRepository.AdicionarContrato(codUsina, anoContrato, numeroContrato, numVersao);
            _repasseReajusteRepository.AdicionarContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato, int usinaProposta, int anoProposta, int numeroProposta, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            /*sqlComando.Append($"DELETE FROM topsys.con_obras");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();*/

            sqlComando.Append($"DELETE FROM topsys.con_proposta_item");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_prop_bomba");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and no_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_chtel_pag");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and ano_chamada={anoProposta}");
            sqlComando.Append($" and num_chamada={numeroProposta};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_log");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_mp");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_trib_mun");
            sqlComando.Append($" where usina_contrato={usinaProposta}");
            sqlComando.Append($" and no_obra={numObra}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_obras_reajuste");
            sqlComando.Append($" where usina={usinaProposta}");
            sqlComando.Append($" and obra={numObra};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            _contratoPagamentoRepository.ExcluirContrato(codUsina, anoContrato, numeroContrato);
            _repasseReajusteRepository.ExcluirContrato(codUsina, anoContrato, numeroContrato);
        }

        public float? ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTracoContrato)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT SUM(cn.qtde_m3_bt) FROM topsys.con_programacao prog");
            sqlComando.AppendLine("INNER JOIN topsys.con_nf cn ON ");
            sqlComando.AppendLine("     cn.usina_contrato = prog.usina AND");
            sqlComando.AppendLine("     cn.ano_contrato = prog.ano_contrato AND");
            sqlComando.AppendLine("     cn.num_contrato = prog.no_contrato AND");
            sqlComando.AppendLine("     cn.seq_prog = prog.seq_prog");
            sqlComando.AppendLine("WHERE");
            sqlComando.AppendLine($"    prog.usina = @{nameof(usinaContrato)}");
            sqlComando.AppendLine($"    AND prog.no_contrato = @{nameof(numeroContrato)}");
            sqlComando.AppendLine($"    AND prog.ano_contrato = @{nameof(anoContrato)}");
            sqlComando.AppendLine($"    AND prog.seq_item_cont = @{nameof(sequenciaTracoContrato)}");
            sqlComando.AppendLine($"    AND cn.motivo_cancel = 0");

            var consumo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usinaContrato, numeroContrato, anoContrato, sequenciaTracoContrato });
            sqlComando.Clear();

            return consumo;

        }

        public float? ObterConsumoPorTraco(int numeroContrato, int anoContrato, string traco, int interveniente)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine($"SELECT SUM(qtde_m3_bt) FROM topsys.con_nf");
            sqlComando.AppendLine($"WHERE num_contrato = {numeroContrato}");
            sqlComando.AppendLine($" AND ano_contrato = {anoContrato}");
            sqlComando.AppendLine($" AND traco_concreto = '{traco}'");
            sqlComando.AppendLine($" AND interv = '{interveniente}'");
            sqlComando.AppendLine($" AND motivo_cancel = 0");

            var consumo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString());
            sqlComando.Clear();

            return consumo;
        }

        public void AlterarMensagemObraReajuste(int codUsina, int codObra, string mensagem)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_reajuste");
            sqlComando.Append($" VALUES({codUsina}, {codObra}, '{mensagem}');");
            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public void AlterarMensagemObraReajusteVersao(int numVersao, int codUsina, int codObra, string mensagem)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_reajuste_versao");
            sqlComando.Append($" VALUES({numVersao}, {codUsina}, {codObra}, '{mensagem}');");
            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public int ObterSegmentacaoPropostaPorObra(int usinaEntregaCodigo, int obraNumero)
        {

            var sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT p.segmentacao FROM con_obras o");
            sqlComando.AppendLine("INNER JOIN con_chtel p ON p.usina = o.usina");
            sqlComando.AppendLine("AND p.ano_chamada = o.ano_chamada AND p.num_chamada = o.no_chamada");
            sqlComando.AppendLine("WHERE o.obra_usina = @Usina AND o.numero = @Numero");

            var parametros = new { Usina = usinaEntregaCodigo, Numero = obraNumero };
            var resultado = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString(), parametros);

            return resultado;

        }

        public Obra ObterObraPorContrato(int codUsina, int anoContrato, int numeroContrato, bool tracking = false)
        {
            return _context.Obras
                .Include(t => t.EnderecoMunicipio)
                .Where(t => t.UsinaCodigo == codUsina && t.AnoContrato == anoContrato && t.NumContrato == numeroContrato)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public void AtualizaObraTracoReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, float m3PrecoProposto, float valorServico, float descontoPercentual, ContratoTracoReajuste tracoReajuste)
        {
            var sqlComando = new StringBuilder();

            if (tracoReajuste != null)
            {
                sqlComando.Append($"UPDATE con_proposta_item SET");
                sqlComando.Append($" custo_serv_a=@{nameof(tracoReajuste.ValorServicoRecalculado)}");
                sqlComando.Append($",custo_serv_ant=@{nameof(tracoReajuste.ValorServicoVigente)}");
                sqlComando.Append($",pr_reajustado_a=@{nameof(tracoReajuste.PrecoRecalculado)}");
                sqlComando.Append($",pr_reajust_ant=@{nameof(tracoReajuste.PrecoVigente)}");
                sqlComando.Append($",dt_ult_reajuste=@{nameof(dataUltimoReajuste)}");
                sqlComando.Append($",preco_unit_prop=@{nameof(m3PrecoProposto)}");
                sqlComando.Append($",custo_servico=@{nameof(valorServico)}");
                sqlComando.Append($",pct_descto=@{nameof(descontoPercentual)}");
                sqlComando.Append($" WHERE usina=@{nameof(usina)}");
                sqlComando.Append($" AND no_obra=@{nameof(obraNumero)}");
                sqlComando.Append($" AND seq=@{nameof(sequencia)}");

                var filtro = new
                {
                    tracoReajuste.ValorServicoRecalculado,
                    tracoReajuste.ValorServicoVigente,
                    tracoReajuste.PrecoRecalculado,
                    tracoReajuste.PrecoVigente,
                    dataUltimoReajuste,
                    m3PrecoProposto,
                    valorServico,
                    descontoPercentual,
                    usina,
                    obraNumero,
                    sequencia
                };
                _context.Database.Connection.Execute(sqlComando.ToString(), filtro);
            }
        }

        public void AtualizaObraBombaReajuste(int usina, int obraNumero, int sequencia, DateTime dataUltimoReajuste, int m3PropostoAte, float taxaMinimaPrecoProposto, float m3PrecoProposto, float descontoPercentual, ContratoBombaReajuste bombaReajuste)
        {
            var sqlComando = new StringBuilder();

            if (bombaReajuste != null)
            {
                sqlComando.Append($"UPDATE con_prop_bomba SET");
                sqlComando.Append($" taxa_reajust_ant=@{nameof(bombaReajuste.ValorVigente)}");
                sqlComando.Append($",m3_pr_reajust_ant=@{nameof(bombaReajuste.VigenteAteM3)}");
                sqlComando.Append($",pr_m3_reajust_ant=@{nameof(bombaReajuste.M3ExcedenteVigente)}");
                sqlComando.Append($",taxa_reajustada=@{nameof(bombaReajuste.ValorReajustado)}");
                sqlComando.Append($",m3_pr_reajustada=@{nameof(bombaReajuste.ReajustadoAteM3)}");
                sqlComando.Append($",pr_m3_reajustada=@{nameof(bombaReajuste.M3ExcedenteReajustado)}");
                sqlComando.Append($",dt_ult_reajuste=@{nameof(dataUltimoReajuste)}");
                sqlComando.Append($",txa_min_pr_prop=@{nameof(taxaMinimaPrecoProposto)}");
                sqlComando.Append($",m3_pr_prop=@{nameof(m3PropostoAte)}");
                sqlComando.Append($",pr_m3_bomb_pr_p=@{nameof(m3PrecoProposto)}");
                sqlComando.Append($",pct_descto=@{nameof(descontoPercentual)}");
                sqlComando.Append($" WHERE usina=@{nameof(usina)}");
                sqlComando.Append($" AND no_obra=@{nameof(obraNumero)}");
                sqlComando.Append($" AND seq=@{nameof(sequencia)}");

                var filtro = new
                {
                    bombaReajuste.ValorVigente,
                    bombaReajuste.VigenteAteM3,
                    bombaReajuste.M3ExcedenteVigente,
                    bombaReajuste.ValorReajustado,
                    bombaReajuste.ReajustadoAteM3,
                    bombaReajuste.M3ExcedenteReajustado,
                    dataUltimoReajuste,
                    m3PropostoAte,
                    taxaMinimaPrecoProposto,
                    m3PrecoProposto,
                    descontoPercentual,
                    usina,
                    obraNumero,
                    sequencia
                };
                _context.Database.Connection.Execute(sqlComando.ToString(), filtro);
            }
        }

        public IEnumerable<ObraProjecao> ListarProjecaoPorObra(int usina, int numero, int? anoChamada, int? noChamada)
        {
            var obraProjecao = _context.ObraProjecao
                .Where(t => t.Usina== usina
                    && t.NoObra == numero
                    && t.AnoChamada == anoChamada
                    && t.NoChamada == noChamada)
                .AsNoTracking()
                .ToList();

            return obraProjecao;
        }

        public float? ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT SUM(cn.qtde_m3_bt) FROM topsys.con_programacao prog");
            sqlComando.AppendLine("INNER JOIN topsys.con_nf cn ON ");
            sqlComando.AppendLine("     cn.usina_contrato = prog.usina AND");
            sqlComando.AppendLine("     cn.ano_contrato = prog.ano_contrato AND");
            sqlComando.AppendLine("     cn.num_contrato = prog.no_contrato AND");
            sqlComando.AppendLine("     cn.seq_prog = prog.seq_prog");
            sqlComando.AppendLine("WHERE");
            sqlComando.AppendLine($"    prog.usina = @{nameof(usinaContrato)}");
            sqlComando.AppendLine($"    AND prog.no_contrato = @{nameof(numeroContrato)}");
            sqlComando.AppendLine($"    AND prog.ano_contrato = @{nameof(anoContrato)}");
            sqlComando.AppendLine($"    AND cn.motivo_cancel = 0");

            var consumo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usinaContrato, numeroContrato, anoContrato });
            sqlComando.Clear();

            return consumo;
        }

        public float? ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT sum(qtde_m3) from con_proposta_item");
            sqlComando.AppendLine(" WHERE");
            sqlComando.AppendLine($" usina = @{nameof(usina)}");
            sqlComando.AppendLine($" AND no_obra = @{nameof(noObra)}");
            sqlComando.AppendLine($" AND ano_chamada = @{nameof(anoChamada)}");
            sqlComando.AppendLine($" AND no_chamada = @{nameof(noChamada)}");

            var volume = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usina, noObra, anoChamada, noChamada });
            sqlComando.Clear();

            return volume;
        }

        public float? ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            DateTime dataAtual = DateTime.Now;

            DateTime periodo = new DateTime(dataAtual.Year, dataAtual.Month, 1);

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT SUM(cn.qtde_m3_bt) FROM topsys.con_programacao prog");
            sqlComando.AppendLine("INNER JOIN topsys.con_nf cn ON ");
            sqlComando.AppendLine("     cn.usina_contrato = prog.usina AND");
            sqlComando.AppendLine("     cn.ano_contrato = prog.ano_contrato AND");
            sqlComando.AppendLine("     cn.num_contrato = prog.no_contrato AND");
            sqlComando.AppendLine("     cn.seq_prog = prog.seq_prog");
            sqlComando.AppendLine("WHERE");
            sqlComando.AppendLine($"    prog.usina = @{nameof(usinaContrato)}");
            sqlComando.AppendLine($"    AND prog.no_contrato = @{nameof(numeroContrato)}");
            sqlComando.AppendLine($"    AND prog.ano_contrato = @{nameof(anoContrato)}");
            sqlComando.AppendLine($"    AND cn.motivo_cancel = 0");
            sqlComando.AppendLine($"    AND cn.data_remessa < @{nameof(periodo)}");

            var consumo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usinaContrato, numeroContrato, anoContrato, periodo });
            sqlComando.Clear();

            return consumo;
        }

        public float? ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {
            DateTime dataAtual = DateTime.Now;

            DateTime periodo = new DateTime(dataAtual.Year, dataAtual.Month, 1);

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT SUM(cn.qtde_m3_bt) FROM topsys.con_programacao prog");
            sqlComando.AppendLine("INNER JOIN topsys.con_nf cn ON ");
            sqlComando.AppendLine("     cn.usina_contrato = prog.usina AND");
            sqlComando.AppendLine("     cn.ano_contrato = prog.ano_contrato AND");
            sqlComando.AppendLine("     cn.num_contrato = prog.no_contrato AND");
            sqlComando.AppendLine("     cn.seq_prog = prog.seq_prog");
            sqlComando.AppendLine("WHERE");
            sqlComando.AppendLine($"    prog.usina = @{nameof(usinaContrato)}");
            sqlComando.AppendLine($"    AND prog.no_contrato = @{nameof(numeroContrato)}");
            sqlComando.AppendLine($"    AND prog.ano_contrato = @{nameof(anoContrato)}");
            sqlComando.AppendLine($"    AND cn.motivo_cancel = 0");
            sqlComando.AppendLine($"    AND cn.data_remessa >= @{nameof(periodo)}");

            var consumo = _context.Database.Connection.QueryFirstOrDefault<float?>(sqlComando.ToString(), new { usinaContrato, numeroContrato, anoContrato, periodo });
            sqlComando.Clear();

            return consumo;
        }

        public void AtualizarDadosReajuste(ObraTraco obraTraco)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_proposta_item SET custo_serv_a=@{nameof(ObraTraco.CustoServicoReajustado)}");
            sqlComando.Append($", custo_serv_ant=@{nameof(ObraTraco.CustoServicoAnterior)}");
            sqlComando.Append($", pr_reajustado_a=@{nameof(ObraTraco.PrecoReajustadoAtual)}");
            sqlComando.Append($", pr_reajust_ant=@{nameof(ObraTraco.PrecoReajustadoAnterior)}");
            sqlComando.Append($", dt_ult_reajuste=@{nameof(ObraTraco.DataUltimoReajuste)}");
            sqlComando.Append($" WHERE usina=@{nameof(ObraTraco.UsinaCodigo)}");
            sqlComando.Append($" AND no_obra=@{nameof(ObraTraco.ObraCodigo)}");
            sqlComando.Append($" AND seq=@{nameof(ObraTraco.Sequencia)}");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                obraTraco.CustoServicoReajustado,
                obraTraco.CustoServicoAnterior,
                obraTraco.PrecoReajustadoAtual,
                obraTraco.PrecoReajustadoAnterior,
                obraTraco.DataUltimoReajuste,
                obraTraco.UsinaCodigo,
                obraTraco.ObraCodigo,
                obraTraco.Sequencia
            });
		}

        public void ObterStatusObra(int obraUsina, int obraNumero, int versao, out int obraStatusCadastro, out int obraStatusComercial, out int statusContrato)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"   o.status_cadastro Cadastro,");
            sql.AppendLine($"   o.status_comercial Comercial,");
            sql.AppendLine($"   c.status Contrato");
            sql.AppendLine($"FROM con_obras{(versao > 0 ? "_versao" : "")} o");
            sql.AppendLine($"INNER JOIN con_contrato{(versao > 0 ? "_versao" : "")} c ON");
            sql.AppendLine($"   c.usina = o.usina");
            sql.AppendLine($"   AND c.num_contrato = o.no_contrato");
            sql.AppendLine($"   AND c.ano_contrato = o.ano_contrato");
			sql.AppendLine($"   {(versao > 0 ? $"AND c.num_versao = o.num_versao" : "")}");
            sql.AppendLine($"WHERE");
            sql.AppendLine($"   o.usina = @{nameof(obraUsina)}");
            sql.AppendLine($"   AND o.numero  =@{nameof(obraNumero)}");
            sql.AppendLine($"   {(versao > 0 ? $"AND o.num_versao = @{nameof(versao)}" : "")}");

            var result = _context.Database.Connection.QueryFirstOrDefault(sql.ToString(), new { obraUsina, obraNumero, versao });

            obraStatusCadastro = result.Cadastro;
            obraStatusComercial = result.Comercial;
            statusContrato = result.Contrato;


        }

        public int ObterCondicaoPagamentoPorContrato(int usina,int anoContrato, int numeroContrato,int versao)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("    o.cond_pgto");
            sql.AppendLine($"FROM con_obras{(versao > 0 ? "_versao" : "")} o");
            sql.AppendLine($"WHERE o.usina = @{nameof(usina)}");
            sql.AppendLine($"  AND o.ano_contrato = @{nameof(anoContrato)}");
            sql.AppendLine($"  AND o.no_contrato = @{nameof(numeroContrato)}");
            if (versao > 0)
                sql.AppendLine($"  AND o.num_versao = @{nameof(versao)}");

            var result = _context.Database.Connection.QueryFirstOrDefault<int>(
                sql.ToString(),
                new { usina, anoContrato, numeroContrato, versao });

            return result;
        }


        public void AtualizarDadosReajuste(ObraBomba obraBomba)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_prop_bomba SET taxa_reajust_ant=@{nameof(ObraBomba.TaxaMinimaReajustadaAnterior)}");
            sqlComando.Append($", m3_pr_reajust_ant=@{nameof(ObraBomba.M3ReajustadoAteAnterior)}");
            sqlComando.Append($", pr_m3_reajust_ant=@{nameof(ObraBomba.M3PrecoReajustadoAnterior)}");
            sqlComando.Append($", taxa_reajustada=@{nameof(ObraBomba.TaxaMinimaReajustadaAtual)}");
            sqlComando.Append($", m3_pr_reajustada=@{nameof(ObraBomba.M3ReajustadoAteAtual)}");
            sqlComando.Append($", pr_m3_reajustada=@{nameof(ObraBomba.M3PrecoReajustadoAtual)}");
            sqlComando.Append($", dt_ult_reajuste=@{nameof(ObraBomba.DataUltimoReajuste)}");
            sqlComando.Append($" WHERE usina=@{nameof(ObraBomba.UsinaCodigo)}");
            sqlComando.Append($" AND no_obra=@{nameof(ObraBomba.ObraCodigo)}");
            sqlComando.Append($" AND seq=@{nameof(ObraBomba.Sequencia)}");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                obraBomba.TaxaMinimaReajustadaAnterior,
                obraBomba.M3ReajustadoAteAnterior,
                obraBomba.M3PrecoReajustadoAnterior,
                obraBomba.TaxaMinimaReajustadaAtual,
                obraBomba.M3ReajustadoAteAtual,
                obraBomba.M3PrecoReajustadoAtual,
                obraBomba.DataUltimoReajuste,
                obraBomba.UsinaCodigo,
                obraBomba.ObraCodigo,
                obraBomba.Sequencia
            });

        }

        public void alterarStatusContratoPelaObra(int obraUsina, int obraNumero, int obraVersao, int novoStatus)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"   no_contrato Numero,");
            sql.AppendLine($"   ano_contrato Ano");
            sql.AppendLine($"FROM con_obras{(obraVersao > 0 ? "_versao" : "")}");
            sql.AppendLine($"WHERE");
            sql.AppendLine($"   usina = @{nameof(obraUsina)}");
            sql.AppendLine($"   AND numero = @{nameof(obraVersao)}");
            sql.AppendLine($"   {(obraVersao > 0 ? $"AND num_versao = @{nameof(obraVersao)}" : "")}");

            var contrato = _context.Database.Connection.QueryFirstOrDefault(sql.ToString(), new { obraUsina, obraNumero, obraVersao });

            sql.Clear();

            sql.AppendLine($"UPDATE con_contrato{(obraVersao > 0 ? "_versao" : "")}");
            sql.AppendLine($"   SET status = @{nameof(novoStatus)}");
            sql.AppendLine($"WHERE");
            sql.AppendLine($"   usina = @{nameof(obraUsina)}");
            sql.AppendLine($"   AND num_contrato = @{nameof(contrato.Numero)}");
            sql.AppendLine($"   AND ano_contrato = @{nameof(contrato.Ano)}");
            sql.AppendLine($"   {(obraVersao > 0 ? $"AND num_versao = @{nameof(obraVersao)}" : "")}");

            _context.Database.Connection.Execute(sql.ToString(), new { novoStatus, obraUsina, contrato.Numero, contrato.Ano });

        }

        public void AtualizarValorReajustePropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, float valorReajustado, float valorServico, float descontoPercentual)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_proposta_item_versao SET");
            sqlComando.Append($" preco_unit_prop=@{nameof(valorReajustado)}");
            sqlComando.Append($", custo_servico=@{nameof(valorServico)}");
            sqlComando.Append($", pct_descto=@{nameof(descontoPercentual)}");
            sqlComando.Append($", pr_reajust_ant=0");
            sqlComando.Append($", pr_reajustado_a=0");
            sqlComando.Append($", custo_serv_ant=0");
            sqlComando.Append($", custo_serv_a=0");
            sqlComando.Append($", dt_ult_reajuste=null");
            sqlComando.Append($" WHERE usina={usina}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numProposta}");
            sqlComando.Append($" AND seq={sequencia}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                numVersao,
                usina,
                anoProposta,
                numProposta,
                sequencia,
                valorReajustado,
                valorServico,
                descontoPercentual
            });

            sqlComando.Clear();
            sqlComando.Append($"UPDATE con_proposta_item SET");
            sqlComando.Append($" pr_reajustado_a=@{nameof(valorReajustado)}");
            sqlComando.Append($", custo_serv_a=@{nameof(valorServico)}");
            sqlComando.Append($" WHERE usina={usina}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numProposta}");
            sqlComando.Append($" AND seq={sequencia};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                usina,
                anoProposta,
                numProposta,
                sequencia,
                valorReajustado,
                valorServico
            });

        }

        public void AtualizarValorReajustePropostaBombaVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, int m3ReajustadoAteAtual, float taxaMinimaReajustadaAtual, float m3PrecoReajustadoAtual, float descontoPercentual)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_prop_bomba_versao SET");
            sqlComando.Append($" txa_min_pr_prop=@{nameof(taxaMinimaReajustadaAtual)}");
            sqlComando.Append($", m3_pr_prop=@{nameof(m3ReajustadoAteAtual)}");
            sqlComando.Append($", pr_m3_bomb_pr_p=@{nameof(m3PrecoReajustadoAtual)}");
            sqlComando.Append($", pct_descto=@{nameof(descontoPercentual)}");
            sqlComando.Append($", taxa_reajust_ant=0");
            sqlComando.Append($", m3_pr_reajust_ant=0");
            sqlComando.Append($", pr_m3_reajust_ant=0");
            sqlComando.Append($", taxa_reajustada=0");
            sqlComando.Append($", m3_pr_reajustada=0");
            sqlComando.Append($", pr_m3_reajustada=0");
            sqlComando.Append($", dt_ult_reajuste=null");
            sqlComando.Append($" WHERE usina={usina}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numProposta}");
            sqlComando.Append($" AND seq={sequencia}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                numVersao,
                usina,
                anoProposta,
                numProposta,
                sequencia,
                taxaMinimaReajustadaAtual,
                m3ReajustadoAteAtual,
                m3PrecoReajustadoAtual,
                descontoPercentual
            });

            sqlComando.Clear();
            sqlComando.Append($"UPDATE con_prop_bomba SET");
            sqlComando.Append($" taxa_reajustada=@{nameof(taxaMinimaReajustadaAtual)}");
            sqlComando.Append($", m3_pr_reajustada=@{nameof(m3ReajustadoAteAtual)}");
            sqlComando.Append($", pr_m3_reajustada=@{nameof(m3PrecoReajustadoAtual)}");
            sqlComando.Append($" WHERE usina={usina}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numProposta}");
            sqlComando.Append($" AND seq={sequencia};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                usina,
                anoProposta,
                numProposta,
                sequencia,
                taxaMinimaReajustadaAtual,
                m3ReajustadoAteAtual,
                m3PrecoReajustadoAtual
            });

        }

        public void AtualizarTracoAtivoPropostaItemVersao(int numVersao, int usina, int anoProposta, int numProposta, int sequencia, string ativo)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE con_proposta_item_versao SET");
            sqlComando.Append($" ativo=@{nameof(ativo)}");
            sqlComando.Append($" WHERE usina={usina}");
            sqlComando.Append($" AND ano_chamada={anoProposta}");
            sqlComando.Append($" AND no_chamada={numProposta}");
            sqlComando.Append($" AND seq={sequencia}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString(), new
            {
                numVersao,
                usina,
                anoProposta,
                numProposta,
                sequencia,
                ativo
            });
        }

        public int ObterTempoDescarga(int idUsina)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT tempo_bt_usina FROM con_ponto_carga WHERE usina={idUsina} LIMIT 1");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }
    }
}

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TituloContasAReceberRepository : RepositoryBase<TituloContasAReceber>, ITituloContasAReceberRepository
    {
        const int DOCUMENTO_TIPO_CARTAO = 88;
        private readonly IdentityHelperService _identityHelperService;

        private readonly IObraRepository _obraRepository;     

        public TituloContasAReceberRepository(AppDataContext context, IdentityHelperService identityHelperService, IObraRepository obraRepository) : base(context)
        {
            _identityHelperService = identityHelperService;
            _obraRepository = obraRepository;
        }

        private IEnumerable<ChaveTituloContasAReceber> ObterChaveDosTitulosDaJuncao(TituloContasAReceber tituloContasAReceber)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT emp {nameof(ChaveTituloContasAReceber.EmpresaCodigo)}");
            sqlComando.Append($", tp_doc {nameof(ChaveTituloContasAReceber.DocumentoTipoCodigo)}");
            sqlComando.Append($", ser_doc {nameof(ChaveTituloContasAReceber.DocumentoSerie)}");
            sqlComando.Append($", num_doc {nameof(ChaveTituloContasAReceber.DocumentoNumero)}");
            sqlComando.Append($", seq {nameof(ChaveTituloContasAReceber.DocumentoSequencia)}");
            sqlComando.Append($", cod_banco_band {nameof(ChaveTituloContasAReceber.BancoCodigoOficial)}");
            sqlComando.Append($", num_agencia {nameof(ChaveTituloContasAReceber.BancoNumeroAgencia)}");
            sqlComando.Append($", num_conta {nameof(ChaveTituloContasAReceber.BancoNumeroConta)}");
            sqlComando.Append($", dv_conta {nameof(ChaveTituloContasAReceber.BancoDvConta)}");
            sqlComando.Append($" FROM fin_car_juncao");
            sqlComando.Append($" WHERE empj=@empresa");
            sqlComando.Append($" AND tp_docj=@tpDoc");
            sqlComando.Append($" AND ser_docj=@serDoc");
            sqlComando.Append($" AND num_docj=@numDoc");
            sqlComando.Append($" AND seqj=@seq");
            sqlComando.Append($" AND cod_banco_bandj=@numBanco");
            sqlComando.Append($" AND num_agenciaj=@numAgencia");
            sqlComando.Append($" AND num_contaj=@numConta");
            sqlComando.Append($" AND dv_contaj=@dvConta");

            return _context.Database.Connection.Query<ChaveTituloContasAReceber>(sqlComando.ToString(), new
            {
                empresa = tituloContasAReceber.EmpresaCodigo,
                tpDoc = tituloContasAReceber.DocumentoTipoCodigo,
                serDoc = tituloContasAReceber.DocumentoSerie,
                numDoc = tituloContasAReceber.DocumentoNumero,
                seq = tituloContasAReceber.DocumentoSequencia,
                numBanco = tituloContasAReceber.BancoCodigoOficial,
                numAgencia = tituloContasAReceber.BancoNumeroAgencia,
                numConta = tituloContasAReceber.BancoNumeroConta,
                dvConta = tituloContasAReceber.BancoDvConta
            });
        }

        public IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao)
        {
            var _numeroCartao = numeroCartao.ToString().PadLeft(4, '0');

            var titulos = _context.TitulosContasAReceber
                .Where(t => t.DocumentoTipoCodigo == DOCUMENTO_TIPO_CARTAO
                    && t.CartaoNumero == _numeroCartao
                    && t.CartaoAutorizacao == autorizacao
                )
                .AsNoTracking()
                .ToList();
    
            ObterCondicaoPagamento(titulos);
      
            return titulos;
        }

        private void ObterCondicaoPagamento(TituloContasAReceber contasAReceber)
        {
            if (contasAReceber == null) return;

            var condicao = _obraRepository.ObterCondicaoPagamentoPorContrato(
                contasAReceber.ContratoUsinaCodigo, 
                contasAReceber.ContratoAno,
                contasAReceber.ContratoNumero,
                contasAReceber.ContratoVersao
            );

            contasAReceber.CondicaoPagamento = condicao;
        }

        private void ObterCondicaoPagamento(List<TituloContasAReceber> contasAReceber)
        {
            if (contasAReceber == null) return;

            foreach (var conta in contasAReceber)
            {
                var condicao = _obraRepository.ObterCondicaoPagamentoPorContrato(
                     conta.ContratoUsinaCodigo,
                     conta.ContratoAno,
                     conta.ContratoNumero,
                     conta.ContratoVersao
                 );

                conta.CondicaoPagamento = condicao;
            }
        }

        private void ObterCondicoesPagamento(IEnumerable<TituloContasAReceber> lista)
        {
            foreach (var item in lista)
            {
                var condicao = _obraRepository.ObterCondicaoPagamentoPorContrato(
                    item.ContratoUsinaCodigo,
                    item.ContratoAno,
                    item.ContratoNumero,
                    item.ContratoVersao
                );

                item.CondicaoPagamento = condicao;
            }
        }


        public IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao)
        {
            var _numeroCartao = numeroCartao.ToString().PadLeft(4, '0');

            var titulos = _context.TitulosContasAReceber
                .Where(t => t.DocumentoTipoCodigo == DOCUMENTO_TIPO_CARTAO
                    && t.CartaoNumero == _numeroCartao
                    && t.CartaoAutorizacao == autorizacao
                    && (t.ContratoUsinaCodigo != idUsina || t.ContratoAno != contratoAno || t.ContratoNumero != contratoNumero)
                )
                .AsNoTracking()
                .ToList();

            ObterCondicaoPagamento(titulos);

            return titulos;
        }

        public TituloContasAReceber ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento,
            int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv,
            int desdobramento, bool dapper = false)
        {
            TituloContasAReceber tituloContasAReceber;

            if (dapper) {
                var sqlComando = TituloContasAReceberSelectSql(mostraDadosFatura: true);

                sqlComando.Append($" WHERE car.emp={empresa} AND car.tp_doc={tipoDocumento} AND car.ser_doc='{serieDocumento}' AND car.num_doc={numeroDocumento} ");
                sqlComando.Append($"AND car.seq={sequencia} AND car.cod_banco_band={codBancoBand} AND car.num_agencia={numAgencia} AND car.num_conta={numConta} ");
                sqlComando.Append($"AND car.dv_conta={numContaDv} AND car.desdo={desdobramento}");

                tituloContasAReceber = _context.Connection.QueryFirstOrDefault<TituloContasAReceber>(sqlComando.ToString());
            }
            else {

                 tituloContasAReceber = _context.TitulosContasAReceber
                    .Where(t => t.EmpresaCodigo == empresa && t.DocumentoTipoCodigo == tipoDocumento &&
                                t.DocumentoSerie == serieDocumento && t.DocumentoNumero == numeroDocumento &&
                                t.DocumentoSequencia == sequencia.ToString() && t.BancoCodigoOficial == codBancoBand &&
                                t.BancoNumeroAgencia == numAgencia && t.BancoNumeroConta == numConta &&
                                t.BancoDvConta == numContaDv && t.Desdobramento == desdobramento)
                    .AsNoTracking()
                    .FirstOrDefault();
            }

            if (tituloContasAReceber != null)
            {
                tituloContasAReceber.ChaveTitulosDaJuncao = ObterChaveDosTitulosDaJuncao(tituloContasAReceber);
            }

            ObterCondicaoPagamento(tituloContasAReceber);

            return tituloContasAReceber;
        }

        public ICollection<TituloContasAReceber>  ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento,
            int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {

            var titulosContasAReceber =  _context.TitulosContasAReceber
                .Where(t => t.EmpresaCodigo == empresa && t.DocumentoTipoCodigo == tipoDocumento &&
                            t.DocumentoSerie == serieDocumento && t.DocumentoNumero == numeroDocumento && t.BancoCodigoOficial == codBancoBand &&
                            t.BancoNumeroAgencia == numAgencia && t.BancoNumeroConta == numConta &&
                            t.BancoDvConta == numContaDv)
                .AsNoTracking()
                .OrderBy(t => t.Desdobramento)
                .ThenBy(t => t.DocumentoSequencia)
                .ToList();

            foreach (var titulo in titulosContasAReceber)
            {
                titulo.ChaveTitulosDaJuncao = ObterChaveDosTitulosDaJuncao(titulo);
            }

            ObterCondicaoPagamento(titulosContasAReceber);

            return titulosContasAReceber;

        }

        public bool VerificaSeExisteEmTabelasRelacionadas(string fieldValue, string fieldName, string tableName)
        {
            if (fieldValue == "")
            {
                return true;
            }
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT 1 FROM {tableName} WHERE {fieldName} = {fieldValue}");

            return _context.Database.Connection.Query<int>(sqlComando.ToString()).Any();
        }

        public ICollection<TituloContasAReceber> ListarComPaginacao(DateTime? dataEmissao, DateTime? dataOperacao, int? tipoDocumento, int? centroCusto, string serieDocumento, long? numeroDocumento, int? cliente, int pagina, int limite)
        {
            var query = _context.TitulosContasAReceber.AsQueryable();

            if (dataEmissao != null) query = query.Where(t => t.DataEmissao == dataEmissao);
            if (dataOperacao != null) query = query.Where(t => t.DataOperacao == dataOperacao);
            if (tipoDocumento != null && tipoDocumento != 0) query = query.Where(t => t.DocumentoTipoCodigo == tipoDocumento);
            if (cliente != null && cliente != 0) query = query.Where(t => t.IntervenienteCodigo == cliente);

            if (centroCusto != null) query = query.Where(t => t.CentroCustoCodigo == centroCusto);
            if (serieDocumento != null) query = query.Where(t => t.DocumentoSerie == serieDocumento);
            if (numeroDocumento != null) query = query.Where(t => t.DocumentoNumero == numeroDocumento);

            var titulosContasAReceber = query.OrderBy(t => t.EmpresaCodigo).Skip((pagina - 1) * limite).Take(limite).ToList(); 
            
            foreach (var titulo in titulosContasAReceber)
            {
                titulo.ChaveTitulosDaJuncao = ObterChaveDosTitulosDaJuncao(titulo);
            }

            ObterCondicaoPagamento(titulosContasAReceber);

            return titulosContasAReceber;
        }

        public Tuple<long, long> RetornaParametrosDeMovimentoBancario(int? operacaoMovimentoBancario)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT nat_Finan FROM fin_operacao");
            sqlComando.Append($" WHERE cod={operacaoMovimentoBancario}");

            var naturezaFinanceira = _context.Database.Connection.QueryFirstOrDefault<long>(sqlComando.ToString());

            long proxLoteBaixa = _context.Database.Connection.QueryFirstOrDefault<long>("SELECT MAX(prox_lote_baixa) AS lote FROM fin_controle");
            _context.Database.Connection.Execute("update fin_controle set prox_lote_baixa=prox_lote_baixa+1");


            return Tuple.Create(naturezaFinanceira, proxLoteBaixa);
        }

        public int? RetornaBancoDeLiquidacao(int? operacaoLiquidacao)
        {
            return _context.Database.Connection.QueryFirstOrDefault<int?>($"select at_bco from fin_operacao where cod={operacaoLiquidacao}");
        }

        public int? RetornaDesdobramentoMaximo(int? empresaCodigo,int? documentoTipoCodigo, string documentoSerie, long? documentoNumero, string documentoSequencia)
        {
            StringBuilder chaveWhereTitulo = new StringBuilder();

            chaveWhereTitulo.Append($" WHERE emp={empresaCodigo} and tp_doc={documentoTipoCodigo} and ser_doc='{documentoSerie}' and num_doc={documentoNumero} and seq={documentoSequencia}");

            int? desdo = _context.Database.Connection.QueryFirstOrDefault<int?>($"select max(desdo)+1 from fin_car" + chaveWhereTitulo.ToString());
            desdo = desdo ?? 1;

            return desdo;
        }

        public int? ValidaOperacaoMovimentoBancario(int? operacaoLiquidacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT movOp.Sub_Sist from fin_operacao movOp");
            sqlComando.Append(" JOIN fin_operacao liqOp ON movOp.cod = liqOp.Op_Bco");
            sqlComando.Append($" WHERE liqOp.cod ={operacaoLiquidacao}");

            string result = _context.Database.Connection.QueryFirstOrDefault<string>(sqlComando.ToString());

            if (result == "BC")
            {
                StringBuilder sqlComando1 = new StringBuilder();
                sqlComando1.Append("SELECT op_bco from fin_operacao");
                sqlComando1.Append($" WHERE cod={operacaoLiquidacao}");

                return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando1.ToString());
            }
            else { return null; }

        }

        public int? ValidaOperacaoBaixa(int? operacaoBaixa)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT cod FROM fin_operacao WHERE");
            sqlComando.Append(" sub_sist='CR' and IB='B' AND inativa='N'");
            sqlComando.Append(" AND cod <> all (SELECT DISTINCT op_bx FROM fin_operacao WHERE op_bx <> 0)");
            sqlComando.Append($" AND cod={operacaoBaixa}");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
        }

        public bool? ValidaBancoLiquidacao(int? empresa, int? operacaoLiquidacao, int? bancoLiquidacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT 1 FROM ger_banco WHERE emp={empresa} AND bco={bancoLiquidacao}");
            if (operacaoLiquidacao == 6) sqlComando.Append($" AND Bco_Of=0");

            return _context.Database.Connection.Query<int>(sqlComando.ToString()).Any();
        }

        public string ObterLinkSegundaViaBoleto(int codigoBanco)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"SELECT link");
            sqlCommand.Append($" FROM topsys.ger_bancos_segunda_via ");
            sqlCommand.Append($" WHERE ");
            sqlCommand.Append($" cod_banco={codigoBanco}");

            return _context.Database.Connection.QueryFirstOrDefault<string>(sqlCommand.ToString());
        }

        public Tuple<int, int> RetornaTipoMovimentoEBaixa(int empresa)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT oper_bx_desc as operBxDesc,tp_doc_mov_bco as tpMovimentoBancario FROM fin_parametro");
            sqlComando.Append($" WHERE emp={empresa} AND inicio_validade<=DATE(NOW())");
            sqlComando.Append($" ORDER BY inicio_validade DESC LIMIT 1");

            var result = _context.Database.Connection.Query<dynamic>(sqlComando.ToString()).Single();

            int tipoMovimentoBancario = int.Parse(Convert.ToString(result.tpMovimentoBancario));
            int operacaoBaixaDesc = int.Parse(Convert.ToString(result.operBxDesc));

            return Tuple.Create(tipoMovimentoBancario, operacaoBaixaDesc);
        }

        public void CriaEAtualizaVinculoMovimentoTitulo(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int? documentoSequencia, int desdobramento, long documentoNumero, float valor, long interveniente, long idMovimentoBancario)
        {
            StringBuilder sqlComando = new StringBuilder();
            StringBuilder sqlComando1 = new StringBuilder();

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");


            var clienteFavorecido = _context.Database.Connection.Query<string>($"SELECT razao FROM ger_interv WHERE cod={interveniente}").Single();

            sqlComando.Append("INSERT INTO fin_vinc_car_mov_bco");
            sqlComando.Append($" SET id_mov_bco={idMovimentoBancario},emp={empresaCodigo}, tp_doc={documentoTipoCodigo}, ser_doc='{documentoSerie}', num_doc={documentoNumero}, seq={documentoSequencia}, desdo={desdobramento}, valor=");

            string comandoSql = sqlComando.ToString() + string.Format(culture, "{0:0.00}", (decimal)valor);

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_vinc_car_mov_bco", sqlComando.ToString());

            sqlComando1.Append("UPDATE fin_mov_banco");
            sqlComando1.Append($" SET obs='{clienteFavorecido}'");
            sqlComando1.Append($" WHERE controle={idMovimentoBancario}");

            _context.Database.Connection.Execute(sqlComando1.ToString());
        }

        public float CalculoRateio(float liquidacaoDespesas, float liquidacaoJuros, float liquidacaoDesconto,
            int empresaCodigo, int documentoTipoCodigo, string documentoSerie,long documentoNumero,
            int documentoSequencia, int codBancoBand, int numAgencia, long numConta, byte numContaDv, 
            float valor, DateTime? dataVencimento)
        {
            StringBuilder sqlComando = new StringBuilder();
            sqlComando.Append("SELECT pct_multa as pctMulta, pct_juros_dia as pctJurosDia FROM fin_car ");
            sqlComando.Append($"WHERE emp={empresaCodigo} AND tp_doc={documentoTipoCodigo} AND ser_doc='{documentoSerie}' AND num_doc={documentoNumero} ");
            sqlComando.Append($"AND seq={documentoSequencia} AND cod_banco_band={codBancoBand} AND num_agencia={numAgencia} AND num_conta={numConta} ");
            sqlComando.Append($"AND dv_conta={numContaDv} AND desdo=0");
            var result = _context.Database.Connection.Query<dynamic>(sqlComando.ToString()).Single();

            float moraRecebida = liquidacaoDespesas + liquidacaoJuros - liquidacaoDesconto;

            float porcentagemMultaJuros = (result.pctMulta / 100);

            var dataCalculoJuros = DateTime.Now.Subtract((DateTime)dataVencimento);
            float porcentagemJurosTotal = (result.pctJurosDia / 100) * dataCalculoJuros.Days;


            float multaCalculo = valor * porcentagemMultaJuros;
            float jurosCalculo = valor * porcentagemJurosTotal;
            float moraTotal = multaCalculo + jurosCalculo;

            float moraNaoLiquidada = moraTotal - moraRecebida;

            return moraNaoLiquidada;
        }

        public void GeraMoraNaoLiquidada(int empresaCodigo, int documentoTipoCodigo, string documentoSerie,int numeroDocumento, int? documentoSequencia, int desdobramento, float valorMoraNaoLiquidada, DateTime dataEmissao, long interveniente)
        {
            string dataEmissaoStr = dataEmissao.ToString("yyyy-MM-dd");
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

            var result = _context.Database.Connection.Query<dynamic>($"SELECT vlr_min_mora as valorMinimoMora, oper_mora_n_liq as operacaoMora from fin_parametro WHERE emp={empresaCodigo} and inicio_validade<=DATE_FORMAT(NOW(), '%Y-%m-%d') order by inicio_validade desc limit 1").Single();

            if (valorMoraNaoLiquidada >= result.valorMinimoMora)
            {
                StringBuilder sqlComando = new StringBuilder();
                sqlComando.Append("SELECT 1 FROM fin_Car WHERE");
                sqlComando.Append($" emp={empresaCodigo} AND ");
                sqlComando.Append($" tp_doc={documentoTipoCodigo} AND ");
                sqlComando.Append($" ser_doc='{documentoSerie}' AND ");
                sqlComando.Append($" num_doc={numeroDocumento} AND ");
                sqlComando.Append($" seq={documentoSequencia + 50 + 20} AND ");
                sqlComando.Append(" desdo=0");

                var exists = _context.Database.Connection.QueryFirstOrDefault<int?>(sqlComando.ToString());

                if (exists != null) throw new Exception();

                StringBuilder sqlComando1 = new StringBuilder();
                sqlComando1.Append("INSERT INTO fin_car SET ");
                sqlComando1.Append($"emp={empresaCodigo},");
                sqlComando1.Append($"tp_doc={documentoTipoCodigo},");
                sqlComando1.Append($"ser_doc='{documentoSerie}',");
                sqlComando1.Append($"num_doc={numeroDocumento},");
                sqlComando1.Append($"seq={documentoSequencia + 50},");
                sqlComando1.Append($"desdo=0,");
                sqlComando1.Append($"cli={interveniente},");
                sqlComando1.Append($"dt_emi=DATE_FORMAT({dataEmissaoStr}, '%Y-%m-%d'), dt_oper=DATE_FORMAT({dataEmissaoStr}, '%Y-%m-%d'), dt_vcto=DATE_FORMAT({dataEmissaoStr}, '%Y-%m-%d'), dt_util_vencto=DATE_FORMAT({dataEmissaoStr}, '%Y-%m-%d'),");
                sqlComando1.Append($"oper={result.operacaoMora},");
                sqlComando1.Append($"obs='Parcela de Mora Não Liquidada',");
                sqlComando1.Append($"vl_bruto=vlr_receber,");
                sqlComando1.Append($"vl=vlr_receber,");
                sqlComando1.Append($"sal=vlr_receber,");
                sqlComando1.Append($"vlr_receber=");
          
                _context.Database.Connection.Execute(sqlComando1.ToString() + string.Format(culture, "{0:0.00}", (decimal)valorMoraNaoLiquidada));
            }
        }

        public PagedList<TituloContasAReceber> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            var sqlComando = TituloContasAReceberSelectSql();

            sqlComando.Append($" WHERE atualizado_em>='{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");

            if (dataFim != null)
                sqlComando.Append($" AND atualizado_em<='{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" ORDER BY atualizado_em");

            var titulosContasAReceber = _context.Connection.QueryPagedList<TituloContasAReceber>(sqlComando.ToString(), page, limit);

            foreach (var titulo in (IEnumerable<TituloContasAReceber>)titulosContasAReceber.Records)
            {
                titulo.ChaveTitulosDaJuncao = ObterChaveDosTitulosDaJuncao(titulo);
                ObterCondicaoPagamento(titulo);
            }

            var titulosContasAReceberResultPagedList = new PagedList<TituloContasAReceber>
            {
                CurrentPage = titulosContasAReceber.CurrentPage,
                PageCount = titulosContasAReceber.PageCount,
                PageSize = titulosContasAReceber.PageSize,
                RecordCount = titulosContasAReceber.RecordCount,
                Records = (IEnumerable<TituloContasAReceber>)titulosContasAReceber.Records
            };

            return titulosContasAReceberResultPagedList;
        }

        public bool ExisteTituloRecebimentoEmCartao(long loteBaixa)
        {
            var tituloContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.LiquidacaoLoteBaixa == loteBaixa && 
                            t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberOperadora)
                .FirstOrDefault();

            return tituloContasAReceber != null;
        }

        public int QuantidadeTitulosDeCredito(long loteBaixa)
        {
            var titulosContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.LiquidacaoLoteBaixa == loteBaixa &&
                            t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente);

            return titulosContasAReceber.Count();
        }

        public bool ExisteTituloRecebimentoEmCheque(long loteBaixa)
        {
            var tituloContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.LiquidacaoLoteBaixa == loteBaixa && 
                            t.LiquidadoEmCheque == "S")
                .FirstOrDefault();

            return tituloContasAReceber != null;
        }

        public bool ExisteTituloDevolucaoEmCheque(int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            var tituloContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.DocumentoTipoCodigo == tipoDocumento &&
                            t.DocumentoSerie == serieDocumento &&
                            t.DocumentoNumero == numeroDocumento &&
                            t.DocumentoSequencia == sequencia.ToString() &&
                            t.BancoCodigoOficial == codBancoBand &&
                            t.BancoNumeroAgencia == numAgencia &&
                            t.BancoNumeroConta == numConta &&
                            t.BancoDvConta == numContaDv &&
                            t.Desdobramento > desdobramento &&
                            t.Devolucao == "S")
                .FirstOrDefault();

            return tituloContasAReceber != null;
        }

        public bool ExisteMovimentoDeBancoConciliado(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long idMovimentoBanco)
        {
            StringBuilder sqlComando = new StringBuilder();
            sqlComando.Append("SELECT mb.dt_conc");
            sqlComando.Append(" FROM fin_car AS c");
            sqlComando.Append(" LEFT JOIN fin_vinc_car_mov_bco AS v");
            sqlComando.Append(" ON v.emp = c.emp");
            sqlComando.Append(" AND v.tp_doc = c.tp_doc");
            sqlComando.Append(" AND v.ser_doc = c.ser_doc");
            sqlComando.Append(" AND v.num_doc = c.num_doc");
            sqlComando.Append(" AND v.seq = c.seq");
            sqlComando.Append(" AND v.cod_banco_band = c.cod_banco_band");
            sqlComando.Append(" AND v.num_agencia = c.num_agencia");
            sqlComando.Append(" AND v.num_conta = c.num_conta");
            sqlComando.Append(" AND v.dv_conta = c.dv_conta");
            sqlComando.Append(" AND v.desdo = c.desdo");
            sqlComando.Append(" LEFT JOIN fin_mov_banco AS mb");
            sqlComando.Append(" ON v.id_mov_bco = mb.controle");
            sqlComando.Append($" WHERE c.Emp = {empresa}");
            sqlComando.Append($" AND c.tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND c.ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND c.num_doc = {numeroDocumento}");
            sqlComando.Append($" AND c.seq = '{sequencia}'");
            sqlComando.Append($" AND c.cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND c.num_agencia = {numAgencia}");
            sqlComando.Append($" AND c.num_conta = {numConta}");
            sqlComando.Append($" AND c.dv_conta = {numContaDv}");
            sqlComando.Append($" AND c.desdo = {desdobramento}");
            if (idMovimentoBanco > 0) sqlComando.Append($" AND v.id_mov_bco = {idMovimentoBanco}");
            sqlComando.Append($" AND mb.dt_conc is not null");


            return _context.Database.Connection.QueryFirstOrDefault<DateTime>(sqlComando.ToString()) != null;
        }

        public bool ExisteChequeLiquidadoRelacionadoABaixaDoLote(long loteOrigem, int desdobramento)
        {
            var tituloContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.LoteOrigem == loteOrigem &&
                            t.Desdobramento == desdobramento &&
                            t.DocumentoTipoCodigo == (int)EDocumentoTipo.Cheque)
                .FirstOrDefault();

            return tituloContasAReceber != null;
        }

        public bool ExisteCreditoCompensadoNaGeracaoDaLiquidacao(long loteOrigem)
        {
            var tituloContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
               .Where(t => t.LoteOrigem == loteOrigem &&
                           t.LoteOrigem != t.LiquidacaoLoteBaixa &&
                           t.LiquidacaoLoteBaixa != 0)
               .FirstOrDefault();

            return tituloContasAReceber != null;
        }

        public void AjustaDescontoDeMora(long loteBaixa, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("UPDATE fin_car AS c ");
            sqlComando.Append(" INNER JOIN (");
            sqlComando.Append(" SELECT IFNULL(emp, 0) AS emp, IFNULL(tp_doc, 0) AS tp_doc, IFNULL(ser_doc, 0) AS ser_doc, ");
            sqlComando.Append(" IFNULL(num_doc, 0) AS num_doc, IFNULL(seq, 0) AS seq, IFNULL(desdo, 0) AS desdo, ");
            sqlComando.Append(" IFNULL(cod_banco_band, 0) AS cod_banco_band, IFNULL(num_agencia, 0) AS num_agencia, ");
            sqlComando.Append(" IFNULL(num_conta, 0) AS num_conta, IFNULL(dv_conta, 0) AS dv_conta, ");
            sqlComando.Append(" IFNULL(SUM(mora_des_acr_ef), 0) AS totalDescMora ");
            sqlComando.Append(" FROM fin_car ");
            sqlComando.Append($" WHERE liq_lot_bx = {loteBaixa} ");
            sqlComando.Append(" AND (seq < 50 OR LENGTH(seq) = 1) AND desdo <> 0 ");

            sqlComando.Append($" AND tp_doc = {tipoDocumento} ");
            sqlComando.Append($" AND cli = {clienteDocumento} ");
            sqlComando.Append($" AND num_doc = {numeroDocumento} ");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}' ");
            
            sqlComando.Append(" ) AS a ");
            sqlComando.Append(" ON c.emp = a.emp ");
            sqlComando.Append(" AND c.tp_doc = a.tp_doc ");
            sqlComando.Append(" AND c.ser_doc = a.ser_doc ");
            sqlComando.Append(" AND c.num_doc = a.num_doc ");
            sqlComando.Append(" AND c.seq = a.seq ");
            sqlComando.Append(" AND c.desdo = 0 ");
            sqlComando.Append(" AND c.cod_banco_band = a.cod_banco_band ");
            sqlComando.Append(" AND c.num_agencia = a.num_agencia ");
            sqlComando.Append(" AND c.num_conta = a.num_conta ");
            sqlComando.Append(" AND c.dv_conta = a.dv_conta ");

            sqlComando.Append("SET c.mora_des_acr_ef = a.totalDescMora");

            var comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public void RemovePendenciaDeCobranca(long loteBaixa)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM con_pendenc_cobranca");
            sqlComando.Append($" WHERE liq_lot_bx={loteBaixa}");
           
            var comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "con_pendenc_cobranca", comandoSql);
        }
 
        public void RemoveDesdobramentos(long loteBaixa, int sequencia, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento)
        {

            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_car ");
            sqlComando.Append($" WHERE liq_lot_bx = {loteBaixa} ");
            sqlComando.Append($" AND seq = {sequencia} ");
            sqlComando.Append($" AND desdo <> {Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL} ");
            sqlComando.Append($" AND tp_doc = {tipoDocumento} ");
            sqlComando.Append($" AND cli = {clienteDocumento} ");
            sqlComando.Append($" AND num_doc = {numeroDocumento} ");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}' ");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);


            sqlComando = new StringBuilder();
            comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_car");
            sqlComando.Append($" WHERE liq_lot_bx = {loteBaixa}");
            sqlComando.Append($" AND (seq>50 or length(seq)=2) AND desdo<>{Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento} ");
            sqlComando.Append($" AND cli = {clienteDocumento} ");
            sqlComando.Append($" AND num_doc = {numeroDocumento} ");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}' ");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public List<TituloContasAReceber> ObterTitulosDeCredito(int empresa, long loteBaixa, long clienteDocumento)
        {
            var titulosContasAReceber = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.LiquidacaoLoteBaixa == loteBaixa &&
                            t.IntervenienteCodigo == clienteDocumento &&
                            t.DocumentoTipoCodigo == (int)EDocumentoTipo.ContasAReceberCliente)
                .ToList();

            return titulosContasAReceber;
        }

        public (long idMovimento, float valor) ObterVinculosMovimentosDeBanco(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long idMovimentoBanco = 0)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append(" SELECT id_mov_bco, valor FROM fin_vinc_car_mov_bco");
            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND desdo = {desdobramento}");

            if (idMovimentoBanco > 0) sqlComando.Append($" AND id_mov_bco = {idMovimentoBanco}");

            var vinculo = _context.Database.Connection.QueryFirstOrDefault(sqlComando.ToString());

            var idMovimento = vinculo?.id_mov_bco ?? 0;
            var valor = vinculo?.valor ?? 0;

            return ((long)idMovimento, (float)valor);
        }

        public void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_vinc_car_mov_bco ");
            sqlComando.Append($" WHERE id_mov_bco = {idMovimentoBanco}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_vinc_car_mov_bco", comandoSql);
        }


        public List<TituloContasAReceber> ObterTituloRecebimentoEmCartao(long loteOrigem)
        {
            var sqlComando = TituloContasAReceberSelectSql();

            sqlComando.Append($" WHERE lote_origem={loteOrigem}");
            sqlComando.Append($" AND tp_doc={(int)EDocumentoTipo.ContasAReceberOperadora}");
            sqlComando.Append($" GROUP BY num_cartao, num_autorizacao");

            var tituloContasAReceber = _context.Connection.Query<TituloContasAReceber>(sqlComando.ToString());

            var titulos = (List<TituloContasAReceber>)tituloContasAReceber;

            ObterCondicaoPagamento(titulos);

            return titulos;
        }

        public void DesvinculaTituloCartaoDeCredito(string numeroCartao, string numeroAutorizacao, DateTime dataEmissao)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

           
            sqlComando.Append("UPDATE fin_car SET alocado=2");
            sqlComando.Append($" , lote_origem=0");
            sqlComando.Append($" , obs=''");
            sqlComando.Append($" WHERE num_cartao='{numeroCartao}'");
            sqlComando.Append($" AND num_autorizacao='{numeroAutorizacao}'");
            sqlComando.Append($" AND YEAR(dt_emi)='{dataEmissao.ToString("yyyy")}'");
            sqlComando.Append($" AND tp_doc={(int)EDocumentoTipo.ContasAReceberOperadora}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
                
        }

        public void RemoveTitulo(long loteOrigem)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_car");
            sqlComando.Append($" WHERE lote_origem = {loteOrigem}");;
            sqlComando.Append(" AND (seq<50 OR length(seq)=1)");
            sqlComando.Append(" AND alocado<>2");;

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public void RemoveTituloDeMora(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = sqlComando.ToString();

            sqlComando.Append("DELETE FROM fin_car");
            sqlComando.Append($" WHERE emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND seq = '{sequencia + 50}'");
            sqlComando.Append($" AND cli = {clienteDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public void RemoveTituloDeCredito(long loteOrigem, int empresa, long clienteDocumento)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = sqlComando.ToString();


            sqlComando.Append("DELETE FROM fin_car");
            sqlComando.Append($" WHERE tp_doc = {(int)EDocumentoTipo.ContasAReceberCliente}");
            sqlComando.Append($" AND emp = {empresa}");
            sqlComando.Append($" AND lote_origem = {loteOrigem}");
            sqlComando.Append($" AND cli = {clienteDocumento}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public DateTime? ObterDataMaximaDesdobramentos(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var data = _context.TitulosContasAReceber.AsNoTracking()
                    .Where(t => t.EmpresaCodigo == empresa
                                && t.DocumentoTipoCodigo == tipoDocumento
                                && t.DocumentoSerie == serieDocumento
                                && t.IntervenienteCodigo == clienteDocumento
                                && t.DocumentoNumero == numeroDocumento
                                && t.DocumentoSequencia == sequencia
                                && t.BancoCodigoOficial == codBancoBand
                                && t.BancoNumeroAgencia == numAgencia
                                && t.BancoNumeroConta == numConta
                                && t.BancoDvConta == numContaDv
                                && t.Desdobramento != 0 && t.DataLiquidacao > t.DataUtilVencimento
                                )
                    .GroupBy(t => t.DocumentoNumero)
                    .AsNoTracking()
                    .Select(g => g.Max(t => t.DataLiquidacao)).FirstOrDefault();
              
            return data;
        }

        public float? ObterSomatorioRecebimentos(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var valorRecebimentos = _context.TitulosContasAReceber.AsNoTracking()
                .Where(t => t.EmpresaCodigo == empresa
                            && t.DocumentoTipoCodigo == tipoDocumento
                            && t.DocumentoSerie == serieDocumento
                            && t.IntervenienteCodigo == clienteDocumento
                            && t.DocumentoNumero == numeroDocumento
                            && t.DocumentoSequencia == sequencia
                            && t.BancoCodigoOficial == codBancoBand
                            && t.BancoNumeroAgencia == numAgencia
                            && t.BancoNumeroConta == numConta
                            && t.BancoDvConta == numContaDv
                            && t.Desdobramento != Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL)
                .GroupBy(t => t.DocumentoNumero)
                .Select(g => g.Sum(t => (t.LiquidacaoValorRecebido - t.LiquidacaoJuros + t.LiquidacaoDesconto + t.LiquidacaoDespesas)))
                .FirstOrDefault();

            return valorRecebimentos;
        }

        public void AtualizarRecebimentosTituloPrincipal(float? valorRecebimentos, DateTime? dataMora, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("UPDATE fin_car SET");
            sqlComando.Append($" soma_recbtos = '{valorRecebimentos}',");
            sqlComando.Append($" vlr_mora_n_liq = 0,");

            if (!dataMora.HasValue)
            {
                sqlComando.Append(" dt_base_mora = dt_util_vencto,");
            }
            else
            {
                sqlComando.Append($" dt_base_mora = '{dataMora.Value.ToString("yyyy-MM-dd")}',");
            }

            sqlComando.Append(" liq_dt = NULL,");
            sqlComando.Append(" liq_dt_comi = NULL,");
            sqlComando.Append(" liq_oper = 0,");
            sqlComando.Append(" liq_doc = 0,");
            sqlComando.Append(" liq_juros = 0,");
            sqlComando.Append(" vl_des_acr_efet = CASE WHEN liq_desc > 0 THEN (liq_desc * -1) ELSE vl_des_acr_efet END,");
            sqlComando.Append(" liq_desc = 0,");
            sqlComando.Append(" liq_desp = 0,");
            sqlComando.Append(" liq_vl_rec = 0,");
            sqlComando.Append(" liq_bco = 0,");
            sqlComando.Append(" liq_cheque = '',");
            sqlComando.Append(" id_liq = '',");
            sqlComando.Append(" liq_lot_bx = 0,");
            sqlComando.Append(" liq_dt_cli = NULL");

            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND desdo = {Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);

            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public float ObterValorOcorrenciaDeDescontoMora(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append(" SELECT vlr_novo FROM fin_car_ocorrencias");
            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND id_aprovacao <> ''");
            sqlComando.Append($" AND cod_ocorrencia={(int)EOcorrencia.DescontoMora}");
            sqlComando.Append($" ORDER BY seq_ocorrencia DESC LIMIT 1");


            return _context.Database.Connection.QueryFirstOrDefault<float>(sqlComando.ToString());
        }

        public void AtualizarDescontoMora(float valor, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("UPDATE fin_car");
            sqlComando.Append($" SET mora_des_acr_ef='{valor}'");
            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND desdo={Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);

            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        public void RemoveVinculoMovimentosDeBanco(long idMovimentoBanco, int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("DELETE FROM fin_vinc_car_mov_bco ");
            sqlComando.Append($" WHERE id_mov_bco = {idMovimentoBanco}");
            sqlComando.Append($" AND Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND desdo = {desdobramento}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_vinc_car_mov_bco", comandoSql);
        }

        public void RemoveVinculoIdMovimentosDeBanco(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            var sqlComando = new StringBuilder();
            var comandoSql = string.Empty;

            sqlComando.Append("UPDATE fin_car SET id_mov_bco=0");
            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND tp_doc = {tipoDocumento}");
            sqlComando.Append($" AND ser_doc = '{serieDocumento}'");
            sqlComando.Append($" AND num_doc = {numeroDocumento}");
            sqlComando.Append($" AND cod_banco_band = {codBancoBand}");
            sqlComando.Append($" AND num_agencia = {numAgencia}");
            sqlComando.Append($" AND num_conta = {numConta}");
            sqlComando.Append($" AND dv_conta = {numContaDv}");
            sqlComando.Append($" AND desdo = {desdobramento}");

            comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_car", comandoSql);
        }

        private StringBuilder TituloContasAReceberSelectSql(bool mostraDadosFatura = false)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT car.emp {nameof(TituloContasAReceber.EmpresaCodigo)}");
            sql.Append($", car.tp_doc {nameof(TituloContasAReceber.DocumentoTipoCodigo)}");
            sql.Append($", car.ser_doc {nameof(TituloContasAReceber.DocumentoSerie)}");
            sql.Append($", car.num_doc {nameof(TituloContasAReceber.DocumentoNumero)}");
            sql.Append($", car.seq {nameof(TituloContasAReceber.DocumentoSequencia)}");
            sql.Append($", car.cod_banco_band {nameof(TituloContasAReceber.BancoCodigoOficial)}");
            sql.Append($", car.num_agencia {nameof(TituloContasAReceber.BancoNumeroAgencia)}");
            sql.Append($", car.num_conta {nameof(TituloContasAReceber.BancoNumeroConta)}");
            sql.Append($", car.dv_conta {nameof(TituloContasAReceber.BancoDvConta)}");
            sql.Append($", car.desdo {nameof(TituloContasAReceber.Desdobramento)}");
            sql.Append($", car.cli {nameof(TituloContasAReceber.IntervenienteCodigo)}");
            sql.Append($", car.dt_emi {nameof(TituloContasAReceber.DataEmissao)}");
            sql.Append($", car.dt_oper {nameof(TituloContasAReceber.DataOperacao)}");
            sql.Append($", car.oper {nameof(TituloContasAReceber.OperacaoFinanceiraCodigo)}");
            sql.Append($", car.dt_vcto {nameof(TituloContasAReceber.DataVencimento)}");
            sql.Append($", car.dt_vencto_orig {nameof(TituloContasAReceber.DataVencimentoOriginal)}");
            sql.Append($", car.vl {nameof(TituloContasAReceber.Valor)}");
            sql.Append($", car.vl {nameof(TituloContasAReceber.ValorToDouble)}");
            sql.Append($", car.soma_recbtos {nameof(TituloContasAReceber.SomaRecebimentos)}");
            sql.Append($", car.soma_recbtos {nameof(TituloContasAReceber.SomaRecebimentosToDouble)}");
            sql.Append($", car.sal {nameof(TituloContasAReceber.Saldo)}");
            sql.Append($", car.sal {nameof(TituloContasAReceber.SaldoToDouble)}");
            sql.Append($", car.cc {nameof(TituloContasAReceber.CentroCustoCodigo)}");
            sql.Append($", car.nosso_num {nameof(TituloContasAReceber.NossoNumero)}");
            sql.Append($", car.usina_contrato {nameof(TituloContasAReceber.ContratoUsinaCodigo)}");
            sql.Append($", car.ano_contrato {nameof(TituloContasAReceber.ContratoAno)}");
            sql.Append($", car.num_contrato {nameof(TituloContasAReceber.ContratoNumero)}");
            sql.Append($", car.num_cartao {nameof(TituloContasAReceber.CartaoNumero)}");
            sql.Append($", car.num_autorizacao {nameof(TituloContasAReceber.CartaoAutorizacao)}");
            sql.Append($", car.conc_cartao {nameof(TituloContasAReceber.CartaoConciliado)}");
            sql.Append($", car.alocado {nameof(TituloContasAReceber.Alocado)}");
            sql.Append($", car.id_mov_bco {nameof(TituloContasAReceber.IdMovimentoBanco)}");
            sql.Append($", car.sit {nameof(TituloContasAReceber.Situacao)}");
            sql.Append($", car.dt_sit {nameof(TituloContasAReceber.DataSituacao)}");
            sql.Append($", car.bco_port {nameof(TituloContasAReceber.BancoPortador)}");
            sql.Append($", car.obs {nameof(TituloContasAReceber.Observacao)}");
            sql.Append($", car.liq_dt {nameof(TituloContasAReceber.DataLiquidacao)}");
            sql.Append($", car.liq_oper {nameof(TituloContasAReceber.OperacaoLiquidacao)}");
            sql.Append($", car.liq_juros {nameof(TituloContasAReceber.LiquidacaoJuros)}");
            sql.Append($", car.liq_desc {nameof(TituloContasAReceber.LiquidacaoDesconto)}");
            sql.Append($", car.liq_desp {nameof(TituloContasAReceber.LiquidacaoDespesas)}");
            sql.Append($", car.liq_vl_rec {nameof(TituloContasAReceber.LiquidacaoValorRecebido)}");
            sql.Append($", car.liq_bco {nameof(TituloContasAReceber.BancoLiquidacao)}");
            sql.Append($", car.liq_lot_bx {nameof(TituloContasAReceber.LiquidacaoLoteBaixa)}");
            sql.Append($", car.liq_dt_cli {nameof(TituloContasAReceber.DataLiquidacaoCliente)}");
            sql.Append($", car.vlr_mora_n_liq {nameof(TituloContasAReceber.ValorMoraNaoLiquidado)}");
            sql.Append($", car.multa_mora_calc {nameof(TituloContasAReceber.MultaMoraCalculado)}");
            sql.Append($", car.mora_des_acr_ef {nameof(TituloContasAReceber.DescontoMora)}");
            sql.Append($", car.liq_doc {nameof(TituloContasAReceber.DocumentoLiquidacao)}");
            sql.Append($", car.at_bco {nameof(TituloContasAReceber.AtualizaBanco)}");
            sql.Append($", car.juros_mora_calc {nameof(TituloContasAReceber.JurosMoraCalculado)}");
            sql.Append($", car.id_liq {nameof(TituloContasAReceber.IdLiquidacao)}");
            sql.Append($", car.atualizado_em {nameof(TituloContasAReceber.DataAtualizacao)}");
            sql.Append($", car.linha_dig {nameof(TituloContasAReceber.LinhaDigitavelBoleto)}");
            sql.Append($", car.meio_pagamento {nameof(TituloContasAReceber.MeioPagamento)}");
            sql.Append($", car.vl_cbs {nameof(TituloContasAReceber.ValorCbs)}");
            sql.Append($", car.vl_ibs {nameof(TituloContasAReceber.ValorIbs)}");
            sql.Append($", car.vl_is {nameof(TituloContasAReceber.ValorIs)}");
            sql.Append($", car.totaliza_fin_reform_trib {nameof(TituloContasAReceber.TotalizaCbsIbsIs)}");
            sql.Append($", car.barra {nameof(TituloContasAReceber.CodigoBarrasBoleto)}");
            sql.Append($", car.noss_num_interm {nameof(TituloContasAReceber.NossoNumeroIntermediarioBoleto)}");
            

            if (mostraDadosFatura) sql.Append($", IFNULL(nf.num_nf_serv_seq, 0) {nameof(TituloContasAReceber.NumeroNFSE)}");
                    
            sql.Append($" FROM fin_car car");

            if (mostraDadosFatura)
            {
                sql.Append($" LEFT JOIN topsys.fis_nf_servico nf");
                sql.Append($" ON (nf.filial DIV 1000)=car.emp");
                sql.Append($" AND nf.cli=car.cli");
                sql.Append($" AND nf.tp_doc=car.tp_doc");
                sql.Append($" AND nf.num_nf=car.num_doc");
                sql.Append($" AND nf.ser=car.ser_doc");
            }

            return sql;
        }
    }
}

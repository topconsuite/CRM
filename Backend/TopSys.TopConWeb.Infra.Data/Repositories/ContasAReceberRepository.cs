using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Infra.Data.Helpers;
using Topsys.TopConWeb.SharedKernel.Services;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ContasAReceberRepository : IContasAReceberRepository
    {
        protected AppDataContext _context;
        private readonly IdentityHelperService _identityHelperService;

        public ContasAReceberRepository(AppDataContext context, IdentityHelperService identityHelperService)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public IEnumerable<ContasAReceber> ListarContasAReceberPeloNumeroCartaoAutorizacaoEDataTransacao(string cartaoNumero, string autorizacaoNumero, DateTime dataTransacao)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"SELECT emp {nameof(ContasAReceber.EmpresaCod)}   ");
            sqlCommand.Append($", tp_doc {nameof(ContasAReceber.DocumentoTipo)}  ");
            sqlCommand.Append($", ser_doc {nameof(ContasAReceber.DocumentoSerie)}  ");
            sqlCommand.Append($", num_doc {nameof(ContasAReceber.DocumentoNumero)} ");
            sqlCommand.Append($", seq {nameof(ContasAReceber.Sequencia)}  ");
            sqlCommand.Append($", desdo {nameof(ContasAReceber.Desdobramento)}  ");
            sqlCommand.Append($", cli {nameof(ContasAReceber.IntervenienteCodigo)}  ");
            sqlCommand.Append($", dt_emi {nameof(ContasAReceber.DataEmissao)} ");
            sqlCommand.Append($", dt_vcto {nameof(ContasAReceber.DataVencimento)} ");
            sqlCommand.Append($", vl {nameof(ContasAReceber.Valor)}  ");
            sqlCommand.Append($", oper {nameof(ContasAReceber.Operacao)}   ");
            sqlCommand.Append($", cc  {nameof(ContasAReceber.CentroCustoCodigo)}  ");
            sqlCommand.Append($", sit  {nameof(ContasAReceber.Situacao)}   ");
            sqlCommand.Append($", bco_port {nameof(ContasAReceber.PortadorCodigo)}  ");
            sqlCommand.Append($", id_aprovacao {nameof(ContasAReceber.IdAprovacao)}  ");
            sqlCommand.Append($", id_cadast {nameof(ContasAReceber.IdCadastro)}  ");
            sqlCommand.Append($", cod_banco_band {nameof(ContasAReceber.BandeiraCodigo)}  ");
            sqlCommand.Append($", num_cartao {nameof(ContasAReceber.CartaoNumero)}  ");
            sqlCommand.Append($", obs {nameof(ContasAReceber.Observacao)} ");
            sqlCommand.Append($", num_autorizacao {nameof(ContasAReceber.AutorizacaoNumero)}   ");
            sqlCommand.Append($", usina_contrato {nameof(ContasAReceber.ContratoUsinaCodigo)} ");
            sqlCommand.Append($", num_contrato {nameof(ContasAReceber.ContratoNumero)}  ");
            sqlCommand.Append($", ano_contrato {nameof(ContasAReceber.ContratoAno)} ");
            sqlCommand.Append($", num_conta {nameof(ContasAReceber.ContaNumero)}  ");
            sqlCommand.Append($", num_agencia {nameof(ContasAReceber.AgenciaNumero)}  ");
            sqlCommand.Append($", alocado {nameof(ContasAReceber.Alocado)}  ");
            sqlCommand.Append($" FROM topsys.fin_car ");
            sqlCommand.Append($" WHERE num_cartao = @CARTAONUMERO ");
            sqlCommand.Append($" AND num_autorizacao = @NUMEROAUTORIZACAO");
            sqlCommand.Append($" AND DATE(dt_emi) = @DATATRANSACAO");

            return _context.Database.Connection.Query<ContasAReceber>(sqlCommand.ToString(), 
                new {
                    CARTAONUMERO = cartaoNumero,
                    NUMEROAUTORIZACAO = autorizacaoNumero,
                    DATATRANSACAO = dataTransacao.Date
                }) ;

        }

        public IEnumerable<ContasAReceber> ListarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(string cartaoNumero, string autorizacaoNumero, int anoTransacao)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"SELECT emp {nameof(ContasAReceber.EmpresaCod)}   ");
            sqlCommand.Append($", tp_doc {nameof(ContasAReceber.DocumentoTipo)}  ");
            sqlCommand.Append($", ser_doc {nameof(ContasAReceber.DocumentoSerie)}  ");
            sqlCommand.Append($", num_doc {nameof(ContasAReceber.DocumentoNumero)} ");
            sqlCommand.Append($", seq {nameof(ContasAReceber.Sequencia)}  ");
            sqlCommand.Append($", cli {nameof(ContasAReceber.IntervenienteCodigo)}  ");
            sqlCommand.Append($", dt_emi {nameof(ContasAReceber.DataEmissao)} ");
            sqlCommand.Append($", dt_vcto {nameof(ContasAReceber.DataVencimento)} ");
            sqlCommand.Append($", vl {nameof(ContasAReceber.Valor)}  ");
            sqlCommand.Append($", oper {nameof(ContasAReceber.Operacao)}   ");
            sqlCommand.Append($", cc  {nameof(ContasAReceber.CentroCustoCodigo)}  ");
            sqlCommand.Append($", sit  {nameof(ContasAReceber.Situacao)}   ");
            sqlCommand.Append($", bco_port {nameof(ContasAReceber.PortadorCodigo)}  ");
            sqlCommand.Append($", id_aprovacao {nameof(ContasAReceber.IdAprovacao)}  ");
            sqlCommand.Append($", id_cadast {nameof(ContasAReceber.IdCadastro)}  ");
            sqlCommand.Append($", cod_banco_band {nameof(ContasAReceber.BandeiraCodigo)}  ");
            sqlCommand.Append($", num_cartao {nameof(ContasAReceber.CartaoNumero)}  ");
            sqlCommand.Append($", obs {nameof(ContasAReceber.Observacao)} ");
            sqlCommand.Append($", num_autorizacao {nameof(ContasAReceber.AutorizacaoNumero)}   ");
            sqlCommand.Append($", usina_contrato {nameof(ContasAReceber.ContratoUsinaCodigo)} ");
            sqlCommand.Append($", num_contrato {nameof(ContasAReceber.ContratoNumero)}  ");
            sqlCommand.Append($", ano_contrato {nameof(ContasAReceber.ContratoAno)} ");
            sqlCommand.Append($", num_conta {nameof(ContasAReceber.ContaNumero)}  ");
            sqlCommand.Append($", num_agencia {nameof(ContasAReceber.AgenciaNumero)}  ");
            sqlCommand.Append($", alocado {nameof(ContasAReceber.Alocado)}  ");
            sqlCommand.Append($", id_mov_bco {nameof(ContasAReceber.IdMovimentoBanco)}  ");
            sqlCommand.Append($" FROM topsys.fin_car ");
            sqlCommand.Append($" WHERE num_cartao = @CARTAONUMERO ");
            sqlCommand.Append($" AND num_autorizacao = @NUMEROAUTORIZACAO");
            sqlCommand.Append($" AND year(dt_emi) = @ANOTRANSACAO");

            return _context.Database.Connection.Query<ContasAReceber>(sqlCommand.ToString(),
                new
                {
                    CARTAONUMERO = cartaoNumero,
                    NUMEROAUTORIZACAO = autorizacaoNumero,
                    ANOTRANSACAO = anoTransacao
                });
        }

        public void InsereContasAReceber(ContasAReceber contasAReceber)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"INSERT INTO topsys.fin_car SET emp= @{nameof(contasAReceber.EmpresaCod)}   ");
            sqlCommand.Append($", tp_doc= @{nameof(contasAReceber.DocumentoTipo)}  ");
            sqlCommand.Append($", ser_doc= @{nameof(contasAReceber.DocumentoSerie)}  ");
            sqlCommand.Append($", num_doc= @{nameof(contasAReceber.DocumentoNumero)} ");
            sqlCommand.Append($", seq= @{nameof(contasAReceber.Sequencia)}  ");
            sqlCommand.Append(", desdo= 0  ");
            sqlCommand.Append($", cli= @{nameof(contasAReceber.IntervenienteCodigo)}  ");
            sqlCommand.Append($", dt_emi= @{nameof(contasAReceber.DataEmissao)} ");
            sqlCommand.Append($", dt_oper= @{nameof(contasAReceber.DataEmissao)} ");
            sqlCommand.Append($", dt_vcto= @{nameof(contasAReceber.DataVencimento)} ");
            sqlCommand.Append($", dt_vencto_orig= @{nameof(contasAReceber.DataVencimento)}   ");
            sqlCommand.Append($", vl= @{nameof(contasAReceber.Valor)}  ");
            sqlCommand.Append(", soma_recbtos=0  ");
            sqlCommand.Append(", sal=vl  ");
            sqlCommand.Append($", oper= @{nameof(contasAReceber.Operacao)}   ");
            sqlCommand.Append($", cc=  @{nameof(contasAReceber.CentroCustoCodigo)}  ");
            sqlCommand.Append(", fluxo_finan=0  ");
            sqlCommand.Append($", sit=  @{nameof(contasAReceber.Situacao)}   ");
            sqlCommand.Append($", dt_sit= @{nameof(contasAReceber.DataEmissao)}  ");
            sqlCommand.Append($", bco_port= @{nameof(contasAReceber.PortadorCodigo)}  ");
            sqlCommand.Append(", vl_bruto=vl  ");
            sqlCommand.Append(", vlr_receber=vl  ");
            sqlCommand.Append($", id_cadast= @{nameof(contasAReceber.IdCadastro)}  ");
            sqlCommand.Append($", id_aprovacao= @{nameof(contasAReceber.IdAprovacao)}  ");
            sqlCommand.Append($", cod_banco_band= @{nameof(contasAReceber.BandeiraCodigo)}  ");
            sqlCommand.Append($", num_cartao= @{nameof(contasAReceber.CartaoNumero)}  ");
            sqlCommand.Append($", obs= @{nameof(contasAReceber.Observacao)} ");
            sqlCommand.Append($", num_autorizacao=@{nameof(contasAReceber.AutorizacaoNumero)}   ");
            sqlCommand.Append($", usina_contrato= @{nameof(contasAReceber.ContratoUsinaCodigo)} ");
            sqlCommand.Append($", num_contrato= @{nameof(contasAReceber.ContratoNumero)}  ");
            sqlCommand.Append($", ano_contrato= @{nameof(contasAReceber.ContratoAno)} ");
            sqlCommand.Append($", num_conta= @{nameof(contasAReceber.ContaNumero)}  ");
            sqlCommand.Append($", dv_conta= @{nameof(contasAReceber.ContaDigito)}  ");
            sqlCommand.Append($", num_agencia= @{nameof(contasAReceber.AgenciaNumero)}  ");
            sqlCommand.Append($", alocado= @{nameof(contasAReceber.Alocado)} ");

            _context.Database.Connection.Execute(sqlCommand.ToString(), contasAReceber);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sqlCommand.ToString(), contasAReceber);
        }

        public ContratoPagamentoDetalheCartao ObterContratoPagamentoDetalheCartao(CartaoTransacao cartaoTransacao)
        {
            return _context.ContratoPagamentoDetalhesCartao
                .Include(t => t.Bandeira)
                .Include(t => t.Bandeira.Portador.Conta)
                //.Include(t => t.Bandeira.Interveniente)
                .Where(t => t.NumeroAutorizacao == cartaoTransacao.AutorizacaoNumero &&
                    t.NumeroCartao == cartaoTransacao.CartaoNumeroAsInteger &&
                    t.DataTransacao.Year == cartaoTransacao.TransacaoDataHora.Year)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void AtualizarAlocadoContasAReceber(ContasAReceber contasAReceber)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.fin_car ");
            sql.Append($"SET alocado = @{nameof(contasAReceber.Alocado)} ");
            sql.Append("WHERE ");
            sql.Append($"emp=@{nameof(contasAReceber.EmpresaCod)} ");
            sql.Append($"AND tp_doc=@{nameof(contasAReceber.DocumentoTipo)} ");
            sql.Append($"AND ser_doc=@{nameof(contasAReceber.DocumentoSerie)} ");
            sql.Append($"AND num_doc=@{nameof(contasAReceber.DocumentoNumero)} ");
            sql.Append($"AND seq=@{nameof(contasAReceber.Sequencia)} ");
            sql.Append($"AND cod_banco_band=@{nameof(contasAReceber.BandeiraCodigo)} ");
            sql.Append($"AND num_agencia=@{nameof(contasAReceber.AgenciaNumero)} ");
            sql.Append($"AND num_conta=@{nameof(contasAReceber.ContaNumero)} ");
            sql.Append($"AND dv_conta=@{nameof(contasAReceber.ContaDigito)} ");

            _context.Database.Connection.Execute(sql.ToString(), contasAReceber);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), contasAReceber);
        }

        public void AtualizarAlocadoContasAReceberPorCartaoEAutorizacao(string numCartao, string autorizacao, int anoDataEmissao, EContasAReceberStatusAlocado eContasAReceberStatusAlocado)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.fin_car ");
            sql.Append($"SET alocado = @{nameof(eContasAReceberStatusAlocado)} ");
            sql.Append("WHERE ");
            sql.Append($"num_cartao=@{nameof(numCartao)} ");
            sql.Append($"AND num_autorizacao=@{nameof(autorizacao)} ");
            sql.Append($"AND YEAR(dt_emi)=@{nameof(anoDataEmissao)} ");
            sql.Append($"AND tp_doc=88");

            var filtro = new
            {
                numCartao,
                autorizacao,
                anoDataEmissao,
                eContasAReceberStatusAlocado = (int)eContasAReceberStatusAlocado
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }

        public void AtualizarIdMovimentoBanco(ContasAReceber contasAReceber)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.fin_car ");
            sql.Append($"SET id_mov_bco = @{nameof(contasAReceber.IdMovimentoBanco)} ");
            sql.Append("WHERE ");
            sql.Append($"emp=@{nameof(contasAReceber.EmpresaCod)} ");
            sql.Append($"AND tp_doc=@{nameof(contasAReceber.DocumentoTipo)} ");
            sql.Append($"AND ser_doc=@{nameof(contasAReceber.DocumentoSerie)} ");
            sql.Append($"AND num_doc=@{nameof(contasAReceber.DocumentoNumero)} ");
            sql.Append($"AND seq=@{nameof(contasAReceber.Sequencia)} ");
            sql.Append($"AND desdo=@{nameof(contasAReceber.Desdobramento)} ");
            sql.Append($"AND cod_banco_band=@{nameof(contasAReceber.BandeiraCodigo)} ");
            sql.Append($"AND num_agencia=@{nameof(contasAReceber.AgenciaNumero)} ");
            sql.Append($"AND num_conta=@{nameof(contasAReceber.ContaNumero)} ");
            sql.Append($"AND dv_conta=@{nameof(contasAReceber.ContaDigito)} ");

            _context.Database.Connection.Execute(sql.ToString(), contasAReceber);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), contasAReceber);
        }

        public void VincularContasAReceberComMovimentoBanco(ContasAReceber contasAReceber, MovimentoBanco movimentoBanco, float valorVinculo)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"INSERT INTO topsys.fin_vinc_car_mov_bco ");
            sqlCommand.Append($" SET id_mov_bco= @{nameof(MovimentoBanco.Id)} ");
            sqlCommand.Append($", emp= @{nameof(ContasAReceber.EmpresaCod)} ");
            sqlCommand.Append($", tp_doc= @{nameof(ContasAReceber.DocumentoTipo)}  ");
            sqlCommand.Append($", ser_doc= @{nameof(ContasAReceber.DocumentoSerie)}  ");
            sqlCommand.Append($", num_doc= @{nameof(ContasAReceber.DocumentoNumero)} ");
            sqlCommand.Append($", seq= @{nameof(ContasAReceber.Sequencia)}  ");
            sqlCommand.Append($", desdo= @{nameof(ContasAReceber.Desdobramento)}  ");
            sqlCommand.Append($", cod_banco_band= @{nameof(ContasAReceber.BandeiraCodigo)}  ");
            sqlCommand.Append($", num_agencia= @{nameof(ContasAReceber.AgenciaNumero)}  ");
            sqlCommand.Append($", num_conta= @{nameof(ContasAReceber.ContaNumero)}  ");
            sqlCommand.Append($", dv_conta= @{nameof(ContasAReceber.ContaDigito)}  ");
            sqlCommand.Append($", valor= @{nameof(valorVinculo)}  ");

            var filtro = new {
                movimentoBanco.Id,
                contasAReceber.EmpresaCod,
                contasAReceber.DocumentoTipo,
                contasAReceber.DocumentoSerie,
                contasAReceber.DocumentoNumero,
                contasAReceber.Sequencia,
                contasAReceber.Desdobramento,
                contasAReceber.BandeiraCodigo,
                contasAReceber.AgenciaNumero,
                contasAReceber.ContaNumero,
                contasAReceber.ContaDigito,
                valorVinculo
            };

            _context.Database.Connection.Execute(sqlCommand.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_vinc_car_mov_bco", sqlCommand.ToString(), filtro);
        }

        public IEnumerable<ContasAReceber> ListarContasAReceberDeCartaoVinculado(ContratoPagamentoDetalheCartao contratoPagamentoDetalheCartao, int empresaCodigo)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($"SELECT emp {nameof(ContasAReceber.EmpresaCod)}   ");
            sqlCommand.Append($", tp_doc {nameof(ContasAReceber.DocumentoTipo)}  ");
            sqlCommand.Append($", ser_doc {nameof(ContasAReceber.DocumentoSerie)}  ");
            sqlCommand.Append($", num_doc {nameof(ContasAReceber.DocumentoNumero)} ");
            sqlCommand.Append($", seq {nameof(ContasAReceber.Sequencia)}  ");
            sqlCommand.Append($", cli {nameof(ContasAReceber.IntervenienteCodigo)}  ");
            sqlCommand.Append($", dt_emi {nameof(ContasAReceber.DataEmissao)} ");
            sqlCommand.Append($", dt_vcto {nameof(ContasAReceber.DataVencimento)} ");
            sqlCommand.Append($", vl {nameof(ContasAReceber.Valor)}  ");
            sqlCommand.Append($", oper {nameof(ContasAReceber.Operacao)}   ");
            sqlCommand.Append($", cc  {nameof(ContasAReceber.CentroCustoCodigo)}  ");
            sqlCommand.Append($", sit  {nameof(ContasAReceber.Situacao)}   ");
            sqlCommand.Append($", bco_port {nameof(ContasAReceber.PortadorCodigo)}  ");
            sqlCommand.Append($", id_aprovacao {nameof(ContasAReceber.IdAprovacao)}  ");
            sqlCommand.Append($", id_cadast {nameof(ContasAReceber.IdCadastro)}  ");
            sqlCommand.Append($", cod_banco_band {nameof(ContasAReceber.BandeiraCodigo)}  ");
            sqlCommand.Append($", num_cartao {nameof(ContasAReceber.CartaoNumero)}  ");
            sqlCommand.Append($", obs {nameof(ContasAReceber.Observacao)} ");
            sqlCommand.Append($", num_autorizacao {nameof(ContasAReceber.AutorizacaoNumero)}   ");
            sqlCommand.Append($", usina_contrato {nameof(ContasAReceber.ContratoUsinaCodigo)} ");
            sqlCommand.Append($", num_contrato {nameof(ContasAReceber.ContratoNumero)}  ");
            sqlCommand.Append($", ano_contrato {nameof(ContasAReceber.ContratoAno)} ");
            sqlCommand.Append($", num_conta {nameof(ContasAReceber.ContaNumero)}  ");
            sqlCommand.Append($", num_agencia {nameof(ContasAReceber.AgenciaNumero)}  ");
            sqlCommand.Append($", alocado {nameof(ContasAReceber.Alocado)}  ");
            sqlCommand.Append($", id_mov_bco {nameof(ContasAReceber.IdMovimentoBanco)}  ");
            sqlCommand.Append($", lote_conc {nameof(ContasAReceber.LoteConciliado)}  ");
            sqlCommand.Append($", soma_recbtos {nameof(ContasAReceber.SomaRecebimentos)}  ");
            sqlCommand.Append($" FROM topsys.fin_car ");
            sqlCommand.Append($" WHERE emp = @EMPRESA ");
            sqlCommand.Append($" AND tp_doc = @DOCUMENTOTIPO ");
            sqlCommand.Append($" AND num_doc = @NUMERODOCUMENTO");
            sqlCommand.Append($" AND cod_banco_band = @BANDEIRACOD");
            sqlCommand.Append($" AND num_cartao = @NUMEROCARTAO");
            if (contratoPagamentoDetalheCartao.NumeroAutorizacao.IsNumeric())
                sqlCommand.Append($"  AND CAST(num_autorizacao AS UNSIGNED)= {contratoPagamentoDetalheCartao.NumeroAutorizacao}");
            else
                sqlCommand.Append($" AND num_autorizacao = '{contratoPagamentoDetalheCartao.NumeroAutorizacao.Trim()}'");


            return _context.Database.Connection.Query<ContasAReceber>(sqlCommand.ToString(),
                new
                {
                    EMPRESA= empresaCodigo,
                    DOCUMENTOTIPO = EDocumentoTipo.ContasAReceberOperadora,
                    NUMERODOCUMENTO = long.Parse(contratoPagamentoDetalheCartao.DataTransacao.ToString("ddMMyy")),
                    BANDEIRACOD = contratoPagamentoDetalheCartao.BandeiraCodigo,
                    NUMEROCARTAO = contratoPagamentoDetalheCartao.NumeroCartao.ToString()
                });
        }

        public IEnumerable<TituloContasAReceber> ListarContasAReceberCartaoASeremDesaprovados(int empresaCod, int usina, string sequencia, long numeroDocumento)
        {
            var sqlCommand = new StringBuilder();

            sqlCommand.Append($" SELECT f.emp {nameof(TituloContasAReceber.EmpresaCodigo)}");
            sqlCommand.Append($" ,f.tp_doc {nameof(TituloContasAReceber.DocumentoTipoCodigo)}");
            sqlCommand.Append($" ,f.ser_doc {nameof(TituloContasAReceber.DocumentoSerie)}");
            sqlCommand.Append($" ,f.num_doc {nameof(TituloContasAReceber.DocumentoNumero)}");
            sqlCommand.Append($" ,f.seq {nameof(TituloContasAReceber.DocumentoSequencia)}");
            sqlCommand.Append($" ,f.cod_banco_band {nameof(TituloContasAReceber.BancoCodigoOficial)}");
            sqlCommand.Append($" ,f.num_agencia {nameof(TituloContasAReceber.BancoNumeroAgencia)}");
            sqlCommand.Append($" ,f.num_conta {nameof(TituloContasAReceber.BancoNumeroConta)}");
            sqlCommand.Append($" ,CAST(f.dv_conta AS UNSIGNED) {nameof(TituloContasAReceber.BancoDvConta)}");
            sqlCommand.Append($" ,f.desdo {nameof(TituloContasAReceber.Desdobramento)}");
            sqlCommand.Append($" ,f.dt_emi {nameof(TituloContasAReceber.DataEmissao)}");
            sqlCommand.Append($" ,f.soma_recbtos {nameof(TituloContasAReceber.SomaRecebimentos)}");
            sqlCommand.Append($" ,f.conc_cartao {nameof(TituloContasAReceber.CartaoConciliado)}");
            sqlCommand.Append($" FROM");
            sqlCommand.Append($" fin_car AS f");
            sqlCommand.Append($" INNER JOIN fin_car f2 ON f2.cod_banco_band = f.cod_banco_band");
            sqlCommand.Append($" AND f2.num_cartao = f.num_cartao");
            sqlCommand.Append($" AND f2.num_autorizacao = f.num_autorizacao");
            sqlCommand.Append($" WHERE");
            sqlCommand.Append($" f2.emp = @EMPRESACOD  AND f2.tp_doc = @DOCUMENTOTIPO");
            sqlCommand.Append($" AND f2.ser_doc = @DOCUMENTOSERIE");
            sqlCommand.Append($" AND f2.num_doc = @DOCUMENTONUMERO");
            sqlCommand.Append($" AND f2.seq in({sequencia})");
            sqlCommand.Append($" AND f2.desdo = 0;");


            return _context.Database.Connection.Query<TituloContasAReceber>(sqlCommand.ToString(),
                new
                {
                    EMPRESACOD = empresaCod,
                    DOCUMENTOTIPO = (int)EDocumentoTipo.ContasAReceberCliente,
                    DOCUMENTOSERIE = usina,
                    DOCUMENTONUMERO = numeroDocumento
                });
        }

        public void DeletarContasAReceberDeCartaoDoCliente(int empresa, int usina,
            long documentoNumero, string sequencia)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE f FROM topsys.fin_car as f");
            sql.Append($", (SELECT cod_banco_band, num_cartao, num_autorizacao");
            sql.Append($" FROM fin_car");
            sql.Append($" WHERE emp=@{nameof(empresa)}");
            sql.Append($" AND tp_doc=@DOCUMENTOTIPO");
            sql.Append($" AND ser_doc=@{nameof(usina)}");
            sql.Append($" AND num_doc=@{nameof(documentoNumero)}");
            if (sequencia != "")
                sql.Append($" AND seq in({sequencia}))");
            sql.Append($" AS f2");
            sql.Append($" WHERE f2.cod_banco_band=f.cod_banco_band");
            sql.Append($" AND f2.num_cartao=f.num_cartao ");
            sql.Append($" AND f2.num_autorizacao=f.num_autorizacao ");
            sql.Append($" AND f.tp_doc=@DOCUMENTOTIPO");


            var filtro = new
            {
                empresa,
                DOCUMENTOTIPO = (int)EDocumentoTipo.ContasAReceberCliente,
                usina,
                documentoNumero,
                sequencia
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }

        public void DeletarContasAReceberDoCliente(int empresa, int usina,
           long documentoNumero, string sequencia)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.fin_car");
            sql.Append($" WHERE emp=@{nameof(empresa)}");
            sql.Append($" AND tp_doc=@DOCUMENTOTIPO");
            sql.Append($" AND ser_doc=@{nameof(usina)}");
            sql.Append($" AND num_doc=@{nameof(documentoNumero)}");
            if (sequencia != "")
                sql.Append($" AND seq in ({sequencia})");


            var filtro = new
            {
                empresa,
                DOCUMENTOTIPO = (int)EDocumentoTipo.ContasAReceberCliente,
                usina,
                documentoNumero
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }

        public void DeletarContasAReceberPeloNumeroCartaoAutorizacaoEAnoTransacao(string numeroCartao, string numeroAutorizacao, int anoTransacao, EDocumentoTipo documentoTipo)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.fin_car");
            sql.Append($" WHERE num_cartao=@{nameof(numeroCartao)}");
            sql.Append($" AND num_autorizacao=@{nameof(numeroAutorizacao)}");
            sql.Append($" AND YEAR(dt_emi)=@{nameof(anoTransacao)}");
            sql.Append($" AND tp_doc=@{nameof(documentoTipo)}");


            var filtro = new
            {
                numeroCartao,
                numeroAutorizacao,
                anoTransacao,
                @documentoTipo = (int)documentoTipo
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);

            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }

        public void DeletarContasAReceberTipoCheque(ContratoPagamentoDetalheCheque detalheCheque)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.fin_car");
            sql.Append($" WHERE tp_doc=@DOCUMENTOTIPO");
            sql.Append($" AND usina_contrato=@{nameof(detalheCheque.UsinaCodigo)}");
            sql.Append($" AND num_contrato=@{nameof(detalheCheque.ContratoNumero)}");
            sql.Append($" AND ano_contrato=@{nameof(detalheCheque.ContratoAno)}");
            sql.Append($" AND num_doc= @{nameof(detalheCheque.NumeroCheque)} ");
            sql.Append($" AND cod_banco_band= @{nameof(detalheCheque.BancoCodigoOficial)}  ");
            sql.Append($" AND num_agencia= @{nameof(detalheCheque.BancoAgencia)}  ");
            sql.Append($" AND num_conta= @{nameof(detalheCheque.BancoContaNumero)}  ");
            sql.Append($" AND dv_conta= @{nameof(detalheCheque.BancoContaDV)}  ");


            var filtro = new
            {
                @documentoTipo = (int)EDocumentoTipo.Cheque,
                detalheCheque.UsinaCodigo,
                detalheCheque.ContratoNumero,
                detalheCheque.ContratoAno,
                detalheCheque.NumeroCheque,
                detalheCheque.BancoCodigoOficial,
                detalheCheque.BancoAgencia,
                detalheCheque.BancoContaNumero,
                detalheCheque.BancoContaDV
            };


            _context.Database.Connection.Execute(sql.ToString(), filtro);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }

        public void DeletarContasAReceberTipoCheque(ContratoPagamentoDetalheChequeVersao detalheCheque)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.fin_car");
            sql.Append($" WHERE tp_doc=@DOCUMENTOTIPO");
            sql.Append($" AND usina_contrato=@{nameof(detalheCheque.UsinaCodigo)}");
            sql.Append($" AND num_contrato=@{nameof(detalheCheque.ContratoNumero)}");
            sql.Append($" AND ano_contrato=@{nameof(detalheCheque.ContratoAno)}");
            sql.Append($" AND num_doc= @{nameof(detalheCheque.NumeroCheque)} ");
            sql.Append($" AND cod_banco_band= @{nameof(detalheCheque.BancoCodigoOficial)}  ");
            sql.Append($" AND num_agencia= @{nameof(detalheCheque.BancoAgencia)}  ");
            sql.Append($" AND num_conta= @{nameof(detalheCheque.BancoContaNumero)}  ");
            sql.Append($" AND dv_conta= @{nameof(detalheCheque.BancoContaDV)}  ");


            var filtro = new
            {
                @documentoTipo = (int)EDocumentoTipo.Cheque,
                detalheCheque.UsinaCodigo,
                detalheCheque.ContratoNumero,
                detalheCheque.ContratoAno,
                detalheCheque.NumeroCheque,
                detalheCheque.BancoCodigoOficial,
                detalheCheque.BancoAgencia,
                detalheCheque.BancoContaNumero,
                detalheCheque.BancoContaDV
            };


            _context.Database.Connection.Execute(sql.ToString(), filtro);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "fin_car", sql.ToString(), filtro);
        }
    }
}

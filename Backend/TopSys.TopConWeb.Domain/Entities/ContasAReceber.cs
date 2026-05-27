using System;
using System.Linq;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContasAReceber
    {
        public ContasAReceber()  : base() { }
        public ContasAReceber(Obra obra, CartaoBandeira cartaoBandeira, CartaoTransacao cartaoTransacao, ContratoPagamentoDetalheCartao contratoPagamentoDetalheCartao,int operacaoCartao, int numeroParcela, string usuario = "AUTO")
        {
            EmpresaCod = cartaoBandeira.EmpresaCod;
            DocumentoTipo = (int)EDocumentoTipo.ContasAReceberOperadora;
            DocumentoSerie = cartaoTransacao.AutorizacaoNumero.Substring(cartaoTransacao.AutorizacaoNumero.Length - 3);
            DocumentoNumero = long.Parse(cartaoTransacao.TransacaoDataHora.ToString("ddMMyy"));
            Sequencia = numeroParcela.ToString();
            IntervenienteCodigo = cartaoBandeira.IntervenienteCodigo;
            DataEmissao = cartaoTransacao.TransacaoDataHora;
            var daystoAdd = cartaoTransacao.FormaPagamentoCredito() ? cartaoBandeira.DatasParcelas.ElementAtOrDefault(numeroParcela - 1) : cartaoBandeira.DiasFloatDebito;
            DataVencimento = cartaoTransacao.TransacaoDataHora.AddDays(daystoAdd);
            Valor = cartaoTransacao.ValoresParcelas.ElementAtOrDefault(numeroParcela - 1);
            Operacao = operacaoCartao;
            CentroCustoCodigo = obra == null ? 0 : obra.UsinaEntregaCodigo;
            Situacao = cartaoBandeira.Situacao;
            PortadorCodigo = cartaoBandeira.PortadorCodigo;
            IdCadastro = StringHelper.GetIDD(usuario);
            IdAprovacao = "";
            BandeiraCodigo = cartaoBandeira.Codigo;
            CartaoNumero = cartaoTransacao.CartaoNumero.PadLeft(4, '0');
            var obs = contratoPagamentoDetalheCartao == null ? "" : $"{contratoPagamentoDetalheCartao.UsinaCodigo}-{contratoPagamentoDetalheCartao.ContratoNumero}/{contratoPagamentoDetalheCartao.ContratoAno} - Cliente: {obra.Contrato.IntervenienteCodigo} - {obra.Contrato.Interveniente.Razao}";
            Observacao = obs;
            AutorizacaoNumero = cartaoTransacao.AutorizacaoNumero;
            ContratoUsinaCodigo = contratoPagamentoDetalheCartao == null ? 0 : contratoPagamentoDetalheCartao.UsinaCodigo;
            ContratoNumero = contratoPagamentoDetalheCartao == null ? 0 : contratoPagamentoDetalheCartao.ContratoNumero;
            ContratoAno = contratoPagamentoDetalheCartao == null ? 0 : contratoPagamentoDetalheCartao.ContratoAno;
            ContaNumero = GerarContaNumeroPorNumeroAutorizacao(cartaoTransacao.AutorizacaoNumero);
            AgenciaNumero = cartaoTransacao.CartaoNumeroAsInteger;
            Alocado = (int)EContasAReceberStatusAlocado.Automatico;
        }
        public ContasAReceber(Obra obra, ContratoPagamento contratoPagamentoCartao, ContratoPagamentoDetalheCartao detalhePagamentoCartao, TipoCobranca tipoCobranca, int operacaoRecebimentoCliente, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.ContasAReceberCliente;
            DocumentoNumero = long.Parse($"{contratoPagamentoCartao.ContratoAno.ToString().PadLeft(2, '0')}{contratoPagamentoCartao.ContratoNumero.ToString().PadLeft(5, '0')}");
            DocumentoSerie = contratoPagamentoCartao.UsinaCodigo.ToString();
            Sequencia = $"{contratoPagamentoCartao.Sequencia}{detalhePagamentoCartao.DetalheSequencia}";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = operacaoRecebimentoCliente;
            DataEmissao = detalhePagamentoCartao.DataTransacao;
            DataVencimento = detalhePagamentoCartao.DataTransacao;
            Situacao = tipoCobranca.Situacao;
            PortadorCodigo = tipoCobranca.PortadorCodigo;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamentoCartao.Valor * -1;
            BandeiraCodigo = detalhePagamentoCartao.BandeiraCodigo;
            CartaoNumero = detalhePagamentoCartao.NumeroCartaoAsString;
            AutorizacaoNumero = detalhePagamentoCartao.NumeroAutorizacao;
            ContaNumero = GerarContaNumeroPorNumeroAutorizacao(detalhePagamentoCartao.NumeroAutorizacao);
            AgenciaNumero = detalhePagamentoCartao.NumeroCartao;
            IdCadastro = StringHelper.GetIDD(usuario);
            IdAprovacao = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = detalhePagamentoCartao.ContratoNumero;
            ContratoAno = detalhePagamentoCartao.ContratoAno;
            Observacao = "";
            Alocado = (int)EContasAReceberStatusAlocado.Vinculado;
        }

        public ContasAReceber(ObraVersao obra, ContratoPagamentoVersao contratoPagamentoCartao, ContratoPagamentoDetalheCartaoVersao detalhePagamentoCartao, TipoCobranca tipoCobranca, int operacaoRecebimentoCliente, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.ContasAReceberCliente;
            DocumentoNumero = long.Parse($"{contratoPagamentoCartao.ContratoAno.ToString().PadLeft(2, '0')}{contratoPagamentoCartao.ContratoNumero.ToString().PadLeft(5, '0')}");
            DocumentoSerie = contratoPagamentoCartao.UsinaCodigo.ToString();
            Sequencia = $"{contratoPagamentoCartao.Sequencia}{detalhePagamentoCartao.DetalheSequencia}";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = operacaoRecebimentoCliente;
            DataEmissao = detalhePagamentoCartao.DataTransacao;
            DataVencimento = detalhePagamentoCartao.DataTransacao;
            Situacao = tipoCobranca.Situacao;
            PortadorCodigo = tipoCobranca.PortadorCodigo;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamentoCartao.Valor * -1;
            BandeiraCodigo = detalhePagamentoCartao.BandeiraCodigo;
            CartaoNumero = detalhePagamentoCartao.NumeroCartaoAsString;
            AutorizacaoNumero = detalhePagamentoCartao.NumeroAutorizacao;
            ContaNumero = GerarContaNumeroPorNumeroAutorizacao(detalhePagamentoCartao.NumeroAutorizacao);
            AgenciaNumero = detalhePagamentoCartao.NumeroCartao;
            IdCadastro = StringHelper.GetIDD(usuario);
            IdAprovacao = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = detalhePagamentoCartao.ContratoNumero;
            ContratoAno = detalhePagamentoCartao.ContratoAno;
            Observacao = "";
            Alocado = (int)EContasAReceberStatusAlocado.Vinculado;
        }

        public ContasAReceber(Obra obra, ContratoPagamentoDetalheCheque detalhePagamentoCheque, ParametroFinanceiroCheque parametrosCheque, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.Cheque;
            DocumentoNumero = detalhePagamentoCheque.NumeroCheque;
            DocumentoSerie = "";
            Sequencia = "1";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = parametrosCheque.OperacaoPadraoInclusao;
            DataEmissao = detalhePagamentoCheque.DataRecebimento ?? DateTime.Today;
            DataVencimento = detalhePagamentoCheque.DataBomPara ?? detalhePagamentoCheque.DataRecebimento ?? DateTime.Today;
            Situacao = parametrosCheque.SituacaoPadrao;
            PortadorCodigo = parametrosCheque.PortadorPadrao;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamentoCheque.Valor;
            BandeiraCodigo = detalhePagamentoCheque.BancoCodigoOficial;
            AgenciaNumero = detalhePagamentoCheque.BancoAgencia;
            ContaNumero = detalhePagamentoCheque.BancoContaNumero;
            ContaDigito = detalhePagamentoCheque.BancoContaDV;
            IdCadastro = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = detalhePagamentoCheque.ContratoNumero;
            ContratoAno = detalhePagamentoCheque.ContratoAno;
        }

        public ContasAReceber(ObraVersao obra, ContratoPagamentoDetalheChequeVersao detalhePagamentoCheque, ParametroFinanceiroCheque parametrosCheque, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.Cheque;
            DocumentoNumero = detalhePagamentoCheque.NumeroCheque;
            DocumentoSerie = "";
            Sequencia = "1";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = parametrosCheque.OperacaoPadraoInclusao;
            DataEmissao = detalhePagamentoCheque.DataRecebimento ?? DateTime.Today;
            DataVencimento = detalhePagamentoCheque.DataBomPara ?? detalhePagamentoCheque.DataRecebimento ?? DateTime.Today;
            Situacao = parametrosCheque.SituacaoPadrao;
            PortadorCodigo = parametrosCheque.PortadorPadrao;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamentoCheque.Valor;
            BandeiraCodigo = detalhePagamentoCheque.BancoCodigoOficial;
            AgenciaNumero = detalhePagamentoCheque.BancoAgencia;
            ContaNumero = detalhePagamentoCheque.BancoContaNumero;
            ContaDigito = detalhePagamentoCheque.BancoContaDV;
            IdCadastro = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = detalhePagamentoCheque.ContratoNumero;
            ContratoAno = detalhePagamentoCheque.ContratoAno;
        }

        public ContasAReceber(Obra obra, ContratoPagamento contratoPagamentoCartao, ContratoPagamentoDetalhe detalhePagamento, int operacaoRecebimentoCliente, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.ContasAReceberCliente;
            DocumentoNumero = long.Parse($"{obra.AnoContrato.ToString().PadLeft(2, '0')}{obra.NumContrato.ToString().PadLeft(5, '0')}");
            DocumentoSerie = obra.UsinaCodigo.ToString();
            Sequencia = $"{contratoPagamentoCartao.Sequencia}{detalhePagamento.DetalheSequencia}";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = operacaoRecebimentoCliente;
            DataEmissao = detalhePagamento.DataTitulo() ?? DateTime.Today;
            DataVencimento = DataEmissao;
            Situacao = contratoPagamentoCartao.TipoCobranca.Situacao;
            if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhePagamento.GetType()))
                PortadorCodigo = (((ContratoPagamentoDetalheDeposito)detalhePagamento).PortadorCodigo ?? contratoPagamentoCartao.TipoCobranca.PortadorCodigo);
            else
                PortadorCodigo = contratoPagamentoCartao.TipoCobranca.PortadorCodigo;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamento.Valor * -1;
            IdCadastro = StringHelper.GetIDD(usuario);
            IdAprovacao = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = obra.NumContrato ?? 0;
            ContratoAno = obra.AnoContrato ?? 0;
        }

        public ContasAReceber(ObraVersao obra, ContratoPagamentoVersao contratoPagamentoCartao, ContratoPagamentoDetalheVersao detalhePagamento, int operacaoRecebimentoCliente, string usuario = "AUTO")
        {
            EmpresaCod = obra.UsinaEntrega.EmpresaCodigo;
            DocumentoTipo = (int)EDocumentoTipo.ContasAReceberCliente;
            DocumentoNumero = long.Parse($"{obra.AnoContrato.ToString().PadLeft(2, '0')}{obra.NumContrato.ToString().PadLeft(5, '0')}");
            DocumentoSerie = obra.UsinaCodigo.ToString();
            Sequencia = $"{contratoPagamentoCartao.Sequencia}{detalhePagamento.DetalheSequencia}";
            IntervenienteCodigo = obra.Contrato.IntervenienteCodigo;
            Operacao = operacaoRecebimentoCliente;
            DataEmissao = detalhePagamento.DataTitulo() ?? DateTime.Today;
            DataVencimento = DataEmissao;
            Situacao = contratoPagamentoCartao.TipoCobranca.Situacao;
            if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhePagamento.GetType()))
                PortadorCodigo = (((ContratoPagamentoDetalheDepositoVersao)detalhePagamento).PortadorCodigo ?? contratoPagamentoCartao.TipoCobranca.PortadorCodigo);
            else
                PortadorCodigo = contratoPagamentoCartao.TipoCobranca.PortadorCodigo;
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            Valor = detalhePagamento.Valor * -1;
            IdCadastro = StringHelper.GetIDD(usuario);
            IdAprovacao = StringHelper.GetIDD(usuario);
            ContratoUsinaCodigo = obra.UsinaCodigo;
            ContratoNumero = obra.NumContrato ?? 0;
            ContratoAno = obra.AnoContrato ?? 0;
        }

        public int EmpresaCod { get; set; }
        public int DocumentoTipo { get; set; }
        public string DocumentoSerie { get; set; } = "";
        public long DocumentoNumero { get; set; }
        public string Sequencia { get; set; } = "";
        public int Desdobramento { get; set; }
        public int AgenciaNumero { get; set; }
        public long ContaNumero { get; set; }
        public int ContaDigito { get; set; }
        public int? IntervenienteCodigo { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public int Operacao { get; set; }
        public int CentroCustoCodigo { get; set; }
        public int Situacao { get; set; }
        public int? PortadorCodigo { get; set; }
        public string IdCadastro { get; set; } = "";
        public string IdAprovacao { get; set; } = "";
        public int BandeiraCodigo { get; set; }
        public string CartaoNumero { get; set; } = "";
        public string Observacao { get; set; } = "";
        public string AutorizacaoNumero { get; set; } = "";
        public int ContratoUsinaCodigo { get; set; }
        public int ContratoNumero { get; set; }
        public int ContratoAno { get; set; }
        public int Alocado { get; set; }
        public long IdMovimentoBanco { get; set; }
        public float SomaRecebimentos { get; set; }
        public int LoteConciliado { get; set; }
        public string CartaoConciliado { get; set; } = "";

        public long GerarContaNumeroPorNumeroAutorizacao(string autorizacaoNumero)
        {
            var numeroAutorizacaoSemletras = autorizacaoNumero.Trim().RemoverCaracteresNaoNumericos();
            long.TryParse(numeroAutorizacaoSemletras.Substring(numeroAutorizacaoSemletras.Length >= 9 ? numeroAutorizacaoSemletras.Length - 9 : 0), out long autorizacaoNumerica);
            return autorizacaoNumerica;
        }

    }
}

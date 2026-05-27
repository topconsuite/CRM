using System;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class MovimentoBanco
    {
        private const string ENTRADA = "1";

        public long Id { get; set; }
        public int EmpresaCodigo { get; set; }
        public int ContaCodigo { get; set; }
        public DateTime DataOperacao { get; set; }
        public int DocumentoTipo { get; set; }
        public string DocumentoNumero { get; set; }
        public string EntradaSaida { get; set; }
        public int OperacaoCodigo { get; set; }
        public string OperacaoDescricao { get; set; }
        public float Valor { get; set; }
        public float Saldo { get; set; }
        public string Origem { get; set; }
        public int CentroCustoCodigo { get; set; }
        public string IdCadastro { get; set; }
        public string Observacao { get; set; }

        //PARA INTEGRACAO PUBLICA

        public long NaturezaFinanceira { get; set; } = 0;

        public MovimentoBanco() { }

        public MovimentoBanco(Obra obra, ContratoPagamento pagamento, ContratoPagamentoDetalhe pagamentoDetalhe, int operacao, string usuario = "AUTO")
        {
            Portador portador;
            if (typeof(ContratoPagamentoDetalheDeposito).Equals(pagamentoDetalhe.GetType()))
                portador = (((ContratoPagamentoDetalheDeposito)pagamentoDetalhe).Portador ?? pagamento.TipoCobranca.Portador);
            else
                portador = pagamento.TipoCobranca.Portador;

            EmpresaCodigo = obra.UsinaEntrega.EmpresaCodigo;
            ContaCodigo = portador.ContaCodigo ?? 0;
            DataOperacao = pagamentoDetalhe.DataTitulo() ?? DateTime.Today;
            DocumentoTipo = (int)EDocumentoTipo.CreditoCliente;
            DocumentoNumero = DataOperacao.ToString("ddMMyy");
            EntradaSaida = ENTRADA;
            OperacaoCodigo = operacao;
            Valor = pagamentoDetalhe.Valor;
            Origem = "CTR";
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            IdCadastro = StringHelper.GetIDD(usuario);
            Observacao = $"{obra.UsinaCodigo.ToString().PadLeft(3, '0')}-{obra.NumContrato.ToString().PadLeft(6, '0')}/{obra.AnoContrato.ToString().PadLeft(2, '0')} {obra.Contrato.IntervenienteCodigo} - {obra.IntervenienteNome}";
        }

        public MovimentoBanco(ObraVersao obra, ContratoPagamentoVersao pagamento, ContratoPagamentoDetalheVersao pagamentoDetalhe, int operacao, string usuario = "AUTO")
        {
            Portador portador;
            if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(pagamentoDetalhe.GetType()))
                portador = (((ContratoPagamentoDetalheDepositoVersao)pagamentoDetalhe).Portador ?? pagamento.TipoCobranca.Portador);
            else
                portador = pagamento.TipoCobranca.Portador;

            EmpresaCodigo = obra.UsinaEntrega.EmpresaCodigo;
            ContaCodigo = portador.ContaCodigo ?? 0;
            DataOperacao = pagamentoDetalhe.DataTitulo() ?? DateTime.Today;
            DocumentoTipo = (int)EDocumentoTipo.CreditoCliente;
            DocumentoNumero = DataOperacao.ToString("ddMMyy");
            EntradaSaida = ENTRADA;
            OperacaoCodigo = operacao;
            Valor = pagamentoDetalhe.Valor;
            Origem = "CTR";
            CentroCustoCodigo = obra.UsinaEntregaCodigo;
            IdCadastro = StringHelper.GetIDD(usuario);
            Observacao = $"{obra.UsinaCodigo.ToString().PadLeft(3, '0')}-{obra.NumContrato.ToString().PadLeft(6, '0')}/{obra.AnoContrato.ToString().PadLeft(2, '0')} {obra.Contrato.IntervenienteCodigo} - {obra.IntervenienteNome}";
        }

        //PARA INTEGRAÇÃO PÚBLICA

        public MovimentoBanco(TituloContasAReceber tituloContasAReceber, int? tipoMovimento, int? Operacao, long? naturezaFinanceira, string usuario = "API AR", string obs = "API ACCOUNTS RECEIVABLE")
        {
            EmpresaCodigo = tituloContasAReceber.EmpresaCodigo;
            ContaCodigo = tituloContasAReceber.BancoLiquidacao;
            DataOperacao = (DateTime)tituloContasAReceber.DataLiquidacao;
            DocumentoTipo = (int)tipoMovimento;
            DocumentoNumero = tituloContasAReceber.DocumentoNumero.ToString();
            EntradaSaida = ENTRADA;
            Origem = "CAR";
            NaturezaFinanceira = (long)naturezaFinanceira;
            OperacaoCodigo = (int)Operacao;
            Valor = tituloContasAReceber.LiquidacaoValorRecebido;
            Observacao = obs;
            IdCadastro = usuario;
        }
    }
}

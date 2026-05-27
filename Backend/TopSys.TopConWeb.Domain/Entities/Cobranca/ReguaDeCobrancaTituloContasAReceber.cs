using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.ReguaDeCobranca
{
    public class ReguaDeCobrancaTituloContasAReceber
    {
        public ReguaDeCobrancaTituloContasAReceber(TituloContasAReceber tituloContasAReceber, Interveniente interveniente, Organizacao organizacao, string linkSegundaViaBoleto)
        {
            TituloContasAReceber = tituloContasAReceber;
            Interveniente = interveniente; ;
            Organizacao = organizacao;
            LinkSegundaViaBoleto = linkSegundaViaBoleto;
        }

        public TituloContasAReceber TituloContasAReceber { get; private set; }
        public Interveniente Interveniente { get; private set; }
        public Organizacao Organizacao { get; private set; }
        public string LinkSegundaViaBoleto { get; private set; }

        public string Id
        {
            get => $"{TituloContasAReceber.EmpresaCodigo.ToString("D2")}" +
                   $"{TituloContasAReceber.DocumentoTipoCodigo.ToString("D2")}" +
                   $"{TituloContasAReceber.DocumentoSerie.PadLeft(5, '0')}" +
                   $"{TituloContasAReceber.DocumentoNumero.ToString("D11")}" +
                   $"{TituloContasAReceber.DocumentoSequencia.PadLeft(3, '0')}" +
                   $"{TituloContasAReceber.BancoCodigoOficial.ToString("D3")}" +
                   $"{TituloContasAReceber.BancoNumeroAgencia.ToString("D4")}" +
                   $"{TituloContasAReceber.BancoNumeroConta.ToString("D10")}" +
                   $"{TituloContasAReceber.BancoDvConta.ToString("D1")}" +
                   $"{TituloContasAReceber.Desdobramento.ToString("D2")}" +
                   $"{Organizacao.Codigo.ToString("D2")}";
        }

        public string Status
        {
            get
            {
                var dataHoje = DateTime.Now.Date;

                if (TituloContasAReceber.DataLiquidacao == null && TituloContasAReceber.DataVencimento?.Date >= dataHoje)
                {
                    return "A VENCER";
                }
                else if (TituloContasAReceber.DataLiquidacao != null)
                {
                    return "RECEBIDO";
                }
                else if (TituloContasAReceber.DataLiquidacao == null && TituloContasAReceber.DataVencimento?.Date < dataHoje)
                {
                    return "ATRASADO";
                }

                return "";

            }
        }


        public string ClienteTipo
        {
            get => Interveniente.IntervenienteTipo == "F" ? "PF" : "PJ";
        }

        public string PagamentoMetodo
        {
            get => TituloContasAReceber.DocumentoTipoCodigo == (int)EDocumentoTipo.Cheque ? "cheque" : (TituloContasAReceber.LinhaDigitavelBoleto ?? "") == "" ? "carteira" : "boleto" ;
        }

        public string ClienteTelefone
        {
            get
            {
                if (Interveniente.CelularDdd != 0 && Interveniente.CelularNumero != 0)
                    return $"{Interveniente.CelularDdd}{Interveniente.CelularNumero}";
                else
                    return $"{Interveniente.TelefoneDdd}{Interveniente.TelefoneNumero}";
            }

        }
    }
}

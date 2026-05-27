using System;
using System.Collections.Generic;
using System.Linq;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Infra.Legacy.QueryResults
{
    public class ConsultarObraQueryResult: IQueryResult
    {
        public int Usina { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }
        public int VendedorCodigo { get; set; }
        public string VendedorNome { get; set; }
        public int ClienteCodigo { get; set; }
        public string ClienteRazao { get; set; }
        public string ClienteCpfCnpj { get; set; }
        public int ClienteTelefoneDdd { get; set; }
        public int ClienteTelefoneNumero { get; set; }
        public int ClienteCelularDdd { get; set; }
        public int ClienteCelularNumero { get; set; }
        public int ClienteTelefoneComercialDdd { get; set; }
        public int ClienteTelefoneComercialNumero { get; set; }
        public float ClienteLimiteValor { get; set; }
        public DateTime? ClienteLimiteData { get; set; }
        public float ClienteSaldoContasAReceber { get; set; }
        public float ClienteValorTotalNfsNaoFaturadas { get; set; }
        public float ValorProgramado { get; set; }
        public float ValorProgramadoCliente { get; set; }
        public float LimiteCreditoDisponivel { get; set; }
        public float LimiteCreditoSaldo { get; set; }
        public string LimiteCreditoAnalise { get; set; }
        public DateTime ContratoData { get; set; }
        public float VolumeTotal { get; set; }
        public int Status { get; set; }
        public string StatusDescricao { get; set; }
        public int AnalistaCodigo { get; set; }
        public string AnalistaNomeReduzido { get; set; }
        public int GrupoEconomicoCodigo { get; set; }
        public string GrupoEconomicoDescricao { get; set; }

        public decimal ContratoValorTotal { get; set; }
        public int UsinaEntrega { get; set; }
        public string UsinaEntregaSigla { get; set; }
        public int UsinaEntregaFilial { get; set; }
        public int TipoCobrancaCodigo { get; set; }
        public string TipoCobrancaDescricao { get; set; }
        public int ObraNumero { get; set; }
        public string ObraNome { get; set; }
        public string ObraContato { get; set; }
        public string ObraMunicipio { get; set; }
        public DateTime DataConcretagem { get; set; }
        public string Horario { get; set; }
        public EObraStatusCadastro StatusCadastro { get; set; }
        public EObraStatusComercial StatusComercial { get; set; }
        public EObraStatusEngenharia StatusEngenharia { get; set; }
        public EObraStatusFinanceiro StatusFinanceiro { get; set; }
        public EObraDemaisStatusComercial VolumeStatusComercial { get; set; }
        public int CondicaoPagamentoCodigo { get; set; }
        public string CondicaoPagamentoDescricao { get; set; }
        public string UsaAdicionalZMRC { get; set; }
        public string NecessitaAprovacaoZMRC { get; set; }
        public ESituacaoAprovacaoComercialAlcadaUsuario StatusAprovComAlcada { get; set; } = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;

        public virtual IEnumerable<ContratoClicksignEnvio> ClicksignEnvio { get; set; } = new List<ContratoClicksignEnvio>();
        public EStatusClicksignDocumento StatusClicksignDocumento
        {
            get
            {
                var ultimoDocumentoEnviado = ClicksignEnvio.ToList().OrderByDescending(t => t.DataEnvio).FirstOrDefault();
                if (ultimoDocumentoEnviado != null)
                    return ultimoDocumentoEnviado.StatusClicksignDocumento;

                return EStatusClicksignDocumento.NaoEnviado;
            }
        }
    }

    public enum ESituacaoAprovacaoComercialAlcadaUsuario
    {

        NaoUtiliza = 0,
        AguardandoAprovacao = 1,
        AguardandoAprovacaoOutroNivel = 2

    }

}

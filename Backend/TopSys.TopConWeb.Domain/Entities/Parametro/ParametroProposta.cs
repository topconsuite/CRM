using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ParametroProposta : Parametro
    {
        public ParametroProposta() : base() { }

        public bool ObrigaNumeroContratoAnterior { get; set; }

        public bool ObrigaProfissao { get; set; }

        public bool ObrigaVolumeEstimadoEPrevisaoInicioTermino { get; set; }

        public bool ObrigaAprovacaoDistanciaUsinaEntrega { get; set; }

        public bool ObrigaEmailPessoaFisica { get; set; }

        public bool BloqueiaImpressaoPropostaContratoPendenteAprovacao { get; set; }

        public float PercentualDescontoLimite { get; set; }
        public bool DadosFilialNaImpressaoProposta { get; set; }

        public bool OcultarTaxaProposta { get; set; }

        public bool ObrigaTipoObra { get; set; }

        public bool ObrigaPorteObra { get; set; }

        public string MensagemReajustePadrao { get; set; }

        public int ValidadeProposta { get; set; }
        public bool InformarBombaTerceiros { get; set; }
        public bool ObrigaFimVigencia { get; set; }
    }
}

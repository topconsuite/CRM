using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities.Integracao
{
    public class TotaisContasRecebereRemessasPorCliente
    {

        public string Cliente { get; set; }

        public double ValorFaturamentoTotal { get; set; }
        public double VolumeM3ContratadoTotal { get; set; }
        public double VolumeM3EntregueTotal { get; set; }

        public ResumoCredito Credito { get; set; }

        public int QuantidadeContratos { get; set; }

        public IEnumerable<TotaisContrato> TotaisPorContrato { get; set; }

    }

    public class ResumoCredito
    {

        public double LimiteCredito { get; set; }
        public double SaldoCredito { get; set; }
        public double ValorEmAbertoTotal { get; set; }
        public double ValorNotasFiscaisNaoFaturadas { get; set; }

    }

    public class TotaisContrato
    {
        public int UsinaContrato { get; set; }
        public int AnoContrato { get; set; }
        public int NumeroContrato { get; set; }

        public double VolumeM3Contratado { get; set; }
        public double VolumeM3Entregue { get; set; }
        public double FaturamentoContrato { get; set; }
    }

}

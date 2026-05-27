using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigitalDetalhesDistribuicao
    {
        public long Nsu { get; set; }

        public string ChaveNfe { get; set; }

        public string SchemaDistribuicao { get; set; }

        public long? CnpjCpfFornecedor { get; set; }

        public DateTime? DataHoraEmissao { get; set; }

        public float? ValorNotaFiscalSefaz { get; set; }

        public DateTime? DataHoraRecebimento { get; set; }

        public long? NumeroProtocolo { get; set; }

        public int? CodigoTipoEvento { get; set; }

        public string XmlEntrada { get; set; }

        public DateTime? DataHoraEvento { get; set; }
        
        public int? IndicadorDestinatarioIe { get; set; }
    }
}

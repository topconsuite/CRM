using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalDigitalDetalhesFiscais
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }

        public int TipoDocumento { get; set; }

        public string Serie { get; set; }

        public long Numero { get; set; }

        public string SituacaoSefaz { get; set; }

        public long? ReciboSefaz { get; set; }

        public long? ProtocoloSefaz { get; set; }

        public int? StatusAutorizacao { get; set; }

        public string MotivoDescricaoStatus { get; set; }

        public DateTime? DataHoraProtocolo { get; set; }

        public string Xml { get; set; }

        public string XmlAutor { get; set; }

        public string NfeUf { get; set; }
        
        public int? IndicadorDestinatarioIe { get; set; }
    }
}

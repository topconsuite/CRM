using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class ContratoClicksignEnvio
    {
        public Guid Id { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public int ContratonUsina { get; set; }
        public Guid IdClicksign { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public string IdEnvio { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string IdCancelamento { get; set; }
        public EStatusClicksignDocumento StatusClicksignDocumento
        {
            get
            {
                if (DataCancelamento != null)
                    return EStatusClicksignDocumento.Cancelado;

                if (DataAssinatura != null)
                    return EStatusClicksignDocumento.Assinado;

                return EStatusClicksignDocumento.Processando;
            }
        }
    }
}

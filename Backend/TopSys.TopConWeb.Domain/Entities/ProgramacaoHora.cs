using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ProgramacaoHora
    {
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public int Sequencia { get; set; }

        public string Horario { get; set; }

        public float VolumeProgramado { get; set; }
        public float VolumeEntregue { get; set; }

        public int CorpoDeProvaQuantidade { get; set; }

        public string Status { get; set; }

        public int? NfFilialCodigo { get; set; } = 0;
        public int? NfIntervenienteCodigo { get; set; } = 0;
        public int? NfTipoDocumentoCodigo { get; set; } = 0;
        public long? NfNumero { get; set; } = 0;
        public string NfSerie { get; set; } = "";
        public int? NfSequencia { get; set; } = 0;

        public virtual NotaFiscalFisica Nf { get; set; }
    }
}

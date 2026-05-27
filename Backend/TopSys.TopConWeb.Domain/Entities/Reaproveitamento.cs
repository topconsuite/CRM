using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Reaproveitamento
    {
        //filial
        public int FilialCodigo { get; set; }

        //interv
        public int IntervenienteCodigo { get; set; }

        //tp_doc
        public int TipoDocumentoCodigo { get; set; }

        //num_nf
        public long Numero { get; set; }

        //serie
        public string Serie { get; set; }

        //seq_nf
        public int Sequencia { get; set; }

        //bt_reaprov
        public string BetoneiraReaproveitamento { get; set; }

        //usina
        public int UsinaCodigo { get; set; }

        //data_remessa
        public DateTime DataRemessa { get; set; }

        //sb_vol_retorno
        public float VolumeRetorno { get; set; }

        //sb_status
        public int Status { get; set; }

        //sb_observacao
        public string Observacao { get; set; }

        //ap_filial
        public int FilialNotaDestino { get; set; }

        //ap_interv
        public int IntervenienteNotaDestino { get; set; }

        //ap_tp_doc
        public int TipoDocumentoNotaDestino { get; set; }

        //ap_num_nf
        public long NumeroNotaDestino { get; set; }

        //ap_serie
        public string SerieNotaDestino { get; set; }

        //ap_seq_nf
        public int SequenciaNotaDestino { get; set; }

        //ap_usina
        public int UsinaNotaDestino { get; set; }

        //id_cadast
        public string IdCadastro { get; set; }

        //id_atual
        public string IdAtual { get; set; }
    }
}

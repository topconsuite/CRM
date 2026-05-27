using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalFisicaDemaisServicos
    {
        public int FilialCodigo { get; set; }

        public int IntervenienteCodigo { get; set; }

        public int TipoDocumentoCodigo { get; set; }

        public long Numero { get; set; }

        public string Serie { get; set; }

        public int Sequencia { get; set; }

        public int SequenciaServico { get; set; }

        public string MercadoriaCodigo { get; set; }

        public string MercadoriaDescricao
        {
            get { return Mercadoria != null ? Mercadoria.Descricao : ""; }
        }

        public virtual Mercadoria Mercadoria { get; set; }

        public double Quantidade { get; set; }

        public double ValorUnitario { get; set; }

        public double ValorTotal { get; set; }

        public double ValorCobrado { get; set; }
    }
}

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ChaveNotaFiscalDigital
    {
        public int Filial { get; set; }

        public int Cliente { get; set; }

        public int TipoDocumento { get; set; }

        public string Serie { get; set; }

        public long Numero { get; set; }

        public int Sequencia { get; set; }
    }
}
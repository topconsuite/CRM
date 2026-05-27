using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class BoletoExterno
    {
        public int? UsinaCodigo { get; set; }

        public int? ContratoNumero { get; set; }

        public int? ContratoAno { get; set; }

        public int? Filial { get; set; }

        public int? Cliente { get; set; }

        public int? TipoDocumento { get; set; }

        public long? FaturaNumero { get; set; }

        public string FaturaSerie { get; set; }

        public int? FaturaSubSerie { get; set; }

        public byte[] Arquivo { get; set; }

        public int? Sequencia { get; set; }

        public string Chave { get; set; }

        public Guid Id { get; set; }

        public string NomeArquivo { get; set; }

        public DateTime DataHora { get; set; }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalDetalhesDistribuicaoMap : EntityTypeConfiguration<NotaFiscalDigitalDetalhesDistribuicao>
    {
        public NotaFiscalDigitalDetalhesDistribuicaoMap()
        {
            ToTable("topnfe.nfe_distribuicao");

            HasKey(t => new { t.Nsu, t.ChaveNfe });

            Property(t => t.Nsu)
                .HasColumnOrder(0)
                .HasColumnName("nfe_distribuicao_NSU");

            Property(t => t.ChaveNfe)
                .HasColumnOrder(1)
                .HasColumnName("nfe_distribuicao_chNFe");

            Property(t => t.SchemaDistribuicao)
                .HasColumnName("nfe_distribuicao_schema");

            Property(t => t.CnpjCpfFornecedor)
                .HasColumnName("nfe_distr_forn_CNPJ_CPF");

            Property(t => t.DataHoraEmissao)
                .HasColumnName("nfe_distribuicao_dhEmi");

            Property(t => t.ValorNotaFiscalSefaz)
                .HasColumnName("nfe_distribuicao_vNF");

            Property(t => t.DataHoraRecebimento)
                .HasColumnName("nfe_distribuicao_dhRecbto");

            Property(t => t.NumeroProtocolo)
                .HasColumnName("nfe_distribuicao_nProt");

            Property(t => t.CodigoTipoEvento)
                .HasColumnName("nfe_distribuicao_tpEvento");

            Property(t => t.XmlEntrada)
                .HasColumnName("nfe_distribuicao_XML");

            Property(t => t.DataHoraEvento)
                .HasColumnName("nfe_distribuicao_dhEvento");
        }
    }
}

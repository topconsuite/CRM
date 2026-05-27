using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalDetalhesFiscaisMap : EntityTypeConfiguration<NotaFiscalDigitalDetalhesFiscais>
    {
        public NotaFiscalDigitalDetalhesFiscaisMap()
        {
            ToTable("topnfe.nfe");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero});

            Property(t => t.Filial)
                .HasColumnOrder(0)
                .HasColumnName("ger_emp_Cod");

            Property(t => t.Cliente)
                .HasColumnOrder(1)
                .HasColumnName("ger_Interv_Cod");

            Property(t => t.TipoDocumento)
                .HasColumnOrder(2)
                .HasColumnName("nfe_Mod");

            Property(t => t.Serie)
                .HasColumnOrder(3)
                .HasColumnName("nfe_Ser");

            Property(t => t.Numero)
                .HasColumnOrder(4)
                .HasColumnName("nfe_nNF");

            Property(t => t.SituacaoSefaz)
                .HasColumnName("nfe_cSit");

            Property(t => t.ReciboSefaz)
                .HasColumnName("nfe_nRec");

            Property(t => t.ProtocoloSefaz)
                .HasColumnName("nfe_nProt");

            Property(t => t.StatusAutorizacao)
                .HasColumnName("nfe_cStat");

            Property(t => t.MotivoDescricaoStatus)
                .HasColumnName("nfe_Motivo");

            Property(t => t.DataHoraProtocolo)
                .HasColumnName("nfe_dhProc");

            Property(t => t.Xml)
                .HasColumnName("nfe_XML");

            Property(t => t.XmlAutor)
                .HasColumnName("nfe_XMLAutor");

            Property(t => t.NfeUf)
                .HasColumnName("nfe_Uf");
        }
    }
}

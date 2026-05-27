using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TipoDocumentoMap : EntityTypeConfiguration<TipoDocumento>
    {
        public TipoDocumentoMap()
        {
            ToTable("topsys.ger_tp_doc");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Abreviacao)
                .HasColumnName("Abrev");
            
            Property(t => t.Descricao)
                .HasColumnName("Descr");

            Property(t => t.Fixo)
                .HasColumnName("fixo");
            
            Property(t => t.ModDoc)
                .HasColumnName("mod_doc");
            
            Property(t => t.Nfse)
                .HasColumnName("nfse");

            Property(t => t.TpDocServ)
                .HasColumnName("tp_doc_serv");
        }
    }
}
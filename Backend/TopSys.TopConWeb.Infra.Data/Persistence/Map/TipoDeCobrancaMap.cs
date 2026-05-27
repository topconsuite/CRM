using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TipoDeCobrancaMap : EntityTypeConfiguration<TipoDeCobranca>
    {
        public TipoDeCobrancaMap()
        {
            ToTable("topsys.con_tipo_cobranca");

            HasKey(t => t.Codigo);
            
            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("tipo_cobranca")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Forma)
                .HasColumnName("forma");
            
            Property(t => t.Portador)
                .HasColumnName("portador");

            Property(t => t.Situacao)
                .HasColumnName("situacao");

            Property(t => t.Obrigatorio)
                .HasColumnName("obrigatorio");

            Property(t => t.Aprovacao)
                .HasColumnName("aprovacao");

            Property(t => t.Fixo)
                .HasColumnName("fixo");

            Property(t => t.UtilCap)
                .HasColumnName("util_cap");

            Ignore(t => t.Descricao);
  
        }
    }
}
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TributacaoPisCofinsMap: EntityTypeConfiguration<TributacaoPisCofins>
    {
        public TributacaoPisCofinsMap()
        {
            ToTable("topsys.ger_tribcontrib");
            
            HasKey(t => t.Codigo);
            
            Property(t => t.Codigo)
                .HasColumnName("Cod");
            
            Property(t => t.Descricao)
                .HasColumnName("Descr");
        }
    }
}
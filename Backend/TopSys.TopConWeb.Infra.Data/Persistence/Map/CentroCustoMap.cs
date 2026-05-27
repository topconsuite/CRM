using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CentroCustoMap: EntityTypeConfiguration<CentroCusto>
    {
        public CentroCustoMap()
        {
            ToTable("topsys.fin_ccusto");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.Fixo)
                .HasColumnName("reg_fixo");

            Property(t => t.Ativo)
                .HasColumnName("ativo");
        }
    }
}

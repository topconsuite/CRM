using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    class TributacaoReformaMap : EntityTypeConfiguration<TributacaoReforma>
    {
        public TributacaoReformaMap()
        {
            ToTable("topsys.ger_imp_tribut_reforma");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnName("id_imp");
        }
    }
}

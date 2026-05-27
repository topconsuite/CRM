using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class UnidadeMap: EntityTypeConfiguration<Unidade>
    {
        public UnidadeMap()
        {
            ToTable("topsys.ger_unidade");

            HasKey(t => new { t.Sigla });

            Property(t => t.Sigla)
                .HasColumnOrder(0)
                .HasColumnName("unidade");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.TipoCodigo)
                .HasColumnName("tipo");
        }
    }
}

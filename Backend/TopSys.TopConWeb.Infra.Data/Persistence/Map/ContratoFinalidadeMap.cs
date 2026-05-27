using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoFinalidadeMap : EntityTypeConfiguration<ContratoFinalidade>
    {

        public ContratoFinalidadeMap()
        {
            ToTable("topsys.con_finalidade_ctr");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

        }

    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades
{
    public class OportunidadeFaseMap : EntityTypeConfiguration<OportunidadeFase>
    {
        public OportunidadeFaseMap()
        {
            ToTable("topsys.con_fase_oportunidade");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.Cor)
                .HasColumnName("cor");
        }
    }
}

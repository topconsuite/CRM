using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class TipoVisitaMap : EntityTypeConfiguration<TipoVisita>
    {
        public TipoVisitaMap()
        {
            ToTable("topsys.con_tipo_visita");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

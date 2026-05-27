using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class GrupoEconomicoMap : EntityTypeConfiguration<GrupoEconomico>
    {
        public GrupoEconomicoMap()
        {
            ToTable("topsys.ger_grupo_economico");

            HasKey(t => t.Codigo);

            HasOptional(t => t.BloqueioMotivo)
                .WithMany()
                .HasForeignKey(t => t.BloqueioMotivoCodigo);

            HasMany(t => t.Clientes)
               .WithOptional()
               .HasForeignKey(t => t.GrupoEconomicoCodigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.LimiteValor)
                .HasColumnName("limite_cred");

            Property(t => t.LimiteData)
                .HasColumnName("lim_cred_val");

            Property(t => t.BloqueioMotivoCodigo)
                .HasColumnName("bloq");

            Property(t => t.BloqueioObservacao)
                .HasColumnName("obs_bloq");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

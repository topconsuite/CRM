using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ParametrosSSOMap : EntityTypeConfiguration<ParametrosSSO>
    {
        public ParametrosSSOMap()
        {
            ToTable("topsys.parametros_sso");

            HasKey(t => new { t.Id });

            Property(t => t.Habilitado)
                .HasColumnName("sso_habilitado");

            Property(t => t.Dominio)
                .HasColumnName("dominio");

            Property(t => t.TipoProvedor)
                .HasColumnName("tipo_provedor");

            Property(t => t.TenantId)
                .HasColumnName("tenant_id");

            Property(t => t.ClientId)
                .HasColumnName("client_id");

            Property(t => t.UrlRedirecionamento)
                .HasColumnName("url_redirecionamento");

            Property(t => t.UrlRedirecionamento)
                .HasColumnName("url_redirecionamento");
        }
    }
}

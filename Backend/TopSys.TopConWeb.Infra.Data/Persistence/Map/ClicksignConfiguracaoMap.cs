using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ClicksignConfiguracaoMap : EntityTypeConfiguration<ClicksignConfiguracao>
    {
        public ClicksignConfiguracaoMap()
        {
            ToTable("topsys.configuracoes_clicksign");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnName("id");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.Token)
                .HasColumnName("token");

            Property(t => t.BaseUrl)
                .HasColumnName("base_url");

            Property(t => t.Alias)
                .HasColumnName("alias");

            Property(t => t.Sha256Secret)
                .HasColumnName("sha256_secret");

            Property(t => t.Ativo)
                .HasColumnName("active");
        }
    }
}

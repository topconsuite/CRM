using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaUsuariosMap : EntityTypeConfiguration<AprovacaoComercialHierarquiaUsuario>
    {

        public AprovacaoComercialHierarquiaUsuariosMap() 
        {

            ToTable("topsys.con_aprovacao_comercial_hierarquia_usuario");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("id");

            Property(x => x.AprovacaoComercialHierarquiaId)
                .HasColumnOrder(0)
                .HasColumnName("aprovacao_comercial_hierarquia_id");

            Property(x => x.UsuarioId)
                .HasColumnOrder(1)
                .HasColumnName("usuario_id");

            HasRequired(x => x.AprovacaoComercialHierarquia)
                .WithMany()
                .HasForeignKey(x => x.AprovacaoComercialHierarquiaId);

        }

    }
}

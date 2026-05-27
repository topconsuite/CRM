using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.LiberacaoAcesso
{
    public class PeriodoAusenciaUsuarioMap : EntityTypeConfiguration<PeriodoAusenciaUsuario>
    {
        public PeriodoAusenciaUsuarioMap()
        {
            ToTable("topsys.usr_periodo_ausencia");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.TipoLiberacao)
                .HasColumnName("tipo_liberacao");

            Property(t => t.TipoAusencia)
                .HasColumnName("tipo_ausencia");

            Property(t => t.InicioPeriodo)
                .HasColumnName("inicio_periodo");

            Property(t => t.FimPeriodo)
                .HasColumnName("fim_periodo");

            Property(t => t.CriadoEm)
                .HasColumnName("criado_em");

            Property(t => t.AtualizadoEm)
                .HasColumnName("atualizado_em");
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using EntidadesLiberacaoAcesso = TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.LiberacaoAcesso
{
    public class LiberacaoAcessoMap : EntityTypeConfiguration<EntidadesLiberacaoAcesso.LiberacaoAcesso>
    {
        public LiberacaoAcessoMap()
        {
            ToTable("topsys.usr_liberacao_acesso");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.Grupo)
                .IsRequired()
                .HasColumnName("grupo");

            Property(t => t.TipoLiberacao)
                .HasColumnName("tipo_liberacao");

            Property(t => t.DiaSemana)
                .HasColumnName("dia_semana");

            Property(t => t.Turno)
                .HasColumnName("turno");

            Property(t => t.HoraEntrada)
                .HasColumnName("hora_entrada");

            Property(t => t.HoraSaida)
                .HasColumnName("hora_saida");

            Property(t => t.Bloquear)
                .HasColumnName("bloquear");

            Property(t => t.CriadoEm)
                .HasColumnName("criado_em");

            Property(t => t.AtualizadoEm)
                .HasColumnName("atualizado_em");
        }
    }
}

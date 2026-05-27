using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.LiberacaoAcesso
{
    public class LiberacaoAcessoLogMap : EntityTypeConfiguration<LiberacaoAcessoLog>
    {
        public LiberacaoAcessoLogMap()
        {
            ToTable("topsys.usr_liberacao_acesso_log");

            HasKey(t => new { t.TipoLiberacao, t.DataHoraEvento, t.Usuario, t.Evento });

            Property(t => t.TipoLiberacao)
                .HasColumnOrder(0)
                .HasColumnName("tipo_liberacao")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.DataHoraEvento)
                .HasColumnOrder(1)
                .HasColumnName("dt_hora_evento")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Usuario)
                .HasColumnOrder(2)
                .HasColumnName("usuario")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Evento)
                .HasColumnOrder(3)
                .HasColumnName("evento")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.UsuarioModificado)
                .HasColumnName("usuario_modificado");

            Property(t => t.UsinaGrupo)
                .HasColumnName("usina_grupo");

            Property(t => t.DescricaoGrupo)
                .HasColumnName("descricao_grupo");

            Property(t => t.Complemento)
                .HasColumnName("complemento");
        }
    }
}

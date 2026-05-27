using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.LiberacaoAcesso
{
    public class GrupoAcessoMap : EntityTypeConfiguration<GrupoAcesso>
    {
        public GrupoAcessoMap()
        {
            ToTable("topsys.usr_grupo_liberacao_acesso");

            HasKey(t => t.Codigo);

            HasMany(t => t.LiberacoesAcessos)
               .WithRequired()
               .HasForeignKey(t => t.Grupo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("codigo")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.CriadoEm)
                .HasColumnName("criado_em");

            Property(t => t.AtualizadoEm)
                .HasColumnName("atualizado_em");
        }
    }
}

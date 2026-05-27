using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CadastroGeralViaCaptacaoMap : EntityTypeConfiguration<CadastroGeralViaCaptacao>
    {
        public CadastroGeralViaCaptacaoMap()
        {
            ToTable("topsys.ger_geral_captacao");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.TipoIndicacao)
                .HasColumnName("captacao_tipo_indicador");

        }
    }
}

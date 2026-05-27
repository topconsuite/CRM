using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CadastroGeralMap:EntityTypeConfiguration<CadastroGeral>
    {
        public CadastroGeralMap()
        {
            ToTable("topsys.ger_geral");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.DescricaoReduzida)
                .HasColumnName("descr_reduzida");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

            HasOptional(x => x.ViaCaptacao)
                .WithRequired()
                .WillCascadeOnDelete(false);

        }
    }
}

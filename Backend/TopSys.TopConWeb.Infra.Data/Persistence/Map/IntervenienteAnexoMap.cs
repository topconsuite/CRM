using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class IntervenienteAnexoMap : EntityTypeConfiguration<IntervenienteAnexo>
    {
        public IntervenienteAnexoMap()
        {
            ToTable("topsys.ger_interv_anex");

            HasKey(t => new { t.IntervenienteCodigo });

            Property(t => t.IntervenienteCodigo)
                .HasColumnOrder(0)
                .HasColumnName("interv");

            Property(t => t.Descricao)
                .HasColumnName("descricao");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.DataHora)
                .HasColumnName("data_hora");

            Property(t => t.Arquivo)
                .HasColumnName("arquivo");

            Property(t => t.AnoChamada)
                .HasColumnName("ano_chamada");

            Property(t => t.NumeroChamada)
                .HasColumnName("num_chamada");

            HasOptional(t => t.OportunidadeAnexo)
                .WithMany()
                .HasForeignKey(t => new { t.IntervenienteCodigo, t.Nome, t.DataHora });
        }
    }
}

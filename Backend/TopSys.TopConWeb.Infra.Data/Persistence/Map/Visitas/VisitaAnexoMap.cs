using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class VisitaAnexoMap : EntityTypeConfiguration<VisitaAnexo>
    {

        public VisitaAnexoMap()
        {
            ToTable("topsys.con_visita_anexo");

            HasKey(t => t.Id);
            HasIndex(t => new { t.Usina, t.Ano, t.Numero });

            Property(t => t.Id)
                .HasColumnName("id");

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.Ano)
                .HasColumnName("ano_visita");

            Property(t => t.Numero)
                .HasColumnName("num_visita");

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
            
        }

    }
}

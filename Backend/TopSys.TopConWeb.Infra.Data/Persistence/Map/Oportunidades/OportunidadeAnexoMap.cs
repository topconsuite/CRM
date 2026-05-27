using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades
{
    public class OportunidadeAnexoMap : EntityTypeConfiguration<OportunidadeAnexo>
    {

        public OportunidadeAnexoMap()
        {

            ToTable("topsys.con_oportunidade_anexo");

            HasKey(t => new { t.Interveniente, t.Nome, t.DataHora });
            HasIndex(t => new { t.Usina, t.AnoOportunidade, t.NumeroOportunidade });

            Property(t => t.Usina)
                .HasColumnName("usina");

            Property(t => t.AnoOportunidade)
                .HasColumnName("ano_oportunidade");

            Property(t => t.NumeroOportunidade)
                .HasColumnName("num_oportunidade");

            Property(t => t.Interveniente)
                .HasColumnName("interv");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.DataHora)
                .HasColumnName("data_hora");

        }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class DemaisServicosMap: EntityTypeConfiguration<DemaisServicos>
    {
        public DemaisServicosMap()
        {
            ToTable("topsys.con_demais_servicos");

            HasKey(t => new { t.Codigo });

            Ignore(t => t.FormaDeCobranca);
            Ignore(t => t.FrequenciaDeCobranca);

            HasRequired(t => t.Usina)
                 .WithMany()
                 .HasForeignKey(t => t.UsinaCodigo);

            HasRequired(t => t.Mercadoria)
                 .WithMany()
                 .HasForeignKey(t => t.MercadoriaCodigo);

            HasRequired(t => t.Unidade)
                 .WithMany()
                 .HasForeignKey(t => t.UnidadeSigla);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.UsinaCodigo)
                .HasColumnType("usmallint")
                .HasColumnName("usina");

            Property(t => t.MercadoriaCodigo)
                .HasColumnName("merc");

            Property(t => t.UnidadeSigla)
                .HasColumnName("Unid_cobranca");

            Property(t => t.NumeroDeCasasDecimais)
                .HasColumnName("Casas_decimais");

            Property(t => t.PrecoSugerido)
                .HasColumnName("Preco_Sugerido");

            Property(t => t.PrecoMinimo)
                .HasColumnName("Preco_Minimo");

            Property(t => t.FrequenciaDeCobrancaString)
                .HasColumnName("Frequencia_Cobranca");

            Property(t => t.FormaDeCobrancaString)
                .HasColumnName("Forma_Cobranca");

            Property(t => t.AtualizaEstoque)
                .HasColumnName("atualiza_estoque");

        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraDemaisServicosMap : EntityTypeConfiguration<ObraDemaisServicos>
    {
        public ObraDemaisServicosMap()
        {
            ToTable("topsys.con_obras_dem_serv");

            HasKey(t => new { t.UsinaCodigo, t.ObraNumero, t.Sequencia });

            Ignore(t => t.FormaDeCobranca);
            Ignore(t => t.FrequenciaDeCobranca);

            HasRequired(t => t.UsinaEntrega)
                 .WithMany()
                 .HasForeignKey(t => t.UsinaEntregaCodigo);

            HasRequired(t => t.Mercadoria)
                 .WithMany()
                 .HasForeignKey(t => t.MercadoriaCodigo);

            HasRequired(t => t.Unidade)
                 .WithMany()
                 .HasForeignKey(t => t.UnidadeSigla);

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina");

            Property(t => t.ObraNumero)
                .HasColumnOrder(1)
                .HasColumnName("obra");

            Property(t => t.Sequencia)
                .HasColumnOrder(2)
                .HasColumnName("seq");

            Property(t => t.Codigo)
                .HasColumnName("cod");

            Property(t => t.UsinaEntregaCodigo)
                .HasColumnName("usina_entrega");

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

            Property(t => t.PrecoProposto)
                .HasColumnName("Preco_Proposto");

            Property(t => t.Quantidade)
                .HasColumnName("Quantidade");

        }
    }
}

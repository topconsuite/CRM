using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class BombaPrecoMap : EntityTypeConfiguration<BombaPreco>
    {
        public BombaPrecoMap()
        {
            ToTable("topsys.con_preco_bomba");

            HasKey(t => new { t.UsinaCodigo, t.BombaTipoCodigo, t.DataInicioVigencia });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            Property(t => t.BombaTipoCodigo)
                .HasColumnOrder(1)
                .HasColumnName("tipo_bomba");

            HasRequired(t => t.BombaTipo)
                .WithMany()
                .HasForeignKey(t => t.BombaTipoCodigo);

            Property(t => t.DataInicioVigencia)
                .HasColumnOrder(2)
                .HasColumnName("inicio_validade");

            Property(t => t.M3Ate)
                .HasColumnName("m3_ate");

            Property(t => t.TaxaMinimaPreco)
                .HasColumnName("taxa_minima");

            Property(t => t.M3Preco)
                .HasColumnName("pr_m3_bombedado");

            Property(t => t.M3AteValorMinimo)
                .HasColumnName("m3_atem");

            Property(t => t.TaxaMinimaPrecoPercentualDescontoMaximo)
                .HasColumnName("pct_desc_tx");

            Property(t => t.TaxaMinimaPrecoValorMinimo)
                .HasColumnName("taxa_minima_des");

            Property(t => t.M3PrecoPercentualDescontoMaximo)
                .HasColumnName("pct_desc_m3");

            Property(t => t.M3PrecoValorMinimo)
                .HasColumnName("pr_m3_bombm");

            Property(t => t.TipoCalculo)
                .HasColumnName("tipo_calc");

            Property(t => t.HoraAte)
                .HasColumnName("tempo_min_tab");

            Property(t => t.HoraAteValorMinimo)
                .HasColumnName("tempo_min");

            Property(t => t.HoraPreco)
                .HasColumnName("vlr_hora_tab");

            Property(t => t.HoraPrecoValorMinimo)
                .HasColumnName("vlr_hora_min");

            Property(t => t.HoraTaxaMinimaPreco)
                .HasColumnName("vlr_tx_min_h_tab");

            Property(t => t.HoraTaxaMinimaPrecoValorMinimo)
                .HasColumnName("vlr_tx_hora_min");

            Property(t => t.HoraTipoCalculo)
                .HasColumnName("tipo_calc_hora");
        }
    }
}

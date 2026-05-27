using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class BombaPrecoTerceiroMap : EntityTypeConfiguration<BombaPrecoTerceiro>
    {
        public BombaPrecoTerceiroMap()
        {
            ToTable("topsys.con_preco_bomba_terc");

            HasKey(t => new { t.BombistaCodigo, t.BombaTipoCodigo, t.DataInicioVigencia });

            Ignore(t => t.HoraAte);
            Ignore(t => t.HoraAteValorMinimo);
            Ignore(t => t.HoraPreco);
            Ignore(t => t.HoraPrecoValorMinimo);
            Ignore(t => t.HoraTaxaMinimaPreco);
            Ignore(t => t.HoraTaxaMinimaPrecoValorMinimo);
            Ignore(t => t.HoraTipoCalculo);

            Property(t => t.BombistaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("bombista");

            HasRequired(t => t.Bombista)
                .WithMany()
                .HasForeignKey(t => t.BombistaCodigo);

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
        }
    }
}

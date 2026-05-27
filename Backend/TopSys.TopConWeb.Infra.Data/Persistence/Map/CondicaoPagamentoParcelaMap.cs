using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CondicaoPagamentoParcelaMap : EntityTypeConfiguration<CondicaoPagamentoParcela>
    {
        public CondicaoPagamentoParcelaMap()
        {
            ToTable("topsys.ger_cond_pag_parc");

            HasKey(t => new { t.CondicaoPagamentoCodigo, t.Dias });

            Property(t => t.CondicaoPagamentoCodigo)
                .HasColumnOrder(0)
                .HasColumnName("cond_pag");

            HasRequired(t => t.CondicaoPagamento)
                .WithMany()
                .HasForeignKey(t => t.CondicaoPagamentoCodigo);

            Property(t => t.Dias)
                .HasColumnOrder(1)
                .HasColumnName("dias_parc");

            Property(t => t.Percentual)
                .HasColumnName("pct_parc");

        }
    }
}

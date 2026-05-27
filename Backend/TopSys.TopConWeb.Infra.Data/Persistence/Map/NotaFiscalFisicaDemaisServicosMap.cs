using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalFisicaDemaisServicosMap : EntityTypeConfiguration<NotaFiscalFisicaDemaisServicos>
    {
        public NotaFiscalFisicaDemaisServicosMap()
        {
            ToTable("topsys.con_nf_servicos");

            HasKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia, t.SequenciaServico });

            Ignore(t => t.MercadoriaDescricao);

            Property(t => t.FilialCodigo)
                .HasColumnName("filial");

            Property(t => t.IntervenienteCodigo)
                .HasColumnName("interv");

            Property(t => t.TipoDocumentoCodigo)
                .HasColumnName("tp_doc");

            Property(t => t.Numero)
                .HasColumnName("num_nf");

            Property(t => t.Serie)
                .HasColumnName("serie");

            Property(t => t.Sequencia)
                .HasColumnName("seq_nf");

            Property(t => t.SequenciaServico)
                .HasColumnName("seq_serv_obra");

            Property(t => t.MercadoriaCodigo)
                .HasColumnName("merc");

            Property(t => t.Quantidade)
                .HasColumnName("quantidade");

            Property(t => t.ValorUnitario)
                .HasColumnName("valor_unitario");

            Property(t => t.ValorTotal)
                .HasColumnName("valor_total");

            Property(t => t.ValorCobrado)
                .HasColumnName("valor_cobrado");
            
            HasOptional(t => t.Mercadoria)
                .WithMany()
                .HasForeignKey(t => t.MercadoriaCodigo);
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheCartaoMap : EntityTypeConfiguration<PropostaPagamentoDetalheCartao>
    {
        public PropostaPagamentoDetalheCartaoMap()
        {
            //ToTable("view_con_chtel_ccredit");

            HasKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(0)
               .HasColumnName("usina");

            Property(t => t.PropostaAno)
               .HasColumnOrder(1)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnOrder(2)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnOrder(3)
               .HasColumnName("no_obra");

            Property(t => t.PagamentoSequencia)
               .HasColumnOrder(4)
               .HasColumnName("seq_pgto");

            Property(t => t.DetalheSequencia)
               .HasColumnOrder(5)
               .HasColumnName("seq");

            Property(t => t.DataTransacao)
               .HasColumnName("dt_transacao");

            Property(t => t.BandeiraCodigo)
               .HasColumnName("bandeira");

            HasRequired(t => t.Bandeira)
                .WithMany()
                .HasForeignKey(t => t.BandeiraCodigo);

            Property(t => t.NumeroCartao)
               .HasColumnName("no_cartao");

            Property(t => t.QuantidadeParcelas)
               .HasColumnName("no_parcelas");

            Property(t => t.NumeroAutorizacao)
               .HasColumnName("no_autorizacao");

            Property(t => t.Valor)
               .HasColumnName("valor");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.ContratoAno)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnName("num_contrato");
        }
    }
}

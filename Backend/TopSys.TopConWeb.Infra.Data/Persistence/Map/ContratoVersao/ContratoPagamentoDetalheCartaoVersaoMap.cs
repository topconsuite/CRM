using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheCartaoVersaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheCartaoVersao>
    {
        public ContratoPagamentoDetalheCartaoVersaoMap()
        {
            //ToTable("con_contrato_ccredit_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.NumeroVersao)
               .HasColumnOrder(0)
               .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(1)
               .HasColumnName("usina");

            Property(t => t.ContratoAno)
               .HasColumnOrder(2)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnOrder(3)
               .HasColumnName("num_contrato");

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

            Property(t => t.PropostaAno)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnName("no_obra");
        }
    }
}

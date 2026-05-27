using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoMap : EntityTypeConfiguration<PropostaPagamento>
    {
        public PropostaPagamentoMap()
        {
            ToTable("topsys.con_chtel_pag");

            HasKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.Sequencia });

            Ignore(t => t.Ativo);
            Ignore(t => t.NecessitaAprovacao);
            Ignore(t => t.ValorFixo);
            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(0)
               .HasColumnName("usina");

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            Property(t => t.PropostaAno)
               .HasColumnOrder(1)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnOrder(2)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnOrder(3)
               .HasColumnName("no_obra");

            Property(t => t.Sequencia)
               .HasColumnOrder(4)
               .HasColumnName("seq");

            Property(t => t.CondicaoPagamentoCodigo)
               .HasColumnName("cond_pgto");

            HasRequired(t => t.CondicaoPagamento)
                .WithMany()
                .HasForeignKey(t => t.CondicaoPagamentoCodigo);

            Property(t => t.TipoCobrancaCodigo)
               .HasColumnName("tp_cobranca");

            HasRequired(t => t.TipoCobranca)
                .WithMany()
                .HasForeignKey(t => t.TipoCobrancaCodigo);

            Property(t => t.Forma)
               .HasColumnName("forma");

            Property(t => t.ValorFixoSimNao)
               .HasColumnName("valor_fixo");

            Property(t => t.Valor)
               .HasColumnName("valor");

            Property(t => t.Percentual)
               .HasColumnName("pct");

            Property(t => t.NecessitaAprovacaoSimNao)
               .HasColumnName("necessita_aprov");

            Property(t => t.AtivoSimNao)
               .HasColumnName("ativo");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.IdAprovacao)
               .HasColumnName("id_aprovacao");

            HasMany(t => t.Detalhes)
                .WithRequired()
                .HasForeignKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia });
        }
    }
}

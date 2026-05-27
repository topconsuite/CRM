using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheCartaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheCartao>
    {
        public ContratoPagamentoDetalheCartaoMap()
        {
            //ToTable("con_contrato_dep");

            HasKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(0)
               .HasColumnName("usina");

            Property(t => t.ContratoAno)
               .HasColumnOrder(1)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnOrder(2)
               .HasColumnName("num_contrato");

            Property(t => t.PagamentoSequencia)
               .HasColumnOrder(3)
               .HasColumnName("seq_pgto");

            Property(t => t.DetalheSequencia)
               .HasColumnOrder(4)
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

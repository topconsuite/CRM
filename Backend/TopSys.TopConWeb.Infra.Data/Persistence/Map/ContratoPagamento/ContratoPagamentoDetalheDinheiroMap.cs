using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheDinheiroMap : EntityTypeConfiguration<ContratoPagamentoDetalheDinheiro>
    {
        public ContratoPagamentoDetalheDinheiroMap()
        {
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

            Property(t => t.NumeroRecibo)
               .HasColumnName("num_recibo");

            Property(t => t.DataPagamento)
               .HasColumnName("dt_pagamento");

            Property(t => t.Valor)
               .HasColumnName("valor");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.PropostaAno)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnName("no_obra");

        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    class ContratoPagamentoDetalheDinheiroVersaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheDinheiroVersao>
    {
        public ContratoPagamentoDetalheDinheiroVersaoMap()
        {
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

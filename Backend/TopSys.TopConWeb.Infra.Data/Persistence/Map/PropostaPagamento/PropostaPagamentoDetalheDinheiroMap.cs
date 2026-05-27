using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheDinheiroMap : EntityTypeConfiguration<PropostaPagamentoDetalheDinheiro>
    {
        public PropostaPagamentoDetalheDinheiroMap()
        {
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

            Property(t => t.ContratoAno)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnName("num_contrato");

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
        }
    }
}

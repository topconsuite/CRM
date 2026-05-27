using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheDinheiroVersaoMap : EntityTypeConfiguration<PropostaPagamentoDetalheDinheiroVersao>
    {
        public PropostaPagamentoDetalheDinheiroVersaoMap()
        {
            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.NumeroVersao)
               .HasColumnOrder(0)
               .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(1)
               .HasColumnName("usina");

            Property(t => t.PropostaAno)
               .HasColumnOrder(2)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnOrder(3)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnOrder(4)
               .HasColumnName("no_obra");

            Property(t => t.PagamentoSequencia)
               .HasColumnOrder(5)
               .HasColumnName("seq_pgto");

            Property(t => t.DetalheSequencia)
               .HasColumnOrder(6)
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

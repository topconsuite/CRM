using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoForSavingVersaoMap : EntityTypeConfiguration<ContratoPagamentoForSavingVersao>
    {
        public ContratoPagamentoForSavingVersaoMap()
        {
            ToTable("topsys.con_contrato_pag_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.Sequencia });

            Ignore(t => t.ObraCodigo);

            Ignore(t => t.Ativo);
            Ignore(t => t.NecessitaAprovacao);
            Ignore(t => t.ValorFixo);
            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);

            Property(t => t.NumeroVersao)
               .HasColumnOrder(0)
               .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(1)
               .HasColumnName("usina");

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            Property(t => t.ContratoAno)
               .HasColumnOrder(2)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnOrder(3)
               .HasColumnName("num_contrato");

            Property(t => t.Sequencia)
               .HasColumnOrder(4)
               .HasColumnName("seq");

            Property(t => t.CondicaoPagamentoCodigo)
               .HasColumnName("cond_pagto");

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

            Property(t => t.ValorApropriado)
               .HasColumnName("valor_apropriado");

            HasMany(t => t.Detalhes)
                .WithRequired()
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia });
        }
    }
}
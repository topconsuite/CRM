using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    class AprovacaoComercialHierarquiaCondicaoPagamentoMap : EntityTypeConfiguration<AprovacaoComercialHierarquiaCondicaoPagamento>
    {

        public AprovacaoComercialHierarquiaCondicaoPagamentoMap()
        {

            ToTable("topsys.con_aprovacao_comercial_hierarquia_condicao_pagamento");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            HasIndex(x => new { x.AprovacaoComercialHierarquiaId, x.TipoPessoaId, x.SegmentacaoId });

            Property(x => x.TipoPessoaId)
                .HasColumnOrder(1)
                .HasColumnName("tipo_pessoa_id");

            Property(x => x.AprovacaoComercialHierarquiaId)
                .HasColumnOrder(2)
                .HasColumnName("aprovacao_comercial_hierarquia_id");

            Property(x => x.SegmentacaoId)
                .HasColumnOrder(3)
                .HasColumnName("segmentacao_id");

            Property(x => x.MediaDiasDe)
                .HasColumnOrder(4)
                .HasColumnName("media_dias_de");

            Property(x => x.MediaDiasAte)
                .HasColumnOrder(5)
                .HasColumnName("media_dias_ate");

            HasRequired(x => x.TipoPessoa)
                .WithMany()
                .HasForeignKey(x => x.TipoPessoaId);

            HasRequired(x => x.AprovacaoComercialHierarquia)
                .WithMany()
                .HasForeignKey(x => x.AprovacaoComercialHierarquiaId);

            HasRequired(x => x.Segmentacao)
                .WithMany()
                .HasForeignKey(x => x.SegmentacaoId);

        }
    }
}

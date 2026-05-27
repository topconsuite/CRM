using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialPendenteCondicaoPagamentoMap : EntityTypeConfiguration<AprovacaoComercialPendenteCondicaoPagamento>
    {

        public AprovacaoComercialPendenteCondicaoPagamentoMap()
        {

            ToTable("con_aprovacao_comercial_pendente_cond_pagto");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("Id");

            Property(x => x.IdAprovacao)
                .HasColumnName("id_aprovacao");

            Property(x => x.ObraVersao)
                .HasColumnName("obra_versao")
                .IsRequired();

            Property(x => x.ObraUsina)
                .HasColumnName("obra_usina")
                .IsRequired();

            Property(x => x.ObraNumero)
                .HasColumnName("obra_numero")
                .IsRequired();

            Property(x => x.NivelHierarquia)
                .HasColumnName("nivel_hierarquia")
                .IsRequired();

            Property(x => x.AprovacaoStatus)
                .HasColumnName("aprovacao_status")
                .IsRequired();

            Property(x => x.AprovacaoData)
                .HasColumnName("aprovacao_data");

            Property(x => x.AprovacaoUsuario)
                .HasColumnName("aprovacao_usuario");

            Property(x => x.AprovacaoSequencia)
                .HasColumnName("aprovacao_sequencia");

        }
    }
}

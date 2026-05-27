using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CondicaoPagamentoMap : EntityTypeConfiguration<CondicaoPagamento>
    {
        public CondicaoPagamentoMap()
        {
            ToTable("topsys.ger_cond_pag");

            Ignore(t => t.CondicaoDaCobranca);

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.QuantidadeParcelas)
                .HasColumnName("qtde_parcelas");

            HasMany(t => t.Parcelas)
                .WithRequired(t => t.CondicaoPagamento)
                .HasForeignKey(t => t.CondicaoPagamentoCodigo);

            Property(t => t.Vencimento1Parcela)
                .HasColumnName("dias_1a_parc");

            Property(t => t.Vencimento2Parcela)
                .HasColumnName("dias_2a_parc");

            Property(t => t.Vencimento3Parcela)
                .HasColumnName("dias_3a_parc");

            Property(t => t.Vencimento4Parcela)
                .HasColumnName("dias_4a_parc");

            Property(t => t.Vencimento5Parcela)
                .HasColumnName("dias_5a_parc");

            Property(t => t.Vencimento6Parcela)
                .HasColumnName("dias_6a_parc");

            Property(t => t.Vencimento7Parcela)
                .HasColumnName("dias_7a_parc");

            Property(t => t.Vencimento8Parcela)
                .HasColumnName("dias_8a_parc");

            Property(t => t.Vencimento9Parcela)
                .HasColumnName("dias_9a_parc");

            Property(t => t.Vencimento10Parcela)
                .HasColumnName("dias_10a_parc");

            Property(t => t.Vencimento11Parcela)
                .HasColumnName("dias_11a_parc");

            Property(t => t.Vencimento12Parcela)
                .HasColumnName("dias_12a_parc");

            Property(t => t.AnalisaFraude)
                .HasColumnName("analise_fraude");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.CondicaoDaCobrancaCod)
                .HasColumnName("base_nf");

            Property(t => t.DiaUltimoVencimento)
                .HasColumnName("dia_util_vcto");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.VencimentoFixoSemana)
                .HasColumnName("dia_decend_fixo");

            Property(t => t.DiaVencimentoFixoSemana)
                .HasColumnName("dia_semana_fixo");

            Property(t => t.DiaUltimoVencimento)
                .HasColumnName("dia_util_vcto");

            Property(t => t.MesFixo30Dias)
                .HasColumnName("mes_30_dias_fix");

            Property(t => t.RetencaoPrimeiraParcela)
                .HasColumnName("iss_ret_1a_parc");

            Property(t => t.TiposDeCobrancaCodigos)
                .HasColumnName("tipos_de_cobran");

            Property(t => t.IdExterno)
                .HasColumnName("cod_integracao");

            Property(t => t.DescricaoCompleta)
                .HasColumnName("descrc");

            Property(t => t.MediaDias)
                .HasColumnName("media_dias");
        }
    }
}

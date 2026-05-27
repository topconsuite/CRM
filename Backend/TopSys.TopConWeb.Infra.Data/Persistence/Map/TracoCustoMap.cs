using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TracoCustoMap : EntityTypeConfiguration<TracoCusto>
    {
        public TracoCustoMap()
        {
            ToTable("topsys.con_custo_concreto");

            HasKey(t => new { t.UsinaCodigo, t.TracoEspecificacao, t.DataInicioVigencia,
                t.UsoCodigo, t.ResistenciaTipoCodigo, t.Mpa, t.Consumo, t.PedraCodigo, t.SlumpCodigo
            });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.TracoEspecificacao)
                .HasColumnOrder(1)
                .HasColumnName("sigla_compos");

            Property(t => t.DataInicioVigencia)
                .HasColumnOrder(2)
                .HasColumnName("dt_inic_valid");

            Property(t => t.UsoCodigo)
                .HasColumnOrder(3)
                .HasColumnName("uso");

            Property(t => t.ResistenciaTipoCodigo)
                .HasColumnOrder(4)
                .HasColumnName("tp_resist");

            Property(t => t.Mpa)
                .HasColumnOrder(5)
                .HasColumnName("fck");

            Property(t => t.Consumo)
                .HasColumnOrder(6)
                .HasColumnName("consumo");

            Property(t => t.PedraCodigo)
                .HasColumnOrder(7)
                .HasColumnName("pedra");

            Property(t => t.SlumpCodigo)
                .HasColumnOrder(8)
                .HasColumnName("slump");

            Property(t => t.CustoPuro)
                .HasColumnName("custo_p");

            Property(t => t.CustoAjustado)
                .HasColumnName("custo_a");

            Property(t => t.DataCalculoCusto)
                .HasColumnName("dt_calc_custo");

            Property(t => t.CustoRecalculado)
                .HasColumnName("custo_recal");

            Property(t => t.DataRecalculoCusto)
                .HasColumnName("dt_recal");

            Property(t => t.PercentalVariacao)
                .HasColumnName("pct_variacao");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.ValorServico)
                .HasColumnName("vlr_servico");

            Property(t => t.ValorOutrosCustos)
                .HasColumnName("vlr_custo_outro");

            Property(t => t.ValorMarkup)
                .HasColumnName("markup");
        }
    }
}

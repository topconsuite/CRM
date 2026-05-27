using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TracoPrecoMap : EntityTypeConfiguration<TracoPreco>
    {
        public TracoPrecoMap()
        {
            ToTable("topsys.con_tab_preco");

            HasKey(t => new { t.NumeroTabela, t.UsinaBaseCodigo, t.VendedorRepresentanteCodigo, t.UsoCodigo,
                                t.ResistenciaTipoCodigo, t.Mpa, t.Consumo, t.PedraCodigo, t.SlumpCodigo, t.DataInicioVigencia });

            Property(t => t.NumeroTabela)
                .HasColumnOrder(0)
                .HasColumnName("num_tab");

            Property(t => t.UsinaBaseCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina_base");

            Property(t => t.VendedorRepresentanteCodigo)
                .HasColumnOrder(2)
                .HasColumnName("vend_repres");

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

            Property(t => t.DataInicioVigencia)
                .HasColumnOrder(9)
                .HasColumnName("dt_inic_tab");

            Property(t => t.DataFinalVigencia)
                .HasColumnName("dt_fim_tab");

            Property(t => t.M3Preco)
                .HasColumnName("preco_m3");

            Property(t => t.M3PrecoRecalculo)
                .HasColumnName("pr_m3_recal");

            Property(t => t.PercentualVariacao)
                .HasColumnName("pct_variacao");

            Property(t => t.DataRecalculo)
                .HasColumnName("dt_recal");

            Property(t => t.CustoMaterial)
                .HasColumnName("custo_servico");

            Property(t => t.Markup)
                .HasColumnName("markup");

            Property(t => t.TracoEspecificacao)
                .HasColumnName("espec_familia");

            Property(t => t.ComissaoPercentualServico)
                .HasColumnName("comis_pct_servico");

            Property(t => t.ComissaoPercentualSobrePreco)
                .HasColumnName("comis_pct_spreco");

            Property(t => t.ComissaoServicoSobrePreco)
                .HasColumnName("comis_serv_spreco");

            Property(t => t.ComissaoSobreMaior)
                .HasColumnName("comis_smaior");

            Property(t => t.UsinaReferenciaCodigo)
                .HasColumnName("usina_ref");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.NumeracaoProduto)
                .HasColumnName("numeracao_produto");

            HasRequired(t => t.UsinaBase)
                .WithMany()
                .HasForeignKey(t => t.UsinaBaseCodigo);

            HasRequired(t => t.VendedorRepresentante)
                .WithMany()
                .HasForeignKey(t => t.VendedorRepresentanteCodigo);

            HasRequired(t => t.ResistenciaTipo)
               .WithMany()
               .HasForeignKey(t => t.ResistenciaTipoCodigo);

            HasRequired(t => t.Uso)
                .WithMany()
                .HasForeignKey(t => t.UsoCodigo);

            HasRequired(t => t.Pedra)
               .WithMany()
               .HasForeignKey(t => t.PedraCodigo);

            HasRequired(t => t.Slump)
               .WithMany()
               .HasForeignKey(t => t.SlumpCodigo);

            HasOptional(t => t.UsinaReferencia)
               .WithMany()
               .HasForeignKey(t => t.UsinaReferenciaCodigo);


        }
    }
}

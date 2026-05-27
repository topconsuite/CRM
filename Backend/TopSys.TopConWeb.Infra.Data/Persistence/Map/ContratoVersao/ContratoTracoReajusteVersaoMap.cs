using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoTracoReajusteVersaoMap : EntityTypeConfiguration<ContratoTracoReajusteVersao>
    {
        public ContratoTracoReajusteVersaoMap()
        {
            ToTable("topsys.con_reajuste_item_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.DataVigencia, t.ObraTracoSequencia });

            HasRequired(t => t.Contrato)
               .WithMany()
               .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero })
               .WillCascadeOnDelete(false);

            Ignore(t => t.Obra);

            HasRequired(t => t.Usina)
              .WithMany()
              .HasForeignKey(t => t.UsinaCodigo);

            HasOptional(t => t.Uso)
                .WithMany()
                .HasForeignKey(t => t.UsoCodigo);

            HasOptional(t => t.Pedra)
               .WithMany()
               .HasForeignKey(t => t.PedraCodigo);

            HasOptional(t => t.Slump)
               .WithMany()
               .HasForeignKey(t => t.SlumpCodigo);

            HasOptional(t => t.ResistenciaTipo)
               .WithMany()
               .HasForeignKey(t => t.ResistenciaTipoCodigo);

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

            Property(t => t.DataVigencia)
                .HasColumnOrder(4)
                .HasColumnName("dt_vigencia");

            Property(t => t.ObraTracoSequencia)
                .HasColumnOrder(5)
                .HasColumnName("item_contrato");

            Property(t => t.ResistenciaTipoCodigo)
                .HasColumnName("tp_resist");

            Property(t => t.Mpa)
                .HasColumnName("fck");

            Property(t => t.Consumo)
                .HasColumnName("consumo");

            Property(t => t.UsoCodigo)
                .HasColumnName("uso");

            Property(t => t.PedraCodigo)
                .HasColumnName("pedra");

            Property(t => t.SlumpCodigo)
                .HasColumnName("slump");

            Property(t => t.PrecoVigente)
                .HasColumnName("preco_vigente");

            Property(t => t.CustoVigente)
                .HasColumnName("custo_vigente");

            Property(t => t.ValorServicoVigente)
                .HasColumnName("servico_vigente");

            Property(t => t.PrecoRecalculado)
                .HasColumnName("preco_recalc");

            Property(t => t.CustoRecalculado)
                .HasColumnName("custo_recalc");

            Property(t => t.ValorServicoRecalculado)
                .HasColumnName("servico_recalc");

            Property(t => t.PorcentagemReajuste)
                .HasColumnName("pct_reajuste");

            Property(t => t.EmiteCartaSimNao)
                .HasColumnName("emite_carta");

            Property(t => t.UsinaEntregaCodigo)
                .HasColumnName("usina_principal");

            Property(t => t.DataCarta)
                .HasColumnName("data_carta");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.DataConfirmacao)
                .HasColumnName("dt_confirmacao");

            Property(t => t.DataCalculo)
                .HasColumnName("dt_calculo");

            Property(t => t.NumeroTabela)
                .HasColumnName("tab_custo");

            Property(t => t.IdAprovacaoVersao)
                .HasColumnName("id_aprov_versao");

            Property(t => t.IdReprovacao)
                .HasColumnName("id_reprovacao");
        }
    }
}

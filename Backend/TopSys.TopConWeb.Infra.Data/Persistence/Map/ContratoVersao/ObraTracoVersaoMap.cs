using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraTracoVersaoMap : EntityTypeConfiguration<ObraTracoVersao>
    {
        public ObraTracoVersaoMap()
        {
            ToTable("topsys.con_proposta_item_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo, t.Sequencia });

            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);
            Ignore(t => t.DescricaoPersonalizada);
            Ignore(t => t.Inativo);

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasRequired(t => t.Uso)
                .WithMany()
                .HasForeignKey(t => t.UsoCodigo);

            HasRequired(t => t.Pedra)
               .WithMany()
               .HasForeignKey(t => t.PedraCodigo);

            HasRequired(t => t.Slump)
               .WithMany()
               .HasForeignKey(t => t.SlumpCodigo);

            HasRequired(t => t.SlumpNominal)
               .WithMany()
               .HasForeignKey(t => t.SlumpNominalCodigo);

            HasRequired(t => t.ResistenciaTipo)
               .WithMany()
               .HasForeignKey(t => t.ResistenciaTipoCodigo);

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(2)
                .HasColumnName("no_obra");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq");

            Property(t => t.Fck)
                .HasColumnName("fck");

            Property(t => t.Consumo)
                .HasColumnName("consumo");

            Property(t => t.UsoCodigo)
                .HasColumnName("uso");

            Property(t => t.PedraCodigo)
                .HasColumnName("pedra");

            Property(t => t.SlumpCodigo)
                .HasColumnName("slump");

            Property(t => t.SlumpNominalCodigo)
                .HasColumnName("slump_nominal");

            Property(t => t.ResistenciaTipoCodigo)
                .HasColumnName("tp_resist");

            Property(t => t.M3Quantidade)
                .HasColumnName("qtde_m3");

            Property(t => t.M3PrecoTabela)
                .HasColumnName("preco_unit_tab");

            Property(t => t.M3PrecoProposto)
                .HasColumnName("preco_unit_prop");

            Property(t => t.M3PrecoAjustado)
                .HasColumnName("preco_ajustado");

            Property(t => t.DescontoPercentual)
                .HasColumnName("pct_descto");

            Property(t => t.PecaConcretar)
                .HasColumnName("peca_concretar");

            Property(t => t.PrecoConcorrencia)
                .HasColumnName("preco_concorren");

            Property(t => t.ValorServico)
                .HasColumnName("custo_servico");

            Property(t => t.DescontoSolicitante)
                .HasColumnName("id_aprov_descto");

            Property(t => t.AprovacaoTipo)
                .HasColumnName("tipo_aprov");

            Property(t => t.AprovacaoVerbal)
                .HasColumnName("aprov_verbal");

            Property(t => t.AprovacaoObservacao)
                .HasColumnName("obs_aprov");

            Property(t => t.AprovacaoOperacao)
                .HasColumnName("status_aprov");

            Property(t => t.AprovacaoCiente)
                .HasColumnName("ciente_aprov");

            Property(t => t.Justificativa)
                .HasColumnName("just_aprov");

            Property(t => t.PropostaAno)
                .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
                .HasColumnName("no_chamada");

            Property(t => t.PrecoReajustadoAnterior)
                .HasColumnName("pr_reajust_ant");

            Property(t => t.DataUltimoReajuste)
                .HasColumnName("dt_ult_reajuste");

            Property(t => t.PrecoReajustadoAtual)
                .HasColumnName("pr_reajustado_a");

            Property(t => t.Observacao)
                .HasColumnName("obs_traco");

            Property(t => t.IdAlteracaoTracoPesado)
                .HasColumnName("id_alt_traco_p");

            Property(t => t.ValorRessarcido)
                .HasColumnName("vl_ressarc");

            Property(t => t.M3QuantidadeBombeada)
                .HasColumnName("qtde_m3_bombeado");

            Property(t => t.MargemPosTransporte)
                .HasColumnName("margem_pos_transporte");

            Property(t => t.Ebitda)
                .HasColumnName("ebitda");

            Property(t => t.IssDedutivel)
                .HasColumnName("iss_aplicado");

            Property(t => t.ImpostoAplicadoEstadual)
                .HasColumnName("imposto_aplicado_estadual");

            Property(t => t.ImpostoAplicadoFederal)
                .HasColumnName("imposto_aplicado_federal");

            Property(t => t.CustoBombagem)
                .HasColumnName("custo_bombagem");

            Property(t => t.CustoServicoReajustado)
                .HasColumnName("custo_serv_a");

            Property(t => t.CustoProjetadoTransporte)
                .HasColumnName("custo_transporte");

            Property(t => t.CustoServicoAnterior)
                .HasColumnName("custo_serv_ant");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.NumeracaoProduto)
                .HasColumnName("numeracao_produto");

            Property(t => t.NumeracaoFamilia)
                .HasColumnName("numeracao_familia");
        }
    }
}
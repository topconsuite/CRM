using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoVersaoMap : EntityTypeConfiguration<ContratoVersao>
    {
        public ContratoVersaoMap()
        {
            ToTable("topsys.con_contrato_versao");

            HasKey(t => new { t.NumeroVersao, t.Usina, t.Ano, t.Numero });

            Ignore(t => t.Fechado);

            HasOptional(t => t.Interveniente)
                .WithMany()
                .HasForeignKey(t => t.IntervenienteCodigo);

            HasOptional(t => t.Vendedor)
                .WithMany()
                .HasForeignKey(t => t.VendedorCodigo);

            HasMany(t => t.ContratoTracoReajustes)
                .WithRequired(t => t.Contrato)
                .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero });

            HasMany(t => t.ContratoBombaReajustes)
               .WithRequired(t => t.Contrato)
               .HasForeignKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.Usina)
                .HasColumnOrder(1)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Ano)
                .HasColumnOrder(2)
                .HasColumnName("ano_contrato");

            Property(t => t.Numero)
                .HasColumnOrder(3)
                .HasColumnType("umediumint")
                .HasColumnName("num_contrato")
                .HasDatabaseGeneratedOption((DatabaseGeneratedOption.None));

            Property(t => t.IntervenienteCodigo)
                .HasColumnType("umediumint")
                .HasColumnName("interv");

            Property(t => t.VendedorCodigo)
                .HasColumnName("vendedor");

            Property(t => t.Status)
                .HasColumnName("status");

            Property(t => t.StatusAnterior)
                .HasColumnName("status_anterior");

            Property(t => t.FaturamentoAc)
                .HasColumnName("faturamento_ac");

            Property(t => t.FechadoSimNao)
                .HasColumnName("fechado");

            Property(t => t.DataEncerramento)
                .HasColumnName("dt_encer_cont");

            Property(t => t.ValorConcreto)
                .HasColumnName("vlr_concreto");

            Property(t => t.ValorBomba)
                .HasColumnName("vlr_bomba");

            Property(t => t.ValorExtras)
                .HasColumnName("vlr_extras");

            Property(t => t.ValorTotalContrato)
                .HasColumnName("vlr_total_ctr");

            Property(t => t.VolumeTotal)
                .HasColumnName("total_m3");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.CodigoObraPrefeitura)
                .HasColumnName("no_obra");

            Property(t => t.LocalFaturamento)
                .HasColumnName("local_fatur");

            Property(t => t.LocalCobranca)
                .HasColumnName("local_cobranca");

            Property(t => t.ResponsavelSolidario)
                .HasColumnName("resp_solidario");

            Property(t => t.IdAprovacaoEngenharia)
                .HasColumnName("id_aprov_eng");

            Property(t => t.DescricaoCoincidencia)
                .HasColumnName("descr_coincid");

            Property(t => t.AguardandoAprovacao)
                .HasColumnName("aguard_aprov");

            Property(t => t.IdAprovacaoCoincidencia)
                .HasColumnName("aprov_coincid");

            Property(t => t.AnalistaCodigo)
                .HasColumnName("analista");

            Property(t => t.AprovaEngenharia)
                .HasColumnName("aprov_eng");

            Property(t => t.CadastroAprovado)
                .HasColumnName("cad_aprovado");

            Property(t => t.IdAprovacaoCadastro)
                .HasColumnName("id_aprov_cad");

            Property(t => t.IdAprovacaoVendedor)
                .HasColumnName("id_aprov_vend");

            Property(t => t.UsinaEntrega)
                .HasColumnName("usina_principal");

            Property(t => t.FaturamentoPendente)
                .HasColumnName("fat_pendente");

            Property(t => t.ModeloDocumentoRemessaConcreto)
                .HasColumnName("modelo_doc_remessa_concreto");

            Property(t => t.ModeloDocumentoRemessaBomba)
                .HasColumnName("modelo_doc_remessa_bomba");

            Property(t => t.PercentualRetencaoContratual)
                .HasColumnName("retencao_contratual");

            Property(t => t.MaoObraPropria)
                .HasColumnName("mao_obra_propria");

            Property(t => t.PercentualLocacao)
                .HasColumnName("percentual_locacao");

            Property(t => t.Inconsistencias)
                .HasColumnName("inconsistencias");

            Property(t => t.ModeloItensDanfeERomaneio)
                .HasColumnName("modelo_danfe");

            Property(t => t.NumeroContratoAnterior)
                .HasColumnName("no_ctr_ant");

            Property(t => t.DataContrato)
                .HasColumnName("dt_contrato");

            Property(t => t.InicioVigencia)
                .HasColumnName("inicio_vigencia");

            Property(t => t.FimVigencia)
                .HasColumnName("fim_vigencia");

            Property(t => t.DataVersaoCriada)
               .HasColumnName("dt_versao_criada");

            Property(t => t.ContratoFinalidade)
                .HasColumnName("finalidade_ctr");

            Property(t => t.AprovacaoMedicao)
                .HasColumnName("aprov_medicao");

            Property(t => t.TempoAprovacaoMedicao)
                .HasColumnName("tempo_aprov_medicao");
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalFisicaItemMap : EntityTypeConfiguration<NotaFiscalFisicaItem>
    {
        public NotaFiscalFisicaItemMap()
        {
            ToTable("topsys.con_item_nf");

            HasKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia, t.SequenciaItem });

            Property(t => t.Cfop)
                .HasColumnName("cfop");

            Property(t => t.CustoTotal)
                .HasColumnName("custo_tot_item");

            Property(t => t.DataHoraUltimaAtualizacao)
                .HasColumnName("dt_hr_ult_atual");

            Property(t => t.DataOperacao)
                .HasColumnName("dt_op");

            Property(t => t.FilialCodigo)
                .HasColumnName("filial");

            Property(t => t.IdAtual)
                .HasColumnName("id_atual");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IntervenienteEstoque)
                .HasColumnName("interv_estq");

            Property(t => t.IntervenienteCodigo)
                .HasColumnName("interv");

            Property(t => t.LocalEstoque)
                .HasColumnName("local_estoque");

            Property(t => t.LocalInsumo)
                .HasColumnName("local_insumo");

            Property(t => t.MercadoriaCodigo)
                .HasColumnName("merc");

            Property(t => t.Numero)
                .HasColumnName("num_nf");

            Property(t => t.SequenciaItem)
                .HasColumnName("num_seq_item_nf");

            Property(t => t.PercentualAjuste)
                .HasColumnName("percentual_ajuste");

            Property(t => t.Peso)
                .HasColumnName("peso");

            Property(t => t.PrecoUnitario)
                .HasColumnName("preco_un");

            Property(t => t.Quantidade)
                .HasColumnName("qt");

            Property(t => t.QuantidadeComissao)
                .HasColumnName("qtde_comis");

            Property(t => t.QuantidadeEstoque)
                .HasColumnName("qtd_estoque");

            Property(t => t.QuantidadeTeorica)
                .HasColumnName("qt_teorica");

            Property(t => t.SequenciaCfop)
                .HasColumnName("seq_cfop");

            Property(t => t.Sequencia)
                .HasColumnName("seq_nf");

            Property(t => t.Serie)
                .HasColumnName("ser");

            Property(t => t.TipoDocumentoCodigo)
                .HasColumnName("tp_doc");

            Property(t => t.TipoEstoque)
                .HasColumnName("tp_estq");

            Property(t => t.TracoConcreto)
                .HasColumnName("traco_concreto");

            Property(t => t.Transacao)
                .HasColumnName("trans");

            Property(t => t.Umidade)
                .HasColumnName("umidade");

            Property(t => t.ValorDesconto)
                .HasColumnName("vl_desc");

            Property(t => t.ValorFrete)
                .HasColumnName("vl_frete");

            Property(t => t.ValorOutrasDespesas)
                .HasColumnName("vl_o_desp");

            Property(t => t.ValorSeguro)
                .HasColumnName("vl_seg");

            Property(t => t.ValorTotal)
                .HasColumnName("vl_tot");

            Property(t => t.Volume)
                .HasColumnName("volume");
            
            HasOptional(t => t.Mercadoria)
                .WithMany()
                .HasForeignKey(t => t.MercadoriaCodigo);

        }
    }
}

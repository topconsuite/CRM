using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ProgramacaoHoraMap : EntityTypeConfiguration<ProgramacaoHora>
    {
        public ProgramacaoHoraMap()
        {
            ToTable("topsys.con_programacao_hora");

            HasKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.Sequencia, t.Horario });

            HasOptional(t => t.Nf)
                .WithMany()
                .HasForeignKey(t => new { t.NfFilialCodigo, t.NfIntervenienteCodigo, t.NfTipoDocumentoCodigo, t.NfNumero, t.NfSerie, t.NfSequencia });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnOrder(1)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnOrder(2)
                .HasColumnName("no_contrato");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq_prog");

            Property(t => t.Horario)
                .HasColumnOrder(4)
                .HasColumnName("horario");

            Property(t => t.VolumeProgramado)
                .HasColumnName("volume_prog");

            Property(t => t.VolumeEntregue)
                .HasColumnName("volume_entregue");

            Property(t => t.CorpoDeProvaQuantidade)
                .HasColumnName("qtde_cp");

            Property(t => t.Status)
                .HasColumnName("status");

            Property(t => t.NfFilialCodigo)
                .HasColumnName("filial");

            Property(t => t.NfIntervenienteCodigo)
                .HasColumnName("interv");

            Property(t => t.NfTipoDocumentoCodigo)
                .HasColumnName("tp_doc");

            Property(t => t.NfNumero)
                .HasColumnName("num_nf");

            Property(t => t.NfSerie)
                .HasColumnName("serie");

            Property(t => t.NfSequencia)
                .HasColumnName("seq_nf");
        }
    }
}

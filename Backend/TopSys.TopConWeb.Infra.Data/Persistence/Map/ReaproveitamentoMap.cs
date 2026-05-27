using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ReaproveitamentoMap : EntityTypeConfiguration<Reaproveitamento>
    {
        public ReaproveitamentoMap()
        {
            ToTable("topsys.con_reaprov");

            HasKey(t => new { t.FilialCodigo, t.IntervenienteCodigo, t.TipoDocumentoCodigo, t.Numero, t.Serie, t.Sequencia, t.BetoneiraReaproveitamento });

            Property(t => t.FilialCodigo)
                .HasColumnOrder(0)
                .HasColumnName("filial");

            Property(t => t.IntervenienteCodigo)
                .HasColumnOrder(1)
                .HasColumnName("interv");

            Property(t => t.TipoDocumentoCodigo)
                .HasColumnOrder(2)
                .HasColumnName("tp_doc");

            Property(t => t.Numero)
                .HasColumnOrder(3)
                .HasColumnName("num_nf");

            Property(t => t.Serie)
                .HasColumnOrder(4)
                .HasColumnName("serie");

            Property(t => t.Sequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq_nf");

            Property(t => t.BetoneiraReaproveitamento)
                .HasColumnOrder(6)
                .HasColumnName("bt_reaprov");

            Property(t => t.UsinaCodigo)
                .HasColumnName("usina");

            Property(t => t.DataRemessa)
                .HasColumnName("data_remessa");

            Property(t => t.VolumeRetorno)
                .HasColumnName("sb_vol_retorno");

            Property(t => t.Status)
                .HasColumnName("sb_status");

            Property(t => t.Observacao)
                .HasColumnName("sb_observacao");

            Property(t => t.FilialNotaDestino)
                .HasColumnName("ap_filial");

            Property(t => t.IntervenienteNotaDestino)
                .HasColumnName("ap_interv");

            Property(t => t.TipoDocumentoNotaDestino)
                .HasColumnName("ap_tp_doc");

            Property(t => t.NumeroNotaDestino)
                .HasColumnName("ap_num_nf");

            Property(t => t.SerieNotaDestino)
                .HasColumnName("ap_serie");

            Property(t => t.SequenciaNotaDestino)
                .HasColumnName("ap_seq_nf");

            Property(t => t.UsinaNotaDestino)
                .HasColumnName("ap_usina");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtual)
                .HasColumnName("id_atual");
        }
    }
}

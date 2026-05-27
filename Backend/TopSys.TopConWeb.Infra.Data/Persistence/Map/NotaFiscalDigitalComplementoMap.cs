using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class NotaFiscalDigitalComplementoMap : EntityTypeConfiguration<NotaFiscalDigitalComplemento>
    {
        public NotaFiscalDigitalComplementoMap()
        {
            ToTable("topsys.fis_nf_complemento");

            HasKey(t => new { t.Filial, t.Cliente, t.TipoDocumento, t.Serie, t.Numero, t.Sequencia });

            Property(t => t.Filial)
                .HasColumnOrder(0)
                .HasColumnName("filial");

            Property(t => t.Cliente)
                .HasColumnOrder(1)
                .HasColumnName("interv");

            Property(t => t.TipoDocumento)
                .HasColumnOrder(2)
                .HasColumnName("tp_doc");

            Property(t => t.Serie)
                .HasColumnOrder(3)
                .HasColumnName("serie");

            Property(t => t.Numero)
                .HasColumnOrder(4)
                .HasColumnName("num_nf");

            Property(t => t.Sequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq_nf");

            Property(t => t.IndicadorPresenca)
                .HasColumnName("ind_presenca");

            Property(t => t.MeioPagamento)
                .HasColumnName("meio_pagamento");

            Property(t => t.PlacaTransportador)
                .HasColumnName("placa_transportador");

            Property(t => t.SequenciaTransacao).HasColumnName("trans_seq");
            Property(t => t.PercentualReducaoGoverno).HasColumnName("p_red_gov");
            Property(t => t.TipoEntidadeGoverno).HasColumnName("tp_ente_gov");
            Property(t => t.TipoOperacaoGoverno).HasColumnName("tp_oper_gov");
            Property(t => t.BaseIBSCBS).HasColumnName("base_ibscbs");
            Property(t => t.ValorCBS).HasColumnName("vl_cbs");
            Property(t => t.ValorIBS).HasColumnName("vl_ibs");
            Property(t => t.ValorIBSMunicipal).HasColumnName("vl_ibs_mun");
            Property(t => t.ValorIBSEstadual).HasColumnName("vl_ibs_uf");
            Property(t => t.BaseIS).HasColumnName("base_is");
            Property(t => t.ValorIS).HasColumnName("vl_is");
            Property(t => t.TipoNotaDebito).HasColumnName("tp_nf_debito");
            Property(t => t.TipoNotaCredito).HasColumnName("tp_nf_credito");
        }
    }
}

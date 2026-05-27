using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    class ContratoBombaReajusteVersaoMap : EntityTypeConfiguration<ContratoBombaReajusteVersao>
    {
        public ContratoBombaReajusteVersaoMap()
        {
            ToTable("topsys.con_reaj_bomba_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.DataVigencia, t.ObraBombaReajusteSequencia });

            HasRequired(t => t.Usina)
              .WithMany()
              .HasForeignKey(t => t.UsinaCodigo);

            HasRequired(t => t.BombaTipo)
               .WithMany()
               .HasForeignKey(t => new { t.BombaTipoCodigo })
               .WillCascadeOnDelete(false);

            Ignore(t => t.Obra);

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

            Property(t => t.ObraBombaReajusteSequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq");

            Property(t => t.BombaTipoCodigo)
                .HasColumnName("tipo_bomba");

            Property(t => t.ValorVigente)
                .HasColumnName("tx_min_atual");

            Property(t => t.VigenteAteM3)
                .HasColumnName("vol_min");

            Property(t => t.M3ExcedenteVigente)
                .HasColumnName("vlr_m3_atual");

            Property(t => t.M3ExcedenteReajustado)
                .HasColumnName("vlr_m3_recalc");

            Property(t => t.ReajustadoAteM3)
                .HasColumnName("vol_min_recalc");

            Property(t => t.ValorReajustado)
                .HasColumnName("tx_min_recalc");

            Property(t => t.DataCarta)
                .HasColumnName("data_carta");

            Property(t => t.DataConfirmacao)
                .HasColumnName("dt_confirmacao");

            Property(t => t.EmiteCartaSimNao)
                .HasColumnName("emite_carta");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.IdAprovacaoVersao)
                .HasColumnName("id_aprov_versao");

            Property(t => t.IdReprovacao)
                .HasColumnName("id_reprovacao");
        }

    }
}
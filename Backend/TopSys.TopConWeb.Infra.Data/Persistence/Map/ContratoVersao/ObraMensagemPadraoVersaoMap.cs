using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraMensagemPadraoVersaoMap : EntityTypeConfiguration<ObraMensagemPadraoVersao>
    {
        public ObraMensagemPadraoVersaoMap()
        {
            ToTable("topsys.con_obras_mp_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraNumero, t.MensagemPadraoCodigo });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ObraNumero)
                .HasColumnOrder(2)
                .HasColumnName("obra");

            Property(t => t.MensagemPadraoCodigo)
                .HasColumnOrder(3)
                .HasColumnName("cod");

            Property(t => t.SelecionadoSimNao)
                .HasColumnName("seleciona");

            HasRequired(t => t.MensagemPadrao)
                .WithMany()
                .HasForeignKey(t => t.MensagemPadraoCodigo);
        }
    }
}

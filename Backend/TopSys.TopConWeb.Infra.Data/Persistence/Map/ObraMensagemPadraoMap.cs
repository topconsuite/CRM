using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraMensagemPadraoMap : EntityTypeConfiguration<ObraMensagemPadrao>
    {
        public ObraMensagemPadraoMap()
        {
            ToTable("topsys.con_obras_mp");

            HasKey(t => new { t.UsinaCodigo, t.ObraNumero, t.MensagemPadraoCodigo });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ObraNumero)
                .HasColumnOrder(1)
                .HasColumnName("obra");

            Property(t => t.MensagemPadraoCodigo)
                .HasColumnOrder(2)
                .HasColumnName("cod");

            Property(t => t.SelecionadoSimNao)
                .HasColumnName("seleciona");

            HasRequired(t => t.MensagemPadrao)
                .WithMany()
                .HasForeignKey(t => t.MensagemPadraoCodigo);
        }
    }
}

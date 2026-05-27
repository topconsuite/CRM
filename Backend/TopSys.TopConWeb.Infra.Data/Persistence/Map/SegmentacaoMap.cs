using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class SegmentacaoMap : EntityTypeConfiguration<Segmentacao>
    {
        public SegmentacaoMap()
        {
            ToTable("topsys.con_segmentacao");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.NomeAbreviado)
                .HasColumnName("nome_abreviado");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

        }
    }
}







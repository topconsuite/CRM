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
    public class IntervenienteControleFaixaMap : EntityTypeConfiguration<IntervenienteSequence>
    {
        public IntervenienteControleFaixaMap()
        {
            ToTable("sequence_ger_interv");

            HasKey(t => t.UltimoID);

            Property(t => t.UltimoID)
                .HasColumnName("ultimo_id_ger_interv");

            Property(t => t.FaixaInicial)
                .HasColumnName("faixa_inicial")
                .IsRequired();

            Property(t => t.FaixaFinal)
                .HasColumnName("faixa_final")
                .IsRequired();
        }
    }
}

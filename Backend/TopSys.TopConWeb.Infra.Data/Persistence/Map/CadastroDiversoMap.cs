using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CadastroDiversoMap : EntityTypeConfiguration<CadastroDiverso>
    {
        public CadastroDiversoMap()
        {
            ToTable("topsys.ger_diverso");

            HasKey(t => new { t.Aplicativo, t.ProgramaNumero, t.ProgramaCampo, t.Codigo });

            Property(t => t.Aplicativo)
                .HasColumnOrder(0)
                .HasColumnName("aplic");

            Property(t => t.ProgramaNumero)
                .HasColumnOrder(1)
                .HasColumnName("prog");

            Property(t => t.ProgramaCampo)
                .HasColumnOrder(2)
                .HasColumnName("campo");

            Property(t => t.Codigo)
                .HasColumnOrder(3)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");
        }
    }
}

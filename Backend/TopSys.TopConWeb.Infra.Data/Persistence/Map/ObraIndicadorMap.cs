using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraIndicadorMap: EntityTypeConfiguration<ObraIndicador>
    {

        public ObraIndicadorMap()
        {

            ToTable("topsys.con_obras_indicador");

            HasKey(x => new { x.ObraUsina, x.ObraNumero });

            Property(x => x.ObraUsina)
                .HasColumnName("obra_usina")
                .IsRequired();

            Property(x => x.ObraNumero)
                .HasColumnName("obra_numero")
                .IsRequired();

            Property(x => x.IntervenienteCodigo)
                .HasColumnName("interveniente")
                .IsOptional();

            Property(x => x.VendedorCodigo)
                .HasColumnName("vendedor")
                .IsOptional();

            HasOptional(x => x.Vendedor)
                .WithMany()
                .HasForeignKey(x => x.VendedorCodigo);

            HasOptional(x => x.Interveniente)
                .WithMany()
                .HasForeignKey(x => x.IntervenienteCodigo);



        }
        

    }
}

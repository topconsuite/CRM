using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraIndicadorVersaoMap : EntityTypeConfiguration<ObraIndicadorVersao>
    {

        public ObraIndicadorVersaoMap()
        {

            ToTable("topsys.con_obras_indicador_versao");

            HasKey(x => new { x.ObraVersao, x.ObraUsina, x.ObraNumero  });

            Property(x => x.ObraUsina)
                .HasColumnName("obra_usina")
                .IsRequired();

            Property(x => x.ObraNumero)
                .HasColumnName("obra_numero")
                .IsRequired();

            Property(x => x.ObraVersao)
                .HasColumnName("obra_versao")
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

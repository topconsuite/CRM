using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TipoCobrancaMap : EntityTypeConfiguration<TipoCobranca>
    {
        public TipoCobrancaMap()
        {
            ToTable("topsys.view_tipo_cobranca");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("tipo_cobranca");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.Forma)
                .HasColumnName("forma");

            Property(t => t.PortadorCodigo)
                .HasColumnName("portador");
            HasOptional(t => t.Portador)
                .WithMany()
                .HasForeignKey(t => t.PortadorCodigo);

            Property(t => t.Fixo)
                .HasColumnName("fixo");

            Property(t => t.Aprovacao)
                .HasColumnName("aprovacao");

            Property(t => t.Situacao)
                .HasColumnName("situacao");
        }
    }
}

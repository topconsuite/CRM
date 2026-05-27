using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class EnderecoMap : EntityTypeConfiguration<Endereco>
    {
        public EnderecoMap()
        {
            ToTable("topsys.view_endereco");

            HasKey(t => t.Cep);

            Property(t => t.Cep)
                .HasColumnOrder(0)
                .HasColumnName("cep");

            Property(t => t.Logradouro)
                .HasColumnName("logradoudo");

            Property(t => t.Numero)
                .HasColumnName("numero");

            Property(t => t.Complemento)
                .HasColumnName("complemento");

            Property(t => t.Bairro)
                .HasColumnName("bairro");
            
            Property(t => t.MunicipioCodigo)
                .HasColumnName("municipio");

            HasRequired(t => t.Municipio)
                .WithMany()
                .HasForeignKey(t => t.MunicipioCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.IsConfiavel)
                .HasColumnName("confiavel");

        }
    }
}

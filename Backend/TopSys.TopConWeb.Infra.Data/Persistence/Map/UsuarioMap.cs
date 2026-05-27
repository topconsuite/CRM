using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class UsuarioMap:EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {

            ToTable("topsys.usr_usuario");

            HasKey(t => t.Id);

            Ignore(t => t.Status);

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("ID");

            Property(t => t.Nome)
                .HasColumnName("Nome");

            Property(t => t.Senha)
                .HasColumnName("Senha");

            Property(t => t.Email)
                .HasColumnName("Email");

            Property(t => t.StatusString)
                .HasColumnName("Status");

        }
    }
}

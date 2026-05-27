using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class VisitaContatoMap : EntityTypeConfiguration<VisitaContato>
    {

        public VisitaContatoMap()
        {
            ToTable("topsys.con_visita_contato");

            HasKey(t => new { t.Usina, t.AnoVisita, t.NumeroVisita, t.Sequencia });

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.AnoVisita)
                .HasColumnOrder(1)
                .HasColumnName("ano_visita");

            Property(t => t.NumeroVisita)
                .HasColumnOrder(2)
                .HasColumnName("num_visita");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq");

            Property(t => t.DddTelefone)
                .HasColumnOrder(4)
                .HasColumnName("ddd_telefone");

            Property(t => t.Telefone)
                .HasColumnOrder(5)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnOrder(6)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnOrder(7)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnOrder(8)
                .HasColumnName("email");

            Property(t => t.Nome)
                .HasColumnOrder(9)
                .HasColumnName("nome");

            Property(t => t.FuncaoCodigo)
                .HasColumnOrder(10)
                .HasColumnName("funcao");

            HasOptional(t => t.Funcao)
                .WithMany()
                .HasForeignKey(t => t.FuncaoCodigo);

        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades
{
    public class OportunidadeContatoMap : EntityTypeConfiguration<OportunidadeContato>
    {

        public OportunidadeContatoMap()
        {
            ToTable("topsys.con_oportunidade_contato");

            HasKey(t => new { t.Usina, t.AnoOportunidade, t.NumeroOportunidade, t.Sequencia });

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.AnoOportunidade)
                .HasColumnOrder(1)
                .HasColumnName("ano_oportunidade");

            Property(t => t.NumeroOportunidade)
                .HasColumnOrder(2)
                .HasColumnName("num_oportunidade");

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
        }

    }
}

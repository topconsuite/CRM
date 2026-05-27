using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PortadorMap : EntityTypeConfiguration<Portador>
    {
        public PortadorMap()
        {
            ToTable("topsys.fin_portador");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.ContaEmpresaCodigo)
                .HasColumnName("emp");

            Property(t => t.ContaCodigo)
                .HasColumnName("bco");
            
            Property(t => t.Situacao)
                .HasColumnName("sit");
            
            Property(t => t.EmiteCobranca)
                .HasColumnName("emite_cob");
            
            HasOptional(t => t.Conta)
                .WithMany()
                .HasForeignKey(t => new { t.ContaEmpresaCodigo, t.ContaCodigo });
        }
    }
}

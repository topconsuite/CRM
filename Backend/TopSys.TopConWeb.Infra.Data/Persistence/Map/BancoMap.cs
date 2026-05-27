using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class BancoMap : EntityTypeConfiguration<Banco>
    {
        public BancoMap()
        {
            ToTable("topsys.ger_banco");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("bco");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.RazaoSocial)
                .HasColumnName("razao");

            Property(t => t.CodigoOficial)
                .HasColumnName("bco_of");

            Property(t => t.NumeroAgencia)
                .HasColumnName("num_ag");

            Property(t => t.DigitoVerificadorAgencia)
                .HasColumnName("dv_ag");

            Property(t => t.NumeroConta)
                .HasColumnName("num_cta");

            Property(t => t.DigitoVerificadorConta)
                .HasColumnName("dv_cta");

            Property(t => t.Empresa)
                .HasColumnName("emp");

            Property(t => t.Empresa_proprietaria)
                .HasColumnName("emp_proprietaria");
        }
    }
}

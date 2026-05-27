using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContaMap : EntityTypeConfiguration<Conta>
    {
        public ContaMap()
        {
            ToTable("topsys.ger_banco");

            HasKey(t => new { t.EmpresaCodigo, t.Codigo });

            Property(t => t.EmpresaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("Emp");

            Property(t => t.Codigo)
                .HasColumnOrder(1)
                .HasColumnName("Bco");

            Property(t => t.Nome)
                .HasColumnName("Nome");

            Property(t => t.Razao)
                .HasColumnName("Razao");

            Property(t => t.BancoCodigoOficial)
                .HasColumnName("Bco_Of");

            Property(t => t.NumeroAgencia)
                .HasColumnName("Num_Ag");

            Property(t => t.NumeroConta)
                .HasColumnName("Num_Cta");

            Property(t => t.DvAgencia)
                .HasColumnName("Dv_Ag");

            Property(t => t.DvConta)
                .HasColumnName("Dv_Cta");

            Property(t => t.EmpresaProprietaria)
                .HasColumnName("emp_proprietaria");

            Property(t => t.DataUltimaConciliacao)
                .HasColumnName("dt_ult_conc");
        }
    }
}

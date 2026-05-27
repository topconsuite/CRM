using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class AprovacaoScriptMap:EntityTypeConfiguration<AprovacaoScript>
    {
        public AprovacaoScriptMap()
        {
            ToTable("topsys.con_aprov_script");

            HasKey(t => new { t.Chave , t.OperacaoTipo});

            Property(t => t.Chave)
                .HasColumnOrder(0)
                .HasColumnName("chave_email");

            Property(t => t.OperacaoTipo)
                .HasColumnName("operacao");

            Property(t => t.Script)
                .HasColumnName("script");

            Property(t => t.Status)
                .HasColumnName("status");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class TituloContasAPagarMap : EntityTypeConfiguration<TituloContasAPagar>
    {
        public TituloContasAPagarMap()
        {
            ToTable("topsys.fin_cap");

            HasKey(t => new {
                t.EmpresaCodigo,
                t.FornecedorCodigo,
                t.DocumentoTipoCodigo,
                t.DocumentoSerie,
                t.DocumentoNumero,
                t.DocumentoSequencia,
                t.Desdobramento,
                t.FornecedorRetidoCodigo
            });

            var i = 0;

            Property(t => t.EmpresaCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("emp");

            Property(t => t.FornecedorCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("forn");

            Property(t => t.DocumentoTipoCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("tp_doc");

            Property(t => t.DocumentoSerie)
                .HasColumnOrder(i++)
                .HasColumnName("ser_doc");

            Property(t => t.DocumentoNumero)
                .HasColumnOrder(i++)
                .HasColumnName("num_doc");

            Property(t => t.DocumentoSequencia)
                .HasColumnOrder(i++)
                .HasColumnName("seq");

            Property(t => t.Desdobramento)
                .HasColumnOrder(i++)
                .HasColumnName("desdo");

            Property(t => t.FornecedorRetidoCodigo)
                .HasColumnOrder(i++)
                .HasColumnName("forn_retido");

            Property(t => t.Valor)
                .HasColumnOrder(i++)
                .HasColumnName("vl");

            Property(t => t.LoteBaixaDeCredito)
                .HasColumnOrder(i++)
                .HasColumnName("lote_bx_cred");

            Property(t => t.LoteBaixa)
                .HasColumnOrder(i++)
                .HasColumnName("lote_baixa_car");

        }
    }
}

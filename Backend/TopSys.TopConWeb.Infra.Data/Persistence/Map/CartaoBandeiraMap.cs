using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class CartaoBandeiraMap : EntityTypeConfiguration<CartaoBandeira>
    {
        public CartaoBandeiraMap()
        {
            ToTable("topsys.view_cartao_bandeira");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod_bandeira");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.TipoIntegracao)
                .HasColumnName("tipo_integracao");

            Property(t => t.IntervenienteCodigo)
                .HasColumnName("interv");

            HasOptional(t => t.Interveniente)
                .WithMany()
                .HasForeignKey(t => t.IntervenienteCodigo);

            Property(t => t.PortadorCodigo)
                .HasColumnName("portador");

            HasOptional(t => t.Portador)
                .WithMany()
                .HasForeignKey(t => t.PortadorCodigo);

            Property(t => t.EstabelecimentoCod)
                .HasColumnName("cod_estab");

            Property(t => t.EmpresaCod)
                .HasColumnName("emp");

            Property(t => t.BancoCod)
                .HasColumnName("cod_banco");

            Property(t => t.CentroDeCusto)
                .HasColumnName("ccusto");

            Property(t => t.CentroDeCusto)
                .HasColumnName("ccusto");

            Property(t => t.DiasPrimeiraParcela)
                .HasColumnName("dias_1a_parc");

            Property(t => t.DiasSegundaParcela)
                .HasColumnName("dias_2a_parc");

            Property(t => t.DiasTerceiraParcela)
                .HasColumnName("dias_3a_parc");

            Property(t => t.DiasQuartaParcela)
                .HasColumnName("dias_4a_parc");

            Property(t => t.DiasQuintaParcela)
                .HasColumnName("dias_5a_parc");

            Property(t => t.DiasSextaParcela)
                .HasColumnName("dias_6a_parc");

            Property(t => t.DiasSetimaParcela)
                .HasColumnName("dias_7a_parc");

            Property(t => t.DiasOitavaParcela)
                .HasColumnName("dias_8a_parc");

            Property(t => t.DiasNonaParcela)
                .HasColumnName("dias_9a_parc");

            Property(t => t.DiasDecimaParcela)
                .HasColumnName("dias_10a_parc");

            Property(t => t.DiasDecimaPrimeiraParcela)
                .HasColumnName("dias_11a_parc");

            Property(t => t.DiasDecimaSegundaParcela)
                .HasColumnName("dias_12a_parc");

            Property(t => t.DiasFloatDebito)
                .HasColumnName("dias_float_deb");

            Property(t => t.Ativo)
                .HasColumnName("ativo");
        }
    }
}

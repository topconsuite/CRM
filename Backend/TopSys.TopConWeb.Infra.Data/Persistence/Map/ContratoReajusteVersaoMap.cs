using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoReajusteVersaoMap : EntityTypeConfiguration<ContratoReajusteVersao>
    {
        public ContratoReajusteVersaoMap()
        {
            ToTable("topsys.con_contrato_reajuste_versao");

            HasKey(t => new { t.NumeroVersao, t.Usina, t.ContratoAno, t.ContratoNumero, t.DataVigencia, t.Tipo });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.Usina)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnOrder(2)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnOrder(3)
                .HasColumnName("num_contrato");

            Property(t => t.DataVigencia)
                .HasColumnOrder(4)
                .HasColumnName("dt_vigencia");

            Property(t => t.Tipo)
                .HasColumnName("tipo");
        }
    }
}

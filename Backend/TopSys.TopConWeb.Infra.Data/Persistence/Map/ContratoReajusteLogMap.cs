using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoReajusteLogMap: EntityTypeConfiguration<ContratoReajusteLog>
    {
        public ContratoReajusteLogMap()
        {
            ToTable("topsys.con_reajuste_log");

            HasKey(t => new { t.Usina, t.ContratoAno, t.ContratoNumero, t.DataVigencia, t.Sequencia, t.Tipo });

            Property(t => t.Usina)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnOrder(1)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnOrder(2)
                .HasColumnName("num_contrato");

            Property(t => t.DataVigencia)
                .HasColumnOrder(3)
                .HasColumnName("dt_vigencia");

            Property(t => t.Sequencia)
                .HasColumnOrder(4)
                .HasColumnName("sequencia");

            Property(t => t.Tipo)
                .HasColumnName("tipo");

            Property(t => t.DataHoraEvento)
                .HasColumnName("dt_hora_evento");

            Property(t => t.Evento)
                .HasColumnName("evento");

            Property(t => t.Complemento)
                .HasColumnName("complemento");
        }
    }
}

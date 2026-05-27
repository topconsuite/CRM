using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ProgramacaoDemaisServicosMap : EntityTypeConfiguration<ProgramacaoDemaisServicos>
    {
        public ProgramacaoDemaisServicosMap()
        {
            ToTable("topsys.con_programacao_dem_serv");

            HasKey(t => new { t.UsinaCodigo, t.ObraNumero, t.ProgramacaoSequencia, t.Sequencia });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina");

            Property(t => t.ObraNumero)
                .HasColumnOrder(1)
                .HasColumnName("obra");

            Property(t => t.ProgramacaoSequencia)
                .HasColumnOrder(2)
                .HasColumnName("seq_prog");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq");

            Property(t => t.Quantidade)
                .HasColumnName("Quantidade");

            Property(t => t.ValorTotal)
                .HasColumnName("valor_total");

            Property(t => t.ValorCobrado)
                .HasColumnName("valor_cobrado");

        }
    }
}

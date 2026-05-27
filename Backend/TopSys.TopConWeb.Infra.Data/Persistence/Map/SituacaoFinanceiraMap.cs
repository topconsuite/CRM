using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class SituacaoFinanceiraMap : EntityTypeConfiguration<SituacaoFinanceira>
    {
        public SituacaoFinanceiraMap()
        {
            ToTable("topsys.fin_situacao");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.Fixo)
                .HasColumnName("fixo");

            Property(t => t.OperacaoBaixa)
                .HasColumnName("oper_baixa");
        }
    }
}

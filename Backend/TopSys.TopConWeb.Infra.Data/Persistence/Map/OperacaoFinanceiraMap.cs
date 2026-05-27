using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class OperacaoFinanceiraMap : EntityTypeConfiguration<OperacaoFinanceira>
    {
        public OperacaoFinanceiraMap()
        {
            ToTable("topsys.fin_operacao");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Descricao)
                .HasColumnName("descr");
            
            Property(t => t.SubSistema)
                .HasColumnName("Sub_Sist");
            
            Property(t => t.InclusaoOuBaixa)
                .HasColumnName("IB");
            
            Property(t => t.SemMovFinanceiro)
                .HasColumnName("s_mov_fin");
            
            Property(t => t.AtualizaBanco)
                .HasColumnName("At_Bco");
            
            Property(t => t.OperacaoBaixa)
                .HasColumnName("Op_Bx");
            
            Property(t => t.OperacaoMovBco)
                .HasColumnName("Op_Bco");
            
            Property(t => t.ReceitaDespesa)
                .HasColumnName("Rec_Desp");
            
            Property(t => t.CentrosDeCusto)
                .HasColumnName("ccustos");
        }
    }
}
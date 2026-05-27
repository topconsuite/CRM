using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheChequeMap : EntityTypeConfiguration<ContratoPagamentoDetalheCheque>
    {
        public ContratoPagamentoDetalheChequeMap()
        {
            HasKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(0)
               .HasColumnName("usina");

            Property(t => t.ContratoAno)
               .HasColumnOrder(1)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnOrder(2)
               .HasColumnName("num_contrato");

            Property(t => t.PagamentoSequencia)
               .HasColumnOrder(3)
               .HasColumnName("seq_pgto");

            Property(t => t.DetalheSequencia)
               .HasColumnOrder(4)
               .HasColumnName("seq");

            Property(t => t.BancoCodigoOficial)
               .HasColumnName("bco_cheque");

            Property(t => t.BancoAgencia)
               .HasColumnName("ag_cheque");

            Property(t => t.BancoContaNumero)
               .HasColumnName("conta_cheque");

            Property(t => t.BancoContaDV)
               .HasColumnName("dv_conta");

            Property(t => t.NumeroCheque)
               .HasColumnName("num_cheque");

            Property(t => t.DataRecebimento)
               .HasColumnName("dt_receb");

            Property(t => t.DataBomPara)
               .HasColumnName("bom_para");

            Property(t => t.Observacao)
               .HasColumnName("obs");

            Property(t => t.Valor)
               .HasColumnName("vlr");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.PropostaAno)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnName("no_obra");
        }
    }
}

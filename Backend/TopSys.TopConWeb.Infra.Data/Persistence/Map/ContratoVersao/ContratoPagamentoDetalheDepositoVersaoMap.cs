using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheDepositoVersaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheDepositoVersao>
    {
        public ContratoPagamentoDetalheDepositoVersaoMap()
        {
            //ToTable("con_contrato_dep_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.NumeroVersao)
               .HasColumnOrder(0)
               .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(1)
               .HasColumnName("usina");

            Property(t => t.ContratoAno)
               .HasColumnOrder(2)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnOrder(3)
               .HasColumnName("num_contrato");

            Property(t => t.PagamentoSequencia)
               .HasColumnOrder(4)
               .HasColumnName("seq_pgto");

            Property(t => t.DetalheSequencia)
               .HasColumnOrder(5)
               .HasColumnName("seq");

            Property(t => t.DataDeposito)
               .HasColumnName("dt_deposito");

            Property(t => t.PortadorCodigo)
               .HasColumnName("portador");

            HasOptional(t => t.Portador)
                .WithMany()
                .HasForeignKey(t => t.PortadorCodigo);

            Property(t => t.NumeroTerminal)
               .HasColumnName("no_terminal");

            Property(t => t.Valor)
               .HasColumnName("valor_dep");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.IdAprovacao)
               .HasColumnName("id_aprovacao");

            Property(t => t.PropostaAno)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnName("no_obra");

            Property(t => t.TomadorBanco)
               .HasColumnName("bco_tomad");

            Property(t => t.TomadorAgencia)
               .HasColumnName("agencia_tomad");

            Property(t => t.TomadorNumeroConta)
               .HasColumnName("no_conta");

        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheDepositoMap : EntityTypeConfiguration<PropostaPagamentoDetalheDeposito>
    {
        public PropostaPagamentoDetalheDepositoMap()
        {
            //ToTable("view_con_chtel_dep");

            HasKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia, t.DetalheSequencia });

            Property(t => t.UsinaCodigo)
               .HasColumnOrder(0)
               .HasColumnName("usina");

            Property(t => t.PropostaAno)
               .HasColumnOrder(1)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnOrder(2)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnOrder(3)
               .HasColumnName("no_obra");

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

            Property(t => t.ContratoAno)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnName("num_contrato");

            Property(t => t.TomadorBanco)
               .HasColumnName("bco_tomad");

            Property(t => t.TomadorAgencia)
               .HasColumnName("agencia_tomad");

            Property(t => t.TomadorNumeroConta)
               .HasColumnName("no_conta");

        }
    }
}

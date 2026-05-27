using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheBoletoVersaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheBoletoVersao>
    {
        public ContratoPagamentoDetalheBoletoVersaoMap()
        {

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

            Property(t => t.PropostaAno)
               .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
               .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
               .HasColumnName("no_obra");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
               .HasColumnName("id_atual");

            Property(t => t.DataVencimento)
               .HasColumnName("dt_vcto");

            Property(t => t.DataHoraImpressao)
               .HasColumnName("dt_hr_imp");

            Property(t => t.NossoNumero)
               .HasColumnName("nosso_num");

            Property(t => t.LinhaDigitavel)
               .HasColumnName("linha_dig");

            Property(t => t.CodigoDeBarras)
               .HasColumnName("cod_barra");

            Property(t => t.DataRemessa)
               .HasColumnName("dt_remessa");

            Property(t => t.DataLiquidacao)
               .HasColumnName("dt_liq");

            Property(t => t.Valor)
               .HasColumnName("vl_orig");

            Property(t => t.ValorLiquidacao)
               .HasColumnName("vl_liq");

            Property(t => t.IdLiquidacao)
               .HasColumnName("id_liq");
        }
    }
}
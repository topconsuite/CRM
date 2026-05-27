using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheBoletoMap : EntityTypeConfiguration<PropostaPagamentoDetalheBoleto>
    {
        public PropostaPagamentoDetalheBoletoMap()
        {
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

            Property(t => t.ContratoAno)
               .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
               .HasColumnName("num_contrato");

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

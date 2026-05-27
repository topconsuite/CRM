using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheMap : EntityTypeConfiguration<ContratoPagamentoDetalhe>
    {
        public ContratoPagamentoDetalheMap()
        {
            Map<ContratoPagamentoDetalheDeposito>( t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_dep");
            });

            Map<ContratoPagamentoDetalheCartao>( t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_ccredit");
            });

            Map<ContratoPagamentoDetalheBoleto>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_boleto");
            });

            Map<ContratoPagamentoDetalheDinheiro>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_dinheir");
            });

            Map<ContratoPagamentoDetalheCheque>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_cheque");
            });

            HasKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });
        }
    }
}

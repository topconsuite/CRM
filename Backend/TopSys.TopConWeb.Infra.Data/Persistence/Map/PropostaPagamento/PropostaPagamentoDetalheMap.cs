using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheMap : EntityTypeConfiguration<PropostaPagamentoDetalhe>
    {
        public PropostaPagamentoDetalheMap()
        {
            Map<PropostaPagamentoDetalheDeposito>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_dep");
            });

            Map<PropostaPagamentoDetalheCartao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_ccredit");
            });

            Map<PropostaPagamentoDetalheBoleto>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_boleto");
            });

            Map<PropostaPagamentoDetalheCheque>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_cheque");
            });

            Map<PropostaPagamentoDetalheDinheiro>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_dinheir");
            });

            HasKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia, t.DetalheSequencia });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class PropostaPagamentoDetalheVersaoMap : EntityTypeConfiguration<PropostaPagamentoDetalheVersao>
    {
        public PropostaPagamentoDetalheVersaoMap()
        {
            Map<PropostaPagamentoDetalheDepositoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_dep_versao");
            });

            Map<PropostaPagamentoDetalheCartaoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_ccredit_versao");
            });

            Map<PropostaPagamentoDetalheBoletoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_boleto_versao");
            });

            Map<PropostaPagamentoDetalheChequeVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_cheque_versao");
            });

            Map<PropostaPagamentoDetalheDinheiroVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.view_con_chtel_dinheir_versao");
            });

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.PagamentoSequencia, t.DetalheSequencia });
        }
    }
}
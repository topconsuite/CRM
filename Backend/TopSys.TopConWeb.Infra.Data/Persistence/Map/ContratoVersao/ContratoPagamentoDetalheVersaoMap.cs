using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ContratoPagamentoDetalheVersaoMap : EntityTypeConfiguration<ContratoPagamentoDetalheVersao>
    {
        public ContratoPagamentoDetalheVersaoMap()
        {
            Map<ContratoPagamentoDetalheDepositoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_dep_versao");
            });

            Map<ContratoPagamentoDetalheCartaoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_ccredit_versao");
            });

            Map<ContratoPagamentoDetalheBoletoVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_boleto_versao");
            });

            Map<ContratoPagamentoDetalheDinheiroVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_dinheir_versao");
            });

            Map<ContratoPagamentoDetalheChequeVersao>(t =>
            {
                t.MapInheritedProperties();
                t.ToTable("topsys.con_contrato_cheque_versao");
            });

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.PagamentoSequencia, t.DetalheSequencia });
        }
    }
}
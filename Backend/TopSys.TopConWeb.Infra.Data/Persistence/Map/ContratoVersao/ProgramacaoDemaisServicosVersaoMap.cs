using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ProgramacaoDemaisServicosVersaoMap : EntityTypeConfiguration<ProgramacaoDemaisServicosVersao>
    {
        public ProgramacaoDemaisServicosVersaoMap()
        {
            ToTable("topsys.con_programacao_dem_serv_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraNumero, t.ProgramacaoSequencia, t.Sequencia });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnType("usmallint")
                .HasColumnName("usina");

            Property(t => t.ObraNumero)
                .HasColumnOrder(2)
                .HasColumnName("obra");

            Property(t => t.ProgramacaoSequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq_prog");

            Property(t => t.Sequencia)
                .HasColumnOrder(4)
                .HasColumnName("seq");

            Property(t => t.Quantidade)
                .HasColumnName("Quantidade");

            Property(t => t.ValorTotal)
                .HasColumnName("valor_total");

            Property(t => t.ValorCobrado)
                .HasColumnName("valor_cobrado");

        }
    }
}
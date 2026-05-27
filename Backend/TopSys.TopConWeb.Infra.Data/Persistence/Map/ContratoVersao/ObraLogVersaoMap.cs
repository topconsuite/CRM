using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraLogVersaoMap : EntityTypeConfiguration<ObraLogVersao>
    {
        public ObraLogVersaoMap()
        {
            ToTable("topsys.con_obras_log_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.ObraCodigo, t.DataHora, t.Usuario, t.Evento, t.AnoChamada, t.NumChamada, t.Sequencia });

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(2)
                .HasColumnName("obra");

            Property(t => t.DataHora)                
                .HasColumnOrder(3)
                .HasColumnName("dt_hora_evento");

            Property(t => t.Usuario)
                .HasColumnOrder(4)
                .HasColumnName("usuario");

            Property(t => t.Evento)
                .HasColumnOrder(5)
                .HasColumnName("evento");

            Property(t => t.AnoChamada)
                .HasColumnOrder(6)
                .HasColumnName("ano_chamada");

            Property(t => t.NumChamada)
                .HasColumnOrder(7)
                .HasColumnName("no_chamada");

            Property(t => t.Sequencia)
                .HasColumnOrder(8)
                .HasColumnName("seq_log");

            Property(t => t.Complemento)
                .HasColumnName("complemento");

            Property(t => t.Observacao)
                .HasColumnName("obs")
                .HasColumnType("text");

            
        }
    }
}
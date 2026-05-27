using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ProgramacaoLogMap : EntityTypeConfiguration<ProgramacaoLog>
    {
        public ProgramacaoLogMap()
        {
            ToTable("topsys.con_programacao_log");

            HasKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero, t.ProgramacaoSequencia, t.Horario, t.Sequencia, t.PropostaAno, t.PropostaNumero, t.ObraCodigo, t.DataHora });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnOrder(1)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnOrder(2)
                .HasColumnName("no_contrato");

            Property(t => t.ProgramacaoSequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq_prog");

            Property(t => t.Horario)
                .HasColumnOrder(4)
                .HasColumnName("horario");

            Property(t => t.Sequencia)
                .HasColumnOrder(5)
                .HasColumnName("seq");

            Property(t => t.PropostaAno)
                .HasColumnOrder(6)
                .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
                .HasColumnOrder(7)
                .HasColumnName("num_chamada");

            Property(t => t.ObraCodigo)
                .HasColumnOrder(8)
                .HasColumnName("no_obra");

            Property(t => t.DataHora)
                .HasColumnOrder(9)
                .HasColumnName("data_hora");

            Property(t => t.Usuario)
                .HasColumnName("usuario");

            Property(t => t.Evento)
                .HasColumnName("evento");

            Property(t => t.Complemento)
                .HasColumnName("complemento");

            Property(t => t.Descricao)
                .HasColumnName("descricao");
        }
    }
}

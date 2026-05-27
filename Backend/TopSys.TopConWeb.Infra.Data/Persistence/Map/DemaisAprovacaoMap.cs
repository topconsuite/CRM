using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class DemaisAprovacaoMap:EntityTypeConfiguration<DemaisAprovacao>
    {
        public DemaisAprovacaoMap()
        {
            ToTable("topsys.con_aprov");

            HasKey(t => t.Chave);

            Ignore(t => t.StatusAprovacao);
            Ignore(t => t.LogObservacao);

            HasMany(t => t.AprovacoesScript)
                .WithOptional()
                .HasForeignKey(t => t.Chave);

            HasRequired(t => t.AprovacaoTipo)
                .WithMany()
                .HasForeignKey(t => t.AprovacaoTipoCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.Chave)
                .HasColumnOrder(0)
                .HasColumnName("chave");

            Property(t => t.AprovacaoTipoCodigo)
                .HasColumnName("tipo_aprov");

            Property(t => t.UsuarioRequisitante)
                .HasColumnName("usuario_req");

            Property(t => t.UsuarioAprovacao)
                .HasColumnName("usuario_aprov");

            Property(t => t.DataHoraSolicitacao)
                .HasColumnName("dt_hora_solic");

            Property(t => t.DataHoraExecucao)
               .HasColumnName("dt_hora_exec");

            Property(t => t.Complemento)
                .HasColumnName("complemento");

            Property(t => t.Observacao)
                .HasColumnName("observacao");

        }
    }
}

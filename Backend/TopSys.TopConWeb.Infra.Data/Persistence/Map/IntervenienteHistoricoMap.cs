using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class IntervenienteHistoricoMap: EntityTypeConfiguration<IntervenienteHistorico>
    {
        public IntervenienteHistoricoMap()
        {
            ToTable("topsys.ger_hist_interv");

            HasKey(t => new { t.CodigoInterveniente, t.SequenciaHistorico });

            Property(t => t.CodigoInterveniente)
               .HasColumnOrder(0)
               .HasColumnName("interv");

            Property(t => t.SequenciaHistorico)
                .HasColumnOrder(1)
                .HasColumnName("seq");

            Property(t => t.Data)
                .HasColumnName("dt");

            Property(t => t.Hora)
                .HasColumnName("hora");

            Property(t => t.Descricao)
                .HasColumnName("descr");
            
            Property(t => t.DataPrevistaDeRetorno)
                .HasColumnName("data_prev_ret");

            Property(t => t.HoraPrevistaDeRetorno)
                .HasColumnName("hora_prev_ret");

            Property(t => t.DataDeRetorno)
                .HasColumnName("data_retorno");

            Property(t => t.HoraDeRetorno)
                .HasColumnName("hora_retorno");

            Property(t => t.Hora)
                .HasColumnName("hora");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtual)
                .HasColumnName("id_atual");

            Property(t => t.Vinculo)
                .HasColumnName("vinculo");

            Property(t => t.EmpresaCodigo)
                .HasColumnName("emp");

            Property(t => t.DocumentoTipo)
                .HasColumnName("tp_doc");

            Property(t => t.DocumentoSerie)
                .HasColumnName("ser_doc");

            Property(t => t.DocumentoNumero)
                .HasColumnName("num_doc");

            Property(t => t.DocumentoSequencia)
                .HasColumnName("seq_doc");

            Property(t => t.BancoBandeiraCodigo)
                .HasColumnName("cod_banco_band");

            Property(t => t.AgenciaNumero)
                .HasColumnName("num_agencia");

            Property(t => t.ContaNumero)
                .HasColumnName("num_conta");

            Property(t => t.ContaDigito)
                .HasColumnName("dv_conta");

            Property(t => t.Desdobramento)
                .HasColumnName("desdo");

            Property(t => t.FornecedoRetido)
                .HasColumnName("forn_retido");
        }
    }
}

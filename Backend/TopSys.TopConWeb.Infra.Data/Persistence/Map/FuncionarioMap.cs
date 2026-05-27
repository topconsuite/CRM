using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class FuncionarioMap:EntityTypeConfiguration<Funcionario>
    {
        public FuncionarioMap()
        {
            ToTable("topsys.con_funcionario");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.Ativo)
               .HasColumnName("ativo");

            Property(t => t.Comprador)
               .HasColumnName("comprador");

            Property(t => t.Usina)
               .HasColumnName("usina");

            Property(t => t.Departamento)
               .HasColumnName("departamento");

            Property(t => t.HoraEntrada)
               .HasColumnName("hr_entrada_padr");

            Property(t => t.HoraSaida)
               .HasColumnName("hr_saida_padr");

            Property(t => t.ExternalId)
               .HasColumnName("external_id");

            Property(t => t.ValorHora)
               .HasColumnName("vlr_hora");

            Property(t => t.RE)
               .HasColumnName("re");

            Property(t => t.Funcao)
               .HasColumnName("funcao");

            Property(t => t.NomeReduzido)
               .HasColumnName("nome_reduzido");

            Property(t => t.Status)
               .HasColumnName("status");

            Property(t => t.UsuarioSistema)
               .HasColumnName("usuario_sist");

            Property(t => t.CodigoInterveniente)
               .HasColumnName("interv");

            Property(t => t.IdCadastro)
               .HasColumnName("id_cadast");

            Property(t => t.IdAtual)
               .HasColumnName("id_atual");

            HasRequired(t => t.Interveniente)
                .WithMany()
                .HasForeignKey(t => t.CodigoInterveniente);
        }

    }
}

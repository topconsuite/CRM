using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class IntervenienteLocalMap : EntityTypeConfiguration<IntervenienteLocal>
    {
        public IntervenienteLocalMap()
        {
            ToTable("topsys.ger_local");

            HasKey(t => new { t.IntervenienteCodigo, t.Sequencia });

            Property(t => t.IntervenienteCodigo)
                .HasColumnOrder(0)
                .HasColumnName("interv");

            Property(t => t.Sequencia)
                .HasColumnOrder(1)
                .HasColumnName("seq");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.Razao)
                .HasColumnName("razao");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("end");

            Property(t => t.EnderecoNumero)
                .HasColumnName("num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("mun");
            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            Property(t => t.CpfCnpj)
                .HasColumnName("cnpj_cpf");

            Property(t => t.Rg)
                .HasColumnName("rg");

            Property(t => t.OrgaoExpedidor)
                .HasColumnName("emissor");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("ie");

            Property(t => t.InscricaoMunicipal)
                .HasColumnName("ccm");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.LocalCobrancaSimNao)
                .HasColumnName("loc_cobr");

            Property(t => t.LocalEntregaSimNao)
                .HasColumnName("loc_entrega");

            Property(t => t.LocalFaturamentoSimNao)
                .HasColumnName("loc_fatur");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

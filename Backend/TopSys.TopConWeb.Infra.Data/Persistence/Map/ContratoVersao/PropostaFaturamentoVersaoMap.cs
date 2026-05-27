using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    class PropostaFaturamentoVersaoMap : EntityTypeConfiguration<PropostaFaturamentoVersao>
    {
        public PropostaFaturamentoVersaoMap()
        {
            ToTable("topsys.con_chtel_faturamento_versao");

            HasKey(t => new { t.NumeroVersao, t.UsinaCodigo, t.PropostaAno, t.PropostaNumero });

            HasRequired(t => t.Proposta)
                .WithOptional(t => t.Faturamento);

            Property(t => t.NumeroVersao)
                .HasColumnOrder(0)
                .HasColumnName("num_versao");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.PropostaAno)
                .HasColumnOrder(2)
                .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
                .HasColumnOrder(3)
                .HasColumnName("num_chamada");

            Property(t => t.IntervenienteTipo)
                .HasColumnName("idem_client");

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
                .HasColumnName("org_uf_emi");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("ie");

            Property(t => t.InscricaoMunicipal)
                .HasColumnName("ccm");

            Property(t => t.Email)
                .HasColumnName("email");
        }
    }
}

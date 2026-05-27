using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class IntervenienteMap:EntityTypeConfiguration<Interveniente>
    {
        public IntervenienteMap()
        {
            ToTable("topsys.ger_interv");

            HasKey(t => t.Codigo);
           
            HasOptional(t => t.BloqueioMotivo)
                .WithMany()
                .HasForeignKey(t => t.BloqueioMotivoCodigo);

            HasOptional(t => t.GrupoEconomico)
                .WithMany()
                .HasForeignKey(t => t.GrupoEconomicoCodigo);

            HasMany(t => t.Contratos)
               .WithOptional()
               .HasForeignKey(t => t.IntervenienteCodigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("Cod");

            Property(t => t.Nome)
                .HasColumnName("Nome");

            Property(t => t.Razao)
                .HasColumnName("Razao");

            Property(t => t.IntervenienteTipo)
                .HasColumnName("tp_cliente");

            Property(t => t.CpfCnpj)
                .HasColumnName("CNPJ_CPF");

            Property(t => t.Rg)
                .HasColumnName("RG");

            Property(t => t.OrgaoExpedidor)
                .HasColumnName("Org_Uf_Emi");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("IE");

            Property(t => t.InscricaoMunicipal)
                .HasColumnName("Ccm");

            Property(t => t.EnderecoCep)
                .HasColumnName("Cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("End");

            Property(t => t.EnderecoNumero)
                .HasColumnName("Num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("Bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("cod_munic");

            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.Profissao)
                .HasColumnName("profissao");

            Property(t => t.EmpresaTrabalho)
                .HasColumnName("emp_trabalho");

            Property(t => t.TelefoneComercialDdd)
                .HasColumnName("ddd_com");

            Property(t => t.TelefoneComercialNumero)
                .HasColumnName("tel_com");

            Property(t => t.NomeMae)
                .HasColumnName("nome_mae");

            Property(t => t.NomeConjuge)
                .HasColumnName("conjuge");

            Property(t => t.Contato)
                .HasColumnName("contato");

            Property(t => t.TelefoneDdd)
                .HasColumnName("DDD");

            Property(t => t.TelefoneNumero)
                .HasColumnName("Tel");

            Property(t => t.Ramal)
                .HasColumnName("Ramal");

            Property(t => t.CelularDdd)
                .HasColumnName("ddd_celular");

            Property(t => t.CelularNumero)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.EmailCobranca)
                .HasColumnName("email_cobranca");

            Property(t => t.LimiteValor)
                 .HasColumnName("limite_cred");

            Property(t => t.LimiteData)
                .HasColumnName("lim_cred_val");

            Property(t => t.BloqueioMotivoCodigo)
                .HasColumnName("bloq");

            Property(t => t.BloqueioObservacao)
                .HasColumnName("obs_bloq");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.bombista)
                .HasColumnName("bombista");

            Property(t => t.VendedorCodigo)
                .HasColumnName("Vend");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            HasOptional(t => t.Vendedor)
                .WithMany()
                .HasForeignKey(t => t.VendedorCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.RetemIss)
                .HasColumnName("ret_iss");

            Property(t => t.IdAprovacaoRetencaoIss)
                .HasColumnName("id_aprov_iss");

            Property(t => t.Cliente)
                .HasColumnName("cli");

            Property(t => t.GrupoEconomicoCodigo)
                .HasColumnName("grupo_economico");

            //For Public Integration

            Property(t => t.Fornecedor)
                .HasColumnName("forn");

            Property(t => t.Transportador)
                .HasColumnName("Transpo");

            Property(t => t.PrestadorServico)
                .HasColumnName("prest_serv");

            Property(t => t.OrgaoPublico)
                .HasColumnName("org_publ");

            Property(t => t.Outros)
                .HasColumnName("outro");

            Property(t => t.Atividade)
                .HasColumnName("ativ");

            Property(t => t.TipoCobranca)
                .HasColumnName("tp_cobranca");

            Property(t => t.PorcentagemDesconto)
                .HasColumnName("pct_desco");

            Property(t => t.ContaContabil)
                .HasColumnName("ctb_cta_contab");

            Property(t => t.FornecedorMp)
                .HasColumnName("forn_mp");

            Property(t => t.Rota)
                .HasColumnName("rot");

            Property(t => t.RotaSequencia)
                .HasColumnName("seq_rot");

            Property(t => t.LocalEntrega)
                .HasColumnName("local_entrega");

            Property(t => t.PortadorCobranca)
                .HasColumnName("port_cobranca");

            Property(t => t.Funcionario)
                .HasColumnName("func");

            Property(t => t.AprovacaoEngenharia)
                .HasColumnName("aprov_eng");

            Property(t => t.SimplesNacional)
                .HasColumnName("simpl_nacional");

            Property(t => t.RetemInss)
                .HasColumnName("ret_inss");

            Property(t => t.ContribuiIcms)
                .HasColumnName("contrib_icms");

            Property(t => t.Inativo)
                .HasColumnName("inativado");

            Property(t => t.IdExterno)
                .HasColumnName("external_id");

            Property(t => t.RetemIrrf)
                .HasColumnName("ret_irrf");

            Property(t => t.RetemCofins)
                .HasColumnName("ret_cofins");

            Property(t => t.RetemPis)
                .HasColumnName("ret_pis");

            Property(t => t.RetemCsll)
                .HasColumnName("ret_csll");

            Property(t => t.DataAtualizacao)
                .HasColumnName("atualizado_em")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

        }
    }
}

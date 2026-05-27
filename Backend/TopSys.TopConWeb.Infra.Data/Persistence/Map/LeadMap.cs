using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadMap : EntityTypeConfiguration<Lead>
    {
        public LeadMap()
        {
            ToTable("topsys.con_lead");

            Ignore(t => t.ContatoPrincipal);

            Ignore(t => t.ContatoSecundario);

            Ignore(t => t.Logs);

            HasKey(t => new { t.UsinaCodigo, t.Ano, t.Numero });

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasOptional(t => t.Vendedor)
                .WithMany()
                .HasForeignKey(t => t.VendedorCodigo);

            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            HasOptional(t => t.ViaCaptacao)
             .WithMany()
             .HasForeignKey(t => t.ViaCaptacaoCodigo)
             .WillCascadeOnDelete(false);

            HasOptional(t => t.Fase)
             .WithMany()
             .HasForeignKey(t => t.FaseCodigo)
             .WillCascadeOnDelete(false);

            HasOptional(t => t.MotivoPerda)
             .WithMany()
             .HasForeignKey(t => t.MotivoPerdaCodigo)
             .WillCascadeOnDelete(false);

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Ano)
                .HasColumnOrder(1)
                .HasColumnName("ano_lead");

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnType("umediumint")
                .HasColumnName("numero_lead")
                .HasDatabaseGeneratedOption((DatabaseGeneratedOption.None));

            Property(t => t.VisitaAno)
                .HasColumnName("ano_visita");

            Property(t => t.VisitaNumero)
                .HasColumnName("numero_visita");

            Property(t => t.OportunidadeAno)
                .HasColumnName("ano_oportunidade");

            Property(t => t.OportunidadeNumero)
                .HasColumnName("numero_oportunidade");

            Property(t => t.Data)
                .HasColumnName("data");

            Property(t => t.Cliente)
                .HasColumnName("cliente");

            Property(t => t.DddTelefone)
                .HasColumnName("ddd");

            Property(t => t.Telefone)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.VendedorCodigo)
                .HasColumnName("vendedor");

            Property(t => t.ViaCaptacaoCodigo)
                .HasColumnName("via_captacao");

            Property(t => t.FaseCodigo)
                .HasColumnName("fase");

            Property(t => t.Classificacao)
                .HasColumnName("classificacao");

            Property(t => t.ProximaEtapa)
                .HasColumnName("proxima_etapa");

            Property(t => t.ObraNome)
                .HasColumnName("obra_nome");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("endereco");

            Property(t => t.EnderecoNumero)
                .HasColumnName("numero");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("complemento");

            Property(t => t.EnderecoBairro)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("municipio");

            Property(t => t.MotivoPerdaCodigo)
                .HasColumnName("motivo_perda");

            Property(t => t.ObservacaoInterna)
                .HasColumnName("observacao_interna");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

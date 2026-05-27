using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class VendedorMap : EntityTypeConfiguration<Vendedor>
    {
        public VendedorMap()
        {
            ToTable("topsys.con_vendedor");

            Ignore(t => t.CpfCnpj);

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None); ;

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.RazaoSocial)
                .HasColumnName("razao_social");

            Property(t => t.Ativo)
                .HasColumnName("ativo");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("endereco");

            Property(t => t.Celular)
                .HasColumnName("celular");

            Property(t => t.Complemento)
                .HasColumnName("compl");

            Property(t => t.Usina)
                .HasColumnName("usina_folha");

            Property(t => t.Municipio)
                .HasColumnName("mun");

            Property(t => t.DDDCelular)
                .HasColumnName("ddd");

            Property(t => t.Email)
                .HasColumnName("email");

            Property(t => t.VendedorPadrinho)
                .HasColumnName("vend_padrinho");

            Property(t => t.Interveniente)
                .HasColumnName("interv");

            Property(t => t.EnderecoNumero)
                .HasColumnName("num");

            Property(t => t.CondicaoPagamento)
                .HasColumnName("cond_pag");

            Property(t => t.Re)
                .HasColumnName("re");

            Property(t => t.Funcao)
                .HasColumnName("funcao");

            Property(t => t.Usuario)
                .HasColumnName("usuario_sist");

            Property(t => t.Cep)
                .HasColumnName("cep");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

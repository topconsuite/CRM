using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class VisitaMap : EntityTypeConfiguration<Visita>
    {
        public VisitaMap()
        {
            ToTable("topsys.con_visita_cliente");

            HasKey(t => new { t.UsinaCodigo, t.Ano, t.Numero });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.Ano)
                .HasColumnOrder(1)
                .HasColumnName("ano_visita");

            Property(t => t.Numero)
                .HasColumnOrder(2)
                .HasColumnName("num_visita");

            Property(t => t.Data)
                .HasColumnOrder(3)
                .HasColumnName("data_visita");

            Property(t => t.HoraVisita)
                .HasColumnOrder(4)
                .HasColumnName("hora_visita");

            Property(t => t.Cliente)
                .HasColumnOrder(5)
                .HasColumnName("cliente");

            Property(t => t.ObraNome)
                .HasColumnOrder(6)
                .HasColumnName("obra_nome");

            Property(t => t.DddTelefone)
                .HasColumnOrder(7)
                .HasColumnName("ddd_telefone");

            Property(t => t.Telefone)
                .HasColumnOrder(8)
                .HasColumnName("telefone");

            Property(t => t.DddCelular)
                .HasColumnOrder(9)
                .HasColumnName("ddd_celular");

            Property(t => t.Celular)
                .HasColumnOrder(10)
                .HasColumnName("celular");

            Property(t => t.Email)
                .HasColumnOrder(11)
                .HasColumnName("email");

            Property(t => t.ObservacaoInterna)
               .HasColumnOrder(12)
               .HasColumnName("obs_interna");

            Property(t => t.VendedorCodigo)
                .HasColumnOrder(13)
                .HasColumnName("vendedor");

            Property(t => t.VisitaTipoCodigo)
                .HasColumnOrder(14)
                .HasColumnName("tipo_visita");

            Property(t => t.EnderecoCep)
                .HasColumnOrder(15)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnOrder(16)
                .HasColumnName("endereco");

            Property(t => t.EnderecoNumero)
                .HasColumnOrder(17)
                .HasColumnName("numero");

            Property(t => t.EnderecoComplemento)
                .HasColumnOrder(18)
                .HasColumnName("complemento");

            Property(t => t.EnderecoBairro)
                .HasColumnOrder(19)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnOrder(20)
                .HasColumnName("municipio");

            Property(t => t.LeadAno)
                .HasColumnOrder(21)
                .HasColumnName("ano_lead");

            Property(t => t.LeadNumero)
                .HasColumnOrder(22)
                .HasColumnName("num_lead");

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasRequired(x => x.Vendedor)
                .WithMany()
                .HasForeignKey(x => x.VendedorCodigo);

            HasRequired(t => t.TipoVisita)
                .WithMany()
                .HasForeignKey(t => t.VisitaTipoCodigo);

            HasRequired(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            HasMany(t => t.Logs)
              .WithRequired(t => t.Visita)
              .HasForeignKey(t => new { t.Usina, t.Ano, t.Numero });

            Ignore(x => x.ContatoPrincipal);
            Ignore(x => x.ContatoSecundario);


        }
    }
    
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class EmpresaMap : EntityTypeConfiguration<Empresa>
    {
        public EmpresaMap()
        {
            ToTable("topsys.ger_empresa");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("Emp");

            Property(t => t.CpfCnpj)
                .HasColumnName("CNPJ_CPF");

            Property(t => t.Email)
                .HasColumnName("Email");

            Property(t => t.EnderecoBairro)
                .HasColumnName("Bairro");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("Compl");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("End");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("Mun");

            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo);

            Property(t => t.EnderecoNumero)
                .HasColumnName("Num");

            Property(t => t.Nome)
                .HasColumnName("Nome");

            Property(t => t.Razao)
                .HasColumnName("Razao");

            Property(t => t.TelefoneDdd)
                .HasColumnName("DDD");

            Property(t => t.TelefoneNumero)
                .HasColumnName("Tel");

            Property(t => t.OptanteSimplesNacional)
                .HasColumnName("opt_simpl_nac");

            Property(t => t.AliquotaSimplesNacional)
                .HasColumnName("aliq_simpl_nac");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("ie");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class FilialMap : EntityTypeConfiguration<Filial>
    {
        public FilialMap()
        {
            ToTable("topsys.fis_filial");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("emp_filial");

            Property(t => t.Nome)
                .HasColumnName("nome");

            Property(t => t.RazaoSocial)
                .HasColumnName("razao_social");

            Property(t => t.ValorDanfe)
                .HasColumnName("vlr_danfe");

            Property(t => t.PermiteDocumentoDiferentePadraoRemessa)
                .HasColumnName("permite_doc_diferente_padrao_rem");

            Property(t => t.PermiteDocumentoDiferentePadraoBomba)
                .HasColumnName("permite_doc_diferente_padrao_bomba");

            Property(t => t.EnderecoCep)
                .HasColumnName("cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("endereco");

            Property(t => t.EnderecoNumero)
                .HasColumnName("num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("bairro");

            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("mun");

            Property(t => t.Cnpj)
                .HasColumnName("cnpj");

            Property(t => t.InscricaoEstadual)
                .HasColumnName("ie");

            Property(t => t.InscricaoMunicipal)
                .HasColumnName("ccm");

            Property(t => t.CentroCusto)
                .HasColumnName("c_custo");
        }
    }
}

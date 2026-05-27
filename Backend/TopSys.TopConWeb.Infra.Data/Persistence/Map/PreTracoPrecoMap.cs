using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{

    public class PreTracoPrecoMap : EntityTypeConfiguration<PreTracoPreco>
    {

        public PreTracoPrecoMap()
        {

            ToTable("topsys.con_tab_preco_pre");

            HasKey(t => new { t.Id });

            HasKey(t => new { t.UsinaCodigo, t.UsoCodigo, t.PedraCodigo, t.SlumpCodigo, t.ResistenciaTipoCodigo, t.Mpa, t.Consumo, t.CreatedAt });

            Property(t => t.Id )
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.UsoCodigo)
                .HasColumnOrder(2)
                .HasColumnName("uso");

            Property(t => t.ResistenciaTipoCodigo)
                .HasColumnOrder(3)
                .HasColumnName("tp_resist");

            Property(t => t.Mpa)
                .HasColumnOrder(4)
                .HasColumnName("fck");

            Property(t => t.Consumo)
                .HasColumnOrder(5)
                .HasColumnName("consumo");

            Property(t => t.PedraCodigo)
                .HasColumnOrder(6)
                .HasColumnName("pedra");

            Property(t => t.SlumpCodigo)
                .HasColumnOrder(7)
                .HasColumnName("slump");

            Property(t => t.CustoMaterial)
                .HasColumnOrder(8)
                .HasColumnName("custo_material");

            Property(t => t.ValorServico)
                .HasColumnOrder(9)
                .HasColumnName("valor_servico");

            Property(t => t.Markup)
                .HasColumnOrder(10)
                .HasColumnName("markup");

            Property(t => t.M3Preco)
                .HasColumnOrder(11)
                .HasColumnName("preco_m3");

            Property(t => t.IDCiencia)
                .HasColumnOrder(12)
                .HasColumnName("id_ciencia");

            Property(t => t.DataCiencia)
                .HasColumnOrder(13)
                .HasColumnName("data_ciencia");

            Property(t => t.TracoEspecificacao)
                .HasColumnOrder(14)
                .HasColumnName("espec_familia");

            Property(t => t.ExternalID)
                .HasColumnOrder(15)
                .HasColumnName("external_id");

            Property(t => t.CreatedAt)
                .HasColumnOrder(16)
                .HasColumnName("CREATED_AT");

            Property(t => t.UpdatedAt)
                .HasColumnOrder(17)
                .HasColumnName("UPDATED_AT");

            Property(t => t.NumeracaoProduto)
                .HasColumnOrder(18)
                .HasColumnName("numeracao_produto");

            HasRequired(t => t.Usina)
               .WithMany()
               .HasForeignKey(t => t.UsinaCodigo);

            HasRequired(t => t.ResistenciaTipo)
               .WithMany()
               .HasForeignKey(t => t.ResistenciaTipoCodigo);

            HasRequired(t => t.Uso)
                .WithMany()
                .HasForeignKey(t => t.UsoCodigo);

            HasRequired(t => t.Pedra)
               .WithMany()
               .HasForeignKey(t => t.PedraCodigo);

            HasRequired(t => t.Slump)
               .WithMany()
               .HasForeignKey(t => t.SlumpCodigo);

        }

    }
}

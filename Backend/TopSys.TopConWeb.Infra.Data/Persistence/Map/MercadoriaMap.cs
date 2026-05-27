using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class MercadoriaMap: EntityTypeConfiguration<Mercadoria>
    {
        public MercadoriaMap()
        {
            ToTable("topsys.fis_mercadoria");

            HasKey(t => new { t.Codigo });

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.Descricao)
                .HasColumnName("descr");

            Property(t => t.ProdutoServico)
                .HasColumnName("prod_serv");

            Property(t => t.NumeracaoProduto)
                .HasColumnName("numeracao_produto");
            
            Property(t => t.IdExterno)
                .HasColumnName("external_id");
        }
    }
}

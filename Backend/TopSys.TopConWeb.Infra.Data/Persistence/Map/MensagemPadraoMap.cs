using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class MensagemPadraoMap : EntityTypeConfiguration<MensagemPadrao>
    {
        public MensagemPadraoMap()
        {
            ToTable("topsys.con_mens_padrao");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnOrder(0)
                .HasColumnName("cod");

            Property(t => t.ProdutoCodigo)
                .HasColumnName("produto");

            Property(t => t.Mensagem)
                .HasColumnName("mens");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraFrenteMap : EntityTypeConfiguration<ObraFrente>
    {

        public ObraFrenteMap()
        {

            ToTable("topsys.con_obras_frente");

            HasKey(x => x.ID);

            HasKey(x => new { x.UsinaCodigo, x.ObraCodigo, x.ObraSequencia });

            HasIndex(x => new { x.UsinaCodigo, x.ObraCodigo });            

            Property(x => x.ID)
                .HasColumnName("id");

            /* -------------- Key Obra -------------- */

            Property(x => x.UsinaCodigo)
                .HasColumnName("obra_usina");

            Property(x => x.ObraCodigo)
                .HasColumnName("obra_numero");

            Property(x => x.ObraSequencia)
                .HasColumnName("obra_sequencia");

            /* -------------- Endereço -------------- */

            Property(t => t.EnderecoNome)
                .HasColumnName("obra_nome")
                .HasMaxLength(40);

            Property(t => t.EnderecoCep)
                .HasColumnName("obra_cep");

            Property(t => t.EnderecoLogradouro)
                .HasColumnName("obra_end");

            Property(t => t.EnderecoNumero)
                .HasColumnName("obra_num");

            Property(t => t.EnderecoComplemento)
                .HasColumnName("obra_compl");

            Property(t => t.EnderecoBairro)
                .HasColumnName("obra_bairro");

        }
    }
}

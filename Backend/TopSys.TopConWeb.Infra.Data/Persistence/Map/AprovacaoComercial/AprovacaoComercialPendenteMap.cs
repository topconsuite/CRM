using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial
{
    public class AprovacaoComercialPendenteMap :  EntityTypeConfiguration<AprovacaoComercialPendente>
    {

        public AprovacaoComercialPendenteMap()
        {

            ToTable("con_aprovacao_comercial_pendente");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .HasColumnName("Id");

            Property(x => x.DataCriacao)
                .HasColumnName("data_criacao")
                .IsRequired();

            Property(x => x.ObraVersao)
                .HasColumnName("obra_versao")
                .IsRequired();

            Property(x => x.ObraUsina)
                .HasColumnName("obra_usina")
                .IsRequired();

            Property(x => x.ObraNumero)
                .HasColumnName("obra_numero")
                .IsRequired();

            Property(x => x.NivelHierarquia)
                .HasColumnName("nivel_hierarquia")
                .IsRequired();

            Property(x => x.AprovacaoStatus)
                .HasColumnName("aprovacao_status")
                .IsRequired();

            Property(x => x.AprovacaoData)
                .HasColumnName("aprovacao_data");

            HasMany(x => x.Tracos)
                .WithRequired()
                .HasForeignKey(x => x.IdAprovacao);

            HasMany(x => x.Bombas)
                .WithRequired()
                .HasForeignKey(x => x.IdAprovacao);

            HasMany(x => x.Volumes)
                .WithRequired()
                .HasForeignKey(x => x.IdAprovacao);

            HasMany(x => x.CondicoesPagamento)
                .WithRequired()
                .HasForeignKey(x => x.IdAprovacao);


        }

    }
}

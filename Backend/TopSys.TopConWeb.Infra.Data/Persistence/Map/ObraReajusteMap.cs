using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ObraReajusteMap : EntityTypeConfiguration<ObraReajuste>
    {
        public ObraReajusteMap()
        {
            ToTable("topsys.con_obras_reajuste");

            HasKey(t => new { t.UsinaCodigo, t.ObraCodigo });

            HasRequired(t => t.Obra)
                .WithOptional(t => t.ObraReajuste);

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnType("usmallint")
                .HasColumnName("usina")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ObraCodigo)
                .HasColumnOrder(1)
                .HasColumnName("obra");

            Property(t => t.MensagemReajuste)
                .HasColumnName("men_reajuste");
        }
    }
}
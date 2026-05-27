using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas
{
    public class VisitaHistoricoMap : EntityTypeConfiguration<VisitaHistorico>
    {

        public VisitaHistoricoMap()
        {

            ToTable("topsys.con_visita_cliente_hist");

            HasKey(t => t.Id);
            HasIndex(t => new { t.Usina, t.AnoVisita, t.NumeroVisita });

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(t => t.Usina)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.AnoVisita)
                .HasColumnOrder(2)
                .HasColumnName("ano_visita");

            Property(t => t.NumeroVisita)
                .HasColumnOrder(3)
                .HasColumnName("num_visita");

            Property(t => t.Tipo)
                .HasColumnOrder(4)
                .HasColumnName("tipo");

            Property(t => t.Descricao)
                .HasColumnOrder(5)
                .HasColumnName("descricao");

            Property(t => t.Data)
                .HasColumnOrder(6)
                .HasColumnName("data");

            Property(t => t.Hora)
                .HasColumnOrder(7)
                .HasColumnName("hora");

            Property(t => t.DataRetorno)
                .HasColumnOrder(8)
                .HasColumnName("data_retorno");

            Property(t => t.HoraRetorno)
                .HasColumnOrder(9)
                .HasColumnName("hora_retorno");

            Property(t => t.IdCadastro)
                .HasColumnOrder(10)
                .HasColumnName("id_cadast");

            Ignore(t => t.TipoHistorico);

        }

    }
}

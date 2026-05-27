using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class LeadInteracaoMap : EntityTypeConfiguration<LeadInteracao>
    {
        public LeadInteracaoMap()
        {
            ToTable("topsys.con_lead_hist");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnOrder(0)
                .HasColumnName("id");

            Property(t => t.Usina)
                .HasColumnOrder(1)
                .HasColumnName("usina");

            Property(t => t.AnoLead)
                .HasColumnOrder(2)
                .HasColumnName("ano_lead");

            Property(t => t.NumeroLead)
                .HasColumnOrder(3)
                .HasColumnName("numero_lead");

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
        }
    }
}

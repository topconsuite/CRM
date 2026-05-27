using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class UsinaDistanciaCepMap : EntityTypeConfiguration<UsinaDistanciaCep>
    {
        public UsinaDistanciaCepMap()
        {
            ToTable("topsys.con_dist_usina_cep");

            HasKey(t => new { t.UsinaEntrega, t.Cep });

            Property(t => t.UsinaEntrega)
                .HasColumnName("usina_entrega");

            Property(t => t.Cep)
                .HasColumnName("cep");

            Property(t => t.DistanciaKm)
                .HasColumnName("distancia_km");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtualizacao)
                .HasColumnName("id_atual");

            Property(t => t.IdAprovacao)
                .HasColumnName("id_aprov");
        }
    }
}
